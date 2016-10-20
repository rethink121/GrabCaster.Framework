// --------------------------------------------------------------------------------------------------
// <copyright file = "PersistentProvider.cs" company="GrabCaster Ltd">
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
namespace GrabCaster.Framework.Storage
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;

    using GrabCaster.Framework.Base;
    using GrabCaster.Framework.Contracts.Bubbling;
    using GrabCaster.Framework.Contracts.Globals;
    using GrabCaster.Framework.Log;

    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Blob;

    using Newtonsoft.Json;

    /// <summary>
    /// Main persistent provider.
    /// </summary>
    public static class PersistentProvider
    {
        public enum CommunicationDiretion
        {
            OffRamp,

            OnRamp
        }

        public static bool PersistMessage(ActionContext actionContext)
        {
            if (ConfigurationBag.Configuration.EnablePersistingMessaging == false)
            {
                return true;
            }

            return PersistMessage(actionContext,CommunicationDiretion.OnRamp);
        }

        /// <summary>
        /// Persist the message in local file system
        /// </summary>
        /// <param name="bubblingEvent">
        /// </param>
        /// <param name="actionContext"></param>
        /// <param name="communicationDiretion">
        /// The communication Diretion.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool PersistMessage(ActionContext actionContext,CommunicationDiretion communicationDiretion)
        {
            try
            {
                if (!ConfigurationBag.Configuration.EnablePersistingMessaging)
                {
                    return true;
                }

                var serializedMessage = JsonConvert.SerializeObject(actionContext.BubblingObjectBag);
                var directoryDate = string.Concat(
                    DateTime.Now.Year, 
                    "\\", 
                    DateTime.Now.Month.ToString().PadLeft(2, '0'), 
                    "\\", 
                    DateTime.Now.Day.ToString().PadLeft(2, '0'), 
                    "\\", 
                    communicationDiretion.ToString());
                var datetimeFile = string.Concat(
                    DateTime.Now.Year, 
                    DateTime.Now.Month.ToString().PadLeft(2, '0'), 
                    DateTime.Now.Day.ToString().PadLeft(2, '0'), 
                    "-", 
                    DateTime.Now.Hour.ToString().PadLeft(2, '0'), 
                    "-", 
                    DateTime.Now.Minute.ToString().PadLeft(2, '0'), 
                    "-", 
                    DateTime.Now.Second.ToString().PadLeft(2, '0'));

                var persistingForlder = Path.Combine(ConfigurationBag.Configuration.LocalStorageConnectionString, directoryDate);
                Directory.CreateDirectory(persistingForlder);
                var filePersisted = Path.Combine(
                    persistingForlder, 
                    string.Concat(datetimeFile, "-", actionContext.MessageId, ConfigurationBag.MessageFileStorageExtension));

                File.WriteAllText(filePersisted, serializedMessage);
                Debug.WriteLine(
                    "Event persisted -  Consistency Transaction Point created.", 
                    ConsoleColor.DarkGreen);
                return true;
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
                return false;
            }
        }

        /// <summary>
        /// Persist the message in local file system
        /// </summary>
        /// <param name="bubblingObject">
        /// </param>
        /// <param name="eventActionContext"></param>
        /// <param name="communicationDiretion">
        /// The communication Diretion.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool PersistMessage(BubblingObject bubblingObject,string MessageId, CommunicationDiretion communicationDiretion)
        {
            try
            {
                if (!ConfigurationBag.Configuration.EnablePersistingMessaging)
                {
                    return true;
                }

                var serializedMessage = JsonConvert.SerializeObject(bubblingObject);
                var directoryDate = string.Concat(
                    DateTime.Now.Year,
                    "\\",
                    DateTime.Now.Month.ToString().PadLeft(2, '0'),
                    "\\",
                    DateTime.Now.Day.ToString().PadLeft(2, '0'),
                    "\\",
                    communicationDiretion.ToString());
                var datetimeFile = string.Concat(
                    DateTime.Now.Year,
                    DateTime.Now.Month.ToString().PadLeft(2, '0'),
                    DateTime.Now.Day.ToString().PadLeft(2, '0'),
                    "-",
                    DateTime.Now.Hour.ToString().PadLeft(2, '0'),
                    "-",
                    DateTime.Now.Minute.ToString().PadLeft(2, '0'),
                    "-",
                    DateTime.Now.Second.ToString().PadLeft(2, '0'));

                var persistingForlder = Path.Combine(ConfigurationBag.Configuration.LocalStorageConnectionString, directoryDate);
                Directory.CreateDirectory(persistingForlder);
                var filePersisted = Path.Combine(
                    persistingForlder,
                    string.Concat(datetimeFile, "-", MessageId, ConfigurationBag.MessageFileStorageExtension));

                File.WriteAllText(filePersisted, serializedMessage);
                Debug.WriteLine(
                    "Event persisted -  Consistency Transaction Point created.",
                    ConsoleColor.DarkGreen);
                return true;
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
                return false;
            }
        }

    }
}