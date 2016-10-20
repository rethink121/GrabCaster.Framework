// --------------------------------------------------------------------------------------------------
// <copyright file = "CSharpTrigger.cs" company="GrabCaster Ltd">
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

namespace GrabCaster.Framework.CSharpTrigger
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using GrabCaster.Framework.Contracts.Attributes;
    using GrabCaster.Framework.Contracts.Globals;
    using GrabCaster.Framework.Contracts.Triggers;

    using Roslyn.Compilers;
    using Roslyn.Scripting.CSharp;

    /// <summary>
    /// The c sharp trigger.
    /// </summary>
    [TriggerContract("{928647A2-9BB3-4D9C-8C4D-C63181AC1686}", "CSharp Trigger",
        "Execute a trigger write in CSharp script", false, true, false)]
    public class CSharpTrigger : ITriggerType
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
        public Dictionary<string, object> MessageProperties { get; set; }

        [TriggerPropertyContract("Syncronous", "Trigger Syncronous")]
        public bool Syncronous { get; set; }
        public AutoResetEvent WaitHandle { get; set; }
        public void SyncAsyncActionReceived(byte[] content)
        {
            throw new NotImplementedException();
        }

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
        [TriggerActionContract("{00437935-DB38-426B-BF4D-A101BD64E96F}", "Main action", "Main action description")]
        public byte[] Execute(ActionTrigger actionTrigger, ActionContext context)
        {
            try
            {
                var script = string.Empty;
                var metaProvider = new MetadataFileProvider();
                metaProvider.GetReference(context.GetType().Assembly.Location);
                var scriptEngine = new ScriptEngine(metaProvider);

                var session = scriptEngine.CreateSession(context);

                session.AddReference(
                    @"C:\Users\ninoc\Documents\Visual Studio 2015\Projects\HybridIntegrationServices\Framework\bin\Debug\Framework.exe");
                session.AddReference(
                    @"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\System.Windows.Forms.dll");

                if (this.ScriptFile != null || this.ScriptFile != string.Empty)
                {
                    // TODO 1020
                    // ReSharper disable once AssignNullToNotNullAttribute
                    script = File.ReadAllText(this.ScriptFile);
                    session.ExecuteFile(script);
                }
                else
                {
                    session.Execute(this.Script);
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