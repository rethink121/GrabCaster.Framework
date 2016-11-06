// Compression.cs
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
using System.IO;
using System.IO.Compression;
using System.Reflection;
using GrabCaster.Framework.Base;
using GrabCaster.Framework.Log;

#endregion

namespace GrabCaster.Framework.CompressionLibrary
{
    public static class Helpers
    {
        /// <summary>
        ///     Compress a folder
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <returns>a byte array</returns>
        public static byte[] CreateFromDirectory(string directoryPath)
        {
            string zipFolderFile = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.zip");

            try
            {
                ZipFile.CreateFromDirectory(directoryPath, zipFolderFile, CompressionLevel.Fastest, true);
                byte[] fileStream = File.ReadAllBytes(zipFolderFile);
                File.Delete(zipFolderFile);
                return fileStream;
            }
            catch (Exception ex)
            {
                LogEngine.WriteLog(ConfigurationBag.EngineName,
                    $"Error in {MethodBase.GetCurrentMethod().Name}",
                    Constant.LogLevelError,
                    Constant.TaskCategoriesError,
                    ex,
                    Constant.LogLevelError);
                return null;
            }
        }

        /// <summary>
        ///     Decompress byte stream
        /// </summary>
        /// <param name="fileContent"></param>
        public static void CreateFromBytearray(byte[] fileContent, string unzipFolder)
        {
            string unzipFolderFile = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.zip");


            try
            {
                File.WriteAllBytes(unzipFolderFile, fileContent);
                ZipFile.ExtractToDirectory(unzipFolderFile, unzipFolder);
                File.Delete(unzipFolderFile);
            }
            catch (Exception ex)
            {
                LogEngine.WriteLog(ConfigurationBag.EngineName,
                    $"Error in {MethodBase.GetCurrentMethod().Name}",
                    Constant.LogLevelError,
                    Constant.TaskCategoriesError,
                    ex,
                    Constant.LogLevelError);
            }
        }
    }
}