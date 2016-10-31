// CoreEngine.cs
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
using System.Reflection;
using System.Text;
using System.Threading;
using GrabCaster.Framework.Base;
using GrabCaster.Framework.Contracts.Globals;
using GrabCaster.Framework.Engine.OffRamp;
using GrabCaster.Framework.Engine.OnRamp;
using GrabCaster.Framework.Log;

#endregion

namespace GrabCaster.Framework.Engine
{
    /// <summary>
    ///     Primary engine, it start all, this is the first point
    /// </summary>
    public static class CoreEngine
    {
        private static int pollingStep;

        public static Assembly HandleAssemblyResolve(object sender, ResolveEventArgs args)
        {
            /* Load the assembly specified in 'args' here and return it, 
               if the assembly is already loaded you can return it here */
            try
            {
                if (args.Name.Substring(0, 10) == "Microsoft.")
                {
                    return null;
                }
                if (args.Name.Substring(0, 7) == "System.")
                {
                    return null;
                }

                return EventsEngine.CacheEngineComponents[args.Name];
            }
            catch (Exception ex)
            {
                var sb = new StringBuilder();
                sb.AppendLine(
                    $"Critical error in {MethodBase.GetCurrentMethod().Name} - The Assembly [{args.Name}] not found.");
                sb.AppendLine(
                    "Workaround: this error because a trigger or event is looking for a particular external library in reference, check if all the libraries referenced by triggers and events are in the triggers and events directories dll or registered in GAC.");

                LogEngine.WriteLog(
                    ConfigurationBag.EngineName,
                    sb.ToString(),
                    Constant.LogLevelError,
                    Constant.TaskCategoriesError,
                    ex,
                    Constant.LogLevelError);
                return null;
            }
        }

        /// <summary>
        ///     Execute polling
        /// </summary>
        /// restart
        public static void StartEventEngine(ActionEvent delegateEmbedded)
        {
            try
            {
                LogEngine.DirectEventViewerLog("Engine starting...", 4);
                var current = AppDomain.CurrentDomain;
                current.AssemblyResolve += HandleAssemblyResolve;

                LogEngine.Enabled = ConfigurationBag.Configuration.LoggingEngineEnabled;
                Debug.WriteLine("Load Engine configuration.");

                //****************************Check for updates
                //Check if need to update files received from partners
                Debug.WriteLine(
                    $"Check Engine Syncronization {ConfigurationBag.Configuration.AutoSyncronizationEnabled}.",
                    ConsoleColor.White);
                if (ConfigurationBag.Configuration.AutoSyncronizationEnabled)
                {
                    EventsEngine.SyncronizePoint();
                }

                //****************************Check for updates

                //Set service states
                Debug.WriteLine("Initialize Engine Service states.");
                ServiceStates.RunPolling = ConfigurationBag.Configuration.RunInternalPolling;
                ServiceStates.RestartNeeded = false;

                Debug.WriteLine("Initialize Engine.");
                EventsEngine.InitializeEventEngine(delegateEmbedded);

                //Init Message ingestor
                MessageIngestor.InitSecondaryPersistProvider();

                //Create the two sends layers
                // in EventsEngine
                if (!ConfigurationBag.Configuration.DisableExternalEventsStreamEngine)
                {
                    Debug.WriteLine("Start Internal Event Engine Channel.");
                    var canStart = EventsEngine.CreateEventUpStream();

                    if (!canStart)
                    {
                        LogEngine.WriteLog(
                            ConfigurationBag.EngineName,
                            $"Error during engine service starting. Name: {ConfigurationBag.EngineName} - ID: {ConfigurationBag.Configuration.ChannelId}",
                            Constant.LogLevelError,
                            Constant.TaskCategoriesError,
                            null,
                            Constant.LogLevelError);
                        Thread.Sleep(ConfigurationBag.Configuration.WaitTimeBeforeRestarting);
                        Environment.Exit(0);
                    }

                    //in EventUpStream
                    Debug.WriteLine("Start External Event Engine Channel.");
                    //OnRamp start the OnRamp Engine
                    canStart = OffRampEngineSending.Init("MSP Device Component.dll (vNext)");

                    if (EventsEngine.HAEnabled)
                    {
                        Thread haCheck = new Thread(EventsEngine.HAPointsUpdate);
                        haCheck.Start();
                        Thread haClean = new Thread(EventsEngine.HAPointsClean);
                        haClean.Start();
                    }

                    if (!canStart)
                    {
                        LogEngine.WriteLog(
                            ConfigurationBag.EngineName,
                            $"Error during engine service starting. Name: {ConfigurationBag.Configuration.ChannelName} - ID: {ConfigurationBag.Configuration.ChannelId}",
                            Constant.LogLevelError,
                            Constant.TaskCategoriesError,
                            null,
                            Constant.LogLevelError);
                        Thread.Sleep(ConfigurationBag.Configuration.WaitTimeBeforeRestarting);
                        Environment.Exit(0);
                    }
                }

                //*****************Event object stream area*********************
                //Load the global event and triggers dlls
                var numOfTriggers = 0;
                var numOfEvents = 0;
                var numOfComponents = 0;

                var triggersAndEventsLoaded = EventsEngine.LoadAssemblyComponents(ref numOfTriggers, ref numOfEvents,
                    ref numOfComponents);
                if (triggersAndEventsLoaded)
                {
                    Debug.WriteLine(
                        $"Triggers loaded {numOfTriggers} - Events loaded {numOfEvents}",
                        ConsoleColor.DarkCyan);
                }

                //Load the Active triggers and the active events
                EventsEngine.RefreshBubblingSetting();
                //Start triggers single instances
                EventsEngine.ExecuteBubblingTriggerConfigurationsSingleInstance();
                //Start triggers polling instances
                if (ConfigurationBag.Configuration.EnginePollingTime > 0)
                {
                    var treadPollingRun = new Thread(StartTriggerPolling);
                    treadPollingRun.Start();
                }
                else
                {
                    LogEngine.WriteLog(ConfigurationBag.EngineName,
                        $"Configuration.EnginePollingTime = {ConfigurationBag.Configuration.EnginePollingTime}, internal polling system disabled.",
                        Constant.LogLevelError,
                        Constant.TaskCategoriesError,
                        null,
                        Constant.LogLevelWarning);
                }

                //Start Engine Service
                Debug.WriteLine(
                    "Asyncronous Threading Service state active.",
                    ConsoleColor.DarkCyan);
                var treadEngineStates = new Thread(CheckServiceStates);
                treadEngineStates.Start();

                if (!ConfigurationBag.Configuration.DisableExternalEventsStreamEngine)
                {
                    Debug.WriteLine("Start On Ramp Engine.", ConsoleColor.Green);
                    var onRampEngineReceiving = new OnRampEngineReceiving();
                    onRampEngineReceiving.Init("component.dll name");
                }
                // Configuration files watcher
                //EventsEngine.StartConfigurationSyncEngine();

                LogEngine.WriteLog(
                    ConfigurationBag.EngineName,
                    $"Engine service initialization procedure terminated. Name: {ConfigurationBag.Configuration.ChannelName} - ID: {ConfigurationBag.Configuration.ChannelId}",
                    Constant.LogLevelError,
                    Constant.TaskCategoriesError,
                    null,
                    Constant.LogLevelInformation);
                LogEngine.DirectEventViewerLog("Engine started.", 4);
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
            }
        }

