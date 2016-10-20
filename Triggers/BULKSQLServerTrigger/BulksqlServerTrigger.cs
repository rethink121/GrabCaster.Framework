// --------------------------------------------------------------------------------------------------
// <copyright file = "BulksqlServerTrigger.cs" company="GrabCaster Ltd">
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

using System.Threading;

namespace GrabCaster.Framework.BulksqlServerTrigger
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Diagnostics.CodeAnalysis;

    using GrabCaster.Framework.Contracts.Attributes;
    using GrabCaster.Framework.Contracts.Globals;
    using GrabCaster.Framework.Contracts.Serialization;
    using GrabCaster.Framework.Contracts.Triggers;

    /// <summary>
    /// The bulksql server trigger.
    /// </summary>
    [TriggerContract("{9A989BD1-C8DE-4FC1-B4BA-02E7D8A4AD76}", "SQL Server Bulk Trigger", "Execute a bulk insert between databases.", false,
        true, true)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
    public class BulksqlServerTrigger : ITriggerType
    {
        /// <summary>
        /// Gets or sets the table name.
        /// </summary>
        [TriggerPropertyContract("TableName", "TableName")]
        public string TableName { get; set; }

        /// <summary>
        /// Gets or sets the bulk select query.
        /// </summary>
        [TriggerPropertyContract("BulkSelectQuery", "BulkSelectQuery")]
        public string BulkSelectQuery { get; set; }

        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        [TriggerPropertyContract("ConnectionString", "ConnectionString")]
        public string ConnectionString { get; set; }

        [TriggerPropertyContract("Syncronous", "Trigger Syncronous")]
        public bool Syncronous { get; set; }
        public string SupportBag { get; set; }

        /// <summary>
        /// Gets or sets the context.
        /// </summary>
        public ActionContext Context { get; set; }

        /// <summary>
        /// Gets or sets the set event action trigger.
        /// </summary>
        public ActionTrigger ActionTrigger { get; set; }

        /// <summary>
        /// Gets or sets the data context.
        /// </summary>
        [TriggerPropertyContract("DataContext", "Trigger Default Main Data")]
        public byte[] DataContext { get; set; }

        /// <summary>
        /// The execute.
        /// </summary>
        /// <param name="actionTrigger">
        /// The set event action trigger.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        [TriggerActionContract("{C55D1D0A-B4B4-4FF0-B41F-38CE0C7A522C}", "Main action", "Main action description")]
        public byte[] Execute(ActionTrigger actionTrigger, ActionContext context)
        {
            try
            {
                this.Context = context;
                this.ActionTrigger = actionTrigger;

                using (var sourceConnection = new SqlConnection(this.ConnectionString))
                {
                    sourceConnection.Open();

                    // Get data from the source table as a SqlDataReader.
                    var commandSourceData = new SqlCommand(this.BulkSelectQuery, sourceConnection);
                    var dataTable = new DataTable();
                    var dataAdapter = new SqlDataAdapter(commandSourceData);
                    dataAdapter.Fill(dataTable);
                    this.DataContext = Serialization.DataTableToByteArray(dataTable);
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