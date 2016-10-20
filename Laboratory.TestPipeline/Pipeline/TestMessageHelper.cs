//---------------------------------------------------------------------------------
// Copyright (c) 2014, Nino Crudele
//
// Blog: http://ninocrudele.me
//---------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace BTSG.TestPipeline
{
    /// <summary>
    /// Helper class for messages
    /// </summary>
    internal class TestMessageHelper
    {
        /// <summary>
        /// Saves a message to disk
        /// </summary>
        /// <param name="path"></param>
        /// <param name="data"></param>
        internal static void SaveMessage(string path, Stream data)
        {
            StreamReader rdr = new StreamReader(data);
            StreamWriter writer = new StreamWriter(path);
            writer.Write(rdr.ReadToEnd());
            writer.Flush();
            data.Seek(0, SeekOrigin.Begin);
            writer.Close();
        }
        /// <summary>
        /// Loads a message from disk
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        internal static Stream LoadMessage(string path)
        {
            StreamReader rdr = new StreamReader(path);
            return rdr.BaseStream;
        }
    }
}
