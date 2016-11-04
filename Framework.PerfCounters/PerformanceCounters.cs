// PerformanceCounters.cs
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

using System.Diagnostics;

#endregion

namespace GrabCaster.Framework.PerfCounters
{
    /// <summary>
    ///     The performance counters.
    /// </summary>
    public class PerformanceCounters
    {
        /// <summary>
        ///     Counter for counting total number of operations
        /// </summary>
        private readonly PerformanceCounter counterTotalTriggerCalls;

        /// <summary>
        ///     Initializes a new instance of the <see cref="PerformanceCounters" /> class.
        ///     Creates a new performance counter category "MyCategory" if it does not already exists and adds some counters to it.
        /// </summary>
        public PerformanceCounters()
        {
            if (PerformanceCounterCategory.Exists("GrabCaster"))
            {
                PerformanceCounterCategory.Delete("GrabCaster");
            }

            var counters = new CounterCreationDataCollection();
            // Counter for counting totals: PerformanceCounterType.NumberOfItems32
            var totalTriggerCalls = new CounterCreationData
            {
                CounterName = "# trigger executed",
                CounterHelp = "Total number of triggers executed per second",
                CounterType = PerformanceCounterType.RateOfCountsPerSecond64
            };
            counters.Add(totalTriggerCalls);

            // create new category with the counters above

            //PerformanceCounterCategory.Create("GrabCaster", "GrabCaster counters", counters);


            // create counters to work with
            counterTotalTriggerCalls = new PerformanceCounter();
            counterTotalTriggerCalls.CategoryName = "GrabCaster";
            counterTotalTriggerCalls.CounterName = "# trigger executed";
            counterTotalTriggerCalls.MachineName = ".";
            counterTotalTriggerCalls.ReadOnly = false;
            counterTotalTriggerCalls.RawValue = 0;
        }

        /// <summary>
        ///     Increments counters.
        /// </summary>
        public void DoSomeProcessing()
        {
            // simply increment the counters
            counterTotalTriggerCalls.Increment();
        }
    }
}