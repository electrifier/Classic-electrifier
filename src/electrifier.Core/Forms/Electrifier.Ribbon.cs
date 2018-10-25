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
using System.Windows.Forms;
using Sunburst.WindowsForms.Ribbon.Controls;
using Sunburst.WindowsForms.Ribbon.Controls.Events;

using electrifier.Win32API;


namespace electrifier.Core.Forms
{
    /// <summary>
    /// The main Window of electrifier application.
    /// </summary>
    public partial class Electrifier : System.Windows.Forms.Form
    {
        private enum RibbonCommand : uint
        {
            //
            // Quick Access Toolbar Commands ==================================================================================
            //
            cmdQATOpenNewShellBrowserPane = 19903,

            //
            // Application Menu Items =========================================================================================
            //
            cmdAppApplicationMenu = 100,
            cmdAppOpenNewWindow = 101,
            cmdAppOpenNewShellBrowserPane = 102,
            cmdAppOpenCommandPrompt = 103,
            cmdAppOpenWindowsPowerShell = 104,
            cmdAppChangeElectrifierOptions = 110,
            cmdAppChangeFolderAndSearchOptions = 111,
            cmdApp_HelpMenu = 120,
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
            cmdBtnClipboardCut = 1101,
            cmdBtnClipboardCopy = 1102,
            cmdBtnClipboardPaste = 1103,
            //
            // Command Group: Home -> Organize ================================================================================
            //
            cmdGrpHomeOrganize = 1200,
            cmdBtnOrganizeMoveTo = 1201,
            cmdBtnOrganizeDelete = 1202,
            cmdBtnOrganizeRename = 1203,
        }


        private Sunburst.WindowsForms.Ribbon.Ribbon rbnRibbon;

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




