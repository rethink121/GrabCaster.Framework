// TestMessageHelper.cs
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

using System.IO;

namespace BTSG.TestPipeline
{
    /// <summary>
    /// Helper class for messages
    /// </summary>
    internal class TestMessageHelper
    {
        /// <summary>
        /// Saves a message to disk
        /// </summary>
        /// <param name="path"></param>
        /// <param name="data"></param>
        internal static void SaveMessage(string path, Stream data)
        {
            StreamReader rdr = new StreamReader(data);
            StreamWriter writer = new StreamWriter(path);
            writer.Write(rdr.ReadToEnd());
            writer.Flush();
            data.Seek(0, SeekOrigin.Begin);
            writer.Close();
        }

        /// <summary>
        /// Loads a message from disk
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        internal static Stream LoadMessage(string path)
        {
            StreamReader rdr = new StreamReader(path);
            return rdr.BaseStream;
        }
    }
}