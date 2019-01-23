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

using Vanara.PInvoke;


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
            this.cmdQATOpenNewShellBrowserPane.ExecuteEvent += new System.EventHandler<Sunburst.WindowsForms.Ribbon.Controls.Events.ExecuteEventArgs>(this.CmdAppOpenNewShellBrowserPane_ExecuteEvent);

            //
            // Application Menu Items =========================================================================================
            //
            this.cmdAppOpenNewWindow = new RibbonButton(this.rbnRibbon, (uint)RibbonCommand.cmdAppOpenNewWindow)
            {
                Enabled = false,
                //ExecuteEvent += new System.EventHandler<Sunburst.WindowsForms.Ribbon.Controls.Events.ExecuteEventArgs>(this.CmdAppOpenNewWindow_Execute),
            };

            this.cmdAppOpenNewShellBrowserPane = new RibbonButton(this.rbnRibbon, (uint)RibbonCommand.cmdAppOpenNewShellBrowserPane);
            this.cmdAppOpenNewShellBrowserPane.ExecuteEvent += new System.EventHandler<Sunburst.WindowsForms.Ribbon.Controls.Events.ExecuteEventArgs>(this.CmdAppOpenNewShellBrowserPane_ExecuteEvent);

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
            this.cmdAppHelpAboutElectrifier.ExecuteEvent += new System.EventHandler<Sunburst.WindowsForms.Ribbon.Controls.Events.ExecuteEventArgs>(this.CmdAppHelpAboutElectrifier_ExecuteEvent);

            this.cmdAppHelpAboutWindows = new RibbonButton(this.rbnRibbon, (uint)RibbonCommand.cmdAppHelpAboutWindows);
            this.cmdAppHelpAboutWindows.ExecuteEvent += new System.EventHandler<Sunburst.WindowsForms.Ribbon.Controls.Events.ExecuteEventArgs>(this.CmdAppHelpAboutWindows_ExecuteEvent);

            this.cmdAppClose = new RibbonButton(this.rbnRibbon, (uint)RibbonCommand.cmdAppClose);
            this.cmdAppClose.ExecuteEvent += new EventHandler<Sunburst.WindowsForms.Ribbon.Controls.Events.ExecuteEventArgs>(this.CmdAppClose_ExecuteEvent);

            this.cmdTabHome = new RibbonTab(this.rbnRibbon, (uint)RibbonCommand.cmdTabHome);
            //
            // Command Group: Home -> Clipboard ===============================================================================
            //
            this.cmdGrpHomeClipboard = new RibbonButton(this.rbnRibbon, (uint)RibbonCommand.cmdGrpHomeClipboard);

            this.cmdBtnClipboardCut = new RibbonButton(this.rbnRibbon, (uint)RibbonCommand.cmdClipboardCut);
            this.cmdBtnClipboardCut.Enabled = false;

            this.cmdBtnClipboardCopy = new RibbonButton(this.rbnRibbon, (uint)RibbonCommand.cmdClipboardCopy);
            //this.cmdBtnClipboardCopy.ExecuteEvent += this.CmdClipboardCopy_ExecuteEvent;
            this.cmdBtnClipboardCopy.Enabled = false;

            this.cmdBtnClipboardPaste = new RibbonButton(this.rbnRibbon, (uint)RibbonCommand.cmdClipboardPaste);
            this.cmdBtnClipboardPaste.ExecuteEvent += this.CmdClipboardPaste_ExecuteEvent;

            //
            // Command Group: Home -> Organize ================================================================================
            //
            this.cmdGrpHomeOrganize = new RibbonButton(this.rbnRibbon, (uint)RibbonCommand.cmdGrpHomeOrganize);

            this.cmdBtnOrganizeMoveTo = new RibbonButton(this.rbnRibbon, (uint)RibbonCommand.cmdOrganizeMoveTo)
            {
                Enabled = false,
                //ExecuteEvent += new System.EventHandler<Sunburst.WindowsForms.Ribbon.Controls.Events.ExecuteEventArgs>(this.CmdBtnOrganizeMoveTo_Execute),
            };

            this.cmdBtnOrganizeDelete = new RibbonButton(this.rbnRibbon, (uint)RibbonCommand.cmdOrganizeDelete)
            {
                Enabled = false,
                //ExecuteEvent += new System.EventHandler<Sunburst.WindowsForms.Ribbon.Controls.Events.ExecuteEventArgs>(this.CmdBtnOrganizeDelete_Execute),
            };

            this.cmdBtnOrganizeRename = new RibbonButton(this.rbnRibbon, (uint)RibbonCommand.cmdOrganizeRename)
            {
                Enabled = false,
                //ExecuteEvent += new System.EventHandler<Sunburst.WindowsForms.Ribbon.Controls.Events.ExecuteEventArgs>(this.CmdBtnOrganizeRename_Execute),
            };
        }

        #region Ribbon event listeners ========================================================================================

        private void CmdAppOpenNewShellBrowserPane_ExecuteEvent(object sender, Sunburst.WindowsForms.Ribbon.Controls.Events.ExecuteEventArgs e)
        {
            AppContext.TraceScope();

            this.CreateNewFileBrowser();
        }

        private void CmdAppHelpAboutElectrifier_ExecuteEvent(object sender, Sunburst.WindowsForms.Ribbon.Controls.Events.ExecuteEventArgs e)
        {
            AppContext.TraceScope();

            var aboutElectrifier = new AboutElectrifier();

            aboutElectrifier.ShowDialog();
        }

        private void CmdAppHelpAboutWindows_ExecuteEvent(object sender, Sunburst.WindowsForms.Ribbon.Controls.Events.ExecuteEventArgs e)
        {
            AppContext.TraceScope();

            string szOtherStuff = ".NET Framework Environment Version: " + Environment.Version.ToString();

            Shell32.ShellAbout(this.Handle, @"electrifier - Windows Info", szOtherStuff, IntPtr.Zero);
        }


        private void CmdAppClose_ExecuteEvent(object sender, Sunburst.WindowsForms.Ribbon.Controls.Events.ExecuteEventArgs e)
        {
            AppContext.TraceScope();

            // Close form asynchronously since we are in a ribbon event handler, so the ribbon is still in use, and calling Close 
            // will eventually call _ribbon.DestroyFramework(), which is a big no-no, if you still use the ribbon.
            this.BeginInvoke(new MethodInvoker(this.Close));
        }

        private void CmdClipboardCopy_ExecuteEvent(object sender, Sunburst.WindowsForms.Ribbon.Controls.Events.ExecuteEventArgs e)
        {
            AppContext.TraceScope();

            //11.11.18:

            //common.Interop.WinShell.IShellFolder desktop = common.Interop.WinShell.DesktopFolder.Get();



            //StringBuilder itemsText = new StringBuilder();

            //foreach (ShellObject item in explorerBrowser.SelectedItems)
            //{
            //    if (item != null)
            //        itemsText.AppendLine("\tItem = " + item.GetDisplayName(DisplayNameType.Default));
            //}

            //this.selectedItemsTextBox.Text = itemsText.ToString();
            //this.itemsTabControl.TabPages[1].Text = "Selected Items (Count=" + explorerBrowser.SelectedItems.Count.ToString() + ")";


            // https://docs.microsoft.com/en-us/dotnet/api/system.windows.clipboard.setfiledroplist?view=netframework-4.7.2

            /* Wichtig:https://docs.microsoft.com/en-us/windows/desktop/shell/dragdrop
             * Clipboard Data Transfers
                The Clipboard is the simplest way to transfer Shell data. The basic procedure is similar to standard Clipboard data transfers.
                However, because you are transferring a pointer to a data object, not the data itself, you must use the OLE clipboard API instead
                of the standard clipboard API. The following procedure outlines how to use the OLE clipboard API to transfer Shell data with the Clipboard:

                * The data source creates a data object to contain the data.
                * The data source calls OleSetClipboard, which places a pointer to the data object's IDataObject interface on the Clipboard.
                * The target calls OleGetClipboard to retrieve the pointer to the data object's IDataObject interface.
                * The target extracts the data by calling the IDataObject::GetData method.
                * With some Shell data transfers, the target might also need to call the data object's IDataObject::SetData method to provide
                  feedback to the data object on the outcome of the data transfer. See Handling Optimized Move Operations for an example of this type of operation.
             */


        }



        public const string CFStr_Filename = "FileNameW";
        public const string CFStr_PreferredDropEffect = "Preferred DropEffect";
        public const string CFStr_ShellIdList = "Shell IDList Array";
        public const string CFStr_ShellIdListOffset = "Shell Object Offsets";



        private void CmdClipboardPaste_ExecuteEvent(object sender, Sunburst.WindowsForms.Ribbon.Controls.Events.ExecuteEventArgs e)
        {
            AppContext.TraceScope();

            if (!this.ClipboardIsKnownFormat())
            {
                AppContext.TraceDebug("No known format or empty clipboard pasted");

                return;
            }

            var dropEffect = this.EvaluateClipboardDropEffect();

            if (dropEffect == DragDropEffects.None)
            {
                // TODO: Only while testing we want to get noticed of cases when Prefrerred DropEffect is not set!
                throw new FormatException("CmdClipboardPaste_ExecuteEvent: Unknown DragDropEffect");
            }

            //var ShellIDList = Clipboard.GetData("CFSTR_SHELLIDLIST");      // 31.10.18: We're looking for CFSTR_SHELLIDLIST or CFSTR_SHELLIDLISTOFFSET

            //if (Clipboard.ContainsFileDropList())     // TODO: Use Vanara!
            //{
            //    var scFileDropList = Clipboard.GetFileDropList();

            //    if (scFileDropList.Count < 1)
            //        return;

            //    using (var shellFileOperation = new WindowsShell.FileOperation(this.Handle))
            //    {
            //        // TODO: When DragDropEffects.Move, move items instead of copying!

            //        foreach (var strFullPathName in scFileDropList)
            //        {
            //            var destFolder = System.IO.Path.GetDirectoryName(strFullPathName);
            //            // TODO: Get Destination folder (Active ShellBrowser!)

            //            shellFileOperation.CopyItem(strFullPathName, destFolder);
            //        }

            //        shellFileOperation.PerformOperations();
            //    }
            //}
        }

        private bool ClipboardIsKnownFormat()
        {
            if (Clipboard.ContainsFileDropList())
                return true;

            return false;
        }

        /// <summary>
        /// Determines the DragDropEffect of the current Clipboard object.
        /// </summary>
        /// <returns>DragDropEffects.Move or DragDropEffects.Copy if succeeded, DragDropEffects.None if validation failed.</returns>
        private DragDropEffects EvaluateClipboardDropEffect()
        {
            var dropEffect = DragDropEffects.None;
            var objDropEffect = Clipboard.GetData(CFStr_PreferredDropEffect);

            if ((objDropEffect is System.IO.MemoryStream msDropEffect) && (msDropEffect.CanRead) && (msDropEffect.Length > 0))
            {
                var baDropEffect = new byte[1];

                msDropEffect.Read(baDropEffect, 0, 1);

                dropEffect = (((baDropEffect[0] & (byte)DragDropEffects.Move) != 0) ? DragDropEffects.Move : DragDropEffects.Copy);
            }

            return dropEffect;
        }



        // 31.10. 21.15: https://stackoverflow.com/questions/2077981/cut-files-to-clipboard-in-c-sharp

        #endregion Ribbon event listeners =====================================================================================
    }
}
