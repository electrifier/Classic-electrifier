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
using System.Windows.Forms;

namespace electrifier.Core.Components
{
    #region EventArgs =====================================================================================================

    /// <summary>
    /// <see cref="FormStatePersistorEventArgs"/> is used for data exchange when raising
    /// <see cref="ElFormStatePersistor.LoadingFormState"/> and
    /// <see cref="ElFormStatePersistor.SavingFormState"/> events.
    /// </summary>
    public class FormStatePersistorEventArgs : EventArgs
    {
        public FormStatePersistorEventArgs(Form form)
        {
            if (form is null)
                throw new ArgumentNullException(nameof(form));

            this.Location = form.Location;
            this.Size = form.Size;
            this.WindowState = form.WindowState;
        }

        public Point Location { get; set; }
        public Size Size { get; set; }
        public FormWindowState WindowState { get; set; }

        /// <summary>
        /// Set to 'True' to cancel the operation.
        /// </summary>
        public bool Cancel { get; set; } = false;
    }

    #endregion ============================================================================================================

    /// <summary>
    /// <see cref="ElFormStatePersistor"/> class.
    /// </summary>
    public class ElFormStatePersistor
      : Component
    {
        #region Fields ========================================================================================================

        private Form clientForm;

        #endregion ============================================================================================================

        #region Properties ====================================================================================================

        /// <summary>
        /// If set to true, <see cref="ElFormStatePersistor"/> will force the windows state to restore its last <i>Normal</i>
        /// Bounds and state instead of being <i>Maximized</i> or <i>Minimized</i> initially.
        /// 
        /// Defaults to <b>true</b>.
        /// </summary>
        public bool FixWindowState { get; set; } = true;

        /// <summary>
        /// FormToDesktopMargin represents the margin between desktop work area and the form's borders, in case
        /// the form has to be resized.
        /// 
        /// Defaults to <see cref="SystemInformation.IconSpacingSize"/> for each of the four sides.
        /// </summary>
        public Size FormToDesktopMargin
        {
            get;
            set;
        }

        public Form ClientForm
        {
            get => this.clientForm;

            set
            {
                if (null == value)
                {
                    // TODO: Remove Event Handlers?!?

                    return;
                }

                if (this.ClientForm == value)
                    return;

                if (null == this.ClientForm)
                {
                    this.clientForm = value;

                    //
                    // Add handler for client's Load-event
                    //
                    this.ClientForm.Load += this.ClientFormLoadEventHandler;


                    //
                    // Add handler for client's FormClosing-event
                    //
                    this.clientForm.FormClosing += this.ClientFormFormClosingEventHandler;
                }
                else
                {
                    throw new InvalidOperationException("ElFormStatePersistor: 'ClientForm' can't be re-assigned once it was set. Create another instance for each Form to be persisted.");
                }
            }
        }

        /// <summary>
        /// The site property is set in design time by the Form Designer.
        /// <br/>
        /// This behaviour is used to find out on which form the component was dropped onto.
        /// <br/>
        /// See also: <see href="https://social.msdn.microsoft.com/Forums/windows/en-US/d5ccfe1d-e579-41a0-b8fb-614cff5abf54/how-to-get-a-reference-to-the-parent-form-from-the-compontent-container?forum=winforms"/>
        /// </summary>
        public override ISite Site
        {
            get => base.Site;

            set
            {
                base.Site = value;

                if (value == null)
                    return;

                if (value.GetService(typeof(IDesignerHost)) is IDesignerHost designerHost)
                {
                    IComponent componentHost = designerHost.RootComponent;

                    if (componentHost is Form designerForm)
                        this.clientForm = designerForm;
                }
            }
        }

        #endregion ============================================================================================================


        #region Published Events ==============================================================================================

        /// <summary>
        /// Fires while the form is loading and <see cref="ElFormStatePersistor"/> has to retrieve
        /// its values from Application's persistence layer.
        /// </summary>
        [Category("Action"), Description("Retrieve Form state from Application's persistence layer while being loaded.")]
        public event EventHandler<FormStatePersistorEventArgs> LoadingFormState;

        /// <summary>
        /// Fires while the form is closing and <see cref="ElFormStatePersistor"/> has to store
        /// its values into Application's persistence layer.
        /// </summary>
        [Category("Action"), Description("Saving Form state into Application's persistence layer while being closed.")]
        public event EventHandler<FormStatePersistorEventArgs> SavingFormState;

        #endregion ============================================================================================================

        public ElFormStatePersistor()
        {
            this.InitializeComponent();

            this.FormToDesktopMargin = SystemInformation.IconSpacingSize;
        }

        public ElFormStatePersistor(IContainer container)
          : this()
        {
            if (null == container)
                throw new ArgumentNullException(nameof(container));

            container.Add(this);
        }

        public ElFormStatePersistor(ContainerControl parentControl)
          : this()
        {
            this.ClientForm = (Form)parentControl;
        }

//        public void Load(bool fixWindowState = true, bool overhaulWindowBounds = true)
//        {
//            string strWindowLocation = this.PropertyKeyPrefix + this.PropertyKeyAffix_WindowLocation;
//            string strWindowSize = this.PropertyKeyPrefix + this.PropertyKeyAffix_WindowSize;
//            string strWindowState = this.PropertyKeyPrefix + this.PropertyKeyAffix_WindowState;

//            var windowLocation = this.ClientForm.Location;
//            var windowSize = this.ClientForm.Size;
//            var windowState = this.ClientForm.WindowState;

//            // Load setting values describing the last known form state
///* Commented out for Issue #25
//            //if (this.SettingExists(strWindowLocation))
//            //    windowLocation = (System.Drawing.Point)Settings.Default[strWindowLocation];
//            //if (this.SettingExists(strWindowSize))
//            //    windowSize = (System.Drawing.Size)Settings.Default[strWindowSize];
//            //if (this.SettingExists(strWindowState))
//            //    windowState = (FormWindowState)Settings.Default[strWindowState];
//// Commented out for Issue #25 */
//            // When fixWindowState is set, adjust window state if not Normal or Maximized
//            if (fixWindowState && (!(windowState == FormWindowState.Normal || windowState == FormWindowState.Maximized)))
//                windowState = FormWindowState.Normal;

//            // When overhaulWindowBounds is set, recalculate proper bounds to suppress off-screen windows
//            if (overhaulWindowBounds)
//                this.OverhaulWindowBounds(ref windowLocation, ref windowSize);

//            // Finally, apply values 
//            this.ClientForm.Location = windowLocation;
//            this.ClientForm.Size = windowSize;
//            this.ClientForm.WindowState = windowState;
//        }

        public void ClientFormLoadEventHandler(object sender, EventArgs args)
        {
            if (this.LoadingFormState != null)
            {
                FormStatePersistorEventArgs formStatePersistorEventArgs = new FormStatePersistorEventArgs(this.ClientForm);

                this.LoadingFormState.Invoke(this, formStatePersistorEventArgs);

                if (!formStatePersistorEventArgs.Cancel)
                {
                    // TODO: Fix Window State
                    // TODO: OverhaulWindowBounds

                    this.ClientForm.Location = formStatePersistorEventArgs.Location;
                    this.ClientForm.Size = formStatePersistorEventArgs.Size;
                    this.ClientForm.WindowState = formStatePersistorEventArgs.WindowState;
                }
            }
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

//        public void Store()
//        {
//            string strWindowLocation = this.PropertyKeyPrefix + this.PropertyKeyAffix_WindowLocation;
//            string strWindowSize = this.PropertyKeyPrefix + this.PropertyKeyAffix_WindowSize;
//            string strWindowState = this.PropertyKeyPrefix + this.PropertyKeyAffix_WindowState;

///* Commented out for Issue #25
//            //// Make sure to store the normalized bounds, i.e. when the form was in normal window state the last time.
//            //if (FormWindowState.Normal == this.ClientForm.WindowState)
//            //{
//            //    Settings.Default[strWindowLocation] = this.ClientForm.Location;
//            //    Settings.Default[strWindowSize] = this.ClientForm.Size;
//            //}
//            //else
//            //{
//            //    Settings.Default[strWindowLocation] = this.ClientForm.RestoreBounds.Location;
//            //    Settings.Default[strWindowSize] = this.ClientForm.RestoreBounds.Size;
//            //}
//            //Settings.Default[strWindowState] = this.ClientForm.WindowState;

//            //Settings.Default.Save();
//// Commented out for Issue #25 */
//        }

        public void ClientFormFormClosingEventHandler(object sender, EventArgs args)
        {
            if (this.SavingFormState != null)
            {
                FormStatePersistorEventArgs formStatePersistorEventArgs = new FormStatePersistorEventArgs(this.ClientForm);

                // Make sure to store the normalized bounds, i.e. when the form was in normal window state the last time.
                if (FormWindowState.Normal != formStatePersistorEventArgs.WindowState)
                {
                    formStatePersistorEventArgs.Location = this.ClientForm.RestoreBounds.Location;
                    formStatePersistorEventArgs.Size = this.ClientForm.RestoreBounds.Size;
                }

                this.SavingFormState.Invoke(this, formStatePersistorEventArgs);
            }
        }


        #region ElFormStatePersistor.Designer.cs

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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }

        #endregion

        #endregion ElFormStatePersistor.Designer.cs
    }
}
