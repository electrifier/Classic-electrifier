using System;
using Sunburst.WindowsForms.Ribbon.Controls;

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
            this.stsStatusStrip = new System.Windows.Forms.StatusStrip();
            this.rbnRibbon = new Sunburst.WindowsForms.Ribbon.Ribbon();
            this.dpnDockPanel = new WeifenLuo.WinFormsUI.Docking.DockPanel();
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
            // Electrifier
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(782, 553);
            this.Controls.Add(this.dpnDockPanel);
            this.Controls.Add(this.rbnRibbon);
            this.Controls.Add(this.stsStatusStrip);
            this.Name = "Electrifier";
            this.LocationChanged += new System.EventHandler(this.electrifier_LocationChanged);
            this.Resize += new System.EventHandler(this.electrifier_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        #region Ribbon members ================================================================================================

        private void InitializeRibbon()
        {
            //
            // Application Menu Items =========================================================================================
            //
            this.cmdAppOpenNewShellBrowserPanel = new RibbonButton(this.rbnRibbon, (uint)rbnCommand.cmdAppOpenNewShellBrowserPanel);
            this.cmdAppOpenNewShellBrowserPanel.ExecuteEvent += new System.EventHandler<Sunburst.WindowsForms.Ribbon.Controls.Events.ExecuteEventArgs>(this.CmdAppOpenNewShellBrowserPanel_Execute);

            this.cmdAppClose = new RibbonButton(this.rbnRibbon, (uint)rbnCommand.cmdAppClose);
            this.cmdAppClose.ExecuteEvent += new EventHandler<Sunburst.WindowsForms.Ribbon.Controls.Events.ExecuteEventArgs>(this.CmdAppClose_Execute);

            this.cmdTabHome = new RibbonTab(this.rbnRibbon, (uint)rbnCommand.cmdTabHome);
        }

        private enum rbnCommand : uint
        {
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


        private RibbonButton cmdAppOpenNewShellBrowserPanel;
        private RibbonButton cmdAppClose;
        private RibbonTab cmdTabHome;

        #endregion

        private System.Windows.Forms.StatusStrip stsStatusStrip;
        private Sunburst.WindowsForms.Ribbon.Ribbon rbnRibbon;
        private WeifenLuo.WinFormsUI.Docking.DockPanel dpnDockPanel;

    }
}