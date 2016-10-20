// --------------------------------------------------------------------------------------------------
// <copyright file = "RESTEventsEngine.cs" company="GrabCaster Ltd">
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

using System.Text.RegularExpressions;

namespace GrabCaster.Framework.Engine
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.ServiceModel.Web;
    using System.Text;
    using System.Xml;

    using GrabCaster.Framework.Base;
    using GrabCaster.Framework.Engine;
    using GrabCaster.Framework.Log;

    public class RestEventsEngine : IRestEventsEngine
    {
        //http://localhost:8000/GrabCaster/Deploy?Configuration=Release&Platform=AnyCpu
        public string Deploy(string configuration, string platform)
        {
            try
            {
                string publishingFolder = Path.Combine(ConfigurationBag.DirectoryDeployment(), ConfigurationBag.DirectoryNamePublishing);

                var regTriggers = new Regex(ConfigurationBag.DeployExtensionLookFor);
                var deployFiles =
                    Directory.GetFiles(publishingFolder, "*.*", SearchOption.AllDirectories)
                      .Where(path => Path.GetExtension(path) == ".trigger" || Path.GetExtension(path) == ".event" || Path.GetExtension(path) == ".component");

                StringBuilder results = new StringBuilder();
                foreach (var file in deployFiles)
                {
                    string projectName = Path.GetFileNameWithoutExtension(publishingFolder + file);
                    string projectType = Path.GetExtension(publishingFolder + file).Replace(".", "");
                    bool resultOk = GrabCaster.Framework.Deployment.Jit.CompilePublishing(projectType, projectName,
                        configuration, platform);
                    if (resultOk)
                    {
                        string message = resultOk ? "without errors" : "with errors";
                        results.AppendLine(
                            $"{projectName}.{projectType} builded {message} check the {projectName}Build.Log file for more information");
                    }
                }

                return results.ToString();


            }
            catch (Exception ex)
            {
                LogEngine.WriteLog(
                    Base.ConfigurationBag.EngineName,
                    $"Error in {MethodBase.GetCurrentMethod().Name}",
                    Constant.LogLevelError,
                    Constant.TaskCategoriesError,
                    ex,
                    Constant.LogLevelError);
                return ex.Message;
            }

        }
        //http://localhost:8000/GrabCaster/SyncPush?ChannelID=*&PointID=*
        public string SyncPush(string channelId, string pointId)
        {
            SyncProvider.SendSyncPush(channelId, pointId);
            return $"Syncronization bag sent to Channel ID {channelId} and Point ID {pointId} - {DateTime.Now}";
        }
        //http://localhost:8000/GrabCaster/SyncPull?ChannelID=*&PointID=*
        public string SyncPull(string channelId, string pointId)
        {
            SyncProvider.SendSyncPull(channelId, pointId);
            return $"Syncronization pull request sent to Channel ID {channelId} and Point ID {pointId} - {DateTime.Now}";
        }

        //http://localhost:8000/GrabCaster/Sync
        public string Sync()
        {
            if(EventsEngine.SyncronizePoint())
             return $"Syncronization completed. - {DateTime.Now}";
            else
            {
                return "Point syncronization failed with errors, check the event viewer and the log.";
            }
        }

        /// <summary>
        /// Refresh the internal bubbling setting
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        /// http://localhost:8000/GrabCaster/RefreshBubblingSetting
        public string RefreshBubblingSetting()
        {
            try
            {
                SyncProvider.RefreshBubblingSetting();
            }
            catch (Exception ex)
            {
                LogEngine.WriteLog(
                    Base.ConfigurationBag.EngineName,
                    $"Error in {MethodBase.GetCurrentMethod().Name}",
                    Constant.LogLevelError,
                    Constant.TaskCategoriesError,
                    ex,
                    Constant.LogLevelError);
                return ex.Message;
            }

            return $"Syncronization Executed at {DateTime.Now}.";
        }


        /// <summary>
        /// Send the all bubbling configuration
        /// </summary>
        /// <param name="channelId">
        /// The Channel ID.
        /// </param>
        /// <param name="pointId">
        /// The Point ID.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public string SyncSendBubblingConfiguration(string channelId, string pointId)
        {
            try
            {
                if (Base.ConfigurationBag.Configuration.DisableExternalEventsStreamEngine)
                {
                    LogEngine.WriteLog(
                        Base.ConfigurationBag.EngineName, 
                        "Warning the Device Provider Interface is disable, the GrabCaster point will be able to work in local mode only.", 
                        Constant.LogLevelWarning, 
                        Constant.TaskCategoriesError, 
                        null, 
                        Constant.LogLevelWarning);

                    return
                        "Warning the Device Provider Interface is disable, the GrabCaster point will be able to work in local mode only.";
                }

                SyncProvider.SyncSendBubblingConfiguration(channelId, pointId);
            }
            catch (Exception ex)
            {
                LogEngine.WriteLog(
                    Base.ConfigurationBag.EngineName, 
                    $"Error in {MethodBase.GetCurrentMethod().Name}", 
                    Constant.LogLevelError, 
                    Constant.TaskCategoriesError, 
                    ex, 
                    Constant.LogLevelError);
                return ex.Message;
            }

            return $"Syncronization Executed at {DateTime.Now}.";
        }


        //TO TEST

        // http://localhost:8000/GrabCaster/SyncSendFileBubblingConfiguration?ChannelID={047B6D1E-A991-4CB1-ACAB-E83C3BDC0097}&PointID={B0A46E60-443C-4E8A-A6ED-7F2CB34CF9E5}&FileName=Demo Get SimpleFile Remote.trg&MessageType=Trigger
        public string SyncSendFileBubblingConfiguration(
            string channelId, 
            string pointId, 
            string fileName, 
            string messageType)
        {
            try
            {
                if (Base.ConfigurationBag.Configuration.DisableExternalEventsStreamEngine)
                {
                    LogEngine.WriteLog(
                        Base.ConfigurationBag.EngineName, 
                        "Warning the Device Provider Interface is disable, the GrabCaster point will be able to work in local mode only.", 
                        Constant.LogLevelWarning, 
                        Constant.TaskCategoriesError, 
                        null, 
                        Constant.LogLevelWarning);

                    return
                        "Warning the Device Provider Interface is disable, the GrabCaster point will be able to work in local mode only.";
                }

                SyncProvider.SyncSendFileBubblingConfiguration(channelId, pointId, fileName, messageType);
            }
            catch (Exception ex)
            {
                LogEngine.WriteLog(
                    Base.ConfigurationBag.EngineName, 
                    $"Error in {MethodBase.GetCurrentMethod().Name}", 
                    Constant.LogLevelError, 
                    Constant.TaskCategoriesError, 
                    ex, 
                    Constant.LogLevelError);
                return ex.Message;
            }

            return $"Syncronization Executed at {DateTime.Now}.";
        }

        // Ask all the bubbling configuration
        // http://localhost:8000/GrabCaster/SyncSendRequestBubblingConfiguration?ChannelID=*&PointID=*
        public string SyncSendRequestBubblingConfiguration(string channelId, string pointId)
        {
            try
            {
                if (Base.ConfigurationBag.Configuration.DisableExternalEventsStreamEngine)
                {
                    LogEngine.WriteLog(
                        Base.ConfigurationBag.EngineName, 
                        "Warning the Device Provider Interface is disable, the GrabCaster point will be able to work in local mode only.", 
                        Constant.LogLevelWarning, 
                        Constant.TaskCategoriesError, 
                        null, 
                        Constant.LogLevelWarning);

                    return
                        "Warning the Device Provider Interface is disable, the GrabCaster point will be able to work in local mode only.";
                }

                SyncProvider.SyncSendRequestBubblingConfiguration(channelId, pointId);
            }
            catch (Exception ex)
            {
                LogEngine.WriteLog(
                    Base.ConfigurationBag.EngineName, 
                    $"Error in {MethodBase.GetCurrentMethod().Name}", 
                    Constant.LogLevelError, 
                    Constant.TaskCategoriesError, 
                    ex, 
                    Constant.LogLevelError);
                return ex.Message;
            }

            return $"Syncronization Executed at {DateTime.Now}.";
        }

        //// Send component
        //// http://localhost:8000/GrabCaster/SyncSendComponent?ChannelID=*&PointID=*&IDComponent={3C62B951-C353-4899-8670-C6687B6EAEFC}
        //public string SyncSendComponent(string channelId, string pointId, string idComponent)
        //{
        //    try
        //    {
        //        if (Base.ConfigurationBag.Configuration.DisableExternalEventsStreamEngine)
        //        {
        //            LogEngine.WriteLog(
        //                Base.ConfigurationBag.EngineName, 
        //                "Warning the Device Provider Interface is disable, the GrabCaster point will be able to work in local mode only.", 
        //                Constant.LogLevelWarning, 
        //                Constant.TaskCategoriesError, 
        //                null, 
        //                Constant.LogLevelWarning);

        //            return
        //                "Warning the Device Provider Interface is disable, the GrabCaster point will be able to work in local mode only.";
        //        }

        //        SyncProvider.SyncSendComponent(channelId, pointId, idComponent);
        //    }
        //    catch (Exception ex)
        //    {
        //        LogEngine.WriteLog(
        //            Base.ConfigurationBag.EngineName, 
        //            $"Error in {MethodBase.GetCurrentMethod().Name}", 
        //            Constant.LogLevelError, 
        //            Constant.TaskCategoriesError, 
        //            ex, 
        //            Constant.LogLevelError);
        //        return ex.Message;
        //    }

        //    return $"Syncronization Executed at {DateTime.Now}.";
        //}

        // Send request component
        // http://localhost:8000/GrabCaster/SyncSendRequestComponent?ChannelID=*&PointID=*&IDComponent={3C62B951-C353-4899-8670-C6687B6EAEFC}
        public string SyncSendRequestComponent(string channelId, string pointId, string idComponent)
        {
            try
            {
                if (Base.ConfigurationBag.Configuration.DisableExternalEventsStreamEngine)
                {
                    LogEngine.WriteLog(
                        Base.ConfigurationBag.EngineName, 
                        "Warning the Device Provider Interface is disable, the GrabCaster point will be able to work in local mode only.", 
                        Constant.LogLevelWarning, 
                        Constant.TaskCategoriesError, 
                        null, 
                        Constant.LogLevelWarning);

                    return
                        "Warning the Device Provider Interface is disable, the GrabCaster point will be able to work in local mode only.";
                }

                SyncProvider.SyncSendRequestComponent(channelId, pointId, idComponent);
            }
            catch (Exception ex)
            {
                LogEngine.WriteLog(
                    Base.ConfigurationBag.EngineName, 
                    $"Error in {MethodBase.GetCurrentMethod().Name}", 
                    Constant.LogLevelError, 
                    Constant.TaskCategoriesError, 
                    ex, 
                    Constant.LogLevelError);
                return ex.Message;
            }

            return $"Syncronization Executed at {DateTime.Now}.";
        }

        // Internal operations
        public string SyncSendRequestConfiguration(string channelId, string pointId)
        {
            try
            {
                if (Base.ConfigurationBag.Configuration.DisableExternalEventsStreamEngine)
                {
                    LogEngine.WriteLog(
                        Base.ConfigurationBag.EngineName, 
                        "Warning the Device Provider Interface is disable, the GrabCaster point will be able to work in local mode only.", 
                        Constant.LogLevelWarning, 
                        Constant.TaskCategoriesError, 
                        null, 
                        Constant.LogLevelWarning);

                    return
                        "Warning the Device Provider Interface is disable, the GrabCaster point will be able to work in local mode only.";
                }

                SyncProvider.SyncSendRequestConfiguration(channelId, pointId);
            }
            catch (Exception ex)
            {
                LogEngine.WriteLog(
                    Base.ConfigurationBag.EngineName, 
                    $"Error in {MethodBase.GetCurrentMethod().Name}", 
                    Constant.LogLevelError, 
                    Constant.TaskCategoriesError, 
                    ex, 
                    Constant.LogLevelError);
                return ex.Message;
            }

            return $"Configuration request sent at ChanelID: {channelId} PoinrID {pointId}.";
        }


        /// <summary>
        /// Execute an internal trigger
        /// </summary>
        /// <param name="triggerId">
        /// </param>
        /// <param name="configurationId"></param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        /// http://localhost:8000/GrabCaster/ExecuteTrigger?ConfigurationID={3C62B951-C353-4899-8670-C6687B6EAEFC}TriggerID={3C62B951-C353-4899-8670-C6687B6EAEFC}&value=text
        public string ExecuteTrigger(string configurationId,string triggerId,string value)
        {
            
            try
            {
                var executed = false;
                try
                {
                    var triggerSingleInstance = (from trigger in EventsEngine.BubblingTriggerConfigurationsSingleInstance
                                                 where trigger.IdComponent == triggerId && trigger.IdConfiguration == (configurationId ?? "")
                                                 select trigger).First();
                    var bubblingTriggerConfiguration = triggerSingleInstance;
                    byte[] content = EncodingDecoding.EncodingString2Bytes(value ?? "");
                    EventsEngine.ExecuteTriggerConfiguration(bubblingTriggerConfiguration,content);
                    executed = true;
                }
                catch
                {



                }

                try
                {
                    //Not executed 

                    var triggerPollingInstance = (from trigger in EventsEngine.BubblingTriggerConfigurationsPolling
                                                  where trigger.IdComponent == triggerId && trigger.IdConfiguration == (configurationId ?? "")
                                                  select trigger).First();
                    var bubblingTriggerConfiguration = triggerPollingInstance;
                    byte[] content = EncodingDecoding.EncodingString2Bytes(value ?? "");
                    EventsEngine.ExecuteTriggerConfiguration(bubblingTriggerConfiguration,content);
                    executed = true;
                }
                catch 
                {

                }

                return executed ? "Trigger executed." : "Trigger not executed check the Windows event viewer and check the trigger Id and configuration Id are present and active in the trigger folder - Looking for Trigger Id {triggerId} and configuration Id {configurationId}.";
            }
            catch (Exception ex)
            {
                return $"Error - {ex.Message} ";
            }
        }

        /// <summary>
        /// Return the complete configuration
        /// </summary>
        /// <returns>
        /// The <see cref="Stream"/>.
        /// </returns>
        /// http://localhost:8000/GrabCaster/Configuration
        public Stream Configuration()
        {
            try
            {
                return SyncProvider.GetConfiguration();
            }
            catch (Exception ex)
            {
                var docMain = new XmlDocument();
                var errorTemplate = docMain.CreateElement(string.Empty, "Error", string.Empty);
                var errorText = docMain.CreateTextNode(ex.Message);
                errorTemplate.AppendChild(errorText);

                var currentWebContext = WebOperationContext.Current;
                if (currentWebContext != null)
                {
                    currentWebContext.OutgoingResponse.ContentType = "text/xml";
                }
                return new MemoryStream(EncodingDecoding.EncodingString2Bytes(errorTemplate.OuterXml));
            }
        }
    }
}