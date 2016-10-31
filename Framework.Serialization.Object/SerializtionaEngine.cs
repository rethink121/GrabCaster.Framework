// SerializtionaEngine.cs
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

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using GrabCaster.Framework.Base;

#endregion

namespace GrabCaster.Framework.Serialization.Object
{
    /// <summary>
    ///     Serialization engine class
    /// </summary>
    public static class SerializationEngine
    {
        /// <summary>
        ///     serialize object to Array
        /// </summary>
        /// <param name="objectData">
        ///     Object to serialize
        /// </param>
        /// <returns>
        ///     Return a byte array
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
        ///     Serialize Array to Object
        /// </summary>
        /// <param name="arrayBytes">
        ///     Array byte to deserialize
        /// </param>
        /// <returns>
        ///     Object deserialized
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