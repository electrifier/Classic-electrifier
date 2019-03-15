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
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Windows.Forms;

using electrifier.Core.Properties;

namespace electrifier.Core.Components
{
    public partial class FormStatePersistor : System.ComponentModel.Component
    {
        #region Fields ========================================================================================================

        protected Form clientForm;
        protected string PropertyKeyAffix_WindowLocation = "_WindowLocation";
        protected string PropertyKeyAffix_WindowSize = "_WindowSize";
        protected string PropertyKeyAffix_WindowState = "_WindowState";

        #endregion ============================================================================================================

        #region Properties ====================================================================================================

        public System.Windows.Forms.Form ClientForm {
            get => this.clientForm;

            set {
                if (null == value)
                    return;

                if (this.ClientForm == value)
                    return;

                if (null == this.ClientForm)
                {
                    this.clientForm = value;

                    // Add anonymous handler for client Load-event
                    this.ClientForm.Load += (sender, e) =>
                    {
                        this.Load();
                    };

                    // Add anonymous handler for client FormClosing-event
                    this.ClientForm.FormClosing += (sender, e) =>
                    {
                        this.Store();
                    };
                }
                else
                {
                    throw new InvalidOperationException("FormStatePersistor: ContainerControl can't be re-assigned once it was set. Create another instance for each Form to be persisted.");
                }
            }
        }

        /// <summary>
        /// The site property is set in design time by the Form Designer.
        /// 
        /// Let's use this behaviour to find out on which form the component was dropped onto.
        /// 
        /// See <href="https://social.msdn.microsoft.com/Forums/windows/en-US/d5ccfe1d-e579-41a0-b8fb-614cff5abf54/how-to-get-a-reference-to-the-parent-form-from-the-compontent-container?forum=winforms"/>
        /// </summary>
        public override ISite Site {
            get => base.Site;
            set {
                base.Site = value;

                if (value == null)
                    return;

                if (value.GetService(typeof(IDesignerHost)) is IDesignerHost designerHost)
                {
                    IComponent componentHost = designerHost.RootComponent;

                    if (componentHost is Form)
                    {
                        this.clientForm = (Form)componentHost;
                        this.PropertyKeyPrefix = ((Form)componentHost).Name;
                    }
                }
            }
        }


        /// <summary>
        /// The PropertyKeyPrefix property represents the Prefix that is used when storing the form's individual values.
        /// 
        /// E.g. when set to "Electrifier", the following key names will be used:
        /// "Electrifier_WindowLocation", "Electrifier_WindowSize", "Electrifier_WindowState".
        /// </summary>
        /// <value>The PropertyKeyPrefix property represents the Prefix, that is used when storing the form's individual values.
        /// By default, this will be the form's Name.</value>
        public string PropertyKeyPrefix {
            get;
            set;
        }

        #endregion ============================================================================================================

        public FormStatePersistor()
        {
            this.InitializeComponent();
        }

        public FormStatePersistor(IContainer container) : this()
        {
            container.Add(this);
        }

        public FormStatePersistor(ContainerControl parentControl) : this()
        {
            this.ClientForm = (Form)parentControl;
        }

        public void Load(bool fixWindowState = true)
        {
            string strWindowLocation = this.PropertyKeyPrefix + this.PropertyKeyAffix_WindowLocation;
            string strWindowSize = this.PropertyKeyPrefix + this.PropertyKeyAffix_WindowSize;
            string strWindowState = this.PropertyKeyPrefix + this.PropertyKeyAffix_WindowState;

            if (this.SettingExists(strWindowLocation) &&
                this.SettingExists(strWindowSize) &&
                this.SettingExists(strWindowState))
            {
                this.ClientForm.Location = (System.Drawing.Point)Settings.Default[strWindowLocation];
                this.ClientForm.Size = (System.Drawing.Size)Settings.Default[strWindowSize];
                FormWindowState formWindowState = (FormWindowState)Settings.Default[strWindowState]; ;

                if (FormWindowState.Normal != formWindowState)
                {
                    //if (this.ClientForm.WindowState == FormWindowState.Minimized)             // TODO: Ask to fix WindowState when hidden (using an event!)
                    //{
                    //    MessageBox.Show("Window was minimized. Do you want to hide it again?");
                    //}
                    //public void Restore()
                    //{
                    //    //// Move window into visible screen bounds if outside screen bounds (prevent off-screen hidden windows)
                    //    //Rectangle screenRect = SystemInformation.VirtualScreen;
                    //    //if (form.Left < screenRect.Left)
                    //    //    form.Left = screenRect.Left;
                    //    //if (form.Top < screenRect.Top)
                    //    //    form.Top = screenRect.Top;
                    //    //if (form.Right > screenRect.Right)
                    //    //    form.Left = screenRect.Right - form.Width;
                    //    //if (form.Bottom > screenRect.Bottom)
                    //    //    form.Top = screenRect.Bottom - form.Height;
                    // if (formstate = hidden) msgbox(form was hidden when closed, keep it hidden again?)
                    //}

                    formWindowState = FormWindowState.Normal;
                }

                this.ClientForm.WindowState = formWindowState;
            }
        }

        private bool SettingExists(string settingName)
        {
            return Settings.Default.Properties.Cast<System.Configuration.SettingsProperty>().Any(prop => prop.Name == settingName);
        }

        public void Store()
        {
            string strWindowLocation = this.PropertyKeyPrefix + this.PropertyKeyAffix_WindowLocation;
            string strWindowSize = this.PropertyKeyPrefix + this.PropertyKeyAffix_WindowSize;
            string strWindowState = this.PropertyKeyPrefix + this.PropertyKeyAffix_WindowState;

            if (FormWindowState.Normal == this.ClientForm.WindowState)
            {
                Settings.Default[strWindowLocation] = this.ClientForm.Location;
                Settings.Default[strWindowSize] = this.ClientForm.Size;
            }
            else
            {
                Settings.Default[strWindowLocation] = this.ClientForm.RestoreBounds.Location;
                Settings.Default[strWindowSize] = this.ClientForm.RestoreBounds.Size;
            }
            Settings.Default[strWindowState] = this.ClientForm.WindowState;

            Settings.Default.Save();
        }
    }
}
