// --------------------------------------------------------------------------------------------------
// <copyright file = "EventTraceWatcher.cs" company="GrabCaster Ltd">
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
namespace GrabCaster.Framework.ETW
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.InteropServices;
    using System.Threading;

    using Core.Eventing.Interop;

    /// <summary>
    /// The trace level.
    /// </summary>
    public enum TraceLevel
    {
        /// <summary>
        /// The critical.
        /// </summary>
        Critical = 1,

        /// <summary>
        /// The error.
        /// </summary>
        Error = 2,

        /// <summary>
        /// The warning.
        /// </summary>
        Warning = 3,

        /// <summary>
        /// The information.
        /// </summary>
        Information = 4,

        /// <summary>
        /// The verbose.
        /// </summary>
        Verbose = 5
    }

    /// <summary>
    /// The event trace watcher.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1404:CodeAnalysisSuppressionMustHaveJustification", Justification = "Reviewed. Suppression is OK here.")]
    public sealed class EventTraceWatcher : IDisposable
    {
        /// <summary>
        /// The logger name.
        /// </summary>
        private readonly string loggerName;

        /// <summary>
        /// The async result.
        /// </summary>
        private IAsyncResult asyncResult;

        /// <summary>
        /// The enabled.
        /// </summary>
        private bool enabled;

        /// <summary>
        /// The event provider id.
        /// </summary>
        private Guid eventProviderId;

        /// <summary>
        /// The event trace properties.
        /// </summary>
        private EventTraceProperties eventTraceProperties;

        /// <summary>
        /// The log file.
        /// </summary>
        private EventTraceLogfile logFile;

        /// <summary>
        /// The process events delgate.
        /// </summary>
        private ProcessTraceDelegate processEventsDelgate;

        /// <summary>
        /// The session handle.
        /// </summary>
        private SessionSafeHandle sessionHandle;

        /// <summary>
        /// The trace event info cache.
        /// </summary>
        private SortedList<byte, TraceEventInfoWrapper> traceEventInfoCache =
            new SortedList<byte /*opcode*/, TraceEventInfoWrapper>();

        /// <summary>
        /// The trace handle.
        /// </summary>
        private TraceSafeHandle traceHandle;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventTraceWatcher"/> class.
        /// </summary>
        /// <param name="loggerName">
        /// The logger name.
        /// </param>
        /// <param name="eventProviderId">
        /// The event provider id.
        /// </param>
        public EventTraceWatcher(string loggerName, Guid eventProviderId)
        {
            this.loggerName = loggerName;
            this.eventProviderId = eventProviderId;
        }

        /// <summary>
        /// Gets or sets the match any keyword.
        /// </summary>
        public ulong MatchAnyKeyword { get; set; }

        /// <summary>
        /// Gets or sets the level.
        /// </summary>
        public TraceLevel Level { get; set; }

        /// <summary>
        /// The dispose.
        /// </summary>
        public void Dispose()
        {
            this.Cleanup();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="EventTraceWatcher"/> class. 
        /// </summary>
        ~EventTraceWatcher()
        {
            this.Cleanup();
        }

        /// <summary>
        /// The event arrived.
        /// </summary>
        public event EventHandler<EventArrivedEventArgs> EventArrived;

        /// <summary>
        /// The cleanup.
        /// </summary>
        private void Cleanup()
        {
            this.SetEnabled(false);
            foreach (var value in this.traceEventInfoCache.Values)
            {
                value.Dispose();
            }

            this.traceEventInfoCache = null;
        }

        /// <summary>
        /// The create event args from event record.
        /// </summary>
        /// <param name="eventRecord">
        /// The event record.
        /// </param>
        /// <returns>
        /// The <see cref="EventArrivedEventArgs"/>.
        /// </returns>
        private EventArrivedEventArgs CreateEventArgsFromEventRecord(EventRecord eventRecord)
        {
            var eventOpcode = eventRecord.EventHeader.EventDescriptor.Opcode;
            TraceEventInfoWrapper traceEventInfo;
            var shouldDispose = false;

            // Find the event information (schema).
            var index = this.traceEventInfoCache.IndexOfKey(eventOpcode);
            if (index >= 0)
            {
                traceEventInfo = this.traceEventInfoCache.Values[index];
            }
            else
            {
                traceEventInfo = new TraceEventInfoWrapper(eventRecord);
                try
                {
                    this.traceEventInfoCache.Add(eventOpcode, traceEventInfo);
                }
                catch (ArgumentException)
                {
                    // Some other thread added this entry.
                    shouldDispose = true;
                }
            }

            // Get the properties using the current event information (schema).
            var properties = traceEventInfo.GetProperties(eventRecord);

            // Dispose the event information because it doesn't live in the cache
            if (shouldDispose)
            {
                traceEventInfo.Dispose();
            }

            var args = new EventArrivedEventArgs(eventOpcode, properties);

            return args;
        }

        /// <summary>
        /// The event record callback.
        /// </summary>
        /// <param name="eventRecord">
        /// The event record.
        /// </param>
        private void EventRecordCallback([In] ref EventRecord eventRecord)
        {
            var eventArrived = this.EventArrived;
            if (eventArrived != null)
            {
                var e = this.CreateEventArgsFromEventRecord(eventRecord);
                eventArrived(this, e);
            }
        }

        /// <summary>
        /// The load existing event trace properties.
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        /// <exception cref="Win32Exception">
        /// </exception>
        private bool LoadExistingEventTraceProperties()
        {
            const int ErrorWmiInstanceNotFound = 4201;
            this.eventTraceProperties = new EventTraceProperties(true);
            var status = NativeMethods.QueryTrace(0, this.loggerName, ref this.eventTraceProperties);
            if (status == 0)
            {
                return true;
            }

            if (status == ErrorWmiInstanceNotFound)
            {
                // The instance name passed was not recognized as valid by a WMI data provider.
                return false;
            }

            throw new Win32Exception(status);
        }

        /// <summary>
        /// The process trace in background.
        /// </summary>
        /// <param name="traceInternalHandle">
        /// The trace handle.
        /// </param>
        /// <exception cref="Win32Exception">
        /// </exception>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private void ProcessTraceInBackground(TraceSafeHandle traceInternalHandle)
        {
            Exception asyncException = null;
            ulong[] array = { traceInternalHandle.UnsafeValue };

            try
            {
                // Begin receiving the events handled by EventRecordCallback.
                // It is a blocking call until the trace handle gets closed.
                var status = NativeMethods.ProcessTrace(array, 1, IntPtr.Zero, IntPtr.Zero);
                if (status != 0)
                {
                    const int ErrorInvalidHandle = 6;
                    if (status == ErrorInvalidHandle)
                    {
                        // The handle was closed before starting processing.
                    }
                    else
                    {
                        // Throw the exception to capture the stack.
                        throw new Win32Exception(status);
                    }
                }
            }
            catch (Exception exception)
            {
                asyncException = exception;
            }

            // Send exception to subscribers.
            var eventArrived = this.EventArrived;
            if (asyncException != null && eventArrived != null)
            {
                try
                {
                    eventArrived(this, new EventArrivedEventArgs(asyncException));
                }
                catch (Exception exception)
                {
                    if (exception is ThreadAbortException || exception is OutOfMemoryException
                        || exception is StackOverflowException)
                    {
                        throw;
                    }

                    // Never fail because non-critical exceptions thown by this method
                    // can be rethrow during disposing of this object.
                    Debug.Assert(false, "Exception was thrown from ProcessEventArrived handler", exception.ToString());
                }
            }
        }

        /// <summary>
        /// The set enabled.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        private void SetEnabled(bool value)
        {

            if (this.enabled == value)
            {
                return;
            }

            if (value)
            {
                this.StartTracing();
            }
            else
            {
                this.StopTracing();
            }

            this.enabled = value;
            
        }

        /// <summary>
        /// The start.
        /// </summary>
        public void Start()
        {
            this.SetEnabled(true);
        }

        /// <summary>
        /// The start tracing.
        /// </summary>
        /// <exception cref="Win32Exception">
        /// </exception>
        [SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands")]
        private void StartTracing()
        {
            const uint RealTime = 0x00000100;
            const uint EventRecord = 0x10000000;
            const uint BufferSize = 64;
            const uint MinBuffers = 20;
            const uint MaxBuffers = 200;
            const uint FlushTimerSeconds = 1;
            int status;

            if (!this.LoadExistingEventTraceProperties())
            {
                this.eventTraceProperties.SetParameters(RealTime, BufferSize, MinBuffers, MaxBuffers, FlushTimerSeconds);

                // Start trace session
                ulong unsafeSessionHandle;
                status = NativeMethods.StartTrace(
                    out unsafeSessionHandle,
                    this.loggerName,
                    ref this.eventTraceProperties);
                if (status != 0)
                {
                    throw new Win32Exception(status);
                }

                this.sessionHandle = new SessionSafeHandle(unsafeSessionHandle, this.loggerName);

                var emptyGuid = Guid.Empty;

                var windows7Version = new Version(6, 1, 7600);
                if (Environment.OSVersion.Version.CompareTo(windows7Version) >= 0)
                {
                    const int TimeToWaitForInitialize = 10 * 1000;
                    var enableParameters = new EnableTraceParameters
                                               {
                                                   Version = 1,
                                                   EnableProperty = EventEnableProperty.Sid
                                               };

                    // ENABLE_TRACE_PARAMETERS_VERSION
                    status = NativeMethods.EnableTraceEx2(
                        unsafeSessionHandle,
                        ref this.eventProviderId,
                        1,


                        // controlCode - EVENT_CONTROL_CODE_ENABLE_PROVIDER
                        (byte)this.Level,
                        this.MatchAnyKeyword,
                        0,
                        // matchAnyKeyword
                        TimeToWaitForInitialize,
                        ref enableParameters);
                }
                else
                {
                    status = NativeMethods.EnableTraceEx(
                        ref this.eventProviderId,
                        ref emptyGuid,
                        // sourceId
                        unsafeSessionHandle,
                        1,
                        // isEnabled
                        (byte)this.Level,
                        this.MatchAnyKeyword,
                        0,
                        // matchAllKeywords
                        EventEnableProperty.Sid,
                        IntPtr.Zero);
                }
                if (status != 0)
                {
                    throw new Win32Exception(status);
                }
            }

            this.logFile = new EventTraceLogfile
                               {
                                   LoggerName = this.loggerName,
                                   EventRecordCallback = this.EventRecordCallback,
                                   ProcessTraceMode = EventRecord | RealTime
                               };

            var unsafeTraceHandle = NativeMethods.OpenTrace(ref this.logFile);
            status = Marshal.GetLastWin32Error();
            if (status != 0)
            {
                throw new Win32Exception(status);
            }

            this.traceHandle = new TraceSafeHandle(unsafeTraceHandle);

            this.processEventsDelgate = this.ProcessTraceInBackground;
            this.asyncResult = this.processEventsDelgate.BeginInvoke(this.traceHandle, null, this.processEventsDelgate);
        }

        /// <summary>
        /// The stop.
        /// </summary>
        public void Stop()
        {
            this.SetEnabled(false);
        }

        /// <summary>
        /// The stop tracing.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands")]
        private void StopTracing()
        {
            if (this.traceHandle != null)
            {
                this.traceHandle.Dispose();
                this.traceHandle = null;
            }

            if (this.sessionHandle != null)
            {
                this.sessionHandle.Dispose();
                this.sessionHandle = null;
            }

            // Once the unmanaged resources got released, end the process trace thread
            // that may throw exception (e.g. OOM).
            if (this.processEventsDelgate != null && this.asyncResult != null)
            {
                this.processEventsDelgate.EndInvoke(this.asyncResult);
            }
        }

        /// <summary>
        /// The process trace delegate.
        /// </summary>
        /// <param name="traceHandle">
        /// The trace handle.
        /// </param>
        private delegate void ProcessTraceDelegate(TraceSafeHandle traceHandle);

        /// <summary>
        /// The trace safe handle.
        /// </summary>
        private sealed class TraceSafeHandle : SafeHandle
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="TraceSafeHandle"/> class.
            /// </summary>
            /// <param name="handle">
            /// The handle.
            /// </param>
            [SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands")]
            public TraceSafeHandle(ulong handle)
                : base(IntPtr.Zero, true)
            {
                this.UnsafeValue = handle;
            }

            /// <summary>
            /// Gets a value indicating whether is invalid.
            /// </summary>
            public override bool IsInvalid => this.UnsafeValue == 0;

            internal ulong UnsafeValue { get; }

            protected override bool ReleaseHandle()
            {
                return NativeMethods.CloseTrace(this.UnsafeValue) != 0;
            }
        }

        private sealed class SessionSafeHandle : SafeHandle
        {
            private readonly string loggerName;

            private readonly ulong sessionHandle;

            [SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands")]
            public SessionSafeHandle(ulong sessionHandle, string loggerName)
                : base(IntPtr.Zero, true)
            {
                this.sessionHandle = sessionHandle;
                this.loggerName = loggerName;
            }

            public override bool IsInvalid => this.sessionHandle == 0;

            protected override bool ReleaseHandle()
            {
                var properties = new EventTraceProperties(true /*initialize*/);
                return NativeMethods.StopTrace(this.sessionHandle, this.loggerName, out properties /*as statistics*/)
                       != 0;
            }
        }
    }
}