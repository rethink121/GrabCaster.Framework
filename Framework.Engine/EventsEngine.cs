// EventsEngine.cs
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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using GrabCaster.Framework.Base;
using GrabCaster.Framework.Contracts;
using GrabCaster.Framework.Contracts.Attributes;
using GrabCaster.Framework.Contracts.Bubbling;
using GrabCaster.Framework.Contracts.Components;
using GrabCaster.Framework.Contracts.Configuration;
using GrabCaster.Framework.Contracts.Events;
using GrabCaster.Framework.Contracts.Globals;
using GrabCaster.Framework.Contracts.Triggers;
using GrabCaster.Framework.Engine.OffRamp;
using GrabCaster.Framework.Log;
using GrabCaster.Framework.Serialization.Object;
using GrabCaster.Framework.Syncronization;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using Roslyn.Scripting.CSharp;

#endregion

namespace GrabCaster.Framework.Engine
{
    /// <summary>
    ///     This is engine class containing the most important methods
    /// </summary>
    public static class EventsEngine
    {
        //HA Scenarios
        //If this is the mastr in gropu
        public static bool HAMaster = false;
        //If HA in enabled
        public static bool HAEnabled;
        //If need to refresh configuration
        public static bool ConfigurationUpdated;
        //iD point (Tick)
        public static long HAPointTickId;
        public static Dictionary<long, DateTime> HAPoints;

        //SyncAsync Scenarios
        public static Hashtable SyncAsyncEvents;
        public static SyncAsyncEventAction syncAsyncEventAction;

        //private static PerformanceCounters performanceCounters = new PerformanceCounters();


        // Global Action Triggers
        /// <summary>
        ///     The delegate action trigger.
        /// </summary>
        private static ActionTrigger delegateActionTrigger;

        // Global Action Events
        /// <summary>
        ///     The delegate action event.
        /// </summary>
        private static ActionEvent delegateActionEvent;

        /// <summary>
        ///     Triggers dll deployed in the bubbling folder
        /// </summary>
        public static Dictionary<string, ITriggerAssembly> CacheTriggerComponents;
        public static Dictionary<string, ITriggerType> CacheTriggerRunning;

        /// <summary>
        ///     Events dll deployed in the bubbling folder
        /// </summary>
        public static Dictionary<string, IEventAssembly> CacheEventComponents;

        /// <summary>
        ///     Chain components dll deployed in the bubbling folder
        /// </summary>
        public static Dictionary<string, IChainComponentAssembly> CacheChainComponents;

        /// <summary>
        ///     Triggers dll deployed in the bubbling folder
        /// </summary>
        public static Dictionary<string, Assembly> CacheEngineComponents;


        // Triggers and Triggers active and running
        /// <summary>
        ///     The bubbling triggers events active.
        /// </summary>
        public static List<BubblingObject> BubblingTriggersEventsActive;

        // Triggers and Events configured and running
        /// <summary>
        ///     The bubbling trigger configurations polling.
        /// </summary>
        public static List<BubblingObject> BubblingTriggerConfigurationsPolling;

        /// <summary>
        ///     The bubbling trigger configurations single instance.
        /// </summary>
        public static List<BubblingObject> BubblingTriggerConfigurationsSingleInstance;


        // Triggers and Events files configurations
        /// <summary>
        ///     The sync configuration file list.
        /// </summary>
        public static List<SyncConfigurationFile> SyncronizationConfigurationFileList;

        // All trigger files configurations
        /// <summary>
        ///     The trigger configuration list.
        /// </summary>
        public static List<TriggerConfiguration> ConfigurationJsonTriggerFileList;

        // All event files configurations
        /// <summary>
        ///     The event configuration list.
        /// </summary>
        public static Dictionary<string, EventConfiguration> ConfigurationJsonEventFileList;

        /// <summary>
        ///     The component configuration list.
        /// </summary>
        public static List<ChainConfiguration> ConfigurationJsonChainFileList;

        /// <summary>
        ///     The component configuration list.
        /// </summary>
        public static List<ComponentConfiguration> ConfigurationJsonComponentList;

        /// <summary>
        ///     The connection string.
        /// </summary>
        private static string connectionString = string.Empty;

        /// <summary>
        ///     The event hub name.
        /// </summary>
        private static string eventHubName = string.Empty;

        // ***********REST*************
        /// <summary>
        ///     The engine host.
        /// </summary>
        private static WebServiceHost engineHost;

        // ***************************
        // ********************
        // Events Sync Watcher
        /// <summary>
        ///     The fsw event folder.
        /// </summary>
        private static readonly FileSystemWatcher FswEventFolder = new FileSystemWatcher();

        /// <summary>
        ///     Gets the hub client.
        /// </summary>
        public static EventHubClient HubClient { get; private set; }

        public static BubblingBagObjet bubblingBagObject { get; set; }
        public static BubblingBag bubblingBag { get; set; }

        /// <summary>
        ///     Trigger Delegate initialization
        /// </summary>
        public static void InitializeTriggerEngine()
        {
            delegateActionTrigger = ActionTriggerReceived;
        }


        /// <summary>
        ///     Trigger Delegate initialization
        /// </summary>
        public static void InitializeEmbeddedEvent(ActionEvent delegateEmbedded)
        {
            delegateActionEvent = delegateEmbedded;
        }

        /// <summary>
        ///     Event Delegate initialization
        /// </summary>
        public static void InitializeEventEngine(ActionEvent delegateEmbedded)
        {
            int minWorker, minIOC;
            //Set ThreadPool
            int processorCount = Environment.ProcessorCount;
            ThreadPool.GetMinThreads(out minWorker, out minIOC);

            int maxWorkerThreads = ConfigurationBag.Configuration.MaxWorkerThreads < processorCount
                ? processorCount
                : ConfigurationBag.Configuration.MaxWorkerThreads;
            int maxAsyncWorkerThreads = ConfigurationBag.Configuration.MaxAsyncWorkerThreads < processorCount
                ? processorCount
                : ConfigurationBag.Configuration.MaxAsyncWorkerThreads;
            int minWorkerThreads = ConfigurationBag.Configuration.MinWorkerThreads < minWorker
                ? minWorker
                : ConfigurationBag.Configuration.MinWorkerThreads;
            int minAsyncWorkerThreads = ConfigurationBag.Configuration.MinAsyncWorkerThreads < minIOC
                ? minIOC
                : ConfigurationBag.Configuration.MinAsyncWorkerThreads;

            ThreadPool.SetMaxThreads(maxWorkerThreads, maxAsyncWorkerThreads);
            ThreadPool.SetMinThreads(minWorkerThreads, minAsyncWorkerThreads);
            string message = "Engine thread settings:\r" +
                             $"MaxWorkerThreads = {maxWorkerThreads}\r" +
                             $"MaxAsyncWorkerThreads = {maxAsyncWorkerThreads}\r" +
                             $"MinWorkerThreads = {minWorkerThreads}\r" +
                             $"MinAsyncWorkerThreads = {minAsyncWorkerThreads}";
            LogEngine.DirectEventViewerLog(message, 4);
            //HA groupd settings
            HAEnabled = ConfigurationBag.Configuration.HAGroup.Length > 0;
            if (HAEnabled)
            {
                Thread.Sleep(1);
                HAPointTickId = DateTime.Now.Ticks;
                HAPoints = new Dictionary<long, DateTime>();
                HAPoints.Add(HAPointTickId, DateTime.Now);
            }

            //initilize SyncAsync scenarions
            SyncAsyncEvents = new Hashtable();

            //Initialize Web and point
            string webApiEndpoint = ConfigurationBag.Configuration.WebApiEndPoint;

            if (webApiEndpoint != String.Empty)
            {
                // Start Web API interface
                Debug.WriteLine(
                    $"Start the Hub Web API Interface at {ConfigurationBag.Configuration.WebApiEndPoint}",
                    ConsoleColor.Yellow);
                engineHost = new WebServiceHost(typeof(RestEventsEngine),
                    new Uri(ConfigurationBag.Configuration.WebApiEndPoint));
                engineHost.AddServiceEndpoint(typeof(IRestEventsEngine), new WebHttpBinding(),
                    ConfigurationBag.EngineName);
                var stp = engineHost.Description.Behaviors.Find<ServiceDebugBehavior>();
                stp.HttpHelpPageEnabled = false;
                engineHost.Open();
                Debug.WriteLine("Hub Web API Interface Running.");
            }
            else
            {
                LogEngine.WriteLog(ConfigurationBag.EngineName,
                    "Configuration.WebApiEndPoint key empty, internal Web Api interface disable",
                    Constant.LogLevelError,
                    Constant.TaskCategoriesError,
                    null,
                    Constant.LogLevelWarning);
            }
            InitializeTriggerEngine();
            if (delegateEmbedded != null)
            {
                //
                delegateActionEvent = delegateEmbedded;
            }
            else
            {
                delegateActionEvent = ActionEventReceived;
            }
        }

        //Sort HA List
        public static void HAPointsUpdate()
        {
            while (true)
            {
                byte[] content = UTF8Encoding.UTF8.GetBytes(HAPointTickId.ToString());
                BubblingObject bubblingObject = new BubblingObject(content);
                bubblingObject.MessageType = "HA";
                OffRampEngineSending.SendMessageOffRamp(bubblingObject,
                    "HA",
                    ConfigurationBag.Configuration.ChannelId,
                    ConfigurationBag.PointAll,
                    string.Empty);
                Thread.Sleep(ConfigurationBag.Configuration.HACheckTime);
            }
        }

        public static void HAPointsClean()
        {
            while (true)
            {
                lock (HAPoints)
                {
                    var arrPoints = HAPoints.ToArray();
                    for (int i = 0; i < arrPoints.Length; i++)
                    {
                        var totalSecs = (DateTime.Now - arrPoints[i].Value).TotalMilliseconds;
                        if (totalSecs >= ConfigurationBag.Configuration.HAInactivity)
                        {
                            long a = HAPointTickId;
                            HAPoints.Remove(arrPoints[i].Key);
                        }
                    }
                }
                Thread.Sleep(1000);
            }
        }

