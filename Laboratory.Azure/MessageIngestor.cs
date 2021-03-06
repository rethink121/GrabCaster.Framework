﻿// MessageIngestor.cs
// 
// BSD 3-Clause License
// 
// Copyright (c) 2014-2016, Nino Crudele <nino dot crudele at live dot com>
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
// 
// * Redistributions of source code must retain the above copyright notice, this
//   list of conditions and the following disclaimer.
// 
// * Redistributions in binary form must reproduce the above copyright notice,
//   this list of conditions and the following disclaimer in the documentation
//   and/or other materials provided with the distribution.
// 
// * Neither the name of the copyright holder nor the names of its
//   contributors may be used to endorse or promote products derived from
//   this software without specific prior written permission.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE
// FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
// DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
// SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
// CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
// OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
// OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
// 
#region Usings

using System;
using System.Reflection;
using GrabCaster.Framework.Contracts.Bubbling;

#endregion

namespace GrabCaster.Framework.Library.Azure
{
    /// <summary>
    ///     Engine main message ingestor
    /// </summary>
    public static class MessageIngestor
    {
        public delegate void SetEventActionEventEmbedded(byte[] message);

        public static string GroupEventHubsStorageAccountName { get; set; }

        public static string GroupEventHubsStorageAccountKey { get; set; }
        public static string GroupEventHubsName { get; set; }


        public static string ChannelId { get; set; }
        public static string PointId { get; set; }

        /// <summary>
        ///     Used internally by the embedded
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
            var bubblingObject = (IBubblingObject) message;

            // ****************************CHECK MESSAGE TYPE*************************
            try
            {
                // Who sent the message
                var senderId = bubblingObject.SenderPointId;
                var senderDescription = bubblingObject.SenderDescriprion;

                // Who receive the message
                LogEngine.TraceInformation(
                    $"Event received step1 from Sender {senderId} Sender description {senderDescription}");

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
                LogEngine.TraceError(
                    $"Error in {MethodBase.GetCurrentMethod().Name} - Not GrabCaster message type received (Missing GrabCaster_MessageType_Name properties.) -DISCARED- Error {ex.Message}");
                return;
            }

            // ****************************CHECK MESSAGE TYPE*************************
            // Check if >256, the restore or not
            LogEngine.TraceInformation($"Event received step2 Check if > 256, the restore or not.");
            if (bubblingObject.Persisting)
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