// BlobDevicePersistentProvider.cs
// 
// Copyright (c) 2014-2016, Nino Crudele <nino dot crudele at live dot com>
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
// 
//   - Redistributions of source code must retain the above copyright notice,
//     this list of conditions and the following disclaimer.
//   - Redistributions in binary form must reproduce the above copyright
//     notice, this list of conditions and the following disclaimer in the
//     documentation and/or other materials provided with the distribution.
//   
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
// ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE
// LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
// CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
// SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
// INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN
// CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
// ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
// POSSIBILITY OF SUCH DAMAGE.

#region Usings

using System;
using System.Diagnostics;
using System.Reflection;
using GrabCaster.Framework.Base;
using GrabCaster.Framework.Contracts.Attributes;
using GrabCaster.Framework.Contracts.Storage;
using GrabCaster.Framework.Log;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

#endregion

namespace GrabCaster.Framework.Storage
{
    /// <summary>
    ///     Main persistent provider.
    /// </summary>
    [DevicePersistentProviderContract("{53158DA4-EAEA-4D8A-90C8-81A66F7A0F74}", "DevicePersistentProvider",
         "Device Persistent Provider for Azure")]
    public class BlobDevicePersistentProvider : IDevicePersistentProvider
    {
        public void PersistEventToStorage(byte[] messageBody, string messageId)
        {
            try
            {
                var storageAccountName = ConfigurationBag.Configuration.GroupEventHubsStorageAccountName;
                var storageAccountKey = ConfigurationBag.Configuration.GroupEventHubsStorageAccountKey;
                var connectionString =
                    $"DefaultEndpointsProtocol=https;AccountName={storageAccountName};AccountKey={storageAccountKey}";
                var storageAccount = CloudStorageAccount.Parse(connectionString);
                var blobClient = storageAccount.CreateCloudBlobClient();

                // Retrieve a reference to a container. 
                var container = blobClient.GetContainerReference(ConfigurationBag.Configuration.GroupEventHubsName);

                // Create the container if it doesn't already exist.
                container.CreateIfNotExists();
                container.SetPermissions(
                    new BlobContainerPermissions {PublicAccess = BlobContainerPublicAccessType.Blob});

                // Create the messageid reference
                var blockBlob = container.GetBlockBlobReference(messageId);
                blockBlob.UploadFromByteArray(messageBody, 0, messageBody.Length);
                Debug.WriteLine(
                    "Event persisted -  Consistency Transaction Point created.");
            }
            catch (Exception ex)
            {
                LogEngine.WriteLog(
                    ConfigurationBag.EngineName,
                    $"Error in {MethodBase.GetCurrentMethod().Name}",
                    Constant.LogLevelError,
                    Constant.TaskCategoriesError,
                    ex,
                    Constant.LogLevelError);
            }
        }

        public byte[] PersistEventFromStorage(string messageId)
        {
            try
            {
                var storageAccountName = ConfigurationBag.Configuration.GroupEventHubsStorageAccountName;
                var storageAccountKey = ConfigurationBag.Configuration.GroupEventHubsStorageAccountKey;
                var connectionString =
                    $"DefaultEndpointsProtocol=https;AccountName={storageAccountName};AccountKey={storageAccountKey}";
                var storageAccount = CloudStorageAccount.Parse(connectionString);
                var blobClient = storageAccount.CreateCloudBlobClient();

                // Retrieve a reference to a container. 
                var container = blobClient.GetContainerReference(ConfigurationBag.Configuration.GroupEventHubsName);

                // Create the container if it doesn't already exist.
                container.CreateIfNotExists();
                container.SetPermissions(
                    new BlobContainerPermissions {PublicAccess = BlobContainerPublicAccessType.Blob});

                // Create the messageid reference
                var blockBlob = container.GetBlockBlobReference(messageId);

                blockBlob.FetchAttributes();
                var msgByteLength = blockBlob.Properties.Length;
                var msgContent = new byte[msgByteLength];
                for (var i = 0; i < msgByteLength; i++)
                {
                    msgContent[i] = 0x20;
                }

                blockBlob.DownloadToByteArray(msgContent, 0);

                Debug.WriteLine(
                    "Event persisted recovered -  Consistency Transaction Point restored.");

                return msgContent;
            }
            catch (Exception ex)
            {
                LogEngine.WriteLog(
                    ConfigurationBag.EngineName,
                    $"Error in {MethodBase.GetCurrentMethod().Name}",
                    Constant.LogLevelError,
                    Constant.TaskCategoriesError,
                    ex,
                    Constant.LogLevelError);
                return null;
            }
        }
    }
}