        public static void StopEventEngine()
        {
            //EventsEngine.DisposeEngine();
        }

        /// <summary>
        ///     If restart required it perform the operations
        /// </summary>
        public static void CheckServiceStates()
        {
            while (true)
            {
                Thread.Sleep(10000);
                if (ServiceStates.RestartNeeded)
                {
                    Debug.WriteLine(
                        "--------------------------------------------------------",
                        ConsoleColor.DarkYellow);
                    Debug.WriteLine(
                        "Service needs restarting.",
                        ConsoleColor.Red);
                    Debug.WriteLine(
                        "--------------------------------------------------------",
                        ConsoleColor.DarkYellow);
                    ServiceStates.RestartNeeded = false;
                    //Thread.Sleep(ConfigurationBag.Configuration.WaitTimeBeforeRestarting);
                }
            }
            // ReSharper disable once FunctionNeverReturns
        }

        /// <summary>
        ///     Execute polling
        /// </summary>
        private static void StartTriggerPolling()
        {
            //running thread polling
            var pollingTime = ConfigurationBag.Configuration.EnginePollingTime;
            try
            {
                if (pollingTime == 0)
                {
                    LogEngine.WriteLog(
                        ConfigurationBag.EngineName,
                        $"EnginePollingTime = 0 - Internal logging system disabled.",
                        Constant.LogLevelError,
                        Constant.TaskCategoriesError,
                        null,
                        Constant.LogLevelWarning);
                    return;
                }
                Debug.WriteLine("Start Trigger Polling Cycle", ConsoleColor.Blue);


                while (ServiceStates.RunPolling)
                {
                    if (ServiceStates.RestartNeeded)
                    {
                        Debug.WriteLine(
                            "--------------------------------------------------------",
                            ConsoleColor.DarkYellow);
                        Debug.WriteLine("- UPDATE READY - SERVICE NEEDS RESTART -", ConsoleColor.Red);
                        Debug.WriteLine(
                            "--------------------------------------------------------",
                            ConsoleColor.DarkYellow);
                        //ServiceStates.RunPolling = false;
                        return;
                    }
                    ++pollingStep;
                    if (pollingStep > 9)
                    {
                        Debug.WriteLine(
                            $"Execute Trigger Polling {pollingStep} Cycle",
                            ConsoleColor.Blue);
                        pollingStep = 0;
                    }
                    Thread.Sleep(pollingTime);
                    var treadPollingRun = new Thread(EventsEngine.ExecuteBubblingTriggerConfigurationPolling);
                    treadPollingRun.Start();
                }
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
            }
        }
    }
}