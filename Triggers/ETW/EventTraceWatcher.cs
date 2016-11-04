// EventTraceWatcher.cs
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
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Threading;
using Core.Eventing.Interop;

#endregion

namespace GrabCaster.Framework.ETW
{
    /// <summary>
    ///     The trace level.
    /// </summary>
    public enum TraceLevel
    {
        /// <summary>
        ///     The critical.
        /// </summary>
        Critical = 1,

        /// <summary>
        ///     The error.
        /// </summary>
        Error = 2,

        /// <summary>
        ///     The warning.
        /// </summary>
        Warning = 3,

        /// <summary>
        ///     The information.
        /// </summary>
        Information = 4,

        /// <summary>
        ///     The verbose.
        /// </summary>
        Verbose = 5
    }

    /// <summary>
    ///     The event trace watcher.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1404:CodeAnalysisSuppressionMustHaveJustification",
         Justification = "Reviewed. Suppression is OK here.")]
    public sealed class EventTraceWatcher : IDisposable
    {
        /// <summary>
        ///     The logger name.
        /// </summary>
        private readonly string _loggerName;

        /// <summary>
        ///     The async result.
        /// </summary>
        private IAsyncResult _asyncResult;

        /// <summary>
        ///     The enabled.
        /// </summary>
        private bool _enabled;

        /// <summary>
        ///     The event provider id.
        /// </summary>
        private Guid _eventProviderId;

        /// <summary>
        ///     The event trace properties.
        /// </summary>
        private EventTraceProperties _eventTraceProperties;

        /// <summary>
        ///     The log file.
        /// </summary>
        private EventTraceLogfile _logFile;

        /// <summary>
        ///     The process events delgate.
        /// </summary>
        private ProcessTraceDelegate _processEventsDelgate;

        /// <summary>
        ///     The session handle.
        /// </summary>
        private SessionSafeHandle _sessionHandle;

        /// <summary>
        ///     The trace event info cache.
        /// </summary>
        private SortedList<byte, TraceEventInfoWrapper> _traceEventInfoCache =
            new SortedList<byte /*opcode*/, TraceEventInfoWrapper>();

        /// <summary>
        ///     The trace handle.
        /// </summary>
        private TraceSafeHandle _traceHandle;

        /// <summary>
        ///     Initializes a new instance of the <see cref="EventTraceWatcher" /> class.
        /// </summary>
        /// <param name="loggerName">
        ///     The logger name.
        /// </param>
        /// <param name="eventProviderId">
        ///     The event provider id.
        /// </param>
        public EventTraceWatcher(string loggerName, Guid eventProviderId)
        {
            _loggerName = loggerName;
            _eventProviderId = eventProviderId;
        }

        /// <summary>
        ///     Gets or sets the match any keyword.
        /// </summary>
        public ulong MatchAnyKeyword { get; set; }

        /// <summary>
        ///     Gets or sets the level.
        /// </summary>
        public TraceLevel Level { get; set; }

        /// <summary>
        ///     The dispose.
        /// </summary>
        public void Dispose()
        {
            Cleanup();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Finalizes an instance of the <see cref="EventTraceWatcher" /> class.
        /// </summary>
        ~EventTraceWatcher()
        {
            Cleanup();
        }

        /// <summary>
        ///     The event arrived.
        /// </summary>
        public event EventHandler<EventArrivedEventArgs> EventArrived;

        /// <summary>
        ///     The cleanup.
        /// </summary>
        private void Cleanup()
        {
            SetEnabled(false);
            foreach (var value in _traceEventInfoCache.Values)
            {
                value.Dispose();
            }

            _traceEventInfoCache = null;
        }

        /// <summary>
        ///     The create event args from event record.
        /// </summary>
        /// <param name="eventRecord">
        ///     The event record.
        /// </param>
        /// <returns>
        ///     The <see cref="EventArrivedEventArgs" />.
        /// </returns>
        private EventArrivedEventArgs CreateEventArgsFromEventRecord(EventRecord eventRecord)
        {
            var eventOpcode = eventRecord.EventHeader.EventDescriptor.Opcode;
            TraceEventInfoWrapper traceEventInfo;
            var shouldDispose = false;

            // Find the event information (schema).
            var index = _traceEventInfoCache.IndexOfKey(eventOpcode);
            if (index >= 0)
            {
                traceEventInfo = _traceEventInfoCache.Values[index];
            }
            else
            {
                traceEventInfo = new TraceEventInfoWrapper(eventRecord);
                try
                {
                    _traceEventInfoCache.Add(eventOpcode, traceEventInfo);
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
        ///     The event record callback.
        /// </summary>
        /// <param name="eventRecord">
        ///     The event record.
        /// </param>
        private void EventRecordCallback([In] ref EventRecord eventRecord)
        {
            var eventArrived = EventArrived;
            if (eventArrived != null)
            {
                var e = CreateEventArgsFromEventRecord(eventRecord);
                eventArrived(this, e);
            }
        }

        /// <summary>
        ///     The load existing event trace properties.
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        /// <exception cref="Win32Exception">
        /// </exception>
        private bool LoadExistingEventTraceProperties()
        {
            const int ErrorWmiInstanceNotFound = 4201;
            _eventTraceProperties = new EventTraceProperties(true);
            var status = NativeMethods.QueryTrace(0, _loggerName, ref _eventTraceProperties);
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
        ///     The process trace in background.
        /// </summary>
        /// <param name="traceInternalHandle">
        ///     The trace handle.
        /// </param>
        /// <exception cref="Win32Exception">
        /// </exception>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private void ProcessTraceInBackground(TraceSafeHandle traceInternalHandle)
        {
            Exception asyncException = null;
            ulong[] array = {traceInternalHandle.UnsafeValue};

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
            var eventArrived = EventArrived;
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
        ///     The set enabled.
        /// </summary>
        /// <param name="value">
        ///     The value.
        /// </param>
        private void SetEnabled(bool value)
        {
            if (_enabled == value)
            {
                return;
            }

            if (value)
            {
                StartTracing();
            }
            else
            {
                StopTracing();
            }

            _enabled = value;
        }

        /// <summary>
        ///     The start.
        /// </summary>
        public void Start()
        {
            SetEnabled(true);
        }

        /// <summary>
        ///     The start tracing.
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

            if (!LoadExistingEventTraceProperties())
            {
                _eventTraceProperties.SetParameters(RealTime, BufferSize, MinBuffers, MaxBuffers, FlushTimerSeconds);

                // Start trace session
                ulong unsafeSessionHandle;
                status = NativeMethods.StartTrace(
                    out unsafeSessionHandle,
                    _loggerName,
                    ref _eventTraceProperties);
                if (status != 0)
                {
                    throw new Win32Exception(status);
                }

                _sessionHandle = new SessionSafeHandle(unsafeSessionHandle, _loggerName);

                var emptyGuid = Guid.Empty;

                var windows7Version = new Version(6, 1, 7600);
                if (Environment.OSVersion.Version.CompareTo(windows7Version) >= 0)
                {
                    const int TimeToWaitForInitialize = 10*1000;
                    var enableParameters = new EnableTraceParameters
                    {
                        Version = 1,
                        EnableProperty = EventEnableProperty.Sid
                    };

                    // ENABLE_TRACE_PARAMETERS_VERSION
                    status = NativeMethods.EnableTraceEx2(
                        unsafeSessionHandle,
                        ref _eventProviderId,
                        1,


                        // controlCode - EVENT_CONTROL_CODE_ENABLE_PROVIDER
                        (byte) Level,
                        MatchAnyKeyword,
                        0,
                        // matchAnyKeyword
                        TimeToWaitForInitialize,
                        ref enableParameters);
                }
                else
                {
                    status = NativeMethods.EnableTraceEx(
                        ref _eventProviderId,
                        ref emptyGuid,
                        // sourceId
                        unsafeSessionHandle,
                        1,
                        // isEnabled
                        (byte) Level,
                        MatchAnyKeyword,
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

            _logFile = new EventTraceLogfile
            {
                LoggerName = _loggerName,
                EventRecordCallback = EventRecordCallback,
                ProcessTraceMode = EventRecord | RealTime
            };

            var unsafeTraceHandle = NativeMethods.OpenTrace(ref _logFile);
            status = Marshal.GetLastWin32Error();
            if (status != 0)
            {
                throw new Win32Exception(status);
            }

            _traceHandle = new TraceSafeHandle(unsafeTraceHandle);

            _processEventsDelgate = ProcessTraceInBackground;
            _asyncResult = _processEventsDelgate.BeginInvoke(_traceHandle, null, _processEventsDelgate);
        }

        /// <summary>
        ///     The stop.
        /// </summary>
        public void Stop()
        {
            SetEnabled(false);
        }

        /// <summary>
        ///     The stop tracing.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands")]
        private void StopTracing()
        {
            if (_traceHandle != null)
            {
                _traceHandle.Dispose();
                _traceHandle = null;
            }

            if (_sessionHandle != null)
            {
                _sessionHandle.Dispose();
                _sessionHandle = null;
            }

            // Once the unmanaged resources got released, end the process trace thread
            // that may throw exception (e.g. OOM).
            if (_processEventsDelgate != null && _asyncResult != null)
            {
                _processEventsDelgate.EndInvoke(_asyncResult);
            }
        }

        /// <summary>
        ///     The process trace delegate.
        /// </summary>
        /// <param name="traceHandle">
        ///     The trace handle.
        /// </param>
        private delegate void ProcessTraceDelegate(TraceSafeHandle traceHandle);

        /// <summary>
        ///     The trace safe handle.
        /// </summary>
        private sealed class TraceSafeHandle : SafeHandle
        {
            /// <summary>
            ///     Initializes a new instance of the <see cref="TraceSafeHandle" /> class.
            /// </summary>
            /// <param name="handle">
            ///     The handle.
            /// </param>
            [SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands")]
            public TraceSafeHandle(ulong handle)
                : base(IntPtr.Zero, true)
            {
                UnsafeValue = handle;
            }

            /// <summary>
            ///     Gets a value indicating whether is invalid.
            /// </summary>
            public override bool IsInvalid => UnsafeValue == 0;

            internal ulong UnsafeValue { get; }

            protected override bool ReleaseHandle()
            {
                return NativeMethods.CloseTrace(UnsafeValue) != 0;
            }
        }

        private sealed class SessionSafeHandle : SafeHandle
        {
            private readonly string _loggerName;

            private readonly ulong _sessionHandle;

            [SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands")]
            public SessionSafeHandle(ulong sessionHandle, string loggerName)
                : base(IntPtr.Zero, true)
            {
                _sessionHandle = sessionHandle;
                _loggerName = loggerName;
            }

            public override bool IsInvalid => _sessionHandle == 0;

            protected override bool ReleaseHandle()
            {
                EventTraceProperties properties;
                return NativeMethods.StopTrace(_sessionHandle, _loggerName, out properties /*as statistics*/)
                       != 0;
            }
        }
    }
}