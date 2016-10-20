namespace TestLicensingApp
{
    partial class LicenseForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LicenseForm));
            this.buttonConfirm = new System.Windows.Forms.Button();
            this.textBoxLicense = new System.Windows.Forms.TextBox();
            this.labelMessage = new System.Windows.Forms.Label();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.linkLabelContact = new System.Windows.Forms.LinkLabel();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonConfirm
            // 
            this.buttonConfirm.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonConfirm.Location = new System.Drawing.Point(11, 109);
            this.buttonConfirm.Name = "buttonConfirm";
            this.buttonConfirm.Size = new System.Drawing.Size(75, 23);
            this.buttonConfirm.TabIndex = 0;
            this.buttonConfirm.Text = "Confirm";
            this.buttonConfirm.UseVisualStyleBackColor = true;
            this.buttonConfirm.Click += new System.EventHandler(this.buttonConfirm_Click);
            // 
            // textBoxLicense
            // 
            this.textBoxLicense.Location = new System.Drawing.Point(12, 83);
            this.textBoxLicense.Name = "textBoxLicense";
            this.textBoxLicense.Size = new System.Drawing.Size(665, 20);
            this.textBoxLicense.TabIndex = 1;
            // 
            // labelMessage
            // 
            this.labelMessage.AutoSize = true;
            this.labelMessage.Location = new System.Drawing.Point(11, 67);
            this.labelMessage.Name = "labelMessage";
            this.labelMessage.Size = new System.Drawing.Size(114, 13);
            this.labelMessage.TabIndex = 2;
            this.labelMessage.Text = "Enter the licensing key";
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(93, 109);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(169, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(508, 61);
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            // 
            // linkLabelContact
            // 
            this.linkLabelContact.AutoSize = true;
            this.linkLabelContact.Location = new System.Drawing.Point(305, 119);
            this.linkLabelContact.Name = "linkLabelContact";
            this.linkLabelContact.Size = new System.Drawing.Size(148, 13);
            this.linkLabelContact.TabIndex = 5;
            this.linkLabelContact.TabStop = true;
            this.linkLabelContact.Text = "Contact the GrabCaster Team";
            this.linkLabelContact.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelContact_LinkClicked);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(189, 119);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(110, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "For a new license key";
            // 
            // LicenseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(687, 145);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.linkLabelContact);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.labelMessage);
            this.Controls.Add(this.textBoxLicense);
            this.Controls.Add(this.buttonConfirm);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "LicenseForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GrabCaster Licensing";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonConfirm;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.PictureBox pictureBox1;
        public System.Windows.Forms.TextBox textBoxLicense;
        private System.Windows.Forms.LinkLabel linkLabelContact;
        public System.Windows.Forms.Label labelMessage;
        public System.Windows.Forms.Label label1;
    }
}