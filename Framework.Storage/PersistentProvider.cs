// PersistentProvider.cs
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
using System.Diagnostics;
using System.IO;
using System.Reflection;
using GrabCaster.Framework.Base;
using GrabCaster.Framework.Contracts.Bubbling;
using GrabCaster.Framework.Contracts.Globals;
using GrabCaster.Framework.Log;
using Newtonsoft.Json;

#endregion

namespace GrabCaster.Framework.Storage
{
    /// <summary>
    ///     Main persistent provider.
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

            return PersistMessage(actionContext, CommunicationDiretion.OnRamp);
        }

        /// <summary>
        ///     Persist the message in local file system
        /// </summary>
        /// <param name="bubblingEvent">
        /// </param>
        /// <param name="actionContext"></param>
        /// <param name="communicationDiretion">
        ///     The communication Diretion.
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public static bool PersistMessage(ActionContext actionContext, CommunicationDiretion communicationDiretion)
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

                var persistingForlder = Path.Combine(ConfigurationBag.Configuration.LocalStorageConnectionString,
                    directoryDate);
                Directory.CreateDirectory(persistingForlder);
                var filePersisted = Path.Combine(
                    persistingForlder,
                    string.Concat(datetimeFile, "-", actionContext.MessageId,
                        ConfigurationBag.MessageFileStorageExtension));

                File.WriteAllText(filePersisted, serializedMessage);
                Debug.WriteLine(
                    "Event persisted -  Consistency Transaction Point created.");
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
        ///     Persist the message in local file system
        /// </summary>
        /// <param name="bubblingObject">
        /// </param>
        /// <param name="eventActionContext"></param>
        /// <param name="communicationDiretion">
        ///     The communication Diretion.
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public static bool PersistMessage(BubblingObject bubblingObject, string MessageId,
            CommunicationDiretion communicationDiretion)
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

                var persistingForlder = Path.Combine(ConfigurationBag.Configuration.LocalStorageConnectionString,
                    directoryDate);
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