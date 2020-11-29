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
    #region EventArgs ==========================================================================================================

    /// <summary>
    /// <see cref="FormStatePersistorEventArgs"/> is used for data exchange when raising
    /// <see cref="FormStatePersistor.LoadFormState"/> and
    /// <see cref="FormStatePersistor.SaveFormState"/> events.
    /// </summary>
    public class FormStatePersistorEventArgs : EventArgs
    {
        public FormStatePersistorEventArgs(Form form, bool storeRestoreBounds = false)
        {
            if (form is null)
                throw new ArgumentNullException(nameof(form));

            this.Location = storeRestoreBounds ? form.RestoreBounds.Location : form.Location;
            this.Size = storeRestoreBounds ? form.RestoreBounds.Size : form.Size;
            this.WindowState = form.WindowState;
        }

        public Point Location { get; set; }
        public Size Size { get; set; }
        public FormWindowState WindowState { get; set; }

        /// <summary>
        /// Set to 'True' to cancel the operation.
        /// </summary>
        public bool Cancel { get; set; }
    }

    #endregion

    /// <summary>
    /// <see cref="FormStatePersistor"/> class.
    /// </summary>
    public class FormStatePersistor
      : Component
    {
        #region Fields =========================================================================================================

        private Form clientForm;

        #endregion

        #region Properties =====================================================================================================

        /// <summary>
        /// If set to true, <see cref="FormStatePersistor"/> will force the windows state to restore its last <i>Normal</i>
        /// Bounds and state instead of being <i>Maximized</i> or <i>Minimized</i> initially.
        /// <br/>
        /// Defaults to <b>true</b>.
        /// </summary>
        [Category("FormStatePersistor")]
        [Description("Force the window state to restore its last (i.e. Normal) bounds and state instead of being maximized or minimized initially.")]
        [DefaultValue(true)]
        public bool FixWindowState { get; set; } = true;

        /// <summary>
        /// Represents the margin between desktop work area and the form borders,
        /// in case the form has to be resized cause Desktop resolution has changed.
        /// <br/>
        /// Defaults to <see cref="SystemInformation.IconSpacingSize"/> for each of the four sides.
        /// </summary>
        [Category("FormStatePersistor")]
        [Description("Adjust the margin between desktop work area and the form borders, in case the form has to be resized cause desktop resolution has changed.")]
        public Size FormToDesktopMargin { get; set; } = SystemInformation.IconSpacingSize;

        /// <summary>
        /// Helper method for Visual Studio Designer to avoid Serialization of default value.
        /// <seealso href="https://stackoverflow.com/questions/3340226/how-does-one-have-the-c-sharp-designer-know-the-default-property-for-a-padding-o"/>
        /// </summary>
        /// <returns></returns>
        protected bool ShouldSerializeFormToDesktopMargin() => this.FormToDesktopMargin != SystemInformation.IconSpacingSize;

        [Category("FormStatePersistor")]
        [ReadOnly(true)]
        public Form ClientForm
        {
            get => this.clientForm;

            set
            {
                if ((null == value) || (this.ClientForm == value))
                    return;

                if (null == this.ClientForm)
                {
                    this.clientForm = value;

                    //
                    // Add handler for client's Load-event
                    //
                    value.Load += this.ClientFormLoadEventHandler;


                    //
                    // Add handler for client's FormClosing-event
                    //
                    value.FormClosing += this.ClientFormFormClosingEventHandler;
                }
                else
                {
                    throw new InvalidOperationException("FormStatePersistor: Property 'ClientForm' can't be re-assigned once it was set. Create another instance for each Form to be persisted.");
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

        #endregion

        #region Published Events ===============================================================================================

        /// <summary>
        /// Fires while the form is loading and <see cref="FormStatePersistor"/> has to retrieve
        /// its values from Application's persistence layer.
        /// </summary>
        [Category("FormStatePersistor")]
        [Description("Retrieve Form state from Application's persistence layer while being loaded.")]
        public event EventHandler<FormStatePersistorEventArgs> LoadFormState;

        /// <summary>
        /// Fires while the form is closing and <see cref="FormStatePersistor"/> has to store
        /// its values into Application's persistence layer.
        /// </summary>
        [Category("FormStatePersistor")]
        [Description("Save Form state into Application's persistence layer while being closed.")]
        public event EventHandler<FormStatePersistorEventArgs> SaveFormState;

        #endregion

        public FormStatePersistor()
        {
            this.InitializeComponent();
        }

        public FormStatePersistor(IContainer container)
          : this()
        {
            if (null == container)
                throw new ArgumentNullException(nameof(container));

            container.Add(this);
        }

        public FormStatePersistor(ContainerControl parentControl)
          : this()
        {
            this.ClientForm = (Form)parentControl;
        }

        public virtual void ClientFormLoadEventHandler(object sender, EventArgs args)
        {
            if (this.LoadFormState != null)
            {
                FormStatePersistorEventArgs formStatePersistorEventArgs = new FormStatePersistorEventArgs(this.ClientForm);

                this.LoadFormState.Invoke(this, formStatePersistorEventArgs);

                if (!formStatePersistorEventArgs.Cancel)
                {
                    Point location = formStatePersistorEventArgs.Location;
                    Size size = formStatePersistorEventArgs.Size;
                    FormWindowState windowState = formStatePersistorEventArgs.WindowState;

                    if (this.FixWindowState)
                    {
                        if (!(windowState == FormWindowState.Normal || windowState == FormWindowState.Maximized))
                            windowState = FormWindowState.Normal;

                        this.OverhaulWindowBounds(ref location, ref size);
                    }

                    this.ClientForm.Location = location;
                    this.ClientForm.Size = size;
                    this.ClientForm.WindowState = windowState;
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

        /// <summary>
        /// Raise the <see cref="SaveFormState"/> event when the <see cref="clientForm"/> gets closed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public virtual void ClientFormFormClosingEventHandler(object sender, EventArgs args)
        {
            if (this.SaveFormState != null)
            {
                // Make sure to store the normalized bounds, i.e. when the form was in normal window state the last time.
                bool restoreBounds = FormWindowState.Normal != this.ClientForm.WindowState;

                this.SaveFormState.Invoke(this, new FormStatePersistorEventArgs(this.ClientForm, restoreBounds));
            }
        }

        #region Designer Support ===============================================================================================

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components;

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

        #endregion
    }
}
