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

using Sunburst.WindowsForms.Ribbon.Controls;

using WeifenLuo.WinFormsUI.Docking;

using electrifier.Core.Components;
using System;
using System.Diagnostics;

namespace electrifier.Core.Forms
{
    /// <summary>
    /// <see cref="ElApplicationWindow"/> is the Main Window of electrifier application.
    /// 
    /// This partial class file contains the creation of the Ribbon.
    /// </summary>
    public partial class ElApplicationWindow
    {
        private class ElApplicationWindowRibbon
          : Sunburst.WindowsForms.Ribbon.Ribbon
        {

            #region Fields ====================================================================================================

            private ElClipboardAbilities clipboardAbilities = ElClipboardAbilities.CanCut | ElClipboardAbilities.CanCopy;

            #endregion Fields =================================================================================================


            private enum RibbonCommandID : uint
            {
                //
                // Quick Access Toolbar Commands ==============================================================================
                //
                CmdQATOpenNewShellBrowserPane = 19903,

                //
                // Application Menu Items =====================================================================================
                //
                //cmdAppApplicationMenu = 100,
                CmdAppOpenNewWindow = 101,
                CmdAppOpenNewShellBrowserPane = 102,
                CmdAppOpenCommandPrompt = 103,
                CmdAppOpenWindowsPowerShell = 104,
                CmdAppChangeElectrifierOptions = 110,
                CmdAppChangeFolderAndSearchOptions = 111,
                //cmdApp_HelpMenu = 120,
                CmdAppHelp = 121,
                CmdAppHelpAboutElectrifier = 122,
                CmdAppHelpAboutWindows = 125,
                CmdAppClose = 130,
                //
                // Ribbon tabs ================================================================================================
                //
                CmdTabHome = 10000,
                CmdTabShare = 20000,
                CmdTabView = 30000,
                //
                // Command Group: Home -> Clipboard ===========================================================================
                //
                CmdGrpHomeClipboard = 1100,
                CmdClipboardCut = 1101,
                CmdClipboardCopy = 1102,
                CmdClipboardPaste = 1103,
                //
                // Command Group: Home -> Organise ============================================================================
                //
                CmdGrpHomeOrganise = 1200,
                CmdBtnOrganiseMoveTo = 1201,
                CmdBtnOrganiseCopyTo = 1202,
                CmdBtnOrganiseDelete = 1203,
                CmdBtnOrganiseRename = 1204,
                //
                // Command Group: Home -> Select ==============================================================================
                //
                CmdGrpHomeSelect = 1500,
                CmdBtnSelectSelectAll = 1501,
                CmdBtnSelectSelectNone = 1502,
                CmdBtnSelectInvertSelection = 1503,
            }

            public ElApplicationWindow ApplicationWindow { get; }

            //
            // Quick Access Toolbar Commands ==================================================================================
            //

            public RibbonButton CmdQATOpenNewShellBrowserPane { get; }

            //
            // Application Menu Commands ======================================================================================
            //

            public RibbonButton CmdAppOpenNewWindow { get; }
            public RibbonButton CmdAppOpenNewShellBrowserPane { get; }
            public RibbonButton CmdAppOpenCommandPrompt { get; }
            public RibbonButton CmdAppOpenWindowsPowerShell { get; }
            public RibbonButton CmdAppChangeElectrifierOptions { get; }
            public RibbonButton CmdAppChangeFolderAndSearchOptions { get; }
            public RibbonButton CmdAppHelp { get; }
            public RibbonButton CmdAppHelpAboutElectrifier { get; }
            public RibbonButton CmdAppHelpAboutWindows { get; }
            public RibbonButton CmdAppClose { get; }

            //
            // Ribbon Tab: Home Commands ======================================================================================
            //

            public RibbonTab CmdTabHome { get; }
            public RibbonButton CmdGrpHomeClipboard { get; }
            public RibbonButton CmdBtnClipboardCut { get; }
            public RibbonButton CmdBtnClipboardCopy { get; }
            public RibbonButton CmdBtnClipboardPaste { get; }
            public RibbonButton CmdGrpHomeOrganise { get; }
            public RibbonButton CmdBtnOrganiseMoveTo { get; }
            public RibbonButton CmdBtnOrganiseCopyTo { get; }
            public RibbonButton CmdBtnOrganiseDelete { get; }
            public RibbonButton CmdBtnOrganiseRename { get; }

            public RibbonButton CmdGrpHomeSelect { get; }
            public RibbonButton CmdBtnSelectSelectAll { get; }
            public RibbonButton CmdBtnSelectSelectNone { get; }
            public RibbonButton CmdBtnSelectInvertSelection { get; }

            public ElClipboardAbilities ClipboardAbilities {
                get => this.clipboardAbilities;
                set {
                    if (this.clipboardAbilities != value)
                    {
                        this.clipboardAbilities = value;

                        // Update ribbon command button states
                        this.CmdBtnClipboardCut.Enabled = value.HasFlag(ElClipboardAbilities.CanCut);
                        this.CmdBtnClipboardCopy.Enabled = value.HasFlag(ElClipboardAbilities.CanCopy);
                    }
                }
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="elApplicationWindow"></param>
            public ElApplicationWindowRibbon(ElApplicationWindow elApplicationWindow)
            {
                this.ApplicationWindow = elApplicationWindow ?? throw new ArgumentNullException(nameof(elApplicationWindow));

                //
                // Quick Access Toolbar Commands ==================================================================================
                //
                this.CmdQATOpenNewShellBrowserPane = new RibbonButton(this, (uint)RibbonCommandID.CmdQATOpenNewShellBrowserPane);
                this.CmdQATOpenNewShellBrowserPane.ExecuteEvent += this.ApplicationWindow.CmdAppOpenNewShellBrowserPane_ExecuteEvent;

                //
                // Application Menu Items =========================================================================================
                //
                this.CmdAppOpenNewWindow = new RibbonButton(this, (uint)RibbonCommandID.CmdAppOpenNewWindow)
                {
                    Enabled = false,
                    //ExecuteEvent += this.ApplicationWindow.CmdAppOpenNewWindow_Execute),
                };

                this.CmdAppOpenNewShellBrowserPane = new RibbonButton(this, (uint)RibbonCommandID.CmdAppOpenNewShellBrowserPane);
                this.CmdAppOpenNewShellBrowserPane.ExecuteEvent += this.ApplicationWindow.CmdAppOpenNewShellBrowserPane_ExecuteEvent;

                this.CmdAppOpenCommandPrompt = new RibbonButton(this, (uint)RibbonCommandID.CmdAppOpenCommandPrompt)
                {
                    Enabled = false,
                    //ExecuteEvent += this.ApplicationWindow.CmdAppOpenCommandPrompt_Execute),
                };

                this.CmdAppOpenWindowsPowerShell = new RibbonButton(this, (uint)RibbonCommandID.CmdAppOpenWindowsPowerShell)
                {
                    Enabled = false,
                    //ExecuteEvent += this.ApplicationWindow.CmdAppOpenWindowsPowerShell_Execute),
                };


                this.CmdAppChangeElectrifierOptions = new RibbonButton(this, (uint)RibbonCommandID.CmdAppChangeElectrifierOptions)
                {
                    Enabled = false,
                    //ExecuteEvent += this.ApplicationWindow.CmdAppChangeElectrifierOptions_Execute),
                };

                this.CmdAppChangeFolderAndSearchOptions = new RibbonButton(this, (uint)RibbonCommandID.CmdAppChangeFolderAndSearchOptions)
                {
                    Enabled = false,
                    //ExecuteEvent += this.ApplicationWindow.CmdAppChangeFolderAndSearchOptions_Execute),
                };

                this.CmdAppHelp = new RibbonButton(this, (uint)RibbonCommandID.CmdAppHelp);
                //this.cmdAppHelp.ExecuteEvent += this.ApplicationWindow.CmdAppHelp_Execute);

                this.CmdAppHelpAboutElectrifier = new RibbonButton(this, (uint)RibbonCommandID.CmdAppHelpAboutElectrifier);
                this.CmdAppHelpAboutElectrifier.ExecuteEvent += this.ApplicationWindow.CmdAppHelpAboutElectrifier_ExecuteEvent;

                this.CmdAppHelpAboutWindows = new RibbonButton(this, (uint)RibbonCommandID.CmdAppHelpAboutWindows);
                this.CmdAppHelpAboutWindows.ExecuteEvent += this.ApplicationWindow.CmdAppHelpAboutWindows_ExecuteEvent;

                this.CmdAppClose = new RibbonButton(this, (uint)RibbonCommandID.CmdAppClose);
                this.CmdAppClose.ExecuteEvent += this.ApplicationWindow.CmdAppClose_ExecuteEvent;

                this.CmdTabHome = new RibbonTab(this, (uint)RibbonCommandID.CmdTabHome);
                //
                // Command Group: Home -> Clipboard ===============================================================================
                //
                this.CmdGrpHomeClipboard = new RibbonButton(this, (uint)RibbonCommandID.CmdGrpHomeClipboard);

                this.CmdBtnClipboardCut = new RibbonButton(this, (uint)RibbonCommandID.CmdClipboardCut);
                this.CmdBtnClipboardCut.ExecuteEvent += this.ApplicationWindow.CmdClipboardCut_ExecuteEvent;

                this.CmdBtnClipboardCopy = new RibbonButton(this, (uint)RibbonCommandID.CmdClipboardCopy);
                this.CmdBtnClipboardCopy.ExecuteEvent += this.ApplicationWindow.CmdClipboardCopy_ExecuteEvent;

                this.CmdBtnClipboardPaste = new RibbonButton(this, (uint)RibbonCommandID.CmdClipboardPaste);
                this.CmdBtnClipboardPaste.ExecuteEvent += this.ApplicationWindow.CmdClipboardPaste_ExecuteEvent;

                //
                // Command Group: Home -> Organise ================================================================================
                //
                this.CmdGrpHomeOrganise = new RibbonButton(this, (uint)RibbonCommandID.CmdGrpHomeOrganise);

                this.CmdBtnOrganiseMoveTo = new RibbonButton(this, (uint)RibbonCommandID.CmdBtnOrganiseMoveTo)
                {
                    Enabled = false,
                    //ExecuteEvent += this.ApplicationWindow.CmdBtnOrganiseMoveTo_Execute),
                };

                this.CmdBtnOrganiseCopyTo = new RibbonButton(this, (uint)RibbonCommandID.CmdBtnOrganiseCopyTo)
                {
                    Enabled = false,
                    //ExecuteEvent += this.ApplicationWindow.CmdBtnOrganiseCopyTo_Execute),
                };

                this.CmdBtnOrganiseDelete = new RibbonButton(this, (uint)RibbonCommandID.CmdBtnOrganiseDelete)
                {
                    Enabled = false,
                    //ExecuteEvent += this.ApplicationWindow.CmdBtnOrganiseDelete_Execute),
                };

                this.CmdBtnOrganiseRename = new RibbonButton(this, (uint)RibbonCommandID.CmdBtnOrganiseRename)
                {
                    Enabled = false,
                    //ExecuteEvent += this.ApplicationWindow.CmdBtnOrganiseRename_Execute),
                };

                //
                // Command Group: Home -> Organise ================================================================================
                //
                this.CmdGrpHomeSelect = new RibbonButton(this, (uint)RibbonCommandID.CmdGrpHomeSelect);
                this.CmdBtnSelectSelectAll = new RibbonButton(this, (uint)RibbonCommandID.CmdBtnSelectSelectAll);
                this.CmdBtnSelectSelectNone = new RibbonButton(this, (uint)RibbonCommandID.CmdBtnSelectSelectNone);
                this.CmdBtnSelectInvertSelection = new RibbonButton(this, (uint)RibbonCommandID.CmdBtnSelectInvertSelection);
            }

            /// <summary>
            /// Process <see cref="DockPanel.ActiveContentChanged"/> event.
            /// 
            /// In case activated DockContent is an IElClipboardConsumer, update the clipboard buttons accordingly.
            /// </summary>
            /// <param name="activatedDockContent">The <see cref="IDockContent"/> that has been activated.</param>
            public void Ribbon_ProcessDockContentChange(IDockContent activatedDockContent)
            {
                // TODO: Move into [property].set()
                this.ClipboardAbilities = (activatedDockContent is IElClipboardConsumer clipboardConsumer) ?
                    clipboardConsumer.GetClipboardAbilities() :
                    ElClipboardAbilities.None;
            }

            /// <summary>
            /// Process <see cref="IElClipboardConsumer.ClipboardAbilitiesChanged"/> event.
            /// 
            /// In case sender is the active DockContent, update the clipboard buttons accordingly.
            /// </summary>
            /// <param name="sender">The <see cref="IElClipboardConsumer"/> that has changed its <see cref="clipboardAbilities"/>.</param>
            /// <param name="e">The <see cref="ClipboardAbilitiesChangedEventArgs"/>.</param>
            public void ClipboardConsumer_ClipboardAbilitiesChanged(object sender, ClipboardAbilitiesChangedEventArgs e)
            {
                Debug.Assert(sender is IElClipboardConsumer, "sender is not of type IElClipboardConsumer");

                if (sender.Equals(this.ApplicationWindow.ActiveDockContent))
                    this.ClipboardAbilities = e.NewClipboardAbilities;
            }
        }
    }
}
