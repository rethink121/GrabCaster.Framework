// SqlServerTrigger.cs
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
using System.Data.SqlClient;
using System.Xml;
using GrabCaster.Framework.Base;
using GrabCaster.Framework.Contracts.Attributes;
using GrabCaster.Framework.Contracts.Globals;
using GrabCaster.Framework.Contracts.Triggers;

#endregion

namespace GrabCaster.Framework.SqlServerTrigger
{
    /// <summary>
    ///     The SQL server trigger.
    /// </summary>
    [TriggerContract("{7920EE0F-CAC8-4ABB-82C2-1C69351EDD28}", "Sql Server Trigger",
         "Execute a Sql query or stored procedure.",
         true, true, false)]
    public class SqlServerTrigger : ITriggerType
    {
        /// <summary>
        ///     Gets or sets the SQL query.
        /// </summary>
        [TriggerPropertyContract("SqlQuery", "Select Command [Select * from or EXEC Stored precedure name]")]
        public string SqlQuery { get; set; }

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
        [TriggerActionContract("{7BA7B689-6A1D-4FF6-87B3-720F9A723FB8}", "Main action", "Main action description")]
        public byte[] Execute(ActionTrigger actionTrigger, ActionContext context)
        {
            try
            {
                Context = context;
                ActionTrigger = actionTrigger;

                using (var myConnection = new SqlConnection(ConnectionString))
                {
                    var selectCommand = new SqlCommand(SqlQuery, myConnection);
                    myConnection.Open();
                    XmlReader readerResult = null;
                    try
                    {
                        readerResult = selectCommand.ExecuteXmlReader();
                        readerResult.Read();
                    }
                    catch (Exception)
                    {
                        return null;
                    }

                    if (readerResult.EOF)
                    {
                        return null;
                    }

                    var xdoc = new XmlDocument();
                    xdoc.Load(readerResult);
                    if (xdoc.OuterXml != string.Empty)
                    {
                        DataContext = EncodingDecoding.EncodingString2Bytes(xdoc.OuterXml);
                        myConnection.Close();
                        actionTrigger(this, context);
                    }
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