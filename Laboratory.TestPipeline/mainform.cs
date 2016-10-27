using Microsoft.BizTalk.Message.Interop;
//---------------------------------------------------------------------------------
// Copyright (c) 2014, Nino Crudele
//
// Blog: http://ninocrudele.me
//---------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using GrabCaster.BizTalk.Extensibility;
using GrabCaster.Framework.Base;

namespace BTSG.TestPipeline
{
  public partial class mainform : Form
  {
    public string prpassemblyfile;
    public string prpassdirectory;
    public string prppipetype;
    public string prpinputinstance;
    public string prpfilename;

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
      this.Text = prppipetype;
      this.lbname.Text = "Pipeline: " + prppipetype;

    }

    private void testPipeline_Click(object sender, EventArgs e)
    {
    

    }

    private void btngo_Click(object sender, EventArgs e)
    {

      prpassemblyfile = @"C:\Users\Nino\Documents\GrabCaster\GrabCaster.Framework\BizTalk.Laoratory\PipelineLaboratory\PipelineLaboratory\bin\Debug\PipelineLaboratory.dll";
      prppipetype = "PipelineLaboratory.ReceivePipelineFlat";
      prpinputinstance =
          @"C:\Users\Nino\Documents\GrabCaster\GrabCaster.Framework\BizTalk.Laoratory\PipelineLaboratory\PipelineLaboratory\TestFile.txt";
      prpfilename = @"C:\Users\Nino\Documents\GrabCaster\GrabCaster.Framework\BizTalk.Laoratory\PipelineLaboratory\PipelineLaboratory\ReceivePipelineFlat.btp";


      byte[] content = BizTalkPipelines.ExecutePipeline(prpassemblyfile, prpassemblyfile, prppipetype, prpinputinstance, prpfilename);
      string s = EncodingDecoding.EncodingBytes2String(content);
      MessageBox.Show(s);


    }

    private void btnclose_Click(object sender, EventArgs e)
    {
      this.Close();
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
      else
      { return; }
      
    }


  }
}
