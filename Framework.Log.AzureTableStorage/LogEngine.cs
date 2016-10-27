
//https://azure.microsoft.com/en-gb/documentation/articles/storage-dotnet-how-to-use-tables/
// --------------------------------------------------------------------------------------------------
// <copyright file = "LogEngine.cs" company="GrabCaster Ltd">
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

using System.Diagnostics;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace GrabCaster.Framework.Log.AzureTableStorage
{
    using Base;
    using GrabCaster.Framework.Contracts.Attributes;
    using GrabCaster.Framework.Contracts.Log;
    using System;
    using System.IO;

    /// <summary>
    /// The log engine, simple version.
    /// </summary>
    [LogContract("{CE541CB7-94CD-4421-B6C4-26FBC3088FF9}", "LogEngine", "Azure Table Storage Log System")]
    public class LogEngine : ILogEngine
    {
        private TableBatchOperation batchOperation = null;
        private CloudTable tableGlobal = null;

        /// <summary>
        /// Initialize log.
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool InitLog()
        {
            var storageAccountName = ConfigurationBag.Configuration.GroupEventHubsStorageAccountName;
            var storageAccountKey = ConfigurationBag.Configuration.GroupEventHubsStorageAccountKey;
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
        /// The write log.
        /// </summary>
        /// <param name="logMessage">
        /// The log message.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
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
                GrabCaster.Framework.Log.LogEngine.DirectEventViewerLog($"Error in GrabCaster.Framework.Log.AzureTableStorage component - {ex.Message}",1);
            }
        }
    }
}