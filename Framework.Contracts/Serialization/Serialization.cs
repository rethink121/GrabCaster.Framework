// Serialization.cs
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

using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;
using GrabCaster.Framework.Base;

#endregion

namespace GrabCaster.Framework.Contracts.Serialization
{
    /// <summary>
    ///     Serialization engine class
    /// </summary>
    public static class Serialization
    {
        /// <summary>
        ///     The object to byte array no compressed.
        /// </summary>
        /// <param name="objectData">
        ///     The object data.
        /// </param>
        /// <returns>
        ///     The
        ///     <see>
        ///         <cref>byte[]</cref>
        ///     </see>
        ///     .
        /// </returns>
        public static byte[] ObjectToByteArrayNoCompressed(object objectData)
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
        ///     The byte array to objectold.
        /// </summary>
        /// <param name="bytesArray">
        ///     The bytes array.
        /// </param>
        /// <returns>
        ///     The <see cref="object" />.
        /// </returns>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly",
             Justification = "Reviewed. Suppression is OK here.")]
        public static object ByteArrayToObjectold(byte[] bytesArray)
        {
            if (bytesArray == null)
            {
                return EncodingDecoding.EncodingString2Bytes(string.Empty);
            }

            var memoryStream = new MemoryStream();
            var binaryFormatter = new BinaryFormatter();
            memoryStream.Write(bytesArray, 0, bytesArray.Length);
            memoryStream.Seek(0, SeekOrigin.Begin);
            var obj = binaryFormatter.Deserialize(memoryStream);
            return obj;
        }

        /// <summary>
        ///     The object to byte array.
        /// </summary>
        /// <param name="objectData">
        ///     The object data.
        /// </param>
        /// <returns>
        ///     The
        ///     <see>
        ///         <cref>byte[]</cref>
        ///     </see>
        ///     .
        /// </returns>
        /// <exception cref="Exception">
        /// </exception>
        public static byte[] ObjectToByteArray(object objectData)
        {
            var objStream = new MemoryStream();
            var bf = new BinaryFormatter();
            var ms = new MemoryStream();

            bf.Serialize(ms, objectData);

            var objDeflated = new DeflateStream(objStream, CompressionMode.Compress);

            objDeflated.Write(ms.GetBuffer(), 0, (int) ms.Length);
            objDeflated.Flush();
            objDeflated.Close();

            return objStream.ToArray();
        }

        /// <summary>
        ///     The byte array to object.
        /// </summary>
        /// <param name="byteArray">
        ///     The byte array.
        /// </param>
        /// <returns>
        ///     The <see cref="object" />.
        /// </returns>
        public static object ByteArrayToObject(byte[] byteArray)
        {
            var inMs = new MemoryStream(byteArray);
            inMs.Seek(0, 0);
            var zipStream = new DeflateStream(inMs, CompressionMode.Decompress, true);

            var outByt = ReadFullStream(zipStream);
            zipStream.Flush();
            zipStream.Close();

            var outMs = new MemoryStream(outByt);
            outMs.Seek(0, 0);

            var bf = new BinaryFormatter();

            object outObject = (DataTable) bf.Deserialize(outMs, null);

            return outObject;
        }

        /// <summary>
        ///     The data table to byte array.
        /// </summary>
        /// <param name="dataTable">
        ///     The data table.
        /// </param>
        /// <returns>
        ///     The
        ///     <see>
        ///         <cref>byte[]</cref>
        ///     </see>
        ///     .
        /// </returns>
        /// <exception cref="Exception">
        /// </exception>
        public static byte[] DataTableToByteArray(DataTable dataTable)
        {
            var objStream = new MemoryStream();
            dataTable.RemotingFormat = SerializationFormat.Binary;
            var bf = new BinaryFormatter();
            var ms = new MemoryStream();

            bf.Serialize(ms, dataTable);

            var objDeflated = new DeflateStream(objStream, CompressionMode.Compress);

            objDeflated.Write(ms.GetBuffer(), 0, (int) ms.Length);
            objDeflated.Flush();
            objDeflated.Close();

            return objStream.ToArray();
        }

        /// <summary>
        ///     The byte array to data table.
        /// </summary>
        /// <param name="byteDataTable">
        ///     The byte data table.
        /// </param>
        /// <returns>
        ///     The <see cref="DataTable" />.
        /// </returns>
        public static DataTable ByteArrayToDataTable(byte[] byteDataTable)
        {
            var outDs = new DataTable();

            var inMs = new MemoryStream(byteDataTable);
            inMs.Seek(0, 0);
            var zipStream = new DeflateStream(inMs, CompressionMode.Decompress, true);

            var outByt = ReadFullStream(zipStream);
            zipStream.Flush();
            zipStream.Close();

            var outMs = new MemoryStream(outByt);
            outMs.Seek(0, 0);
            outDs.RemotingFormat = SerializationFormat.Binary;
            var bf = new BinaryFormatter();

            outDs = (DataTable) bf.Deserialize(outMs, null);

            return outDs;
        }

        /// <summary>
        ///     The read full stream.
        /// </summary>
        /// <param name="stream">
        ///     The stream.
        /// </param>
        /// <returns>
        ///     The
        ///     <see>
        ///         <cref>byte[]</cref>
        ///     </see>
        ///     .
        /// </returns>
        private static byte[] ReadFullStream(Stream stream)
        {
            var buffer = new byte[32768];

            using (var ms = new MemoryStream())
            {
                while (true)
                {
                    var read = stream.Read(buffer, 0, buffer.Length);
                    if (read <= 0)
                    {
                        return ms.ToArray();
                    }

                    ms.Write(buffer, 0, read);
                }
            }
        }
    }
}