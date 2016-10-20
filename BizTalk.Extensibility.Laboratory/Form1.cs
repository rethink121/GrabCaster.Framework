using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Xsl;
using GrabCaster.BizTalk.Extensibility;
using GrabCaster.Framework.Base;
using Microsoft.BizTalk.Message.Interop;
using Microsoft.XLANGs.BaseTypes;


namespace GrabCaster.BizTalk.Extensibility.Laboratory
{
    public partial class Form1 : Form
    {
        public string prpassemblyfile;
        public string prpassdirectory;
        public string prppipetype;
        public string prpinputinstance;
        public string prpfilename;

        public Form1()
        {
            InitializeComponent();
        }



        private void buttonTry_Click(object sender, EventArgs e)
        {
            prpassemblyfile =@"C:\Users\Nino\Documents\GrabCaster\GrabCaster.Framework\BizTalk.Laoratory\PipelineLaboratory\PipelineLaboratory\bin\Debug\PipelineLaboratory.dll";
            prppipetype = "PipelineLaboratory.ReceivePipelineFlat";
            prpinputinstance =
                @"C:\Users\Nino\Documents\GrabCaster\GrabCaster.Framework\BizTalk.Laoratory\PipelineLaboratory\PipelineLaboratory\TestFile.txt";
            prpfilename = @"C:\Users\Nino\Documents\GrabCaster\GrabCaster.Framework\BizTalk.Laoratory\PipelineLaboratory\PipelineLaboratory\ReceivePipelineFlat.btp";
            byte[] content = BizTalkPipelines.ExecutePipeline(prpassemblyfile, prpassemblyfile, prppipetype, prpinputinstance, prpfilename);
            string s = EncodingDecoding.EncodingBytes2String(content);
            MessageBox.Show(s);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //string inputinstance = File.ReadAllText("");
            //IBaseMessage inputMessage = MessageHelper.CreateFromStream(BizTalkPipelines.LoadMessage(inputinstance));
            //if (inputMessage == null)
            //    throw new ArgumentNullException("pInMsg");
            //TransformBase transform = BizTalkTransform.CreateMapInstance();
            //Stream newMessageData = BizTalkTransform.MapMessage(inputMessage.BodyPart.GetOriginalDataStream(), transform);

            //StreamReader reader = new StreamReader(newMessageData);
            //string contentRp = reader.ReadToEnd();

            //byte[] returnContent = System.Text.EncodingDecoding.EncodingString2Bytes(contentRp);
            string s = LoadInternalArtifactData(
                @"C:\Users\Nino\Documents\GrabCaster\GrabCaster.Framework\BizTalk.Laoratory\PipelineLaboratory\PipelineLaboratory\bin\Debug\PipelineLaboratory.dll",
                "PipelineLaboratory.Map1",
                true);

        }

        private string LoadInternalArtifactData(string parentAssemblyName, string artypename, bool isasslocation)
        {


            try
            {


                Assembly asm = Assembly.LoadFrom(parentAssemblyName);

                Type t = asm.GetType(artypename);
                if (t == null)
                    return artypename + " no found in assembly " + parentAssemblyName;
                object o = asm.CreateInstance(artypename);


                PropertyInfo pi = o.GetType().GetProperty("XmlContent", BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance);
                object viewData = pi.GetValue(o, null);
                string tmpViewData = viewData != null ? viewData.ToString() : string.Empty;
                string dataXslt = tmpViewData.Replace("utf-16", "utf-8");

                XslCompiledTransform proc = new XslCompiledTransform();

                using (StringReader sr = new StringReader(dataXslt))
                {
                    using (XmlReader xr = XmlReader.Create(sr))
                    {
                        proc.Load(xr);
                    }
                }

                string resultXML;

                string xmlData = File.ReadAllText("c:\\demoxml.txt");
                using (StringReader sr = new StringReader(xmlData))
                {
                    using (XmlReader xr = XmlReader.Create(sr))
                    {
                        using (StringWriter sw = new StringWriter())
                        {
                            proc.Transform(xr, null, sw);
                            resultXML = sw.ToString();
                        }
                    }
                }

                return resultXML;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }


        }



    }
}
