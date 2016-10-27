// Program.cs
// 
// Copyright (c) 2014-2016, Nino Crudle <nino dot crudele at live dot com>
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
