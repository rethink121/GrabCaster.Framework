// --------------------------------------------------------------------------------------------------
// <copyright file = "MessageIngestor.cs" company="GrabCaster Ltd">
//   Copyright (c) 2013 - 2016 GrabCaster Ltd All Rights Reserved.
// </copyright>
// <summary>
//    Author: Nino Crudele
//    Blog:   http://ninocrudele.me
// 
//    Unless explicitly acquired and licensed from Licensor under another
//    license, the contents of this file are subject to the Reciprocal Public
//    License ("RPL") Version 1.5, or subsequent versions as allowed by the RPL,
//    and You may not copy or use this file in either source code or executable
//    form, except in compliance with the terms and conditions of the RPL.
//    
//    All software distributed under the RPL is provided strictly on an "AS
//    IS" basis, WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESS OR IMPLIED, AND
//    LICENSOR HEREBY DISCLAIMS ALL SUCH WARRANTIES, INCLUDING WITHOUT
//    LIMITATION, ANY WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
//    PURPOSE, QUIET ENJOYMENT, OR NON-INFRINGEMENT. See the RPL for specific
//    language governing rights and limitations under the RPL. 
// 
//    Reciprocal Public License 1.5 (RPL1.5) license is described here: 
//    http://www.opensource.org/licenses/rpl1.5.txt
//  </summary>
// --------------------------------------------------------------------------------------------------
namespace GrabCaster.Framework.Library.Azure
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    using GrabCaster.Framework.Base;
    using GrabCaster.Framework.Common;
    using GrabCaster.Framework.Contracts.Attributes;
    using GrabCaster.Framework.Contracts.Bubbling;
    using GrabCaster.Framework.Contracts.Events;
    using GrabCaster.Framework.Contracts.Globals;
    using GrabCaster.Framework.Contracts.Messaging;
    using GrabCaster.Framework.Serialization.Object;
    using GrabCaster.Framework.Storage;

    /// <summary>
    /// Engine main message ingestor
    /// </summary>
    public static class MessageIngestor
    {


        public static string GroupEventHubsStorageAccountName { get; set; }

        public static string GroupEventHubsStorageAccountKey { get; set; }
        public static string GroupEventHubsName { get; set; }


        public static string ChannelId { get; set; }
        public static string PointId { get; set; }

        public delegate void SetEventActionEventEmbedded(byte[] message);
        /// <summary>
        /// Used internally by the embedded
        /// </summary>
        public static SetEventActionEventEmbedded setEventActionEventEmbedded { get; set; }

        public static void Init(SetEventActionEventEmbedded setEventOnRampMessageReceived)
        {
            try
            {

                setEventActionEventEmbedded = setEventOnRampMessageReceived;


            }
            catch (Exception ex)
            {

                LogEngine.TraceError($"Error in {MethodBase.GetCurrentMethod().Name} - Error {ex.Message}");
            }
        }
        public static void IngestMessagge(object message)
        {

            byte[] eventDataByte = null;
            var bubblingObject = (IBubblingObject)message;

            // ****************************CHECK MESSAGE TYPE*************************
            try
            {
                // Who sent the message
                var senderId = bubblingObject.SenderPointId;
                var senderDescription = bubblingObject.SenderDescriprion;

                // Who receive the message
                LogEngine.TraceInformation($"Event received step1 from Sender {senderId} Sender description {senderDescription}");

                var receiverChannelId =
                    bubblingObject.DestinationChannelId;
                var receiverPointId =
                    bubblingObject.DestinationPointId;

                ChannelId = "asd";
                PointId = "asd";
                var requestAvailable = (receiverChannelId.Contains(ChannelId)
                                        && receiverPointId.Contains(PointId))
                                        || (receiverChannelId.Contains("*")
                                            && receiverPointId.Contains(PointId))
                                        || (receiverChannelId.Contains(ChannelId)
                                            && receiverPointId.Contains("*"))
                                        || (receiverChannelId.Contains("*")
                                            && receiverPointId.Contains("*"));
                if (!requestAvailable)
                {
                    return;
                }
                
            }
            catch (Exception ex)
            {
                // If error then not message typeof (no property present.)
                LogEngine.TraceError($"Error in {MethodBase.GetCurrentMethod().Name} - Not GrabCaster message type received (Missing GrabCaster_MessageType_Name properties.) -DISCARED- Error {ex.Message}");
                return;
            }

            // ****************************CHECK MESSAGE TYPE*************************
            // Check if >256, the restore or not
            LogEngine.TraceInformation($"Event received step2 Check if > 256, the restore or not.");
            if (bubblingObject.Persisting ==true)
            {
                LogEngine.TraceInformation($"Event received step3 it is > 256");
                BlobDevicePersistentProvider storagePersistent = new BlobDevicePersistentProvider();
            }
            else
            {
                LogEngine.TraceInformation($"Event received step4 it is < 256");
                eventDataByte = bubblingObject.Data;
            }

            LogEngine.TraceInformation($"Event received step5 before serialization.");
            setEventActionEventEmbedded(eventDataByte);

        }
    }
}