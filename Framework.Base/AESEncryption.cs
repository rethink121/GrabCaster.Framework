// AESEncryption.cs
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
using System.Linq;
using System.Security.Cryptography;

#endregion

namespace GrabCaster.Framework.Base
{
    public class AesEncryption
    {
        private static string SecurityFile = "KeyFile.gck";

        public static bool CreateSecurityKey_Aes()
        {
            byte[] Key = new byte[32];
            byte[] IV = new byte[16];
            bool ret = false;

            // Create an AesManaged object
            // with the specified key and IV.
            using (AesManaged aesAlg = new AesManaged())
            {
                byte[] keyContent = aesAlg.Key.Concat(aesAlg.IV).ToArray();
                File.WriteAllBytes(GetSecurityFileName_Aes(), keyContent);
                string[] filesConf = Directory.GetFiles(ConfigurationBag.Configuration.BaseDirectory, "*.cfg");
                foreach (var item in filesConf)
                {
                    byte[] content = File.ReadAllBytes(item);
                    byte[] contentCrypted = EncryptByteToBytes_Aes(content);
                    File.WriteAllBytes(item, contentCrypted);
                }
            }
            ret = true;


            return ret;
        }

        public static bool DeleteSecurityKey_Aes()
        {
            byte[] Key = new byte[32];
            byte[] IV = new byte[16];
            bool ret = false;


            // Create an AesManaged object
            // with the specified key and IV.
            using (AesManaged aesAlg = new AesManaged())
            {
                byte[] keyContent = aesAlg.Key.Concat(aesAlg.IV).ToArray();
                File.WriteAllBytes(GetSecurityFileName_Aes(), keyContent);
                string[] filesConf = Directory.GetFiles(ConfigurationBag.Configuration.BaseDirectory, "*.cfg");
                foreach (var item in filesConf)
                {
                    byte[] content = File.ReadAllBytes(item);
                    byte[] contentCrypted = EncryptByteToBytes_Aes(content);
                    File.WriteAllBytes(item, contentCrypted);
                }
            }
            ret = true;


            return ret;
        }

        public static bool SecurityOn_Aes()
        {
            return File.Exists(GetSecurityFileName_Aes());
        }

        public static string GetSecurityFileName_Aes()
        {
            string secFile = Path.Combine(ConfigurationBag.Configuration.BaseDirectory, SecurityFile);
            return secFile;
        }

        public static byte[] GetSecurityContent_Aes()
        {
            return File.ReadAllBytes(GetSecurityFileName_Aes());
        }

        public static byte[] EncryptByteToBytes_Aes(byte[] byteContent)
        {
            byte[] encrypted = null;


            // Create an AesManaged object
            // with the specified key and IV.

            using (AesManaged aesAlg = new AesManaged())
            {
                byte[] Key = new byte[32];
                byte[] IV = new byte[16];

                byte[] contentKey = GetSecurityContent_Aes();
                Array.Copy(contentKey, 0, Key, 0, 32);
                Array.Copy(contentKey, 32, IV, 0, 16);

                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        csEncrypt.Write(byteContent, 0, byteContent.Length);
                        csEncrypt.FlushFinalBlock();
                        encrypted = msEncrypt.ToArray();
                        msEncrypt.Close();
                        csEncrypt.Close();
                    }
                }
            }
            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }

        public static byte[] DecryptByteFromBytes_Aes(byte[] cipherText)
        {
            // the decrypted byte array.
            byte[] deCipherText = new byte[cipherText.Length];
            ;

            // Create an AesManaged object
            // with the specified key and IV.

            using (AesManaged aesAlg = new AesManaged())
            {
                byte[] Key = new byte[32];
                byte[] IV = new byte[16];
                byte[] contentKey = GetSecurityContent_Aes();
                Array.Copy(contentKey, 0, Key, 0, 32);
                Array.Copy(contentKey, 32, IV, 0, 16);

                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        csDecrypt.Read(deCipherText, 0, deCipherText.Length);
                        msDecrypt.Close();
                        csDecrypt.Close();
                    }
                }
            }
            return deCipherText;
        }
    }
}