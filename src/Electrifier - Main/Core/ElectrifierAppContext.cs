using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Electrifier.Core {
	/// <summary>
	/// Zusammenfassung für ElectrifierAppContext.
	/// </summary>
	public class ElectrifierAppContext : System.Windows.Forms.Form {
		/// <summary>
		/// Self introduced member variables and properties
		/// </summary>
		protected Icon applicationIcon = null;
		protected Bitmap applicationLogo = null;

		private System.ComponentModel.IContainer components;
		private TD.SandBar.SandBarManager sandBarManager;
		private TD.SandBar.ToolBarContainer leftSandBarDock;
		private TD.SandBar.ToolBarContainer rightSandBarDock;
		private TD.SandBar.ToolBarContainer bottomSandBarDock;
		private TD.SandBar.ToolBarContainer topSandBarDock;
		private TD.SandBar.MenuBar menuBar1;
		private TD.SandBar.MenuBarItem menuBarItem1;
		private TD.SandBar.MenuBarItem menuBarItem2;
		private TD.SandBar.MenuBarItem menuBarItem3;
		private TD.SandBar.MenuBarItem menuBarItem4;
		private TD.SandBar.MenuBarItem menuBarItem5;
		private System.Windows.Forms.StatusBar statusBar;
		private System.Windows.Forms.NotifyIcon notifyIcon;
		private System.Windows.Forms.Panel pnlClientArea;
		private System.Windows.Forms.GroupBox grpBoxOpenForms;
		private System.Windows.Forms.TreeView trVwOpenForms;
		private System.Windows.Forms.ImageList imgLstSmall;
		private TD.SandBar.ToolBar tlBrMain;
		private System.Windows.Forms.ImageList imgLstLarge;
		private TD.SandBar.MenuButtonItem mnuBtnItmNewShellBrowser;
		private TD.SandBar.MenuButtonItem mnuBtnItmNewInternetBrowser;
		private TD.SandBar.MenuButtonItem mnuBtnItmNewFTPBrowser;
		private TD.SandBar.MenuButtonItem mnuBtnItmExit;
		private TD.SandBar.DropDownMenuItem ddMnuItmNew;
		private TD.SandBar.MenuButtonItem menuButtonItem1;

		public ElectrifierAppContext(string[] args, Icon applicationIcon, Bitmap applicationLogo, Form splashScreen) {
			// Enable Windows XP visual styles and OLE
			Application.EnableVisualStyles();
			Application.OleRequired();

			//
			// Erforderlich für die Windows Form-Designerunterstützung
			//
			InitializeComponent();

			this.applicationIcon = applicationIcon;
			this.applicationLogo = applicationLogo;

			Icon                 = applicationIcon;
			notifyIcon.Icon      = applicationIcon;

			// Close the splash screen
			splashScreen.Close();
		}

		/// <summary>
		/// Die verwendeten Ressourcen bereinigen.
		/// </summary>
		protected override void Dispose( bool disposing ) {
			if( disposing ) {
				if(components != null) {
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Vom Windows Form-Designer generierter Code
		/// <summary>
		/// Erforderliche Methode für die Designerunterstützung. 
		/// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
		/// </summary>
		private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			this.sandBarManager = new TD.SandBar.SandBarManager();
			this.bottomSandBarDock = new TD.SandBar.ToolBarContainer();
			this.leftSandBarDock = new TD.SandBar.ToolBarContainer();
			this.rightSandBarDock = new TD.SandBar.ToolBarContainer();
			this.topSandBarDock = new TD.SandBar.ToolBarContainer();
			this.menuBar1 = new TD.SandBar.MenuBar();
			this.menuBarItem1 = new TD.SandBar.MenuBarItem();
			this.menuBarItem2 = new TD.SandBar.MenuBarItem();
			this.menuBarItem3 = new TD.SandBar.MenuBarItem();
			this.menuBarItem4 = new TD.SandBar.MenuBarItem();
			this.menuBarItem5 = new TD.SandBar.MenuBarItem();
			this.tlBrMain = new TD.SandBar.ToolBar();
			this.statusBar = new System.Windows.Forms.StatusBar();
			this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
			this.pnlClientArea = new System.Windows.Forms.Panel();
			this.grpBoxOpenForms = new System.Windows.Forms.GroupBox();
			this.trVwOpenForms = new System.Windows.Forms.TreeView();
			this.imgLstSmall = new System.Windows.Forms.ImageList(this.components);
			this.imgLstLarge = new System.Windows.Forms.ImageList(this.components);
			this.mnuBtnItmNewShellBrowser = new TD.SandBar.MenuButtonItem();
			this.mnuBtnItmNewInternetBrowser = new TD.SandBar.MenuButtonItem();
			this.mnuBtnItmNewFTPBrowser = new TD.SandBar.MenuButtonItem();
			this.mnuBtnItmExit = new TD.SandBar.MenuButtonItem();
			this.ddMnuItmNew = new TD.SandBar.DropDownMenuItem();
			this.menuButtonItem1 = new TD.SandBar.MenuButtonItem();
			this.topSandBarDock.SuspendLayout();
			this.pnlClientArea.SuspendLayout();
			this.grpBoxOpenForms.SuspendLayout();
			this.SuspendLayout();
			// 
			// sandBarManager
			// 
			this.sandBarManager.BottomContainer = this.bottomSandBarDock;
			this.sandBarManager.LeftContainer = this.leftSandBarDock;
			this.sandBarManager.OwnerForm = this;
			this.sandBarManager.RightContainer = this.rightSandBarDock;
			this.sandBarManager.TopContainer = this.topSandBarDock;
			// 
			// bottomSandBarDock
			// 
			this.bottomSandBarDock.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.bottomSandBarDock.Location = new System.Drawing.Point(0, 264);
			this.bottomSandBarDock.Manager = this.sandBarManager;
			this.bottomSandBarDock.Name = "bottomSandBarDock";
			this.bottomSandBarDock.Size = new System.Drawing.Size(248, 0);
			this.bottomSandBarDock.TabIndex = 2;
			// 
			// leftSandBarDock
			// 
			this.leftSandBarDock.Dock = System.Windows.Forms.DockStyle.Left;
			this.leftSandBarDock.Location = new System.Drawing.Point(0, 48);
			this.leftSandBarDock.Manager = this.sandBarManager;
			this.leftSandBarDock.Name = "leftSandBarDock";
			this.leftSandBarDock.Size = new System.Drawing.Size(0, 216);
			this.leftSandBarDock.TabIndex = 0;
			// 
			// rightSandBarDock
			// 
			this.rightSandBarDock.Dock = System.Windows.Forms.DockStyle.Right;
			this.rightSandBarDock.Location = new System.Drawing.Point(248, 48);
			this.rightSandBarDock.Manager = this.sandBarManager;
			this.rightSandBarDock.Name = "rightSandBarDock";
			this.rightSandBarDock.Size = new System.Drawing.Size(0, 216);
			this.rightSandBarDock.TabIndex = 1;
			// 
			// topSandBarDock
			// 
			this.topSandBarDock.Controls.Add(this.menuBar1);
			this.topSandBarDock.Controls.Add(this.tlBrMain);
			this.topSandBarDock.Dock = System.Windows.Forms.DockStyle.Top;
			this.topSandBarDock.Location = new System.Drawing.Point(0, 0);
			this.topSandBarDock.Manager = this.sandBarManager;
			this.topSandBarDock.Name = "topSandBarDock";
			this.topSandBarDock.Size = new System.Drawing.Size(248, 48);
			this.topSandBarDock.TabIndex = 3;
			// 
			// menuBar1
			// 
			this.menuBar1.Buttons.AddRange(new TD.SandBar.ToolbarItemBase[] {
																									 this.menuBarItem1,
																									 this.menuBarItem2,
																									 this.menuBarItem3,
																									 this.menuBarItem4,
																									 this.menuBarItem5});
			this.menuBar1.Guid = new System.Guid("5ac79558-6d07-419a-a8f2-32ffca87d5a9");
			this.menuBar1.Location = new System.Drawing.Point(2, 0);
			this.menuBar1.Name = "menuBar1";
			this.menuBar1.Size = new System.Drawing.Size(246, 24);
			this.menuBar1.TabIndex = 0;
			// 
			// menuBarItem1
			// 
			this.menuBarItem1.MenuItems.AddRange(new TD.SandBar.MenuButtonItem[] {
																											this.mnuBtnItmNewShellBrowser,
																											this.mnuBtnItmNewInternetBrowser,
																											this.mnuBtnItmNewFTPBrowser,
																											this.mnuBtnItmExit});
			this.menuBarItem1.Text = "&File";
			// 
			// menuBarItem2
			// 
			this.menuBarItem2.Text = "&Edit";
			// 
			// menuBarItem3
			// 
			this.menuBarItem3.Text = "&Tools";
			// 
			// menuBarItem4
			// 
			this.menuBarItem4.Text = "&Window";
			// 
			// menuBarItem5
			// 
			this.menuBarItem5.Text = "&Help";
			// 
			// tlBrMain
			// 
			this.tlBrMain.Buttons.AddRange(new TD.SandBar.ToolbarItemBase[] {
																									 this.ddMnuItmNew});
			this.tlBrMain.DockLine = 1;
			this.tlBrMain.Guid = new System.Guid("c7134934-dcc6-4f6f-8564-ebcb80a9d5a0");
			this.tlBrMain.Location = new System.Drawing.Point(2, 24);
			this.tlBrMain.Name = "tlBrMain";
			this.tlBrMain.Size = new System.Drawing.Size(98, 24);
			this.tlBrMain.TabIndex = 1;
			this.tlBrMain.Text = "toolBar1";
			// 
			// statusBar
			// 
			this.statusBar.Location = new System.Drawing.Point(0, 264);
			this.statusBar.Name = "statusBar";
			this.statusBar.Size = new System.Drawing.Size(248, 22);
			this.statusBar.TabIndex = 4;
			this.statusBar.Text = "statusBar1";
			// 
			// notifyIcon
			// 
			this.notifyIcon.Text = "Electrifier";
			this.notifyIcon.Visible = true;
			// 
			// pnlClientArea
			// 
			this.pnlClientArea.Controls.Add(this.grpBoxOpenForms);
			this.pnlClientArea.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlClientArea.Location = new System.Drawing.Point(0, 48);
			this.pnlClientArea.Name = "pnlClientArea";
			this.pnlClientArea.Size = new System.Drawing.Size(248, 216);
			this.pnlClientArea.TabIndex = 5;
			// 
			// grpBoxOpenForms
			// 
			this.grpBoxOpenForms.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.grpBoxOpenForms.Controls.Add(this.trVwOpenForms);
			this.grpBoxOpenForms.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.grpBoxOpenForms.Location = new System.Drawing.Point(8, 8);
			this.grpBoxOpenForms.Name = "grpBoxOpenForms";
			this.grpBoxOpenForms.Size = new System.Drawing.Size(232, 199);
			this.grpBoxOpenForms.TabIndex = 0;
			this.grpBoxOpenForms.TabStop = false;
			this.grpBoxOpenForms.Text = "Electrifier Windows";
			// 
			// trVwOpenForms
			// 
			this.trVwOpenForms.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.trVwOpenForms.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.trVwOpenForms.ImageIndex = -1;
			this.trVwOpenForms.Location = new System.Drawing.Point(8, 16);
			this.trVwOpenForms.Name = "trVwOpenForms";
			this.trVwOpenForms.SelectedImageIndex = -1;
			this.trVwOpenForms.Size = new System.Drawing.Size(216, 175);
			this.trVwOpenForms.TabIndex = 0;
			// 
			// imgLstSmall
			// 
			this.imgLstSmall.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
			this.imgLstSmall.ImageSize = new System.Drawing.Size(16, 16);
			this.imgLstSmall.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// imgLstLarge
			// 
			this.imgLstLarge.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
			this.imgLstLarge.ImageSize = new System.Drawing.Size(24, 24);
			this.imgLstLarge.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// mnuBtnItmNewShellBrowser
			// 
			this.mnuBtnItmNewShellBrowser.Text = "&New Shell Browser";
			// 
			// mnuBtnItmNewInternetBrowser
			// 
			this.mnuBtnItmNewInternetBrowser.Text = "New &Internet Browser";
			// 
			// mnuBtnItmNewFTPBrowser
			// 
			this.mnuBtnItmNewFTPBrowser.Text = "New &FTP Browser";
			// 
			// mnuBtnItmExit
			// 
			this.mnuBtnItmExit.BeginGroup = true;
			this.mnuBtnItmExit.Shortcut = System.Windows.Forms.Shortcut.AltF4;
			this.mnuBtnItmExit.Text = "&Exit all Windows";
			// 
			// ddMnuItmNew
			// 
			this.ddMnuItmNew.MenuItems.AddRange(new TD.SandBar.MenuButtonItem[] {
																										  this.menuButtonItem1});
			this.ddMnuItmNew.Tag = "Hallo!";
			// 
			// menuButtonItem1
			// 
			this.menuButtonItem1.Text = "menuButtonItem1";
			// 
			// ElectrifierAppContext
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(248, 286);
			this.Controls.Add(this.pnlClientArea);
			this.Controls.Add(this.leftSandBarDock);
			this.Controls.Add(this.rightSandBarDock);
			this.Controls.Add(this.bottomSandBarDock);
			this.Controls.Add(this.topSandBarDock);
			this.Controls.Add(this.statusBar);
			this.MinimumSize = new System.Drawing.Size(168, 168);
			this.Name = "ElectrifierAppContext";
			this.Text = "ElectrifierAppContext";
			this.topSandBarDock.ResumeLayout(false);
			this.pnlClientArea.ResumeLayout(false);
			this.grpBoxOpenForms.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion
	}
}
