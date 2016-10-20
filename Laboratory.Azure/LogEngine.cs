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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrabCaster.Framework.Library.Azure
{
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Table;

    public static class LogEngine
    {
        public static void TraceInformation(string message)
        {
            string logMessage = $"{""} - {message}";

            WriteMessage(logMessage);
        }
        public static void TraceWarning(string message)
        {
            string logMessage = $"{""} - {message}";
            WriteMessage(logMessage);
        }
        public static void TraceError(string message)
        {
            string logMessage = $"{""} - {message}";
            WriteMessage(logMessage);
        }

        private static void WriteMessage(string message)
        {
            try
            {
                Console.WriteLine(message);
                //var storageAccountName = "grabcastersa";
                //var storageAccountKey =
                //    "Xmq9EjpObhOkzETSxFphF/diQFxb2RMGEAUwWvRvEAraCPFzZ+Alr4mHnwGbBAEYAYWQZ91yq5bLREUg9MImAA==";
                //var connectionString =
                //    $"DefaultEndpointsProtocol=https;AccountName={storageAccountName};AccountKey={storageAccountKey}";
                //var storageAccount = CloudStorageAccount.Parse(connectionString);

                //// Create the table client.
                //CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
                //// Create the table if it doesn't exist.
                //CloudTable table = tableClient.GetTableReference("grabcasterpointlog");
                //table.CreateIfNotExists();
                //LogrEntity log = new LogrEntity(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
                //log.Message = message;
                //log.TimeMessage = DateTime.Now;
                //TableOperation insertOperation = TableOperation.Insert(log);
                //// Execute the insert operation.
                //table.Execute(insertOperation);
            }
            catch (Exception)
            {
                
        
            }

        }
    }
    public class LogrEntity : TableEntity
    {
        public LogrEntity(string partitionKey, string rowKey)
        {
            this.PartitionKey = partitionKey;
            this.RowKey = rowKey;
        }

        public LogrEntity() { }

        public string Message { get; set; }

        public DateTime TimeMessage { get; set; }
    }
}
