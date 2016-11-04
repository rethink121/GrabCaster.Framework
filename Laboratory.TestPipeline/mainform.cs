// mainform.cs
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
using System.Windows.Forms;
using GrabCaster.BizTalk.Extensibility;
using GrabCaster.Framework.Base;

#endregion

namespace BTSG.TestPipeline
{
    public partial class mainform : Form
    {
        public string prpassdirectory;
        public string prpassemblyfile;
        public string prpfilename;
        public string prpinputinstance;
        public string prppipetype;

        public mainform(string commands)
        {
            InitializeComponent();


            string[] args = commands.Split('&');


            prpassemblyfile = args[0];
            prpassdirectory = args[1];
            prppipetype = args[2];
            prpinputinstance = args[3];
            prpfilename = args[4];


            //foreach (string s in args)
            //{
            //  MessageBox.Show(s);
            //}
        }

        private void mainform_Load(object sender, EventArgs e)
        {
            Text = prppipetype;
            lbname.Text = "Pipeline: " + prppipetype;
        }

        private void testPipeline_Click(object sender, EventArgs e)
        {
        }

        private void btngo_Click(object sender, EventArgs e)
        {
            prpassemblyfile =
                @"C:\Users\Nino\Documents\GrabCaster\GrabCaster.Framework\BizTalk.Laoratory\PipelineLaboratory\PipelineLaboratory\bin\Debug\PipelineLaboratory.dll";
            prppipetype = "PipelineLaboratory.ReceivePipelineFlat";
            prpinputinstance =
                @"C:\Users\Nino\Documents\GrabCaster\GrabCaster.Framework\BizTalk.Laoratory\PipelineLaboratory\PipelineLaboratory\TestFile.txt";
            prpfilename =
                @"C:\Users\Nino\Documents\GrabCaster\GrabCaster.Framework\BizTalk.Laoratory\PipelineLaboratory\PipelineLaboratory\ReceivePipelineFlat.btp";


            byte[] content = BizTalkPipelines.ExecutePipeline(prpassemblyfile, prpassemblyfile, prppipetype,
                prpinputinstance, prpfilename);
            string s = EncodingDecoding.EncodingBytes2String(content);
            MessageBox.Show(s);
        }

        private void btnclose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            OpenFileDialog opd = new OpenFileDialog();

            DialogResult result = opd.ShowDialog(); // Show the dialog.

            if (result == DialogResult.OK) // Test result.
            {
                prpinputinstance = opd.FileName;
                lbfile.Text = "Instance: " + Path.GetFileName(prpinputinstance);
            }
        }
    }
}