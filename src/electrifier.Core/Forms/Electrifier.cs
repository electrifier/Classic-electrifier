/*
** 
**  electrifier
** 
**  Copyright 2018 Thorsten Jung, www.electrifier.org
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

using electrifier.Win32API;
using electrifier.Core.Components.DockContents;

using common.Interop;


namespace electrifier.Core.Forms
{
    /// <summary>
    /// The main Window of electrifier application.
    /// </summary>
    public partial class Electrifier : System.Windows.Forms.Form
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

        public Electrifier(Icon icon) : base()
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

            // Initialize NavigationToolStrip
            this.ntsNavigation.NavigateBackwardClick += this.NtsNavigation_NavigateBackwardClick;
            this.ntsNavigation.NavigateForwardClick += this.NtsNavigation_NavigateForwardClick;
            this.ntsNavigation.NavigateRecentLocationsClicked += this.NtsNavigation_NavigateRecentLocationsClicked;
            this.ntsNavigation.NavigateRefreshClick += this.NtsNavigation_NavigateRefreshClick;

            // TODO: Update status bar to values of currently active Document

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

        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
        protected override void WndProc(ref Message m)
        {
            switch ((WinAPI.WM)m.Msg)
            {
                case WinAPI.WM.ClipboardUpdate:
                    AppContext.TraceDebug("WM_CLIPBOARDUPDATE");

                    break;
            }

            base.WndProc(ref m);
        }

        private void DpnDockPanel_ActiveContentChanged(object sender, EventArgs e)
        {
            var activeShellBrowser = this.dpnDockPanel.ActiveContent as ShellBrowserDockContent;

            if (activeShellBrowser is null)
            {
                AppContext.TraceDebug("DpnDockPanel_ActiveContentChanged, Type:" + sender.GetType().ToString());
            }
            else
            {
                this.ActiveShellBrowserChanged(activeShellBrowser, e);
            }
        }

        protected void ActiveShellBrowserChanged(ShellBrowserDockContent shellBrowser, EventArgs e)
        {
            AppContext.TraceDebug("Activating '" + shellBrowser.Text + "'");

            // Update Navigation Log and check for all possible changes
            // Issue #21 Refactoring: Remove Windows-API-Code-Pack
            //var navigationLogEventArgs = new Microsoft.WindowsAPICodePack.Controls.NavigationLogEventArgs()
            //{
            //    CanNavigateBackwardChanged = true,
            //    CanNavigateForwardChanged = true,
            //    LocationsChanged = true,
            //};
            //this.ntsNavigation.UpdateNavigationLog(navigationLogEventArgs, shellBrowser.NavigationLog);

            // Update Status Bar
            this.UpdateTslItemCount(shellBrowser);
            this.UpdateTslSelectionCount(shellBrowser);
        }

        [Conditional("DEBUG")]
        private void FormTitle_AddDebugRemark()
        {
            base.Text += " [DEBUG]";
        }

        private void CreateNewFileBrowser(DockAlignment? dockAlignment = null)
        {
            var newDockContent = new Components.DockContents.ShellBrowserDockContent();
            var activeDocumentPane = this.dpnDockPanel.ActiveDocumentPane;

            AppContext.TraceScope();

            newDockContent.ItemsChanged += this.NewDockContent_ItemsChanged;
            newDockContent.SelectionChanged += this.NewDockContent_SelectionChanged;
            //newDockContent.NavigationLogChanged += this.NewDockContent_NavigationLogChanged;


            // See <href="https://github.com/dockpanelsuite/dockpanelsuite/issues/348"/>, we only take care of DocumentStyle.DockingWindow
            //
            //if (this.dpnDockPanel.DocumentStyle == WeifenLuo.WinFormsUI.Docking.DocumentStyle.SystemMdi)
            //{
            //    newDockContent.Show();
            //    newDockContent.MdiParent = this;
            //} else ...

            if ((dockAlignment != null) && (activeDocumentPane != null))
                newDockContent.Show(activeDocumentPane, dockAlignment.Value, 0.5d);
            else
                newDockContent.Show(this.dpnDockPanel, DockState.Document);

            // TODO: Add windows to open window list
            // TODO: Browse to standard folder...
        }

        public bool LoadConfiguration(string fullFileName)
        {
            try
            {
                this.dpnDockPanel.LoadFromXml(fullFileName, new DeserializeDockContent(this.DockContent_Deserialize));
            }
            catch (Exception e)
            {
                MessageBox.Show("Electrifier.cs->LoadConfiguration" +
                    "\n\tError loading configuarion file." +
                    "\n\nError description: " + e.Message);
            }

            return true;
        }

        private IDockContent DockContent_Deserialize(string persistString)
        {
            // e.g. PersistString="ShellBrowserDockContent URI=file:///S:/%5BGit.Workspace%5D/electrifier"
            var typeNameSeperatorPos = persistString.IndexOf(" ");
            string dockContentTypeName, dockContentArguments = null;

            if (typeNameSeperatorPos < 0)
                dockContentTypeName = persistString;
            else
            {
                dockContentTypeName = persistString.Substring(0, typeNameSeperatorPos);
                dockContentArguments = persistString.Substring(typeNameSeperatorPos);
            }



            if (nameof(ShellBrowserDockContent).Equals(dockContentTypeName, StringComparison.CurrentCultureIgnoreCase))
            {
                var newDockContent = new Components.DockContents.ShellBrowserDockContent(dockContentArguments);

                newDockContent.ItemsChanged += this.NewDockContent_ItemsChanged;
                newDockContent.SelectionChanged += this.NewDockContent_SelectionChanged;
                //newDockContent.NavigationLogChanged += this.NewDockContent_NavigationLogChanged;

                return newDockContent;
            }

            return null;        // TODO: Throw Exception cause of unkown type in XML?!?
        }

        /// <summary>
        /// Called by AppContext.Session.SaveConfiguration()
        /// </summary>
        /// <param name="fullFileName">The full file name including its path.</param>
        public void SaveConfiguration(string fullFileName)
        {
            this.dpnDockPanel.SaveAsXml(fullFileName);
        }

        #region Event Listeners ===============================================================================================

        private void TsbNewFileBrowser_ButtonClick(object sender, EventArgs e)
        {
            AppContext.TraceScope();

            this.CreateNewFileBrowser();
        }

        private void TsbNewFileBrowserLeft_Click(object sender, EventArgs e)
        {
            AppContext.TraceScope();

            this.CreateNewFileBrowser(DockAlignment.Left);
        }

        private void TsbNewFileBrowserRight_Click(object sender, EventArgs e)
        {
            AppContext.TraceScope();

            this.CreateNewFileBrowser(DockAlignment.Right);
        }

        private void TsbNewFileBrowserTop_Click(object sender, EventArgs e)
        {
            AppContext.TraceScope();

            this.CreateNewFileBrowser(DockAlignment.Top);
        }

        private void TsbNewFileBrowserBottom_Click(object sender, EventArgs e)
        {
            AppContext.TraceScope();

            this.CreateNewFileBrowser(DockAlignment.Bottom);
        }

        private void TsbNewFileBrowserFloating_Click(object sender, EventArgs e)
        {
            AppContext.TraceScope();

            var newDockContent = new Components.DockContents.ShellBrowserDockContent();
            var floatWindowBounds = new Rectangle(this.Location, this.Size);

            floatWindowBounds.Offset((this.Width - this.ClientSize.Width), (this.Height - this.ClientSize.Height));

            newDockContent.ItemsChanged += this.NewDockContent_ItemsChanged;
            newDockContent.SelectionChanged += this.NewDockContent_SelectionChanged;
            //newDockContent.NavigationLogChanged += this.NewDockContent_NavigationLogChanged;

            newDockContent.Show(this.dpnDockPanel, floatWindowBounds);
        }

        #endregion

        private void NewDockContent_ItemsChanged(ShellBrowserDockContent sender, EventArgs eventArgs)
        {
            this.UpdateTslItemCount(sender);
        }

        private void NewDockContent_SelectionChanged(ShellBrowserDockContent sender, EventArgs eventArgs)
        {
            this.UpdateTslSelectionCount(sender);
        }

        protected void UpdateTslItemCount(ShellBrowserDockContent activeContent)
        {
            string txt = null;

            if (!(activeContent is null))
            {
                int itemCount = activeContent.ItemsCount;

                switch (itemCount)
                {
                    case 1:
                        txt = "1 item";
                        break;
                    default:
                        txt = itemCount + " items";
                        break;
                }
            }

            this.tslItemCount.Text = txt;
        }

        protected void UpdateTslSelectionCount(ShellBrowserDockContent activeContent)
        {
            string txt = null;

            if (!(activeContent is null))
            {
                int selectedItemsCount = activeContent.SelectedItemsCount;

                switch (selectedItemsCount)
                {
                    case 0:
                        break;
                    case 1:
                        txt = "1 item selected";
                        break;
                    default:
                        txt = selectedItemsCount + " items selected";
                        break;
                }
            }

            this.tslSelectionCount.Text = txt;
        }

        // Issue #21 Refactoring: Remove Windows-API-Code-Pack
        private void NewDockContent_NavigationLogChanged(object sender, /*Microsoft.WindowsAPICodePack.Controls.NavigationLogEventArgs*/ IntPtr e)
        {
            //// TODO: Check if this DockContent is (still) the ActiveContent
            //// TODO: Or, Change NavigationLogEventArgs to send the active ShellBrowserDockContent with it
            //this.ntsNavigation.UpdateNavigationLog(e, this.GetActiveShellBrowserDockContent().NavigationLog);

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ShellBrowserDockContent GetActiveShellBrowserDockContent()
        {
            return (this.dpnDockPanel.ActiveDocumentPane?.ActiveContent as ShellBrowserDockContent);
        }

        private void NtsNavigation_NavigateBackwardClick(object sender, EventArgs eventArgs)
        {
            this.GetActiveShellBrowserDockContent()?.NavigateBackward();
        }

        private void NtsNavigation_NavigateForwardClick(object sender, EventArgs eventArgs)
        {
            this.GetActiveShellBrowserDockContent()?.NavigateForward();
        }

        private void NtsNavigation_NavigateRecentLocationsClicked(object sender, Components.Controls.NavigationToolStrip.NavigateRecentLocationsEventArgs e)
        {
            this.GetActiveShellBrowserDockContent()?.NavigateLogLocation(e.NavigationLogIndex);
        }

        private void NtsNavigation_NavigateRefreshClick(object sender, EventArgs e)
        {
            this.GetActiveShellBrowserDockContent()?.NavigateRefresh();
        }

    }
}
