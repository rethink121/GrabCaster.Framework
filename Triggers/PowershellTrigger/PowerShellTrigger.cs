// --------------------------------------------------------------------------------------------------
// <copyright file = "PowerShellTrigger.cs" company="GrabCaster Ltd">
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
using GrabCaster.Framework.Base;

namespace GrabCaster.Framework.PowerShellTrigger
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Management.Automation;
    using System.Text;

    using GrabCaster.Framework.Contracts.Attributes;
    using GrabCaster.Framework.Contracts.Globals;
    using GrabCaster.Framework.Contracts.Triggers;

    /// <summary>
    /// TODO The power shell trigger.
    /// </summary>
    [TriggerContract("{18BB5E65-23A2-4743-8773-32F039AA3D16}", "PowerShell Trigger", 
        "Execute a trigger write in Powerhell script", true, true, false)]
    public class PowerShellTrigger : ITriggerType
    {
        /// <summary>
        /// Gets or sets the script.
        /// </summary>
        [TriggerPropertyContract("Script", "Script to execute")]
        public string Script { get; set; }

        /// <summary>
        /// Gets or sets the script file.
        /// </summary>
        [TriggerPropertyContract("ScriptFile", "Script from file")]
        public string ScriptFile { get; set; }

        /// <summary>
        /// Gets or sets the message properties.
        /// </summary>
        [EventPropertyContract("MessageProperties", "MessageProperties")]
        public string MessageProperties { get; set; }

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
        /// <exception cref="Exception">
        /// </exception>
        [TriggerActionContract("{78B0F3C0-96D6-4DF6-83CD-C282FB6C6D54}", "Main action", "Main action description")]
        public byte[] Execute(ActionTrigger actionTrigger, ActionContext context)
        {
            var script = string.Empty;
            script = this.ScriptFile != string.Empty ? File.ReadAllText(this.ScriptFile) : this.Script;

            var powerShellScript = PowerShell.Create();
            powerShellScript.AddScript(script);

            // foreach (var prop in MessageProperties)
            // {
            // powerShellScript.AddParameter(prop.Key, prop.Value);
            // }
            powerShellScript.AddParameter("DataContext", this.DataContext);
            powerShellScript.Invoke();
            if (powerShellScript.HadErrors)
            {
                var sb = new StringBuilder();
                foreach (var error in powerShellScript.Streams.Error)
                {
                    sb.AppendLine(error.Exception.Message);
                }

                throw new Exception(sb.ToString());
            }

            var outVar = powerShellScript.Runspace.SessionStateProxy.PSVariable.GetValue("DataContext");
            if (outVar != null && outVar.ToString() != string.Empty)
            {
                try
                {
                    var po = (PSObject)outVar;
                    var logEntry = po.BaseObject as EventLogEntry;
                    if (logEntry != null)
                    {
                        var ev = logEntry;
                        this.DataContext = EncodingDecoding.EncodingString2Bytes(ev.Message);
                    }
                    else
                    {
                        this.DataContext = EncodingDecoding.EncodingString2Bytes(outVar.ToString());
                    }

                    if (this.DataContext.Length != 0)
                    {
                        actionTrigger(this, context);
                    }
                    return null;
                }
                catch
                {
                    // if multiple pso
                    var results = (object[])outVar;
                    foreach (var pos in results)
                    {
                        var po = (PSObject)pos;
                        var logEntry = po.BaseObject as EventLogEntry;
                        if (logEntry != null)
                        {
                            var ev = logEntry;
                            this.DataContext = EncodingDecoding.EncodingString2Bytes(ev.Message);
                        }
                        else
                        {
                            this.DataContext = EncodingDecoding.EncodingString2Bytes(outVar.ToString());
                        }

                        if (this.DataContext.Length != 0)
                        {
                            actionTrigger(this, context);
                        }
                    }
                    return null;
                }
            }
            return null;
        }
    }
}