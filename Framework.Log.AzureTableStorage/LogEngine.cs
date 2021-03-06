﻿// LogEngine.cs
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
using GrabCaster.Framework.Base;
using GrabCaster.Framework.Contracts.Attributes;
using GrabCaster.Framework.Contracts.Log;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

#endregion

namespace GrabCaster.Framework.Log.AzureTableStorage
{
    /// <summary>
    ///     The log engine, simple version.
    /// </summary>
    [LogContract("{CE541CB7-94CD-4421-B6C4-26FBC3088FF9}", "LogEngine", "Azure Table Storage Log System")]
    public class LogEngine : ILogEngine
    {
        private TableBatchOperation batchOperation;
        private CloudTable tableGlobal;

        /// <summary>
        ///     Initialize log.
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public bool InitLog()
        {
            var storageAccountName = ConfigurationBag.Configuration.GroupStorageAccountName;
            var storageAccountKey = ConfigurationBag.Configuration.GroupStorageAccountKey;
            var connectionString =
                $"DefaultEndpointsProtocol=https;AccountName={storageAccountName};AccountKey={storageAccountKey}";
            var storageAccount = CloudStorageAccount.Parse(connectionString);

            // Create the table client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            tableGlobal = tableClient.GetTableReference("grabcastergloballog");
            tableGlobal.CreateIfNotExists();
            batchOperation = new TableBatchOperation();
            return true;
        }

        /// <summary>
        ///     The write log.
        /// </summary>
        /// <param name="logMessage">
        ///     The log message.
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public bool WriteLog(LogMessage logMessage)
        {
            //TableOperation insertOperation = TableOperation.Insert(logMessage);
            //tableGlobal.Execute(insertOperation);

            batchOperation.Insert(logMessage);
            return true;
        }

        public void Flush()
        {
            try
            {
                // Execute the insert operation.
                tableGlobal.ExecuteBatch(batchOperation);
                batchOperation.Clear();
            }
            catch (Exception ex)
            {
                Log.LogEngine.DirectEventViewerLog(
                    $"Error in GrabCaster.Framework.Log.AzureTableStorage component - {ex.Message}", 1);
            }
        }
    }
}