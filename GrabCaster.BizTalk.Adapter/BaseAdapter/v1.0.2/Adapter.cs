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
using System.Diagnostics;
using Microsoft.BizTalk.Component.Interop;
using Microsoft.BizTalk.TransportProxy.Interop;

namespace GrabCaster.Framework.BizTalk.Adapter.Common
{
	/// <summary>
	/// Summary description for Adapter.
	/// </summary>
	public abstract class Adapter :
		IBTTransport,
		IBTTransportControl,
		IPersistPropertyBag
	{
		//  core member data
		private string propertyNamespace;
		private IBTTransportProxy transportProxy;
		private IPropertyBag handlerPropertyBag;
		private bool initialized;

		//  member data for implementing IBTTransport
		private string name;
		private string version;
		private string description;
		private string transportType;
		private Guid   clsid;

		protected Adapter (
			string name,
			string version,
			string description,
			string transportType,
			Guid clsid,
			string propertyNamespace)
		{
			Trace.WriteLine(String.Format("Adapter.Adapter name: {0}", name));

			this.transportProxy     = null;
			this.handlerPropertyBag = null;
			this.initialized        = false;

			this.name               = name;
			this.version            = version;
			this.description        = description;
			this.transportType      = transportType;
			this.clsid              = clsid;

			this.propertyNamespace  = propertyNamespace;
		}

		protected string            PropertyNamespace  { get { return propertyNamespace; } }
		public IBTTransportProxy    TransportProxy     { get { return transportProxy; } }
		protected IPropertyBag      HandlerPropertyBag { get { return handlerPropertyBag; } }
		protected bool              Initialized        { get { return initialized; } }

		//  IBTTransport
		public string Name { get { return name; } }
		public string Version { get { return version; } }
		public string Description { get { return description; } }
		public string TransportType { get { return transportType; } }
		public Guid ClassID { get { return clsid; } }

		//  IBTransportControl
		public virtual void Initialize (IBTTransportProxy transportProxy)
		{
            Trace.WriteLine("Adapter.Initialize");

			//  this is a Singleton and this should only ever be called once
			if (this.initialized)
				throw new AlreadyInitialized();				

			this.transportProxy = transportProxy;
			this.initialized = true;
		}
		public virtual void Terminate ()
		{
            Trace.WriteLine("Adapter.Terminate");

			if (!this.initialized)
				throw new NotInitialized();
			
			this.transportProxy = null;
		}

		protected virtual void HandlerPropertyBagLoaded ()
		{
			// let any derived classes know the property bag has now been loaded
		}

		// IPersistPropertyBag
		public void GetClassID (out Guid classid) { classid = this.clsid; }
		public void InitNew () { }
		public void Load (IPropertyBag pb, int pErrorLog)
		{
            Trace.WriteLine("Adapter.Load");
            
            this.handlerPropertyBag = pb;
			HandlerPropertyBagLoaded();
		}
		public void Save (IPropertyBag pb, bool fClearDirty, bool fSaveAllProperties) { }
	}
}
