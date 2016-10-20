using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using Microsoft.BizTalk.Component.Interop;
using Microsoft.BizTalk.Message.Interop;
using Microsoft.BizTalk.Streaming;
using ICSharpCode.SharpZipLib.Zip;
using Tamoil.EAI.Common.Services;

namespace Tamoil.EAI.Common.Pipeline
{

    [ComponentCategory(CategoryTypes.CATID_PipelineComponent)]
    [ComponentCategory(CategoryTypes.CATID_DisassemblingParser)]
    [Guid("BF2BB09F-6B7D-44B0-91A1-9712DDCC36D1")]
    public class UnZipDisassemblerComponent : IDisassemblerComponent, IBaseComponent, IPersistPropertyBag, IComponentUI
    {

        public UnZipDisassemblerComponent()
        {
        }

        #region Private Properties

        private string password;
        private string messageType;
        private string messageNamespace;
        private string rootElementName;

        System.Collections.Queue qOutputMsgs = new System.Collections.Queue();

        #endregion

        #region Get/Set Methods

        /// <summary>
        /// Archive password.
        /// </summary>
        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                password = value;
            }
        }

        /// <summary>
        /// Archive MessageType.
        /// </summary>
        public string MessageType
        {
            get
            {
                return messageType;
            }
            set
            {
                messageType = value;
            }
        }

        /// <summary>
        /// Archive Namespace.
        /// </summary>
        public string MessageNamespace
        {
            get
            {
                return messageNamespace;
            }
            set
            {
                messageNamespace = value;
            }
        }

        /// <summary>
        /// RootElementName.
        /// </summary>
        public string RootElementName
        {
            get
            {
                return rootElementName;
            }
            set
            {
                rootElementName = value;
            }
        }

        #endregion

        #region IBaseComponent members

        /// <summary>
        /// Name of the component
        /// </summary>
        [Browsable(false)]
        public string Name
        {
            get
            {
                return "UnZip Disassembler Component";
            }
        }

        /// <summary>
        /// Version of the component
        /// </summary>
        [Browsable(false)]
        public string Version
        {
            get
            {
                return "1.0.0.0";
            }
        }

        /// <summary>
        /// Description of the component
        /// </summary>
        [Browsable(false)]
        public string Description
        {
            get
            {
                return "UnZip Disassembler Component";
            }
        }
        #endregion

        #region IPersistPropertyBag members

        /// <summary>
        /// Gets class ID of component for usage from unmanaged code.
        /// </summary>
        /// <param name="classid">
        /// Class ID of the component
        /// </param>
        public void GetClassID(out Guid classid)
        {
            classid = new Guid("2FF22C30-17BF-48DB-A673-CD773B69B4B6");
        }

        /// <summary>
        /// not implemented
        /// </summary>
        public void InitNew()
        {
        }

        /// <summary>
        /// Loads configuration properties for the component
        /// </summary>
        /// <param name="pb">Configuration property bag</param>
        /// <param name="errlog">Error status</param>
        public virtual void Load(IPropertyBag pb, int errlog)
        {

            try
            {

                LoggingServices.TraceDebugInfo("IN");

                object val;

                val = ReadPropertyBag(pb, "Password");

                if ((val != null))
                {
                    password = ((string)(val));
                }

                val = ReadPropertyBag(pb, "MessageType");

                if ((val != null))
                {
                    messageType = ((string)(val));
                }

                val = ReadPropertyBag(pb, "MessageNamespace");

                if ((val != null))
                {
                    messageNamespace = ((string)(val));
                }

                val = ReadPropertyBag(pb, "RootElementName");

                if ((val != null))
                {
                    rootElementName = ((string)(val));
                }
            }
            catch (Exception exc)
            {
                LoggingServices.TraceError(exc);
            }
            finally
            {
                LoggingServices.TraceDebugInfo("OUT");
            }

        }

        /// <summary>
        /// Saves the current component configuration into the property bag
        /// </summary>
        /// <param name="pb">Configuration property bag</param>
        /// <param name="fClearDirty">not used</param>
        /// <param name="fSaveAllProperties">not used</param>
        public virtual void Save(IPropertyBag pb, bool fClearDirty, bool fSaveAllProperties)
        {
            try
            {

                LoggingServices.TraceDebugInfo("IN");

                WritePropertyBag(pb, "Password", Password);
                WritePropertyBag(pb, "MessageType", MessageType);
                WritePropertyBag(pb, "MessageNamespace", MessageNamespace);
                WritePropertyBag(pb, "RootElementName", RootElementName);

            }
            catch (Exception exc)
            {
                LoggingServices.TraceError(exc);
            }
            finally
            {
                LoggingServices.TraceDebugInfo("OUT");
            }
        }

        #region utility functionality

        /// <summary>
        /// Reads property value from property bag
        /// </summary>
        /// <param name="pb">Property bag</param>
        /// <param name="propName">Name of property</param>
        /// <returns>Value of the property</returns>
        private object ReadPropertyBag(IPropertyBag pb, string propName)
        {
            object val = null;

            try
            {
                pb.Read(propName, out val, 0);
            }
            catch (ArgumentException)
            {
                return val;
            }
            catch (Exception e)
            {
                throw new ApplicationException(e.Message);
            }

            return val;
        }

        /// <summary>
        /// Writes property values into a property bag.
        /// </summary>
        /// <param name="pb">Property bag.</param>
        /// <param name="propName">Name of property.</param>
        /// <param name="val">Value of property.</param>
        private void WritePropertyBag(IPropertyBag pb, string propName, object val)
        {

            try
            {
                LoggingServices.TraceDebugInfo("IN");
                pb.Write(propName, ref val);
            }
            catch (Exception exc)
            {
                LoggingServices.TraceError(exc);
                throw;
            }
            finally
            {
                LoggingServices.TraceDebugInfo("OUT");
            }
        }
        #endregion

        #endregion

        #region IComponentUI members
        /// <summary>
        /// Component icon to use in BizTalk Editor
        /// </summary>
        [Browsable(false)]
        public IntPtr Icon
        {
            get
            {
                return new IntPtr();
            }
        }

        /// <summary>
        /// The Validate method is called by the BizTalk Editor during the build 
        /// of a BizTalk project.
        /// </summary>
        /// <param name="obj">An Object containing the configuration properties.</param>
        /// <returns>The IEnumerator enables the caller to enumerate through a collection of strings containing error messages. These error messages appear as compiler error messages. To report successful property validation, the method should return an empty enumerator.</returns>
        public System.Collections.IEnumerator Validate(object obj)
        {
            return null;
        }
        #endregion

        #region IDisassemblerComponent Members


        public void Disassemble(Microsoft.BizTalk.Component.Interop.IPipelineContext pc,
                                Microsoft.BizTalk.Message.Interop.IBaseMessage inmsg)
        {

            Stream strmZipFile;
            IBaseMessagePart msgPart;
            msgPart = inmsg.BodyPart;
            strmZipFile = msgPart.GetOriginalDataStream();
            ZipInputStream oZipStream = new ZipInputStream(strmZipFile);

            if (!string.IsNullOrEmpty(password))
                oZipStream.Password = password;

            try
            {

                LoggingServices.TraceDebugInfo("IN");

                ZipEntry sEntry = oZipStream.GetNextEntry();

                while (sEntry != null)
                {

                    if (sEntry.IsDirectory)
                    {
                        sEntry = oZipStream.GetNextEntry();
                        continue;
                    }

                    MemoryStream strmMem = new MemoryStream();
                    byte[] buffer = new byte[4096];
                    int count = 0;

                    while ((count = oZipStream.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        strmMem.Write(buffer, 0, count);
                    }

                    strmMem.Seek(0, SeekOrigin.Begin);
                    msgPart.Data = strmMem;
                    IBaseMessage outMsg;
                    outMsg = pc.GetMessageFactory().CreateMessage();
                    outMsg.AddPart("Body", pc.GetMessageFactory().CreateMessagePart(), true);
                    outMsg.BodyPart.Data = strmMem;

                    for (int iProp = 0; iProp < inmsg.Context.CountProperties; iProp++)
                    {
                        string strName;
                        string strNSpace;
                        object val = inmsg.Context.ReadAt(iProp, out strName, out strNSpace);

                        // If the property has been promoted, respect the settings
                        if (inmsg.Context.IsPromoted(strName, strNSpace))
                            outMsg.Context.Promote(strName, strNSpace, val);
                        else
                            outMsg.Context.Write(strName, strNSpace, val);
                    }

                    if (this.MessageNamespace != null && this.RootElementName != null)
                    {
                        string messageType = string.Format("{0}#{1}", this.MessageNamespace, this.RootElementName);
                        outMsg.Context.Promote("MessageType", "http://schemas.microsoft.com/BizTalk/2003/system-properties", messageType);
                        outMsg.Context.Write("ReceivedFileName", "http://schemas.microsoft.com/BizTalk/2003/file-properties", sEntry.Name);
                    }

                    qOutputMsgs.Enqueue(outMsg);
                    sEntry = oZipStream.GetNextEntry();
                }
            }
            catch (Exception exc)
            {
                LoggingServices.TraceError(exc);
                throw;
            }
            finally
            {
                LoggingServices.TraceDebugInfo("OUT");
            }

        }


        public IBaseMessage GetNext(IPipelineContext pContext)
        {
            if (qOutputMsgs.Count > 0)
                return (IBaseMessage)qOutputMsgs.Dequeue();
            else
                return null;
        }

        #endregion

    }
}
