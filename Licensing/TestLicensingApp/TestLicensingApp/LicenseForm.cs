using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestLicensingApp
{
    using LogicNP.CryptoLicensing;

    public partial class LicenseForm : Form
    {
        CryptoLicense CreateLicense()
        {
            CryptoLicense ret = new CryptoLicense();

            //Uses the validation key of the "samples.netlicproj" license project file from the "Samples" directory
            // Get the validation key using Ctrl+K in the Generator UI.
            ret.ValidationKey = "AMAAMACAan6T73Ctw/rY+L9u9uwHqu7uns3owx7c1/oqgVhKLo+dFEG34875h3IWCcNU8e0DAAEAAQ==";

            // *** IMPORTANT: Set the LicenseServiceURL property below to the URL of your license service
            // See video tutorial at http://www.ssware.com/cryptolicensing/demo/license_service_net.htm to learn 
            // how to create the license service
            ret.LicenseServiceURL = ""; // your license service URL here!

            return ret;
        }
        public LicenseForm()
        {
            InitializeComponent();
        }

        private void linkLabelContact_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://grabcaster.io/grabcaster-contact/");
        }

        private void buttonConfirm_Click(object sender, EventArgs e)
        {

        }
    }
}
