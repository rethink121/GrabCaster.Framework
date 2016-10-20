namespace TestLicensingApp
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
            this.buttonTestLicense = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonTestLicense
            // 
            this.buttonTestLicense.Location = new System.Drawing.Point(39, 24);
            this.buttonTestLicense.Name = "buttonTestLicense";
            this.buttonTestLicense.Size = new System.Drawing.Size(180, 23);
            this.buttonTestLicense.TabIndex = 0;
            this.buttonTestLicense.Text = "Test License";
            this.buttonTestLicense.UseVisualStyleBackColor = true;
            this.buttonTestLicense.Click += new System.EventHandler(this.buttonTestLicense_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(626, 345);
            this.Controls.Add(this.buttonTestLicense);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonTestLicense;
    }
}

