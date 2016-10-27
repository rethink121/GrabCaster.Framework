// EncodingDecoding.cs
// 
// Copyright (c) 2014-2016, Nino Crudle <nino dot crudele at live dot com>
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrabCaster.Framework.Common;

namespace GrabCaster.Framework.Base
{
    public enum EncodingType
    {
        UTF8,
        ASCII,
        BigEndianUnicode,
        Default,
        UTF32,
        UTF7,
        Unicode
    }
    public static class EncodingDecoding
    {
        /// <summary>
        /// The last error point
        /// </summary>
        public static string EncodingBytes2String(byte[] value)
        {
            EncodingType encodingType = ConfigurationBag.Configuration.EncodingType;
            switch (encodingType)
            {
                case EncodingType.UTF8:
                    return Encoding.UTF8.GetString(value);
                case EncodingType.ASCII:
                    return Encoding.ASCII.GetString(value);
                case EncodingType.BigEndianUnicode:
                    return Encoding.BigEndianUnicode.GetString(value);
                case EncodingType.Default:
                    return Encoding.Default.GetString(value);
                case EncodingType.UTF32:
                    return Encoding.UTF32.GetString(value);
                case EncodingType.UTF7:
                    return Encoding.UTF7.GetString(value);
                case EncodingType.Unicode:
                    return Encoding.Unicode.GetString(value);
                default:
                    throw new ArgumentOutOfRangeException(nameof(encodingType), encodingType, null);
            }
        }
        /// <summary>
        /// The last error point
        /// </summary>
        public static byte[] EncodingString2Bytes(string value)
        {
            EncodingType encodingType = ConfigurationBag.Configuration.EncodingType;
            switch (encodingType)
            {
                case EncodingType.UTF8:
                    return Encoding.UTF8.GetBytes(value);
                case EncodingType.ASCII:
                    return Encoding.ASCII.GetBytes(value);
                case EncodingType.BigEndianUnicode:
                    return Encoding.BigEndianUnicode.GetBytes(value);
                case EncodingType.Default:
                    return Encoding.Default.GetBytes(value);
                case EncodingType.UTF32:
                    return Encoding.UTF32.GetBytes(value);
                case EncodingType.UTF7:
                    return Encoding.UTF7.GetBytes(value);
                case EncodingType.Unicode:
                    return Encoding.Unicode.GetBytes(value);
                default:
                    throw new ArgumentOutOfRangeException(nameof(encodingType), encodingType, null);
            }
        }
    }
}
