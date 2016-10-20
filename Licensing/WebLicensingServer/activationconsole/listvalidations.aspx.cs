using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LogicNP.CryptoLicensing;
using System.Text;
using System.Drawing;
using System.IO;

namespace LicService
{
    public partial class ListValidations : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                InitSettingFilesDropDown();
            }
        }

        protected void btnGetValidationData_Click(object sender, EventArgs e)
        {
            try
            {
                lstRecords.Items.Clear();

                string settingsFilePath = Path.Combine(install.basePath, cmbSettingFiles.SelectedValue);
                LicenseServiceClass srvc = new LicenseServiceClass();
                ValidationData vd = srvc.GetValidations(txtLicenseCode.Text, settingsFilePath);
                foreach (Validation v in vd.validations)
                {
                    string str = v.machineCode;
                    if (!chkUseHashedMachineCodes.Checked)
                    {
                        byte[] temp = Convert.FromBase64String(str);
                        str = Encoding.UTF8.GetString(temp);
                    }
                    lstRecords.Items.Add(
                        "Machine Code: " + str
                        + ", IP: " + v.ip
                        + ", Validated: " + v.dateValidated.ToString()
                        );
                }

                lblMessage.Text = string.Empty;
                lblMessage.ForeColor = Color.Empty;

            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error: " + ex.Message;
                lblMessage.ForeColor = Color.Red;
            }
        }

        protected void chkUseHashedMachineCodes_CheckedChanged(object sender, EventArgs e)
        {
            btnGetValidationData_Click(null, null);
        }

        void InitSettingFilesDropDown()
        {
            cmbSettingFiles.Items.Clear();
            foreach (string file in Directory.GetFiles(install.basePath, "*.xml"))
            {
                cmbSettingFiles.Items.Add(Path.GetFileName(file));
            }
            cmbSettingFiles.SelectedIndex = 0;
        }


    }
}
