// --------------------------------------------------------------------------------------------------
// <copyright file = "Demos.cs" company="GrabCaster Ltd">
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

using GrabCaster.Framework.Base;

namespace Laboratory
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Windows.Forms;

    /// <summary>
    /// The demos.
    /// </summary>
    public partial class Demos : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Demos"/> class.
        /// </summary>
        public Demos()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// The button power json_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void ButtonPowerJsonClick(object sender, EventArgs e)
        {
            this.labellastrun.Text = string.Empty;
            var numof = int.Parse(this.textBoxNum.Text);
            for (var i = 0; i < numof; i++)
            {
                if (!EventLog.SourceExists(this.comboBoxsource.Text))
                {
                    EventLog.CreateEventSource(this.comboBoxsource.Text, "Application");
                }
                string message = $"{Environment.MachineName}: {textBoxMessage.Text} {DateTime.Now.ToString()}";
                EventLog.WriteEntry(
                    this.comboBoxsource.Text,
                    // ReSharper disable once SpecifyACultureInStringConversionExplicitly
                    message, EventLogEntryType.Error);
            }

            this.labellastrun.Text = DateTime.Now.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// The demos_ load.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void DemosLoad(object sender, EventArgs e)
        {
            this.comboBoxsource.SelectedIndex = 0;
        }

        /// <summary>
        /// The label 2_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void Label2Click(object sender, EventArgs e)
        {
        }
    }
}