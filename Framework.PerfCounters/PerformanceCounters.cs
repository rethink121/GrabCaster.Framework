// --------------------------------------------------------------------------------------------------
// <copyright file = "PerformanceCounters.cs" company="GrabCaster Ltd">
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
namespace GrabCaster.Framework.PerfCounters
{
    using System.Diagnostics;

    using GrabCaster.Framework.Base;

    /// <summary>
    /// The performance counters.
    /// </summary>
    public class PerformanceCounters
    {
        /// <summary>
        ///     Counter for counting total number of operations
        /// </summary>
        private readonly PerformanceCounter counterTotalTriggerCalls;

        /// <summary>
        /// Initializes a new instance of the <see cref="PerformanceCounters"/> class. 
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

            PerformanceCounterCategory.Create("GrabCaster", "GrabCaster counters", counters);


            // create counters to work with
            this.counterTotalTriggerCalls = new PerformanceCounter();
            this.counterTotalTriggerCalls.CategoryName = "GrabCaster";
            this.counterTotalTriggerCalls.CounterName = "# trigger executed";
            this.counterTotalTriggerCalls.MachineName = ".";
            this.counterTotalTriggerCalls.ReadOnly = false;
            this.counterTotalTriggerCalls.RawValue = 0;
        }

        /// <summary>
        /// Increments counters.
        /// </summary>
        public void DoSomeProcessing()
        {
            // simply increment the counters
            this.counterTotalTriggerCalls.Increment();
        }
    }
}