//	<file>
//		<copyright see="www.electrifier.org"/>
//		<license   see="www.electrifier.org"/>
//		<owner     name="Thorsten Jung" email="taj@electrifier.org"/>
//		<version   value="$Id: IExtTreeViewNode.cs,v 1.1 2004/08/25 17:59:07 jung2t Exp $"/>
//	</file>

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using Electrifier.Core.Forms;
using Electrifier.Core.Controls;
using Electrifier.Core.Controls.ActionControls;

namespace Electrifier.Core {
	/// <summary>
	/// Zusammenfassung für ElectrifierAppContext.
	/// </summary>
	public sealed class ElectrifierAppContext : System.Windows.Forms.Form {
		private static ElectrifierAppContext appContext = null;
		public  static ElectrifierAppContext AppContext { get { return appContext; } }
		private static Icon                  appIcon    = null;
		public  static Icon                  AppIcon    { get { return appIcon; } }
		private static Bitmap                appLogo    = null;
		public  static Bitmap                AppLogo    { get { return appLogo; } }

		#region Erforderliche Designervariablen.
		private TD.SandBar.ToolBarContainer leftSandBarDock;
		private TD.SandBar.ToolBarContainer rightSandBarDock;
		private TD.SandBar.ToolBarContainer bottomSandBarDock;
		private TD.SandBar.ToolBarContainer topSandBarDock;
		private TD.SandBar.MenuBar menuBar;
		private TD.SandBar.ToolBar toolBar;
		private System.Windows.Forms.StatusBar statusBar;
		private TD.SandBar.SandBarManager sandBarManager;
		private System.Windows.Forms.TreeView trVwOpenForms;
		private System.Windows.Forms.GroupBox grpBxOpenForms;
		private TD.SandBar.MenuBarItem mnuBarItmFile;
		private TD.SandBar.DropDownMenuItem dropDownMenuItem1;
		private System.Windows.Forms.Panel pnlClientArea;
		private System.Windows.Forms.NotifyIcon notifyIcon;
		private System.ComponentModel.IContainer components;
		#endregion

