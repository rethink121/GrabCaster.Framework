using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using GrabCaster.Framework.Base;
using GrabCaster.Framework.Contracts.Attributes;
using GrabCaster.Framework.Contracts.Globals;
using GrabCaster.Framework.Contracts.Triggers;

namespace HM.OMS.PageOneMessageTrigger
{
    /// <summary>
    /// The file trigger.
    /// </summary>
    [TriggerContract("{EFE0DDCC-4470-4D0C-AB00-BEB6549D8591}", "PageOneMessageTrigger", "PageOneMessageTrigger is the OMS service to send mails", false, true, false)]
    public class PageOneMessageTrigger : ITriggerType
    {
        /// <summary>
        /// [message] to send an email - [auth] to check the authentication
        /// </summary>
        [TriggerPropertyContract("input", "[message] to send an email - [auth] to check the authentication")]
        public string input { get; set; }

        /// <summary>
        /// [message] to send an email - [auth] to check the authentication
        /// </summary>
        [TriggerPropertyContract("From", "The mail from")]
        public string From { get; set; }
        /// <summary>
        /// [message] to send an email - [auth] to check the authentication
        /// </summary>
        [TriggerPropertyContract("To", "The mail to")]
        public string To { get; set; }
        /// <summary>
        /// [message] to send an email - [auth] to check the authentication
        /// </summary>
        [TriggerPropertyContract("Message", "Body message")]
        public string Message { get; set; }


        public string SupportBag { get; set; }

        [TriggerPropertyContract("Syncronous", "Trigger Syncronous")]
        public bool Syncronous { get; set; }

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
        [TriggerActionContract("{D60E0168-8CDF-4443-B9AB-704E943012E3}", "Main action", "Main action description")]
        public byte[] Execute(ActionTrigger actionTrigger, ActionContext context)
        {
            try
            {
                if (input.Length != 0)
                {
                    

                    actionTrigger(this, context);
                }
                return this.DataContext;

            }
            catch (Exception ex)
            {
                this.DataContext = EncodingDecoding.EncodingString2Bytes(ex.Message); 
                actionTrigger(this, context);
                this.ActionTrigger = actionTrigger;
                this.Context = context;
                return EncodingDecoding.EncodingString2Bytes(ex.Message);
            }
        }

    }
}
