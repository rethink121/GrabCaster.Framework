// --------------------------------------------------------------------------------------------------
// <copyright file = "PowershellEvent.cs" company="GrabCaster Ltd">
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

using GrabCaster.Framework.Base;

namespace GrabCaster.Framework.PowershellEvent
{
    using System;
    using System.IO;
    using System.Management.Automation;
    using System.Text;

    using GrabCaster.Framework.Contracts.Attributes;
    using GrabCaster.Framework.Contracts.Events;
    using GrabCaster.Framework.Contracts.Globals;

    /// <summary>
    /// The PowerShell event.
    /// </summary>
    [EventContract("{F9A0B69C-64D3-4120-A52D-09D2E014EA91}", "Execute a Powershell Event", "Execute a Powershell Event",
        true)]
    public class PowershellEvent : IEventType
    {
        /// <summary>
        /// Gets or sets the script event.
        /// </summary>
        [EventPropertyContract("ScriptEvent", "Script to execute")]
        public string ScriptEvent { get; set; }

        /// <summary>
        /// Gets or sets the script file event.
        /// </summary>
        [EventPropertyContract("ScriptFileEvent", "Script from file")]
        public string ScriptFileEvent { get; set; }

        /// <summary>
        /// Gets or sets the context.
        /// </summary>
        public ActionContext Context { get; set; }

        /// <summary>
        /// Gets or sets the set event action event.
        /// </summary>
        public ActionEvent ActionEvent { get; set; }

        /// <summary>
        /// Gets or sets the data context.
        /// </summary>
        [EventPropertyContract("DataContext", "Event Default Main Data")]
        public byte[] DataContext { get; set; }

        /// <summary>
        /// The execute.
        /// </summary>
        /// <param name="actionEvent">
        /// The set event action event.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        [EventActionContract("{979A5EE0-C029-4518-98C2-CFB4526F2C86}", "Main action", "Main action description")]
        public byte[] Execute(ActionEvent actionEvent, ActionContext context)
        {
            try
            {
                var script = string.Empty;
                if (!string.IsNullOrEmpty(this.ScriptFileEvent))
                {
                    script = File.ReadAllText(this.ScriptFileEvent);
                }
                else
                {
                    script = this.ScriptEvent;
                }

                var powerShellScript = PowerShell.Create();
                powerShellScript.AddScript(script);

                // TODO 1020
                powerShellScript.AddParameter("DataContext", this.DataContext);
                powerShellScript.Invoke();
                var outVar = powerShellScript.Runspace.SessionStateProxy.PSVariable.GetValue("DataContext");
                if (outVar != null)
                {
                    this.DataContext = EncodingDecoding.EncodingString2Bytes(outVar.ToString());
                    actionEvent(this, context);
                }
                return  null;
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