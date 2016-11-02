// LogEngine.cs
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
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using GrabCaster.Framework.Base;
using GrabCaster.Framework.Contracts.Attributes;
using GrabCaster.Framework.Contracts.Log;

#endregion

namespace GrabCaster.Framework.Log
{
    /// <summary>
    ///     Class to manage the console messages
    /// </summary>
    public class ConsoleMessage
    {
        public ConsoleMessage(string message, ConsoleColor consoleColor)
        {
            ConsoleColor = consoleColor;
            Message = message;
        }

        public ConsoleColor ConsoleColor { get; set; }

        public string Message { get; set; }
    }

    /// <summary>
    ///     Log engine master class
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
                DirectEventViewerLog($"Error in {MethodBase.GetCurrentMethod().Name} - {ex.Message}", 1);
                WriteLog(
                    ConfigurationBag.EngineName,
                    $"Error in {MethodBase.GetCurrentMethod().Name}",
                    Constant.LogLevelError,
                    Constant.TaskCategoriesError,
                    ex,
                    Constant.LogLevelError);
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
            if (!Enabled || logLevel >= ConfigurationBag.Configuration.LoggingLevel)
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
                var exceptionText = logMessage.ExceptionObject != ""
                    ? "\r-->Exception:" + logMessage.ExceptionObject
                    : "";
                logMessage.Message =
                    $"-Level:{logLevel}|Source:{source}|Message:{message}|Severity:{eventId}|-TaskCategory:{taskCategory}{exceptionText}";

                QueueAbstractMessage.Enqueue(logMessage);
            }
            catch (Exception)
            {
                //Last error point
                DirectEventViewerLog(logMessage.Message, Constant.LogLevelError);
            }
        }

        /// <summary>
        ///     Write in eventviewer
        /// </summary>
        /// <param name="message"></param>
        /// <param name="eventLogEntryType"></param>
        public static void DirectEventViewerLog(string message, int eventLogEntryType)
        {
            EventLog.WriteEntry("GrabCaster", message, (EventLogEntryType) eventLogEntryType, 0);
        }

        #region MAIN LOG ABSTRACTED ENGINE

        /// <summary>
        ///     Lock slim class for console messages
        /// </summary>
        public sealed class LogQueueAbstractMessage : LockSlimQueueLog<LogMessage>
        {
            public LogQueueAbstractMessage(int capLimit, int timeLimit)
            {
                CapLimit = capLimit;
                TimeLimit = timeLimit;
                InitTimer();
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
            if (logMessages.Count > 0)
                LogEngineComponent.Flush();
        }

        #endregion

        #region INTERNAL CONSOLE LOG

        /// <summary>
        ///     Lock slim class for console messages
        /// </summary>
        public sealed class LogQueueConsoleMessage : LockSlimQueueLog<ConsoleMessage>
        {
            public LogQueueConsoleMessage(int capLimit, int timeLimit)
            {
                CapLimit = capLimit;
                TimeLimit = timeLimit;
                InitTimer();
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