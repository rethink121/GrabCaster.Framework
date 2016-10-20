using Framework;
using Framework.Base;
using Framework.Contracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.ServiceBus.Messaging;
using System.Runtime.Serialization;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.ServiceBus;
using System.Collections;
using System.Diagnostics;
using Microsoft.WindowsAzure.Storage.Blob;
using Roslyn.Scripting.CSharp;
using System.Collections.Concurrent;
using System.Threading;
using System.Timers;
using Microsoft.WindowsAzure;
using System.Data.SqlClient;
using System.Xml;
using System.Xml.Linq;

namespace Lab
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //Global 
        //List<AssemblyEvent> AssemblyEventListShared;
        List<BubblingEvent> BubblingEventList;
        private void buttonGetTypes_Click(object sender, EventArgs e)
        {
            //******************************************
            //Load all Triggers and Events DLLs in the main dictionary
            //Directory working:
            //Root\\Triggers
            //Root\\Events

            bool result = LoadBubblingEventList();
            MessageBox.Show(result.ToString());
        }

        private void buttonCreateEH_Click(object sender, EventArgs e)
        {
            var manager = new Microsoft.ServiceBus.NamespaceManager("hybridintegrationservices.servicebus.windows.net");
            var description = manager.CreateEventHub("MyEventHub");
        }

        private void buttonTrash_Click(object sender, EventArgs e)
        {
            string[] sss = {"a|a","b|b" };
            string sasd = JsonConvert.SerializeObject(sss);

            provaclasse a = new provaclasse();

            List<Parameter> obj = new List<Parameter>();
            Parameter za = new Parameter("name", Type.GetType("string"), 3);
            Parameter za2 = new Parameter("name2", Type.GetType("string"), "3");

            obj.Add(za);
            obj.Add(za2);

            string s = JsonConvert.SerializeObject(obj);



        }

        private void buttonCreateTrigger_Click(object sender, EventArgs e)
        {
            //Create a Triggers event
            //Directory working:
            //Root\\Bubbling\\Triggers
            Configuration.LoadConfiguration();

            {
                //FILE
                TriggerConfiguation jitgTriggerConfiguration = new TriggerConfiguation();
                Trigger jitgTrigger = new Trigger("{3C62B951-C353-4899-8670-C6687B6EAEFC}",
                    "Test Correlation",
                    "Test Correlation");
                jitgTriggerConfiguration.Trigger = jitgTrigger;
                jitgTriggerConfiguration.TriggerProperties = new List<TriggerProperty>();
                jitgTriggerConfiguration.TriggerProperties.Add(new TriggerProperty("RegexFilePattern",
                    @".(txt|a)"));
                jitgTriggerConfiguration.TriggerProperties.Add(new TriggerProperty("DoneExtensionName", "done"));
                jitgTriggerConfiguration.TriggerProperties.Add(new TriggerProperty("InputDirectory", "c:\\test"));
                jitgTriggerConfiguration.EndPointID = new List<string>();

                jitgTriggerConfiguration.EndPointID.Add("{047B6D1E-A991-4CB1-ACAB-E83C3BDC0097}");

                //Load events
                jitgTriggerConfiguration.Events = new List<Event>();
                Event jitgEvent = new Event("{D438C746-5E75-4D59-B595-8300138FB1EA}", "{D438C746-5E75-4D59-B595-8300138FB1E3}", "write file some where",
                    "write file some where description");
                jitgTriggerConfiguration.Events.Add(jitgEvent);

                jitgTriggerConfiguration.Correlation = new Correlation("Person entered", "Script c#",
                    jitgTriggerConfiguration.Events, jitgTriggerConfiguration.EndPointID);
                jitgTriggerConfiguration.CreateTriggerEvent();
            }



            MessageBox.Show("done");


        }

        private void buttonLoadTriggerevents_Click(object sender, EventArgs e)
        {
            LoadBubblingSettings();

        }

        /// <summary>
        /// Load all active trigger from Bubbling trigger directory
        /// </summary>
        private void LoadBubblingSettings()
        {
            //Load active triggers and event property bag
            //Directory working:
            //Root\\Bubbling\\Triggers
            //Root\\Bubbling\\Events
            //For each trigger... find dll and execute 


            //TRIGGERS***************************************************************************
            //Loop in the directory
            string triggerBubblingDirectory = Configuration.Get_Directory_Bubbling_Triggers();
            Regex regTriggers = new Regex(Configuration.Bubbling_Triggers_Extension);
            var triggerEventsFiles = Directory.GetFiles(triggerBubblingDirectory, "*")
                                    .Where(path => regTriggers.IsMatch(path))
                                    .ToList();
            //For each trigger search for the trigger in event bubbling and set the properties
            foreach (string triggerEventsFile in triggerEventsFiles)
            {
                byte[] triggerEventsByteContent = File.ReadAllBytes(triggerEventsFile);
                TriggerConfiguation jitgTriggerConfiguration = JsonConvert.DeserializeObject<TriggerConfiguation>(Encoding.UTF8.GetString(triggerEventsByteContent));


                //look for the trigger in the event bubbling list
                BubblingEvent jitgBubblingEvent = BubblingEventList.Find(property => property.ID == jitgTriggerConfiguration.Trigger.ID);
                //Got it?
                if (jitgBubblingEvent != null)
                {
                    jitgBubblingEvent.isActive = true;
                    jitgBubblingEvent.Events = jitgTriggerConfiguration.Events;
                    //Yes, so set all the properties
                    foreach (TriggerProperty jitgTriggerProperty in jitgTriggerConfiguration.TriggerProperties)
                    {
                        Property jitgProperties = jitgBubblingEvent.Properties.Find(property => property.Name == jitgTriggerProperty.Name);
                        if (jitgProperties != null)
                            jitgProperties.Value = jitgTriggerProperty.Value;
                    }
                }


            }


            //EVENTS******************************************************************************
            //Loop in the directory
            string eventsBubblingDirectory = Configuration.Get_Directory_Bubbling_Events();
            Regex regEvents = new Regex(Configuration.Bubbling_Events_Extension);
            var propertyEventsFiles = Directory.GetFiles(eventsBubblingDirectory, "*")
                                    .Where(path => regEvents.IsMatch(path))
                                    .ToList();
            //For each trigger search for the trigger in event bubbling and set the properties
            foreach (string propertyEventsFile in propertyEventsFiles)
            {
                byte[] propertyEventsByteContent = File.ReadAllBytes(propertyEventsFile);
                EventConfiguration jitgEventConfiguration = JsonConvert.DeserializeObject<EventConfiguration>(Encoding.UTF8.GetString(propertyEventsByteContent));


                //look for the trigger in the event bubbling list
                BubblingEvent jitgBubblingEvent = BubblingEventList.Find(property => property.ID == jitgEventConfiguration.Event.IDComponent);
                //Got it?
                if (jitgBubblingEvent != null)
                {
                    jitgBubblingEvent.isActive = true;
                    jitgBubblingEvent.Events = jitgBubblingEvent.Events;
                    //Yes, so set all the properties
                    foreach (EventProperty jitgEventProperty in jitgEventConfiguration.EventProperties)
                    {
                        Property jitgProperties = jitgBubblingEvent.Properties.Find(property => property.Name == jitgEventProperty.Name);
                        if (jitgProperties != null)
                            jitgProperties.Value = jitgEventProperty.Value;
                    }
                }


            }
            LoadBubblingTriggerEventPolling();
            MessageBox.Show("done");

        }
        //private void watchTriggerEvents()
        //{
        //    FileSystemWatcher watcher = new FileSystemWatcher();
        //    watcher.Path = path;
        //    watcher.NotifyFilter = NotifyFilters.LastWrite;
        //    watcher.Filter = "*.*";
        //    watcher.Changed += new FileSystemEventHandler(OnChanged);
        //    watcher.EnableRaisingEvents = true;
        //}
        //private void OnChanged(object source, FileSystemEventArgs e)
        //{
        //    //Copies file to another directory.
        //}

        private bool LoadBubblingEventList()
        {
            //******************************************
            //Load all Triggers and Events DLLs in the main dictionary
            //Directory working:
            //Root\\Triggers
            //Root\\Events

            BubblingEventList = new List<BubblingEvent>();


            //Load triggers bubbling path
            string triggersDirectory = Configuration.Get_Directory_Triggers();
            Regex regTriggers = new Regex(Configuration.TriggersDLL_Extension);
            var assemblyFilesTriggers = Directory.GetFiles(triggersDirectory, Configuration.TriggersDLL_Extension_LookFor)
                                    .Where(path => regTriggers.IsMatch(path))
                                    .ToList();

            //Load event bubbling path
            string eventsDirectory = Configuration.Get_Directory_Events();
            Regex regEvents = new Regex(Configuration.EventsDLL_Extension);
            var assemblyFilesEvents = Directory.GetFiles(eventsDirectory, Configuration.EventsDLL_Extension_LookFor)
                                    .Where(path => regEvents.IsMatch(path))
                                    .ToList();

            //****************************************************
            //Load Triggers
            //****************************************************
            foreach (string assemblyFile in assemblyFilesTriggers)
            {
                var assmblyClasses = (dynamic)null;
                //Get all classes with Attribute = Event
                Assembly assembly = Assembly.LoadFrom(assemblyFile);
                assmblyClasses = from t in assembly.GetTypes()
                                 let attributes = t.GetCustomAttributes(typeof(TriggerContract), false)
                                 where t.IsClass && attributes != null && attributes.Length > 0
                                 select t;

                foreach (Type assemblyClass in assmblyClasses)
                {
                    BubblingEvent jitgBubblingEvent = null;
                    object[] classAttributes = assemblyClass.GetCustomAttributes(typeof(TriggerContract), true);
                    if (classAttributes.Length > 0)
                    {
                        TriggerContract jitgTrigger = (TriggerContract)classAttributes[0];
                        //Create event bubbling
                        jitgBubblingEvent = new BubblingEvent();
                        jitgBubblingEvent.AssemblyObject = assembly;
                        jitgBubblingEvent.AssemblyFile = assemblyFile;
                        jitgBubblingEvent.BubblingEventType = BubblingEventType.Trigger;
                        jitgBubblingEvent.Description = jitgTrigger.Description;
                        jitgBubblingEvent.EndPointID = new List<string>();
                        jitgBubblingEvent.ID = jitgTrigger.ID;
                        jitgBubblingEvent.Name = jitgTrigger.Name;
                        jitgBubblingEvent.AssemblyClassType = assemblyClass;
                        jitgBubblingEvent.PollingRequired = false;
                        jitgBubblingEvent.Shared = jitgTrigger.Shared;
                    }
                    //TODO optimize to get the properties with attribute directly
                    jitgBubblingEvent.Properties = new List<Property>();
                    foreach (PropertyInfo propertyInfo in assemblyClass.GetProperties())
                    {
                        object[] propertyAttributes = propertyInfo.GetCustomAttributes(typeof(TriggerPropertyContract), true);
                        if (propertyAttributes.Length > 0)
                        {
                            TriggerPropertyContract jitgTriggerProperty = (TriggerPropertyContract)propertyAttributes[0];
                            jitgBubblingEvent.Properties.Add(new Property(jitgTriggerProperty.Name, propertyInfo, propertyInfo.GetType(), null));
                        }

                    }
                    //TODO optimize to get the methods with attribute directly
                    jitgBubblingEvent.Actions = new List<Action>();
                    foreach (MethodInfo MethodInfo in assemblyClass.GetMethods())
                    {
                        object[] methodInfoAttributes = MethodInfo.GetCustomAttributes(typeof(TriggerActionContract), true);
                        if (methodInfoAttributes.Length > 0)
                        {
                            TriggerActionContract jitgTriggerAction = (TriggerActionContract)methodInfoAttributes[0];
                            //Add the method
                            Action jitgAction = new Action(jitgTriggerAction.ID, jitgTriggerAction.Name, jitgTriggerAction.Description, MethodInfo, null);

                            jitgAction.Parameters = new List<Parameter>();


                            ParameterInfo[] parameters = MethodInfo.GetParameters();
                            foreach (ParameterInfo parameter in parameters)
                            {
                                Parameter jitgParameter = new Parameter(parameter.Name, parameter.ParameterType, null);
                            }
                            jitgBubblingEvent.Actions.Add(jitgAction);
                        }
                    }
                    //Add the bubbling event
                    BubblingEventList.Add(jitgBubblingEvent);
                }

                

            }

            //****************************************************
            //Load Events
            //****************************************************
            foreach (string assemblyFile in assemblyFilesEvents)
            {
                var assmblyClasses = (dynamic)null;
                //Get all classes with Attribute = Event
                Assembly assembly = Assembly.LoadFrom(assemblyFile);
                assmblyClasses = from t in assembly.GetTypes()
                                 let attributes = t.GetCustomAttributes(typeof(EventContract), false)
                                 where t.IsClass && attributes != null && attributes.Length > 0
                                 select t;

                foreach (Type assemblyClass in assmblyClasses)
                {
                    BubblingEvent jitgBubblingEvent = null;
                    object[] classAttributes = assemblyClass.GetCustomAttributes(typeof(EventContract), true);
                    if (classAttributes.Length > 0)
                    {
                        EventContract jitgEvent = (EventContract)classAttributes[0];
                        //Create event bubbling
                        jitgBubblingEvent = new BubblingEvent();
                        jitgBubblingEvent.AssemblyFile = assemblyFile;
                        jitgBubblingEvent.AssemblyObject = assembly;
                        jitgBubblingEvent.BubblingEventType = BubblingEventType.Event;
                        jitgBubblingEvent.Description = jitgEvent.Description;
                        jitgBubblingEvent.EndPointID = new List<string>();
                        jitgBubblingEvent.ID = jitgEvent.ID;
                        jitgBubblingEvent.Name = jitgEvent.Name;
                        jitgBubblingEvent.AssemblyClassType = assemblyClass;
                        jitgBubblingEvent.PollingRequired = false;
                        jitgBubblingEvent.Shared = jitgEvent.Shared;
                    }
                    //TODO optimize to get the properties with attribute directly
                    jitgBubblingEvent.Properties = new List<Property>();
                    foreach (PropertyInfo propertyInfo in assemblyClass.GetProperties())
                    {
                        object[] propertyAttributes = propertyInfo.GetCustomAttributes(typeof(EventPropertyContract), true);
                        if (propertyAttributes.Length > 0)
                        {
                            EventPropertyContract jitgEventProperty = (EventPropertyContract)propertyAttributes[0];
                            jitgBubblingEvent.Properties.Add(new Property(jitgEventProperty.Name, propertyInfo, propertyInfo.GetType(), null));
                        }

                    }
                    //TODO optimize to get the methods with attribute directly
                    jitgBubblingEvent.Actions = new List<Action>();
                    foreach (MethodInfo MethodInfo in assemblyClass.GetMethods())
                    {
                        object[] methodInfoAttributes = MethodInfo.GetCustomAttributes(typeof(EventActionContract), true);
                        if (methodInfoAttributes.Length > 0)
                        {
                            EventActionContract jitgEventAction = (EventActionContract)methodInfoAttributes[0];
                            //Add the method
                            Action jitgAction = new Action(jitgEventAction.ID, jitgEventAction.Name, jitgEventAction.Description, MethodInfo, null);

                            jitgAction.Parameters = new List<Parameter>();


                            ParameterInfo[] parameters = MethodInfo.GetParameters();
                            foreach (ParameterInfo parameter in parameters)
                            {
                                Parameter jitgParameter = new Parameter(parameter.Name, parameter.ParameterType, null);
                            }
                            jitgBubblingEvent.Actions.Add(jitgAction);
                        }
                    }

                    //Add the bubbling event
                    BubblingEventList.Add(jitgBubblingEvent);
                }

            }


            //        ////Get all Event properties
            //        //jitgBubblingEvent.Properties = new List<Property>();
            //        //var properties = from p in t.GetProperties()
            //        //                 let attributes = p.GetCustomAttributes(typeof(EventProperty), true)
            //        //                 where attributes != null && attributes.Length > 0
            //        //                 select p; 
            //        jitgBubblingEvent.Properties = new List<Property>();
            //        var properties = from p in t.GetProperties()
            //                         select p;

            //        properties.ToList().ForEach(p =>
            //        {
            //            object[] attrProp = p.GetCustomAttributes(typeof(EventProperty), true);
            //            if (attrProp.Length > 0)
            //            {
            //                EventProperty jitgEventProperty = (EventProperty)attrProp[0];
            //                //Add the property
            //                jitgBubblingEvent.Properties.Add(new Property(jitgEventProperty.Name, p.GetType(), null));
            //            }
            //        });

            //        ////Get all event methods 
            //        //jitgBubblingEvent.Actions = new List<Action>();
            //        //var methods = from p in t.GetMethods()
            //        //                 let attributes = p.GetCustomAttributes(typeof(EventAction), true)
            //        //                 where attributes != null && attributes.Length > 0
            //        //                 select p;
            //        jitgBubblingEvent.Actions = new List<Action>();
            //        var methods = from p in t.GetMethods()
            //                      select p;
            //        methods.ToList().ForEach(method =>
            //        {
            //            object[] actionAttributes = method.GetCustomAttributes(typeof(EventAction), true);
            //            if (actionAttributes.Length > 0)
            //            {
            //                EventAction jitgEventAction = (EventAction)actionAttributes[0];
            //                //Add the method
            //                Action jitgAction = new Framework.Action(jitgEventAction.ID, jitgEventAction.Name, jitgEventAction.Description, null);

            //                jitgAction.Parameters = new List<Parameter>();


            //                ParameterInfo[] parameters = method.GetParameters();
            //                foreach (ParameterInfo parameter in parameters)
            //                {
            //                    Parameter jitgParameter = new Parameter(parameter.Name, parameter.ParameterType, null);
            //                }
            //            }

            //        });

            //        BubblingEventList.Add(jitgBubblingEvent);
            //    }
            //});



            return false ? BubblingEventList.Count == 0 : true;
        }

        List<BubblingEvent> jitgBubblingTriggerEventsPolling = null;
        private void buttonStartTriggers_Click(object sender, EventArgs e)
        {

            ExecuteBubblingTriggerEventPolling();
        }

        private void LoadBubblingTriggerEventPolling()
        {
            jitgBubblingTriggerEventsPolling = (from jitgBubblingTriggerEvent in BubblingEventList
                                                where jitgBubblingTriggerEvent.isActive == true
                                                && jitgBubblingTriggerEvent.BubblingEventType == BubblingEventType.Trigger
                                                select jitgBubblingTriggerEvent).ToList();
        }

        private void ExecuteBubblingTriggerEventPolling()
        {
            
            foreach(BubblingEvent BubblingTriggerEvent in jitgBubblingTriggerEventsPolling)
            {
                ExecuteTrigger(BubblingTriggerEvent);
            }
        
        }

        /// <summary>
        /// Execute a trigger and if the Execute method return != null then it set all return value in a action and excute the action
        /// </summary>
        /// <param name="BubblingTriggerEvent"></param>
        /// <returns></returns>
        private void ExecuteTrigger(BubblingEvent BubblingTriggerEvent)
        {
            //In the first execute the main Execute method
            Action jitgAction = BubblingTriggerEvent.Actions.Find(prop => prop.AssemblyMethodInfo.Name == "Execute");

            if(jitgAction == null)
            {
                return;
            }
            //Invoke the method and set the result
            jitgAction.ReturnValue = jitgAction.AssemblyMethodInfo.Invoke(BubblingTriggerEvent.AssemblyClassType, null).ToString();
            //if result of Execute != null then execute the other method too
            if(jitgAction.ReturnValue!= null)
            {
                List<Action> ActionCustoms = (from action in BubblingTriggerEvent.Actions
                                                    where action.AssemblyMethodInfo.Name != "Execute"
                                                    select action).ToList();
                //Execute the other method
                foreach (Action _jitgAction in ActionCustoms)
                {
                    //result != null so set the result value
                    _jitgAction.ReturnValue = jitgAction.AssemblyMethodInfo.Invoke(BubblingTriggerEvent.AssemblyClassType, null).ToString();
                }
                //Try to Execute locally
                ExecuteBubblingActionEvent(BubblingTriggerEvent,true,Configuration.GetID());
                //Send to EH
               // EventUpStream.sendMessages(BubblingTriggerEvent);

            }

        }
        /// <summary>
        ///Execute all the action correlate to the trigger
        ///it send the trigger event
        /// </summary>
        /// <param name="BubblingTriggerEvent"></param>
        /// <param name="internalCall"> if is an internal call or arrived from EH (Performances)</param>
        public void ExecuteBubblingActionEvent(BubblingEvent BubblingTriggerEvent, bool internalCall,string SenderEndpointID)
        {
            //Check local endpoint execution
            string localEndPoint = Configuration.GetID();
            List<string> EndPointsList = BubblingTriggerEvent.EndPointID.FindAll(prop => prop == localEndPoint);

            foreach (string endPoint in EndPointsList)
            {
                //this is a message already sent by the current 
                if (!internalCall && SenderEndpointID == localEndPoint)
                    continue;
                foreach (Event jitgEvent in BubblingTriggerEvent.Events)
                {
                    //Ecetute the event
                    //Lokk for the Event
                    List<BubblingEvent> jitgBubblingEventsToExecute = (from jitgBubblingTriggerEvent in BubblingEventList
                                                                           where jitgBubblingTriggerEvent.ID == jitgEvent.IDComponent
                                                                           && jitgBubblingTriggerEvent.BubblingEventType == BubblingEventType.Event
                                                                           select jitgBubblingTriggerEvent).ToList();
                    foreach(BubblingEvent jitgBubblingEvent in jitgBubblingEventsToExecute)
                    {
                        //Assign all propertyies value trigger to event and pass all parameter to event actions and execute
                        foreach(Property jitgPropertyEvent in jitgBubblingEvent.Properties)
                        {
                            //Look if exist the property in the trigger, if exist then copy value
                            Property jitgPropertyTriger = BubblingTriggerEvent.Properties.Find(prop => prop.Name == jitgPropertyEvent.Name);
                            if(jitgPropertyTriger!=null)
                            {
                                jitgPropertyEvent.Value = jitgPropertyTriger.Value;
                            }
                        }

                        //for each action with the same name the pass the parameter and execute the event
                        foreach (Action jitgActionEvent in jitgBubblingEvent.Actions)
                        {
                            //Look if exist the property in the trigger, if exist then copy value
                            Action jitgActionTriger = BubblingTriggerEvent.Actions.Find(prop => prop.Name == jitgActionEvent.Name);
                            if (jitgActionTriger != null)
                            {
                                jitgActionEvent.AssemblyMethodInfo.Invoke(jitgBubblingEvent.AssemblyClassType, new object[] {jitgActionTriger.ReturnValue});
                            }
                        }

                    }
                }
            }

        }

        private void buttonStartDownStream_Click(object sender, EventArgs e)
        {
           // provaclasse.valore = 10;
           // EventsDownstream.Run();
            
            MessageBox.Show("done");
        }

        private void buttonCreateEventPropertyBag_Click(object sender, EventArgs e)
        {
            //Create a Triggers event
            //Directory working:
            //Root\\Bubbling\\Triggers

            EventConfiguration jitgEventConfiguration = new EventConfiguration();

            Event jitgEvent = new Event("{D438C746-5E75-4D59-B595-8300138FB1EA}", "{D438C746-5E75-4D59-B595-8300138FB1E3}",
                                                        "write file some where",
                                                        "write file some where description");

            jitgEventConfiguration.Event = jitgEvent;
            jitgEventConfiguration.EventProperties = new List<EventProperty>();
            jitgEventConfiguration.EventProperties.Add(new EventProperty("OutputDirectory", "c:\\test\\output"));
            jitgEventConfiguration.CreatePropertyEvent();

            MessageBox.Show("done");


        }

        private void buttonstorage_Click(object sender, EventArgs e)
        {



        }

        private void buttonConfiguration_Click(object sender, EventArgs e)
        {

            ConfigurationStorage configurationStorage = new ConfigurationStorage();
            configurationStorage.Get_EH_Name = "jitg";
            configurationStorage.Get_EH_ReceiveConnectionString = "Endpoint=sb://hybridintegrationservices.servicebus.windows.net/;SharedAccessKeyName=Receive;SharedAccessKey=bKB4wB0ZdcYo0iRsrf7NXd9Et5MbBBlWnqfIwQhjMIU=";
            configurationStorage.Get_EH_SendConnectionString = "Endpoint=sb://hybridintegrationservices.servicebus.windows.net/;SharedAccessKeyName=Send;SharedAccessKey=lvIy0WqMKlGdBh+DyBwoD1yDSj7VRV1PlepRKdenzuM=";
            configurationStorage.Get_EH_StorageAccountKey = "HHCLXqg8RnpD1P1mzYwPUxXuQyIcTH59UB9iHJKc2C6R5pAk1+v4uQcIxbDQlH6OoOcTA+Tii151LYL+U0m2ow==";
            configurationStorage.Get_EH_StorageAccountName = "jitg";
            configurationStorage.Get_PersistMessage = false;
            configurationStorage.GetDescription = " For sap interface in departement B";
            configurationStorage.GetID = "{047B6D1E-A991-4CB1-ACAB-E83C3BDC0097}";
            configurationStorage.GetNAME = "Node SAP 101";
            configurationStorage.GetLocalStorageConnectionString = "c:\\\\Storage";
            configurationStorage.GetLocalStorageProvider = "FILE";
            //TODO manage this var with the conf file, no time now
            configurationStorage.GetLoggingState = true;
            configurationStorage.GetPollingTime = 2000;
            configurationStorage.Get_ResetWaitTime = 5000;
            configurationStorage.Get_Directory_Root = "-MANAGED BY THE SERVICE-";
            configurationStorage.Get_Directory_ = "-MANAGED BY THE SERVICE-";
 
            Configuration.SaveConfgurtation(configurationStorage);
            Configuration.LoadConfiguration();

            MessageBox.Show("done");
        }

        private void buttonSerialization_Click(object sender, EventArgs e)
        {
            byte[] b = File.ReadAllBytes("c:\\test\\New Text Document.done");

            bodymessage bmes = new bodymessage();
            bmes.a = "body";
            bmes.valueReturn = b;
            
            message mes = new message();
            mes.a = "message";
            mes.valueReturn = b;
            mes.bodymessages = new List<bodymessage>();
            mes.bodymessages.Add(bmes);
                        object o = mes;

            var sero = JsonConvert.SerializeObject(mes);

            EventData data1 = new EventData(Encoding.UTF8.GetBytes(sero));

            message jitgTriggerEvent = JsonConvert.DeserializeObject<message>(Encoding.UTF8.GetString(data1.GetBytes()));

            //byte[] 


            //byte[] ob = ObjectToByteArray(o);

            ///EventData data = new EventData(ob);

            //byte[] ob2  = data.GetBytes();
            //object mes2 = ByteArrayToObject(ob2);
 
            //message jitgBubblingEvent = JsonConvert.DeserializeObject<message>(Encoding.UTF8.GetString(data.GetBytes()));

            MessageBox.Show("");


        }

        private byte[] ObjectToByteArray(Object obj)
        {
            if (obj == null)
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, obj);
            return ms.ToArray();
        }

        private Object ByteArrayToObject(byte[] arrBytes)
        {
            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            Object obj = (Object)binForm.Deserialize(memStream);
            return obj;
        }
        FileSystemWatcher fswEventFolder = new FileSystemWatcher();
        private void buttonFileSW_Click(object sender, EventArgs e)
        {

            fswEventFolder.Path = @"C:\test\Root";
            fswEventFolder.NotifyFilter = NotifyFilters.FileName| NotifyFilters.Size ;
            fswEventFolder.Filter = "*.*";

            fswEventFolder.EnableRaisingEvents = true;
            fswEventFolder.IncludeSubdirectories = true;

            fswEventFolder.Changed += new FileSystemEventHandler(EventFolderChanged);
            fswEventFolder.Created += new FileSystemEventHandler(EventFolderCreated);
            fswEventFolder.Deleted += new FileSystemEventHandler(EventFolderDeleted);
            //fswEventFolder.Renamed += new FileSystemEventHandler(EventFolderRenamed);
            MessageBox.Show("RUN");

        }
        string extensionsEvents = @".dll|.evn|.trg";
        
       
        private void EventFolderChanged(object source, FileSystemEventArgs e)
        {
                if (Regex.IsMatch(Path.GetExtension(e.Name), extensionsEvents, RegexOptions.IgnoreCase))
                {
                    MessageBox.Show("Changed-" + e.FullPath);
                }
 

        }
        private void EventFolderCreated(object source, FileSystemEventArgs e)
        {
            if (Regex.IsMatch(Path.GetExtension(e.Name), extensionsEvents, RegexOptions.IgnoreCase))
            {
                MessageBox.Show("Created-" + e.FullPath);
            }
        }
        private void EventFolderDeleted(object source, FileSystemEventArgs e)
        {
            if (Regex.IsMatch(Path.GetExtension(e.Name), extensionsEvents, RegexOptions.IgnoreCase))
            {
                MessageBox.Show("Deleted-" + e.FullPath);
            }
        }
        private void EventFolderRenamed(object source, FileSystemEventArgs e)
        {
            if (Regex.IsMatch(Path.GetExtension(e.Name), extensionsEvents, RegexOptions.IgnoreCase))
            {
                MessageBox.Show("Renamed-" + e.FullPath);
            }
        }

        private void buttonCheckEH_Click(object sender, EventArgs e)
        {

            // Create namespace client

            //string eventHubConnectionString = Configuration.Get_EH_ReceiveConnectionString();
            string eventHubConnectionString = "Endpoint=sb://hybridintegrationservices.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=iJsdBeZlhM/z5rokoae69HKQ0z8BF7NiH9nsq2t6IVA=";

            NamespaceManager namespaceManager = NamespaceManager.CreateFromConnectionString(eventHubConnectionString);

            IEnumerable<EventHubDescription> eventHubDescriptions = namespaceManager.GetEventHubs();

            foreach(EventHubDescription eventHubDescription in eventHubDescriptions)
            {
                MessageBox.Show(eventHubDescription.Path);
            }


        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                if (!Directory.GetFiles("c:\\test", "*.txt").Any())
                    return;
                    byte[] data = null;
                //todo lerrore e qui
                string file = Directory.GetFiles("c:\\test", "*.txt").First() ;
                data = File.ReadAllBytes(file);
                File.Move(file, Path.ChangeExtension(file, "done"));
            }
            catch(Exception ex) { MessageBox.Show(ex.Message);   }
        }

        private void EventLog_Click(object sender, EventArgs e)
        {
            EventLog myNewLog = new EventLog();
            myNewLog.Log = "Application";

            myNewLog.EntryWritten += new EntryWrittenEventHandler(MyOnEntryWritten);
            myNewLog.EnableRaisingEvents = true;
            MessageBox.Show("done");

        }

        public static void MyOnEntryWritten(object source, EntryWrittenEventArgs e)
        {
            MessageBox.Show(e.Entry.Message);
        }

        private void button2_Click(object sender, EventArgs e)
        {

            for(int i = 0;i<10;i++)
            {
                //Thread myNewThread = new Thread(() => insertEvents(i));
                //myNewThread.Start();
            }

            MessageBox.Show(" done");


        }
        public void insertEvents(int i2)
        {
            EventLog myNewLog = new EventLog();
            myNewLog.Source = "Application";
            int num = int.Parse(textBoxNum.Text);
            for (int i = 0; i < num; i++)
            {
                myNewLog.WriteEntry("test");

            }
            MessageBox.Show(i2.ToString() + " done");
        }
        private void buttonEVTrigger_Click(object sender, EventArgs e)
        {
            Configuration.LoadConfiguration();

            //Create a Triggers event
            //Directory working:
            //Root\\Bubbling\\Triggers

            TriggerConfiguation jitgTriggerConfiguration = new TriggerConfiguation();

            Trigger jitgTrigger = new Trigger("{843008B6-F4E1-4A29-8082-BDC111EA0E99}",
                                                        "Send EV message",
                                                        "Send EV message description");

            jitgTriggerConfiguration.Trigger = jitgTrigger;
            jitgTriggerConfiguration.TriggerProperties = new List<TriggerProperty>();
            jitgTriggerConfiguration.TriggerProperties.Add(new TriggerProperty("EventLog", "Application"));
            jitgTriggerConfiguration.EndPointID = new List<string>();

            jitgTriggerConfiguration.EndPointID.Add("{047B6D1E-A991-4CB1-ACAB-E83C3BDC0097}");

            //Load events
            jitgTriggerConfiguration.Events = new List<Event>();
            Event jitgEvent = new Event("{D438C746-5E75-4D59-B595-8300138FB1EA}", "{D438C746-5E75-4D59-B595-8300138FB1Es}", "write file some where", "write file some where description");
            jitgTriggerConfiguration.Events.Add(jitgEvent);
            jitgTriggerConfiguration.CreateTriggerEvent();

            MessageBox.Show("done");

        }



        private void button3_Click(object sender, EventArgs e)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=jitg;AccountKey=HHCLXqg8RnpD1P1mzYwPUxXuQyIcTH59UB9iHJKc2C6R5pAk1+v4uQcIxbDQlH6OoOcTA+Tii151LYL+U0m2ow==");
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            // Retrieve a reference to a container. 
            CloudBlobContainer container = blobClient.GetContainerReference("mycontainer");

            // Create the container if it doesn't already exist.
            container.CreateIfNotExists();
            container.SetPermissions( new BlobContainerPermissions
            {        PublicAccess =    BlobContainerPublicAccessType.Blob    });

            CloudBlockBlob blockBlob = container.GetBlockBlobReference("mytest");
            byte[] bta = File.ReadAllBytes(@"c:\\test\\New Text Document.done");

            blockBlob.UploadFromByteArray(bta,0,bta.Length);

            //Receive......
            CloudBlockBlob blockBlobRec = container.GetBlockBlobReference("mytests");

            blockBlobRec.FetchAttributes();
            long fileByteLength = blockBlobRec.Properties.Length;
            byte[] fileContent = new byte[fileByteLength];
            for (int i = 0; i < fileByteLength; i++)
            {
                fileContent[i] = 0x20;
            }
            blockBlobRec.DownloadToByteArray(fileContent, 0);

   

            File.WriteAllBytes(@"C:\test\aaa.txt", fileContent);


        }
        public class HostContext
        {
            public object DataContext;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var hostObject = new HostContext();
            hostObject.DataContext = false;

            ScriptEngine RoslynEngine = new ScriptEngine();
            Roslyn.Scripting.Session Session;

            RoslynEngine = new ScriptEngine();
            Session = RoslynEngine.CreateSession(hostObject);
            if (hostObject != null)
                Session.AddReference(hostObject.GetType().Assembly);
            Session.ImportNamespace("System");
            Session.ImportNamespace("System.Windows.Forms");
            Session.AddReference(@"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\System.Windows.Forms.dll");
            string s = "bool b = (bool)DataContext == true;b";
            
            var b = Session.Execute(s);
            MessageBox.Show(b.ToString());

            //http://stackoverflow.com/questions/24733556/pass-object-instance-to-roslyn-scriptengine

        }

        private void buttonPerfLog_Click(object sender, EventArgs e)
        {

        }

