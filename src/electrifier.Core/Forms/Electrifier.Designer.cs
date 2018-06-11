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
using Sunburst.WindowsForms.Ribbon.Controls;
using Sunburst.WindowsForms.Ribbon.Controls.Events;

namespace electrifier.Core.Forms
{
    partial class Electrifier
    {
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.stsStatusStrip = new System.Windows.Forms.StatusStrip();
            this.rbnRibbon = new Sunburst.WindowsForms.Ribbon.Ribbon();
            this.dpnDockPanel = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this.fspFormStatePersistor = new electrifier.Core.Components.FormStatePersistor(this.components);
            this.SuspendLayout();
            // 
            // stsStatusStrip
            // 
            this.stsStatusStrip.AutoSize = false;
            this.stsStatusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.stsStatusStrip.Location = new System.Drawing.Point(0, 511);
            this.stsStatusStrip.Name = "stsStatusStrip";
            this.stsStatusStrip.Size = new System.Drawing.Size(782, 42);
            this.stsStatusStrip.TabIndex = 0;
            // 
            // rbnRibbon
            // 
            this.rbnRibbon.Location = new System.Drawing.Point(0, 0);
            this.rbnRibbon.Minimized = false;
            this.rbnRibbon.Name = "rbnRibbon";
            this.rbnRibbon.ResourceName = "Electrifier.Ribbon";
            this.rbnRibbon.ShortcutTableResourceName = null;
            this.rbnRibbon.Size = new System.Drawing.Size(782, 122);
            this.rbnRibbon.TabIndex = 3;
            // 
            // dpnDockPanel
            // 
            this.dpnDockPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dpnDockPanel.DocumentStyle = WeifenLuo.WinFormsUI.Docking.DocumentStyle.DockingWindow;
            this.dpnDockPanel.Location = new System.Drawing.Point(0, 122);
            this.dpnDockPanel.Name = "dpnDockPanel";
            this.dpnDockPanel.Size = new System.Drawing.Size(782, 389);
            this.dpnDockPanel.TabIndex = 4;
            // 
            // fspFormStatePersistor
            // 
            this.fspFormStatePersistor.ClientForm = this;
            this.fspFormStatePersistor.PropertyKeyPrefix = "Electrifier";
            // 
            // Electrifier
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(782, 553);
            this.Controls.Add(this.dpnDockPanel);
            this.Controls.Add(this.rbnRibbon);
            this.Controls.Add(this.stsStatusStrip);
            this.Name = "Electrifier";
            this.ResumeLayout(false);

        }

        #endregion

        #region Ribbon members ================================================================================================

