// -----------------------------------------------------------------------------------
// 
// GRABCASTER LTD CONFIDENTIAL
// ___________________________
// 
// Copyright © 2013 - 2016 GrabCaster Ltd. All rights reserved.
// This work is registered with the UK Copyright Service: Registration No:284701085
// 
// 
// NOTICE:  All information contained herein is, and remains
// the property of GrabCaster Ltd and its suppliers,
// if any.  The intellectual and technical concepts contained
// herein are proprietary to GrabCaster Ltd
// and its suppliers and may be covered by UK and Foreign Patents,
// patents in process, and are protected by trade secret or copyright law.
// Dissemination of this information or reproduction of this material
// is strictly forbidden unless prior written permission is obtained
// from GrabCaster Ltd.
// 
// -----------------------------------------------------------------------------------
using System;
using System.IO;
using System.Net;
using System.Xml;
using System.Text;
using System.Security;
using System.Threading;
using System.Collections;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.BizTalk.Component.Interop;
using Microsoft.BizTalk.Message.Interop;
using Microsoft.BizTalk.TransportProxy.Interop;
using GrabCaster.Framework.BizTalk.Adapter.Common;


namespace GrabCaster.Framework.BizTalk.Adapter
{
    using GrabCaster.Framework.Base;
    using GrabCaster.Framework.Common;
    using GrabCaster.Framework.Contracts.Events;
    using GrabCaster.Framework.Contracts.Globals;
    using GrabCaster.Framework.Library;

    /// <summary>
    /// This class corresponds to a Receive Location/URI.  It handles polling the
    /// given folder for new messages.
    /// </summary>
    internal class GrabCasterReceiverEndpoint : ReceiverEndpoint
    {
        // constants
        private const string MESSAGE_BODY = "body";
        private const string PROP_REMOTEMESSAGEID = "RemoteMessageId";
        private const string PROP_IDCONFIGURATION = "idConfiguration";
        private const string PROP_IDCOMPONENT = "idComponent";
        private const string PROP_POINTNAME = "pointName";
        private const string PROP_NAMESPACE = "https://GrabCaster.BizTalk.Schemas.GrabCasterProperties";
        
        public GrabCasterReceiverEndpoint()
        {
        }

        /// <summary>
        /// This method is called when a Receive Location is enabled.
        /// </summary>
        public override void Open(
            string uri,
            IPropertyBag config,
            IPropertyBag bizTalkConfig,
            IPropertyBag handlerPropertyBag,
            IBTTransportProxy transportProxy,
            string transportType,
            string propertyNamespace,
            ControlledTermination control)
        {
            Trace.WriteLine("[GrabCasterReceiverEndpoint] Open called");
            this.errorCount = 0;

            this.properties = new GrabCasterReceiveProperties();
            //  Location properties - possibly override some Handler properties
            XmlDocument locationConfigDom = ConfigProperties.ExtractConfigDom(config);
            this.properties.ReadLocationConfiguration(locationConfigDom);

            //  this is our handle back to the EPM
            this.transportProxy = transportProxy;

            // used to create new messages / message parts etc.
            this.messageFactory = this.transportProxy.GetMessageFactory();

            //  used in the creation of messages
            this.transportType = transportType;

            //  used in the creation of messages
            this.propertyNamespace = propertyNamespace;

            // used to track inflight work for shutting down properly
            this.controlledTermination = control;

            //start the task
            Start();
        }

        /// <summary>
        /// This method is called when the configuration for this receive location is modified.
        /// </summary>
        public override void Update (IPropertyBag config, IPropertyBag bizTalkConfig, IPropertyBag handlerPropertyBag)
        {
            Trace.WriteLine("[GrabCasterReceiverEndpoint] Updated called");
            lock (this)
            {
                Stop();

                errorCount = 0;

                //  keep handles to these property bags until we are ready
                this.updatedConfig             = config;
                this.updatedBizTalkConfig      = bizTalkConfig;
                this.updatedHandlerPropertyBag = handlerPropertyBag;

                if (updatedConfig != null)
                {
                    XmlDocument locationConfigDom = ConfigProperties.ExtractConfigDom(this.updatedConfig);
                    this.properties.ReadLocationConfiguration(locationConfigDom);
                }

                //Schedule the polling event
                Start();
            }
        }

