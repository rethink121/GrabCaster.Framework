using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Xsl;
using GrabCaster.Framework.Base;
using GrabCaster.Framework.Contracts.Components;
using GrabCaster.Framework.Log;

namespace GrabCaster.Framework.BTSTransformComponent
{
    using BizTalk.Extensibility;
    using GrabCaster.Framework.Contracts.Attributes;
    using GrabCaster.Framework.Contracts.Events;
    using GrabCaster.Framework.Contracts.Globals;

    [ComponentContract("{EFA41557-6AE0-4880-B8B2-EE3DF2C1E48E}", "BizTalk Transform Executor", "Execute a BizTalk Transform")]
    public class BizTalkTransform : IChainComponentType
    {
        [ComponentPropertyContract("AssemblyFile", "Transform assembly file")]
        public string AssemblyFile { get; set; }
        [ComponentPropertyContract("TransformTypeName", "Transform type name")]
        public string TransformTypeName { get; set; }

        /// <summary>
        /// Gets or sets the data context.
        /// </summary>
        /// <value>
        /// The data context.
        /// </value>
        [ComponentPropertyContract("DataContext", "Event Default Main Data")]
        public byte[] DataContext { get; set; }
        [ComponentActionContract("{C256542A-9248-4E37-A2C4-53AA801A82DC}", "Main action", "Main action description")]
        public byte[] Execute()
        {
            try
            {
                Assembly asm = Assembly.LoadFrom(AssemblyFile);

                Type t = asm.GetType(TransformTypeName);
                if (t == null)
                {
                    LogEngine.WriteLog(ConfigurationBag.EngineName,
                              $"Error in {MethodBase.GetCurrentMethod().Name} - {TransformTypeName} not found in assembly {AssemblyFile}",
                              Constant.LogLevelError,
                              Constant.TaskCategoriesError,
                              null,
                              Constant.LogLevelError);
                }
                
                object assemblyTransform = asm.CreateInstance(TransformTypeName);


                PropertyInfo pi = assemblyTransform.GetType().GetProperty("XmlContent", BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance);
                object viewData = pi.GetValue(assemblyTransform, null);
                string tmpViewData = viewData != null ? viewData.ToString() : string.Empty;
                string dataXslt = tmpViewData.Replace("utf-16", "utf-8");

                XslCompiledTransform proc = new XslCompiledTransform();

                using (StringReader sr = new StringReader(dataXslt))
                {
                    using (XmlReader xr = XmlReader.Create(sr))
                    {
                        proc.Load(xr);
                    }
                }

                string resultXML;

                string xmlData = EncodingDecoding.EncodingBytes2String(DataContext);
                using (StringReader sr = new StringReader(xmlData))
                {
                    using (XmlReader xr = XmlReader.Create(sr))
                    {
                        using (StringWriter sw = new StringWriter())
                        {
                            proc.Transform(xr, null, sw);
                            resultXML = sw.ToString();
                            return EncodingDecoding.EncodingString2Bytes(resultXML);
                        }
                    }
                }

            }
            catch (Exception ex)
            {

                LogEngine.WriteLog(ConfigurationBag.EngineName,
                                              $"Error in {MethodBase.GetCurrentMethod().Name}",
                                              Constant.LogLevelError,
                                              Constant.TaskCategoriesError,
                                              ex,
                                              Constant.LogLevelError);

                return null;
            }



        }
    }
}
