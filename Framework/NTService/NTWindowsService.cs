// --------------------------------------------------------------------------------------------------
// <copyright file = "NTWindowsService.cs" company="GrabCaster Ltd">
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
namespace GrabCaster.Framework.NTService
{
    using System;
    using System.Diagnostics;
    using System.Reflection;
    using System.ServiceProcess;
    using System.Threading;

    using GrabCaster.Framework.Base;
    using GrabCaster.Framework.Engine;
    using GrabCaster.Framework.Log;

    /// <summary>
    /// Component that represents the Windows Service.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public partial class NTWindowsService : ServiceBase
    {
        /// <summary>
        /// Starts the core Engine.
        /// </summary>
        public static void StartEngine()
        {
            try
            {
                // Start NT service
                Debug.WriteLine("LogEventUpStream - Initialization--Start Engine.");
                Debug.WriteLine("Initialization--Start Engine.", ConsoleColor.Green);
                LogEngine.Init();
                Debug.WriteLine("LogEventUpStream - StartEventEngine.");
                CoreEngine.StartEventEngine(null);
            }
            catch (Exception ex)
            {
                LogEngine.WriteLog(
                    ConfigurationBag.EngineName, 
                    $"Error in {MethodBase.GetCurrentMethod().Name}", 
                    Constant.LogLevelError, 
                    Constant.TaskCategoriesError, 
                    ex, 
                    Constant.LogLevelError);
                Thread.Sleep(ConfigurationBag.Configuration.WaitTimeBeforeRestarting);
                Environment.Exit(0);
            }
        }
 // StartEngine

        /// <summary>
        /// Called when the Windows Service starts.
        /// </summary>
        /// <param name="args">
        /// The arguments.
        /// </param>
        protected override void OnStart(string[] args)
        {
            try
            {
                // ******************************************
                LogEngine.WriteLog(
                    ConfigurationBag.EngineName, 
                    $"Instance {this.ServiceName} engine starting.", 
                    Constant.LogLevelError, 
                    Constant.TaskCategoriesError, 
                    null, 
                    Constant.LogLevelInformation);

                var engineThreadProcess = new Thread(StartEngine);

                engineThreadProcess.Start();

                LogEngine.WriteLog(
                    ConfigurationBag.EngineName, 
                    $"Instance {this.ServiceName} engine started.", 
                    Constant.LogLevelError, 
                    Constant.TaskCategoriesError, 
                    null, 
                    Constant.LogLevelInformation);
            }
            catch (Exception ex)
            {
                LogEngine.WriteLog(
                    ConfigurationBag.EngineName, 
                    $"Error in {MethodBase.GetCurrentMethod().Name}", 
                    Constant.LogLevelError, 
                    Constant.TaskCategoriesError, 
                    ex, 
                    Constant.LogLevelError);
                Thread.Sleep(ConfigurationBag.Configuration.WaitTimeBeforeRestarting);
                Environment.Exit(0);
            }
        }
 // OnStart

        /// <summary>
        /// Called when Windows Service stops.
        /// </summary>
        protected override void OnStop()
        {
            CoreEngine.StopEventEngine();
        }
 // OnStop
    } // NTWindowsService
} // namespace