        public override void Dispose()
        {
            Trace.WriteLine("[GrabCasterReceiverEndpoint] Dispose called");
            //  stop the schedule
            Stop();
        }

        private void Start()
        {
            ControlledEndpointTask(null);
        }

        private void Stop()
        {
            this.timer.Dispose();
        }

        /// <summary>
        /// this method is called from the task scheduler when the polling interval has elapsed.
        /// </summary>
        public void ControlledEndpointTask (object val)
        {
            if (this.controlledTermination.Enter())
            {
                try
                {
                    lock (this)
                    {
                        this.EndpointTask();
                    }
                    GC.Collect();
                }
                finally
                {
                    this.controlledTermination.Leave();
                }
            }
        }

        /// <summary>
        /// The set event action event embedded.
        /// </summary>
        private static Embedded.SetEventActionEventEmbedded setEventActionEventEmbedded;

        /// <summary>
        /// Handle the work to be performed each polling interval
        /// </summary>
        private void EndpointTask () 
        {
            try 
            {
                setEventActionEventEmbedded = EventReceivedFromEmbedded;
                Embedded.setEventActionEventEmbedded = setEventActionEventEmbedded;
                Console.WriteLine("Start GrabCaster Embedded Library");
                GrabCaster.Framework.Library.Embedded.StartEngine();

                //Success, reset the error count
                errorCount = 0; 
            }
            catch (Exception e) 
            {
                transportProxy.SetErrorInfo(e);
                //Track number of failures
                errorCount++;
            }
            
        }

        /// <summary>
        /// The event received from embedded.
        /// </summary>
        /// <param name="eventType">
        /// The event type.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        private void EventReceivedFromEmbedded(IEventType eventType, EventActionContext context)
        {
            string stringValue = Encoding.UTF8.GetString(eventType.DataContext);
            System.Diagnostics.Debug.WriteLine("---------------EVENT RECEIVED FROM EMBEDDED LIBRARY---------------");

            PrepareMessageAndSubmit(eventType, context);
            Trace.WriteLine("---------------EVENT RECEIVED FROM EMBEDDED LIBRARY---------------");
            Trace.WriteLine(stringValue);
            CheckErrorThreshold();

        }
        private bool CheckErrorThreshold ()
        {
            if ((0 != this.properties.ErrorThreshold) && (this.errorCount > this.properties.ErrorThreshold))
            {
              
                //Stop the timer.
                Stop();
                return false;
            }
            return true;
        }
        List<BatchMessage> batchMessages = new List<BatchMessage>();
        //  The algorithm implemented here splits the list of messages according to the
        //  batch tuning parameters (number of bytes and number of messages) because the
        //  list is randomly ordered it is possible to have non-optimal batches. It would
        //  be a slight optimization to order by increasing size and then cut the batches.
        private void PrepareMessageAndSubmit (IEventType eventType, EventActionContext context)
        {
            Trace.WriteLine("[GrabCasterReceiverEndpoint] PrepareMessageAndSubmit called");
            
            int maxBatchSize     = this.properties.MaximumBatchSize;
            int maximumNumberOfMessages = this.properties.MaximumNumberOfMessages;

            
            long bytesInBatch = 0;
            
            IBaseMessage msg = CreateMessage(eventType, context);
            if ( null == msg )
                return;
            else
                batchMessages.Add(new BatchMessage(msg, context.BubblingConfiguration.MessageId, BatchOperationType.Submit));            //  keep a running total for the current batch
            bytesInBatch += eventType.DataContext.Length;

            //  zero for the value means infinite 
            bool messagesCountExceeded = ((0 != maximumNumberOfMessages) && (batchMessages.Count >= maximumNumberOfMessages));
            bool byteCountExceeded = ((0 != maxBatchSize)     && (bytesInBatch    >  maxBatchSize));

            if (messagesCountExceeded || byteCountExceeded)
            {
                //  check if we have been asked to stop - if so don't start another batch
                if (this.controlledTermination.TerminateCalled)
                    return;

                //  execute the batch
                this.SubmitMessages(batchMessages);

                //  reset the running totals
                bytesInBatch = 0;
                batchMessages.Clear();
            }
            

            //  check if we have been asked to stop - if so don't start another batch
            if (this.controlledTermination.TerminateCalled)
                return;

            //  end of message list - one final batch to do
            if (batchMessages.Count > 0)
                this.SubmitMessages(batchMessages);
        }

