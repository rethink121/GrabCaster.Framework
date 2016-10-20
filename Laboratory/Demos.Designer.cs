namespace Laboratory
{
    using System.ComponentModel;
    using System.Windows.Forms;

    partial class Demos
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
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
            this.buttonPowerJson = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxNum = new System.Windows.Forms.TextBox();
            this.comboBoxsource = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.labellastrun = new System.Windows.Forms.Label();
            this.textBoxMessage = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonPowerJson
            // 
            this.buttonPowerJson.Location = new System.Drawing.Point(103, 240);
            this.buttonPowerJson.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonPowerJson.Name = "buttonPowerJson";
            this.buttonPowerJson.Size = new System.Drawing.Size(188, 37);
            this.buttonPowerJson.TabIndex = 0;
            this.buttonPowerJson.Text = "Write in Event Vierwer";
            this.buttonPowerJson.UseVisualStyleBackColor = true;
            this.buttonPowerJson.Click += new System.EventHandler(this.ButtonPowerJsonClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "How many";
            // 
            // textBoxNum
            // 
            this.textBoxNum.Location = new System.Drawing.Point(103, 14);
            this.textBoxNum.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBoxNum.Name = "textBoxNum";
            this.textBoxNum.Size = new System.Drawing.Size(189, 22);
            this.textBoxNum.TabIndex = 3;
            this.textBoxNum.Text = "1";
            // 
            // comboBoxsource
            // 
            this.comboBoxsource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxsource.FormattingEnabled = true;
            this.comboBoxsource.Items.AddRange(new object[] {
            "DEMOPOWERSHELLJOB",
            "DEMOTEXTMESSAGE",
            "DEMONOTEPAD",
            "DEMOEV"});
            this.comboBoxsource.Location = new System.Drawing.Point(103, 44);
            this.comboBoxsource.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.comboBoxsource.Name = "comboBoxsource";
            this.comboBoxsource.Size = new System.Drawing.Size(401, 24);
            this.comboBoxsource.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 17);
            this.label2.TabIndex = 5;
            this.label2.Text = "Type";
            this.label2.Click += new System.EventHandler(this.Label2Click);
            // 
            // labellastrun
            // 
            this.labellastrun.AutoSize = true;
            this.labellastrun.Location = new System.Drawing.Point(312, 258);
            this.labellastrun.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labellastrun.Name = "labellastrun";
            this.labellastrun.Size = new System.Drawing.Size(12, 17);
            this.labellastrun.TabIndex = 6;
            this.labellastrun.Text = " ";
            // 
            // textBoxMessage
            // 
            this.textBoxMessage.Location = new System.Drawing.Point(103, 78);
            this.textBoxMessage.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxMessage.Name = "textBoxMessage";
            this.textBoxMessage.Size = new System.Drawing.Size(401, 22);
            this.textBoxMessage.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 81);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 17);
            this.label3.TabIndex = 8;
            this.label3.Text = "Text";
            // 
            // Demos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(853, 313);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxMessage);
            this.Controls.Add(this.labellastrun);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBoxsource);
            this.Controls.Add(this.textBoxNum);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonPowerJson);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Demos";
            this.Text = "Demo and Labs";
            this.Load += new System.EventHandler(this.DemosLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button buttonPowerJson;
        private Label label1;
        private TextBox textBoxNum;
        private ComboBox comboBoxsource;
        private Label label2;
        private Label labellastrun;
        private TextBox textBoxMessage;
        private Label label3;
    }
}