        private void InitializeRibbon()
        {
            //
            // Quick Access Toolbar Commands ==================================================================================
            //
            this.cmdQATOpenNewShellBrowserPanel = new RibbonButton(this.rbnRibbon, (uint)rbnCommand.cmdQATOpenNewShellBrowserPanel);
            this.cmdQATOpenNewShellBrowserPanel.ExecuteEvent += new System.EventHandler<Sunburst.WindowsForms.Ribbon.Controls.Events.ExecuteEventArgs>(this.CmdAppOpenNewShellBrowserPanel_Execute);

            //
            // Application Menu Items =========================================================================================
            //
            this.cmdAppOpenNewWindow = new RibbonButton(this.rbnRibbon, (uint)rbnCommand.cmdAppOpenNewWindow);
            this.cmdAppOpenNewWindow.Enabled = false;
            //this.cmdAppOpenNewShellBrowserPanel.ExecuteEvent += new System.EventHandler<Sunburst.WindowsForms.Ribbon.Controls.Events.ExecuteEventArgs>(this.CmdAppOpenNewWindow_Execute);

            this.cmdAppOpenNewShellBrowserPanel = new RibbonButton(this.rbnRibbon, (uint)rbnCommand.cmdAppOpenNewShellBrowserPanel);
            this.cmdAppOpenNewShellBrowserPanel.ExecuteEvent += new System.EventHandler<Sunburst.WindowsForms.Ribbon.Controls.Events.ExecuteEventArgs>(this.CmdAppOpenNewShellBrowserPanel_Execute);

            this.cmdAppOpenCommandPrompt = new RibbonButton(this.rbnRibbon, (uint)rbnCommand.cmdAppOpenCommandPrompt);
            this.cmdAppOpenCommandPrompt.Enabled = false;
            //this.cmdAppOpenCommandPrompt.ExecuteEvent += new System.EventHandler<Sunburst.WindowsForms.Ribbon.Controls.Events.ExecuteEventArgs>(this.CmdAppOpenCommandPrompt_Execute);

            this.cmdAppOpenWindowsPowerShell = new RibbonButton(this.rbnRibbon, (uint)rbnCommand.cmdAppOpenWindowsPowerShell);
            this.cmdAppOpenWindowsPowerShell.Enabled = false;
            //this.cmdAppOpenWindowsPowerShell.ExecuteEvent += new System.EventHandler<Sunburst.WindowsForms.Ribbon.Controls.Events.ExecuteEventArgs>(this.CmdAppOpenWindowsPowerShell_Execute);

            this.cmdAppChangeElectrifierOptions = new RibbonButton(this.rbnRibbon, (uint)rbnCommand.cmdAppChangeElectrifierOptions);
            this.cmdAppChangeElectrifierOptions.Enabled = false;
            //this.cmdAppChangeElectrifierOptions.ExecuteEvent += new System.EventHandler<Sunburst.WindowsForms.Ribbon.Controls.Events.ExecuteEventArgs>(this.CmdAppChangeElectrifierOptions_Execute);

            this.cmdAppChangeFolderAndSearchOptions = new RibbonButton(this.rbnRibbon, (uint)rbnCommand.cmdAppChangeFolderAndSearchOptions);
            this.cmdAppChangeFolderAndSearchOptions.Enabled = false;
            //this.cmdAppChangeFolderAndSearchOptions.ExecuteEvent += new System.EventHandler<Sunburst.WindowsForms.Ribbon.Controls.Events.ExecuteEventArgs>(this.CmdAppChangeFolderAndSearchOptions_Execute);

            this.cmdAppHelp = new RibbonButton(this.rbnRibbon, (uint)rbnCommand.cmdAppHelp);
            //this.cmdAppHelp.ExecuteEvent += new System.EventHandler<Sunburst.WindowsForms.Ribbon.Controls.Events.ExecuteEventArgs>(this.CmdAppHelp_Execute);

            this.cmdAppHelpAboutElectrifier = new RibbonButton(this.rbnRibbon, (uint)rbnCommand.cmdAppHelpAboutElectrifier);
            this.cmdAppHelpAboutElectrifier.ExecuteEvent += new System.EventHandler<Sunburst.WindowsForms.Ribbon.Controls.Events.ExecuteEventArgs>(this.CmdAppHelpAboutElectrifier_Execute);

            this.cmdAppHelpAboutWindows = new RibbonButton(this.rbnRibbon, (uint)rbnCommand.cmdAppHelpAboutWindows);
            this.cmdAppHelpAboutWindows.ExecuteEvent += new System.EventHandler<Sunburst.WindowsForms.Ribbon.Controls.Events.ExecuteEventArgs>(this.CmdAppHelpAboutWindows_Execute);

            this.cmdAppClose = new RibbonButton(this.rbnRibbon, (uint)rbnCommand.cmdAppClose);
            this.cmdAppClose.ExecuteEvent += new EventHandler<Sunburst.WindowsForms.Ribbon.Controls.Events.ExecuteEventArgs>(this.CmdAppClose_Execute);

            this.cmdTabHome = new RibbonTab(this.rbnRibbon, (uint)rbnCommand.cmdTabHome);
            //
            // Command Group: Home -> Clipboard ===============================================================================
            //
            this.cmdGrpHomeClipboard = new RibbonButton(this.rbnRibbon, (uint)rbnCommand.cmdGrpHomeClipboard);
            this.cmdGrpHomeClipboard.Enabled = false;
            //this.cmdGrpHomeClipboard.ExecuteEvent += new System.EventHandler<Sunburst.WindowsForms.Ribbon.Controls.Events.ExecuteEventArgs>(this.CmdGrpHomeClipboard_Execute);

            this.cmdBtnClipboardCut = new RibbonButton(this.rbnRibbon, (uint)rbnCommand.cmdBtnClipboardCut);
            this.cmdBtnClipboardCut.Enabled = false;
            //this.cmdBtnClipboardCut.ExecuteEvent += new System.EventHandler<Sunburst.WindowsForms.Ribbon.Controls.Events.ExecuteEventArgs>(this.CmdBtnClipboardCut_Execute);

            this.cmdBtnClipboardCopy = new RibbonButton(this.rbnRibbon, (uint)rbnCommand.cmdBtnClipboardCopy);
            this.cmdBtnClipboardCopy.Enabled = false;
            //this.cmdBtnClipboardCopy.ExecuteEvent += new System.EventHandler<Sunburst.WindowsForms.Ribbon.Controls.Events.ExecuteEventArgs>(this.CmdBtnClipboardCopy_Execute);

            this.cmdBtnClipboardPaste = new RibbonButton(this.rbnRibbon, (uint)rbnCommand.cmdBtnClipboardPaste);
            this.cmdBtnClipboardPaste.Enabled = false;
            //this.cmdBtnClipboardPaste.ExecuteEvent += new System.EventHandler<Sunburst.WindowsForms.Ribbon.Controls.Events.ExecuteEventArgs>(this.CmdBtnClipboardPaste_Execute);
            //
            // Command Group: Home -> Organize ================================================================================
            //
            this.cmdGrpHomeOrganize = new RibbonButton(this.rbnRibbon, (uint)rbnCommand.cmdGrpHomeOrganize);
            this.cmdGrpHomeOrganize.Enabled = false;
            //this.cmdGrpHomeOrganize.ExecuteEvent += new System.EventHandler<Sunburst.WindowsForms.Ribbon.Controls.Events.ExecuteEventArgs>(this.CmdGrpHomeOrganize_Execute);

            this.cmdBtnOrganizeMoveTo = new RibbonButton(this.rbnRibbon, (uint)rbnCommand.cmdBtnOrganizeMoveTo);
            this.cmdBtnOrganizeMoveTo.Enabled = false;
            //this.cmdBtnOrganizeMoveTo.ExecuteEvent += new System.EventHandler<Sunburst.WindowsForms.Ribbon.Controls.Events.ExecuteEventArgs>(this.CmdBtnOrganizeMoveTo_Execute);

            this.cmdBtnOrganizeDelete = new RibbonButton(this.rbnRibbon, (uint)rbnCommand.cmdBtnOrganizeDelete);
            this.cmdBtnOrganizeDelete.Enabled = false;
            //this.cmdBtnOrganizeDelete.ExecuteEvent += new System.EventHandler<Sunburst.WindowsForms.Ribbon.Controls.Events.ExecuteEventArgs>(this.CmdBtnOrganizeDelete_Execute);

            this.cmdBtnOrganizeRename = new RibbonButton(this.rbnRibbon, (uint)rbnCommand.cmdBtnOrganizeRename);
            this.cmdBtnOrganizeRename.Enabled = false;
            //this.cmdBtnOrganizeRename.ExecuteEvent += new System.EventHandler<Sunburst.WindowsForms.Ribbon.Controls.Events.ExecuteEventArgs>(this.CmdBtnOrganizeRename_Execute);



        }

