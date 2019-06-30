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
using System.Windows.Forms;
using electrifier.Core.Components;
using Sunburst.WindowsForms.Ribbon.Controls;
using Vanara.PInvoke;
using WeifenLuo.WinFormsUI.Docking;


namespace electrifier.Core.Forms
{
    /// <summary>
    /// The main Window of electrifier application.
    /// </summary>
    public partial class ElApplicationWindow
    {

        // ==> TODO: 26.06. 12:00: Move Ribbon-stuff into its own sub-class

        #region Fields ========================================================================================================

        private ElClipboardAbilities clipboardAbilities = ElClipboardAbilities.CanCut | ElClipboardAbilities.CanCopy;

        #endregion Fields =====================================================================================================


        private enum RibbonCommandID : uint
        {
            //
            // Quick Access Toolbar Commands ==================================================================================
            //
            cmdQATOpenNewShellBrowserPane = 19903,

            //
            // Application Menu Items =========================================================================================
            //
            //cmdAppApplicationMenu = 100,
            cmdAppOpenNewWindow = 101,
            cmdAppOpenNewShellBrowserPane = 102,
            cmdAppOpenCommandPrompt = 103,
            cmdAppOpenWindowsPowerShell = 104,
            cmdAppChangeElectrifierOptions = 110,
            cmdAppChangeFolderAndSearchOptions = 111,
            //cmdApp_HelpMenu = 120,
            cmdAppHelp = 121,
            cmdAppHelpAboutElectrifier = 122,
            cmdAppHelpAboutWindows = 125,
            cmdAppClose = 130,
            //
            // Ribbon tabs ====================================================================================================
            //
            cmdTabHome = 1000,
            cmdTabShare = 2000,
            cmdTabView = 3000,
            //
            // Command Group: Home -> Clipboard ===============================================================================
            //
            cmdGrpHomeClipboard = 1100,
            cmdClipboardCut = 1101,
            cmdClipboardCopy = 1102,
            cmdClipboardPaste = 1103,
            //
            // Command Group: Home -> Organize ================================================================================
            //
            cmdGrpHomeOrganize = 1200,
            cmdOrganizeMoveTo = 1201,
            cmdOrganizeDelete = 1202,
            cmdOrganizeRename = 1203,
        }


#pragma warning disable IDE0052 // Remove unread private members

        // Quick Access Toolbar Commands

        private RibbonButton cmdQATOpenNewShellBrowserPane;

        // Application Menu Commands

        private RibbonButton cmdAppOpenNewWindow;
        private RibbonButton cmdAppOpenNewShellBrowserPane;
        private RibbonButton cmdAppOpenCommandPrompt;
        private RibbonButton cmdAppOpenWindowsPowerShell;
        private RibbonButton cmdAppChangeElectrifierOptions;
        private RibbonButton cmdAppChangeFolderAndSearchOptions;
        private RibbonButton cmdAppHelp;
        private RibbonButton cmdAppHelpAboutElectrifier;
        private RibbonButton cmdAppHelpAboutWindows;
        private RibbonButton cmdAppClose;

        // Ribbon Tab: Home Commands

        private RibbonTab cmdTabHome;
        private RibbonButton cmdGrpHomeClipboard;
        private RibbonButton cmdBtnClipboardCut;
        private RibbonButton cmdBtnClipboardCopy;
        private RibbonButton cmdBtnClipboardPaste;
        private RibbonButton cmdGrpHomeOrganize;
        private RibbonButton cmdBtnOrganizeMoveTo;
        private RibbonButton cmdBtnOrganizeDelete;
        private RibbonButton cmdBtnOrganizeRename;

#pragma warning restore IDE0052 // Remove unread private members

