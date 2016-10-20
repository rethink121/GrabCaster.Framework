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
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BTSG.TestPipeline
{
  public partial class flatViewer : Form
  {
    public string text = "";
    public flatViewer()
    {
      InitializeComponent();
    }

    private void close_Click(object sender, EventArgs e)
    {
      this.Close();

    }

    private void flatViewer_Load(object sender, EventArgs e)
    {
      txtflat.Text = text;
    }
  }
}
