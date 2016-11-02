// FileEvent.cs
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
using GrabCaster.Framework.Contracts.Attributes;
using GrabCaster.Framework.Contracts.Events;
using GrabCaster.Framework.Contracts.Globals;
using GrabCaster.Framework.Contracts.Serialization;

#endregion

namespace GrabCaster.Framework.FileEvent
{
    /// <summary>
    ///     The file event.
    /// </summary>
    [EventContract("{D438C746-5E75-4D59-B595-8300138FB1EA}", "Write File",
         "Write the content in a file in a specific folder.", true)]
    public class FileEvent : IEventType
    {
        /// <summary>
        ///     Gets or sets the output directory.
        /// </summary>
        [EventPropertyContract("MacroFileName", "specify the file name to use")]
        public string MacroFileName { get; set; }

        /// <summary>
        ///     Gets or sets the output directory.
        /// </summary>
        [EventPropertyContract("OutputDirectory", "When the file has to be created")]
        public string OutputDirectory { get; set; }

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
        [EventActionContract("{1FBD0C6E-1A49-4BEF-8876-33A21B23C933}", "Main action", "Main action description")]
        public byte[] Execute(ActionEvent actionEvent, ActionContext context)
        {
            try
            {
                Debug.WriteLine("In FileEvent Event.");
                File.WriteAllBytes(OutputDirectory + GenerateFileName(MacroFileName),
                    DataContext == null ? new byte[0] : DataContext);
                DataContext = Serialization.ObjectToByteArray(true);
                actionEvent(this, context);
                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("FileEvent error > " + ex.Message);
                actionEvent(this, null);
                return null;
            }
        }

        private string GenerateFileName(string fileNameMacro)
        {
            string fileName = "";
            fileName = fileNameMacro.Replace("%DATETIME%", DateTime.Now.ToString("yy-MM-dd-hh-mm-ss"))
            .Replace("%DATE%", DateTime.Now.ToString("yy-MM-dd"))
            .Replace("%GUID%", Guid.NewGuid().ToString());

            return fileName;

        }
    }
}