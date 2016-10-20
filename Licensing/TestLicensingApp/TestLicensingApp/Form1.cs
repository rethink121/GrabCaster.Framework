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

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }



        private void buttonTestLicense_Click(object sender, EventArgs e)
        {


            //string validationKey = "AMAAMACTpBpgKZVChC9PEsB9elxMi/wlCgoiEoAqMXMZR4EuOkUe+cwwTm2hnaNREzEQhjsDAAEAAQ==";

            //CryptoLicense license = new CryptoLicense("FgKAgFN4q6orYNEBAQABBxAHl9rRMwzN3cHHV49VcToBYWSHtfimwgWVkD+qvvnFa9AxMH307AO4MtpNzI/ydBA=", validationKey);
            //if (license.Status != LicenseStatus.Valid)
            //{
            //    MessageBox.Show("License validation failed");
            //}
            //else
            //{
            //    // Continue normal execution...
            //}


            Licensing licensing = new Licensing();
            if (!licensing.EvaluateLicense())
            {
                MessageBox.Show(
                    "License key not valid, contact the GrabCaster Team.",
                    "GrabCaster",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show("License valid.");
            }

        }
    }
}
