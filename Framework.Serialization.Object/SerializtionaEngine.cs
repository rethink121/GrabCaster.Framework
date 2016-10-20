// --------------------------------------------------------------------------------------------------
// <copyright file = "SerializtionaEngine.cs" company="GrabCaster Ltd">
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

namespace GrabCaster.Framework.Serialization.Object
{
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text;

    /// <summary>
    ///     Serialization engine class
    /// </summary>
    public static class SerializationEngine
    {
        /// <summary>
        /// serialize object to Array
        /// </summary>
        /// <param name="objectData">
        /// Object to serialize
        /// </param>
        /// <returns>
        /// Return a byte array
        /// </returns>
        public static byte[] ObjectToByteArray(object objectData)
        {
            if (objectData == null)
            {
                return null;
            }

            var binaryFormatter = new BinaryFormatter();
            var memoryStream = new MemoryStream();
            binaryFormatter.Serialize(memoryStream, objectData);
            return memoryStream.ToArray();
        }

        /// <summary>
        /// Serialize Array to Object
        /// </summary>
        /// <param name="arrayBytes">
        /// Array byte to deserialize
        /// </param>
        /// <returns>
        /// Object deserialized
        /// </returns>
        public static object ByteArrayToObject(byte[] arrayBytes)
        {
            if (arrayBytes == null)
            {
                return EncodingDecoding.EncodingString2Bytes(string.Empty);
            }

            var memoryStream = new MemoryStream();
            var binaryFormatter = new BinaryFormatter();
            memoryStream.Write(arrayBytes, 0, arrayBytes.Length);
            memoryStream.Seek(0, SeekOrigin.Begin);
            var obj = binaryFormatter.Deserialize(memoryStream);
            return obj;
        }
    }
}