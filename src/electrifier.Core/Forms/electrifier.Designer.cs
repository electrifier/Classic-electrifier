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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Electrifier));
            this.stsStatusStrip = new System.Windows.Forms.StatusStrip();
            this.mnuMenuStrip = new System.Windows.Forms.MenuStrip();
            this.mnuFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mniFileExit = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.favoritesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addFolderToFavoritesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sessionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scriptsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.visitElectrifierorgToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.machineInformationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.sendBugReportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mniHelpAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.tscToolStripContainer = new System.Windows.Forms.ToolStripContainer();
            this.tstFileToolStrip = new System.Windows.Forms.ToolStrip();
            this.tdbNewFileBrowser = new System.Windows.Forms.ToolStripDropDownButton();
            this.tmiNewPanelFloating = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tmiDockToLeft = new System.Windows.Forms.ToolStripMenuItem();
            this.tmiDockToRight = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tmiDockToTop = new System.Windows.Forms.ToolStripMenuItem();
            this.tmiDockToBottom = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.dpnDockPanel = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this.mnuMenuStrip.SuspendLayout();
            this.tscToolStripContainer.ContentPanel.SuspendLayout();
            this.tscToolStripContainer.TopToolStripPanel.SuspendLayout();
            this.tscToolStripContainer.SuspendLayout();
            this.tstFileToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // stsStatusStrip
            // 
            this.stsStatusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.stsStatusStrip.Location = new System.Drawing.Point(0, 531);
            this.stsStatusStrip.Name = "stsStatusStrip";
            this.stsStatusStrip.Size = new System.Drawing.Size(782, 22);
            this.stsStatusStrip.TabIndex = 0;
            this.stsStatusStrip.Text = "statusStrip1";
            // 
            // mnuMenuStrip
            // 
            this.mnuMenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.mnuMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.favoritesToolStripMenuItem,
            this.sessionToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.scriptsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.mnuMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.mnuMenuStrip.Name = "mnuMenuStrip";
            this.mnuMenuStrip.Size = new System.Drawing.Size(782, 28);
            this.mnuMenuStrip.TabIndex = 1;
            this.mnuMenuStrip.Text = "menuStrip1";
            // 
            // mnuFileToolStripMenuItem
            // 
            this.mnuFileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mniFileExit});
            this.mnuFileToolStripMenuItem.Name = "mnuFileToolStripMenuItem";
            this.mnuFileToolStripMenuItem.Size = new System.Drawing.Size(44, 24);
            this.mnuFileToolStripMenuItem.Text = "&File";
            // 
            // mniFileExit
            // 
            this.mniFileExit.Image = ((System.Drawing.Image)(resources.GetObject("mniFileExit.Image")));
            this.mniFileExit.Name = "mniFileExit";
            this.mniFileExit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.mniFileExit.Size = new System.Drawing.Size(216, 26);
            this.mniFileExit.Text = "&Exit";
            this.mniFileExit.Click += new System.EventHandler(this.mniFileExit_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Enabled = false;
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(47, 24);
            this.editToolStripMenuItem.Text = "&Edit";
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.Enabled = false;
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(53, 24);
            this.viewToolStripMenuItem.Text = "&View";
            // 
            // favoritesToolStripMenuItem
            // 
            this.favoritesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addFolderToFavoritesToolStripMenuItem});
            this.favoritesToolStripMenuItem.Name = "favoritesToolStripMenuItem";
            this.favoritesToolStripMenuItem.Size = new System.Drawing.Size(79, 24);
            this.favoritesToolStripMenuItem.Text = "&Favorites";
            // 
            // addFolderToFavoritesToolStripMenuItem
            // 
            this.addFolderToFavoritesToolStripMenuItem.Name = "addFolderToFavoritesToolStripMenuItem";
            this.addFolderToFavoritesToolStripMenuItem.Size = new System.Drawing.Size(238, 26);
            this.addFolderToFavoritesToolStripMenuItem.Text = "&Add Folder to Favorites";
            // 
            // sessionToolStripMenuItem
            // 
            this.sessionToolStripMenuItem.Enabled = false;
            this.sessionToolStripMenuItem.Name = "sessionToolStripMenuItem";
            this.sessionToolStripMenuItem.Size = new System.Drawing.Size(70, 24);
            this.sessionToolStripMenuItem.Text = "&Session";
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.Enabled = false;
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(56, 24);
            this.toolsToolStripMenuItem.Text = "&Tools";
            // 
            // scriptsToolStripMenuItem
            // 
            this.scriptsToolStripMenuItem.Enabled = false;
            this.scriptsToolStripMenuItem.Name = "scriptsToolStripMenuItem";
            this.scriptsToolStripMenuItem.Size = new System.Drawing.Size(65, 24);
            this.scriptsToolStripMenuItem.Text = "&Scripts";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.visitElectrifierorgToolStripMenuItem,
            this.toolStripSeparator4,
            this.machineInformationToolStripMenuItem,
            this.toolStripSeparator5,
            this.sendBugReportToolStripMenuItem,
            this.mniHelpAbout});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(53, 24);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // visitElectrifierorgToolStripMenuItem
            // 
            this.visitElectrifierorgToolStripMenuItem.Enabled = false;
            this.visitElectrifierorgToolStripMenuItem.Name = "visitElectrifierorgToolStripMenuItem";
            this.visitElectrifierorgToolStripMenuItem.Size = new System.Drawing.Size(222, 26);
            this.visitElectrifierorgToolStripMenuItem.Text = "&Visit electrifier.org";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(219, 6);
            // 
            // machineInformationToolStripMenuItem
            // 
            this.machineInformationToolStripMenuItem.Enabled = false;
            this.machineInformationToolStripMenuItem.Name = "machineInformationToolStripMenuItem";
            this.machineInformationToolStripMenuItem.Size = new System.Drawing.Size(222, 26);
            this.machineInformationToolStripMenuItem.Text = "&Machine Information";
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(219, 6);
            // 
            // sendBugReportToolStripMenuItem
            // 
            this.sendBugReportToolStripMenuItem.Enabled = false;
            this.sendBugReportToolStripMenuItem.Name = "sendBugReportToolStripMenuItem";
            this.sendBugReportToolStripMenuItem.Size = new System.Drawing.Size(222, 26);
            this.sendBugReportToolStripMenuItem.Text = "&Send Bug Report";
            // 
            // mniHelpAbout
            // 
            this.mniHelpAbout.Name = "mniHelpAbout";
            this.mniHelpAbout.Size = new System.Drawing.Size(222, 26);
            this.mniHelpAbout.Text = "&About...";
            // 
            // tscToolStripContainer
            // 
            // 
            // tscToolStripContainer.ContentPanel
            // 
            this.tscToolStripContainer.ContentPanel.Controls.Add(this.dpnDockPanel);
            this.tscToolStripContainer.ContentPanel.Size = new System.Drawing.Size(782, 464);
            this.tscToolStripContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tscToolStripContainer.Location = new System.Drawing.Point(0, 28);
            this.tscToolStripContainer.Name = "tscToolStripContainer";
            this.tscToolStripContainer.Size = new System.Drawing.Size(782, 503);
            this.tscToolStripContainer.TabIndex = 2;
            this.tscToolStripContainer.Text = "toolStripContainer1";
            // 
            // tscToolStripContainer.TopToolStripPanel
            // 
            this.tscToolStripContainer.TopToolStripPanel.Controls.Add(this.tstFileToolStrip);
            // 
            // tstFileToolStrip
            // 
            this.tstFileToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.tstFileToolStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.tstFileToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tdbNewFileBrowser,
            this.toolStripSeparator3});
            this.tstFileToolStrip.Location = new System.Drawing.Point(3, 0);
            this.tstFileToolStrip.Name = "tstFileToolStrip";
            this.tstFileToolStrip.Size = new System.Drawing.Size(187, 39);
            this.tstFileToolStrip.TabIndex = 0;
            // 
            // tdbNewFileBrowser
            // 
            this.tdbNewFileBrowser.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tmiNewPanelFloating,
            this.toolStripSeparator1,
            this.tmiDockToLeft,
            this.tmiDockToRight,
            this.toolStripSeparator2,
            this.tmiDockToTop,
            this.tmiDockToBottom});
            this.tdbNewFileBrowser.Image = ((System.Drawing.Image)(resources.GetObject("tdbNewFileBrowser.Image")));
            this.tdbNewFileBrowser.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tdbNewFileBrowser.Name = "tdbNewFileBrowser";
            this.tdbNewFileBrowser.Size = new System.Drawing.Size(169, 36);
            this.tdbNewFileBrowser.Text = "&New File Browser";
            this.tdbNewFileBrowser.ToolTipText = "Open &new file browser";
            // 
            // tmiNewPanelFloating
            // 
            this.tmiNewPanelFloating.Name = "tmiNewPanelFloating";
            this.tmiNewPanelFloating.Size = new System.Drawing.Size(216, 26);
            this.tmiNewPanelFloating.Text = "&Floating";
            this.tmiNewPanelFloating.Click += new System.EventHandler(this.tmiNewPanelFloating_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(213, 6);
            // 
            // tmiDockToLeft
            // 
            this.tmiDockToLeft.Name = "tmiDockToLeft";
            this.tmiDockToLeft.Size = new System.Drawing.Size(216, 26);
            this.tmiDockToLeft.Text = "Dock &Left-sided";
            // 
            // tmiDockToRight
            // 
            this.tmiDockToRight.Name = "tmiDockToRight";
            this.tmiDockToRight.Size = new System.Drawing.Size(216, 26);
            this.tmiDockToRight.Text = "Dock &Right-Sided";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(213, 6);
            // 
            // tmiDockToTop
            // 
            this.tmiDockToTop.Name = "tmiDockToTop";
            this.tmiDockToTop.Size = new System.Drawing.Size(216, 26);
            this.tmiDockToTop.Text = "Dock to &Top";
            // 
            // tmiDockToBottom
            // 
            this.tmiDockToBottom.Name = "tmiDockToBottom";
            this.tmiDockToBottom.Size = new System.Drawing.Size(216, 26);
            this.tmiDockToBottom.Text = "Dock to &Bottom";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 39);
            // 
            // dpnDockPanel
            // 
            this.dpnDockPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dpnDockPanel.DocumentStyle = WeifenLuo.WinFormsUI.Docking.DocumentStyle.DockingWindow;
            this.dpnDockPanel.Location = new System.Drawing.Point(0, 0);
            this.dpnDockPanel.Name = "dpnDockPanel";
            this.dpnDockPanel.Size = new System.Drawing.Size(782, 464);
            this.dpnDockPanel.TabIndex = 0;
            // 
            // Electrifier
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(782, 553);
            this.Controls.Add(this.tscToolStripContainer);
            this.Controls.Add(this.stsStatusStrip);
            this.Controls.Add(this.mnuMenuStrip);
            this.MainMenuStrip = this.mnuMenuStrip;
            this.Name = "Electrifier";
            this.LocationChanged += new System.EventHandler(this.electrifier_LocationChanged);
            this.Resize += new System.EventHandler(this.electrifier_Resize);
            this.mnuMenuStrip.ResumeLayout(false);
            this.mnuMenuStrip.PerformLayout();
            this.tscToolStripContainer.ContentPanel.ResumeLayout(false);
            this.tscToolStripContainer.TopToolStripPanel.ResumeLayout(false);
            this.tscToolStripContainer.TopToolStripPanel.PerformLayout();
            this.tscToolStripContainer.ResumeLayout(false);
            this.tscToolStripContainer.PerformLayout();
            this.tstFileToolStrip.ResumeLayout(false);
            this.tstFileToolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip stsStatusStrip;
        private System.Windows.Forms.MenuStrip mnuMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem mnuFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripContainer tscToolStripContainer;
        private System.Windows.Forms.ToolStrip tstFileToolStrip;
        private System.Windows.Forms.ToolStripDropDownButton tdbNewFileBrowser;
        private System.Windows.Forms.ToolStripMenuItem tmiNewPanelFloating;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem tmiDockToLeft;
        private System.Windows.Forms.ToolStripMenuItem tmiDockToRight;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem tmiDockToTop;
        private System.Windows.Forms.ToolStripMenuItem tmiDockToBottom;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem favoritesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sessionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mniHelpAbout;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem scriptsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mniFileExit;
        private System.Windows.Forms.ToolStripMenuItem visitElectrifierorgToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem machineInformationToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem sendBugReportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addFolderToFavoritesToolStripMenuItem;
        private WeifenLuo.WinFormsUI.Docking.DockPanel dpnDockPanel;
    }
}