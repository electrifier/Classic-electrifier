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
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

using Vanara.PInvoke;

using WeifenLuo.WinFormsUI.Docking;

using electrifier.Core.Components;
using electrifier.Core.Components.DockContents;
using RibbonLib.Controls;
using EntityLighter;
using System.ComponentModel;

namespace electrifier.Core.Forms
{
    /// <summary>
    /// <see cref="ElApplicationWindow"/> is the Main Window of electrifier application.
    /// </summary>
    public partial class ElApplicationWindow
      : System.Windows.Forms.Form
      , System.Windows.Forms.IWin32Window       // Used for ShellFileOperations
    {
        #region Fields ========================================================================================================

        // TODO: Add Creation timestamp! in debug mode
        private static readonly string formTitle_Affix = $"electrifier { AppContext.AssemblyVersion.ToString(3) }";
        public RibbonItems RibbonItems { get; }

        #endregion ============================================================================================================

        #region Properties ====================================================================================================

        public SessionContext SessionContext { get; }

        public override string Text
        {
            get => base.Text;
            set
            {
                if (null == value)
                    throw new ArgumentNullException(nameof(this.Text));

                if (!value.Equals(this.Text, StringComparison.CurrentCultureIgnoreCase))
                {
                    string newText = ((value.Length > 0) ? (value + " - " + formTitle_Affix) : formTitle_Affix);
                    AppContext.AddDebugRemark(ref newText);
                    base.Text = newText;
                }
            }
        }

        #endregion ============================================================================================================

        #region Published Events ==============================================================================================

        public event EventHandler ClipboardUpdate;

        #endregion ============================================================================================================


        public ElApplicationWindow(SessionContext sessionContext)
          : base()
        {
            this.SessionContext = sessionContext ?? throw new ArgumentNullException(nameof(sessionContext));

            AppContext.TraceScope();

            this.InitializeComponent();
            this.RibbonItems = new RibbonItems(this, this.rbnRibbon);

            // Initialize properties
            this.Text = this.SessionContext.Name;

            // Set Application Icon as form Icon
            this.Icon = this.SessionContext.ApplicationIcon;

            // Initialize DockPanel
            this.dpnDockPanel.Theme = new WeifenLuo.WinFormsUI.Docking.VS2015LightTheme();
            this.dpnDockPanel.ShowDocumentIcon = true;
            this.dpnDockPanel.ActiveContentChanged += this.DpnDockPanel_ActiveContentChanged;

            // Add this window to clipboard format listener list, i.e. register for clipboard changes
            AppContext.TraceDebug("AddClipboardFormatListener");
            User32.AddClipboardFormatListener(this.Handle);
            this.FormClosed += this.Electrifier_FormClosed;
        }

        private void Electrifier_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Remove this window from clipboard format listener list, i.e. cancel registration of clipboard changes
            AppContext.TraceDebug("RemoveClipboardFormatListener");
            User32.RemoveClipboardFormatListener(this.Handle);
        }

        /// <summary>
        /// Overridden WndProc processes Windows Messages not handled by .net framework.
        /// 
        /// The following messages are handled:
        ///   WM_CLIPBOARDUPDATE
        /// </summary>
        /// <param name="m">The message that has to be processed.</param>
        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
        protected override void WndProc(ref Message m)
        {
            if ((int)User32.ClipboardNotificationMessage.WM_CLIPBOARDUPDATE == m.Msg)
            {
                this.OnClipboardUpdate();
            }

            base.WndProc(ref m);
        }

