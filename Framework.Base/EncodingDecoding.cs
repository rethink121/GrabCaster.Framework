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
