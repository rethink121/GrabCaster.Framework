// Form1.cs
// 
// Copyright (c) 2014-2016, Nino Crudele <nino dot crudele at live dot com>
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
// 
//   - Redistributions of source code must retain the above copyright notice,
//     this list of conditions and the following disclaimer.
//   - Redistributions in binary form must reproduce the above copyright
//     notice, this list of conditions and the following disclaimer in the
//     documentation and/or other materials provided with the distribution.
//   
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
// ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE
// LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
// CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
// SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
// INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN
// CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
// ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
// POSSIBILITY OF SUCH DAMAGE.

#region Usings

using System;
using System.IO;
using System.Security.Cryptography;
using System.Windows.Forms;

#endregion

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