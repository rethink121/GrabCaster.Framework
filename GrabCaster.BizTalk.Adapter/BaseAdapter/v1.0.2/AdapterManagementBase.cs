// -----------------------------------------------------------------------------------
// 
// GRABCASTER LTD CONFIDENTIAL
// ___________________________
// 
// Copyright © 2013 - 2016 GrabCaster Ltd. All rights reserved.
// This work is registered with the UK Copyright Service: Registration No:284701085
// 
// 
// NOTICE:  All information contained herein is, and remains
// the property of GrabCaster Ltd and its suppliers,
// if any.  The intellectual and technical concepts contained
// herein are proprietary to GrabCaster Ltd
// and its suppliers and may be covered by UK and Foreign Patents,
// patents in process, and are protected by trade secret or copyright law.
// Dissemination of this information or reproduction of this material
// is strictly forbidden unless prior written permission is obtained
// from GrabCaster Ltd.
// 
// -----------------------------------------------------------------------------------
using System;
using System.IO;
using System.Xml;
using System.Resources;
using System.Reflection;
using Microsoft.Win32;

namespace GrabCaster.Framework.BizTalk.Adapter.Common
{
	public class AdapterManagementBase
	{
		//  implementation - access schema buried in resources

		protected string GetSchemaFromResource (string name)
		{
			Assembly assem = this.GetType().Assembly;
            using (Stream stream = assem.GetManifestResourceStream(name))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    string schema = reader.ReadToEnd();
                    return schema;
                }
            }
		}

		protected XmlDocument LocalizeSchemaDOM (string schema, ResourceManager resourceManager)
		{
			XmlDocument document = new XmlDocument();
			document.LoadXml(schema);

			XmlNodeList nodes = document.SelectNodes("/descendant::*[@_locID]");
			foreach (XmlNode node in nodes)
			{
				string locID = node.Attributes["_locID"].Value;
				node.InnerText = resourceManager.GetString(locID);
			}

			return document;
		}

		protected string MakeString (XmlDocument document)
		{
            using (StringWriter writer = new StringWriter())
            {
                document.WriteTo(new XmlTextWriter(writer));
                return writer.ToString();
            }
		}

		protected string LocalizeSchema (string schema, ResourceManager resourceManager)
		{
			return MakeString(LocalizeSchemaDOM(schema, resourceManager));
		}

		protected XmlDocument AddPathToEditorAssembly (XmlDocument document, string assemblyPath)
		{
			XmlNamespaceManager nsmgr = new XmlNamespaceManager(new NameTable());
			nsmgr.AddNamespace("baf", "BiztalkAdapterFramework.xsd");

			//  add editor and converter elements that have an assembly attribute
			string xpath = "/descendant::baf:editor[@assembly] | /descendant::baf:converter[@assembly]";

			XmlNodeList nodes = document.SelectNodes(xpath, nsmgr);
			foreach (XmlNode node in nodes)
			{
				XmlAttribute attribute = node.Attributes["assembly"];
				attribute.Value = assemblyPath + attribute.Value;
			}

			return document;
		}

		//  useful debug code
		protected string GetSchemaFromFile (string name)
		{
            using (RegistryKey bts30 = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\BizTalk Server\\3.0"))
            {
                string installPath = (string)bts30.GetValue("InstallPath");
                string productLanguage = (string)bts30.GetValue("ProductLanguage");
                string fullName = installPath + productLanguage + "\\" + name;

                using (StreamReader reader = new StreamReader(fullName))
                {
                    string schema = reader.ReadToEnd();
                    return schema;
                }
            }
		}
	}
}