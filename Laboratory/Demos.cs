// Demos.cs
// 
// BSD 3-Clause License
// 
// Copyright (c) 2014-2016, Nino Crudele <nino dot crudele at live dot com>
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
// 
// * Redistributions of source code must retain the above copyright notice, this
//   list of conditions and the following disclaimer.
// 
// * Redistributions in binary form must reproduce the above copyright notice,
//   this list of conditions and the following disclaimer in the documentation
//   and/or other materials provided with the distribution.
// 
// * Neither the name of the copyright holder nor the names of its
//   contributors may be used to endorse or promote products derived from
//   this software without specific prior written permission.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE
// FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
// DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
// SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
// CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
// OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
// OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
// 
#region Usings

using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

#endregion

namespace Laboratory
{
    /// <summary>
    ///     The demos.
    /// </summary>
    public partial class Demos : Form
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Demos" /> class.
        /// </summary>
        public Demos()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     The button power json_ click.
        /// </summary>
        /// <param name="sender">
        ///     The sender.
        /// </param>
        /// <param name="e">
        ///     The e.
        /// </param>
        private void ButtonPowerJsonClick(object sender, EventArgs e)
        {
            labellastrun.Text = string.Empty;
            var numof = int.Parse(textBoxNum.Text);
            for (var i = 0; i < numof; i++)
            {
                if (!EventLog.SourceExists(comboBoxsource.Text))
                {
                    EventLog.CreateEventSource(comboBoxsource.Text, "Application");
                }
                string message = $"{Environment.MachineName}: {textBoxMessage.Text} {DateTime.Now}";
                EventLog.WriteEntry(
                    comboBoxsource.Text,
                    // ReSharper disable once SpecifyACultureInStringConversionExplicitly
                    message, EventLogEntryType.Error);
            }

            labellastrun.Text = DateTime.Now.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        ///     The demos_ load.
        /// </summary>
        /// <param name="sender">
        ///     The sender.
        /// </param>
        /// <param name="e">
        ///     The e.
        /// </param>
        private void DemosLoad(object sender, EventArgs e)
        {
            comboBoxsource.SelectedIndex = 0;
        }

        /// <summary>
        ///     The label 2_ click.
        /// </summary>
        /// <param name="sender">
        ///     The sender.
        /// </param>
        /// <param name="e">
        ///     The e.
        /// </param>
        private void Label2Click(object sender, EventArgs e)
        {
        }

        private void buttonMultiGC_Click(object sender, EventArgs e)
        {
            int from = int.Parse(textBoxFrom.Text);
            int to = int.Parse(textBoxTo.Text);

            for (int i = from; i < to; i++)
            {
                string newfolder = @"C:\Program Files (x86)\GrabCaster" + i;
                copyFolder(@"C:\Program Files (x86)\GrabCaster", newfolder);
                string fContent = File.ReadAllText(newfolder + "\\GrabCaster.cfg");
                fContent = fContent.Replace("[POINTNUM]", i.ToString());
                File.WriteAllText(newfolder + "\\GrabCaster.cfg",fContent);
                Process.Start(newfolder + "\\GrabCaster.exe","M");
            }

        }

        private void copyFolder(string SourcePath, string DestinationPath)
        {
            //Now Create all of the directories
            foreach (string dirPath in Directory.GetDirectories(SourcePath, "*",
                SearchOption.AllDirectories))
                Directory.CreateDirectory(dirPath.Replace(SourcePath, DestinationPath));

            //Copy all the files & Replaces any files with the same name
            foreach (string newPath in Directory.GetFiles(SourcePath, "*.*",
                SearchOption.AllDirectories))
                File.Copy(newPath, newPath.Replace(SourcePath, DestinationPath), true);
        }

    }
}