        /// <summary>
        /// Clipboard content has been updated, which may have happened inside, but also outside electrifier.
        /// </summary>
        protected virtual void OnClipboardUpdate()
        {
            // TODO: Build event args: Type of Clipboard content
            this.ClipboardUpdate?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// Currently active DockContent has been changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DpnDockPanel_ActiveContentChanged(object sender, EventArgs e)
        {
            // Info: sender should be the DockPanel
            if (!sender.Equals(this.dpnDockPanel))
                throw new ArgumentException("TODO: Test purposes only: sender is not dpnDockPanel! @ DpnDockPanel_ActiveContentChanged");

            var activeContent = this.dpnDockPanel.ActiveContent;

            // Process Ribbon-part of DockContent-Activation
            this.RibbonItems.ActiveDockContent = activeContent;

            // Process Interface IElNavigationHost-part of DockContent-Activation
            if (null == activeContent)
            {
                AppContext.TraceDebug("DpnDockPanel_ActiveContentChanged, activeContent is null");
            }
            else
            {
                var activatedContentType = activeContent?.GetType();

                AppContext.TraceDebug("DpnDockPanel_ActiveContentChanged: Sender=" + sender.ToString()
                    + ", ActivatedContent=" + activeContent
                    + ", ActivatedContentType=" + activatedContentType);

                // ElShellBrowserDockContent has been activated.
                if (typeof(ElShellBrowserDockContent).Equals(activatedContentType))
                {
                    this.ActivateDockContent(activeContent as ElShellBrowserDockContent);
                }
                else
                {
                    AppContext.TraceWarning("DpnDockPanel_ActiveContentChanged => Unknwon ActiveContent");
                }
            }
        }

        private ElShellBrowserDockContent CreateNewShellBrowser(DockAlignment? dockAlignment = null)
        {
            AppContext.TraceScope();

            ElShellBrowserDockContent newShellBrowser = ElDockContentFactory.CreateShellBrowser(this);

            this.AddDockContent(newShellBrowser);       // TODO => dockAlignment

            return newShellBrowser;
        }

        public bool LoadConfiguration(string fullFileName)
        {
            try
            {
                //this.dpnDockPanel.LoadFromXml(fullFileName,
                //    new DeserializeDockContent(delegate (string persistString)
                //    {
                //        return ElDockContentFactory.Deserialize(this, persistString); // TODO: Throw Exception cause of unkown type in XML ? !?
                //    }));
            }
            catch (Exception e)
            {
                MessageBox.Show("ElApplicationWindow.cs->LoadConfiguration" +
                    "\n\tError loading configuarion file." +
                    "\n\nError description: " + e.Message);
            }

            return true;
        }

        /// <summary>
        /// Called by AppContext.Session.SaveConfiguration()
        /// </summary>
        /// <param name="fullFileName">The full file name including its path.</param>
        public void SaveConfiguration(string fullFileName)
        {
            this.dpnDockPanel.SaveAsXml(fullFileName);
        }

        private void fspFormStatePersistor_LoadingFormState(object sender, FormStatePersistorEventArgs args)
        {
            TypeConverter pointConvert = TypeDescriptor.GetConverter(typeof(Point));
            TypeConverter sizeConvert = TypeDescriptor.GetConverter(typeof(Size));
            TypeConverter stateConvert = TypeDescriptor.GetConverter(typeof(FormWindowState));

            // NULL-Check!

            // TODO: Put Converters into EntityLighter!
            args.Location = (Point)pointConvert.ConvertFromString(this.SessionContext.Properties.SyncProperty("ElApplicationWindow.Location", "0,0"));
            args.Size = (Size)sizeConvert.ConvertFromString(this.SessionContext.Properties.SyncProperty("ElApplicationWindow.Size", "800, 600"));
            args.WindowState = (FormWindowState)stateConvert.ConvertFromString(this.SessionContext.Properties.SyncProperty("ElApplicationWindow.WindowState", "Normal"));

            //args.Cancel = false;
        }

        private void FormStatePersistor_SavingFormState(object sender, FormStatePersistorEventArgs args)
        {
            TypeConverter pointConvert = TypeDescriptor.GetConverter(typeof(Point));
            TypeConverter sizeConvert = TypeDescriptor.GetConverter(typeof(Size));
            TypeConverter stateConvert = TypeDescriptor.GetConverter(typeof(FormWindowState));

            this.SessionContext.Properties.SafeSetProperty("ElApplicationWindow.Location", pointConvert.ConvertTo(args.Location, typeof(string)) as string);
            this.SessionContext.Properties.SafeSetProperty("ElApplicationWindow.Size", sizeConvert.ConvertTo(args.Size, typeof(string)) as string);
            this.SessionContext.Properties.SafeSetProperty("ElApplicationWindow.WindowState", stateConvert.ConvertTo(args.WindowState, typeof(string)) as string);
        }
    }
}
