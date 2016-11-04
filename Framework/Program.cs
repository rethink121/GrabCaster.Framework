// Program.cs
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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Windows.Forms;
using GrabCaster.Framework.Base;
using GrabCaster.Framework.Engine;
using GrabCaster.Framework.Log;
using GrabCaster.Framework.NTService;

#endregion

namespace GrabCaster.Framework
{
    /// <summary>
    ///     Class containing the main entry to the program.
    /// </summary>
    public class Program
    {
        /// <summary>
        ///     Handles the ProcessExit event of the CurrentDomain control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        public static void CurrentDomainProcessExit(object sender, EventArgs e)
        {
            CoreEngine.StopEventEngine();
        } // CurrentDomain_ProcessExit

        /// <summary>
        ///     Mains the main entry to the program.
        /// </summary>
        /// <param name="args">The arguments to the program.</param>
        /// <exception cref="System.NotImplementedException">
        ///     Exception thrown if incorrect parameters are passed to the command-line.
        /// </exception>
        public static void Main(string[] args)
        {
            try
            {
                LogEngine.Init();
                Debug.WriteLine(
                    $"Version {Assembly.GetExecutingAssembly().GetName().Version}",
                    ConsoleColor.Green);

                if (!Environment.UserInteractive)
                {
                    Debug.WriteLine(
                        "GrabCaster-services To Run - Not UserInteractive Environment the service will start in ServiceBase mode.");
                    if (args.Length > 0)
                    {
                        //Run in batch and console mode
                        Debug.WriteLine(
                            $"GrabCaster-services To Run - Command line > 0 start NT Service mode . args = {args[0]}.");
                        switch (args[0].ToUpper())
                        {
                            case "S":
                                Debug.WriteLine("GrabCaster-services To Run - Service Fabric mode requested.");
                                Debug.WriteLine(
                                    "--GrabCaster Sevice Initialization--Start Engine.");
                                CoreEngine.StartEventEngine(null);
                                Console.WriteLine("\rEngine started...");
                                Console.ReadLine();
                                break;
                            case "M":
                                AppDomain.CurrentDomain.ProcessExit += CurrentDomainProcessExit;
                                Debug.WriteLine(
                                    "--GrabCaster Sevice Initialization--Start Engine.");
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
                        Debug.WriteLine(
                            $"GrabCaster-services To Run - Environment.OSVersion:{Environment.OSVersion} Environment.Version:{Environment.Version}");
                        Debug.WriteLine("GrabCaster-services To Run procedure initialization.");
                        ServiceBase[] servicesToRun = {new NTWindowsService()};
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
                        Console.WriteLine(@"[O] Clone a new GrabCaster Point.");
                        Console.WriteLine(@"[Ctrl + C] Exit.");
                        Console.ForegroundColor = ConsoleColor.White;
                        var consoleKeyInfo = Console.ReadKey();

                        string param1 = ";";

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
                            case ConsoleKey.C:
                                //string CloneName = AskInputLine("Enter the Clone name.");
                                Process.Start(Path.Combine(Application.StartupPath, "Create new Clone.cmd"));
                                break;
                            case ConsoleKey.I:
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.Clear();
                                Console.WriteLine("Specify the Windows NT Service Name and press Enter:");
                                CoreNtService.ServiceName = AskInputLine("Specify the Windows NT Service Name:");
                                CoreNtService.InstallService();
                                break;
                            case ConsoleKey.U:
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.Clear();
                                Console.WriteLine("Specify the Windows NT Service Name and press Enter:");
                                CoreNtService.ServiceName = AskInputLine("Specify the Windows NT Service Name:");
                                CoreNtService.StopService();
                                CoreNtService.UninstallService();
                                break;
                            case ConsoleKey.S:
                                //string CloneName = AskInputLine("Enter the Clone name.");
                                Process.Start(Path.Combine(Application.StartupPath, "Create new Clone.cmd"));
                                break;
                            case ConsoleKey.B:
                                param1 = AskInputLine("Enter the BizTalk installation folder.");
                                Process.Start(Path.Combine(Application.StartupPath, $"Create new Clone.cmd {param1}"));
                                break;
                            case ConsoleKey.O:
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