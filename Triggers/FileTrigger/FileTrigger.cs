// FileTrigger.cs
// 
// Copyright (c) 2014-2016, Nino Crudle <nino dot crudele at live dot com>
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

namespace GrabCaster.Framework.FileTrigger
{
    using Contracts.Attributes;
    using Contracts.Globals;
    using Contracts.Triggers;
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading;

    /// <summary>
    /// The file trigger.
    /// </summary>
    [TriggerContract("{3C62B951-C353-4899-8670-C6687B6EAEFC}", "FileTrigger",
         "Get the content from file in a specific directory or shared forlder.", false, true, false)]
    public class FileTrigger : ITriggerType
    {
        /// <summary>
        /// Gets or sets the regex file pattern.
        /// </summary>
        [TriggerPropertyContract("RegexFilePattern", "File pattern, could be a reular expression")]
        public string RegexFilePattern { get; set; }

        /// <summary>
        /// Gets or sets the polling time.
        /// </summary>
        [TriggerPropertyContract("BatchFilesSize", "Number of file to receive fro each batch.")]
        public int BatchFilesSize { get; set; }

        /// <summary>
        /// Gets or sets the polling time.
        /// </summary>
        [TriggerPropertyContract("PollingTime", "Polling time.")]
        public int PollingTime { get; set; }

        /// <summary>
        /// Gets or sets the done extension name.
        /// </summary>
        [TriggerPropertyContract("DoneExtensionName", "Rename extension file received.")]
        public string DoneExtensionName { get; set; }

        /// <summary>
        /// Gets or sets the input directory.
        /// </summary>
        [TriggerPropertyContract("InputDirectory", "Input Directory location")]
        public string InputDirectory { get; set; }

        /// <summary>
        /// Gets or sets the context.
        /// </summary>
        public ActionContext Context { get; set; }

        /// <summary>
        /// Gets or sets the set event action trigger.
        /// </summary>
        public ActionTrigger ActionTrigger { get; set; }

        /// <summary>
        /// If must be syncronous
        /// </summary>
        public bool Syncronous { get; set; }

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
        [TriggerActionContract("{58EEAFEF-CF6A-44C3-9BB9-81EFD680CA36}", "Main action", "Main action description")]
        public byte[] Execute(ActionTrigger actionTrigger, ActionContext context)
        {
            try
            {
                //context.EventBehaviour = EventBehaviour.SyncAsync;
                //context.SyncAsyncEventAction = SyncAsyncActionReceived;
                while (true)
                {
                    var files =
                        Directory.GetFiles(InputDirectory, "*.*", SearchOption.AllDirectories)
                            .Where(path => Path.GetExtension(path) == RegexFilePattern)
                            .ToArray();

                    if (files.Length != 0)
                    {
                        int numberOfBatch = 0;
                        if (files.Length >= BatchFilesSize)
                            numberOfBatch = BatchFilesSize;
                        else
                            numberOfBatch = files.Length;
                        string file = string.Empty;

                        for (int i = 0; i < numberOfBatch; i++)
                        {
                            file = files[i];
                            var data = File.ReadAllBytes(file);
                            File.Delete(Path.ChangeExtension(file, DoneExtensionName));
                            File.Move(file, Path.ChangeExtension(file, DoneExtensionName));
                            DataContext = data;
                            actionTrigger(this, context);
                        }
                    }

                    Thread.Sleep(PollingTime);
                }
            }
            catch (Exception)
            {
                actionTrigger(this, null);
                return null;
            }
        }

        //private void SyncAsyncActionReceived(byte[] DataContext)
        //{
        //    string s = "I got it!";
        //}
    }
}