        protected ElClipboardAbilities ClipboardAbilities {
            get => this.clipboardAbilities;
            set {
                if (this.clipboardAbilities != value)
                {
                    this.clipboardAbilities = value;

                    // Update ribbon command button states
                    this.cmdBtnClipboardCut.Enabled = value.HasFlag(ElClipboardAbilities.CanCut);
                    this.cmdBtnClipboardCopy.Enabled = value.HasFlag(ElClipboardAbilities.CanCopy);
                }
            }
        }



        private void InitializeRibbon()
        {
            //
            // Quick Access Toolbar Commands ==================================================================================
            //
            this.cmdQATOpenNewShellBrowserPane = new RibbonButton(this.rbnRibbon, (uint)RibbonCommandID.cmdQATOpenNewShellBrowserPane);
            this.cmdQATOpenNewShellBrowserPane.ExecuteEvent += new System.EventHandler<Sunburst.WindowsForms.Ribbon.Controls.Events.ExecuteEventArgs>(this.CmdAppOpenNewShellBrowserPane_ExecuteEvent);

            //
            // Application Menu Items =========================================================================================
            //
            this.cmdAppOpenNewWindow = new RibbonButton(this.rbnRibbon, (uint)RibbonCommandID.cmdAppOpenNewWindow)
            {
                Enabled = false,
                //ExecuteEvent += new System.EventHandler<Sunburst.WindowsForms.Ribbon.Controls.Events.ExecuteEventArgs>(this.CmdAppOpenNewWindow_Execute),
            };

            this.cmdAppOpenNewShellBrowserPane = new RibbonButton(this.rbnRibbon, (uint)RibbonCommandID.cmdAppOpenNewShellBrowserPane);
            this.cmdAppOpenNewShellBrowserPane.ExecuteEvent += new System.EventHandler<Sunburst.WindowsForms.Ribbon.Controls.Events.ExecuteEventArgs>(this.CmdAppOpenNewShellBrowserPane_ExecuteEvent);

            this.cmdAppOpenCommandPrompt = new RibbonButton(this.rbnRibbon, (uint)RibbonCommandID.cmdAppOpenCommandPrompt)
            {
                Enabled = false,
                //ExecuteEvent += new System.EventHandler<Sunburst.WindowsForms.Ribbon.Controls.Events.ExecuteEventArgs>(this.CmdAppOpenCommandPrompt_Execute),
            };

            this.cmdAppOpenWindowsPowerShell = new RibbonButton(this.rbnRibbon, (uint)RibbonCommandID.cmdAppOpenWindowsPowerShell)
            {
                Enabled = false,
                //ExecuteEvent += new System.EventHandler<Sunburst.WindowsForms.Ribbon.Controls.Events.ExecuteEventArgs>(this.CmdAppOpenWindowsPowerShell_Execute),
            };


            this.cmdAppChangeElectrifierOptions = new RibbonButton(this.rbnRibbon, (uint)RibbonCommandID.cmdAppChangeElectrifierOptions)
            {
                Enabled = false,
                //ExecuteEvent += new System.EventHandler<Sunburst.WindowsForms.Ribbon.Controls.Events.ExecuteEventArgs>(this.CmdAppChangeElectrifierOptions_Execute),
            };

            this.cmdAppChangeFolderAndSearchOptions = new RibbonButton(this.rbnRibbon, (uint)RibbonCommandID.cmdAppChangeFolderAndSearchOptions)
            {
                Enabled = false,
                //ExecuteEvent += new System.EventHandler<Sunburst.WindowsForms.Ribbon.Controls.Events.ExecuteEventArgs>(this.CmdAppChangeFolderAndSearchOptions_Execute),
            };

            this.cmdAppHelp = new RibbonButton(this.rbnRibbon, (uint)RibbonCommandID.cmdAppHelp);
            //this.cmdAppHelp.ExecuteEvent += new System.EventHandler<Sunburst.WindowsForms.Ribbon.Controls.Events.ExecuteEventArgs>(this.CmdAppHelp_Execute);

            this.cmdAppHelpAboutElectrifier = new RibbonButton(this.rbnRibbon, (uint)RibbonCommandID.cmdAppHelpAboutElectrifier);
            this.cmdAppHelpAboutElectrifier.ExecuteEvent += new System.EventHandler<Sunburst.WindowsForms.Ribbon.Controls.Events.ExecuteEventArgs>(this.CmdAppHelpAboutElectrifier_ExecuteEvent);

            this.cmdAppHelpAboutWindows = new RibbonButton(this.rbnRibbon, (uint)RibbonCommandID.cmdAppHelpAboutWindows);
            this.cmdAppHelpAboutWindows.ExecuteEvent += new System.EventHandler<Sunburst.WindowsForms.Ribbon.Controls.Events.ExecuteEventArgs>(this.CmdAppHelpAboutWindows_ExecuteEvent);

            this.cmdAppClose = new RibbonButton(this.rbnRibbon, (uint)RibbonCommandID.cmdAppClose);
            this.cmdAppClose.ExecuteEvent += new EventHandler<Sunburst.WindowsForms.Ribbon.Controls.Events.ExecuteEventArgs>(this.CmdAppClose_ExecuteEvent);

            this.cmdTabHome = new RibbonTab(this.rbnRibbon, (uint)RibbonCommandID.cmdTabHome);
            //
            // Command Group: Home -> Clipboard ===============================================================================
            //
            this.cmdGrpHomeClipboard = new RibbonButton(this.rbnRibbon, (uint)RibbonCommandID.cmdGrpHomeClipboard);

            this.cmdBtnClipboardCut = new RibbonButton(this.rbnRibbon, (uint)RibbonCommandID.cmdClipboardCut);
            this.cmdBtnClipboardCut.ExecuteEvent += this.CmdClipboardCut_ExecuteEvent;

            this.cmdBtnClipboardCopy = new RibbonButton(this.rbnRibbon, (uint)RibbonCommandID.cmdClipboardCopy);
            this.cmdBtnClipboardCopy.ExecuteEvent += this.CmdClipboardCopy_ExecuteEvent;

            this.cmdBtnClipboardPaste = new RibbonButton(this.rbnRibbon, (uint)RibbonCommandID.cmdClipboardPaste);
            this.cmdBtnClipboardPaste.ExecuteEvent += this.CmdClipboardPaste_ExecuteEvent;

            //
            // Command Group: Home -> Organize ================================================================================
            //
            this.cmdGrpHomeOrganize = new RibbonButton(this.rbnRibbon, (uint)RibbonCommandID.cmdGrpHomeOrganize);

            this.cmdBtnOrganizeMoveTo = new RibbonButton(this.rbnRibbon, (uint)RibbonCommandID.cmdOrganizeMoveTo)
            {
                Enabled = false,
                //ExecuteEvent += new System.EventHandler<Sunburst.WindowsForms.Ribbon.Controls.Events.ExecuteEventArgs>(this.CmdBtnOrganizeMoveTo_Execute),
            };

            this.cmdBtnOrganizeDelete = new RibbonButton(this.rbnRibbon, (uint)RibbonCommandID.cmdOrganizeDelete)
            {
                Enabled = false,
                //ExecuteEvent += new System.EventHandler<Sunburst.WindowsForms.Ribbon.Controls.Events.ExecuteEventArgs>(this.CmdBtnOrganizeDelete_Execute),
            };

            this.cmdBtnOrganizeRename = new RibbonButton(this.rbnRibbon, (uint)RibbonCommandID.cmdOrganizeRename)
            {
                Enabled = false,
                //ExecuteEvent += new System.EventHandler<Sunburst.WindowsForms.Ribbon.Controls.Events.ExecuteEventArgs>(this.CmdBtnOrganizeRename_Execute),
            };


        }

