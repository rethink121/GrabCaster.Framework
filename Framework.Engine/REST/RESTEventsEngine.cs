// RESTEventsEngine.cs
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
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.Web;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using GrabCaster.Framework.Base;
using GrabCaster.Framework.Deployment;
using GrabCaster.Framework.Log;

#endregion

namespace GrabCaster.Framework.Engine
{
    public class RestEventsEngine : IRestEventsEngine
    {
        //http://localhost:8000/GrabCaster/Deploy?Configuration=Release&Platform=AnyCpu
        public string Deploy(string configuration, string platform)
        {
            try
            {
                string publishingFolder = Path.Combine(ConfigurationBag.DirectoryDeployment(),
                    ConfigurationBag.DirectoryNamePublishing);

                var regTriggers = new Regex(ConfigurationBag.DeployExtensionLookFor);
                var deployFiles =
                    Directory.GetFiles(publishingFolder, "*.*", SearchOption.AllDirectories)
                        .Where(
                            path =>
                                Path.GetExtension(path) == ".trigger" || Path.GetExtension(path) == ".event" ||
                                Path.GetExtension(path) == ".component");

                StringBuilder results = new StringBuilder();
                foreach (var file in deployFiles)
                {
                    string projectName = Path.GetFileNameWithoutExtension(publishingFolder + file);
                    string projectType = Path.GetExtension(publishingFolder + file).Replace(".", "");
                    bool resultOk = Jit.CompilePublishing(projectType, projectName,
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
                    ConfigurationBag.EngineName,
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
            if (EventsEngine.SyncronizePoint())
                return $"Syncronization completed. - {DateTime.Now}";
            return "Point syncronization failed with errors, check the event viewer and the log.";
        }

        /// <summary>
        ///     Refresh the internal bubbling setting
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
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
                    ConfigurationBag.EngineName,
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
        ///     Execute an internal trigger
        /// </summary>
        /// <param name="triggerId">
        /// </param>
        /// <param name="configurationId"></param>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        /// http://localhost:8000/GrabCaster/ExecuteTrigger?ConfigurationID={5D793BC4-B111-4BF4-BAAF-196F661E13E2}&TriggerID={9A989BD1-C8DE-4FC1-B4BA-02E7D8A4AD76}&value=text
        public string ExecuteTrigger(string configurationId, string triggerId, string value)
        {
            try
            {
                var BubblingObjects = (from trigger in EventsEngine.BubblingTriggerConfigurationsSingleInstance
                    where trigger.IdComponent == triggerId && trigger.IdConfiguration == (configurationId ?? "")
                    select trigger).ToArray();
                if (BubblingObjects.Length != 0)
                {
                    byte[] content = EncodingDecoding.EncodingString2Bytes(value ?? "");
                    EventsEngine.ExecuteTriggerConfiguration(BubblingObjects[0], content);
                    return "Trigger executed.";
                }
                return
                    $"Trigger not found. - Looking for Trigger Id {triggerId} and configuration Id {configurationId}.";
            }
            catch (Exception ex)
            {
                return
                    $"Trigger not executed check the Windows event viewer and check the trigger Id and configuration Id are present and active in the trigger folder - Looking for Trigger Id {triggerId} and configuration Id {configurationId} - Exception {ex.Message}.";
            }
        }

        /// <summary>
        ///     Return the complete configuration
        /// </summary>
        /// <returns>
        ///     The <see cref="Stream" />.
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