// CSharpTrigger.cs
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
using System.Collections.Generic;
using System.IO;
using System.Threading;
using GrabCaster.Framework.Contracts.Attributes;
using GrabCaster.Framework.Contracts.Globals;
using GrabCaster.Framework.Contracts.Triggers;
using Roslyn.Compilers;
using Roslyn.Scripting.CSharp;

#endregion

namespace GrabCaster.Framework.CSharpTrigger
{
    /// <summary>
    ///     The c sharp trigger.
    /// </summary>
    [TriggerContract("{928647A2-9BB3-4D9C-8C4D-C63181AC1686}", "CSharp Trigger",
         "Execute a trigger write in CSharp script", false, true, false)]
    public class CSharpTrigger : ITriggerType
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
        public Dictionary<string, object> MessageProperties { get; set; }

        public AutoResetEvent WaitHandle { get; set; }

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

                if (ScriptFile != null || ScriptFile != string.Empty)
                {
                    // TODO 1020
                    // ReSharper disable once AssignNullToNotNullAttribute
                    script = File.ReadAllText(ScriptFile);
                    session.ExecuteFile(script);
                }
                else
                {
                    session.Execute(Script);
                }
                return null;
            }
            catch (Exception)
            {
                // ignored
                return null;
            }
        }

        public void SyncAsyncActionReceived(byte[] content)
        {
            throw new NotImplementedException();
        }
    }
}