		public ElectrifierAppContext(string[] args, Icon appIcon, Bitmap appLogo, Form splashScreenForm) {
			//
			// Erforderlich für die Windows Form-Designerunterstützung
			//
			InitializeComponent();

			//
			// TODO: Fügen Sie den Konstruktorcode nach dem Aufruf von InitializeComponent hinzu
			//
			if(appContext == null) {
				ElectrifierAppContext.appContext = this;
				ElectrifierAppContext.appIcon    = appIcon;
				ElectrifierAppContext.appLogo    = appLogo;
			} else {
				throw new InvalidOperationException("Electrifier.Core.ElectrifierAppContext: " +
					"Instantion made although already instantiated");
			}

			Icon            = appIcon;
			notifyIcon.Icon = appIcon;

			// Initialize actions implemented by ElectrifierAppContext
			string acNameSpace = "Electrifier.Core.Actions.";
			BasicAction bacNewBrowserForm = new BasicAction(acNameSpace + "NewBrowserForm",
				true, 0, new ExecutionEventHandler(action_NewElectrifierBrowserForm));
			BasicAction bacCloseAllForms = new BasicAction(acNameSpace + "CloseAllForms",
				true, 0, new ExecutionEventHandler(action_CloseAllForms));

			// Initialize menu bar
			mnuBarItmFile.MenuItems.Add(new ActionMenuButtonItem(bacNewBrowserForm));
			mnuBarItmFile.MenuItems.Add(new ActionMenuButtonItem(bacCloseAllForms));

			// Close splash screen form
			splashScreenForm.Close();
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

		private void action_NewElectrifierBrowserForm(object sender, ExecutionEventArgs e) {
			MessageBox.Show("New browser");
//			new ElectrifierBrowserForm().Show();
		}

		private void action_CloseAllForms(object sender, ExecutionEventArgs e) {
			Close();
		}

		#region Vom Windows Form-Designer generierter Code
		/// <summary>
		/// Erforderliche Methode für die Designerunterstützung. 
		/// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
		/// </summary>
		private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			this.statusBar = new System.Windows.Forms.StatusBar();
			this.sandBarManager = new TD.SandBar.SandBarManager();
			this.bottomSandBarDock = new TD.SandBar.ToolBarContainer();
			this.leftSandBarDock = new TD.SandBar.ToolBarContainer();
			this.rightSandBarDock = new TD.SandBar.ToolBarContainer();
			this.topSandBarDock = new TD.SandBar.ToolBarContainer();
			this.menuBar = new TD.SandBar.MenuBar();
			this.mnuBarItmFile = new TD.SandBar.MenuBarItem();
			this.toolBar = new TD.SandBar.ToolBar();
			this.dropDownMenuItem1 = new TD.SandBar.DropDownMenuItem();
			this.pnlClientArea = new System.Windows.Forms.Panel();
			this.grpBxOpenForms = new System.Windows.Forms.GroupBox();
			this.trVwOpenForms = new System.Windows.Forms.TreeView();
			this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
			this.topSandBarDock.SuspendLayout();
			this.pnlClientArea.SuspendLayout();
			this.grpBxOpenForms.SuspendLayout();
			this.SuspendLayout();
			// 
			// statusBar
			// 
			this.statusBar.Location = new System.Drawing.Point(0, 240);
			this.statusBar.Name = "statusBar";
			this.statusBar.Size = new System.Drawing.Size(288, 22);
			this.statusBar.TabIndex = 0;
			this.statusBar.Text = "No Electrifier Windows opened.";
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
			this.bottomSandBarDock.Location = new System.Drawing.Point(0, 240);
			this.bottomSandBarDock.Manager = this.sandBarManager;
			this.bottomSandBarDock.Name = "bottomSandBarDock";
			this.bottomSandBarDock.Size = new System.Drawing.Size(288, 0);
			this.bottomSandBarDock.TabIndex = 3;
			// 
			// leftSandBarDock
			// 
			this.leftSandBarDock.Dock = System.Windows.Forms.DockStyle.Left;
			this.leftSandBarDock.Location = new System.Drawing.Point(0, 48);
			this.leftSandBarDock.Manager = this.sandBarManager;
			this.leftSandBarDock.Name = "leftSandBarDock";
			this.leftSandBarDock.Size = new System.Drawing.Size(0, 192);
			this.leftSandBarDock.TabIndex = 1;
			// 
			// rightSandBarDock
			// 
			this.rightSandBarDock.Dock = System.Windows.Forms.DockStyle.Right;
			this.rightSandBarDock.Location = new System.Drawing.Point(288, 48);
			this.rightSandBarDock.Manager = this.sandBarManager;
			this.rightSandBarDock.Name = "rightSandBarDock";
			this.rightSandBarDock.Size = new System.Drawing.Size(0, 192);
			this.rightSandBarDock.TabIndex = 2;
			// 
			// topSandBarDock
			// 
			this.topSandBarDock.Controls.Add(this.menuBar);
			this.topSandBarDock.Controls.Add(this.toolBar);
			this.topSandBarDock.Dock = System.Windows.Forms.DockStyle.Top;
			this.topSandBarDock.Location = new System.Drawing.Point(0, 0);
			this.topSandBarDock.Manager = this.sandBarManager;
			this.topSandBarDock.Name = "topSandBarDock";
			this.topSandBarDock.Size = new System.Drawing.Size(288, 48);
			this.topSandBarDock.TabIndex = 4;
			// 
			// menuBar
			// 
			this.menuBar.Buttons.AddRange(new TD.SandBar.ToolbarItemBase[] {
																									this.mnuBarItmFile});
			this.menuBar.Guid = new System.Guid("52a509f1-a81b-4b18-90b9-466652cfe1bf");
			this.menuBar.Location = new System.Drawing.Point(2, 0);
			this.menuBar.Name = "menuBar";
			this.menuBar.Size = new System.Drawing.Size(286, 24);
			this.menuBar.TabIndex = 0;
			// 
			// mnuBarItmFile
			// 
			this.mnuBarItmFile.Text = "&File";
			// 
			// toolBar
			// 
			this.toolBar.Buttons.AddRange(new TD.SandBar.ToolbarItemBase[] {
																									this.dropDownMenuItem1});
			this.toolBar.DockLine = 1;
			this.toolBar.Guid = new System.Guid("4680e90f-86e6-4c5d-81dd-e9925f05f13d");
			this.toolBar.Location = new System.Drawing.Point(2, 24);
			this.toolBar.Name = "toolBar";
			this.toolBar.Size = new System.Drawing.Size(70, 24);
			this.toolBar.TabIndex = 1;
			this.toolBar.Text = "toolBar1";
			// 
			// dropDownMenuItem1
			// 
			this.dropDownMenuItem1.Text = "New";
			// 
			// pnlClientArea
			// 
			this.pnlClientArea.Controls.Add(this.grpBxOpenForms);
			this.pnlClientArea.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlClientArea.Location = new System.Drawing.Point(0, 48);
			this.pnlClientArea.Name = "pnlClientArea";
			this.pnlClientArea.Size = new System.Drawing.Size(288, 192);
			this.pnlClientArea.TabIndex = 5;
			// 
			// grpBxOpenForms
			// 
			this.grpBxOpenForms.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.grpBxOpenForms.Controls.Add(this.trVwOpenForms);
			this.grpBxOpenForms.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.grpBxOpenForms.Location = new System.Drawing.Point(8, 8);
			this.grpBxOpenForms.Name = "grpBxOpenForms";
			this.grpBxOpenForms.Size = new System.Drawing.Size(272, 174);
			this.grpBxOpenForms.TabIndex = 0;
			this.grpBxOpenForms.TabStop = false;
			this.grpBxOpenForms.Text = "Open Electrifier Windows";
			// 
			// trVwOpenForms
			// 
			this.trVwOpenForms.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.trVwOpenForms.ImageIndex = -1;
			this.trVwOpenForms.Location = new System.Drawing.Point(8, 16);
			this.trVwOpenForms.Name = "trVwOpenForms";
			this.trVwOpenForms.SelectedImageIndex = -1;
			this.trVwOpenForms.Size = new System.Drawing.Size(256, 150);
			this.trVwOpenForms.TabIndex = 0;
			// 
			// notifyIcon
			// 
			this.notifyIcon.Text = "Electrifier";
			this.notifyIcon.Visible = true;
			// 
			// ElectrifierAppContext
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(288, 262);
			this.Controls.Add(this.pnlClientArea);
			this.Controls.Add(this.leftSandBarDock);
			this.Controls.Add(this.rightSandBarDock);
			this.Controls.Add(this.bottomSandBarDock);
			this.Controls.Add(this.topSandBarDock);
			this.Controls.Add(this.statusBar);
			this.Name = "ElectrifierAppContext";
			this.Text = "Electrifier";
			this.topSandBarDock.ResumeLayout(false);
			this.pnlClientArea.ResumeLayout(false);
			this.grpBxOpenForms.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion
	}
}
