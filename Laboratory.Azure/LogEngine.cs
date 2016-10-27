// LogEngine.cs
// 
// Copyright (c) 2014-2016, Nino Crudle <nino dot crudele at live dot com>
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
