namespace Lab
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
            this.buttonGetTypes = new System.Windows.Forms.Button();
            this.buttonCreateEH = new System.Windows.Forms.Button();
            this.buttonTrash = new System.Windows.Forms.Button();
            this.buttonCreateTrigger = new System.Windows.Forms.Button();
            this.buttonLoadTriggerevents = new System.Windows.Forms.Button();
            this.buttonStartTriggers = new System.Windows.Forms.Button();
            this.buttonStartDownStream = new System.Windows.Forms.Button();
            this.buttonCreateEventPropertyBag = new System.Windows.Forms.Button();
            this.buttonstorage = new System.Windows.Forms.Button();
            this.buttonConfiguration = new System.Windows.Forms.Button();
            this.buttonSerialization = new System.Windows.Forms.Button();
            this.buttonFileSW = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.buttonCheckEH = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.EventLog = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.buttonEVTrigger = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.textBoxNum = new System.Windows.Forms.TextBox();
            this.button4 = new System.Windows.Forms.Button();
            this.buttonPerfLog = new System.Windows.Forms.Button();
            this.buttonQueue = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.buttonSQL = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonGetTypes
            // 
            this.buttonGetTypes.Location = new System.Drawing.Point(1009, 33);
            this.buttonGetTypes.Name = "buttonGetTypes";
            this.buttonGetTypes.Size = new System.Drawing.Size(168, 23);
            this.buttonGetTypes.TabIndex = 0;
            this.buttonGetTypes.Text = "Get types";
            this.buttonGetTypes.UseVisualStyleBackColor = true;
            this.buttonGetTypes.Click += new System.EventHandler(this.buttonGetTypes_Click);
            // 
            // buttonCreateEH
            // 
            this.buttonCreateEH.Location = new System.Drawing.Point(1009, 280);
            this.buttonCreateEH.Name = "buttonCreateEH";
            this.buttonCreateEH.Size = new System.Drawing.Size(168, 23);
            this.buttonCreateEH.TabIndex = 1;
            this.buttonCreateEH.Text = "Create EH";
            this.buttonCreateEH.UseVisualStyleBackColor = true;
            this.buttonCreateEH.Click += new System.EventHandler(this.buttonCreateEH_Click);
            // 
            // buttonTrash
            // 
            this.buttonTrash.Location = new System.Drawing.Point(1009, 328);
            this.buttonTrash.Name = "buttonTrash";
            this.buttonTrash.Size = new System.Drawing.Size(168, 23);
            this.buttonTrash.TabIndex = 2;
            this.buttonTrash.Text = "Trassh";
            this.buttonTrash.UseVisualStyleBackColor = true;
            this.buttonTrash.Click += new System.EventHandler(this.buttonTrash_Click);
            // 
            // buttonCreateTrigger
            // 
            this.buttonCreateTrigger.Location = new System.Drawing.Point(589, 182);
            this.buttonCreateTrigger.Name = "buttonCreateTrigger";
            this.buttonCreateTrigger.Size = new System.Drawing.Size(222, 23);
            this.buttonCreateTrigger.TabIndex = 3;
            this.buttonCreateTrigger.Text = "Configure File Trigger";
            this.buttonCreateTrigger.UseVisualStyleBackColor = true;
            this.buttonCreateTrigger.Click += new System.EventHandler(this.buttonCreateTrigger_Click);
            // 
            // buttonLoadTriggerevents
            // 
            this.buttonLoadTriggerevents.Location = new System.Drawing.Point(1009, 118);
            this.buttonLoadTriggerevents.Name = "buttonLoadTriggerevents";
            this.buttonLoadTriggerevents.Size = new System.Drawing.Size(168, 23);
            this.buttonLoadTriggerevents.TabIndex = 4;
            this.buttonLoadTriggerevents.Text = "Load Trigger events";
            this.buttonLoadTriggerevents.UseVisualStyleBackColor = true;
            this.buttonLoadTriggerevents.Click += new System.EventHandler(this.buttonLoadTriggerevents_Click);
            // 
            // buttonStartTriggers
            // 
            this.buttonStartTriggers.Location = new System.Drawing.Point(1009, 165);
            this.buttonStartTriggers.Name = "buttonStartTriggers";
            this.buttonStartTriggers.Size = new System.Drawing.Size(168, 26);
            this.buttonStartTriggers.TabIndex = 5;
            this.buttonStartTriggers.Text = "Start Triggers Polling";
            this.buttonStartTriggers.UseVisualStyleBackColor = true;
            this.buttonStartTriggers.Click += new System.EventHandler(this.buttonStartTriggers_Click);
            // 
            // buttonStartDownStream
            // 
            this.buttonStartDownStream.Location = new System.Drawing.Point(387, 104);
            this.buttonStartDownStream.Name = "buttonStartDownStream";
            this.buttonStartDownStream.Size = new System.Drawing.Size(168, 26);
            this.buttonStartDownStream.TabIndex = 6;
            this.buttonStartDownStream.Text = "Start DownStream";
            this.buttonStartDownStream.UseVisualStyleBackColor = true;
            this.buttonStartDownStream.Click += new System.EventHandler(this.buttonStartDownStream_Click);
            // 
            // buttonCreateEventPropertyBag
            // 
            this.buttonCreateEventPropertyBag.Location = new System.Drawing.Point(348, 153);
            this.buttonCreateEventPropertyBag.Name = "buttonCreateEventPropertyBag";
            this.buttonCreateEventPropertyBag.Size = new System.Drawing.Size(249, 23);
            this.buttonCreateEventPropertyBag.TabIndex = 7;
            this.buttonCreateEventPropertyBag.Text = "Create EventPropertyBag";
            this.buttonCreateEventPropertyBag.UseVisualStyleBackColor = true;
            this.buttonCreateEventPropertyBag.Click += new System.EventHandler(this.buttonCreateEventPropertyBag_Click);
            // 
            // buttonstorage
            // 
            this.buttonstorage.Location = new System.Drawing.Point(376, 195);
            this.buttonstorage.Name = "buttonstorage";
            this.buttonstorage.Size = new System.Drawing.Size(168, 26);
            this.buttonstorage.TabIndex = 8;
            this.buttonstorage.Text = "Storage";
            this.buttonstorage.UseVisualStyleBackColor = true;
            this.buttonstorage.Click += new System.EventHandler(this.buttonstorage_Click);
            // 
            // buttonConfiguration
            // 
            this.buttonConfiguration.Location = new System.Drawing.Point(538, 72);
            this.buttonConfiguration.Name = "buttonConfiguration";
            this.buttonConfiguration.Size = new System.Drawing.Size(168, 26);
            this.buttonConfiguration.TabIndex = 9;
            this.buttonConfiguration.Text = "Configuration";
            this.buttonConfiguration.UseVisualStyleBackColor = true;
            this.buttonConfiguration.Click += new System.EventHandler(this.buttonConfiguration_Click);
            // 
            // buttonSerialization
            // 
            this.buttonSerialization.Location = new System.Drawing.Point(538, 30);
            this.buttonSerialization.Name = "buttonSerialization";
            this.buttonSerialization.Size = new System.Drawing.Size(168, 26);
            this.buttonSerialization.TabIndex = 10;
            this.buttonSerialization.Text = "Serialization";
            this.buttonSerialization.UseVisualStyleBackColor = true;
            this.buttonSerialization.Click += new System.EventHandler(this.buttonSerialization_Click);
            // 
            // buttonFileSW
            // 
            this.buttonFileSW.Location = new System.Drawing.Point(1009, 357);
            this.buttonFileSW.Name = "buttonFileSW";
            this.buttonFileSW.Size = new System.Drawing.Size(168, 26);
            this.buttonFileSW.TabIndex = 11;
            this.buttonFileSW.Text = "FileSW";
            this.buttonFileSW.UseVisualStyleBackColor = true;
            this.buttonFileSW.Click += new System.EventHandler(this.buttonFileSW_Click);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 16;
            this.listBox1.Location = new System.Drawing.Point(649, 260);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(302, 260);
            this.listBox1.TabIndex = 12;
            // 
            // buttonCheckEH
            // 
            this.buttonCheckEH.Location = new System.Drawing.Point(723, 72);
            this.buttonCheckEH.Name = "buttonCheckEH";
            this.buttonCheckEH.Size = new System.Drawing.Size(228, 23);
            this.buttonCheckEH.TabIndex = 13;
            this.buttonCheckEH.Text = "Check EH";
            this.buttonCheckEH.UseVisualStyleBackColor = true;
            this.buttonCheckEH.Click += new System.EventHandler(this.buttonCheckEH_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(723, 33);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(228, 23);
            this.button1.TabIndex = 14;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // EventLog
            // 
            this.EventLog.Location = new System.Drawing.Point(300, 518);
            this.EventLog.Name = "EventLog";
            this.EventLog.Size = new System.Drawing.Size(376, 23);
            this.EventLog.TabIndex = 15;
            this.EventLog.Text = "sTART Evnet log";
            this.EventLog.UseVisualStyleBackColor = true;
            this.EventLog.Click += new System.EventHandler(this.EventLog_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(723, 144);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(228, 23);
            this.button2.TabIndex = 16;
            this.button2.Text = "wRITE eVENT lOG";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // buttonEVTrigger
            // 
            this.buttonEVTrigger.Location = new System.Drawing.Point(591, 211);
            this.buttonEVTrigger.Name = "buttonEVTrigger";
            this.buttonEVTrigger.Size = new System.Drawing.Size(222, 23);
            this.buttonEVTrigger.TabIndex = 17;
            this.buttonEVTrigger.Text = "Configure EV Trigger";
            this.buttonEVTrigger.UseVisualStyleBackColor = true;
            this.buttonEVTrigger.Click += new System.EventHandler(this.buttonEVTrigger_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(723, 115);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(228, 23);
            this.button3.TabIndex = 18;
            this.button3.Text = "BLOB";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // textBoxNum
            // 
            this.textBoxNum.Location = new System.Drawing.Point(617, 145);
            this.textBoxNum.Name = "textBoxNum";
            this.textBoxNum.Size = new System.Drawing.Size(100, 22);
            this.textBoxNum.TabIndex = 19;
            this.textBoxNum.Text = "1";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(376, 231);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(246, 23);
            this.button4.TabIndex = 20;
            this.button4.Text = "Execute Roslyn";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // buttonPerfLog
            // 
            this.buttonPerfLog.Location = new System.Drawing.Point(376, 260);
            this.buttonPerfLog.Name = "buttonPerfLog";
            this.buttonPerfLog.Size = new System.Drawing.Size(267, 23);
            this.buttonPerfLog.TabIndex = 21;
            this.buttonPerfLog.Text = "Perf log";
            this.buttonPerfLog.UseVisualStyleBackColor = true;
            this.buttonPerfLog.Click += new System.EventHandler(this.buttonPerfLog_Click);
            // 
            // buttonQueue
            // 
            this.buttonQueue.Location = new System.Drawing.Point(387, 308);
            this.buttonQueue.Name = "buttonQueue";
            this.buttonQueue.Size = new System.Drawing.Size(221, 23);
            this.buttonQueue.TabIndex = 22;
            this.buttonQueue.Text = "Queue";
            this.buttonQueue.UseVisualStyleBackColor = true;
            this.buttonQueue.Click += new System.EventHandler(this.buttonQueue_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(376, 364);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(221, 23);
            this.button5.TabIndex = 23;
            this.button5.Text = "Topic";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // buttonSQL
            // 
            this.buttonSQL.Location = new System.Drawing.Point(376, 417);
            this.buttonSQL.Name = "buttonSQL";
            this.buttonSQL.Size = new System.Drawing.Size(221, 23);
            this.buttonSQL.TabIndex = 24;
            this.buttonSQL.Text = "SQL";
            this.buttonSQL.UseVisualStyleBackColor = true;
            this.buttonSQL.Click += new System.EventHandler(this.buttonSQL_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(929, 441);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(129, 23);
            this.button6.TabIndex = 25;
            this.button6.Text = "button6";
            this.button6.UseVisualStyleBackColor = true;
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(811, 198);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(179, 23);
            this.button7.TabIndex = 26;
            this.button7.Text = "Perf cpunter";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1276, 672);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.buttonSQL);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.buttonQueue);
            this.Controls.Add(this.buttonPerfLog);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.textBoxNum);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.buttonEVTrigger);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.EventLog);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.buttonCheckEH);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.buttonFileSW);
            this.Controls.Add(this.buttonSerialization);
            this.Controls.Add(this.buttonConfiguration);
            this.Controls.Add(this.buttonstorage);
            this.Controls.Add(this.buttonCreateEventPropertyBag);
            this.Controls.Add(this.buttonStartDownStream);
            this.Controls.Add(this.buttonStartTriggers);
            this.Controls.Add(this.buttonLoadTriggerevents);
            this.Controls.Add(this.buttonCreateTrigger);
            this.Controls.Add(this.buttonTrash);
            this.Controls.Add(this.buttonCreateEH);
            this.Controls.Add(this.buttonGetTypes);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonGetTypes;
        private System.Windows.Forms.Button buttonCreateEH;
        private System.Windows.Forms.Button buttonTrash;
        private System.Windows.Forms.Button buttonCreateTrigger;
        private System.Windows.Forms.Button buttonLoadTriggerevents;
        private System.Windows.Forms.Button buttonStartTriggers;
        private System.Windows.Forms.Button buttonStartDownStream;
        private System.Windows.Forms.Button buttonCreateEventPropertyBag;
        private System.Windows.Forms.Button buttonstorage;
        private System.Windows.Forms.Button buttonConfiguration;
        private System.Windows.Forms.Button buttonSerialization;
        private System.Windows.Forms.Button buttonFileSW;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button buttonCheckEH;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button EventLog;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button buttonEVTrigger;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox textBoxNum;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button buttonPerfLog;
        private System.Windows.Forms.Button buttonQueue;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button buttonSQL;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
    }
}