        /// <summary>
        /// TODO: https://docs.microsoft.com/de-de/dotnet/api/system.windows.dataobject?view=netframework-4.7.2
        /// </summary>
        private void Ribbon_ProcessDockContentChange(IDockContent activatedDockContent)
        {
            // In case activated DockContent is an IElClipboardConsumer, update the clipboard buttons accordingly
            this.ClipboardAbilities = (activatedDockContent is IElClipboardConsumer clipboardConsumer) ?
                clipboardConsumer.GetClipboardAbilities() :
                ElClipboardAbilities.None;
        }

        #region Ribbon event listeners ========================================================================================

        private void CmdAppOpenNewShellBrowserPane_ExecuteEvent(object sender, Sunburst.WindowsForms.Ribbon.Controls.Events.ExecuteEventArgs e)
        {
            AppContext.TraceScope();

            this.CreateNewShellBrowser();
        }

        private void TsbNewFileBrowser_ButtonClick(object sender, EventArgs e)
        {
            AppContext.TraceScope();

            this.CreateNewShellBrowser();
        }

        private void TsbNewFileBrowserLeft_Click(object sender, EventArgs e)
        {
            AppContext.TraceScope();

            this.CreateNewShellBrowser(WeifenLuo.WinFormsUI.Docking.DockAlignment.Left);
        }

