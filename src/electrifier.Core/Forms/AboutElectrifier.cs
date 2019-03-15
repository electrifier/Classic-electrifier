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

using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace electrifier.Core.Forms
{
    partial class AboutElectrifier : Form
    {
        public AboutElectrifier()
        {
            AppContext.TraceScope();

            this.InitializeComponent();

            // Load background image of version tab from electrifier.exe embedded resource
            this.tpgVersion.BackgroundImage = new Bitmap(Assembly.GetEntryAssembly().
                GetManifestResourceStream(@"electrifier.SplashScreenForm.png"));

            // Load license document from electrifier.Core.dll embedded resource
            this.rtbLicense.Rtf = Properties.Resources.License.ToString();

            this.lblProductVersion.Text = String.Format("{0} v{1}", this.AssemblyProduct, this.AssemblyVersion);
            this.lblCopyright.Text = this.AssemblyCopyright;
        }

        private void LblVisitElectrifierOrg_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            AppContext.TraceScope();

            System.Diagnostics.Process.Start("http://www.electrifier.org");
        }

        private void RtbLicense_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            AppContext.TraceScope();

            System.Diagnostics.Process.Start(e.LinkText);
        }

        #region Assembly Attribute Accessors

        public string AssemblyTitle {
            get {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);

                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "")
                    {
                        return titleAttribute.Title;
                    }
                }
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        public string AssemblyVersion {
            get {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public string AssemblyDescription {
            get {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);

                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        public string AssemblyProduct {
            get {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);

                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        public string AssemblyCopyright {
            get {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);

                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        public string AssemblyCompany {
            get {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);

                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }
        #endregion
    }
}
