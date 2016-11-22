// ChatEvent.cs
// 
// BSD 3-Clause License
// 
// Copyright (c) 2014-2016, Nino Crudele <nino dot crudele at live dot com>
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
// 
// * Redistributions of source code must retain the above copyright notice, this
//   list of conditions and the following disclaimer.
// 
// * Redistributions in binary form must reproduce the above copyright notice,
//   this list of conditions and the following disclaimer in the documentation
//   and/or other materials provided with the distribution.
// 
// * Neither the name of the copyright holder nor the names of its
//   contributors may be used to endorse or promote products derived from
//   this software without specific prior written permission.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE
// FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
// DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
// SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
// CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
// OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
// OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
// 
#region Usings

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using GrabCaster.Framework.Base;
using GrabCaster.Framework.Contracts.Attributes;
using GrabCaster.Framework.Contracts.Events;
using GrabCaster.Framework.Contracts.Globals;

#endregion

namespace GrabCaster.Framework.ChatEvent
{
    /// <summary>
    ///     The chat event.
    /// </summary>
    [EventContract("{90662D0F-9BBD-4E74-A12D-79BCC0B76BAA}", "Chat Event", "Create a P2P chat bridge.", true)]
    public class ChatEvent : IEventType
    {
        /// <summary>
        ///     The e m_ replacesel.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly",
             Justification = "Reviewed. Suppression is OK here.")] [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1310:FieldNamesMustNotContainUnderscore",
                                                                        Justification = "Reviewed. Suppression is OK here.")]
        // ReSharper disable once InconsistentNaming
        private const int EM_REPLACESEL = 0x00C2;

        /// <summary>
        ///     The e m_ setsel.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1310:FieldNamesMustNotContainUnderscore",
             Justification = "Reviewed. Suppression is OK here.")] [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1306:FieldNamesMustBeginWithLowerCaseLetter",
                                                                        Justification = "Reviewed. Suppression is OK here.")] [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly",
                                                                                                                                   Justification = "Reviewed. Suppression is OK here.")] private readonly int EM_SETSEL = 0x00B1;

        /// <summary>
        ///     The w m_ gettextlength.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1310:FieldNamesMustNotContainUnderscore",
             Justification = "Reviewed. Suppression is OK here.")] [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly",
                                                                        Justification = "Reviewed. Suppression is OK here.")] private readonly int WM_GETTEXTLENGTH = 0x000E;

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
        [EventActionContract("{{3C670559-B77F-498F-9855-BC5C8E22C758}", "Main action", "Main action description")]
        public byte[] Execute(ActionEvent actionEvent, ActionContext context)
        {
            var content = EncodingDecoding.EncodingBytes2String(DataContext);
            var notepads = Process.GetProcessesByName("notepad");

            if (notepads.Length == 0)
            {
                return null;
            }

            if (notepads[0] != null && notepads[0].MainWindowTitle.ToUpper() == "CHAT.TXT - NOTEPAD")
            {
                EmptyClipboard();
                var child = FindWindowEx(notepads[0].MainWindowHandle, new IntPtr(0), "Edit", null);
                var length = SendMessageGetTextLength(child, WM_GETTEXTLENGTH, IntPtr.Zero, IntPtr.Zero);
                SendMessage(child, EM_SETSEL, length, length); // search end of file position
                content += "\r\n";
                SendMessage(child, EM_REPLACESEL, 1, content); // append new line
            }
            return null;
        }

        /// <summary>
        ///     The send message.
        /// </summary>
        /// <param name="hWnd">
        ///     The h wnd.
        /// </param>
        /// <param name="uMsg">
        ///     The u msg.
        /// </param>
        /// <param name="wParam">
        ///     The w param.
        /// </param>
        /// <param name="lParam">
        ///     The l param.
        /// </param>
        /// <returns>
        ///     The <see cref="int" />.
        /// </returns>
        [DllImport("User32.dll")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation",
             Justification = "Reviewed. Suppression is OK here.")]
        public static extern int SendMessage(IntPtr hWnd, int uMsg, int wParam, int lParam);

        /// <summary>
        ///     The find window ex.
        /// </summary>
        /// <param name="hwndParent">
        ///     The hwnd parent.
        /// </param>
        /// <param name="hwndChildAfter">
        ///     The hwnd child after.
        /// </param>
        /// <param name="lpszClass">
        ///     The lpsz class.
        /// </param>
        /// <param name="lpszWindow">
        ///     The lpsz window.
        /// </param>
        /// <returns>
        ///     The <see cref="IntPtr" />.
        /// </returns>
        [DllImport("user32.dll", EntryPoint = "FindWindowEx")]
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly",
             Justification = "Reviewed. Suppression is OK here.")]
        [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1204:StaticElementsMustAppearBeforeInstanceElements",
             Justification = "Reviewed. Suppression is OK here.")]
        public static extern IntPtr FindWindowEx(
            IntPtr hwndParent,
            IntPtr hwndChildAfter,
            string lpszClass,
            string lpszWindow);

        /// <summary>
        ///     The send message.
        /// </summary>
        /// <param name="hWnd">
        ///     The h wnd.
        /// </param>
        /// <param name="uMsg">
        ///     The u msg.
        /// </param>
        /// <param name="wParam">
        ///     The w param.
        /// </param>
        /// <param name="lParam">
        ///     The l param.
        /// </param>
        /// <returns>
        ///     The <see cref="int" />.
        /// </returns>
        [DllImport("User32.dll")]
        [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1204:StaticElementsMustAppearBeforeInstanceElements",
             Justification = "Reviewed. Suppression is OK here.")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation",
             Justification = "Reviewed. Suppression is OK here.")]
        public static extern int SendMessage(IntPtr hWnd, int uMsg, int wParam, string lParam);

        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation",
             Justification = "Reviewed. Suppression is OK here.")]
        private static extern int SendMessageGetTextLength(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern bool EmptyClipboard();
    }
}