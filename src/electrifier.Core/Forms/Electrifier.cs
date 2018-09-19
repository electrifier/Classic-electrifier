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
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

using electrifier.Win32API;
using electrifier.Core.Components.DockContents;

namespace electrifier.Core.Forms
{
    /// <summary>
    /// The main Window of electrifier application.
    /// </summary>
    public partial class Electrifier : System.Windows.Forms.Form
    {
        #region Fields ========================================================================================================

        protected Guid guid = Guid.NewGuid();

        private static string formTitle_Affix = String.Format("electrifier v{0}", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString());

        #endregion ============================================================================================================

        #region Properties ====================================================================================================

        public Guid Guid { get { return this.guid; } }

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
            this.Text = this.Text;  // TODO: Add formTitleAffix



            this.dpnDockPanel.Theme = new WeifenLuo.WinFormsUI.Docking.VS2015LightTheme();

        }

        [Conditional("DEBUG")]
        private void FormTitle_AddDebugRemark()
        {
            AppContext.TraceScope();

            base.Text += " [DEBUG]";
        }

        private void CreateNewFileBrowser(DockAlignment? dockAlignment = null)
        {
            var newDockContent = new Components.DockContents.ShellBrowserDockContent();
            var activeDocumentPane = this.dpnDockPanel.ActiveDocumentPane;

            AppContext.TraceScope();

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
                return new ShellBrowserDockContent(dockContentArguments);
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

        #region Ribbon event listeners ========================================================================================

        private void CmdAppOpenNewShellBrowserPane_Execute(object sender, Sunburst.WindowsForms.Ribbon.Controls.Events.ExecuteEventArgs e)
        {
            AppContext.TraceScope();

            this.CreateNewFileBrowser();
        }

        private void CmdAppHelpAboutElectrifier_Execute(object sender, Sunburst.WindowsForms.Ribbon.Controls.Events.ExecuteEventArgs e)
        {
            AppContext.TraceScope();

            var aboutElectrifier = new AboutElectrifier();

            aboutElectrifier.ShowDialog();
        }

        private void CmdAppHelpAboutWindows_Execute(object sender, Sunburst.WindowsForms.Ribbon.Controls.Events.ExecuteEventArgs e)
        {
            AppContext.TraceScope();

            string szOtherStuff = ".NET Framework Version: " + Environment.Version.ToString();

            ShellAPI.ShellAbout(this.Handle, "Microsoft Windows", szOtherStuff, IntPtr.Zero);
        }


        private void CmdAppClose_Execute(object sender, Sunburst.WindowsForms.Ribbon.Controls.Events.ExecuteEventArgs e)
        {
            AppContext.TraceScope();

            // Close form asynchronously since we are in a ribbon event 
            // handler, so the ribbon is still in use, and calling Close 
            // will eventually call _ribbon.DestroyFramework(), which is 
            // a big no-no, if you still use the ribbon.
            this.BeginInvoke(new MethodInvoker(this.Close));
        }

        #endregion Ribbon event listeners =====================================================================================

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

            newDockContent.Show(this.dpnDockPanel, floatWindowBounds);
        }

        #endregion
    }
}
