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
using System.Xml;
using System.Net;
using Microsoft.BizTalk.Message.Interop;
using GrabCaster.Framework.BizTalk.Adapter.Common;

namespace GrabCaster.Framework.BizTalk.Adapter
{
    using System.Data;
    using System.Diagnostics;

    using GrabCaster.Framework.Base;
    using GrabCaster.Framework.Common;

    /// <summary>
    /// This class handles properties for a given Receive Location
    /// </summary>
    internal class GrabCasterReceiveProperties : ConfigProperties
    {
        // Handler properties
        private static int handlerMaximumNumberOfMessages = 0;

        // Endpoint properties
        private string pointName;
        private int maximumBatchSize;
        private int maximumNumberOfMessages;
        private int errorThreshold;
        private string workInProgress;
        private string uri;

        public string PointName { get { return this.pointName; } }
        public int MaximumBatchSize { get { return maximumBatchSize; } }
        public int MaximumNumberOfMessages { get { return this.maximumNumberOfMessages; } }
        public int ErrorThreshold { get { return errorThreshold; } }
        public string Uri { get { return uri; } }

        public GrabCasterReceiveProperties() : base()
        {
            // establish defaults
            this.pointName = String.Empty;
            maximumBatchSize = 0;
            this.maximumNumberOfMessages = handlerMaximumNumberOfMessages; // default to handler value, override if set on the endpoint
            workInProgress = String.Empty;
        }

        /// <summary>
        /// Load the Configuration for the Receive Handler
        /// </summary>
        public static void ReceiveHandlerConfiguration(XmlDocument configDOM)
        {
            // Handler properties
            handlerMaximumNumberOfMessages = IfExistsExtractInt(configDOM, "/Config/maximumNumberOfMessages", handlerMaximumNumberOfMessages);
        }

        /// <summary>
        /// Load the Configuration for a Receive Location
        /// </summary>
        public void ReadLocationConfiguration(XmlDocument configDOM)
        {

            this.pointName = IfExistsExtract(configDOM, "/Config/pointName", string.Empty);
            this.maximumBatchSize = IfExistsExtractInt(configDOM, "/Config/maximumBatchSize", 0);
            this.maximumNumberOfMessages = IfExistsExtractInt(configDOM, "/Config/maximumNumberOfMessages", handlerMaximumNumberOfMessages);
            this.errorThreshold = ExtractInt(configDOM, "/Config/errorThreshold");
            this.workInProgress = IfExistsExtract(configDOM, "/Config/workInProgress", string.Empty);
            this.uri = IfExistsExtract(configDOM, "/Config/uri", string.Empty);
        }

    }

    /// <summary>
	/// This class maintains send port properties associated with a message. These properties
	/// will be extracted from the message context for static send ports.
	/// </summary>
    internal class GrabCasterTransmitProperties : ConfigProperties
    {
        // Handler properties
        private static int handlerSendBatchSize = 20;
        private static int handlerbufferSize = 4096;
        private static int handlerthreadsPerCPU = 1;

        // Endpoint properties
        private string idConfiguration;
        private string idComponent;
        private string uri;

        public string IdConfiguration { get { return idConfiguration; } }
        public string IdComponent { get { return idComponent; } }
        public string Uri { get { return uri; } }
        public static int BatchSize { get { return handlerSendBatchSize; } }

        public GrabCasterTransmitProperties(IBaseMessage message, string propertyNamespace)
        {

            XmlDocument locationConfigDom = null;

            //  get the adapter configuration off the message
            IBaseMessageContext context = message.Context;
            string config = (string)context.Read("AdapterConfig", propertyNamespace);

            //  the config can be null all that means is that we are doing a dynamic send
            if (null != config)
            {
                locationConfigDom = new XmlDocument();
                locationConfigDom.LoadXml(config);

                this.ReadLocationConfiguration(locationConfigDom);
            }
        }

        /// <summary>
        /// Load the Transmit Handler configuration settings
        /// </summary>
        public static void ReadTransmitHandlerConfiguration(XmlDocument configDOM)
        {

            // Handler properties
            handlerSendBatchSize = ExtractInt(configDOM, "/Config/sendBatchSize");
            handlerbufferSize = ExtractInt(configDOM, "/Config/bufferSize");
            handlerthreadsPerCPU = ExtractInt(configDOM, "/Config/threadsPerCPU");
        }

        /// <summary>
        /// Load the configuration for the Message that is being transmitted
        /// </summary>
        /// <param name="configDOM"></param>
        public void ReadLocationConfiguration(XmlDocument configDOM)
        {


            this.idConfiguration = Extract(configDOM, "/Config/idConfiguration", string.Empty);
            this.idComponent = Extract(configDOM, "/Config/idComponent", string.Empty);
            uri = Extract(configDOM, "/Config/uri", string.Empty);

        }
    }
}

