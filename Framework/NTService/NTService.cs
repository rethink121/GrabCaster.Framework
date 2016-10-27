// NTService.cs
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