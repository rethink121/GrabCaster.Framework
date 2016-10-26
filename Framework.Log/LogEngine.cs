﻿// --------------------------------------------------------------------------------------------------
// <copyright file = "LogEngine.cs" company="GrabCaster Ltd">
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
namespace GrabCaster.Framework.Log
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading;

    using GrabCaster.Framework.Base;
    using GrabCaster.Framework.Common;
    using GrabCaster.Framework.Contracts.Attributes;
    using GrabCaster.Framework.Contracts.Log;

    /// <summary>
    /// Class to manage the console messages
    /// </summary>
    public class ConsoleMessage
    {
        public ConsoleMessage(string message, ConsoleColor consoleColor)
        {
            this.ConsoleColor = consoleColor;
            this.Message = message;
        }

        public ConsoleColor ConsoleColor { get; set; }

        public string Message { get; set; }
    }

    /// <summary>
    /// Log engine master class
    /// </summary>
    public static class LogEngine
    {
        public enum Level
        {
            Info,

            Warning,

            Error
        }

        public static LogQueueConsoleMessage QueueConsoleMessage;

        public static LogQueueAbstractMessage QueueAbstractMessage;

        public static bool Enabled;

        public static bool Verbose;

        public static bool ConsoleOut = true;

        private static readonly string EventViewerSource = ConfigurationBag.EngineName;

        private static readonly string EventViewerLog = ConfigurationBag.EngineName;

        private static ILogEngine LogEngineComponent;

        public static void Init()
        {
            try
            {
                ConfigurationBag.LoadConfiguration();
                Enabled = ConfigurationBag.Configuration.LoggingEngineEnabled;
                Verbose = ConfigurationBag.Configuration.LoggingVerbose;
                //Load logging external component
                var loggingComponent = Path.Combine(
                    ConfigurationBag.Configuration.DirectoryOperativeRootExeName,
                    ConfigurationBag.Configuration.LoggingComponent);

                Debug.WriteLine("Check Abstract Logging Engine.");

                //Create the reflection method cached 
                var assembly = Assembly.LoadFrom(loggingComponent);
                //Main class logging
                var assemblyClass = (from t in assembly.GetTypes()
                                     let attributes = t.GetCustomAttributes(typeof(LogContract), true)
                                     where t.IsClass && attributes != null && attributes.Length > 0
                                     select t).First();


                LogEngineComponent = Activator.CreateInstance(assemblyClass) as ILogEngine;

                Debug.WriteLine("LogEventUpStream - Inizialize the external log");

                LogEngineComponent.InitLog();

                Debug.WriteLine("Initialize Abstract Logging Engine.");

                Debug.WriteLine("LogEventUpStream - CreateEventSource if not exist");
                if (!EventLog.SourceExists(EventViewerSource))
                {
                    EventLog.CreateEventSource(EventViewerSource, EventViewerLog);
                }

                //Create the QueueConsoleMessage internal queue
                Debug.WriteLine(
                    "LogEventUpStream - logQueueConsoleMessage.OnPublish += LogQueueConsoleMessageOnPublish");
                QueueConsoleMessage =
                    new LogQueueConsoleMessage(
                        ConfigurationBag.Configuration.ThrottlingConsoleLogIncomingRateNumber,
                        ConfigurationBag.Configuration.ThrottlingConsoleLogIncomingRateSeconds);
                QueueConsoleMessage.OnPublish += QueueConsoleMessageOnPublish;

                //Create the QueueAbstractMessage internal queue
                Debug.WriteLine(
                    "LogEventUpStream - logQueueAbstractMessage.OnPublish += LogQueueAbstractMessageOnPublish");
                QueueAbstractMessage = new LogQueueAbstractMessage(
                    ConfigurationBag.Configuration.ThrottlingLsiLogIncomingRateNumber,
                    ConfigurationBag.Configuration.ThrottlingLsiLogIncomingRateSeconds);
                QueueAbstractMessage.OnPublish += QueueAbstractMessageOnPublish;
                Debug.WriteLine("LogEventUpStream - Log Queues initialized.");
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

        public static void WriteLog(
        string source,
        string message,
        int eventId,
        string taskCategory,
        Exception exception,
        int logLevel)
        {
            if (!Enabled || ConfigurationBag.Configuration.LoggingLevel <= logLevel)
                return;

            Debug.WriteLine($"GrabCaster-{message}");
            var logMessage = new LogMessage();
            try
            {


                if (exception != null)
                {
                    logMessage.ExceptionObject =
                        $"-HResult: {exception.HResult}\r -Error Message: {exception.Message + ""}\r -InnerExcetion: {exception.InnerException}\r -Source: {exception.Source}\r -StackTrace: {exception.StackTrace}";
                }
                else
                {
                    logMessage.ExceptionObject = "";
                }

                // ReSharper disable once SpecifyACultureInStringConversionExplicitly
                logMessage.DateTime = DateTime.Now.ToString();
                logMessage.EventId = eventId;
                logMessage.MessageId = Guid.NewGuid().ToString();
                logMessage.Level = logLevel;
                logMessage.Source = ConfigurationBag.EngineName;
                logMessage.PointId = ConfigurationBag.Configuration.PointId;
                logMessage.PointName = ConfigurationBag.Configuration.PointName;
                logMessage.ChannelId = ConfigurationBag.Configuration.ChannelId;

                logMessage.PartitionKey = ConfigurationBag.Configuration.PointId;
                logMessage.RowKey = Guid.NewGuid().ToString();

                logMessage.ChannelName = ConfigurationBag.Configuration.ChannelName;
                logMessage.TaskCategory = taskCategory;
                var exceptionText = logMessage.ExceptionObject != "" ? "\r-->Exception:" + logMessage.ExceptionObject : "";
                logMessage.Message =
                    $"-Level:{logLevel}|Source:{source}|Message:{message}|Severity:{eventId}|-TaskCategory:{taskCategory}{exceptionText}";

                if (QueueAbstractMessage != null)
                {
                    QueueAbstractMessage.Enqueue(logMessage);
                }
            }
            catch (Exception ex)
            {
                //Last error point
                DirectEventViewerLog(logMessage.Message, Constant.LogLevelError);
            }
        }
        /// <summary>
        /// Write in eventviewer
        /// </summary>
        /// <param name="message"></param>
        /// <param name="eventLogEntryType"></param>
        public static void DirectEventViewerLog(string message, int eventLogEntryType)
        {
            EventLog.WriteEntry("GrabCaster", message, (EventLogEntryType)eventLogEntryType, 0);
        }
        #region MAIN LOG ABSTRACTED ENGINE

        /// <summary>
        /// Lock slim class for console messages
        /// </summary>
        public sealed class LogQueueAbstractMessage : LockSlimQueueLog<LogMessage>
        {

            public LogQueueAbstractMessage(int capLimit, int timeLimit)
            {
                this.CapLimit = capLimit;
                this.TimeLimit = timeLimit;
                this.InitTimer();
            }
        }

        public static void QueueAbstractMessageOnPublish(List<LogMessage> logMessages)
        {
            if (!Enabled)
                return;

            foreach (var logMessage in logMessages)
            {
                if (logMessage.Level == 1)
                    DirectEventViewerLog(logMessage.Message, 1);

                LogEngineComponent.WriteLog(logMessage);
            }
            //If something logged then flush
            if(logMessages.Count > 0)
                LogEngineComponent.Flush();
        }


        #endregion

        #region INTERNAL CONSOLE LOG

        /// <summary>
        /// Lock slim class for console messages
        /// </summary>
        public sealed class LogQueueConsoleMessage : LockSlimQueueLog<ConsoleMessage>
        {
            public LogQueueConsoleMessage(int capLimit, int timeLimit)
            {
                this.CapLimit = capLimit;
                this.TimeLimit = timeLimit;
                this.InitTimer();
            }
        }

        public static void QueueConsoleMessageOnPublish(List<ConsoleMessage> consoleMessages)
        {
            if (!Enabled)
                return;

            foreach (var consoleMessage in consoleMessages)
            {
                Debug.WriteLine(consoleMessage.Message, consoleMessage.ConsoleColor);
            }
        }

        #endregion
    }
}