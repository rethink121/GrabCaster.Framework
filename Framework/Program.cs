// --------------------------------------------------------------------------------------------------
// <copyright file = "Program.cs" company="GrabCaster Ltd">
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
namespace GrabCaster.Framework
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.ServiceProcess;
    using System.Threading;
    using System.Windows.Forms;

    using GrabCaster.Framework.Base;
    using GrabCaster.Framework.Common;
    using GrabCaster.Framework.Engine;
    using GrabCaster.Framework.Log;
    using GrabCaster.Framework.NTService;

    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// Class containing the main entry to the program.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Handles the ProcessExit event of the CurrentDomain control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        public static void CurrentDomainProcessExit(object sender, EventArgs e)
        {
            CoreEngine.StopEventEngine();
        } // CurrentDomain_ProcessExit

        /// <summary>
        /// Mains the main entry to the program.
        /// </summary>
        /// <param name="args">The arguments to the program.</param>
        /// <exception cref="System.NotImplementedException">
        /// Exception thrown if incorrect parameters are passed to the command-line.
        /// </exception>
        public static void Main(string[] args)
        {
            try
            {

                LogEngine.Init();
                Debug.WriteLine(
                    $"Version {Assembly.GetExecutingAssembly().GetName().Version}",
                    ConsoleColor.Green);

                if (ConfigurationBag.Configuration.DisableExternalEventsStreamEngine)
                {
                    LogEngine.WriteLog(
                        ConfigurationBag.EngineName,
                        "Warning the Device Provider Interface is disable, the GrabCaster point will be able to work in local mode only.",
                        Constant.LogLevelWarning,
                        Constant.TaskCategoriesError,
                        null,
                        Constant.LogLevelWarning);
                }

                if (!Environment.UserInteractive)
                {
                    Debug.WriteLine("GrabCaster-services To Run - Not UserInteractive Environment the service will start in ServiceBase mode.");
                    if (args.Length > 0)
                    {
                        //Run in batch and console mode
                        Debug.WriteLine($"GrabCaster-services To Run - Command line > 0 start NT Service mode . args = {args[0]}.");
                        switch (args[0].ToUpper())
                        {
                            case "S":
                                Debug.WriteLine("GrabCaster-services To Run - Service Fabric mode requested.");
                                Debug.WriteLine(
                                    "--GrabCaster Sevice Initialization--Start Engine.",
                                    ConsoleColor.Green);
                                CoreEngine.StartEventEngine(null);
                                Console.ReadLine();
                                break;
                            case "M":
                                AppDomain.CurrentDomain.ProcessExit += CurrentDomainProcessExit;
                                Debug.WriteLine(
                                    "--GrabCaster Sevice Initialization--Start Engine.",
                                    ConsoleColor.Green);
                                CoreEngine.StartEventEngine(null);
                                Console.WriteLine("\rEngine started...");
                                Console.ReadLine();
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
     
                        Debug.WriteLine("GrabCaster-services To Run - Command line = 0 start NT Service mode.");
                        Debug.WriteLine($"GrabCaster-services To Run - Environment.OSVersion:{Environment.OSVersion} Environment.Version:{Environment.Version}");
                        Debug.WriteLine("GrabCaster-services To Run procedure initialization.");
                        ServiceBase[] servicesToRun = { new NTWindowsService() };
                        Debug.WriteLine("GrabCaster-services To Run procedure starting.");
                        ServiceBase.Run(servicesToRun);

                    }
                }
                else
                {

                    if (args.Length == 0)
                    {

                        // Set Console windows
                        Console.Title = ConfigurationBag.Configuration.PointName;
                        Console.SetWindowPosition(0, 0);
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(@"[M] Run GrabCaster in MS-DOS Console mode.");
                        Console.WriteLine(@"[I] Install GrabCaster Windows NT Service.");
                        Console.WriteLine(@"[U] Uninstall GrabCaster Windows NT Service.");
                        Console.WriteLine(@"[C] Clone a new GrabCaster Point.");
                        Console.WriteLine(@"[Ctrl + C] Exit.");
                        Console.ForegroundColor = ConsoleColor.White;
                        var consoleKeyInfo = Console.ReadKey();

                        switch (consoleKeyInfo.Key)
                        {
                            case ConsoleKey.M:
                                AppDomain.CurrentDomain.ProcessExit += CurrentDomainProcessExit;
                                Debug.WriteLine(
                                    "--GrabCaster Sevice Initialization--Start Engine.",
                                    ConsoleColor.Green);
                                CoreEngine.StartEventEngine(null);
                                Console.WriteLine("\rEngine started...");
                                Console.ReadLine();
                                break;
                            case ConsoleKey.I:
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.Clear();
                                CoreNtService.ServiceName = AskInputLine("Specify the Windows NT Service Name:");
                                CoreNtService.InstallService();
                                break;
                            case ConsoleKey.U:
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.Clear();
                                CoreNtService.ServiceName = AskInputLine("Specify the Windows NT Service Name:");
                                CoreNtService.StopService();
                                CoreNtService.UninstallService();
                                break;
                            case ConsoleKey.C:
                                //string CloneName = AskInputLine("Enter the Clone name.");
                                Process.Start(Path.Combine(Application.StartupPath, "Create new Clone.cmd"));
                                break;
                        }
                    } //if
                    else
                    {
                        //Run in batch and console mode
                        if (args.Length == 1 && args[0].ToUpper() == "M")
                        {
                            Debug.WriteLine(
                                "--GrabCaster Sevice Initialization--Start Engine.",
                                ConsoleColor.Green);
                            CoreEngine.StartEventEngine(null);
                            Console.WriteLine("\rEngine started...");
                            Console.ReadLine();
                        }
                        else if (args.Length == 2 && args[0].ToUpper() == "I")
                        {
                            CoreNtService.ServiceName = string.Concat("GrabCaster", args[1]);
                            CoreNtService.InstallService();
                            Environment.Exit(0);
                        }
                        else if (args.Length == 2 && args[0].ToUpper() == "U")
                        {
                            CoreNtService.ServiceName = string.Concat("GrabCaster", args[1]);
                            CoreNtService.StopService();
                            CoreNtService.UninstallService();
                            Environment.Exit(0);
                        }
                        else
                        {
                            Console.Clear();
                            Debug.WriteLine(
                                @"GrabCaster [M]  [I] [U]",
                                ConsoleColor.Green);
                            Debug.WriteLine("M", ConsoleColor.DarkGreen);
                            Debug.WriteLine(
                                @"[M] Execute GrabCaster in MS Dos console mode.",
                                ConsoleColor.Green);
                            Debug.WriteLine(
                                "[I servicename] Install GrabCaster as Windows NT Service servicename.",
                                ConsoleColor.Green);
                            Debug.WriteLine(
                                "[U servicename] Uninstall GrabCaster Windows NT Service servicename.",
                                ConsoleColor.Green);

                            Console.ReadLine();
                            Environment.Exit(0);
                        }
                    }
                } // if
            }
            catch (NotImplementedException ex)
            {
                LogEngine.WriteLog(
                    ConfigurationBag.EngineName,
                    "Error in " + MethodBase.GetCurrentMethod().Name,
                    Constant.LogLevelError,
                    Constant.TaskCategoriesError,
                    ex,
                    Constant.LogLevelError);
            }
            catch (Exception ex)
            {
                LogEngine.WriteLog(
                    ConfigurationBag.EngineName,
                    "Error in " + MethodBase.GetCurrentMethod().Name,
                    Constant.LogLevelError,
                    Constant.TaskCategoriesError,
                    ex,
                    Constant.LogLevelError);
                Environment.Exit(0);
            } // try/catch
            finally
            {
                //Spool log queues
                if (LogEngine.QueueAbstractMessage != null)
                {
                    LogEngine.QueueAbstractMessageOnPublish(LogEngine.QueueAbstractMessage.ToArray().ToList());
                }
                if (LogEngine.QueueConsoleMessage != null)
                {
                    LogEngine.QueueConsoleMessageOnPublish(LogEngine.QueueConsoleMessage.ToArray().ToList());
                }
            }
        } // Main

        private static string AskInputLine(string message)
        {
            var ret = string.Empty;
            while (ret == string.Empty)
            {
                Debug.WriteLine(message, ConsoleColor.Green);
                ret = Console.ReadLine();
            }
            return ret;
        }
    } // Program
} // namespace