private void buttonQueue_Click(object sender, EventArgs e)
{

    string connectionString = "Endpoint=sb://azureofthings.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=uxlUw5J2YSUj5EBlh8FFv6R0RKLNUx6gfem2VsvgIKw=";
    var namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);

    if (!namespaceManager.QueueExists("TestQueue"))
    {
        namespaceManager.CreateQueue("TestQueue");
    }
    QueueClient Client =
        QueueClient.CreateFromConnectionString(connectionString, "TestQueue");

    //If you want to use properties
    //message.Properties["TestProperty"] = "TestValue";
    //message.Properties["Message number"] = i;
    byte[] b = Encoding.UTF8.GetBytes("Text to Send");
    Client.Send(new BrokeredMessage(b));

    ////Reciving***********************************
    //Callback lambda approach, faster and easy
    // Configure the callback options
    OnMessageOptions options = new OnMessageOptions();
    options.AutoComplete = false;
    options.AutoRenewTimeout = TimeSpan.FromMinutes(1);

    // Callback to handle received messages
    Client.OnMessage((message) =>
    {
        try
        {
            // Process message from queue, here to change the type for custom class 
            string bodymessage = message.GetBody<string>();
                  
            string propertymessage =  message.Properties["TestProperty"].ToString();
            // Remove message from queue
            message.Complete();
            MessageBox.Show(bodymessage);
        }
        catch (Exception)
        {
            // Indicates a problem, unlock message in queue
            message.Abandon();
        }
    }, options);
}

