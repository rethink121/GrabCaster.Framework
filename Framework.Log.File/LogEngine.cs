// LogEngine.cs
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
using System.IO;
using GrabCaster.Framework.Base;
using GrabCaster.Framework.Contracts.Attributes;
using GrabCaster.Framework.Contracts.Log;

#endregion

namespace GrabCaster.Framework.Log.File
{
    /// <summary>
    ///     The log engine, simple version.
    /// </summary>
    [LogContract("{4DACE829-1462-4A3D-ACC9-1EE41B3C2D53}", "LogEngine", "File Log System")]
    public class LogEngine : ILogEngine
    {
        StreamWriter logFile;
        private string PathFile = "";

        /// <summary>
        ///     Initialize log.
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public bool InitLog()
        {
            Directory.CreateDirectory(ConfigurationBag.DirectoryLog());
            PathFile = Path.Combine(ConfigurationBag.DirectoryLog(),
                $"{DateTime.Now.Month}{DateTime.Now.Day}{DateTime.Now.Year}-{Guid.NewGuid()}.txt");
            logFile = System.IO.File.AppendText(PathFile);
            return true;
        }

        /// <summary>
        ///     The write log.
        /// </summary>
        /// <param name="logMessage">
        ///     The log message.
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public bool WriteLog(LogMessage logMessage)
        {
            lock (logFile)
            {
                logFile.WriteLine($"{DateTime.Now} - {logMessage.Message}");
                return true;
            }
        }

        public void Flush()
        {
            logFile.Flush();
        }
    }
}