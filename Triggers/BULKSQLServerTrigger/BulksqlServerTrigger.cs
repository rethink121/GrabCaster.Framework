// BulksqlServerTrigger.cs
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
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using GrabCaster.Framework.Contracts.Attributes;
using GrabCaster.Framework.Contracts.Globals;
using GrabCaster.Framework.Contracts.Serialization;
using GrabCaster.Framework.Contracts.Triggers;

#endregion

namespace GrabCaster.Framework.BulksqlServerTrigger
{
    /// <summary>
    ///     The bulksql server trigger.
    /// </summary>
    [TriggerContract("{9A989BD1-C8DE-4FC1-B4BA-02E7D8A4AD76}", "SQL Server Bulk Trigger",
         "Execute a bulk insert between databases.", false,
         true, true)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly",
         Justification = "Reviewed. Suppression is OK here.")]
    public class BulksqlServerTrigger : ITriggerType
    {
        /// <summary>
        ///     Gets or sets the table name.
        /// </summary>
        [TriggerPropertyContract("TableName", "TableName")]
        public string TableName { get; set; }

        /// <summary>
        ///     Gets or sets the bulk select query.
        /// </summary>
        [TriggerPropertyContract("BulkSelectQuery", "BulkSelectQuery")]
        public string BulkSelectQuery { get; set; }

        /// <summary>
        ///     Gets or sets the connection string.
        /// </summary>
        [TriggerPropertyContract("ConnectionString", "ConnectionString")]
        public string ConnectionString { get; set; }

        public string SupportBag { get; set; }

        [TriggerPropertyContract("Syncronous", "Trigger Syncronous")]
        public bool Syncronous { get; set; }

        /// <summary>
        ///     Gets or sets the context.
        /// </summary>
        public ActionContext Context { get; set; }

        /// <summary>
        ///     Gets or sets the set event action trigger.
        /// </summary>
        public ActionTrigger ActionTrigger { get; set; }

        /// <summary>
        ///     Gets or sets the data context.
        /// </summary>
        [TriggerPropertyContract("DataContext", "Trigger Default Main Data")]
        public byte[] DataContext { get; set; }

        /// <summary>
        ///     The execute.
        /// </summary>
        /// <param name="actionTrigger">
        ///     The set event action trigger.
        /// </param>
        /// <param name="context">
        ///     The context.
        /// </param>
        [TriggerActionContract("{C55D1D0A-B4B4-4FF0-B41F-38CE0C7A522C}", "Main action", "Main action description")]
        public byte[] Execute(ActionTrigger actionTrigger, ActionContext context)
        {
            try
            {
                Context = context;
                ActionTrigger = actionTrigger;

                using (var sourceConnection = new SqlConnection(ConnectionString))
                {
                    sourceConnection.Open();

                    // Get data from the source table as a SqlDataReader.
                    var commandSourceData = new SqlCommand(BulkSelectQuery, sourceConnection);
                    var dataTable = new DataTable();
                    var dataAdapter = new SqlDataAdapter(commandSourceData);
                    dataAdapter.Fill(dataTable);
                    DataContext = Serialization.DataTableToByteArray(dataTable);
                    actionTrigger(this, context);
                }
                return null;
            }
            catch (Exception)
            {
                // ignored
                return null;
            }
        }
    }
}