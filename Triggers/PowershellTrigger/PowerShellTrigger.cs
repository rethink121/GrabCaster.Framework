// PowerShellTrigger.cs
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
using System.IO;
using System.Management.Automation;
using System.Text;
using GrabCaster.Framework.Base;
using GrabCaster.Framework.Contracts.Attributes;
using GrabCaster.Framework.Contracts.Globals;
using GrabCaster.Framework.Contracts.Triggers;

#endregion

namespace GrabCaster.Framework.PowerShellTrigger
{
    /// <summary>
    ///     TODO The power shell trigger.
    /// </summary>
    [TriggerContract("{18BB5E65-23A2-4743-8773-32F039AA3D16}", "PowerShell Trigger",
         "Execute a trigger write in Powerhell script", true, true, false)]
    public class PowerShellTrigger : ITriggerType
    {
        /// <summary>
        ///     Gets or sets the script.
        /// </summary>
        [TriggerPropertyContract("Script", "Script to execute")]
        public string Script { get; set; }

        /// <summary>
        ///     Gets or sets the script file.
        /// </summary>
        [TriggerPropertyContract("ScriptFile", "Script from file")]
        public string ScriptFile { get; set; }

        /// <summary>
        ///     Gets or sets the message properties.
        /// </summary>
        [EventPropertyContract("MessageProperties", "MessageProperties")]
        public string MessageProperties { get; set; }

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
        /// <exception cref="Exception">
        /// </exception>
        [TriggerActionContract("{78B0F3C0-96D6-4DF6-83CD-C282FB6C6D54}", "Main action", "Main action description")]
        public byte[] Execute(ActionTrigger actionTrigger, ActionContext context)
        {
            var script = string.Empty;
            script = ScriptFile != string.Empty ? File.ReadAllText(ScriptFile) : Script;

            var powerShellScript = PowerShell.Create();
            powerShellScript.AddScript(script);

            // foreach (var prop in MessageProperties)
            // {
            // powerShellScript.AddParameter(prop.Key, prop.Value);
            // }
            powerShellScript.AddParameter("DataContext", DataContext);
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
                    var po = (PSObject) outVar;
                    var logEntry = po.BaseObject as EventLogEntry;
                    if (logEntry != null)
                    {
                        var ev = logEntry;
                        DataContext = EncodingDecoding.EncodingString2Bytes(ev.Message);
                    }
                    else
                    {
                        DataContext = EncodingDecoding.EncodingString2Bytes(outVar.ToString());
                    }

                    if (DataContext.Length != 0)
                    {
                        actionTrigger(this, context);
                    }
                    return null;
                }
                catch
                {
                    // if multiple pso
                    var results = (object[]) outVar;
                    foreach (var pos in results)
                    {
                        var po = (PSObject) pos;
                        var logEntry = po.BaseObject as EventLogEntry;
                        if (logEntry != null)
                        {
                            var ev = logEntry;
                            DataContext = EncodingDecoding.EncodingString2Bytes(ev.Message);
                        }
                        else
                        {
                            DataContext = EncodingDecoding.EncodingString2Bytes(outVar.ToString());
                        }

                        if (DataContext.Length != 0)
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