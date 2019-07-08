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
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using electrifier.Core.Properties;

namespace electrifier.Core.Components
{
    public partial class ElFormStatePersistor : System.ComponentModel.Component
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
                    throw new InvalidOperationException("ElFormStatePersistor: 'ClientForm' can't be re-assigned once it was set. Create another instance for each Form to be persisted.");
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
        /// E.g. when set to "ElApplicationWindow", the following key names will be used:
        /// "ElApplicationWindow_WindowLocation", "ElApplicationWindow_WindowSize", "ElApplicationWindow_WindowState".
        /// </summary>
        /// <value>The PropertyKeyPrefix property represents the Prefix, that is used when storing the form's individual values.
        /// By default, this will be the form's Name.</value>
        public string PropertyKeyPrefix {
            get;
            set;
        }

        /// <summary>
        /// FormToDesktopMargin represents the margin between desktop work area and the form's borders, in case
        /// the form has to be resized.
        /// 
        /// Defaults to <see cref="SystemInformation.IconSpacingSize"/> for each of the four sides.
        /// </summary>
        public Size FormToDesktopMargin {
            get;
            set;
        }

        #endregion ============================================================================================================

        public ElFormStatePersistor()
        {
            this.InitializeComponent();

            this.FormToDesktopMargin = SystemInformation.IconSpacingSize;
        }

        public ElFormStatePersistor(IContainer container) : this()
        {
            container.Add(this);
        }

        public ElFormStatePersistor(ContainerControl parentControl) : this()
        {
            this.ClientForm = (Form)parentControl;
        }

        public void Load(bool fixWindowState = true, bool overhaulWindowBounds = true)
        {
            string strWindowLocation = this.PropertyKeyPrefix + this.PropertyKeyAffix_WindowLocation;
            string strWindowSize = this.PropertyKeyPrefix + this.PropertyKeyAffix_WindowSize;
            string strWindowState = this.PropertyKeyPrefix + this.PropertyKeyAffix_WindowState;

            var windowLocation = this.ClientForm.Location;
            var windowSize = this.ClientForm.Size;
            var windowState = this.ClientForm.WindowState;

            // Load setting values describing the last known form state
            if (this.SettingExists(strWindowLocation))
                windowLocation = (System.Drawing.Point)Settings.Default[strWindowLocation];
            if (this.SettingExists(strWindowSize))
                windowSize = (System.Drawing.Size)Settings.Default[strWindowSize];
            if (this.SettingExists(strWindowState))
                windowState = (FormWindowState)Settings.Default[strWindowState];

            // When fixWindowState is set, adjust window state if not Normal or Maximized
            if (fixWindowState && (!(windowState == FormWindowState.Normal || windowState == FormWindowState.Maximized)))
                windowState = FormWindowState.Normal;

            // When overhaulWindowBounds is set, recalculate proper bounds to suppress off-screen windows
            if (overhaulWindowBounds)
                this.OverhaulWindowBounds(ref windowLocation, ref windowSize);

            // Finally, apply values 
            this.ClientForm.Location = windowLocation;
            this.ClientForm.Size = windowSize;
            this.ClientForm.WindowState = windowState;
        }

        public virtual void OverhaulWindowBounds(ref Point location, ref Size size)
        {
            // Get Desktop area of primary screen
            Rectangle workArea = Screen.GetWorkingArea(this.clientForm);

            // Overhaul window size if necessary
            if (size.Width > workArea.Width)
            {
                size.Width = workArea.Width - this.FormToDesktopMargin.Width * 2;
                location.X = workArea.X + this.FormToDesktopMargin.Width;
            }

            if (size.Height > workArea.Height)
            {
                size.Height = workArea.Height - this.FormToDesktopMargin.Height * 2;
                location.Y = workArea.Y + this.FormToDesktopMargin.Height;
            }

            // Overhaul window position if necessary
            if (location.X < workArea.X)
                location.X = workArea.X;

            if (location.X + size.Width > workArea.X + workArea.Width)
                location.X = workArea.X + workArea.Width - size.Width;

            if (location.Y < workArea.Y)
                location.Y = workArea.Y;

            if (location.Y + size.Height > workArea.Y + workArea.Height)
                location.Y = workArea.Y + workArea.Height - size.Height;
        }

        // TODO: Move SettingExists to AppContext, or to new settings class respectively
        private bool SettingExists(string settingName)
        {
            return Settings.Default.Properties.Cast<System.Configuration.SettingsProperty>().Any(prop => prop.Name == settingName);
        }

        public void Store()
        {
            string strWindowLocation = this.PropertyKeyPrefix + this.PropertyKeyAffix_WindowLocation;
            string strWindowSize = this.PropertyKeyPrefix + this.PropertyKeyAffix_WindowSize;
            string strWindowState = this.PropertyKeyPrefix + this.PropertyKeyAffix_WindowState;

            // Make sure to store the normalized bounds, i.e. when the form was in normal window state the last time.
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
