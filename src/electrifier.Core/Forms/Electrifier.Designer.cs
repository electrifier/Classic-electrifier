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
            this.mniFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mniFileExit = new System.Windows.Forms.ToolStripMenuItem();
            this.mniEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.mniView = new System.Windows.Forms.ToolStripMenuItem();
            this.mniFavorites = new System.Windows.Forms.ToolStripMenuItem();
            this.addFolderToFavoritesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mniTools = new System.Windows.Forms.ToolStripMenuItem();
            this.mniWindow = new System.Windows.Forms.ToolStripMenuItem();
            this.mniHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.visitElectrifierorgToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.machineInformationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.sendBugReportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mniHelpAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.tscToolStripContainer = new System.Windows.Forms.ToolStripContainer();
            this.dpnDockPanel = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this.tstFileToolStrip = new System.Windows.Forms.ToolStrip();
            this.tsbNewFileBrowser = new System.Windows.Forms.ToolStripSplitButton();
            this.tsbNewFileBrowserLeft = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbNewFileBrowserRight = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbNewFileBrowserTop = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbNewFileBrowserBottom = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbNewFileBrowserFloating = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuMenuStrip.SuspendLayout();
            this.tscToolStripContainer.ContentPanel.SuspendLayout();
            this.tscToolStripContainer.TopToolStripPanel.SuspendLayout();
            this.tscToolStripContainer.SuspendLayout();
            this.tstFileToolStrip.SuspendLayout();
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
            // mnuMenuStrip
            // 
            this.mnuMenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.mnuMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mniFile,
            this.mniEdit,
            this.mniView,
            this.mniFavorites,
            this.mniTools,
            this.mniWindow,
            this.mniHelp});
            this.mnuMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.mnuMenuStrip.Name = "mnuMenuStrip";
            this.mnuMenuStrip.Size = new System.Drawing.Size(782, 28);
            this.mnuMenuStrip.TabIndex = 1;
            this.mnuMenuStrip.Text = "menuStrip1";
            // 
            // mniFile
            // 
            this.mniFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mniFileExit});
            this.mniFile.Name = "mniFile";
            this.mniFile.Size = new System.Drawing.Size(44, 24);
            this.mniFile.Text = "&File";
            // 
            // mniFileExit
            // 
            this.mniFileExit.Image = ((System.Drawing.Image)(resources.GetObject("mniFileExit.Image")));
            this.mniFileExit.Name = "mniFileExit";
            this.mniFileExit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.mniFileExit.Size = new System.Drawing.Size(161, 26);
            this.mniFileExit.Text = "&Exit";
            this.mniFileExit.Click += new System.EventHandler(this.mniFileExit_Click);
            // 
            // mniEdit
            // 
            this.mniEdit.Enabled = false;
            this.mniEdit.Name = "mniEdit";
            this.mniEdit.Size = new System.Drawing.Size(47, 24);
            this.mniEdit.Text = "&Edit";
            // 
            // mniView
            // 
            this.mniView.Enabled = false;
            this.mniView.Name = "mniView";
            this.mniView.Size = new System.Drawing.Size(53, 24);
            this.mniView.Text = "&View";
            // 
            // mniFavorites
            // 
            this.mniFavorites.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addFolderToFavoritesToolStripMenuItem});
            this.mniFavorites.Name = "mniFavorites";
            this.mniFavorites.Size = new System.Drawing.Size(79, 24);
            this.mniFavorites.Text = "&Favorites";
            // 
            // addFolderToFavoritesToolStripMenuItem
            // 
            this.addFolderToFavoritesToolStripMenuItem.Name = "addFolderToFavoritesToolStripMenuItem";
            this.addFolderToFavoritesToolStripMenuItem.Size = new System.Drawing.Size(238, 26);
            this.addFolderToFavoritesToolStripMenuItem.Text = "&Add Folder to Favorites";
            // 
            // mniTools
            // 
            this.mniTools.Enabled = false;
            this.mniTools.Name = "mniTools";
            this.mniTools.Size = new System.Drawing.Size(56, 24);
            this.mniTools.Text = "&Tools";
            // 
            // mniWindow
            // 
            this.mniWindow.Enabled = false;
            this.mniWindow.Name = "mniWindow";
            this.mniWindow.Size = new System.Drawing.Size(76, 24);
            this.mniWindow.Text = "&Window";
            // 
            // mniHelp
            // 
            this.mniHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.visitElectrifierorgToolStripMenuItem,
            this.toolStripSeparator4,
            this.machineInformationToolStripMenuItem,
            this.toolStripSeparator5,
            this.sendBugReportToolStripMenuItem,
            this.mniHelpAbout});
            this.mniHelp.Name = "mniHelp";
            this.mniHelp.Size = new System.Drawing.Size(53, 24);
            this.mniHelp.Text = "&Help";
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
            this.mniHelpAbout.Click += new System.EventHandler(this.mniHelpAbout_Click);
            // 
            // tscToolStripContainer
            // 
            // 
            // tscToolStripContainer.ContentPanel
            // 
            this.tscToolStripContainer.ContentPanel.Controls.Add(this.dpnDockPanel);
            this.tscToolStripContainer.ContentPanel.Size = new System.Drawing.Size(782, 444);
            this.tscToolStripContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tscToolStripContainer.Location = new System.Drawing.Point(0, 28);
            this.tscToolStripContainer.Name = "tscToolStripContainer";
            this.tscToolStripContainer.Size = new System.Drawing.Size(782, 483);
            this.tscToolStripContainer.TabIndex = 2;
            this.tscToolStripContainer.Text = "toolStripContainer1";
            // 
            // tscToolStripContainer.TopToolStripPanel
            // 
            this.tscToolStripContainer.TopToolStripPanel.Controls.Add(this.tstFileToolStrip);
            // 
            // dpnDockPanel
            // 
            this.dpnDockPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dpnDockPanel.DocumentStyle = WeifenLuo.WinFormsUI.Docking.DocumentStyle.DockingWindow;
            this.dpnDockPanel.Location = new System.Drawing.Point(0, 0);
            this.dpnDockPanel.Name = "dpnDockPanel";
            this.dpnDockPanel.Size = new System.Drawing.Size(782, 444);
            this.dpnDockPanel.TabIndex = 0;
            // 
            // tstFileToolStrip
            // 
            this.tstFileToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.tstFileToolStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.tstFileToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbNewFileBrowser});
            this.tstFileToolStrip.Location = new System.Drawing.Point(3, 0);
            this.tstFileToolStrip.Name = "tstFileToolStrip";
            this.tstFileToolStrip.Size = new System.Drawing.Size(225, 39);
            this.tstFileToolStrip.TabIndex = 0;
            // 
            // tsbNewFileBrowser
            // 
            this.tsbNewFileBrowser.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbNewFileBrowserLeft,
            this.tsbNewFileBrowserRight,
            this.toolStripSeparator6,
            this.tsbNewFileBrowserTop,
            this.tsbNewFileBrowserBottom,
            this.toolStripSeparator7,
            this.tsbNewFileBrowserFloating});
            this.tsbNewFileBrowser.Image = ((System.Drawing.Image)(resources.GetObject("tsbNewFileBrowser.Image")));
            this.tsbNewFileBrowser.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbNewFileBrowser.Name = "tsbNewFileBrowser";
            this.tsbNewFileBrowser.Size = new System.Drawing.Size(174, 36);
            this.tsbNewFileBrowser.Text = "&New File Browser";
            this.tsbNewFileBrowser.ButtonClick += new System.EventHandler(this.tsbNewFileBrowser_ButtonClick);
            // 
            // tsbNewFileBrowserLeft
            // 
            this.tsbNewFileBrowserLeft.Name = "tsbNewFileBrowserLeft";
            this.tsbNewFileBrowserLeft.Size = new System.Drawing.Size(216, 26);
            this.tsbNewFileBrowserLeft.Text = "Dock &Left-Sieded";
            this.tsbNewFileBrowserLeft.Click += new System.EventHandler(this.tsbNewFileBrowserLeft_Click);
            // 
            // tsbNewFileBrowserRight
            // 
            this.tsbNewFileBrowserRight.Name = "tsbNewFileBrowserRight";
            this.tsbNewFileBrowserRight.Size = new System.Drawing.Size(216, 26);
            this.tsbNewFileBrowserRight.Text = "Dock &Right-Sided";
            this.tsbNewFileBrowserRight.Click += new System.EventHandler(this.tsbNewFileBrowserRight_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(213, 6);
            // 
            // tsbNewFileBrowserTop
            // 
            this.tsbNewFileBrowserTop.Name = "tsbNewFileBrowserTop";
            this.tsbNewFileBrowserTop.Size = new System.Drawing.Size(216, 26);
            this.tsbNewFileBrowserTop.Text = "Dock to &Top";
            this.tsbNewFileBrowserTop.Click += new System.EventHandler(this.tsbNewFileBrowserTop_Click);
            // 
            // tsbNewFileBrowserBottom
            // 
            this.tsbNewFileBrowserBottom.Name = "tsbNewFileBrowserBottom";
            this.tsbNewFileBrowserBottom.Size = new System.Drawing.Size(216, 26);
            this.tsbNewFileBrowserBottom.Text = "Dock to &Bottom";
            this.tsbNewFileBrowserBottom.Click += new System.EventHandler(this.tsbNewFileBrowserBottom_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(213, 6);
            // 
            // tsbNewFileBrowserFloating
            // 
            this.tsbNewFileBrowserFloating.Name = "tsbNewFileBrowserFloating";
            this.tsbNewFileBrowserFloating.Size = new System.Drawing.Size(216, 26);
            this.tsbNewFileBrowserFloating.Text = "&Floating";
            this.tsbNewFileBrowserFloating.Click += new System.EventHandler(this.tsbNewFileBrowserFloating_Click);
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
        private System.Windows.Forms.ToolStripMenuItem mniFile;
        private System.Windows.Forms.ToolStripContainer tscToolStripContainer;
        private System.Windows.Forms.ToolStrip tstFileToolStrip;
        private System.Windows.Forms.ToolStripMenuItem mniEdit;
        private System.Windows.Forms.ToolStripMenuItem mniView;
        private System.Windows.Forms.ToolStripMenuItem mniFavorites;
        private System.Windows.Forms.ToolStripMenuItem mniHelp;
        private System.Windows.Forms.ToolStripMenuItem mniTools;
        private System.Windows.Forms.ToolStripMenuItem mniHelpAbout;
        private System.Windows.Forms.ToolStripMenuItem mniWindow;
        private System.Windows.Forms.ToolStripMenuItem mniFileExit;
        private System.Windows.Forms.ToolStripMenuItem visitElectrifierorgToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem machineInformationToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem sendBugReportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addFolderToFavoritesToolStripMenuItem;
        private WeifenLuo.WinFormsUI.Docking.DockPanel dpnDockPanel;
        private System.Windows.Forms.ToolStripSplitButton tsbNewFileBrowser;
        private System.Windows.Forms.ToolStripMenuItem tsbNewFileBrowserLeft;
        private System.Windows.Forms.ToolStripMenuItem tsbNewFileBrowserRight;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem tsbNewFileBrowserTop;
        private System.Windows.Forms.ToolStripMenuItem tsbNewFileBrowserBottom;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripMenuItem tsbNewFileBrowserFloating;
    }
}