        private enum rbnCommand : uint
        {
            //
            // Quick Access Toolbar Commands ==================================================================================
            //
            cmdQATOpenNewShellBrowserPanel = 19903,

            //
            // Application Menu Items =========================================================================================
            //
            cmdAppOpenNewWindow = 100,
            cmdAppOpenNewShellBrowserPanel = 101,
            cmdAppOpenCommandPrompt = 102,
            cmdAppOpenWindowsPowerShell = 103,
            cmdAppChangeElectrifierOptions = 110,
            cmdAppChangeFolderAndSearchOptions = 111,
            cmdAppHelp = 120,
            cmdAppHelpAboutElectrifier = 121,
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


        private RibbonButton cmdQATOpenNewShellBrowserPanel;


        private RibbonTab cmdTabHome;


        private RibbonButton cmdAppOpenNewWindow;
        private RibbonButton cmdAppOpenNewShellBrowserPanel;
        private RibbonButton cmdAppOpenCommandPrompt;
        private RibbonButton cmdAppOpenWindowsPowerShell;
        private RibbonButton cmdAppChangeElectrifierOptions;
        private RibbonButton cmdAppChangeFolderAndSearchOptions;
        private RibbonButton cmdAppHelp;
        private RibbonButton cmdAppHelpAboutElectrifier;
        private RibbonButton cmdAppHelpAboutWindows;
        private RibbonButton cmdAppClose;
        private RibbonButton cmdGrpHomeClipboard;
        private RibbonButton cmdBtnClipboardCut;
        private RibbonButton cmdBtnClipboardCopy;
        private RibbonButton cmdBtnClipboardPaste;
        private RibbonButton cmdGrpHomeOrganize;
        private RibbonButton cmdBtnOrganizeMoveTo;
        private RibbonButton cmdBtnOrganizeDelete;
        private RibbonButton cmdBtnOrganizeRename;




        #endregion

        private System.Windows.Forms.StatusStrip stsStatusStrip;
        private Sunburst.WindowsForms.Ribbon.Ribbon rbnRibbon;
        private WeifenLuo.WinFormsUI.Docking.DockPanel dpnDockPanel;
        private Components.FormStatePersistor fspFormStatePersistor;
    }
}