        private void InitializeRibbon()
        {
            //
            // Quick Access Toolbar Commands ==================================================================================
            //
            this.cmdQATOpenNewShellBrowserPane = new RibbonButton(this.rbnRibbon, (uint)RibbonCommand.cmdQATOpenNewShellBrowserPane);
            this.cmdQATOpenNewShellBrowserPane.ExecuteEvent += new System.EventHandler<Sunburst.WindowsForms.Ribbon.Controls.Events.ExecuteEventArgs>(this.CmdAppOpenNewShellBrowserPane_Execute);

            //
            // Application Menu Items =========================================================================================
            //
            this.cmdAppOpenNewWindow = new RibbonButton(this.rbnRibbon, (uint)RibbonCommand.cmdAppOpenNewWindow)
            {
                Enabled = false,
                //ExecuteEvent += new System.EventHandler<Sunburst.WindowsForms.Ribbon.Controls.Events.ExecuteEventArgs>(this.CmdAppOpenNewWindow_Execute),
            };

            this.cmdAppOpenNewShellBrowserPane = new RibbonButton(this.rbnRibbon, (uint)RibbonCommand.cmdAppOpenNewShellBrowserPane);
            this.cmdAppOpenNewShellBrowserPane.ExecuteEvent += new System.EventHandler<Sunburst.WindowsForms.Ribbon.Controls.Events.ExecuteEventArgs>(this.CmdAppOpenNewShellBrowserPane_Execute);

            this.cmdAppOpenCommandPrompt = new RibbonButton(this.rbnRibbon, (uint)RibbonCommand.cmdAppOpenCommandPrompt)
            {
                Enabled = false,
                //ExecuteEvent += new System.EventHandler<Sunburst.WindowsForms.Ribbon.Controls.Events.ExecuteEventArgs>(this.CmdAppOpenCommandPrompt_Execute),
            };

            this.cmdAppOpenWindowsPowerShell = new RibbonButton(this.rbnRibbon, (uint)RibbonCommand.cmdAppOpenWindowsPowerShell)
            {
                Enabled = false,
                //ExecuteEvent += new System.EventHandler<Sunburst.WindowsForms.Ribbon.Controls.Events.ExecuteEventArgs>(this.CmdAppOpenWindowsPowerShell_Execute),
            };


            this.cmdAppChangeElectrifierOptions = new RibbonButton(this.rbnRibbon, (uint)RibbonCommand.cmdAppChangeElectrifierOptions)
            {
                Enabled = false,
                //ExecuteEvent += new System.EventHandler<Sunburst.WindowsForms.Ribbon.Controls.Events.ExecuteEventArgs>(this.CmdAppChangeElectrifierOptions_Execute),
            };

            this.cmdAppChangeFolderAndSearchOptions = new RibbonButton(this.rbnRibbon, (uint)RibbonCommand.cmdAppChangeFolderAndSearchOptions)
            {
                Enabled = false,
                //ExecuteEvent += new System.EventHandler<Sunburst.WindowsForms.Ribbon.Controls.Events.ExecuteEventArgs>(this.CmdAppChangeFolderAndSearchOptions_Execute),
            };

            this.cmdAppHelp = new RibbonButton(this.rbnRibbon, (uint)RibbonCommand.cmdAppHelp);
            //this.cmdAppHelp.ExecuteEvent += new System.EventHandler<Sunburst.WindowsForms.Ribbon.Controls.Events.ExecuteEventArgs>(this.CmdAppHelp_Execute);

            this.cmdAppHelpAboutElectrifier = new RibbonButton(this.rbnRibbon, (uint)RibbonCommand.cmdAppHelpAboutElectrifier);
            this.cmdAppHelpAboutElectrifier.ExecuteEvent += new System.EventHandler<Sunburst.WindowsForms.Ribbon.Controls.Events.ExecuteEventArgs>(this.CmdAppHelpAboutElectrifier_Execute);

            this.cmdAppHelpAboutWindows = new RibbonButton(this.rbnRibbon, (uint)RibbonCommand.cmdAppHelpAboutWindows);
            this.cmdAppHelpAboutWindows.ExecuteEvent += new System.EventHandler<Sunburst.WindowsForms.Ribbon.Controls.Events.ExecuteEventArgs>(this.CmdAppHelpAboutWindows_Execute);

            this.cmdAppClose = new RibbonButton(this.rbnRibbon, (uint)RibbonCommand.cmdAppClose);
            this.cmdAppClose.ExecuteEvent += new EventHandler<Sunburst.WindowsForms.Ribbon.Controls.Events.ExecuteEventArgs>(this.CmdAppClose_Execute);

            this.cmdTabHome = new RibbonTab(this.rbnRibbon, (uint)RibbonCommand.cmdTabHome);
            //
            // Command Group: Home -> Clipboard ===============================================================================
            //
            this.cmdGrpHomeClipboard = new RibbonButton(this.rbnRibbon, (uint)RibbonCommand.cmdGrpHomeClipboard)
            {
                Enabled = false,
                //ExecuteEvent += new System.EventHandler<Sunburst.WindowsForms.Ribbon.Controls.Events.ExecuteEventArgs>(this.CmdGrpHomeClipboard_Execute),
            };

            this.cmdBtnClipboardCut = new RibbonButton(this.rbnRibbon, (uint)RibbonCommand.cmdBtnClipboardCut)
            {
                Enabled = false,
                //ExecuteEvent += new System.EventHandler<Sunburst.WindowsForms.Ribbon.Controls.Events.ExecuteEventArgs>(this.CmdBtnClipboardCut_Execute),
            };

            this.cmdBtnClipboardCopy = new RibbonButton(this.rbnRibbon, (uint)RibbonCommand.cmdBtnClipboardCopy)
            {
                Enabled = false,
                //ExecuteEvent += new System.EventHandler<Sunburst.WindowsForms.Ribbon.Controls.Events.ExecuteEventArgs>(this.CmdBtnClipboardCopy_Execute),
            };

            this.cmdBtnClipboardPaste = new RibbonButton(this.rbnRibbon, (uint)RibbonCommand.cmdBtnClipboardPaste)
            {
                Enabled = false,
                //ExecuteEvent += new System.EventHandler<Sunburst.WindowsForms.Ribbon.Controls.Events.ExecuteEventArgs>(this.CmdBtnClipboardPaste_Execute),
            };

            //
            // Command Group: Home -> Organize ================================================================================
            //
            this.cmdGrpHomeOrganize = new RibbonButton(this.rbnRibbon, (uint)RibbonCommand.cmdGrpHomeOrganize)
            {
                Enabled = false,
                //ExecuteEvent += new System.EventHandler<Sunburst.WindowsForms.Ribbon.Controls.Events.ExecuteEventArgs>(this.CmdGrpHomeOrganize_Execute),
            };

            this.cmdBtnOrganizeMoveTo = new RibbonButton(this.rbnRibbon, (uint)RibbonCommand.cmdBtnOrganizeMoveTo)
            {
                Enabled = false,
                //ExecuteEvent += new System.EventHandler<Sunburst.WindowsForms.Ribbon.Controls.Events.ExecuteEventArgs>(this.CmdBtnOrganizeMoveTo_Execute),
            };

            this.cmdBtnOrganizeDelete = new RibbonButton(this.rbnRibbon, (uint)RibbonCommand.cmdBtnOrganizeDelete)
            {
                Enabled = false,
                //ExecuteEvent += new System.EventHandler<Sunburst.WindowsForms.Ribbon.Controls.Events.ExecuteEventArgs>(this.CmdBtnOrganizeDelete_Execute),
            };

            this.cmdBtnOrganizeRename = new RibbonButton(this.rbnRibbon, (uint)RibbonCommand.cmdBtnOrganizeRename)
            {
                Enabled = false,
                //ExecuteEvent += new System.EventHandler<Sunburst.WindowsForms.Ribbon.Controls.Events.ExecuteEventArgs>(this.CmdBtnOrganizeRename_Execute),
            };
        }

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

            // Close form asynchronously since we are in a ribbon event handler, so the ribbon is still in use, and calling Close 
            // will eventually call _ribbon.DestroyFramework(), which is a big no-no, if you still use the ribbon.
            this.BeginInvoke(new MethodInvoker(this.Close));
        }

        #endregion Ribbon event listeners =====================================================================================
    }
}
