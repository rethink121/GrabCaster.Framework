using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrabCaster.Framework.Base;
using HM.OMS.PageOneMessageConsole;

namespace HM.OMS.PageOneMessageEvent
{
    using System;
    using System.Diagnostics;
    using System.IO;

    using GrabCaster.Framework.Contracts.Attributes;
    using GrabCaster.Framework.Contracts.Events;
    using GrabCaster.Framework.Contracts.Globals;
    using GrabCaster.Framework.Contracts.Serialization;

    /// <summary>
    /// The file event.
    /// </summary>
    [EventContract("{BC76EEB8-369E-4B0B-BC52-4DFBD4FA33B1}", "PageOneMessageEvent", "PageOneMessageEvent is the OMS service to send mails", true)]
    public class PageOneMessageEvent : IEventType
    {
        /// <summary>
        /// [message] to send an email - [auth] to check the authentication
        /// </summary>
        [EventPropertyContract("input", "[message] to send an email - [auth] to check the authentication")]
        public string input { get; set; }

        /// <summary>
        /// [message] to send an email - [auth] to check the authentication
        /// </summary>
        [EventPropertyContract("From", "The mail from")]
        public string From { get; set; }
        /// <summary>
        /// [message] to send an email - [auth] to check the authentication
        /// </summary>
        [EventPropertyContract("To", "The mail to")]
        public string To { get; set; }
        /// <summary>
        /// [message] to send an email - [auth] to check the authentication
        /// </summary>
        [EventPropertyContract("Message", "Body message")]
        public string Message { get; set; }

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
        [EventActionContract("{E4FD267F-92B2-45F3-B198-0F1581E2EBBA}", "Main action", "Main action description")]
        public byte[] Execute(ActionEvent actionEvent, ActionContext context)
        {
            try
            {
                this.DataContext = EncodingDecoding.EncodingString2Bytes(To);
                //Console.WriteLine("In event before handle");
                //Console.WriteLine($"EVENT {context.BubblingConfiguration.MessageId} - {DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt")}");

                actionEvent(this, context);

                return null;
                string guid = Guid.NewGuid().ToString();

                string result = "Nothing";
                PageOneRestApi pageOne = new PageOneRestApi();
                string[] addressTo = To.Split(';');

                switch (input)
                {
                    case "/message":
                        result = pageOne.SendMessage(new PageOneRestApi.SendMessageRequest
                        {
                            From = this.From,
                            To = new List<string>(addressTo),
                            Message = this.Message
                        });
                        this.DataContext = EncodingDecoding.EncodingString2Bytes(result);
                        actionEvent(this, context);

                        break;
                    case "/auth":
                        //result = pageOne.Authentication();
                        //this.DataContext = EncodingDecoding.EncodingString2Bytes(result);
                        this.DataContext = EncodingDecoding.EncodingString2Bytes("from event....");
                        actionEvent(this, context);

                        break;
                }
                return null;

            }
            catch (Exception ex)
            {
                this.DataContext = EncodingDecoding.EncodingString2Bytes(ex.Message); ;
                actionEvent(this, context);
                return null;
            }
        }

    }
}