        private void TsbNewFileBrowserRight_Click(object sender, EventArgs e)
        {
            AppContext.TraceScope();

            this.CreateNewShellBrowser(DockAlignment.Right);
        }

        private void TsbNewFileBrowserTop_Click(object sender, EventArgs e)
        {
            AppContext.TraceScope();

            this.CreateNewShellBrowser(DockAlignment.Top);
        }

        private void TsbNewFileBrowserBottom_Click(object sender, EventArgs e)
        {
            AppContext.TraceScope();

            this.CreateNewShellBrowser(DockAlignment.Bottom);
        }

        // TODO 03/02/19: TsbNewFileBrowserFloating_Click has not been used yet
        //private void TsbNewFileBrowserFloating_Click(object sender, EventArgs e)
        //{
        //    AppContext.TraceScope();

        //    var newDockContent = new Components.DockContents.ElShellBrowserDockContent();
        //    var floatWindowBounds = new Rectangle(this.Location, this.Size);

        //    floatWindowBounds.Offset((this.Width - this.ClientSize.Width), (this.Height - this.ClientSize.Height));

        //    newDockContent.ItemsChanged += this.NewDockContent_ItemsChanged;
        //    newDockContent.SelectionChanged += this.NewDockContent_SelectionChanged;
        //    //newDockContent.NavigationLogChanged += this.NewDockContent_NavigationLogChanged;

        //    newDockContent.Show(this.dpnDockPanel, floatWindowBounds);
        //}

        private void CmdAppHelpAboutElectrifier_ExecuteEvent(object sender, Sunburst.WindowsForms.Ribbon.Controls.Events.ExecuteEventArgs e)
        {
            AppContext.TraceScope();

            new ElAboutDialog().ShowDialog();
        }

        private void CmdAppHelpAboutWindows_ExecuteEvent(object sender, Sunburst.WindowsForms.Ribbon.Controls.Events.ExecuteEventArgs e)
        {
            AppContext.TraceScope();

            Shell32.ShellAbout(this.Handle, @"electrifier - Windows Info", AppContext.GetDotNetFrameworkVersion(), AppContext.Icon.Handle);
        }


        private void CmdAppClose_ExecuteEvent(object sender, Sunburst.WindowsForms.Ribbon.Controls.Events.ExecuteEventArgs e)
        {
            AppContext.TraceScope();

            // Close form asynchronously since we are in a ribbon event handler, so the ribbon is still in use, and calling Close 
            // will eventually call _ribbon.DestroyFramework(), which is a big no-no, if you still use the ribbon.
            this.BeginInvoke(new MethodInvoker(this.Close));
        }

        #endregion Ribbon event listeners =====================================================================================
    }
}
