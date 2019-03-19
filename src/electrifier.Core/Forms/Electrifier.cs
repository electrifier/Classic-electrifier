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
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

using electrifier.Core.Components;
using electrifier.Core.Components.DockContents;

using Vanara.PInvoke;


namespace electrifier.Core.Forms
{
    /// <summary>
    /// The main Window of electrifier application.
    /// </summary>
    public partial class Electrifier
      : System.Windows.Forms.Form
      , System.Windows.Forms.IWin32Window       // Used for ShellFileOperations
    {
        #region Fields ========================================================================================================

        private static string formTitle_Affix = String.Format("electrifier v{0}", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString());

        #endregion ============================================================================================================

        #region Properties ====================================================================================================

        public override string Text {
            get { return base.Text; }
            set {
                base.Text = ((value.Length > 0) ? (value + " - " + Electrifier.formTitle_Affix) : Electrifier.formTitle_Affix);
                this.FormTitle_AddDebugRemark();
            }
        }

        #endregion ============================================================================================================

        #region Published Events ==============================================================================================

        public event EventHandler ClipboardUpdate;

        #endregion ============================================================================================================


        public Electrifier(Icon icon)
          : base()
        {
            AppContext.TraceScope();

            this.InitializeComponent();
            this.InitializeRibbon();

            this.Icon = icon;
            this.Text = this.Text;          //this.FormTitle_AddDebugRemark(); // TODO: Add formTitleAffix

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
            var activeContent = this.dpnDockPanel.ActiveContent;

            if (!sender.Equals(this.dpnDockPanel))
                throw new ArgumentException("TODO: Test purposes only: sender is not dpnDockPanel! @ DpnDockPanel_ActiveContentChanged");

            // Process Clipboard-part of DockContent-Activation
            this.Clipboard_ActiveDockContentChanged(activeContent);

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

                // ShellBrowserDockContent has been activated.
                if (typeof(ShellBrowserDockContent).Equals(activatedContentType))
                {
                    this.ActivateDockContent(activeContent as ShellBrowserDockContent);
                }
                else
                {
                    AppContext.TraceWarning("DpnDockPanel_ActiveContentChanged => Unknwon ActiveContent");
                }
            }
        }

        [Conditional("DEBUG")]
        private void FormTitle_AddDebugRemark()
        {
            base.Text += " [DEBUG]";
        }

        private ShellBrowserDockContent CreateNewShellBrowser(DockAlignment? dockAlignment = null)
        {
            ShellBrowserDockContent newShellBrowser;

            AppContext.TraceScope();

            this.AddDockContent(newShellBrowser = ElDockContentFactory.CreateShellBrowser(this));
            // TODO: Browse to standard folder...

            return newShellBrowser;
        }

        public bool LoadConfiguration(string fullFileName)
        {
            try
            {
                this.dpnDockPanel.LoadFromXml(fullFileName, new DeserializeDockContent(
                    delegate(string persistString)
                    {
                        return ElDockContentFactory.Deserialize(this, persistString); // TODO: Throw Exception cause of unkown type in XML ? !?
                    }));
            }
            catch (Exception e)
            {
                MessageBox.Show("Electrifier.cs->LoadConfiguration" +
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

        // Issue #21 Refactoring: Remove Windows-API-Code-Pack
        private void NewDockContent_NavigationLogChanged(object sender, /*Microsoft.WindowsAPICodePack.Controls.NavigationLogEventArgs*/
                IntPtr e)
        {
            //// TODO: Check if this DockContent is (still) the ActiveContent
            //// TODO: Or, Change NavigationLogEventArgs to send the active ShellBrowserDockContent with it
            //this.ntsNavigation.UpdateNavigationLog(e, this.GetActiveShellBrowserDockContent().NavigationLog);

        }
    }
}
