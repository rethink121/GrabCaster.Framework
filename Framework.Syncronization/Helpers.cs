// --------------------------------------------------------------------------------------------------
// <copyright file = "Helpers.cs" company="GrabCaster Ltd">
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
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using GrabCaster.Framework.Base;
using GrabCaster.Framework.Log;

namespace GrabCaster.Framework.Syncronization
{
    using System.IO;

    using Microsoft.Synchronization;
    using Microsoft.Synchronization.Files;

    /// <summary>
    /// Syncronozation Class
    /// </summary>
    public static class Helpers
    {
        public static string syncFile = "SyncronizationStatus.gc";



    
        /// <summary>
        /// Syncronize 2 folders
        /// </summary>
        /// <param name="SourceFolder"></param>
        /// <param name="DestinationFolder"></param>
        public static void SyncFolders(string SourceFolder, string DestinationFolder)
        {
            SyncOrchestrator syncOrchestrator = new SyncOrchestrator();


            syncOrchestrator.LocalProvider = new FileSyncProvider(SourceFolder);
            syncOrchestrator.RemoteProvider = new FileSyncProvider(DestinationFolder);
            syncOrchestrator.Synchronize();
        }
        

        //Less than zero t1 is earlier than t2.
        //Zero t1 is the same as t2.
        //Greater than zero t1 is later than t2.
        public static bool ToBeSyncronized(string sourceFolder, string restinationFolder, bool syncronize)
        {
            int synTo = 0;
            try
            {
                if (syncronize)
                {
                    SyncFolders(sourceFolder, restinationFolder);
                }
                LogEngine.DirectEventViewerLog("Point syncronization done without errors.",4);
                return true;
            }
            catch (Exception ex)
            {

                LogEngine.WriteLog(ConfigurationBag.EngineName,
                              $"Error in {MethodBase.GetCurrentMethod().Name}",
                              Constant.LogLevelError,
                              Constant.TaskCategoriesError,
                              ex,
                              Constant.LogLevelError);
                return false;

            }
            
        }
    }
}
