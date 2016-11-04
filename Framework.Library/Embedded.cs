// Embedded.cs
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
using System.Linq;
using System.Reflection;
using System.Threading;
using GrabCaster.Framework.Base;
using GrabCaster.Framework.Contracts;
using GrabCaster.Framework.Contracts.Events;
using GrabCaster.Framework.Contracts.Globals;
using GrabCaster.Framework.Engine;
using GrabCaster.Framework.Log;

#endregion

namespace GrabCaster.Framework.Library
{
    /// <summary>
    ///     The embedded point.
    /// </summary>
    public class Embedded
    {
        public delegate void SetEventActionEventEmbedded(IEventType _this, ActionContext context);

        /// <summary>
        /// </summary>
        public static bool engineLoaded;

        // Global Action Events
        /// <summary>
        ///     The delegate action event.
        /// </summary>
        private static ActionEvent _delegate;

        private static byte[] _syncronousDataContext;

        /// <summary>
        ///     Used internally by the embedded
        /// </summary>
        public static SetEventActionEventEmbedded setEventActionEventEmbedded { get; set; }

        public static AutoResetEvent eventStop { get; set; }
        public static SyncAsyncEventAction SyncAsyncEventAction { get; set; }


        /// <summary>
        ///     Handles the ProcessExit event of the CurrentDomain control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
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

                Debug.WriteLine("--GrabCaster Sevice Initialization--Start Engine.");
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
        ///     The delegate event executed by a event
        /// </summary>
        /// <param name="eventType">
        /// </param>
        /// <param name="context">
        ///     EventActionContext cosa deve essere restituito
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
        ///     Load the bubbling settings
        /// </summary>
        public static void InitializeOffRampEmbedded(ActionEvent delegateEmbedded)
        {
            //Load Configuration
            ConfigurationBag.LoadConfiguration();

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
            EventsEngine.LoadAssemblyComponents(ref triggers, ref events, ref components);

            //Load event list configuration
            EventsEngine.RefreshBubblingSetting();
        }


        private static Assembly HandleAssemblyResolve(object sender, ResolveEventArgs args)
        {
            return CoreEngine.HandleAssemblyResolve(sender, args);
        }

        /// <summary>
        ///     Execute an internal trigger, this is used to execute a configured trigger
        ///     To use: configure a trigger and call it by
        /// </summary>
        /// <param name="componeId">
        /// </param>
        /// <returns>
        ///     The <see cref="string" />.
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
        ///     Initialize an embedded trigger
        /// </summary>
        /// <param name="componeId">
        /// </param>
        /// <param name="configurationId"></param>
        /// <param name="componentId"></param>
        /// <returns>
        ///     The <see cref="string" />.
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

        /// <summary>
        ///     Execute an embedded trigger
        /// </summary>
        /// <param name="componeId">
        /// </param>
        /// <param name="triggerEmbeddedBag"></param>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public static byte[] ExecuteEmbeddedTrigger(TriggerEmbeddedBag triggerEmbeddedBag)
        {
            try
            {
                eventStop = new AutoResetEvent(false);
                SyncAsyncEventAction = SyncAsyncActionReceived;
                triggerEmbeddedBag.ActionContext.BubblingObjectBag.SyncronousToken = Guid.NewGuid().ToString();
                EventsEngine.SyncAsyncEventsAddDelegate(
                    triggerEmbeddedBag.ActionContext.BubblingObjectBag.SyncronousToken,
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