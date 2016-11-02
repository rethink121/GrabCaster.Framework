// -----------------------------------------------------------------------------------
// 
// GRABCASTER LTD CONFIDENTIAL
// ___________________________
// 
// Copyright � 2013 - 2016 GrabCaster Ltd. All rights reserved.
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
using System.Reflection;
using System.Resources;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Microsoft.Win32;
using Microsoft.BizTalk.Component.Interop;
using Microsoft.BizTalk.Adapter.Framework;

namespace GrabCaster.Framework.BizTalk.Adapter.Designtime 
{
    
    /// <summary>
    /// Description: 
    /// Class DynamicAdapterManagement implements
    /// IAdapterConfig and IDynamicAdapterConfig interfaces for
    /// management to illustrate a dynamic adapter that uses the
    /// adapter framework.
    /// </summary>
    public class DynamicAdapterManagement : IAdapterConfig, IDynamicAdapterConfig 
	{
        
		private static ResourceManager resourceManager = new ResourceManager("GrabCaster.Framework.BizTalk.Adapter.Designtime.GrabCasterResource", Assembly.GetExecutingAssembly());

		/// <summary>
        /// Returns the configuration schema as a string.
        /// (Implements IAdapterConfig)
        /// </summary>
        /// <param name="type">Configuration schema to return</param>
        /// <returns>Selected xsd schema as a string</returns>
        public string GetConfigSchema(ConfigType type) 
		{
            switch (type) 
			{
				case ConfigType.ReceiveHandler:
					return LocalizeSchema(GetResource("AdapterManagement.ReceiveHandler.xsd"), resourceManager);

				case ConfigType.ReceiveLocation:
					return LocalizeSchema(GetResource("AdapterManagement.ReceiveLocation.xsd"), resourceManager);

				case ConfigType.TransmitHandler:
					return LocalizeSchema(GetResource("AdapterManagement.TransmitHandler.xsd"), resourceManager);

				case ConfigType.TransmitLocation:
					return LocalizeSchema(GetResource("AdapterManagement.TransmitLocation.xsd"), resourceManager);

				default:
					return null;
            }
        }

		protected string LocalizeSchema (string schema, ResourceManager resourceManager)
		{
			XmlDocument document = new XmlDocument();
			document.LoadXml(schema);

			XmlNodeList nodes = document.SelectNodes("/descendant::*[@_locID]");
			foreach (XmlNode node in nodes)
			{
				string locID = node.Attributes["_locID"].Value;
				node.InnerText = resourceManager.GetString(locID);
			}

			StringWriter writer = new StringWriter();
			document.WriteTo(new XmlTextWriter(writer));

			string localizedSchema = writer.ToString();
			return localizedSchema;
		}

		/// <summary>
        /// Get the WSDL file name for the selected WSDL
        /// </summary>
        /// <param name="wsdls">place holder</param>
        /// <returns>An empty string[]</returns>
        public string [] GetServiceDescription(string [] wsdls) 
		{
            string []result = null;
            return result;
        }

        /// <summary>
        /// Acquire externally referenced xsd's.
        /// </summary>
        /// <param name="xsdLocation">Location of schema</param>
        /// <param name="xsdNamespace">Namespace</param>
        /// <param name="XSDFileName">Schmea file name (return)</param>
        /// <returns>Outcome of acquisition</returns>
        public Result GetSchema( string xsdLocation, string xsdNamespace, out string xsdSchema) 
		{
            xsdSchema = null;
            return Result.Continue;
        }

        /// <summary>
        /// Acquire wsdl(s) from which to build the user interface
        /// </summary>
        /// <param name="endPointConfiguration"></param>
        /// <param name="owner"></param>
        /// <param name="WSDLList">Array of custom UI's WSDL (returned)</param>
        /// <returns></returns>
        public Result DisplayUI(IPropertyBag endPointConfiguration, 
            IWin32Window owner,
            out string [] WSDLList) 
		{
            WSDLList = new string[1];
            WSDLList[0] = GetResource("AdapterManagement.service1.wsdl");
            return Result.Continue;
        }

        /// <summary>
        /// Helper to get resource from manafest.  Replace with ResourceManager.GetString if .resources or
        /// .resx files are used for managing this assemblies resources.
        /// </summary>
        /// <param name="resource">Full resource name</param>
        /// <returns>Resource value</returns>
        private string GetResource(string resource) 
		{
            string value = null;
            if (null != resource) 
			{
                Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resource);
                StreamReader reader = null;
                using (reader = new StreamReader(stream)) 
				{
                    value = reader.ReadToEnd();
                }
            }

