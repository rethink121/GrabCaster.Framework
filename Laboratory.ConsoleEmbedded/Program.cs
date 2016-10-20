// --------------------------------------------------------------------------------------------------
// <copyright file = "Program.cs" company="GrabCaster Ltd">
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
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using GrabCaster.Framework.Base;
using GrabCaster.Framework.Contracts;
using GrabCaster.Framework.Contracts.Bubbling;
using GrabCaster.Framework.Contracts.Serialization;
using GrabCaster.Framework.FileTrigger;

namespace GrabCaster.Laboratory.ConsoleEmbedded
{
    using System.Text;
    using System.Threading.Tasks;
    using System.Collections.Concurrent;
    using System.Threading;
    using GrabCaster.Framework.Contracts.Events;
    using GrabCaster.Framework.Contracts.Globals;
    using GrabCaster.Framework.Library;
    class Program
    {
        /// <summary>
        /// The set event action event embedded.
        /// </summary>
        private static Embedded.SetEventActionEventEmbedded setEventActionEventEmbedded;


        /// <summary>
        /// The main.
        /// </summary>
        /// <param name="args">
        /// The args.
        /// </param>
        static void Main(string[] args)
        {
            //*********************************************************************
            //Sample executing from configuration file
            //setEventActionEventEmbedded = EventReceivedFromEmbedded;
            //Embedded.setEventActionEventEmbedded = setEventActionEventEmbedded;
            //Thread t = new Thread(start);
            //t.Start();
            //byte[] content = EncodingDecoding.EncodingString2Bytes("Test content string");
            //GrabCaster.Framework.Library.Embedded.ExecuteTrigger(
            //        "{94CE145A-46B6-4C5A-973A-05B2FB57504A}",
            //        "{3C62B951-C353-4899-8670-C6687B6EAEFC}",
            //        content);

            //-------------------------------------------------------
            //Overriding properties
            //Single call test
            //*********************************************************************

            //while (true)
            //{
            //    stopwatchSingle.Restart();
            //    byte[] value = GrabCaster.Framework.Library.Embedded.ExecuteEmbeddedTrigger(triggerEmbeddedBag);
            //    stopwatchSingle.Stop();
            //    Console.WriteLine($"End Test... {stopwatchSingle.ElapsedMilliseconds}");
            //    Console.WriteLine("********************************************************");
            //    Thread.Sleep(500);
            //}
            //*********************************************************************

            //Multi call test
            //*********************************************************************
            //while (true)
            //{
            //    stopwatchSingle.Restart();

            //    byte[] b = GrabCaster.Framework.Library.Embedded.ExecuteEmbeddedTrigger(triggerEmbeddedBag);
            //    stopwatchSingle.Stop();
            //    if(stopwatchSingle.ElapsedMilliseconds==2)
            //    Console.WriteLine($"{stopwatchSingle.ElapsedMilliseconds} milliseconds");

            //}
            //return;

            Console.WriteLine("Start Test...");
            GrabCaster.Framework.Library.Embedded.StartMinimalEngine();

            //One run
            {
                //Initialize trigger


                TriggerEmbeddedBag triggerEmbeddedBag = 
                    GrabCaster.Framework.Library.Embedded.InitializeEmbeddedTrigger(
                        "{DF3F9F3F-938F-43F7-AE39-2DA5F9C1BD9E}",
                        "{306DE168-1CEF-4D29-B280-225B5D0D76FD}");

                string guid = Guid.NewGuid().ToString();
                List<Property> properties = new List<Property>();
                properties.Add(new Property("To", "To", null, typeof(string), guid));
                properties.Add(new Property("From", "From", null, typeof(string), "nino.crudele@live.com"));
                properties.Add(new Property("input", "input", null, typeof(string), "/auth"));
                properties.Add(new Property("Message", "ConnectionString", null, typeof(string),
                    "Data Source=.;Initial Catalog=Demo;Integrated Security=True"));

                triggerEmbeddedBag.Properties = properties;

                long calls = 0;
                string ret = "";

                long totalCycles = 0;
                double totalElapsed = 0;
                double avgElapsedSeconds = 0;
                double minimunElapsedSeconds = 0;
                bool threadSafe = false;
                minimunElapsedSeconds = 10000000000000;
                while (true)
                {
                    totalCycles++;
                    long StartingTime = Stopwatch.GetTimestamp();

                    //Task t = new Task(() => ExecuteTestTriggerTest(triggerEmbeddedBag));
                    //t.RunSynchronously();
                    ret = ExecuteTestTriggerTest(triggerEmbeddedBag);


                    long EndingTime = Stopwatch.GetTimestamp();
                    long ElapsedTime = EndingTime - StartingTime;


                    double ElapsedSeconds = ElapsedTime * (1000.0 / Stopwatch.Frequency);


                    totalElapsed = totalElapsed + ElapsedSeconds;
                    avgElapsedSeconds = totalElapsed / totalCycles;

                    if (ElapsedSeconds < minimunElapsedSeconds)
                        minimunElapsedSeconds = ElapsedSeconds;
                    threadSafe = guid == ret;



                    Console.WriteLine($"--------------------------------------------------------------");
                    Console.WriteLine($"Elapsed     :{ElapsedSeconds}");
                    Console.WriteLine($"AVG Elapsed :{avgElapsedSeconds}");
                    Console.WriteLine($"MIN Elapsed :{minimunElapsedSeconds}");
                    if (threadSafe)
                        Console.WriteLine($"Thread Safe OK :[{guid}]-[{ret}]");
                    else
                    {
                        Console.WriteLine($"Thread Safe KO :[{guid}]-[{ret}]");
                        Console.ReadLine();



                    }



                }

            }


        }

        static string ExecuteTestTriggerTest(TriggerEmbeddedBag triggerEmbeddedBag)
        {
                return EncodingDecoding.EncodingBytes2String(GrabCaster.Framework.Library.Embedded.ExecuteEmbeddedTrigger(triggerEmbeddedBag));
        }
        static void start()
        {
            GrabCaster.Framework.Library.Embedded.StartEngine();
        }
        /// <summary>
        /// The event received from embedded.
        /// </summary>
        /// <param name="eventType">
        /// The event type.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        private static void EventReceivedFromEmbedded(IEventType eventType, ActionContext context)
        {

            string stringValue = EncodingDecoding.EncodingBytes2String(eventType.DataContext);
            Console.WriteLine("---------------EVENT RECEIVED FROM EMBEDDED LIBRARY---------------");
            Console.WriteLine(stringValue);
         }
    }
}
