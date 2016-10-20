// --------------------------------------------------------------------------------------------------
// <copyright file = "NTService.cs" company="GrabCaster Ltd">
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
    using System.Collections;
    using System.Configuration.Install;
    using System.Diagnostics;
    using System.Reflection;
    using System.ServiceProcess;

    using GrabCaster.Framework.Base;
    using GrabCaster.Framework.Log;

    /// <summary>
    /// Contains helper methods to start, stop and (un)install service.
    /// </summary>
    internal static class CoreNtService
    {
        /// <summary>
        /// Gets or sets the service name.
        /// </summary>
        public static string ServiceName { get; set; }

        /// <summary>
        /// Determines whether this Windows Service is installed.
        /// </summary>
        /// <returns><c>true</c> is installed, <c>false</c> otherwise.</returns>
        public static bool IsInstalled()
        {
            using (var controller = new ServiceController(ServiceName))
            {
                try
                {
                    // ReSharper disable once UnusedVariable
                    var status = controller.Status;
                }
                catch
                {
                    return false;
                }
 // try/catch

                return true;
            }
 // using
        }
 // IsInstalled

        /// <summary>
        /// Determines whether the Windows Service is running.
        /// </summary>
        /// <returns><c>true</c> is running, <c>false</c> otherwise.</returns>
        public static bool IsRunning()
        {
            using (var controller = new ServiceController(ServiceName))
            {
                if (!IsInstalled())
                {
                    return false;
                }
 // if

                return controller.Status == ServiceControllerStatus.Running;
            }
 // using
        }
 // IsRunning

        /// <summary>
        /// Creates an <see cref="AssemblyInstaller"/> object to perform the service installation.
        /// </summary>
        /// <returns>Returns an <see cref="AssemblyInstaller"/> object to perform the service installation.</returns>
        public static AssemblyInstaller GetInstaller()
        {
            var installer = new AssemblyInstaller(typeof(NTWindowsService).Assembly, null)
                                              {
                                                  UseNewContext
                                                      = true
                                              };

            return installer;
        }
 // GetInstaller

        /// <summary>
        /// Installs the Windows Service.
        /// </summary>
        public static void InstallService()
        {
            if (IsInstalled())
            {
                LogEngine.WriteLog(
                    ConfigurationBag.EngineName, 
                    $"NT Service instance {ServiceName} is already installed.", 
                    Constant.LogLevelError, 
                    Constant.TaskCategoriesConsole, 
                    null, 
                    Constant.LogLevelInformation);
                Console.ReadLine();

                return;
            }
 // if

            using (var installer = GetInstaller())
            {
                IDictionary state = new Hashtable();
                try
                {
                    installer.Install(state);
                    installer.Commit(state);

                    LogEngine.WriteLog(
                        ConfigurationBag.EngineName, 
                        $"NT Service instance {ServiceName} installation completed.", 
                        Constant.LogLevelError, 
                        Constant.TaskCategoriesConsole, 
                        null, 
                        Constant.LogLevelInformation);
                    Console.ReadLine();
                }
                catch
                {
                    try
                    {
                        installer.Rollback(state);
                    }
                    catch
                    {
                        // ignored
                    }
                    // try/catch

                    throw;
                }
            }
        }
 // InstallService

        /// <summary>
        /// Uninstalls the Windows Service.
        /// </summary>
        public static void UninstallService()
        {
            if (!IsInstalled())
            {
                LogEngine.WriteLog(
                    ConfigurationBag.EngineName, 
                    $"NT Service instance {ServiceName} is not installed.", 
                    Constant.LogLevelError, 
                    Constant.TaskCategoriesConsole, 
                    null, 
                    Constant.LogLevelWarning);
                Console.ReadLine();

                return;
            }
 // if

            using (var installer = GetInstaller())
            {
                IDictionary state = new Hashtable();
                installer.Uninstall(state);
                LogEngine.WriteLog(
                    ConfigurationBag.EngineName, 
                    $"Service {ServiceName} Uninstallation completed.", 
                    Constant.LogLevelError, 
                    Constant.TaskCategoriesConsole, 
                    null, 
                    Constant.LogLevelInformation);
                Console.ReadLine();
            }
 // using
        }
 // UninstallService

        /// <summary>
        /// Starts the Windows Service.
        /// </summary>
        public static void StartService()
        {
            if (!IsInstalled())
            {
                LogEngine.WriteLog(
                    ConfigurationBag.EngineName, 
                    $"NT Service instance {ServiceName} is not installed.", 
                    Constant.LogLevelError, 
                    Constant.TaskCategoriesConsole, 
                    null, 
                    Constant.LogLevelWarning);
                Console.ReadLine();

                return;
            }
 // if

            try
            {
                ServiceBase[] servicesToRun = { new NTWindowsService() };
                ServiceBase.Run(servicesToRun);
            }
            catch (Exception ex)
            {
                LogEngine.WriteLog(
                    ConfigurationBag.EngineName, 
                    "Error in " + MethodBase.GetCurrentMethod().Name, 
                    Constant.LogLevelError, 
                    Constant.TaskCategoriesConsole, 
                    ex, 
                    Constant.LogLevelError);
                Console.ReadLine();

                throw;
            }
 // try/catch
        }
 // StartService

        /// <summary>
        /// Stops the Windows Service.
        /// </summary>
        public static void StopService()
        {
            if (!IsInstalled())
            {
                return;
            }

            using (var controller = new ServiceController(ServiceName))
            {
                try
                {
                    if (controller.Status != ServiceControllerStatus.Stopped)
                    {
                        controller.Stop();
                        controller.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(10));
                    }
 // if
                }
                catch (Exception ex)
                {
                    LogEngine.WriteLog(
                        ConfigurationBag.EngineName, 
                        "Error in " + MethodBase.GetCurrentMethod().Name, 
                        Constant.LogLevelError, 
                        Constant.TaskCategoriesConsole, 
                        ex, 
                        Constant.LogLevelError);
                    Console.ReadLine();

                    throw;
                }
 // try/catch
            }
 // usnig
        }
 // StopService
    } // NTService
} // namespace