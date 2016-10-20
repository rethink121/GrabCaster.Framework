// --------------------------------------------------------------------------------------------------
// <copyright file = "ChatEvent.cs" company="GrabCaster Ltd">
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

namespace GrabCaster.Framework.ChatEvent
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.InteropServices;
    using System.Text;

    using GrabCaster.Framework.Contracts.Attributes;
    using GrabCaster.Framework.Contracts.Events;
    using GrabCaster.Framework.Contracts.Globals;

    /// <summary>
    /// The chat event.
    /// </summary>
    [EventContract("{90662D0F-9BBD-4E74-A12D-79BCC0B76BAA}", "Chat Event", "Create a P2P chat bridge.", true)]
    public class ChatEvent : IEventType
    {
        /// <summary>
        /// The e m_ replacesel.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1310:FieldNamesMustNotContainUnderscore", Justification = "Reviewed. Suppression is OK here.")]
        // ReSharper disable once InconsistentNaming
        private const int EM_REPLACESEL = 0x00C2;

        /// <summary>
        /// The e m_ setsel.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1310:FieldNamesMustNotContainUnderscore", Justification = "Reviewed. Suppression is OK here.")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1306:FieldNamesMustBeginWithLowerCaseLetter", Justification = "Reviewed. Suppression is OK here.")]
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        private readonly int EM_SETSEL = 0x00B1;

        /// <summary>
        /// The w m_ gettextlength.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1310:FieldNamesMustNotContainUnderscore", Justification = "Reviewed. Suppression is OK here.")]
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        private readonly int WM_GETTEXTLENGTH = 0x000E;

        /// <summary>
        /// The send message.
        /// </summary>
        /// <param name="hWnd">
        /// The h wnd.
        /// </param>
        /// <param name="uMsg">
        /// The u msg.
        /// </param>
        /// <param name="wParam">
        /// The w param.
        /// </param>
        /// <param name="lParam">
        /// The l param.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        [DllImport("User32.dll")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed. Suppression is OK here.")]
        public static extern int SendMessage(IntPtr hWnd, int uMsg, int wParam, int lParam);

        /// <summary>
        /// The find window ex.
        /// </summary>
        /// <param name="hwndParent">
        /// The hwnd parent.
        /// </param>
        /// <param name="hwndChildAfter">
        /// The hwnd child after.
        /// </param>
        /// <param name="lpszClass">
        /// The lpsz class.
        /// </param>
        /// <param name="lpszWindow">
        /// The lpsz window.
        /// </param>
        /// <returns>
        /// The <see cref="IntPtr"/>.
        /// </returns>
        [DllImport("user32.dll", EntryPoint = "FindWindowEx")]
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1204:StaticElementsMustAppearBeforeInstanceElements", Justification = "Reviewed. Suppression is OK here.")]
        public static extern IntPtr FindWindowEx(
            IntPtr hwndParent,
            IntPtr hwndChildAfter,
            string lpszClass,
            string lpszWindow);

        /// <summary>
        /// The send message.
        /// </summary>
        /// <param name="hWnd">
        /// The h wnd.
        /// </param>
        /// <param name="uMsg">
        /// The u msg.
        /// </param>
        /// <param name="wParam">
        /// The w param.
        /// </param>
        /// <param name="lParam">
        /// The l param.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        [DllImport("User32.dll")]
        [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1204:StaticElementsMustAppearBeforeInstanceElements", Justification = "Reviewed. Suppression is OK here.")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed. Suppression is OK here.")]
        public static extern int SendMessage(IntPtr hWnd, int uMsg, int wParam, string lParam);

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
        [EventActionContract("{{3C670559-B77F-498F-9855-BC5C8E22C758}", "Main action", "Main action description")]
        public byte[] Execute(ActionEvent actionEvent, ActionContext context)
        {
            var content = EncodingDecoding.EncodingBytes2String(this.DataContext);
            var notepads = Process.GetProcessesByName("notepad");

            if (notepads.Length == 0)
            {
                return null;
            }

            if (notepads[0] != null)
            {
                EmptyClipboard();
                var child = FindWindowEx(notepads[0].MainWindowHandle, new IntPtr(0), "Edit", null);
                var length = SendMessageGetTextLength(child, this.WM_GETTEXTLENGTH, IntPtr.Zero, IntPtr.Zero);
                SendMessage(child, this.EM_SETSEL, length, length); // search end of file position
                content += "\r\n";
                SendMessage(child, EM_REPLACESEL, 1, content); // append new line
            }
            return null;
        }

        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed. Suppression is OK here.")]
        private static extern int SendMessageGetTextLength(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern bool EmptyClipboard();
    }
}