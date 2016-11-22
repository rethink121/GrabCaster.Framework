// ChatTrigger.cs
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
using System.Text;
using System.Windows.Forms;
using GrabCaster.Framework.Base;
using GrabCaster.Framework.Contracts.Attributes;
using GrabCaster.Framework.Contracts.Globals;
using GrabCaster.Framework.Contracts.Triggers;

#endregion

namespace GrabCaster.Framework.ChatTrigger
{
    /// <summary>
    ///     The chat trigger.
    /// </summary>
    [TriggerContract("{B515A376-B91E-495C-ADEE-2AD3DC54C2B3}", "ChatTrigger", "Create a P2P chat bridge.", false, true,
         false)]
    public class ChatTrigger : ITriggerType
    {
        /// <summary>
        ///     The w h_ keyboar d_ ll.
        /// </summary>
        private const int WH_KEYBOARD_LL = 13;

        /// <summary>
        ///     The w m_ keydown.
        /// </summary>
        private const int WM_KEYDOWN = 0x0100;

        /// <summary>
        ///     The line chat.
        /// </summary>
        private static readonly StringBuilder LineChat = new StringBuilder();

        /// <summary>
        ///     The _proc.
        /// </summary>
        private static readonly LowLevelKeyboardProc Proc =HookCallback;

        /// <summary>
        ///     The hook id.
        /// </summary>
        private static IntPtr hookId = IntPtr.Zero;

        /// <summary>
        ///     The _ trigger.
        /// </summary>
        private static ITriggerType trigger;

        /// <summary>
        ///     Gets or sets the _context.
        /// </summary>
        public static ActionContext InternalContext { get; set; }

        /// <summary>
        ///     Gets or sets the _ set event action trigger.
        /// </summary>
        public static ActionTrigger InternalActionTrigger { get; set; }

        /// <summary>
        ///     Gets or sets the _ data context.
        /// </summary>
        public static byte[] InternalDataContext { get; set; }

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
        [TriggerActionContract("{26FB94BE-ABC6-4EEA-A94C-ECA6FFDB4704}", "Main action", "Main action description")]
        public byte[] Execute(ActionTrigger actionTrigger, ActionContext context)
        {
            trigger = this;
            InternalContext = context;
            InternalActionTrigger = actionTrigger;
            InternalDataContext = DataContext;
            StartHooking();
            return null;
        }

        /// <summary>
        ///     The set hook.
        /// </summary>
        /// <param name="proc">
        ///     The proc.
        /// </param>
        /// <returns>
        ///     The <see cref="IntPtr" />.
        /// </returns>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly",
             Justification = "Reviewed. Suppression is OK here.")]
        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (var curProcess = Process.GetCurrentProcess())
            using (var curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        /// <summary>
        ///     The hook callback.
        /// </summary>
        /// <param name="nCode">
        ///     The n code.
        /// </param>
        /// <param name="wParam">
        ///     The w param.
        /// </param>
        /// <param name="lParam">
        ///     The l param.
        /// </param>
        /// <returns>
        ///     The <see cref="IntPtr" />.
        /// </returns>
        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (wParam == (IntPtr) WM_KEYDOWN)
            {
                var notepads = Process.GetProcessesByName("notepad");
                if (notepads.Length > 0)
                {
                    if (notepads[0] != null && notepads[0].MainWindowTitle.ToUpper() == "CHAT.TXT - NOTEPAD")
                    {
                        var virtualkCode = Marshal.ReadInt32(lParam);
                        if (virtualkCode != 13)
                        {
                            LineChat.Append((char) virtualkCode);
                        }

                        if (virtualkCode == 13)
                        {

                            trigger.DataContext =
                                EncodingDecoding.EncodingString2Bytes(
                                    string.Concat("[", ConfigurationBag.Configuration.PointName, "]: ",
                                        LineChat.ToString()));
                            LineChat.Clear();
                            InternalActionTrigger(trigger, InternalContext);
                        }
                    }
                }
            }

            return CallNextHookEx(hookId, nCode, wParam, lParam);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]

        private static extern IntPtr SetWindowsHookEx(
            int idHook,
            LowLevelKeyboardProc lpfn,
            IntPtr hMod,
            uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        /// <summary>
        ///     The start hooking.
        /// </summary>
        public static void StartHooking()
        {
            hookId = SetHook(Proc);
            Application.Run();
            UnhookWindowsHookEx(hookId);
        }

        /// <summary>
        ///     The low level keyboard proc.
        /// </summary>
        /// <param name="nCode">
        ///     The n code.
        /// </param>
        /// <param name="wParam">
        ///     The w param.
        /// </param>
        /// <param name="lParam">
        ///     The l param.
        /// </param>
        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);
    }
}