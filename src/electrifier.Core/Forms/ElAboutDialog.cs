/*
** 
**  electrifier
** 
**  Copyright 2017-19 Thorsten Jung, www.electrifier.org
**  
**  Licensed under the Apache License, Version 2.0 (the "License");
**  you may not use this file except in compliance with the License.
**  You may obtain a copy of the License at
**  
**      http://www.apache.org/licenses/LICENSE-2.0
**  
**  Unless required by applicable law or agreed to in writing, software
**  distributed under the License is distributed on an "AS IS" BASIS,
**  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
**  See the License for the specific language governing permissions and
**  limitations under the License.
**
*/

using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Windows.Forms;

namespace electrifier.Core.Forms
{
    partial class ElAboutDialog
      : Form
    {
        public ElAboutDialog()
        {
            AppContext.TraceScope();

            this.InitializeComponent();

            // Load background image of version tab from electrifier.exe embedded resource
            this.VersionTabPage.BackgroundImage = new Bitmap(Assembly.GetEntryAssembly().
                GetManifestResourceStream(@"electrifier.SplashScreenForm.png"));

            // Load license document from electrifier.Core.dll embedded resource
            this.LicenseRichTextBox.Rtf = Properties.Resources.License.ToString(CultureInfo.InvariantCulture);

            // Show additional Assembly Informations
            this.ProductVersionLabel.Text = $"{AppContext.AssemblyProduct} v{AppContext.AssemblyVersion}";
            this.CopyrightLabel.Text = AppContext.AssemblyCopyright;
        }

        private void VisitElectrifierOrgLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            AppContext.TraceScope();

            Process.Start("http://www.electrifier.org");
        }

        private void LicenseRichTextBox_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            AppContext.TraceScope();

            Process.Start(e.LinkText);
        }
    }
}
