namespace GrabCaster.InternalLaboratory
{
    partial class Form1
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
            this.buttonAES = new System.Windows.Forms.Button();
            this.buttonAESBytes = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonAES
            // 
            this.buttonAES.Location = new System.Drawing.Point(12, 12);
            this.buttonAES.Name = "buttonAES";
            this.buttonAES.Size = new System.Drawing.Size(243, 35);
            this.buttonAES.TabIndex = 0;
            this.buttonAES.Text = "AES String";
            this.buttonAES.UseVisualStyleBackColor = true;
            this.buttonAES.Click += new System.EventHandler(this.buttonAES_Click);
            // 
            // buttonAESBytes
            // 
            this.buttonAESBytes.Location = new System.Drawing.Point(12, 53);
            this.buttonAESBytes.Name = "buttonAESBytes";
            this.buttonAESBytes.Size = new System.Drawing.Size(243, 35);
            this.buttonAESBytes.TabIndex = 1;
            this.buttonAESBytes.Text = "AES Bytes";
            this.buttonAESBytes.UseVisualStyleBackColor = true;
            this.buttonAESBytes.Click += new System.EventHandler(this.buttonAESBytes_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(594, 298);
            this.Controls.Add(this.buttonAESBytes);
            this.Controls.Add(this.buttonAES);
            this.Name = "Form1";
            this.Text = "Laboratory";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonAES;
        private System.Windows.Forms.Button buttonAESBytes;
    }
}

