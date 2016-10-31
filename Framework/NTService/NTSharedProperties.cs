// NTSharedProperties.cs
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

using System.ServiceProcess;

#endregion

namespace GrabCaster.Framework.NTService
{
    /// <summary>
    ///     Properties for the Windows Service.
    /// </summary>
    internal static class NtSharedProperties
    {
        /// <summary>
        ///     Gets or sets the account for the Windows Service.
        /// </summary>
        /// <value>
        ///     The account for the Windows Service.
        /// </value>
        public static ServiceAccount Account { get; set; }

        /// <summary>
        ///     Gets or sets the password for the Windows Service.
        /// </summary>
        /// <value>
        ///     The password for the Windows Service.
        /// </value>
        public static string Password { get; set; }

        /// <summary>
        ///     Gets or sets the username for the Windows Service.
        /// </summary>
        /// <value>
        ///     The username for the Windows Service.
        /// </value>
        public static string Username { get; set; }

        /// <summary>
        ///     Gets or sets the name for the Windows Service.
        /// </summary>
        /// <value>
        ///     The name for the Windows Service.
        /// </value>
        public static string WindowsNtServiceName { get; set; }

        /// <summary>
        ///     Gets or sets the display name for the Windows Service.
        /// </summary>
        /// <value>
        ///     The display name for the Windows Service.
        /// </value>
        public static string DisplayName { get; set; }

        /// <summary>
        ///     Gets or sets the description for the Windows Service.
        /// </summary>
        /// <value>
        ///     The description for the Windows Service.
        /// </value>
        public static string Description { get; set; }
    } // NTSharedProperties
} // namespace