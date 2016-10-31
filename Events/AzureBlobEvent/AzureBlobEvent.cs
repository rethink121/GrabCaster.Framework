// AzureBlobEvent.cs
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

using GrabCaster.Framework.Contracts.Attributes;
using GrabCaster.Framework.Contracts.Events;
using GrabCaster.Framework.Contracts.Globals;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

#endregion

namespace GrabCaster.Framework.AzureBlobEvent
{
    /// <summary>
    ///     Handles the Azure Blob event.
    /// </summary>
    [EventContract("{C185F004-62E4-45A4-97B1-BD0D382FFE33}", "Azure Blob Event", "Show a messagebox", true)]
    public class AzureBlobEvent : IEventType
    {
        /// <summary>
        ///     Gets or sets the Azure storage account.
        /// </summary>
        /// <value>
        ///     The Azure storage account.
        /// </value>
        [EventPropertyContract("StorageAccount", "Azure StorageAccount")]
        public string StorageAccount { get; set; }

        /// <summary>
        ///     Gets or sets the Azure BLOB container.
        /// </summary>
        /// <value>
        ///     The Azure BLOB container.
        /// </value>
        [EventPropertyContract("BlobContainer", "Azure Blob Container")]
        public string BlobContainer { get; set; }

        /// <summary>
        ///     Gets or sets the Azure BLOB block reference.
        /// </summary>
        /// <value>
        ///     The Azure BLOB block reference.
        /// </value>
        [EventPropertyContract("BlobBlockReference", "Azure Blob BlockReference")]
        public string BlobBlockReference { get; set; }

        /// <summary>
        ///     Gets or sets the event context.
        /// </summary>
        /// <value>
        ///     The context.
        /// </value>
        public ActionContext Context { get; set; }

        /// <summary>
        ///     Gets or sets the set event action event.
        /// </summary>
        /// <value>
        ///     The set event action event.
        /// </value>
        public ActionEvent ActionEvent { get; set; }

        /// <summary>
        ///     Gets or sets the data context.
        /// </summary>
        /// <value>
        ///     The data context.
        /// </value>
        [EventPropertyContract("DataContext", "Event Default Main Data")]
        public byte[] DataContext { get; set; }

        /// <summary>
        ///     The execute.
        /// </summary>
        /// <param name="actionEvent">
        ///     The set event action event.
        /// </param>
        /// <param name="context">
        ///     The context.
        /// </param>
        [EventActionContract("{346FC437-8464-4566-8AD6-A7E4B29A7EBC}", "Main action", "Main action description")]
        public byte[] Execute(ActionEvent actionEvent, ActionContext context)
        {
            try
            {
                Context = context;
                ActionEvent = actionEvent;

                var storageAccount = CloudStorageAccount.Parse(StorageAccount);
                var blobClient = storageAccount.CreateCloudBlobClient();

                // Retrieve a reference to a container. 
                var container = blobClient.GetContainerReference(BlobContainer);

                // Create the container if it doesn't already exist.
                container.CreateIfNotExists();
                container.SetPermissions(
                    new BlobContainerPermissions {PublicAccess = BlobContainerPublicAccessType.Blob});

                var blockBlob = container.GetBlockBlobReference(BlobBlockReference);
                blockBlob.UploadFromByteArray(DataContext, 0, DataContext.Length);

                actionEvent(this, context);
                return null;
            }
            catch
            {
                return null;
            }
        } // Execute
    } // AzureBlobEvent
} // namespace