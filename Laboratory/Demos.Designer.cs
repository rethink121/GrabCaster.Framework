﻿namespace Laboratory
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
            this.buttonMultiGC = new System.Windows.Forms.Button();
            this.textBoxFrom = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxTo = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonPowerJson
            // 
            this.buttonPowerJson.Location = new System.Drawing.Point(77, 100);
            this.buttonPowerJson.Margin = new System.Windows.Forms.Padding(2);
            this.buttonPowerJson.Name = "buttonPowerJson";
            this.buttonPowerJson.Size = new System.Drawing.Size(141, 30);
            this.buttonPowerJson.TabIndex = 0;
            this.buttonPowerJson.Text = "Write in Event Vierwer";
            this.buttonPowerJson.UseVisualStyleBackColor = true;
            this.buttonPowerJson.Click += new System.EventHandler(this.ButtonPowerJsonClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 11);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "How many";
            // 
            // textBoxNum
            // 
            this.textBoxNum.Location = new System.Drawing.Point(77, 11);
            this.textBoxNum.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxNum.Name = "textBoxNum";
            this.textBoxNum.Size = new System.Drawing.Size(143, 20);
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
            this.comboBoxsource.Location = new System.Drawing.Point(77, 36);
            this.comboBoxsource.Name = "comboBoxsource";
            this.comboBoxsource.Size = new System.Drawing.Size(302, 21);
            this.comboBoxsource.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 39);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 15);
            this.label2.TabIndex = 5;
            this.label2.Text = "Type";
            this.label2.Click += new System.EventHandler(this.Label2Click);
            // 
            // labellastrun
            // 
            this.labellastrun.AutoSize = true;
            this.labellastrun.Location = new System.Drawing.Point(234, 210);
            this.labellastrun.Name = "labellastrun";
            this.labellastrun.Size = new System.Drawing.Size(10, 15);
            this.labellastrun.TabIndex = 6;
            this.labellastrun.Text = " ";
            // 
            // textBoxMessage
            // 
            this.textBoxMessage.Location = new System.Drawing.Point(77, 63);
            this.textBoxMessage.Name = "textBoxMessage";
            this.textBoxMessage.Size = new System.Drawing.Size(302, 20);
            this.textBoxMessage.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 66);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(30, 15);
            this.label3.TabIndex = 8;
            this.label3.Text = "Text";
            // 
            // buttonMultiGC
            // 
            this.buttonMultiGC.Location = new System.Drawing.Point(77, 195);
            this.buttonMultiGC.Name = "buttonMultiGC";
            this.buttonMultiGC.Size = new System.Drawing.Size(128, 42);
            this.buttonMultiGC.TabIndex = 9;
            this.buttonMultiGC.Text = "Create multi GC";
            this.buttonMultiGC.UseVisualStyleBackColor = true;
            this.buttonMultiGC.Click += new System.EventHandler(this.buttonMultiGC_Click);
            // 
            // textBoxFrom
            // 
            this.textBoxFrom.Location = new System.Drawing.Point(78, 138);
            this.textBoxFrom.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxFrom.Name = "textBoxFrom";
            this.textBoxFrom.Size = new System.Drawing.Size(48, 20);
            this.textBoxFrom.TabIndex = 11;
            this.textBoxFrom.Text = "0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 138);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(36, 15);
            this.label4.TabIndex = 10;
            this.label4.Text = "From";
            // 
            // textBoxTo
            // 
            this.textBoxTo.Location = new System.Drawing.Point(77, 162);
            this.textBoxTo.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxTo.Name = "textBoxTo";
            this.textBoxTo.Size = new System.Drawing.Size(48, 20);
            this.textBoxTo.TabIndex = 13;
            this.textBoxTo.Text = "50";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 162);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(21, 15);
            this.label5.TabIndex = 12;
            this.label5.Text = "To";
            // 
            // Demos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(394, 256);
            this.Controls.Add(this.textBoxTo);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBoxFrom);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.buttonMultiGC);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxMessage);
            this.Controls.Add(this.labellastrun);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBoxsource);
            this.Controls.Add(this.textBoxNum);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonPowerJson);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Demos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
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
        private Button buttonMultiGC;
        private TextBox textBoxFrom;
        private Label label4;
        private TextBox textBoxTo;
        private Label label5;
    }
}