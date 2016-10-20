// --------------------------------------------------------------------------------------------------
// <copyright file = "FileTrigger.cs" company="GrabCaster Ltd">
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
namespace GrabCaster.Framework.FileTrigger
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading;

    using GrabCaster.Framework.Contracts.Attributes;
    using GrabCaster.Framework.Contracts.Globals;
    using GrabCaster.Framework.Contracts.Triggers;
    using GrabCaster.Framework.Storage;

    /// <summary>
    /// The file trigger.
    /// </summary>
    [TriggerContract("{3C62B951-C353-4899-8670-C6687B6EAEFC}", "FileTrigger", "Get the content from file in a specific directory or shared forlder.", false, true, false)]
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
                    var reg = new Regex(this.RegexFilePattern);
                    if (Directory.GetFiles(this.InputDirectory, "*.txt").Where(path => reg.IsMatch(path)).ToList().Any())
                    {
                        var files =
                            Directory.GetFiles(this.InputDirectory, "*.txt").Where(path => reg.IsMatch(path)).ToList();
                        foreach (var file in files)
                        {
                            var data = File.ReadAllBytes(file);
                            File.Delete(Path.ChangeExtension(file, this.DoneExtensionName));
                            File.Move(file, Path.ChangeExtension(file, this.DoneExtensionName));
                            this.DataContext = data;
                            actionTrigger(this, context);
                            
                        }

                    }

                    Thread.Sleep(this.PollingTime);
                }
                return null;
            }
            catch (Exception ex)
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