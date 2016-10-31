// PowershellEvent.cs
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
using System.IO;
using System.Management.Automation;
using GrabCaster.Framework.Base;
using GrabCaster.Framework.Contracts.Attributes;
using GrabCaster.Framework.Contracts.Events;
using GrabCaster.Framework.Contracts.Globals;

#endregion

namespace GrabCaster.Framework.PowershellEvent
{
    /// <summary>
    ///     The PowerShell event.
    /// </summary>
    [EventContract("{F9A0B69C-64D3-4120-A52D-09D2E014EA91}", "Execute a Powershell Event", "Execute a Powershell Event",
         true)]
    public class PowershellEvent : IEventType
    {
        /// <summary>
        ///     Gets or sets the script event.
        /// </summary>
        [EventPropertyContract("ScriptEvent", "Script to execute")]
        public string ScriptEvent { get; set; }

        /// <summary>
        ///     Gets or sets the script file event.
        /// </summary>
        [EventPropertyContract("ScriptFileEvent", "Script from file")]
        public string ScriptFileEvent { get; set; }

        /// <summary>
        ///     Gets or sets the context.
        /// </summary>
        public ActionContext Context { get; set; }

        /// <summary>
        ///     Gets or sets the set event action event.
        /// </summary>
        public ActionEvent ActionEvent { get; set; }

        /// <summary>
        ///     Gets or sets the data context.
        /// </summary>
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
        [EventActionContract("{979A5EE0-C029-4518-98C2-CFB4526F2C86}", "Main action", "Main action description")]
        public byte[] Execute(ActionEvent actionEvent, ActionContext context)
        {
            try
            {
                var script = string.Empty;
                if (!string.IsNullOrEmpty(ScriptFileEvent))
                {
                    script = File.ReadAllText(ScriptFileEvent);
                }
                else
                {
                    script = ScriptEvent;
                }

                var powerShellScript = PowerShell.Create();
                powerShellScript.AddScript(script);

                // TODO 1020
                powerShellScript.AddParameter("DataContext", DataContext);
                powerShellScript.Invoke();
                var outVar = powerShellScript.Runspace.SessionStateProxy.PSVariable.GetValue("DataContext");
                if (outVar != null)
                {
                    DataContext = EncodingDecoding.EncodingString2Bytes(outVar.ToString());
                    actionEvent(this, context);
                }
                return null;
                // TODO 1030
            }
            catch (Exception)
            {
                // ignored
                return null;
            }
        }
    }
}