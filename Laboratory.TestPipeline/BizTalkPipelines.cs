﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using BTSGBizTalkAddins;
using Microsoft.BizTalk.Message.Interop;

namespace BTSG.TestPipeline
{
    public static class BizTalkPipelines
    {

        public static byte[] ExecutePipeline(string assemblyfile, string assdirectory, string pipetype, string inputinstance, string filename)
        {
            IBaseMessage inputMessage = MessageHelper.CreateFromStream(TestMessageHelper.LoadMessage(inputinstance));
            Assembly btAssembly;
            btAssembly = Assembly.LoadFrom(assemblyfile);
            string AssemblyName = btAssembly.FullName;

            Type t = btAssembly.GetType(pipetype);

            //set pype type
            int first = 0;
            int sec = 0;
            string pipelinetype = GetPipeTypeRecOrSend(filename);
            Type tschema = null;
            List<string> values = null;

            //Get spec doc
            string specdoc = "";
            string assname = "";

            MessageCollection rppoutputMessages = null;
            IBaseMessage sppoutputMessages = null;
            StreamReader rdr = null;
            byte[] returnContent = null;
            try
            {
                switch (pipelinetype)
                {
                    case "ReceivePipeline":
                        RWPipeline rppwrapper = MainPipelineHelper.RetReceivePipeline(t);
                        values = getspecandassnamefrompipeline(filename,
                            "/Document/Stages/Stage/Components/Component/Properties/Property[@Name='DocumentSpecName']/Value",
                            assemblyfile, assdirectory);

                        foreach (string s in values)
                        {
                            specdoc = s.Substring(0, s.IndexOf('|'));
                            assname = s.Substring(s.IndexOf('|') + 1);

                            Assembly prjAssembly = Assembly.LoadFrom(assname);
                            tschema = prjAssembly.GetType(specdoc);

                            if (tschema == null)
                            {
                                MessageBox.Show("Cannot find schema document in " + assname);
                                returnContent = null;
                            }
                        }


                        rppwrapper.AddDocSpec(tschema);
                        rppoutputMessages = rppwrapper.Execute(inputMessage);
                        rdr = new StreamReader(rppoutputMessages[0].BodyPart.Data);
                        string contentRp = rdr.ReadToEnd();
                        returnContent = System.Text.Encoding.UTF8.GetBytes(contentRp);
                        break;
                    case "SendPipeline":
                        SWPipeline sppwrapper = MainPipelineHelper.RetSendPipeline(t);
                        values = getspecandassnamefrompipeline(filename,
                            "/Document/Stages/Stage/Components/Component/Properties/Property[@Name='DocumentSpecName']/Value",
                            assemblyfile, assdirectory);

                        foreach (string s in values)
                        {
                            specdoc = s.Substring(0, s.IndexOf('|'));
                            assname = s.Substring(s.IndexOf('|') + 1);

                            Assembly prjAssembly = Assembly.LoadFrom(assname);
                            tschema = prjAssembly.GetType(specdoc);

                            if (tschema == null)
                            {
                                MessageBox.Show("Cannot find schema document in " + assname);
                                returnContent = null;
                            }
                        }

                        sppwrapper.AddDocSpec(tschema);
                        sppoutputMessages = sppwrapper.Execute(inputMessage);
                        rdr = new StreamReader(sppoutputMessages.BodyPart.Data);
                        string contentSp = rdr.ReadToEnd();
                        returnContent = System.Text.Encoding.UTF8.GetBytes(contentSp);
                        break;
                    default:
                        MessageBox.Show("Select a correct pipeline.");
                        returnContent = null;
                        break;

                        //***********************************************************

                }

                return returnContent;
            }
            catch (Exception ex)
            {
                string fileresult = assdirectory + Path.GetFileNameWithoutExtension(inputinstance) + "_" +
                                    Guid.NewGuid().ToString() + "txt";
                File.WriteAllText(fileresult, "Error: " + ex.Message + "\r\n\r\n" + ex.StackTrace);
                System.Diagnostics.Process.Start("IExplore.exe", fileresult);
                return returnContent;
            }

        }

        private static  List<string> getspecandassnamefrompipeline(string xmlfile, string pathnodes, string assemblypath, string assdirectory)
        {
            List<string> values = new List<string>();

            XmlDocument doc = new XmlDocument();

            //string contents = File.ReadAllText(@"C:\Users\Administrator\Documents\London 2014 Session\Sources\BizTalk Server Project Test\BizTalk Server Project pipelines\ReceivePipeline external.btp");
            string contents = File.ReadAllText(xmlfile);
            contents.Replace("encoding=\"utf-16\"", "encoding=\"utf-8\"");

            doc.LoadXml(contents);
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
            nsmgr.AddNamespace("xsd", "http://www.w3.org/2001/XMLSchema");
            //nsmgr.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");

            //  XmlNodeList nodelist = doc.SelectNodes("/Document/Stages/Stage/Components/Component/Properties/Property[@Name='DocumentSpecName']/Value", nsmgr);
            XmlNodeList nodelist = doc.SelectNodes(pathnodes, nsmgr);

            if (nodelist.Count == 0)
            {
                pathnodes = pathnodes.Replace("DocumentSpecName", "DocumentSpecNames");
                nodelist = doc.SelectNodes(pathnodes, nsmgr);
            }

            string docspec = "";
            string assname = "";

            foreach (XmlNode node in nodelist)
            {
                try
                {
                    int firstcomma = node.InnerText.IndexOf(',');
                    if (firstcomma < 0)
                    {
                        docspec = node.InnerText.Trim();
                        assname = assemblypath;

                    }
                    else
                    {
                        int secondcomma = node.InnerText.IndexOf(',', firstcomma + 1) - 1;

                        docspec = node.InnerText.Substring(0, node.InnerText.IndexOf(',')).Trim();
                        assname = assdirectory + node.InnerText.Substring(firstcomma + 1, secondcomma - firstcomma).Trim() + ".dll";
                    }
                    values.Add(docspec + "|" + assname);


                }
                catch { MessageBox.Show("Cannot extract docspec from pipeline."); }

            }
            return values;
        }


        public static string GetPipeTypeRecOrSend(string filename)
        {
            //Get pipetype
            string content = "";
            try
            {
                content = File.ReadAllText(filename + ".cs");
            }
            catch { MessageBox.Show("Compile the project."); }

            int first = 0;
            int sec = 0;
            first = content.IndexOf("namespace ");
            sec = content.IndexOf("{");
            string ppnamespace = content.Substring(first + "namespace ".Length, sec - "namespace ".Length).Trim();

            string ppclassname = "";
            string pptype = "";


            first = content.IndexOf(" : Microsoft.BizTalk.PipelineOM.");
            int firstgraph = content.IndexOf("{");
            sec = content.IndexOf("{", firstgraph + 2);
            string pppipelinetype = content.Substring(first + " : Microsoft.BizTalk.PipelineOM.".Length, sec - first - " : Microsoft.BizTalk.PipelineOM.".Length).Trim();

            return pppipelinetype;
        }

    }
}
