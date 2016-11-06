// SerializationHelper.cs
// 
// BSD 3-Clause License
// 
// Copyright (c) 2014-2016, Nino Crudele <nino dot crudele at live dot com>
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
// 
// * Redistributions of source code must retain the above copyright notice, this
//   list of conditions and the following disclaimer.
// 
// * Redistributions in binary form must reproduce the above copyright notice,
//   this list of conditions and the following disclaimer in the documentation
//   and/or other materials provided with the distribution.
// 
// * Neither the name of the copyright holder nor the names of its
//   contributors may be used to endorse or promote products derived from
//   this software without specific prior written permission.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE
// FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
// DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
// SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
// CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
// OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
// OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
// 
#region Usings

using System;
using System.Collections.Generic;
using GrabCaster.Framework.Contracts.Bubbling;
using GrabCaster.Framework.Contracts.Channels;
using GrabCaster.Framework.Contracts.Configuration;
using GrabCaster.Framework.Contracts.Events;
using GrabCaster.Framework.Contracts.Points;
using GrabCaster.Framework.Contracts.Triggers;
using Newtonsoft.Json;

#endregion

namespace GrabCaster.Framework.Serialization.Xml
{
    /// <summary>
    ///     TODO The serialization helper.
    /// </summary>
    public static class SerializationHelper
    {
        /// <summary>
        ///     TODO The crete json trigger configuration template.
        /// </summary>
        /// <param name="triggerObject">
        ///     TODO The bubbling event.
        /// </param>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public static string CreteJsonTriggerConfigurationTemplate(ITriggerAssembly triggerObject)
        {
            var eventCorrelationTemplate = new Event(
                "{Event component ID to execute if Correlation = true}",
                "{Configuration ID to execute if Correlation = true}",
                "Event Name Sample",
                "Event Description Sample");
            try
            {
                var triggerConfiguration = new TriggerConfiguration();
                triggerConfiguration.Trigger = new Trigger(
                    triggerObject.Id,
                    Guid.NewGuid().ToString(),
                    triggerObject.Name,
                    triggerObject.Description);
                triggerConfiguration.Trigger.TriggerProperties = new List<TriggerProperty>();
                foreach (var Property in triggerObject.Properties)
                {
                    if (Property.Value.Name != "DataContext")
                    {
                        var triggerProperty = new TriggerProperty(Property.Value.Name, "Value to set");
                        triggerConfiguration.Trigger.TriggerProperties.Add(triggerProperty);
                    }
                }

                triggerConfiguration.Events = new List<Event>();

                // 1*
                var eventTriggerTemplate = new Event(
                    "{Event component ID  Sample to execute}",
                    "{Configuration ID  Sample to execute}",
                    "Event Name Sample",
                    "Event Description Sample");
                eventTriggerTemplate.Channels = new List<Channel>();
                var points = new List<Point>();
                points.Add(new Point("Point ID Sample", "Point Name Sample", "Point Description Sample"));
                eventTriggerTemplate.Channels.Add(
                    new Channel("Channel ID Sample", "Channel Name Sample", "Channel Description Sample", points));

                eventCorrelationTemplate.Channels = new List<Channel>();
                eventCorrelationTemplate.Channels.Add(
                    new Channel("Channel ID Sample", "Channel Name Sample", "Channel Description Sample", points));

                triggerConfiguration.Events.Add(eventTriggerTemplate);

                var events = new List<Event>();
                events.Add(eventCorrelationTemplate);
                eventTriggerTemplate.Correlation = new Correlation("Correlation Name Sample", "C# script", events);

                var serializedMessage = JsonConvert.SerializeObject(
                    triggerConfiguration,
                    Formatting.Indented,
                    new JsonSerializerSettings {ReferenceLoopHandling = ReferenceLoopHandling.Ignore});

                // string serializedMessage = JsonConvert.SerializeObject(triggerConfiguration);
                return serializedMessage;

                // return "<![CDATA[" + serializedMessage + "]]>";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        ///     TODO The crete json event configuration template.
        /// </summary>
        /// <param name="eventObject">
        ///     TODO The bubbling event.
        /// </param>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public static string CreteJsonEventConfigurationTemplate(IEventAssembly eventObject)
        {
            try
            {
                var eventConfiguration = new EventConfiguration();
                eventConfiguration.Event = new Event(
                    eventObject.Id,
                    "{Configuration ID to execute}",
                    eventObject.Name,
                    eventObject.Description);

                eventConfiguration.Event.EventProperties = new List<EventProperty>();
                foreach (var Property in eventObject.Properties)
                {
                    if (Property.Value.Name != "DataContext")
                    {
                        var eventProperty = new EventProperty(Property.Value.Name, "Value to set");
                        eventConfiguration.Event.EventProperties.Add(eventProperty);
                    }
                }

                var eventCorrelationTemplate = new Event(
                    "{Event component ID to execute if Correlation = true}",
                    "{Configuration ID to execute if Correlation = true}",
                    "EventName",
                    "EventDescription");
                eventCorrelationTemplate.Channels = new List<Channel>();
                var points = new List<Point>();
                points.Add(new Point("Point ID", "Point Name", "Point Description"));
                eventCorrelationTemplate.Channels.Add(
                    new Channel("Channel ID", "Channel Name", "Channel Description", points));

                var events = new List<Event>();
                events.Add(eventCorrelationTemplate);
                eventConfiguration.Event.Channels = new List<Channel>();
                eventConfiguration.Event.Channels.Add(
                    new Channel("Channel ID", "Channel Name", "Channel Description", points));

                eventConfiguration.Event.Correlation = new Correlation("Correlation Name", "C# script", events);

                var serializedMessage = JsonConvert.SerializeObject(
                    eventConfiguration,
                    Formatting.Indented,
                    new JsonSerializerSettings {ReferenceLoopHandling = ReferenceLoopHandling.Ignore});
                return serializedMessage;

                // return "<![CDATA[" + serializedMessage + "]]>";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}