        /// <summary>
        ///     Return
        /// </summary>
        /// <returns></returns>
        public static bool SyncAsyncEventsExecuteDelegate(string key, byte[] DataContext)
        {
            try
            {
                if (SyncAsyncEvents.ContainsKey(key))
                {
                    var delegateToExecute = (SyncAsyncEventAction) SyncAsyncEvents[key];

                    delegateToExecute(DataContext);
                    SyncAsyncEventsRemoveDelegate(key);
                }
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

        /// <summary>
        ///     Return
        /// </summary>
        /// <returns></returns>
        public static bool SyncAsyncEventsAddDelegate(string key, SyncAsyncEventAction syncAsyncEventAction)
        {
            try
            {
                SyncAsyncEvents.Add(key, syncAsyncEventAction);
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

        /// <summary>
        ///     Return
        /// </summary>
        /// <returns></returns>
        public static bool SyncAsyncEventsRemoveDelegate(string key)
        {
            try
            {
                if (SyncAsyncEvents.ContainsKey(key))
                {
                    SyncAsyncEvents.Remove(key);
                }
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

        public static void ExecuteSyncAsyncEventAction(byte[] DataContext)
        {
        }

        /// <summary>
        ///     The delegate event executed by a trigger
        /// </summary>
        /// <param name="trigger">
        /// </param>
        /// <param name="context">
        /// </param>
        public static void ActionTriggerReceived(ITriggerType trigger, ActionContext context)
        {
            if (trigger.DataContext == null)
            {
                return;
            }
            try
            {
                LogEngine.WriteLog(ConfigurationBag.EngineName,
                    $"ActionTriggerReceived bubblingObject.SenderChannelId {context.BubblingObjectBag.SenderChannelId} " +
                    $"bubblingObject.SenderPointId {context.BubblingObjectBag.SenderPointId} " +
                    $"bubblingObject.DestinationChannelId {context.BubblingObjectBag.DestinationChannelId} " +
                    $" bubblingObject.DestinationPointId {context.BubblingObjectBag.DestinationPointId} " +
                    $"bubblingObject.MessageType {context.BubblingObjectBag.MessageType}" +
                    $"bubblingObject.Persisting {context.BubblingObjectBag.Persisting} " +
                    $"bubblingObject.MessageId {context.BubblingObjectBag.MessageId} " +
                    $"bubblingObject.Name {context.BubblingObjectBag.Name}" +
                    $"bubblingObject.IdConfiguration {context.BubblingObjectBag.IdConfiguration}" +
                    $"bubblingObject.IdComponent {context.BubblingObjectBag.IdComponent}",
                    Constant.LogLevelError,
                    Constant.TaskCategoriesConsole,
                    null,
                    Constant.LogLevelInformation);

                //If there is a chain to execute then execute the chain component
                if (context.BubblingObjectBag.Chains != null)
                {
                    Byte[] contentFromChain = ExecuteChain(context.BubblingObjectBag.Chains, trigger.DataContext);
                    trigger.DataContext = contentFromChain;
                }

                //CHANGE0110.1706
                //IEnumerable<PropertyInfo> propertyInfos = trigger.GetType().GetProperties().ToList().Where(p => p.GetCustomAttributes(typeof(TriggerPropertyContract), true).Length > 0 && p.Name != "DataContext");

                //foreach (var propertyInfo in propertyInfos)
                //{
                //    context.BubblingObjectBag.Properties[propertyInfo.Name].Value = propertyInfo.GetValue(trigger);
                //}
                Event[] events = context.BubblingObjectBag.Events.ToArray();
                // Events mapping
                foreach (var eventToExecute in events)
                {
                    //Create a clone to copy a separate object in memory

                    //var bubblingEventClone = (BubblingObject)ObjectHelper.CloneObject(context.BubblingObjectBag);
                    context.BubblingObjectBag.DataContext = trigger.DataContext;
                    context.BubblingObjectBag.SenderPointId = ConfigurationBag.Configuration.PointId;
                    context.BubblingObjectBag.SenderChannelId = ConfigurationBag.Configuration.ChannelId;
                    context.BubblingObjectBag.Syncronous = trigger.Syncronous;

                    // if channel null then direct into the queue
                    if (eventToExecute.Channels == null)
                    {
                        context.BubblingObjectBag.LocalEvent = true;
                        OffRampEngineSending.QueueMessage(context.BubblingObjectBag);
                        return;
                    }

                    //var eventsCloned =(Event) ObjectHelper.CloneObject(eventToExecute);
                    context.BubblingObjectBag.Events.Clear();
                    context.BubblingObjectBag.Events.Add(eventToExecute);
                    foreach (var channel in eventToExecute.Channels)
                    {
                        foreach (var point in channel.Points)
                        {
                            OffRampEngineSending.SendMessageOffRamp(context.BubblingObjectBag,
                                "Event",
                                channel.ChannelId,
                                point.PointId,
                                null);
                        }
                    }

                    //todo optimization non credo serva piu, adesso uso l'embedded dall execute, va testato
                    //todo ricodrdati di mettere il ciclo for per canali e punti del canale

                    //if (eventToExecute.IdComponent == GrabCaster.Framework.Base.Constant.EmbeddedEventId)
                    //{
                    //    byte[] message = null;
                    //    message = trigger.DataContext;
                    //    var receiverChannelId = "";
                    //    var receiverPointId = "";


                    //    foreach (var channel in eventToExecute.Channels)
                    //    {
                    //        receiverChannelId += channel.ChannelId + "|";
                    //        receiverPointId = channel.Points.Aggregate(receiverPointId,
                    //            (current, pointId) => current + (channel.ChannelId + "|"));
                    //    }
                    //    //performanceCounters.DoSomeProcessing();
                    //    //For embedded purpose
                    //    OffRampEngineSending.SendMessageOnRamp(
                    //        message,
                    //        ConfigurationBag.ByteArray,
                    //        receiverChannelId,
                    //        receiverPointId,
                    //        null);
                    //}
                    //else
                    //{
                    //    //performanceCounters.DoSomeProcessing();
                    //    //Send event to the MSP
                    //    OffRampEngineSending.SendMessageOnRamp(
                    //        bubblingEventClone,
                    //        ConfigurationBag.Event,
                    //        string.Empty,
                    //        string.Empty,
                    //        null);
                    //}
                }
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

        /// <summary>
        ///     The delegate event executed by a event
        /// </summary>
        /// <param name="eventType">
        /// </param>
        /// <param name="context">
        ///     EventActionContext cosa deve essere esuito
        /// </param>
        public static void ActionEventReceived(IEventType eventType, ActionContext context)
        {
            try
            {
                LogEngine.WriteLog(ConfigurationBag.EngineName,
                    $"ActionEventReceived bubblingObject.SenderChannelId {context.BubblingObjectBag.SenderChannelId} " +
                    $"bubblingObject.SenderPointId {context.BubblingObjectBag.SenderPointId} " +
                    $"bubblingObject.DestinationChannelId {context.BubblingObjectBag.DestinationChannelId} " +
                    $" bubblingObject.DestinationPointId {context.BubblingObjectBag.DestinationPointId} " +
                    $"bubblingObject.MessageType {context.BubblingObjectBag.MessageType}" +
                    $"bubblingObject.Persisting {context.BubblingObjectBag.Persisting} " +
                    $"bubblingObject.MessageId {context.BubblingObjectBag.MessageId} " +
                    $"bubblingObject.Name {context.BubblingObjectBag.Name}" +
                    $"bubblingObject.IdConfiguration {context.BubblingObjectBag.IdConfiguration}" +
                    $"bubblingObject.IdComponent {context.BubblingObjectBag.IdComponent}",
                    Constant.LogLevelError,
                    Constant.TaskCategoriesConsole,
                    null,
                    Constant.LogLevelInformation);
                //if SyncAsync == true 
                //then send the event and keep going to exe
                if (context.BubblingObjectBag.Syncronous)
                {
                    context.BubblingObjectBag.DataContext = eventType.DataContext;
                    context.BubblingObjectBag.Syncronous = false;
                    context.BubblingObjectBag.SyncronousFromEvent = true;

                    //If the event is just locel the engine doesn't send the message to the message provider
                    if (context.BubblingObjectBag.LocalEvent)
                    {
                        SyncAsyncEventsExecuteDelegate(context.BubblingObjectBag.SyncronousToken, eventType.DataContext);
                        context.BubblingObjectBag.SenderPointId = "";
                        context.BubblingObjectBag.SenderChannelId = "";
                        context.BubblingObjectBag.SyncronousToken = String.Empty;
                        return;
                    }
                    OffRampEngineSending.SendMessageOffRamp(context.BubblingObjectBag,
                        "Event",
                        context.BubblingObjectBag.SenderChannelId,
                        context.BubblingObjectBag.SenderPointId,
                        null);
                    return;
                }

                //todo optimization sarebbe carino includere sotto eventi ed eseguirlli qui
                // esempio:
                //se esistion sotto eventi, per ogni canale e punto del canale invia il sotto evento


                //todo optimization controlla la correlazione qua sotto
                // if exist correlation then execute
                if (context.BubblingObjectBag.Correlation == null
                    && context.BubblingObjectBag.CorrelationOverride == null)
                {
                    return;
                }

                // Correlation exist -> execute it
                // execute the rule
                // CSharp using rosling passing datacontext = propertyInfo.GetValue(IEventType) if true then execute

                // Set correlation base
                var correlation = context.BubblingObjectBag.Correlation;

                // Check if overriding required
                if (context.BubblingObjectBag.CorrelationOverride != null)
                {
                    correlation = context.BubblingObjectBag.CorrelationOverride;
                }

                // TODO 1010
                var propDataContext = eventType.DataContext;


                if (correlation != null && ExecuteRoslynRule(propDataContext, correlation.ScriptRule) == false)
                {
                    context.BubblingObjectBag.CorrelationOverride = null;
                    return;
                }

                context.BubblingObjectBag.CorrelationOverride = null;
                if (context.BubblingObjectBag.Correlation != null)
                {
                    Debug.WriteLine(
                        $"-!CORRELATION NAME {context.BubblingObjectBag.Correlation.Name} EVALUATE TRUE!-",
                        ConsoleColor.Green);

                    context.BubblingObjectBag.Events = context.BubblingObjectBag.Correlation.Events;
                }

                // keep easy and discard the correlation, I don't need more
                context.BubblingObjectBag.Correlation = null;
                IEnumerable<PropertyInfo> propertyInfos =
                    eventType.GetType()
                        .GetProperties()
                        .ToList()
                        .Where(
                            p =>
                                p.GetCustomAttributes(typeof(EventPropertyContract), true).Length > 0 &&
                                p.Name != "DataContext");

                foreach (var propertyInfo in propertyInfos)
                {
                    context.BubblingObjectBag.Properties[propertyInfo.Name].Value = propertyInfo.GetValue(eventType);
                }


                foreach (var eventToExecute in context.BubblingObjectBag.Events)
                {
                    //If a local call
                    if (eventToExecute.Channels == null)
                    {
                        // ReSharper disable once UseObjectOrCollectionInitializer
                        var contextLocal = new ActionContext(context.BubblingObjectBag);
                        new Task(() =>
                        {
                            ExecuteEventsInTrigger(contextLocal.BubblingObjectBag, eventToExecute, true,
                                ConfigurationBag.Configuration.ChannelId);
                        }).Start();
                        //ExecuteEventsInTrigger(contextLocal.BubblingObjectBag, eventToExecute, true,
                        //    ConfigurationBag.Configuration.ChannelId);
                    }
                    else
                    {
                        // Send to MSP
                        var remoteContext = new ActionContext(context.BubblingObjectBag);
                        remoteContext.BubblingObjectBag.Events.Clear();
                        remoteContext.BubblingObjectBag.Events.Add(eventToExecute);
                        OffRampEngineSending.SendMessageOffRamp(
                            remoteContext.BubblingObjectBag,
                            "Event",
                            string.Empty,
                            string.Empty,
                            null);
                    }
                }
            }
            catch (Exception ex)
            {
                context.BubblingObjectBag.CorrelationOverride = null;

                LogEngine.WriteLog(
                    ConfigurationBag.EngineName,
                    $"Error in {MethodBase.GetCurrentMethod().Name}",
                    Constant.LogLevelError,
                    Constant.TaskCategoriesError,
                    ex,
                    Constant.LogLevelError);
            }
        }

        /// <summary>
        ///     The execute roslyn rule.
        /// </summary>
        /// <param name="dataContext">
        ///     The data context.
        /// </param>
        /// <param name="script">
        ///     The script.
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        private static bool ExecuteRoslynRule(object dataContext, string script)
        {
            try
            {
                // return true;
                var hostObject = new HostContext
                {
                    DataContext = EncodingDecoding.EncodingBytes2String((byte[]) dataContext)
                };

                var roslynEngine = new ScriptEngine();
                var session = roslynEngine.CreateSession(hostObject);
                session.AddReference(hostObject.GetType().Assembly);
                session.ImportNamespace("System");
                session.ImportNamespace("System.Windows.Forms");
                session.ImportNamespace("Framework");
                session.ImportNamespace("System.Text");

                // TODO 1006
                session.AddReference(
                    @"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\System.Windows.Forms.dll");

                var bval = session.Execute(script);

                return (bool) bval;
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

                return false;
            }
        }


        /// <summary>
        ///     The check for file to update.
        /// </summary>
        public static bool SyncronizePoint()
        {
            try
            {
                string currentSyncFolder = ConfigurationBag.SyncDirectorySyncIn();

                if (!Directory.Exists(currentSyncFolder))
                {
                    LogEngine.DirectEventViewerLog("Nothing to sync.", 4);
                    return true;
                }
                LogEngine.DirectEventViewerLog("Package to sync.", 2);
                //Get the internal root folder because could be different from the default value
                string internalRootFolderTemp = Directory.EnumerateDirectories(currentSyncFolder).First();
                String internalRootFolder =
                    internalRootFolderTemp.Substring(internalRootFolderTemp.LastIndexOf("\\") + 1);

                if (Helpers.ToBeSyncronized(Path.Combine(currentSyncFolder, internalRootFolder),
                    ConfigurationBag.Configuration.DirectoryOperativeRootExeName, true))
                {
                    //Clean sync folder
                    string backupFolder = currentSyncFolder.Replace("\\Sync\\In",
                        $"\\Sync\\{DateTime.Now.ToString("yyyyMMdd_HHmmss")}");
                    Directory.Move(currentSyncFolder, backupFolder);
                    RefreshBubblingSetting(true);
                    return true;
                }
                LogEngine.DirectEventViewerLog(
                    "Point syncronization failed with errors, check the event viewer and the log.", 1);
                return false;
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
                return false;
            }
        }

        /// <summary>
        ///     Create a bubbling event from DLL class
        /// </summary>
        /// <param name="lastAssemblyFileLoaded"></param>
        /// <param name="numOfTriggers"></param>
        /// <param name="assemblyFile"></param>
        /// <returns></returns>
        public static BubblingObject CreateBubblingObject(Type assemblyClass, Assembly assembly, string assemblyFile)
        {
            try
            {
                var bubblingObject = new BubblingObject(null);
                var classAttributes = assemblyClass.GetCustomAttributes(typeof(TriggerContract), true);
                if (classAttributes.Length > 0)
                {
                    var trigger = (TriggerContract) classAttributes[0];


                    ITriggerType triggerType = Activator.CreateInstance(assemblyClass) as ITriggerType;


                    // Create event bubbling
                    bubblingObject.BubblingEventType = BubblingEventType.Trigger;
                    bubblingObject.Description = trigger.Description;
                    bubblingObject.IdComponent = trigger.Id;
                    bubblingObject.Name = trigger.Name;
                    bubblingObject.PollingRequired = trigger.PollingRequired;
                    bubblingObject.Nop = trigger.Nop;
                    bubblingObject.Shared = trigger.Shared;
                    bubblingObject.Version = assembly.GetName().Version;

                    Debug.WriteLine(
                        $"[SYNC ROOT-{Path.GetFileName(assemblyFile)}]-[NAME-{trigger.Name}]",
                        ConsoleColor.Gray);
                }

                bubblingObject.Properties = new Dictionary<string, Property>();
                foreach (var propertyInfo in assemblyClass.GetProperties())
                {
                    var propertyAttributes =
                        propertyInfo.GetCustomAttributes(typeof(TriggerPropertyContract), true);
                    if (propertyAttributes.Length > 0)
                    {
                        var triggerProperty = (TriggerPropertyContract) propertyAttributes[0];

                        // TODO 1004
                        if (propertyInfo.Name != triggerProperty.Name)
                        {
                            throw new Exception(
                                $"Critical error! the properies {propertyAttributes[0]} and {propertyInfo.Name} are different! Class name {assemblyClass.Name}");
                        }

                        bubblingObject.Properties.Add(triggerProperty.Name,
                            new Property(
                                triggerProperty.Name,
                                triggerProperty.Description,
                                propertyInfo,
                                propertyInfo.GetType(),
                                null));
                    }
                }


                // Add the bubbling event
                return bubblingObject;
            }
            catch (Exception ex)
            {
                LogEngine.WriteLog(
                    ConfigurationBag.EngineName,
                    $"Assembly file {assemblyFile} - Error in {MethodBase.GetCurrentMethod().Name}",
                    Constant.LogLevelError,
                    Constant.TaskCategoriesError,
                    ex,
                    Constant.LogLevelError);
                return null;
            }
        }

        /// <summary>
        ///     Create a event bubbling from a event dll class
        /// </summary>
        /// <param name="lastAssemblyFileLoaded"></param>
        /// <param name="numOfEvents"></param>
        /// <param name="assemblyClass"></param>
        /// <param name="assembly"></param>
        /// <param name="assemblyFile"></param>
        /// <returns></returns>
        public static BubblingObject CreateBubblingObjectEvent(Type assemblyClass, Assembly assembly,
            string assemblyFile)
        {
            try
            {
                var bubblingEvent = new BubblingObject(null);
                var classAttributes = assemblyClass.GetCustomAttributes(typeof(EventContract), true);
                if (classAttributes.Length > 0)
                {
                    var classAttribute = (EventContract) classAttributes[0];

                    // Create event bubbling
                    bubblingEvent.BubblingEventType = BubblingEventType.Event;
                    bubblingEvent.Description = classAttribute.Description;
                    bubblingEvent.IdComponent = classAttribute.Id;
                    bubblingEvent.Name = classAttribute.Name;
                    bubblingEvent.PollingRequired = false;
                    bubblingEvent.Nop = false;
                    bubblingEvent.Shared = classAttribute.Shared;
                    bubblingEvent.Version = assembly.GetName().Version;

                    Debug.WriteLine(
                        $"[SYNC ROOT-{Path.GetFileName(assemblyFile)}]-[NAME-{classAttribute.Name}]",
                        ConsoleColor.Gray);
                }

                bubblingEvent.Properties = new Dictionary<string, Property>();
                foreach (var propertyInfo in assemblyClass.GetProperties())
                {
                    var propertyAttributes = propertyInfo.GetCustomAttributes(
                        typeof(EventPropertyContract),
                        true);
                    if (propertyAttributes.Length > 0)
                    {
                        var propertyAttribute = (EventPropertyContract) propertyAttributes[0];
                        if (propertyInfo.Name != propertyAttribute.Name)
                        {
                            throw new Exception(
                                $"Critical error! the properies {propertyAttributes[0]} and {propertyInfo.Name} are different! Class name {assemblyClass.Name}");
                        }

                        bubblingEvent.Properties.Add(propertyAttribute.Name,
                            new Property(
                                propertyAttribute.Name,
                                propertyAttribute.Description,
                                propertyInfo,
                                propertyInfo.GetType(),
                                null));
                    }
                }


                // Add the bubbling event
                return bubblingEvent;
            }
            catch (Exception ex)
            {
                LogEngine.WriteLog(
                    ConfigurationBag.EngineName,
                    $"Assembly file {assemblyFile} - Error in {MethodBase.GetCurrentMethod().Name}",
                    Constant.LogLevelError,
                    Constant.TaskCategoriesError,
                    ex,
                    Constant.LogLevelError);
                return null;
            }
        }

        /// <summary>
        ///     Create a event bubbling from a event dll class
        /// </summary>
        /// <param name="lastAssemblyFileLoaded"></param>
        /// <param name="numOfEvents"></param>
        /// <param name="assemblyClass"></param>
        /// <param name="assembly"></param>
        /// <param name="assemblyFile"></param>
        /// <returns></returns>
        public static BubblingObject CreateBubblingObjectComponent(Type assemblyClass, Assembly assembly,
            string assemblyFile)
        {
            try
            {
                var bubblingEvent = new BubblingObject(null);
                var classAttributes = assemblyClass.GetCustomAttributes(typeof(ComponentContract), true);
                if (classAttributes.Length > 0)
                {
                    var classAttribute = (ComponentContract) classAttributes[0];

                    // Create event bubbling
                    bubblingEvent.BubblingEventType = BubblingEventType.Component;
                    bubblingEvent.Description = classAttribute.Description;
                    bubblingEvent.IdComponent = classAttribute.Id;
                    bubblingEvent.Name = classAttribute.Name;
                    bubblingEvent.PollingRequired = false;
                    bubblingEvent.Nop = false;
                    bubblingEvent.Shared = false;
                    bubblingEvent.Version = assembly.GetName().Version;

                    Debug.WriteLine(
                        $"[SYNC ROOT-{Path.GetFileName(assemblyFile)}]-[NAME-{classAttribute.Name}]",
                        ConsoleColor.Gray);
                }

                bubblingEvent.Properties = new Dictionary<string, Property>();
                foreach (var propertyInfo in assemblyClass.GetProperties())
                {
                    var propertyAttributes = propertyInfo.GetCustomAttributes(
                        typeof(ComponentPropertyContract),
                        true);
                    if (propertyAttributes.Length > 0)
                    {
                        var propertyAttribute = (ComponentPropertyContract) propertyAttributes[0];
                        if (propertyInfo.Name != propertyAttribute.Name)
                        {
                            throw new Exception(
                                $"Critical error! the properies {propertyAttributes[0]} and {propertyInfo.Name} are different! Class name {assemblyClass.Name}");
                        }

                        bubblingEvent.Properties.Add(propertyAttribute.Name,
                            new Property(
                                propertyAttribute.Name,
                                propertyAttribute.Description,
                                propertyInfo,
                                propertyInfo.GetType(),
                                null));
                    }
                }

                // Add the bubbling event
                return bubblingEvent;
            }
            catch (Exception ex)
            {
                LogEngine.WriteLog(
                    ConfigurationBag.EngineName,
                    $"Assembly file {assemblyFile} - Error in {MethodBase.GetCurrentMethod().Name}",
                    Constant.LogLevelError,
                    Constant.TaskCategoriesError,
                    ex,
                    Constant.LogLevelError);
                return null;
            }
        }


        /// <summary>
        ///     Load the events list from the directory
        /// </summary>
        /// <param name="numOfTriggers">
        ///     The num Of Triggers.
        /// </param>
        /// <param name="numOfEvents">
        ///     The num Of Events.
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public static bool LoadAssemblyComponents(ref int numOfTriggers, ref int numOfEvents, ref int numOfComponents)
        {
            //todo optimization 
            // Load all Triggers and Events DLLs in the main dictionary

            CacheTriggerComponents = new Dictionary<string, ITriggerAssembly>();
            CacheTriggerRunning = new Dictionary<string, ITriggerType>();
            CacheEventComponents = new Dictionary<string, IEventAssembly>();
            CacheChainComponents = new Dictionary<string, IChainComponentAssembly>();
            CacheEngineComponents = new Dictionary<string, Assembly>();


            numOfTriggers = 0;
            numOfEvents = 0;
            numOfComponents = 0;


            var lastAssemblyFileLoaded = string.Empty;


            // Load triggers bubbling path
            var triggersDirectory = ConfigurationBag.DirectoryTriggers();
            var regTriggers = new Regex(ConfigurationBag.TriggersDllExtension);
            var assemblyFilesTriggers =
                Directory.GetFiles(triggersDirectory, ConfigurationBag.TriggersDllExtensionLookFor)
                    .Where(path => regTriggers.IsMatch(path))
                    .ToList();

            // Load event bubbling path
            var eventsDirectory = ConfigurationBag.DirectoryEvents();
            var regEvents = new Regex(ConfigurationBag.EventsDllExtension);
            var assemblyFilesEvents =
                Directory.GetFiles(eventsDirectory, ConfigurationBag.EventsDllExtensionLookFor)
                    .Where(path => regEvents.IsMatch(path))
                    .ToList();

            // Load event bubbling path
            var componentsDirectory = ConfigurationBag.DirectoryComponents();
            var regComponents = new Regex(ConfigurationBag.ComponentsDllExtension);
            var assemblyFilesComponents =
                Directory.GetFiles(componentsDirectory, ConfigurationBag.ComponentsDllExtensionLookFor)
                    .Where(path => regComponents.IsMatch(path))
                    .ToList();

            try
            {
                // ****************************************************
                // Load Triggers
                // ****************************************************
                foreach (var assemblyFile in assemblyFilesTriggers)
                {
                    try
                    {
                        lastAssemblyFileLoaded = assemblyFile;

                        // TODO 1005
                        try
                        {
                            // TEST SYNC 
                            if (Path.GetFileName(assemblyFile) == "Framework.Contracts.dll")
                            {
                                continue;
                            }

                            var fileName = Path.GetFileName(assemblyFile);
                            if (fileName != null && fileName.Substring(0, 10) == "Microsoft.")
                            {
                                continue;
                            }

                            if (Path.GetFileName(assemblyFile).Substring(0, 7) == "System.")
                            {
                                continue;
                            }
                        }
                        catch
                        {
                            // ignored
                        }

                        // Get all classes with Attribute = Event
                        var assembly = Assembly.LoadFrom(assemblyFile);

                        //Add to all components dictionary
                        CacheEngineComponents.Add(assembly.FullName, assembly);


                        var assemblyClasses = from t in assembly.GetTypes()
                            let attributes = t.GetCustomAttributes(typeof(TriggerContract), false)
                            where t.IsClass && attributes != null && attributes.Length > 0
                            select t;
                        //For each class in the assembly will instance a new activator
                        foreach (var assemblyClass in assemblyClasses)
                        {
                            ++numOfTriggers;
                            var classAttributes = assemblyClass.GetCustomAttributes(typeof(TriggerContract), true);
                            if (classAttributes.Length > 0)
                            {
                                var trigger = (TriggerContract) classAttributes[0];
                                ITriggerAssembly triggerAssembly = new TriggerAssembly();
                                triggerAssembly.TriggerType = Activator.CreateInstance(assemblyClass) as ITriggerType;
                                ;
                                triggerAssembly.AssemblyClassType = assemblyClass;
                                triggerAssembly.AssemblyContent = File.ReadAllBytes(assemblyFile);
                                triggerAssembly.AssemblyFile = assemblyFile;
                                triggerAssembly.AssemblyObject = assembly;
                                triggerAssembly.BaseActions = null;
                                triggerAssembly.Version = assembly.GetName().Version;

                                triggerAssembly.Properties = new Dictionary<string, Property>();
                                foreach (var propertyInfo in assemblyClass.GetProperties())
                                {
                                    var propertyAttributes =
                                        propertyInfo.GetCustomAttributes(typeof(TriggerPropertyContract), true);
                                    if (propertyAttributes.Length > 0)
                                    {
                                        var propertyAttribute = (TriggerPropertyContract) propertyAttributes[0];

                                        // TODO 1004
                                        if (propertyInfo.Name != propertyAttribute.Name)
                                        {
                                            throw new Exception(
                                                $"Critical error! the properies {propertyAttributes[0]} and {propertyInfo.Name} are different! Class name {assemblyClass.Name}");
                                        }
                                        triggerAssembly.Properties.Add(propertyAttribute.Name,
                                            new Property(
                                                propertyAttribute.Name,
                                                propertyAttribute.Description,
                                                propertyInfo,
                                                propertyInfo.GetType(),
                                                null));
                                    }
                                }
                                CacheTriggerComponents.Add(trigger.Id, triggerAssembly);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        LogEngine.WriteLog(
                            ConfigurationBag.EngineName,
                            $"Assembly file {lastAssemblyFileLoaded} - Error in {MethodBase.GetCurrentMethod().Name}",
                            Constant.LogLevelError,
                            Constant.TaskCategoriesError,
                            ex,
                            Constant.LogLevelError);
                        return false;
                    }
                }

                // ****************************************************
                // Load Events
                // ****************************************************
                foreach (var assemblyFile in assemblyFilesEvents)
                {
                    try
                    {
                        // TODO 1003
                        lastAssemblyFileLoaded = assemblyFile;

                        // TEST SYNC 
                        if (Path.GetFileName(assemblyFile) == "Framework.Contracts.dll")
                        {
                            continue;
                        }

                        if (Path.GetFileName(assemblyFile).Substring(0, 10) == "Microsoft.")
                        {
                            continue;
                        }

                        if (Path.GetFileName(assemblyFile).Substring(0, 7) == "System.")
                        {
                            continue;
                        }

                        // Get all classes with Attribute = Event
                        var assembly = Assembly.LoadFrom(assemblyFile);
                        //Add to all components dictionary
                        CacheEngineComponents.Add(assembly.FullName, assembly);

                        var assemblyClasses = from t in assembly.GetTypes()
                            let attributes = t.GetCustomAttributes(typeof(EventContract), false)
                            where t.IsClass && attributes != null && attributes.Length > 0
                            select t;

                        foreach (var assemblyClass in assemblyClasses)
                        {
                            ++numOfEvents;
                            var classAttributes = assemblyClass.GetCustomAttributes(typeof(EventContract), true);
                            if (classAttributes.Length > 0)
                            {
                                var eventContract = (EventContract) classAttributes[0];
                                IEventAssembly eventAssembly = new EventAssembly();
                                eventAssembly.EventType = Activator.CreateInstance(assemblyClass) as IEventType;
                                ;
                                eventAssembly.AssemblyClassType = assemblyClass;
                                eventAssembly.AssemblyContent = File.ReadAllBytes(assemblyFile);
                                eventAssembly.AssemblyFile = assemblyFile;
                                eventAssembly.AssemblyObject = assembly;
                                eventAssembly.BaseActions = null;
                                eventAssembly.Version = assembly.GetName().Version;

                                eventAssembly.Properties = new Dictionary<string, Property>();
                                foreach (var propertyInfo in assemblyClass.GetProperties())
                                {
                                    var propertyAttributes =
                                        propertyInfo.GetCustomAttributes(typeof(EventPropertyContract), true);
                                    if (propertyAttributes.Length > 0)
                                    {
                                        var propertyAttribute = (EventPropertyContract) propertyAttributes[0];

                                        // TODO 1004
                                        if (propertyInfo.Name != propertyAttribute.Name)
                                        {
                                            throw new Exception(
                                                $"Critical error! the properies {propertyAttributes[0]} and {propertyInfo.Name} are different! Class name {assemblyClass.Name}");
                                        }
                                        eventAssembly.Properties.Add(propertyAttribute.Name,
                                            new Property(
                                                propertyAttribute.Name,
                                                propertyAttribute.Description,
                                                propertyInfo,
                                                propertyInfo.GetType(),
                                                null));
                                    }
                                }
                                CacheEventComponents.Add(eventContract.Id, eventAssembly);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        LogEngine.WriteLog(
                            ConfigurationBag.EngineName,
                            $"Assembly file {lastAssemblyFileLoaded} - Error in {MethodBase.GetCurrentMethod().Name} - Possible workaround: Check if the propeties name and the corresponding contract properties name are the same in the assembly.",
                            Constant.LogLevelError,
                            Constant.TaskCategoriesError,
                            ex,
                            Constant.LogLevelError);
                        return false;
                    }
                }


                // ****************************************************
                // Load Components
                // ****************************************************
                foreach (var assemblyFile in assemblyFilesComponents)
                {
                    try
                    {
                        // TODO 1003
                        lastAssemblyFileLoaded = assemblyFile;

                        // TEST SYNC 
                        if (Path.GetFileName(assemblyFile) == "Framework.Contracts.dll")
                        {
                            continue;
                        }

                        if (Path.GetFileName(assemblyFile).Substring(0, 10) == "Microsoft.")
                        {
                            continue;
                        }

                        if (Path.GetFileName(assemblyFile).Substring(0, 7) == "System.")
                        {
                            continue;
                        }

                        // Get all classes with Attribute = Event
                        var assembly = Assembly.LoadFrom(assemblyFile);

                        //Add to all components dictionary
                        CacheEngineComponents.Add(assembly.FullName, assembly);

                        var assemblyClasses = from t in assembly.GetTypes()
                            let attributes = t.GetCustomAttributes(typeof(ComponentContract), false)
                            where t.IsClass && attributes != null && attributes.Length > 0
                            select t;

                        foreach (var assemblyClass in assemblyClasses)
                        {
                            ++numOfComponents;
                            var classAttributes = assemblyClass.GetCustomAttributes(typeof(ComponentContract), true);
                            if (classAttributes.Length > 0)
                            {
                                var trigger = (ComponentContract) classAttributes[0];
                                IChainComponentAssembly chainComponentAssembly = new ChainComponentAssembly();
                                chainComponentAssembly.ChainComponentType =
                                    Activator.CreateInstance(assemblyClass) as IChainComponentType;
                                chainComponentAssembly.AssemblyClassType = assemblyClass;
                                chainComponentAssembly.AssemblyContent = File.ReadAllBytes(assemblyFile);
                                chainComponentAssembly.AssemblyFile = assemblyFile;
                                chainComponentAssembly.AssemblyObject = assembly;
                                chainComponentAssembly.BaseActions = null;
                                chainComponentAssembly.Version = assembly.GetName().Version;

                                chainComponentAssembly.Properties = new Dictionary<string, Property>();
                                foreach (var propertyInfo in assemblyClass.GetProperties())
                                {
                                    var propertyAttributes =
                                        propertyInfo.GetCustomAttributes(typeof(ComponentPropertyContract), true);
                                    if (propertyAttributes.Length > 0)
                                    {
                                        var propertyAttribute = (ComponentPropertyContract) propertyAttributes[0];

                                        // TODO 1004
                                        if (propertyInfo.Name != propertyAttribute.Name)
                                        {
                                            throw new Exception(
                                                $"Critical error! the properies {propertyAttributes[0]} and {propertyInfo.Name} are different! Class name {assemblyClass.Name}");
                                        }
                                        chainComponentAssembly.Properties.Add(propertyAttribute.Name,
                                            new Property(
                                                propertyAttribute.Name,
                                                propertyAttribute.Description,
                                                propertyInfo,
                                                propertyInfo.GetType(),
                                                null));
                                    }
                                }
                                CacheChainComponents.Add(trigger.Id, chainComponentAssembly);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        LogEngine.WriteLog(
                            ConfigurationBag.EngineName,
                            $"Assembly file {lastAssemblyFileLoaded} - Error in {MethodBase.GetCurrentMethod().Name} - Possible workaround: Check if the propeties name and the corresponding contract properties name are the same in the assembly.",
                            Constant.LogLevelError,
                            Constant.TaskCategoriesError,
                            ex,
                            Constant.LogLevelError);
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                LogEngine.WriteLog(
                    ConfigurationBag.EngineName,
                    $"Assembly file {lastAssemblyFileLoaded} - Error in {MethodBase.GetCurrentMethod().Name}",
                    Constant.LogLevelError,
                    Constant.TaskCategoriesError,
                    ex,
                    Constant.LogLevelError);
                return false;
            }
        }

        /// <summary>
        ///     Create all bubbling object for trigger event and components
        ///     to be used by the engine
        /// </summary>
        public static void RefreshBubblingSetting(bool RefreshTriggersRunning)
        {
            //Instantiate vars
            try
            {
                LogEngine.DirectEventViewerLog("Update Point settings...", 4);

                SyncronizationConfigurationFileList = new List<SyncConfigurationFile>();
                ConfigurationJsonTriggerFileList = new List<TriggerConfiguration>();
                ConfigurationJsonEventFileList = new Dictionary<string, EventConfiguration>();
                ConfigurationJsonChainFileList = new List<ChainConfiguration>();
                ConfigurationJsonComponentList = new List<ComponentConfiguration>();

                BubblingTriggersEventsActive = new List<BubblingObject>();
                BubblingTriggerConfigurationsPolling = new List<BubblingObject>();
                BubblingTriggerConfigurationsSingleInstance = new List<BubblingObject>();

                Debug.WriteLine("Load configuration triggers and events.", ConsoleColor.Green);
                CollectBubblingConfigurationFiles();

                // LOAD TRIGGERS

                var triggerBubblingDirectory = ConfigurationBag.DirectoryBubblingTriggers();
                var regTriggers = new Regex(ConfigurationBag.BubblingTriggersExtension);
                //todo optimization
                var triggerConfigurationsFiles =
                    Directory.GetFiles(triggerBubblingDirectory, "*", SearchOption.AllDirectories)
                        .Where(path => regTriggers.IsMatch(path))
                        .ToList();

                // For each trigger search for the trigger in event bubbling and set the properties
                foreach (var triggerConfigurationsFile in triggerConfigurationsFiles)
                {
                    TriggerConfiguration triggerConfiguration = null;

                    var fileLocked = true;
                    while (fileLocked)
                    {
                        try
                        {
                            // TODO 10001.execute
                            triggerConfiguration =
                                JsonConvert.DeserializeObject<TriggerConfiguration>(
                                    File.ReadAllText(triggerConfigurationsFile));
                            fileLocked = false;


                            //Check if can be executed by this point
                            //If channels node doesn't exist then execute anyway
                            //If channel node exist check if this point needs to load this configuration and execute the trigger
                            //all point contains the configuration off all the nodes in the same message provider
                            bool configurationToUse = false;
                            if (triggerConfiguration.Trigger.Channels != null)
                            {
                                //Check if trigger is for this point
                                foreach (var channel in triggerConfiguration.Trigger.Channels)
                                {
                                    foreach (var point in channel.Points)
                                    {
                                        var channelIdToCheck = channel.ChannelId;
                                        var pointIdToCheck = point.PointId;

                                        var toDo = (channelIdToCheck == ConfigurationBag.Configuration.ChannelId
                                                    && pointIdToCheck == ConfigurationBag.Configuration.PointId)
                                                   || (channelIdToCheck == ConfigurationBag.ChannelAll
                                                       && pointIdToCheck == ConfigurationBag.Configuration.PointId)
                                                   || (channelIdToCheck == ConfigurationBag.Configuration.ChannelId
                                                       && pointIdToCheck == ConfigurationBag.PointAll)
                                                   || (channelIdToCheck == ConfigurationBag.ChannelAll
                                                       && pointIdToCheck == ConfigurationBag.PointAll);
                                        if (toDo)
                                        {
                                            configurationToUse = true;
                                            break;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                configurationToUse = true;
                            }

                            // Add to the global list
                            if (configurationToUse)
                            {
                                ConfigurationJsonTriggerFileList.Add(triggerConfiguration);
                            }
                        }
                        catch (IOException ioex)
                        {
                            Debug.WriteLine("<Lock>" + ioex.Message);
                            Debug.WriteLine("<Lock>");
                            fileLocked = true;
                        }
                        catch (Exception ex)
                        {
                            LogEngine.WriteLog(
                                ConfigurationBag.EngineName,
                                $"Error in {MethodBase.GetCurrentMethod().Name} File {triggerConfigurationsFile}",
                                Constant.LogLevelError,
                                Constant.TaskCategoriesError,
                                ex,
                                Constant.LogLevelError);
                            fileLocked = false;
                        }
                    }

                    //todo optimization - commentato la clonazione, devo controllare possibili errori
                    // Get the bubbling object for the specific trigger
                    //Check if the dll is deployed with the appropiate idComponent
                    ITriggerAssembly triggerAssembly;
                    CacheTriggerComponents.TryGetValue(triggerConfiguration.Trigger.IdComponent, out triggerAssembly);


                    //var bubblingTrigger =
                    //    GlobalAssemblyFiles.Find(
                    //        property => property.IdComponent == triggerConfiguration.Trigger.IdComponent);
                    // Assembly founded
                    if (triggerAssembly != null)
                    {
                        //Serialize clone to be abstracted in memory
                        //BubblingObject bubblingTriggerClone = (BubblingObject) ObjectHelper.CloneObject(bubblingTrigger);
                        //set the the 
                        BubblingObject bubblingOTriggerClone = new BubblingObject(null);
                        bubblingOTriggerClone.IsActive = true;

                        bubblingOTriggerClone.Syncronous =
                            bool.Parse(
                                triggerConfiguration.Trigger.TriggerProperties.Find(p => p.Name == "Syncronous")
                                    .Value.ToString());
                        bubblingOTriggerClone.IdComponent = triggerConfiguration.Trigger.IdComponent;
                        bubblingOTriggerClone.IdConfiguration = triggerConfiguration.Trigger.IdConfiguration;
                        bubblingOTriggerClone.Chains = triggerConfiguration.Trigger.Chains;
                        bubblingOTriggerClone.Events = triggerConfiguration.Events;
                        bubblingOTriggerClone.BubblingEventType = BubblingEventType.Trigger;

                        //Set contract attributes
                        var classAttributes =
                            triggerAssembly.TriggerType.GetType().GetCustomAttributes(typeof(TriggerContract), true);
                        if (classAttributes.Length > 0)
                        {
                            var trigger = (TriggerContract) classAttributes[0];
                            // Create event bubbling
                            bubblingOTriggerClone.Description = trigger.Description;
                            bubblingOTriggerClone.IdComponent = trigger.Id;
                            bubblingOTriggerClone.Name = trigger.Name;
                            bubblingOTriggerClone.PollingRequired = trigger.PollingRequired;
                            bubblingOTriggerClone.Nop = trigger.Nop;
                            bubblingOTriggerClone.Shared = trigger.Shared;
                            bubblingOTriggerClone.Version = triggerAssembly.Version;
                        }

                        // Copy all the properties from configuration to assembly
                        IEnumerable<PropertyInfo> propertyInfos =
                            triggerAssembly.TriggerType.GetType()
                                .GetProperties()
                                .ToList()
                                .Where(
                                    p =>
                                        p.GetCustomAttributes(typeof(TriggerPropertyContract), true).Length > 0 &&
                                        p.Name != "DataContext");

                        foreach (var propertyInfo in propertyInfos)
                        {
                            TriggerProperty triggerProperty =
                                triggerConfiguration.Trigger.TriggerProperties.First(p => p.Name == propertyInfo.Name);
                            //propertyInfo.SetValue(triggerAssembly.TriggerType,
                            //    Convert.ChangeType(triggerProperty.Value, propertyInfo.PropertyType),
                            //    null);
                            bubblingOTriggerClone.Properties.Add(triggerProperty.Name,
                                new Property(triggerProperty.Name, triggerProperty.Name, propertyInfo,
                                    propertyInfo.PropertyType, triggerProperty.Value));


                        }

                        PropertyInfo propertyInfosDataContext =
                            triggerAssembly.TriggerType.GetType().GetProperties().First(p => p.Name == "DataContext");
                        bubblingOTriggerClone.Properties.Add(propertyInfosDataContext.Name,
                            new Property(propertyInfosDataContext.Name, propertyInfosDataContext.Name,
                                propertyInfosDataContext, propertyInfosDataContext.PropertyType, null));

                        if (RefreshTriggersRunning)
                        {
                            //Refresh properties trigger running
                            ITriggerType triggerTypeRunning;
                            CacheTriggerRunning.TryGetValue(triggerConfiguration.Trigger.IdConfiguration + triggerConfiguration.Trigger.IdComponent, out triggerTypeRunning);

                            IEnumerable<PropertyInfo> propertyInfosRunning =
                                triggerTypeRunning.GetType()
                                    .GetProperties()
                                    .ToList()
                                    .Where(
                                        p =>
                                            p.GetCustomAttributes(typeof(TriggerPropertyContract), true).Length > 0 &&
                                            p.Name != "DataContext");

                            foreach (var propertyInfo in propertyInfosRunning)
                            {
                                TriggerProperty triggerProperty =
                                    triggerConfiguration.Trigger.TriggerProperties.First(p => p.Name == propertyInfo.Name);

                                propertyInfo.SetValue(triggerTypeRunning,
                                    Convert.ChangeType(triggerProperty.Value, propertyInfo.PropertyType),
                                    null);
                            }
                        }
                     


                        // Add in the list
                        if (bubblingOTriggerClone.PollingRequired)
                        {
                            BubblingTriggerConfigurationsPolling.Add(bubblingOTriggerClone);
                            Debug.WriteLine(
                                $"Load configuration trigger file - {bubblingOTriggerClone.Name}",
                                ConsoleColor.Green);
                        }
                        else
                        {
                            BubblingTriggerConfigurationsSingleInstance.Add(bubblingOTriggerClone);
                            Debug.WriteLine(
                                $"Load configuration trigger file- {bubblingOTriggerClone.Name}",
                                ConsoleColor.Green);
                        }
                        BubblingTriggersEventsActive.Add(bubblingOTriggerClone);
                    }
                    else
                    {
                        LogEngine.WriteLog(
                            ConfigurationBag.EngineName,
                            $"Warning in {MethodBase.GetCurrentMethod().Name},Trigger [{triggerConfiguration.Trigger.Name}] with  IDComponent {triggerConfiguration.Trigger.IdComponent} present in the configuration event directory and not found in the event dll directory.",
                            Constant.LogLevelError,
                            Constant.TaskCategoriesError,
                            null,
                            Constant.LogLevelError);
                    }
                }

                // EVENTS******************************************************************************
                // Loop in the directory
                var eventsBubblingDirectory = ConfigurationBag.DirectoryBubblingEvents();
                var regEvents = new Regex(ConfigurationBag.BubblingEventsExtension);
                var propertyEventsFiles =
                    Directory.GetFiles(eventsBubblingDirectory, "*", SearchOption.AllDirectories)
                        .Where(path => regEvents.IsMatch(path))
                        .ToList();

                foreach (var propertyEventsFile in propertyEventsFiles)
                {
                    EventConfiguration eventPropertyBag = null;
                    var fileLocked = true;
                    while (fileLocked)
                    {
                        try
                        {
                            var propertyEventsByteContent = File.ReadAllBytes(propertyEventsFile);
                            fileLocked = false;
                            eventPropertyBag =
                                JsonConvert.DeserializeObject<EventConfiguration>(
                                    EncodingDecoding.EncodingBytes2String(propertyEventsByteContent));

                            // Add to the global list
                            string key = eventPropertyBag.Event.IdConfiguration + eventPropertyBag.Event.IdComponent;
                            eventPropertyBag.Event.CacheEventProperties = new Dictionary<string, EventProperty>();
                            //Fill the cached properties
                            if (eventPropertyBag.Event.EventProperties != null)
                            {
                                foreach (var property in eventPropertyBag.Event.EventProperties)
                                {
                                    eventPropertyBag.Event.CacheEventProperties.Add(property.Name, property);
                                }
                            }
                            ConfigurationJsonEventFileList.Add(key, eventPropertyBag);
                        }
                        catch (IOException ioex)
                        {
                            Debug.WriteLine("<Lock>" + ioex.Message);
                            Debug.WriteLine("<Lock>");
                            fileLocked = true;
                        }
                        catch (Exception ex)
                        {
                            LogEngine.WriteLog(
                                ConfigurationBag.EngineName,
                                $"Error in {MethodBase.GetCurrentMethod().Name} File {propertyEventsFile}",
                                Constant.LogLevelError,
                                Constant.TaskCategoriesError,
                                ex,
                                Constant.LogLevelError);
                            fileLocked = false;
                        }
                    }

                    //var bubblingEvent =
                    //    GlobalAssemblyFiles.Find(property => property.IdComponent == eventPropertyBag.Event.IdComponent);

                    IEventAssembly eventAssembly;
                    CacheEventComponents.TryGetValue(eventPropertyBag.Event.IdComponent, out eventAssembly);

                    // Dll founded
                    if (eventAssembly != null)
                    {
                        //var bubblingEventClone = (BubblingObject) ObjectHelper.CloneObject(bubblingEvent);

                        BubblingObject bubblingObjectEvent = new BubblingObject(null);

                        bubblingObjectEvent.IdComponent = eventPropertyBag.Event.IdComponent;
                        bubblingObjectEvent.IdConfiguration = eventPropertyBag.Event.IdConfiguration;
                        bubblingObjectEvent.Chains = eventPropertyBag.Event.Chains;
                        bubblingObjectEvent.IsActive = true;
                        bubblingObjectEvent.Events = null;
                        //todo optimization per quale ragione qui alvavo gli eventi dal bubbling? quando l'evento non ha sotto eventi?
                        bubblingObjectEvent.BubblingEventType = BubblingEventType.Event;
                        // Yes, so set all the properties

                        IEnumerable<PropertyInfo> propertyInfos =
                            eventAssembly.EventType.GetType()
                                .GetProperties()
                                .ToList()
                                .Where(
                                    p =>
                                        p.GetCustomAttributes(typeof(EventPropertyContract), true).Length > 0 &&
                                        p.Name != "DataContext");
                        foreach (var propertyInfo in propertyInfos)
                        {
                            EventProperty eventProperty =
                                eventPropertyBag.Event.EventProperties.First(p => p.Name == propertyInfo.Name);
                            propertyInfo.SetValue(eventAssembly.EventType,
                                Convert.ChangeType(eventProperty.Value, propertyInfo.PropertyType),
                                null);
                            bubblingObjectEvent.Properties.Add(eventProperty.Name,
                                new Property(eventProperty.Name, eventProperty.Name, propertyInfo,
                                    propertyInfo.PropertyType, eventProperty.Value));
                        }

                        PropertyInfo propertyInfosDataContext =
                            eventAssembly.EventType.GetType().GetProperties().First(p => p.Name == "DataContext");
                        bubblingObjectEvent.Properties.Add(propertyInfosDataContext.Name,
                            new Property(propertyInfosDataContext.Name, propertyInfosDataContext.Name,
                                propertyInfosDataContext, propertyInfosDataContext.PropertyType, null));

                        // Add in the list
                        BubblingTriggersEventsActive.Add(bubblingObjectEvent);
                        Debug.WriteLine(
                            $"Load configuration Event file- {bubblingObjectEvent.Name}",
                            ConsoleColor.Green);
                    }
                    else
                    {
                        LogEngine.WriteLog(
                            ConfigurationBag.EngineName,
                            string.Format(
                                "Warning in {0}, Event [{2}] with IDComponent {1} present in the configuration event directory and not found in the event dll directory.",
                                MethodBase.GetCurrentMethod().Name,
                                eventPropertyBag.Event.IdComponent,
                                eventPropertyBag.Event.Name),
                            Constant.LogLevelError,
                            Constant.TaskCategoriesError,
                            null,
                            Constant.LogLevelWarning);
                    }
                }

                // Chains******************************************************************************
                // Loop in the directory
                var chainsBubblingDirectory = ConfigurationBag.DirectoryBubblingChains();
                var regChains = new Regex(ConfigurationBag.BubblingChainsExtension);
                var propertyChainsFiles =
                    Directory.GetFiles(chainsBubblingDirectory, "*", SearchOption.AllDirectories)
                        .Where(path => regChains.IsMatch(path))
                        .ToList();

                // For each trigger search for the chain in the bubbling and set the properties
                foreach (var propertyChainsFile in propertyChainsFiles)
                {
                    ChainConfiguration chainPropertyBag = null;
                    var fileLocked = true;
                    while (fileLocked)
                    {
                        try
                        {
                            var propertyChainsByteContent = File.ReadAllBytes(propertyChainsFile);
                            fileLocked = false;
                            chainPropertyBag =
                                JsonConvert.DeserializeObject<ChainConfiguration>(
                                    EncodingDecoding.EncodingBytes2String(propertyChainsByteContent));

                            // Add to the global list
                            ConfigurationJsonChainFileList.Add(chainPropertyBag);
                        }
                        catch (IOException ioex)
                        {
                            Debug.WriteLine("<Lock>" + ioex.Message);
                            Debug.WriteLine("<Lock>");
                            fileLocked = true;
                        }
                        catch (Exception ex)
                        {
                            LogEngine.WriteLog(
                                ConfigurationBag.EngineName,
                                $"Error in {MethodBase.GetCurrentMethod().Name} File {propertyChainsFile}",
                                Constant.LogLevelError,
                                Constant.TaskCategoriesError,
                                ex,
                                Constant.LogLevelError);
                            fileLocked = false;
                        }
                    }
                }


                // Components******************************************************************************
                // Loop in the directory
                var componentsBubblingDirectory = ConfigurationBag.DirectoryBubblingComponents();
                var regComponents = new Regex(ConfigurationBag.BubblingComponentsExtension);
                var propertyComponentsFiles =
                    Directory.GetFiles(componentsBubblingDirectory, "*", SearchOption.AllDirectories)
                        .Where(path => regComponents.IsMatch(path))
                        .ToList();

                // For each trigger search for the trigger in event bubbling and set the properties
                foreach (var propertyComponentsFile in propertyComponentsFiles)
                {
                    ComponentConfiguration componentPropertyBag = null;
                    var fileLocked = true;
                    while (fileLocked)
                    {
                        try
                        {
                            var propertyComponentsByteContent = File.ReadAllBytes(propertyComponentsFile);
                            fileLocked = false;
                            componentPropertyBag =
                                JsonConvert.DeserializeObject<ComponentConfiguration>(
                                    EncodingDecoding.EncodingBytes2String(propertyComponentsByteContent));

                            // Add to the global list
                            ConfigurationJsonComponentList.Add(componentPropertyBag);
                        }
                        catch (IOException ioex)
                        {
                            Debug.WriteLine("<Lock>" + ioex.Message);
                            Debug.WriteLine("<Lock>");
                            fileLocked = true;
                        }
                        catch (Exception ex)
                        {
                            LogEngine.WriteLog(
                                ConfigurationBag.EngineName,
                                $"Error in {MethodBase.GetCurrentMethod().Name} File {propertyComponentsFile}",
                                Constant.LogLevelError,
                                Constant.TaskCategoriesError,
                                ex,
                                Constant.LogLevelError);
                            fileLocked = false;
                        }
                    }


                    //var bubblingEvent =
                    //    GlobalAssemblyFiles.Find(
                    //        property => property.IdComponent == componentPropertyBag.Component.IdComponent);

                    IChainComponentAssembly chainComponentAssembly;
                    CacheChainComponents.TryGetValue(componentPropertyBag.Component.IdComponent,
                        out chainComponentAssembly);

                    // Dll founded
                    if (chainComponentAssembly != null)
                    {
                        //var bubblingEventClone = (BubblingEvent)ObjectHelper.CloneObject(bubblingEvent);
                        BubblingObject bubblingObjectComponent = new BubblingObject(null);
                        bubblingObjectComponent.IdComponent = componentPropertyBag.Component.IdComponent;

                        IEnumerable<PropertyInfo> propertyInfos =
                            chainComponentAssembly.ChainComponentType.GetType()
                                .GetProperties()
                                .ToList()
                                .Where(
                                    p =>
                                        p.GetCustomAttributes(typeof(ComponentPropertyContract), true).Length > 0 &&
                                        p.Name != "DataContext");
                        foreach (var propertyInfo in propertyInfos)
                        {
                            ComponentProperty componentProperty =
                                componentPropertyBag.Component.ComponentProperties.First(
                                    p => p.Name == propertyInfo.Name);
                            propertyInfo.SetValue(chainComponentAssembly.ChainComponentType,
                                Convert.ChangeType(componentProperty.Value, propertyInfo.PropertyType),
                                null);
                            bubblingObjectComponent.Properties.Add(componentProperty.Name,
                                new Property(componentProperty.Name, componentProperty.Name, propertyInfo,
                                    propertyInfo.PropertyType, componentProperty.Value));
                        }

                        PropertyInfo propertyInfosDataContext =
                            chainComponentAssembly.ChainComponentType.GetType()
                                .GetProperties()
                                .First(p => p.Name == "DataContext");
                        bubblingObjectComponent.Properties.Add(propertyInfosDataContext.Name,
                            new Property(propertyInfosDataContext.Name, propertyInfosDataContext.Name,
                                propertyInfosDataContext, propertyInfosDataContext.PropertyType, null));


                        Debug.WriteLine(
                            $"Load chain component file- {bubblingObjectComponent.Name}",
                            ConsoleColor.Green);
                    }
                    else
                    {
                        LogEngine.WriteLog(
                            ConfigurationBag.EngineName,
                            string.Format(
                                "Warning in {0}, Event [{2}] with IDComponent {1} present in the configuration event directory and not found in the event dll directory.",
                                MethodBase.GetCurrentMethod().Name,
                                componentPropertyBag.Component.IdComponent,
                                componentPropertyBag.Component.Name),
                            Constant.LogLevelError,
                            Constant.TaskCategoriesError,
                            null,
                            Constant.LogLevelWarning);
                    }
                }


                Debug.WriteLine($"Preparing the Bubbling Data Syncronization Bag.", ConsoleColor.Green);
                //Set up the bubblingBag for console
                bubblingBagObject = new BubblingBagObjet();
                bubblingBagObject.TriggerConfigurationList = ConfigurationJsonTriggerFileList;
                bubblingBagObject.EventConfigurationList = ConfigurationJsonEventFileList;
                bubblingBagObject.ComponentConfigurationList = ConfigurationJsonComponentList;
                bubblingBagObject.ChainConfigurationList = ConfigurationJsonChainFileList;
                bubblingBagObject.Configuration = ConfigurationBag.Configuration;

                //Get bubbling folder
                bubblingBag = new BubblingBag();
                string gcRootConfiguration = ConfigurationBag.Configuration.DirectoryOperativeRootExeName;
                bubblingBag.contentBubblingFolder =
                    CompressionLibrary.Helpers.CreateFromDirectory(gcRootConfiguration);
                Debug.WriteLine($"Bubbling Data Syncronization Bag prepared.", ConsoleColor.Green);
                LogEngine.DirectEventViewerLog("Point settings updated.", 4);
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

        /// <summary>
        ///     Load all trigger and events from Bubbling trigger directory
        /// </summary>
        public static void CollectBubblingConfigurationFiles()
        {
            // Create the instance lists
            SyncronizationConfigurationFileList = new List<SyncConfigurationFile>();
            Debug.WriteLine("Load sync configuration triggers and events.", ConsoleColor.Green);

            // TRIGGERS***************************************************************************
            // Loop in the directory
            var triggerBubblingDirectory = ConfigurationBag.DirectoryBubblingTriggers();
            var regTriggers = new Regex(ConfigurationBag.GcEventsConfigurationFilesExtensions);
            var triggerConfigurationsFiles =
                Directory.GetFiles(triggerBubblingDirectory, "*", SearchOption.AllDirectories)
                    .Where(path => regTriggers.IsMatch(path))
                    .ToList();

            // For each trigger search for the trigger in event bubbling and set the properties
            foreach (var triggerConfigurationsFile in triggerConfigurationsFiles)
            {
                var fileLocked = true;
                while (fileLocked)
                {
                    try
                    {
                        var triggerConfigurationsByteContent = File.ReadAllBytes(triggerConfigurationsFile);
                        fileLocked = false;
                        JsonConvert.DeserializeObject<TriggerConfiguration>(
                            EncodingDecoding.EncodingBytes2String(triggerConfigurationsByteContent));

                        // Add to the list
                        SyncronizationConfigurationFileList.Add(
                            new SyncConfigurationFile(
                                "Trigger",
                                triggerConfigurationsFile,
                                triggerConfigurationsByteContent,
                                ConfigurationBag.Configuration.ChannelId));
                    }
                    catch (IOException ioex)
                    {
                        Debug.WriteLine("<Lock>" + ioex.Message);
                        Debug.WriteLine("<Lock>");
                        fileLocked = true;
                    }
                    catch (Exception ex)
                    {
                        LogEngine.WriteLog(
                            ConfigurationBag.EngineName,
                            $"Error in {MethodBase.GetCurrentMethod().Name} File {triggerConfigurationsFile}",
                            Constant.LogLevelError,
                            Constant.TaskCategoriesError,
                            ex,
                            Constant.LogLevelError);
                        fileLocked = false;
                    }
                }
            }

            // EVENTS******************************************************************************
            // Loop in the directory
            var eventsBubblingDirectory = ConfigurationBag.DirectoryBubblingEvents();
            var regEvents = new Regex(ConfigurationBag.GcEventsConfigurationFilesExtensions);
            var propertyEventsFiles =
                Directory.GetFiles(eventsBubblingDirectory, "*", SearchOption.AllDirectories)
                    .Where(path => regEvents.IsMatch(path))
                    .ToList();

            // For each event search for the trigger in event bubbling and set the properties
            foreach (var propertyEventsFile in propertyEventsFiles)
            {
                var fileLocked = true;
                while (fileLocked)
                {
                    try
                    {
                        var propertyEventsByteContent = File.ReadAllBytes(propertyEventsFile);
                        fileLocked = false;
                        JsonConvert.DeserializeObject<EventConfiguration>(
                            EncodingDecoding.EncodingBytes2String(propertyEventsByteContent));

                        // Add to the global list
                        SyncronizationConfigurationFileList.Add(
                            new SyncConfigurationFile(
                                "Event",
                                propertyEventsFile,
                                propertyEventsByteContent,
                                ConfigurationBag.Configuration.ChannelId));
                    }
                    catch (IOException ioex)
                    {
                        Debug.WriteLine("<Lock>" + ioex.Message);
                        Debug.WriteLine("<Lock>");
                        fileLocked = true;
                    }
                    catch (Exception ex)
                    {
                        LogEngine.WriteLog(
                            ConfigurationBag.EngineName,
                            $"Error in {MethodBase.GetCurrentMethod().Name} File {propertyEventsFile}",
                            Constant.LogLevelError,
                            Constant.TaskCategoriesError,
                            ex,
                            Constant.LogLevelError);
                        fileLocked = false;
                    }
                }
            }
        }

        /// <summary>
        ///     Execute the triggres in polling required
        /// </summary>
        public static void ExecuteBubblingTriggerConfigurationPolling()
        {
            foreach (var bubblingTriggerConfiguration in BubblingTriggerConfigurationsPolling)
            {
                //This function is for internall polling only, no thread pool available
                ExecuteTriggerConfigurationForPolling(bubblingTriggerConfiguration,null);

            }
        }

        /// <summary>
        ///     Execute the triggres in single instance required
        /// </summary>
        public static void ExecuteBubblingTriggerConfigurationsSingleInstance()
        {
            foreach (var bubblingTriggerConfiguration in BubblingTriggerConfigurationsSingleInstance)
            {
                // If NOP the execute
                // NOP is for the Not Operation Execute
                if (!bubblingTriggerConfiguration.Nop)
                {
                    Debug.WriteLine(
                        $"Run single instances {bubblingTriggerConfiguration.Name}",
                        ConsoleColor.Green);
                    ExecuteTriggerConfiguration(bubblingTriggerConfiguration, null);

                    //ExecuteTriggerConfiguration(bubblingTriggerConfiguration, null);
                }
            }
        }

        public static void ExecuteTriggerConfiguration(BubblingObject bubblingObject, byte[] embeddedContent)
        {
            ThreadPool.QueueUserWorkItem(ExecuteTriggerConfigurationInPool,
                new object[]
                {
                    bubblingObject,
                    embeddedContent
                });


        }

        /// <summary>
        ///     Execute a trigger and if the Execute method return != null then it set all return value in a action and excute the
        ///     action
        /// </summary>
        /// <param name="bubblingObject">
        ///     The bubbling Trigger Configuration.
        /// </param>
        public static void ExecuteTriggerConfigurationForPolling(BubblingObject bubblingObject, byte[] embeddedContent)
        {


            try
            {
                // Set master EventActionContext eccoloqua
                var eventActionContext = new ActionContext(bubblingObject);

                eventActionContext.MessageId = Guid.NewGuid().ToString();

                // In the first execute the main Execute method
                ITriggerAssembly triggerAssemblyTemp;
                CacheTriggerComponents.TryGetValue(bubblingObject.IdComponent, out triggerAssemblyTemp);
                ITriggerType triggerType =
                    Activator.CreateInstance(triggerAssemblyTemp.AssemblyClassType) as ITriggerType;

                lock (CacheTriggerRunning)
                {
                    CacheTriggerRunning.Add(bubblingObject.IdConfiguration + bubblingObject.IdComponent, triggerType);
                }

                triggerType.DataContext = embeddedContent;


                //todo optimization ma le proprieta' sono gia settate nel refreshbubbling
                // Assign all propertyies value trigger to class instance and execute

                //triggerConfiguration.Properties["DataContext"].Value = embeddedContent;

                //Set assembly properties values
                IEnumerable<PropertyInfo> propertyInfos =
                    triggerType.GetType()
                        .GetProperties()
                        .ToList()
                        .Where(
                            p =>
                                p.GetCustomAttributes(typeof(TriggerPropertyContract), true).Length > 0 &&
                                p.Name != "DataContext");
                foreach (var propertyInfo in propertyInfos)
                {
                    propertyInfo.SetValue(triggerType,
                        Convert.ChangeType(bubblingObject.Properties[propertyInfo.Name].Value, propertyInfo.PropertyType),
                        null);
                }

                try
                {
                    triggerType.Execute(delegateActionTrigger, eventActionContext);
                }
                catch (TargetInvocationException ex)
                {
                    LogEngine.WriteLog(
                        ConfigurationBag.EngineName,
                        $"Critical Error in {MethodBase.GetCurrentMethod().Name} invoking the component {triggerAssemblyTemp.AssemblyFile}",
                        Constant.LogLevelError,
                        Constant.TaskCategoriesError,
                        ex,
                        Constant.LogLevelError);
                }
            }
            catch (Exception ex)
            {
                LogEngine.WriteLog(
                    ConfigurationBag.EngineName,
                    $"Error in {MethodBase.GetCurrentMethod().Name}\r IDConfiguration: {bubblingObject.IdConfiguration}" +
                    $" - IdComponent: {bubblingObject.IdComponent}.",
                    Constant.LogLevelError,
                    Constant.TaskCategoriesError,
                    ex,
                    Constant.LogLevelError);
            }
        }

        /// <summary>
        ///     Execute a trigger and if the Execute method return != null then it set all return value in a action and excute the
        ///     action
        /// </summary>
        /// <param name="bubblingObject">
        ///     The bubbling Trigger Configuration.
        /// </param>
        public static void ExecuteTriggerConfigurationInPool(object objectState)
        {

            object[] callBackParameters = objectState as object[];
            BubblingObject bubblingObject = (BubblingObject)callBackParameters[0];
            byte[] embeddedContent = (byte[])callBackParameters[1];
            try
            {
                // Set master EventActionContext eccoloqua
                var eventActionContext = new ActionContext(bubblingObject);

                eventActionContext.MessageId = Guid.NewGuid().ToString();

                // In the first execute the main Execute method
                ITriggerAssembly triggerAssemblyTemp;
                CacheTriggerComponents.TryGetValue(bubblingObject.IdComponent, out triggerAssemblyTemp);
                ITriggerType triggerType =
                    Activator.CreateInstance(triggerAssemblyTemp.AssemblyClassType) as ITriggerType;

                lock (CacheTriggerRunning)
                {
                    CacheTriggerRunning.Add(bubblingObject.IdConfiguration + bubblingObject.IdComponent, triggerType);
                }

                triggerType.DataContext = embeddedContent;


                //todo optimization ma le proprieta' sono gia settate nel refreshbubbling
                // Assign all propertyies value trigger to class instance and execute

                //triggerConfiguration.Properties["DataContext"].Value = embeddedContent;

                //Set assembly properties values
                IEnumerable<PropertyInfo> propertyInfos =
                    triggerType.GetType()
                        .GetProperties()
                        .ToList()
                        .Where(
                            p =>
                                p.GetCustomAttributes(typeof(TriggerPropertyContract), true).Length > 0 &&
                                p.Name != "DataContext");
                foreach (var propertyInfo in propertyInfos)
                {
                    propertyInfo.SetValue(triggerType,
                        Convert.ChangeType(bubblingObject.Properties[propertyInfo.Name].Value, propertyInfo.PropertyType),
                        null);
                }

                try
                {
                    triggerType.Execute(delegateActionTrigger, eventActionContext);
                }
                catch (TargetInvocationException ex)
                {
                    LogEngine.WriteLog(
                        ConfigurationBag.EngineName,
                        $"Critical Error in {MethodBase.GetCurrentMethod().Name} invoking the component {triggerAssemblyTemp.AssemblyFile}",
                        Constant.LogLevelError,
                        Constant.TaskCategoriesError,
                        ex,
                        Constant.LogLevelError);
                }
            }
            catch (Exception ex)
            {
                LogEngine.WriteLog(
                    ConfigurationBag.EngineName,
                    $"Error in {MethodBase.GetCurrentMethod().Name}\r IDConfiguration: {bubblingObject.IdConfiguration}" +
                    $" - IdComponent: {bubblingObject.IdComponent}.",
                    Constant.LogLevelError,
                    Constant.TaskCategoriesError,
                    ex,
                    Constant.LogLevelError);
            }
        }

        /// <summary>
        ///     Initialize a trigger and if the Execute method return != null then it set all return value in a action and excute
        ///     the
        ///     action
        /// </summary>
        /// <param name="bubblingTriggerConfiguration">
        ///     The bubbling Trigger Configuration.
        /// </param>
        public static TriggerEmbeddedBag InitializeEmbeddedTrigger(string configurationId, string componeId)
        {
            //todo optimiztion usare un dictionary? metti confid +idcomponent per identificare la chiave
            var triggerConfiguration = (from trigger in BubblingTriggerConfigurationsSingleInstance
                where trigger.IdComponent == componeId && trigger.IdConfiguration == configurationId
                select trigger).First();


            try
            {
                if (triggerConfiguration == null)
                {
                    throw new ArgumentNullException(nameof(triggerConfiguration));
                }

                // Set master EventActionContext eccoloqua
                var eventActionContext = new ActionContext(triggerConfiguration);

                ITriggerAssembly triggerAssemblyTemp;
                CacheTriggerComponents.TryGetValue(triggerConfiguration.IdComponent, out triggerAssemblyTemp);
                ITriggerType triggerType =
                    Activator.CreateInstance(triggerAssemblyTemp.AssemblyClassType) as ITriggerType;

                // Create the object instance

                IEnumerable<PropertyInfo> propertyInfos =
                    triggerType.GetType()
                        .GetProperties()
                        .ToList()
                        .Where(
                            p =>
                                p.GetCustomAttributes(typeof(TriggerPropertyContract), true).Length > 0 &&
                                p.Name != "DataContext");
                // Assign all propertyies value trigger to class instance and execute
                foreach (var propertyInfo in propertyInfos)
                {
                    propertyInfo.SetValue(triggerType,
                        Convert.ChangeType(triggerConfiguration.Properties[propertyInfo.Name].Value,
                            propertyInfo.PropertyType), null);
                }

                object[] parameters = {delegateActionTrigger, eventActionContext};

                TriggerEmbeddedBag triggerEmbeddedBag = new TriggerEmbeddedBag();
                triggerEmbeddedBag.DelegateActionTrigger = delegateActionTrigger;
                triggerEmbeddedBag.ActionContext = eventActionContext;

                triggerEmbeddedBag.Parameters = parameters;
                triggerEmbeddedBag.Properties = null;
                triggerEmbeddedBag.BaseActionTrigger = null;
                triggerEmbeddedBag.ITriggerTypeInstance = triggerType;
                triggerEmbeddedBag.ActionContext = eventActionContext;

                return triggerEmbeddedBag;
            }
            catch (Exception ex)
            {
                LogEngine.WriteLog(
                    ConfigurationBag.EngineName,
                    $"Error in {MethodBase.GetCurrentMethod().Name}\r Possible reasons: A property is missing or null, a configuration file is wrong.",
                    Constant.LogLevelError,
                    Constant.TaskCategoriesError,
                    ex,
                    Constant.LogLevelError);
                return null;
            }
        }


        /// <summary>
        ///     Initialize a trigger and if the Execute method return != null then it set all return value in a action and excute
        ///     the
        ///     action
        /// </summary>
        /// <param name="bubblingTriggerConfiguration">
        ///     The bubbling Trigger Configuration.
        /// </param>
        public static byte[] EngineExecuteEmbeddedTrigger(TriggerEmbeddedBag triggerEmbeddedBag)
        {
            try
            {
                try
                {
                    //invoke trigger

                    triggerEmbeddedBag.ITriggerTypeInstance.DataContext =
                        SerializationEngine.ObjectToByteArray(triggerEmbeddedBag.Properties);

                    byte[] ret =
                        triggerEmbeddedBag.ITriggerTypeInstance.Execute(triggerEmbeddedBag.DelegateActionTrigger,
                            triggerEmbeddedBag.ActionContext);
                    return ret;
                }
                catch (TargetInvocationException ex)
                {
                    LogEngine.WriteLog(
                        ConfigurationBag.EngineName,
                        $"Critical Error in {MethodBase.GetCurrentMethod().Name} invoking the method [Id: {triggerEmbeddedBag.BaseActionTrigger.Id}  Name: {triggerEmbeddedBag.BaseActionTrigger.Name} Description: {triggerEmbeddedBag.BaseActionTrigger.Description}",
                        Constant.LogLevelError,
                        Constant.TaskCategoriesError,
                        ex,
                        Constant.LogLevelError);
                    return null;
                }
            }
            catch (Exception ex)
            {
                LogEngine.WriteLog(
                    ConfigurationBag.EngineName,
                    $"Error in {MethodBase.GetCurrentMethod().Name}\r Possible reasons: A property is missing or null, a configuration file is wrong.",
                    Constant.LogLevelError,
                    Constant.TaskCategoriesError,
                    ex,
                    Constant.LogLevelError);
                return null;
            }
        }

        public static void SyncAsyncActionReceived(byte[] content)
        {
        }


        /// <summary>
        ///     Execute a trigger and if the Execute method return != null then it set all return value in a action and excute the
        ///     action
        /// </summary>
        /// <param name="bubblingTriggerConfiguration">
        ///     The bubbling Trigger Configuration.
        /// </param>
        public static object ExecuteComponentConfiguration(string IdComponent, byte[] Content)
        {
            try
            {
                IChainComponentAssembly ChainComponentAssemblyTemp;
                CacheChainComponents.TryGetValue(IdComponent, out ChainComponentAssemblyTemp);
                IChainComponentType chainComponentType =
                    Activator.CreateInstance(ChainComponentAssemblyTemp.AssemblyClassType) as IChainComponentType;

                var componentConfiguration =
                    ConfigurationJsonComponentList.Find(comp => comp.Component.IdComponent == IdComponent);

                // Create the object instance


                IEnumerable<PropertyInfo> propertyInfos =
                    chainComponentType.GetType()
                        .GetProperties()
                        .ToList()
                        .Where(
                            p =>
                                p.GetCustomAttributes(typeof(ComponentPropertyContract), true).Length > 0 &&
                                p.Name != "DataContext");

                // Assign all propertyies value trigger to class instance and execute
                foreach (var propertyInfo in propertyInfos)
                {
                    var propertyComponent =
                        componentConfiguration.Component.ComponentProperties.First(p => p.Name == propertyInfo.Name);
                    propertyInfo.SetValue(chainComponentType, propertyComponent.Value,
                        null);
                }

                //Set DataContext
                PropertyInfo propertyInfosDataContext = chainComponentType.GetType().GetProperties().First(p => p.Name == "DataContext");
                propertyInfosDataContext.SetValue(chainComponentType, Content,null);
                try
                {
                    return chainComponentType.Execute();
                }
                catch (TargetInvocationException ex)
                {
                    LogEngine.WriteLog(
                        ConfigurationBag.EngineName,
                        $"Critical Error in {MethodBase.GetCurrentMethod().Name} invoking the component {ChainComponentAssemblyTemp.AssemblyFile}",
                        Constant.LogLevelError,
                        Constant.TaskCategoriesError,
                        ex,
                        Constant.LogLevelError);
                    return null;
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
                return null;
            }
        }

        public static byte[] ExecuteChain(List<Chain> chains, byte[] content)
        {
            try
            {
                byte[] chainContent = content;
                foreach (var chain in chains)
                {
                    Debug.WriteLine(
                        $"-!CHAINS HAS TO BE EXECUTED!- Chain name {chain.IdChain} - Chain Name {chain.Name}");
                    var bubblingComponentConfiguration =
                        ConfigurationJsonChainFileList.Find(property => property.Chain.IdChain == chain.IdChain);
                    if (bubblingComponentConfiguration == null)
                    {
                        LogEngine.WriteLog(ConfigurationBag.EngineName,
                            $"Error in {MethodBase.GetCurrentMethod().Name},Chain Id [{chain.IdChain}] not found in the event chains directory.",
                            Constant.LogLevelError,
                            Constant.TaskCategoriesError,
                            null,
                            Constant.LogLevelError);
                        return null;
                    }

                    ChainConfiguration chainConfiguration = bubblingComponentConfiguration;
                    foreach (var component in chainConfiguration.Chain.Components)
                    {
                        object newContent = ExecuteComponentConfiguration(component.idComponent, chainContent);
                        chainContent = (byte[]) newContent;
                    }
                }
                return chainContent;
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

        public static void ExecuteEventsInTrigger(BubblingObject bubblingObject,
            Event bubblingObjectEvent,
            bool internalCall,
            string senderEndpointId)
        {
            ThreadPool.QueueUserWorkItem(ExecuteEventsInTriggerPool,
                new object[]
                {
                    bubblingObject,
                    bubblingObject.Events[0],
                    false,
                    bubblingObject.SenderPointId
                });
        }

        /// <summary>
        ///     Execute all the action correlate to the trigger
        ///     it send the trigger event
        /// </summary>
        /// <param name="objectState"></param>
        public static void ExecuteEventsInTriggerPool(object objectState)
        {
            object[] callBackParameters = objectState as object[];
            BubblingObject bubblingObject = (BubblingObject) callBackParameters[0];
            Event bubblingObjectEvent = (Event) callBackParameters[1];
            bool internalCall = Convert.ToBoolean(callBackParameters[2]);
            string senderEndpointId = Convert.ToString(callBackParameters[3]);


            //Check if embedded trigger
            ITriggerAssembly triggerAssembly;
            CacheTriggerComponents.TryGetValue(bubblingObject.IdComponent, out triggerAssembly);

            bool embeddedTrigger = triggerAssembly != null && triggerAssembly.AssemblyClassType.Name ==
                                   "EmbeddedTrigger";


            try
            {
                // Set master EventActionContext 

                Debug.WriteLine("-!ACTIONS HAS TO BE EXECUTED!-", ConsoleColor.Green);


                // Trigger
                // Execute the event
                // Override the Correlation
                if (bubblingObjectEvent.Correlation != null)
                {
                    bubblingObject.CorrelationOverride = bubblingObjectEvent.Correlation;
                }

                // Look for the Event
                Debug.WriteLine("Event check > " + bubblingObjectEvent.Name);
                Debug.WriteLine("Event.IDConfiguration > " + bubblingObjectEvent.IdConfiguration);
                Debug.WriteLine(
                    "BubblingTriggersEventsActive.Count > " + BubblingTriggersEventsActive.Count);


                Debug.WriteLine("BubblingEvent loop on > " + BubblingTriggersEventsActive.Count);

                // Event
                Debug.WriteLine("BubblingEvent > " + bubblingObjectEvent.Name);

                IEventAssembly eventAssembly;
                CacheEventComponents.TryGetValue(bubblingObjectEvent.IdComponent, out eventAssembly);

                IEventType eventType = Activator.CreateInstance(eventAssembly.AssemblyClassType) as IEventType;


                var eventExecuted = true;

                //If embedded trigger and exist proberties in the dataconext then the properties event will be overrided
                if (embeddedTrigger)
                {
                    eventType.DataContext = null;

                    List<Property> properties =
                        (List<Property>)
                        SerializationEngine.ByteArrayToObject(bubblingObject.DataContext);
                    foreach (var property in properties)
                    {
                        try
                        {
                            var propertyInfoAssembly = eventType.GetType().GetProperty(property.Name);
                            propertyInfoAssembly.SetValue(eventType,
                                Convert.ChangeType(property.Value, propertyInfoAssembly.PropertyType), null);
                        }
                        catch (Exception ex)
                        {
                            LogEngine.WriteLog(
                                ConfigurationBag.EngineName,
                                $"Warning! in {MethodBase.GetCurrentMethod().Name} - Error mapping property {property.Name} with Id Component {bubblingObjectEvent.IdComponent}",
                                Constant.LogLevelError,
                                Constant.TaskCategoriesError,
                                ex,
                                Constant.LogLevelWarning);
                        }
                    }
                }
                else
                {
                    eventType.DataContext = bubblingObject.DataContext;

                    //Map the event property
                    //Get configuration file
                    var eventConfiguration =
                        ConfigurationJsonEventFileList[
                            bubblingObjectEvent.IdConfiguration + bubblingObjectEvent.IdComponent];

                    if (eventConfiguration.Event.EventProperties != null)
                    {
                        IEnumerable<PropertyInfo> propertyInfos =
                            eventType.GetType()
                                .GetProperties()
                                .ToList()
                                .Where(
                                    p =>
                                        p.GetCustomAttributes(typeof(EventPropertyContract), true).Length > 0 &&
                                        p.Name != "DataContext");
                        //todo optimization vedi se riesci a ottimizzarlo.
                        //problema e' che il dictionary non e' serializzabile, un array?
                        foreach (var propertyInfo in propertyInfos)
                        {
                            propertyInfo.SetValue(eventType,
                                Convert.ChangeType(
                                    eventConfiguration.Event.CacheEventProperties[propertyInfo.Name].Value,
                                    propertyInfo.PropertyType),
                                null);
                        }
                    }
                    // Overriding properties?
                    if (bubblingObjectEvent.EventProperties != null)
                    {
                        foreach (var bubblingProperty in bubblingObjectEvent.EventProperties)
                        {
                            PropertyInfo propertyInfo =eventType.GetType()
                                                                .GetProperties().First(p => p.Name == bubblingProperty.Name);
                            propertyInfo.SetValue(eventType,
                                        Convert.ChangeType(bubblingProperty.Value, propertyInfo.PropertyType),
                                        null);
                        }
   
                    }
                }
                // Pass Data property to Execute
                Debug.WriteLine("Invoking > " + eventType);

                var eventActionContext = new ActionContext(bubblingObject);
                eventType.Execute(delegateActionEvent, eventActionContext);


                if (eventExecuted)
                {
                    Debug.WriteLine($"-!EXECUTING EVENT {bubblingObjectEvent.Name}!-", ConsoleColor.Green);
                }
                else
                {
                    LogEngine.WriteLog(
                        ConfigurationBag.EngineName,
                        $"Warning! in {MethodBase.GetCurrentMethod().Name} - Try to execute a not available event for trigger {bubblingObject.Name} and IdComponent {bubblingObject.IdComponent}",
                        Constant.LogLevelError,
                        Constant.TaskCategoriesError,
                        null,
                        Constant.LogLevelWarning);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error catched > " + ex.Message);
                LogEngine.WriteLog(
                    ConfigurationBag.EngineName,
                    $"Warning! in {MethodBase.GetCurrentMethod().Name} - Try to execute a not available event for trigger IdComponent {bubblingObject.IdComponent}",
                    Constant.LogLevelError,
                    Constant.TaskCategoriesError,
                    ex,
                    Constant.LogLevelError);
            }
        }


        /// <summary>
        ///     The create event up stream.
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public static bool CreateEventUpStream()
        {
            try
            {
                // Event Configuration
                connectionString = ConfigurationBag.Configuration.AzureNameSpaceConnectionString;
                eventHubName = ConfigurationBag.Configuration.GroupEventHubsName;

                LogEngine.WriteLog(
                    ConfigurationBag.EngineName,
                    $"Creating event up stream.",
                    Constant.LogLevelError,
                    Constant.TaskCategoriesError,
                    null,
                    Constant.LogLevelInformation);

                var builder = new ServiceBusConnectionStringBuilder(connectionString)
                {
                    TransportType =
                        TransportType.Amqp
                };

                HubClient = EventHubClient.CreateFromConnectionString(builder.ToString(), eventHubName);
                return true;
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
                return false;
            }
        }

        // *************EVENTS WATCHER MANAGEMENT*************************************
        // Main area managing the File system watcher (Change DLL, ChangeTrigger configuration ectcetera.)
        /// <summary>
        ///     By these area the engine is going to manage what is going to change and look and start the provisioning
        /// </summary>
        // *************EVENTS WATCHER MANAGEMENT*************************************
        public static void StartFolderWatcherEngine()
        {
            try
            {
                FswEventFolder.Path = ConfigurationBag.DirectoryBubbling();
                FswEventFolder.NotifyFilter = NotifyFilters.FileName | NotifyFilters.Size | NotifyFilters.LastWrite
                                              | NotifyFilters.Attributes;
                FswEventFolder.Filter = "*.*";

                FswEventFolder.EnableRaisingEvents = true;
                FswEventFolder.IncludeSubdirectories = true;

                FswEventFolder.Created += EventFolderChanged;
                FswEventFolder.Changed += EventFolderChanged;
                FswEventFolder.Deleted += EventFolderChanged;
                FswEventFolder.Renamed += EventFolderChanged;

                new Task(CheckConfigurationupdated).Start();

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

        /// <summary>
        ///     The event folder changed.
        /// </summary>
        /// <param name="source">
        ///     The source.
        /// </param>
        /// <param name="e">
        ///     The e.
        /// </param>
        private static void EventFolderChanged(object source, FileSystemEventArgs e)
        {
            ConfigurationUpdated = true;
        }

        private static void CheckConfigurationupdated()
        {
            while (true)
            {
                if (ConfigurationUpdated)
                {
                    ConfigurationUpdated = false;
                    try
                    {
                        SyncProvider.RefreshBubblingSetting(true);
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
                Thread.Sleep(5000);
            }
            
        }
    }

    /// <summary>
    ///     The host context.
    /// </summary>
    public class HostContext
    {
        /// <summary>
        ///     The data context.
        /// </summary>
        public object DataContext;
    }
}