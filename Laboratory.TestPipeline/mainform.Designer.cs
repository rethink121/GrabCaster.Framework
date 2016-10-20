namespace BTSG.TestPipeline
{
  partial class mainform
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(mainform));
      this.btngo = new System.Windows.Forms.PictureBox();
      this.btnclose = new System.Windows.Forms.PictureBox();
      this.directorySearcher1 = new System.DirectoryServices.DirectorySearcher();
      this.pictureBox1 = new System.Windows.Forms.PictureBox();
      this.lbname = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.pictureBox2 = new System.Windows.Forms.PictureBox();
      this.lbfile = new System.Windows.Forms.Label();
      ((System.ComponentModel.ISupportInitialize)(this.btngo)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.btnclose)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
      this.SuspendLayout();
      // 
      // btngo
      // 
      this.btngo.Cursor = System.Windows.Forms.Cursors.Hand;
      this.btngo.Image = ((System.Drawing.Image)(resources.GetObject("btngo.Image")));
      this.btngo.Location = new System.Drawing.Point(436, 122);
      this.btngo.Name = "btngo";
      this.btngo.Size = new System.Drawing.Size(41, 35);
      this.btngo.TabIndex = 1;
      this.btngo.TabStop = false;
      this.btngo.Click += new System.EventHandler(this.btngo_Click);
      // 
      // btnclose
      // 
      this.btnclose.Cursor = System.Windows.Forms.Cursors.Hand;
      this.btnclose.Image = ((System.Drawing.Image)(resources.GetObject("btnclose.Image")));
      this.btnclose.Location = new System.Drawing.Point(436, 163);
      this.btnclose.Name = "btnclose";
      this.btnclose.Size = new System.Drawing.Size(41, 35);
      this.btnclose.TabIndex = 2;
      this.btnclose.TabStop = false;
      this.btnclose.Click += new System.EventHandler(this.btnclose_Click);
      // 
      // directorySearcher1
      // 
      this.directorySearcher1.ClientTimeout = System.TimeSpan.Parse("-00:00:01");
      this.directorySearcher1.ServerPageTimeLimit = System.TimeSpan.Parse("-00:00:01");
      this.directorySearcher1.ServerTimeLimit = System.TimeSpan.Parse("-00:00:01");
      // 
      // pictureBox1
      // 
      this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
      this.pictureBox1.Location = new System.Drawing.Point(-39, 22);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new System.Drawing.Size(382, 219);
      this.pictureBox1.TabIndex = 3;
      this.pictureBox1.TabStop = false;
      // 
      // lbname
      // 
      this.lbname.AutoSize = true;
      this.lbname.Location = new System.Drawing.Point(4, 0);
      this.lbname.Name = "lbname";
      this.lbname.Size = new System.Drawing.Size(10, 13);
      this.lbname.TabIndex = 4;
      this.lbname.Text = " ";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(103, 43);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(332, 39);
      this.label2.TabIndex = 5;
      this.label2.Text = "1) Select pipeline instance using button below\r\n2) Attach BTSG TestPipeline proce" +
    "ss from Visual Studio\r\n3) Use Process Title to indentify differents pipelines te" +
    "st environments";
      // 
      // pictureBox2
      // 
      this.pictureBox2.Cursor = System.Windows.Forms.Cursors.Hand;
      this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
      this.pictureBox2.Location = new System.Drawing.Point(106, 96);
      this.pictureBox2.Name = "pictureBox2";
      this.pictureBox2.Size = new System.Drawing.Size(41, 35);
      this.pictureBox2.TabIndex = 6;
      this.pictureBox2.TabStop = false;
      this.pictureBox2.Click += new System.EventHandler(this.pictureBox2_Click);
      // 
      // lbfile
      // 
      this.lbfile.AutoSize = true;
      this.lbfile.Location = new System.Drawing.Point(4, 13);
      this.lbfile.Name = "lbfile";
      this.lbfile.Size = new System.Drawing.Size(10, 13);
      this.lbfile.TabIndex = 7;
      this.lbfile.Text = " ";
      // 
      // mainform
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.Color.White;
      this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
      this.ClientSize = new System.Drawing.Size(489, 204);
      this.Controls.Add(this.lbfile);
      this.Controls.Add(this.pictureBox2);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.lbname);
      this.Controls.Add(this.pictureBox1);
      this.Controls.Add(this.btnclose);
      this.Controls.Add(this.btngo);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "mainform";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Test Pipeline";
      this.Load += new System.EventHandler(this.mainform_Load);
      ((System.ComponentModel.ISupportInitialize)(this.btngo)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.btnclose)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.PictureBox btngo;
    private System.Windows.Forms.PictureBox btnclose;
    private System.DirectoryServices.DirectorySearcher directorySearcher1;
    private System.Windows.Forms.PictureBox pictureBox1;
    private System.Windows.Forms.Label lbname;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.PictureBox pictureBox2;
    private System.Windows.Forms.Label lbfile;
  }
}