            return value;
        }
    }

    /// <summary>
    /// Class StaticAdapterManagement implements
    /// IAdapterConfig and IStaticAdapterConfig interfaces for
    /// management to illustrate a static adapter that uses the
    /// adapter framework
    /// </summary>
    public class StaticAdapterManagement : IAdapterConfig, IStaticAdapterConfig, IAdapterConfigValidation 
	{
		private static ResourceManager resourceManager = new ResourceManager("AdapterManagement.GrabCasterResource", Assembly.GetExecutingAssembly());
																																				  
		
		protected string LocalizeSchema (string schema, ResourceManager resourceManager)
		{
			XmlDocument document = new XmlDocument();
			document.LoadXml(schema);

			XmlNodeList nodes = document.SelectNodes("/descendant::*[@_locID]");
			foreach (XmlNode node in nodes)
			{
				string locID = node.Attributes["_locID"].Value;
				node.InnerText = resourceManager.GetString(locID);
			}

			StringWriter writer = new StringWriter();
			document.WriteTo(new XmlTextWriter(writer));

			string localizedSchema = writer.ToString();
			return localizedSchema;
		}

		/// <summary>
        /// Returns the configuration schema as a string.
        /// (Implements IAdapterConfig)
        /// </summary>
        /// <param name="type">Configuration schema to return</param>
        /// <returns>Selected xsd schema as a string</returns>
        public string GetConfigSchema(ConfigType type) 
		{
            switch (type) 
			{
				case ConfigType.ReceiveHandler:
					return LocalizeSchema(GetResource("AdapterManagement.ReceiveHandler.xsd"), resourceManager);

				case ConfigType.ReceiveLocation:
					return LocalizeSchema(GetResource("AdapterManagement.ReceiveLocation.xsd"), resourceManager);
	            
				case ConfigType.TransmitHandler:
					return LocalizeSchema(GetResource("AdapterManagement.TransmitHandler.xsd"), resourceManager);

				case ConfigType.TransmitLocation:
					return LocalizeSchema(GetResource("AdapterManagement.TransmitLocation.xsd"), resourceManager);

				default:
					return null;
            }
        }

        /// <summary>
        /// Get the WSDL file name for the selected WSDL
        /// </summary>
        /// <param name="wsdls">place holder</param>
        /// <returns>An empty string[]</returns>
        public string[] GetServiceDescription(string[] wsdls) 
		{
            string[] result = new string[1];
            result[0] = GetResource("AdapterManagement.service1.wsdl");
            return result;
        }

        /// <summary>
        /// Gets the XML instance of TreeView that needs to be rendered
        /// </summary>
        /// <param name="endPointConfiguration"></param>
        /// <param name="NodeIdentifier"></param>
        /// <returns>Location of TreeView xml instance</returns>
        public string GetServiceOrganization(IPropertyBag endPointConfiguration,
                                             string NodeIdentifier) 
		{
            string result = GetResource("AdapterManagement.CategorySchema.xml");
            return result;
        }

        
        /// <summary>
        /// Acquire externally referenced xsd's
        /// </summary>
        /// <param name="xsdLocation">Location of schema</param>
        /// <param name="xsdNamespace">Namespace</param>
        /// <param name="XSDFileName">Schmea file name (return)</param>
        /// <returns>Outcome of acquisition</returns>
        public Result GetSchema(string xsdLocation,
                                string xsdNamespace,
								out string xsdSchema) 
		{
            xsdSchema = null;
            return Result.Continue;
        }

        /// <summary>
        /// Helper to get resource from manafest.  Replace with 
        /// ResourceManager.GetString if .resources or
        /// .resx files are used for managing this assemblies resources.
        /// </summary>
        /// <param name="resource">Full resource name</param>
        /// <returns>Resource value</returns>
        private string GetResource(string resource) 
		{
            string value = null;
            if (null != resource) 
			{
				Assembly assem = this.GetType().Assembly;
				Stream stream = assem.GetManifestResourceStream(resource);
				StreamReader reader = null;

                using (reader = new StreamReader(stream)) 
				{
                    value = reader.ReadToEnd();
                }
            }

            return value;
        }
        /// <summary>
        /// Validate xmlInstance against configuration.  In this example it does nothing.
        /// </summary>
        /// <param name="type">Type of port or location being configured</param>
        /// <param name="xmlInstance">Instance value to be validated</param>
        /// <returns>Validated configuration.</returns>
        public string ValidateConfiguration(ConfigType configType,
            string xmlInstance)
        {
            string validXml = String.Empty;

            switch (configType)
            {
                case ConfigType.ReceiveHandler:
                    validXml = xmlInstance;
                    break;

                case ConfigType.ReceiveLocation:
                    validXml = ValidateReceiveLocation(xmlInstance);
                    break;

                case ConfigType.TransmitHandler:
                    validXml = xmlInstance;
                    break;

                case ConfigType.TransmitLocation:
                    validXml = ValidateTransmitLocation(xmlInstance);
                    break;
            }

            return validXml;
        }
        /// <summary>
        /// Generate uri entry based on directory and fileMask values
        /// </summary>
        /// <param name="type">Type of port or location being configured</param>
        /// <param name="xmlInstance">Instance value to be validated</param>
        /// <returns>Validated configuration.</returns>
        private string ValidateReceiveLocation(string xmlInstance)
        {
            // Load up document
            XmlDocument document = new XmlDocument();
            document.LoadXml(xmlInstance);

            // Build up inner text
            StringBuilder builder = new StringBuilder();

            XmlNode pointName = document.SelectSingleNode("Config/pointName");
            if (null != pointName && 0 < pointName.InnerText.Length)
            {
                builder.Append(pointName.InnerText + @"\");
            }

            XmlNode maximumBatchSize = document.SelectSingleNode("Config/maximumBatchSize");
            if (null != maximumBatchSize && 0 < maximumBatchSize.InnerText.Length)
            {
                builder.Append(maximumBatchSize.InnerText);
            }
            else
            {
                throw new System.ArgumentException("Set the maximumBatchSize parameter.");
            }
            XmlNode maximumNumberOfMessages = document.SelectSingleNode("Config/maximumNumberOfMessages");
            if (null != maximumNumberOfMessages && 0 < maximumBatchSize.InnerText.Length)
            {
                builder.Append(maximumNumberOfMessages.InnerText);
            }
            else
            {
                throw new System.ArgumentException("Set the maximumNumberOfMessages parameter.");
            }
            XmlNode errorThreshold = document.SelectSingleNode("Config/errorThreshold");
            if (null != errorThreshold && 0 < errorThreshold.InnerText.Length)
            {
                builder.Append(errorThreshold.InnerText);
            }
            else
            {
                throw new System.ArgumentException("Set the errorThreshold parameter.");
            }
            XmlNode uri = document.SelectSingleNode("Config/uri");
            if (null == uri)
            {
                uri = document.CreateElement("uri");
                document.DocumentElement.AppendChild(uri);
            }

            uri.InnerText = $"GrabCaster://{uri.InnerText = builder.ToString()}";

            return document.OuterXml;
        }

        /// <summary>
        /// Generate uri entry based on directory and fileName values
        /// </summary>
        /// <param name="type">Type of port or location being configured</param>
        /// <param name="xmlInstance">Instance value to be validated</param>
        /// <returns>Validated configuration.</returns>
        private string ValidateTransmitLocation(string xmlInstance)
        {
            // Load up document
            XmlDocument document = new XmlDocument();
            document.LoadXml(xmlInstance);

            // Build up inner text
            StringBuilder builder = new StringBuilder();

            XmlNode idConfiguration = document.SelectSingleNode("Config/idConfiguration");
            if (null != idConfiguration && 0 < idConfiguration.InnerText.Length)
            {
                builder.Append(idConfiguration.InnerText + @"\");
            }
            else
            {
                throw new System.ArgumentException("Set the idConfiguration parameter.");
            }

            XmlNode idComponent = document.SelectSingleNode("Config/idComponent");
            if (null != idComponent && 0 < idComponent.InnerText.Length)
            {
                builder.Append(idComponent.InnerText);
            }
            else
            {
                throw new System.ArgumentException("Set the idComponent parameter.");
            }
            XmlNode pointName = document.SelectSingleNode("Config/pointName");
            if (null != pointName && 0 < pointName.InnerText.Length)
            {
                builder.Append(pointName.InnerText);
            }

            XmlNode uri = document.SelectSingleNode("Config/uri");
            if (null == uri)
            {
                uri = document.CreateElement("uri");
                document.DocumentElement.AppendChild(uri);
            }
            uri.InnerText = $"GrabCaster://{uri.InnerText = builder.ToString()}";

            return document.OuterXml;
        }

    }
}
