// --------------------------------------------------------------------------------------------------
// <copyright file = "Embedded.cs" company="GrabCaster Ltd">
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

using System.Threading.Tasks;
using GrabCaster.Framework.Contracts;
using GrabCaster.Framework.Contracts.Bubbling;
using GrabCaster.Framework.Contracts.Serialization;
using GrabCaster.Framework.Contracts.Triggers;

namespace GrabCaster.Framework.Library
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Diagnostics;
    using System.Reflection;
    using System.Threading;
    using GrabCaster.Framework.Base;
    using GrabCaster.Framework.Common;
    using GrabCaster.Framework.Contracts.Configuration;
    using GrabCaster.Framework.Contracts.Events;
    using GrabCaster.Framework.Contracts.Globals;
    using GrabCaster.Framework.Engine;
    using GrabCaster.Framework.Engine.OffRamp;
    using GrabCaster.Framework.Log;
    using System.IO;
    using System.Text;    /// <summary>
                          /// The embedded point.
                          /// </summary>
    public class Embedded
    {

        public delegate void SetEventActionEventEmbedded(IEventType _this, ActionContext context);

        /// <summary>
        /// Used internally by the embedded
        /// </summary>
        public static SetEventActionEventEmbedded setEventActionEventEmbedded { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public static bool engineLoaded = false;
        // Global Action Events
        /// <summary>
        /// The delegate action event.
        /// </summary>
        private static ActionEvent _delegate;


        /// <summary>
        /// Handles the ProcessExit event of the CurrentDomain control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        public static void CurrentDomainProcessExit(object sender, EventArgs e)
        {
            CoreEngine.StopEventEngine();
        } // CurrentDomain_ProcessExit

        public static void InitializeEngine()
        {
            Thread t = new Thread(StartEngine);
            t.Start();
            EngineStartedAsync();
        }
        public static void StartMinimalEngine()
        {
            try
            {
                ConfigurationBag.LoadConfiguration();
                LogEngine.Init();
                Debug.WriteLine(
                    $"Version {Assembly.GetExecutingAssembly().GetName().Version}",
                    ConsoleColor.Green);

                if (ConfigurationBag.Configuration.DisableExternalEventsStreamEngine)
                {
                    LogEngine.WriteLog(
                        ConfigurationBag.EngineName,
                        "Warning the Device Provider Interface is disable, the GrabCaster point will be able to work in local mode only.",
                        Constant.LogLevelError,
                        Constant.TaskCategoriesError,
                        null,
                        Constant.LogLevelWarning);
                }
                // Could be useful?
                //if (!Environment.UserInteractive)
                //{
                //    Debug.WriteLine("GrabCaster-servicesToRun procedure initialization.");
                //    // ServiceBase[] servicesToRun = { new NTWindowsService() };
                //    Debug.WriteLine("GrabCaster-servicesToRun procedure starting.");
                //    // ServiceBase.Run(servicesToRun);
                //}

                Debug.WriteLine("--GrabCaster Sevice Initialization--Start Engine.", ConsoleColor.Green);
               // delegateActionEvent = delegateActionEventEmbedded;
                CoreEngine.StartEventEngine(null);
                engineLoaded = true;
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

        }

        public static void StartEngine()
        {
            try
            {
                ConfigurationBag.LoadConfiguration();
                LogEngine.Init();
                Debug.WriteLine(
                    $"Version {Assembly.GetExecutingAssembly().GetName().Version}",
                    ConsoleColor.Green);

                if (ConfigurationBag.Configuration.DisableExternalEventsStreamEngine)
                {
                    LogEngine.WriteLog(
                        ConfigurationBag.EngineName,
                        "Warning the Device Provider Interface is disable, the GrabCaster point will be able to work in local mode only.",
                        Constant.LogLevelError,
                        Constant.TaskCategoriesError,
                        null,
                        Constant.LogLevelWarning);
                }
                // Could be useful?
                //if (!Environment.UserInteractive)
                //{
                //    Debug.WriteLine("GrabCaster-servicesToRun procedure initialization.");
                //    // ServiceBase[] servicesToRun = { new NTWindowsService() };
                //    Debug.WriteLine("GrabCaster-servicesToRun procedure starting.");
                //    // ServiceBase.Run(servicesToRun);
                //}

                Debug.WriteLine("--GrabCaster Sevice Initialization--Start Engine.", ConsoleColor.Green);
                _delegate = delegateActionEventEmbedded;
                CoreEngine.StartEventEngine(_delegate);
                engineLoaded = true;
                Thread.Sleep(Timeout.Infinite);
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

        }

        public static void EngineStartedAsync()
        {
            while (!engineLoaded) ;
        }

        /// <summary>
        /// The delegate event executed by a event
        /// </summary>
        /// <param name="eventType">
        /// </param>
        /// <param name="context">
        /// EventActionContext cosa deve essere restituito
        /// </param>
        private static void delegateActionEventEmbedded(IEventType eventType, ActionContext context)
        {
            try
            {
                //If embedded mode and trigger source == embeddedtrigger then not execute the internal embedded delelegate 
                //todo optimization qui controllavo se chi ha chiamato levento e un trigger? forse meglio usare un approccio diverso, ho rimosso il check fdel trigger, sembra inutile
               
             //   if (context.BubblingObjectBag.AssemblyClassType != typeof(GrabCaster.Framework.EmbeddedTrigger.EmbeddedTrigger))
                setEventActionEventEmbedded(eventType, context);

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
        /// Load the bubbling settings
        /// </summary>
        public static void InitializeOffRampEmbedded(ActionEvent delegateEmbedded)
        {
            //Load Configuration
            GrabCaster.Framework.Base.ConfigurationBag.LoadConfiguration();

            LogEngine.WriteLog(ConfigurationBag.EngineName,
                            "Inizialize Off Ramp embedded messaging.",
                            Constant.LogLevelError,
                            Constant.TaskCategoriesError,
                            null,
                            Constant.LogLevelInformation);

            //Solve App domain environment
            var current = AppDomain.CurrentDomain;
            current.AssemblyResolve += HandleAssemblyResolve;


            int triggers = 0;
            int events = 0;
            int components = 0;

            EventsEngine.InitializeTriggerEngine();
            EventsEngine.InitializeEmbeddedEvent(delegateEmbedded);
            //Load component list configuration
            EventsEngine.LoadAssemblyComponents(ref triggers,ref events, ref components);

            //Load event list configuration
            EventsEngine.RefreshBubblingSetting();
        }


        private static Assembly HandleAssemblyResolve(object sender, ResolveEventArgs args)
        {
            return CoreEngine.HandleAssemblyResolve(sender, args);
        }

        /// <summary>
        /// Execute an internal trigger, this is used to execute a configured trigger
        /// To use: configure a trigger and call it by 
        /// </summary>
        /// <param name="componeId">
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        /// http://localhost:8000/GrabCaster/ExecuteTrigger?TriggerID={3C62B951-C353-4899-8670-C6687B6EAEFC}
        public static bool ExecuteTrigger(string configurationId, string componeId, byte[] data)
        {
            try
            {
                var triggerSingleInstance = (from trigger in EventsEngine.BubblingTriggerConfigurationsSingleInstance
                                             where trigger.IdComponent == componeId && trigger.IdConfiguration == configurationId
                                             select trigger).First();
                EventsEngine.ExecuteTriggerConfiguration(triggerSingleInstance, data);
                return true;
            }
            catch (Exception ex)
            {
                LogEngine.WriteLog(
                    ConfigurationBag.EngineName,
                    $"Error in {MethodBase.GetCurrentMethod().Name} - The trigger ID {componeId} does not exist.",
                    Constant.LogLevelError,
                    Constant.TaskCategoriesError,
                    ex,
                    Constant.LogLevelError);
                return false;
            }
        }


        /// <summary>
        /// Initialize an embedded trigger 
        /// </summary>
        /// <param name="componeId">
        /// </param>
        /// <param name="configurationId"></param>
        /// <param name="componentId"></param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static TriggerEmbeddedBag InitializeEmbeddedTrigger(string configurationId, string componentId)
        {
            try
            {

                return EventsEngine.InitializeEmbeddedTrigger(configurationId, componentId);

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

        public static AutoResetEvent eventStop { get; set; }
        public static SyncAsyncEventAction SyncAsyncEventAction { get; set; }
        private static byte[] _syncronousDataContext;
        /// <summary>
        /// Execute an embedded trigger 
        /// </summary>
        /// <param name="componeId">
        /// </param>
        /// <param name="triggerEmbeddedBag"></param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static byte[] ExecuteEmbeddedTrigger(TriggerEmbeddedBag triggerEmbeddedBag)
        {
            try
            {
                eventStop = new AutoResetEvent(false);
                SyncAsyncEventAction = SyncAsyncActionReceived;
                triggerEmbeddedBag.ActionContext.BubblingObjectBag.SyncronousToken = Guid.NewGuid().ToString();
                EventsEngine.SyncAsyncEventsAddDelegate(triggerEmbeddedBag.ActionContext.BubblingObjectBag.SyncronousToken,
                    SyncAsyncActionReceived);

                EventsEngine.EngineExecuteEmbeddedTrigger(triggerEmbeddedBag);

                eventStop.WaitOne();

                return _syncronousDataContext;


               
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

        public static void SyncAsyncActionReceived(byte[] content)
        {
            _syncronousDataContext = content;
            eventStop.Set();
        }





    }
}