private void button5_Click(object sender, EventArgs e)
{
    string connectionString = "Endpoint=sb://azureofthings.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=uxlUw5J2YSUj5EBlh8FFv6R0RKLNUx6gfem2VsvgIKw=";

    var namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);

    if (!namespaceManager.TopicExists("TestTopic"))
    {
        namespaceManager.CreateTopic("TestTopic");
    }
    TopicClient Client =
        TopicClient.CreateFromConnectionString(connectionString, "TestTopic");

    byte[] b = Encoding.UTF8.GetBytes("Ti Amo");
    BrokeredMessage message =  new BrokeredMessage(b);
    message.Properties["MessageNumber"] = 4;
    //message.Properties["Message number"] = i;


    Client.Send(message);
    MessageBox.Show("Sent");
    //Reciving***********************************

    // Here to create a subscription 
    //Create a a filtered subscription
    SqlFilter highMessagesFilter =
        new SqlFilter("MessageNumber > 3");

    if (!namespaceManager.SubscriptionExists("TestTopic", "HighMessages"))
    {
        namespaceManager.CreateSubscription("TestTopic",
            "HighMessages",
            highMessagesFilter);
    }

    SubscriptionClient subscriptionClientHigh =
        SubscriptionClient.CreateFromConnectionString
                (connectionString, "TestTopic", "HighMessages");

    // Configure the callback options
    OnMessageOptions options = new OnMessageOptions();
    options.AutoComplete = false;
    options.AutoRenewTimeout = TimeSpan.FromMinutes(1);

    subscriptionClientHigh.OnMessage((brokerMessage) =>
    {
        try
        {
            // Process message from subscription
            string bodymessage = brokerMessage.GetBody<string>();
            string propertymessage =  message.Properties["TestProperty"].ToString();

            // Remove message from queue
            brokerMessage.Complete();
            MessageBox.Show(bodymessage);
        }
        catch (Exception)
        {
            // Indicates a problem, unlock message in subscription
            message.Abandon();
        }
    }, options);
}

        private void buttonSQL_Click(object sender, EventArgs e)
        {
            string selectCommand = "EXEC CallAllarmSensors";
            using (SqlConnection myConnection = new SqlConnection(@"Data Source=NINOC-LAPTOP\SQLEXPRESS;Initial Catalog=BizTalkSummitDemoDB;Integrated Security=True"))
            {

                SqlCommand SelectCommand = new SqlCommand(selectCommand, myConnection);
                myConnection.Open();
                XmlReader readerResult = SelectCommand.ExecuteXmlReader();
                XmlDocument xdoc = new XmlDocument();
                xdoc.Load(readerResult);
                MessageBox.Show(xdoc.OuterXml);
                myConnection.Close();
            }
        }

        private static PerformanceCounter counters_EventPublishingIncomingRate;
        private static PerformanceCounter counters_EventDeliveringIncomingRate;

        private void button7_Click(object sender, EventArgs e)
        {


            if (PerformanceCounterCategory.Exists("MyCategory"))
            {
                PerformanceCounterCategory.Delete("MyCategory");
            }



            PerformanceCounter _TotalOperations = new PerformanceCounter();
            _TotalOperations.CategoryName = "MyCategory";
            _TotalOperations.CounterName = "# operations executed";
            _TotalOperations.MachineName = ".";
            _TotalOperations.ReadOnly = false;
            _TotalOperations.RawValue = 0;

            counters_EventDeliveringIncomingRate = new PerformanceCounter("MyCategory",
                "s",
                true);

            for (int i = 0; i < 1000; i++)
            {
                Thread.Sleep(100);
                counters_EventPublishingIncomingRate.Increment();
                counters_EventDeliveringIncomingRate.Increment();

            }
            try
            {
                PerformanceCounterCategory.Delete("jitGate");
            }
            catch { }

            if (!PerformanceCounterCategory.Exists("jitGate"))
            {

                CounterCreationDataCollection counterDataCollection = new CounterCreationDataCollection();

                // Add the counter.
                CounterCreationData data1 = new CounterCreationData();
                data1.CounterType = PerformanceCounterType.AverageCount64;
                data1.CounterName = "Event_publishing_incoming_rate";
                counterDataCollection.Add(data1);

                // Add the base counter.
                CounterCreationData data2 = new CounterCreationData();
                data2.CounterType = PerformanceCounterType.AverageBase;
                data2.CounterName = "Event_delivering_incoming_rate";
                counterDataCollection.Add(data2);

                // Create the category.
                PerformanceCounterCategory.Create("jitGate",
                    "jitGate Counters",
                    PerformanceCounterCategoryType.SingleInstance, counterDataCollection);

            }


            //Create counters

            counters_EventPublishingIncomingRate = new PerformanceCounter("Event publishing incoming rate",
                "Event publishing incoming rate",
                false);


            counters_EventDeliveringIncomingRate = new PerformanceCounter("Event delivering incoming rate",
                "Event delivering incoming rate",
                false);

            for (int i = 0; i < 1000; i++)
            {
                Thread.Sleep(100);
                counters_EventPublishingIncomingRate.Increment();
                counters_EventDeliveringIncomingRate.Increment();

            }
        }
    }

    [Serializable]
    public class message
    {

        public string a { get; set; }

        public object valueReturn { get; set; }
        public List<bodymessage> bodymessages { get; set; }
    }

    [Serializable]
    public class bodymessage
    {
  
        public string a { get; set; }

        public object valueReturn { get; set; }

    }

}
