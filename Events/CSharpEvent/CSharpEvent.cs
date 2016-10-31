// CSharpEvent.cs
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

using System.Collections.Generic;
using System.IO;
using GrabCaster.Framework.Contracts.Attributes;
using GrabCaster.Framework.Contracts.Events;
using GrabCaster.Framework.Contracts.Globals;
using Roslyn.Compilers;
using Roslyn.Scripting.CSharp;

#endregion

namespace GrabCaster.Framework.CSharpEvent
{
    /// <summary>
    ///     The c sharp event.
    /// </summary>
    [EventContract("{A54EDF55-52E9-4D03-B14B-F5D438AF43F1}", "Execute a CSharp Event", "Execute a CSharp Event", true)]
    public class CSharpEvent : IEventType
    {
        /// <summary>
        ///     Gets or sets the script.
        /// </summary>
        [EventPropertyContract("Script", "Script to execute")]
        public string Script { get; set; }

        /// <summary>
        ///     Gets or sets the script file.
        /// </summary>
        [EventPropertyContract("ScriptFile", "Script from file")]
        public string ScriptFile { get; set; }

        /// <summary>
        ///     Gets or sets the message properties.
        /// </summary>
        [EventPropertyContract("MessageProperties", "MessageProperties")]
        public Dictionary<string, object> MessageProperties { get; set; }

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
        [EventActionContract("{3855F421-9451-45BD-8379-980F93FBB587}", "Main action", "Main action description")]
        public byte[] Execute(ActionEvent actionEvent, ActionContext context)
        {
            try
            {
                Context = context;
                ActionEvent = actionEvent;
                var script = string.Empty;
                var metaProvider = new MetadataFileProvider();
                metaProvider.GetReference(context.GetType().Assembly.Location);
                var scriptEngine = new ScriptEngine(metaProvider);

                var session = scriptEngine.CreateSession(context);

                // TODO 1040
                session.AddReference(
                    @"C:\Users\ninoc\Documents\Visual Studio 2015\Projects\HybridIntegrationServices\Framework\bin\Debug\Framework.exe");
                session.AddReference(
                    @"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\System.Windows.Forms.dll");

                // TODO 1041
                if (ScriptFile != null || ScriptFile != string.Empty)
                {
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
            catch
            {
                // ignored
                return null;
            }
        }
    }
}