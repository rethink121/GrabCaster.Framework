// --------------------------------------------------------------------------------------------------
// <copyright file = "ChatTrigger.cs" company="GrabCaster Ltd">
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
using GrabCaster.Framework.Base;

namespace GrabCaster.Framework.ChatTrigger
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Windows.Forms;

    using GrabCaster.Framework.Contracts.Attributes;
    using GrabCaster.Framework.Contracts.Globals;
    using GrabCaster.Framework.Contracts.Triggers;

    /// <summary>
    /// The chat trigger.
    /// </summary>
    [TriggerContract("{B515A376-B91E-495C-ADEE-2AD3DC54C2B3}", "ChatTrigger", "Create a P2P chat bridge.", false, true, 
        false)]
    public class ChatTrigger : ITriggerType
    {
        /// <summary>
        /// The w h_ keyboar d_ ll.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1310:FieldNamesMustNotContainUnderscore", Justification = "Reviewed. Suppression is OK here.")]
        // ReSharper disable once InconsistentNaming
        private const int WH_KEYBOARD_LL = 13;

        /// <summary>
        /// The w m_ keydown.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1310:FieldNamesMustNotContainUnderscore", Justification = "Reviewed. Suppression is OK here.")]
        // ReSharper disable once InconsistentNaming
        private const int WM_KEYDOWN = 0x0100;
        
        /// <summary>
        /// The line chat.
        /// </summary>
        private static readonly StringBuilder LineChat = new StringBuilder();

        /// <summary>
        /// The _proc.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        private static readonly LowLevelKeyboardProc Proc = HookCallback;

        /// <summary>
        /// The hook id.
        /// </summary>
        private static IntPtr hookId = IntPtr.Zero;

        /// <summary>
        /// The _ trigger.
        /// </summary>
        private static ITriggerType trigger;

        /// <summary>
        /// The low level keyboard proc.
        /// </summary>
        /// <param name="nCode">
        /// The n code.
        /// </param>
        /// <param name="wParam">
        /// The w param.
        /// </param>
        /// <param name="lParam">
        /// The l param.
        /// </param>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1615:ElementReturnValueMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        /// <summary>
        /// The set hook.
        /// </summary>
        /// <param name="proc">
        /// The proc.
        /// </param>
        /// <returns>
        /// The <see cref="IntPtr"/>.
        /// </returns>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (var curProcess = Process.GetCurrentProcess())
            using (var curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        /// <summary>
        /// The hook callback.
        /// </summary>
        /// <param name="nCode">
        /// The n code.
        /// </param>
        /// <param name="wParam">
        /// The w param.
        /// </param>
        /// <param name="lParam">
        /// The l param.
        /// </param>
        /// <returns>
        /// The <see cref="IntPtr"/>.
        /// </returns>
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed. Suppression is OK here.")]
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                var notepads = Process.GetProcessesByName("notepad");
                Process notepadChat = null;
                foreach (Process notepad in notepads)
                {
                    if (notepad.MainWindowTitle.ToUpper() == "CHAT.TXT - NOTEPAD")
                        notepadChat = notepad;
                }
                

                if (notepadChat != null )
                {
                    var virtualkCode = Marshal.ReadInt32(lParam);
                    if (virtualkCode != 13)
                    {
                        LineChat.Append((char)virtualkCode);
                    }

                    if (virtualkCode == 13)
                    {
                        trigger.DataContext =
                            EncodingDecoding.EncodingString2Bytes(
                                string.Concat("[", ConfigurationBag.Configuration.PointName, "]: ", LineChat.ToString()));
                        LineChat.Clear();
                        InternalActionTrigger(trigger, InternalContext);
                    }
                }
                
            }

            return CallNextHookEx(hookId, nCode, wParam, lParam);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed. Suppression is OK here.")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed. Suppression is OK here.")]
        private static extern IntPtr SetWindowsHookEx(
            int idHook,
            LowLevelKeyboardProc lpfn,
            IntPtr hMod,
            uint dwThreadId);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed. Suppression is OK here.")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed. Suppression is OK here.")]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed. Suppression is OK here.")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed. Suppression is OK here.")]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        /// <summary>
        /// The start hooking.
        /// </summary>
        public static void StartHooking()
        {
            hookId = SetHook(Proc);
            Application.Run();
            UnhookWindowsHookEx(hookId);
        }

        /// <summary>
        /// Gets or sets the _context.
        /// </summary>
        public static ActionContext InternalContext { get; set; }

        /// <summary>
        /// Gets or sets the _ set event action trigger.
        /// </summary>
        public static ActionTrigger InternalActionTrigger { get; set; }

        /// <summary>
        /// Gets or sets the _ data context.
        /// </summary>
        public static byte[] InternalDataContext { get; set; }

        [TriggerPropertyContract("Syncronous", "Trigger Syncronous")]
        public bool Syncronous { get; set; }

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
        [TriggerActionContract("{26FB94BE-ABC6-4EEA-A94C-ECA6FFDB4704}", "Main action", "Main action description")]
        public byte[] Execute(ActionTrigger actionTrigger, ActionContext context)
        {
            trigger = this;
            InternalContext = context;
            InternalActionTrigger = actionTrigger;
            InternalDataContext = this.DataContext;
            StartHooking();
            return null;
        }
    }
}