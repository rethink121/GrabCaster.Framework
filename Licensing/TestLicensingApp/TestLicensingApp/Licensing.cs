using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestLicensingApp
{
    using System.Net.Mime;
    using System.Windows.Forms;

    using LogicNP.CryptoLicensing;

    public class Licensing
    {
        CryptoLicense CreateLicense()
        {
            CryptoLicense ret = new CryptoLicense();

            //Uses the validation key of the "samples.netlicproj" license project file from the "Samples" directory
            // Get the validation key using Ctrl+K in the Generator UI.
            ret.ValidationKey = "AMAAMACTpBpgKZVChC9PEsB9elxMi/wlCgoiEoAqMXMZR4EuOkUe+cwwTm2hnaNREzEQhjsDAAEAAQ==";

            // *** IMPORTANT: Set the LicenseServiceURL property below to the URL of your license service
            // See video tutorial at http://www.ssware.com/cryptolicensing/demo/license_service_net.htm to learn 
            // how to create the license service
            ret.LicenseServiceURL = ""; // your license service URL here!

            return ret;
        }
        public bool EvaluateLicense()
        {
            /*
             This code demonstrates typically evaluation license scenario. The idea is as follows....
             First you check if a full license is present (using CryptoLicense.Load method). 
             If .Load returns false, you switch to a hardcoded evaluation license code (using .LicenseCode property). 
             Then, you validate using .Status property.

             Even if user uninstalls, all the usage data associated which each license code remains in the 
             registry and if user reinstalls, the evaluation continues from where it left off.
             */
            var licenseValid = false;

            CryptoLicense license = CreateLicense();

            // The license will be loaded from/saved to the registry
            license.StorageMode = LicenseStorageMode.ToRegistry;

            // To avoid conflicts with other scenarios from this sample, the default load/save registry key is changed
            license.RegistryStoragePath = license.RegistryStoragePath + "EvalLicense";

            // The remove method can be useful during development and testing - it deletes a previously saved license.
            //license.Remove();

            // Another useful method during development and testing is .ResetEvaluationInfo()



            // Load the license from the registry 
            bool loadDialog = !license.Load() || license.Status != LicenseStatus.Valid;
            
            if (loadDialog)
            {
                string dialogMessage = !license.Load()?"Licensing missing, enter the license key": license.Status != LicenseStatus.Valid?"Licensing expired, enter a new license key": "Licensing missing, enter the license key";
                // When app runs for first time, the load will fail, so specify an evaluation code....
                // This license code was generated from the Generator UI with a "Limit Usage Days To" setting of 30 days.
                LicenseForm licenseForm = new LicenseForm();
                licenseForm.labelMessage.Text = dialogMessage;
                if (licenseForm.ShowDialog() == DialogResult.OK)
                {

                    string licenseKey= licenseForm.textBoxLicense.Text;
                    license.LicenseCode = licenseKey;
                    // Save it so that it will get loaded the next time app runs
 

                    if (license.Status != LicenseStatus.Valid)
                    {
                        licenseValid = false;

                    }
                    else
                    {
                        license.Save();
                        licenseValid = true;
                    }
                }
                else
                {
                    Environment.Exit(0);
                }
            }

            if (license.Status != LicenseStatus.Valid)
            {
                licenseValid = false;

            }
            else
            {
                licenseValid = true;
            }



            return licenseValid;
            // ShowEvaluationInfoDialog shows the dialog only if the license specifies evaluation limits 
            if (license.ShowEvaluationInfoDialog("GrabCaster", "http://www.grabcaster.io") == false)
            {
                // license has expired, new license entered is also expired 
                // or user choose the 'Exit Program' option 
                licenseValid =  false;

                // In your app, you may wish to exit app when eval license has expired
                Application.Exit();
            }
            else
            {
                // The current license is valid or the new license entered is valid 
                // or the user choose the 'Continue Evaluation' option 

                // If the user enters a new valid license code, it replaces the existing code  
                // and is automatically saved to the currently specified  
                // storage medium (registry in this sample) using the CryptoLicense.Save method.  
                // The new license code is thus available the next time your software runs. 

                // We still need to check whether the license is an evaluation license 
                if (license.IsEvaluationLicense() == true)
                {
                    // reduce functionality in evaluation version if so desired 
                    licenseValid = true;
                }
            }

            license.Dispose(); // Be sure to call Dispose during app exit or when done using the CryptoLicense object
            return licenseValid;
        }
    }
}
