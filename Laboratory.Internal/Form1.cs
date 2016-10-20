// --------------------------------------------------------------------------------------------------
// <copyright file = "Form1.cs" company="GrabCaster Ltd">
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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GrabCaster.InternalLaboratory
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void buttonAES_Click(object sender, EventArgs e)
        {
            //string original = "Here is some data to encrypt!";

            //// Create a new instance of the AesManaged
            //// class.  This generates a new key and initialization 
            //// vector (IV).
            //using (AesManaged myAes = new AesManaged())
            //{

            //    // Encrypt the string to an array of bytes.
            //    byte[] encrypted = AESEncryption.EncryptStringToBytes_Aes(original, myAes.Key, myAes.IV);

            //    // Decrypt the bytes to a string.
            //    string roundtrip = AESEncryption.DecryptStringFromBytes_Aes(encrypted, myAes.Key, myAes.IV);

            //    //Display the original data and the decrypted data.
            //    MessageBox.Show($"Original:   {original}");
            //    MessageBox.Show($"Round Trip: {roundtrip}");
            //}
        }

        private void buttonAESBytes_Click(object sender, EventArgs e)
        {
            byte[] contentFile = File.ReadAllBytes("c:\\test.txt");

            // Create a new instance of the AesManaged
            // class.  This generates a new key and initialization 
            // vector (IV).
            using (AesManaged myAes = new AesManaged())
            {

                // Encrypt the string to an array of bytes.
               
        //        byte[] encrypted = AESEncryption.EncryptByteToBytes_Aes(contentFile, myAes.Key, myAes.IV);
         //       File.WriteAllBytes("c:\\testCrypted.txt", encrypted);

                // Decrypt the bytes to a string.
         //       byte[] decrypted = AESEncryption.DecryptByteFromBytes_Aes(encrypted, myAes.Key, myAes.IV);
          //      File.WriteAllBytes("c:\\testdeCrypted.txt", decrypted);

                //Display the original data and the decrypted data.
                MessageBox.Show("done");

            }
        }
    }
}