        /// <summary>
        /// Given a List of Messages submit them to BizTalk for processing
        /// </summary>
        private void SubmitMessages(List<BatchMessage> items)
        {
            if (items == null || items.Count == 0) throw new ArgumentException("SubmitMessages was called with an empty list of messages");

            Trace.WriteLine(string.Format("[GrabCasterReceiverEndpoint] SubmitMessages called. Submitting a batch of {0} messages.", items.Count));

            //This class is used to track the messages associated with this ReceiveBatch
            BatchInfo batchInfo = new BatchInfo(items);
            using (ReceiveBatch batch = new ReceiveBatch(this.transportProxy, this.controlledTermination, batchInfo.OnBatchComplete, this.properties.MaximumNumberOfMessages))
            {
                foreach (BatchMessage item in items)
                {
                    // submit message to batch
                    batch.SubmitMessage(item.Message, item.UserData);
                }

                batch.Done(null);
            }
        }

        /// <summary>
        /// This class tracks a collection of messages associated with a EPM Batch
        /// </summary>
        private class BatchInfo
        {
            BatchMessage[] messages;

            internal BatchInfo(List<BatchMessage> messageList)
            {
                this.messages = new BatchMessage[messageList.Count];
                messageList.CopyTo(messages);
            }

            /// <summary>
            /// Called when the BizTalk Batch has been submitted.  If all the messages were submitted (good or suspended)
            /// we delete the messages from the folder
            /// </summary>
            /// <param name="overallStatus"></param>
            internal void OnBatchComplete(bool overallStatus)
            {
                Trace.WriteLine(string.Format("[GrabCasterReceiverEndpoint] OnBatchComplete called. overallStatus == {0}.", overallStatus));

                if (overallStatus == true) //Batch completed
                {
                    //Delete the messages
                    foreach (BatchMessage batchMessage in messages)
                    {
                        //Close the stream so we can delete this message
                        batchMessage.Message.BodyPart.Data.Close();
                    }
                }
            }
        }

        private IBaseMessage CreateMessage (IEventType eventType, EventActionContext contextItem)
        {
            Stream fs;
            fs = new MemoryStream(eventType.DataContext);
            IBaseMessagePart part = this.messageFactory.CreateMessagePart();
            part.Data = fs;
            IBaseMessage message = this.messageFactory.CreateMessage();
            message.AddPart(MESSAGE_BODY, part, true);

            SystemMessageContext context = new SystemMessageContext(message.Context);
            context.InboundTransportType     = this.transportType;
            context.InboundTransportLocation = this.properties.Uri;
            //Write/Promote any adapter specific properties on the message context
            message.Context.Write(PROP_REMOTEMESSAGEID, PROP_NAMESPACE, contextItem.BubblingConfiguration.MessageId);
            
            return message;
        }

        //  properties
        private GrabCasterReceiveProperties properties;

        //  handle to the EPM
        private IBTTransportProxy transportProxy;

        // used to create new messages / message parts etc.
        private IBaseMessageFactory messageFactory;

        //  used in the creation of messages
        private string transportType;

        //  used in the creation of messages
        private string propertyNamespace;

        // used to track inflight work
        private ControlledTermination controlledTermination;

        //  error count for comparison with the error threshold
        int errorCount;

        private System.Threading.Timer timer = null;

        //  support for Update
        IPropertyBag updatedConfig;
        IPropertyBag updatedBizTalkConfig;
        IPropertyBag updatedHandlerPropertyBag;
    }
}
