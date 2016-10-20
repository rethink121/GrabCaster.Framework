// --------------------------------------------------------------------------------------------------
// <copyright file = "Compression.cs" company="GrabCaster Ltd">
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrabCaster.Framework.CompressionLibrary
{
    using System.Diagnostics;
    using System.IO;
    using System.IO.Compression;
    using System.Reflection;

    using GrabCaster.Framework.Base;
    using GrabCaster.Framework.Log;

    public static class Helpers
    {
        /// <summary>
        /// Compress a folder
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <returns>a byte array</returns>
        public static byte[] CreateFromDirectory(string directoryPath)
        {
            string zipFolderFile = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid().ToString()}.zip");

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
        /// Decompress byte stream
        /// </summary>
        /// <param name="fileContent"></param>
        public static void CreateFromBytearray(byte[] fileContent,string unzipFolder)
        {
            string unzipFolderFile = Path.Combine(Path.GetTempPath(),$"{Guid.NewGuid().ToString()}.zip");
            

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
