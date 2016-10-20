namespace BTSG.TestPipeline
{
  partial class flatViewer
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
      this.txtflat = new System.Windows.Forms.TextBox();
      this.close = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // txtflat
      // 
      this.txtflat.Dock = System.Windows.Forms.DockStyle.Top;
      this.txtflat.Location = new System.Drawing.Point(0, 0);
      this.txtflat.Multiline = true;
      this.txtflat.Name = "txtflat";
      this.txtflat.Size = new System.Drawing.Size(784, 604);
      this.txtflat.TabIndex = 0;
      // 
      // close
      // 
      this.close.Location = new System.Drawing.Point(341, 619);
      this.close.Name = "close";
      this.close.Size = new System.Drawing.Size(75, 23);
      this.close.TabIndex = 1;
      this.close.Text = "Close";
      this.close.UseVisualStyleBackColor = true;
      this.close.Click += new System.EventHandler(this.close_Click);
      // 
      // flatViewer
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(784, 654);
      this.Controls.Add(this.close);
      this.Controls.Add(this.txtflat);
      this.Name = "flatViewer";
      this.Text = "Flat Viewer";
      this.Load += new System.EventHandler(this.flatViewer_Load);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TextBox txtflat;
    private System.Windows.Forms.Button close;
  }
}