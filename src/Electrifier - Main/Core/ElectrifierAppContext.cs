//	<file>
//		<copyright see="www.electrifier.org"/>
//		<license   see="www.electrifier.org"/>
//		<owner     name="Thorsten Jung" email="taj@electrifier.org"/>
//		<version   value="$Id: BasicShellObject.cs,v 1.13 2004/09/10 20:30:33 taj bender Exp $"/>
//	</file>

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
		private System.ComponentModel.IContainer components;

		/// <summary>
		/// Erforderliche Designervariable.
		/// </summary>
<<<<<<< .working
		private System.ComponentModel.Container components = null;

		public ElectrifierAppContext(string[] args, Icon applicationIcon, Bitmap applicationLogo, Form splashScreenForm) {
			// Initialize application object to use Windows XP visual styles and OLE
=======
		protected Icon applicationIcon = null;
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
		private TD.SandBar.ToolBar toolBar1;
		private System.Windows.Forms.StatusBar statusBar;
		private System.Windows.Forms.NotifyIcon notifyIcon;
		private System.Windows.Forms.Panel pnlClientArea;
		private System.Windows.Forms.GroupBox grpBoxOpenForms;
		private System.Windows.Forms.TreeView trVwOpenForms;
		protected Bitmap applicationLogo = null;


		public ElectrifierAppContext(string[] args, Icon applicationIcon, Bitmap applicationLogo, Form splashScreen) {
			// Enable Windows XP visual styles
>>>>>>> .merge-right.r7
			Application.EnableVisualStyles();

			//
			// Erforderlich für die Windows Form-Designerunterstützung
			//
			InitializeComponent();

<<<<<<< .working
			//
			// TODO: Fügen Sie den Konstruktorcode nach dem Aufruf von InitializeComponent hinzu
			//
			splashScreenForm.Close();
=======
			this.applicationIcon = applicationIcon;
			this.applicationLogo = applicationLogo;
//			trayNotifyIcon       = new ExtTrayNotifyIcon(applicationIcon);
//			trayNotifyIcon.Text  = "Electrifier";

			Icon = applicationIcon;
			splashScreen.Close();
>>>>>>> .merge-right.r7
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
<<<<<<< .working
			this.Size = new System.Drawing.Size(300,300);
=======
			this.sandBarManager = new TD.SandBar.SandBarManager();
			this.leftSandBarDock = new TD.SandBar.ToolBarContainer();
			this.rightSandBarDock = new TD.SandBar.ToolBarContainer();
			this.bottomSandBarDock = new TD.SandBar.ToolBarContainer();
			this.topSandBarDock = new TD.SandBar.ToolBarContainer();
			this.menuBar1 = new TD.SandBar.MenuBar();
			this.menuBarItem1 = new TD.SandBar.MenuBarItem();
			this.menuBarItem2 = new TD.SandBar.MenuBarItem();
			this.menuBarItem3 = new TD.SandBar.MenuBarItem();
			this.menuBarItem4 = new TD.SandBar.MenuBarItem();
			this.menuBarItem5 = new TD.SandBar.MenuBarItem();
			this.toolBar1 = new TD.SandBar.ToolBar();
			this.statusBar = new System.Windows.Forms.StatusBar();
			this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
			this.pnlClientArea = new System.Windows.Forms.Panel();
			this.grpBoxOpenForms = new System.Windows.Forms.GroupBox();
			this.trVwOpenForms = new System.Windows.Forms.TreeView();
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
			// leftSandBarDock
			// 
			this.leftSandBarDock.Dock = System.Windows.Forms.DockStyle.Left;
			this.leftSandBarDock.Location = new System.Drawing.Point(0, 47);
			this.leftSandBarDock.Manager = this.sandBarManager;
			this.leftSandBarDock.Name = "leftSandBarDock";
			this.leftSandBarDock.Size = new System.Drawing.Size(0, 201);
			this.leftSandBarDock.TabIndex = 0;
			// 
			// rightSandBarDock
			// 
			this.rightSandBarDock.Dock = System.Windows.Forms.DockStyle.Right;
			this.rightSandBarDock.Location = new System.Drawing.Point(272, 47);
			this.rightSandBarDock.Manager = this.sandBarManager;
			this.rightSandBarDock.Name = "rightSandBarDock";
			this.rightSandBarDock.Size = new System.Drawing.Size(0, 201);
			this.rightSandBarDock.TabIndex = 1;
			// 
			// bottomSandBarDock
			// 
			this.bottomSandBarDock.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.bottomSandBarDock.Location = new System.Drawing.Point(0, 248);
			this.bottomSandBarDock.Manager = this.sandBarManager;
			this.bottomSandBarDock.Name = "bottomSandBarDock";
			this.bottomSandBarDock.Size = new System.Drawing.Size(272, 0);
			this.bottomSandBarDock.TabIndex = 2;
			// 
			// topSandBarDock
			// 
			this.topSandBarDock.Controls.Add(this.menuBar1);
			this.topSandBarDock.Controls.Add(this.toolBar1);
			this.topSandBarDock.Dock = System.Windows.Forms.DockStyle.Top;
			this.topSandBarDock.Location = new System.Drawing.Point(0, 0);
			this.topSandBarDock.Manager = this.sandBarManager;
			this.topSandBarDock.Name = "topSandBarDock";
			this.topSandBarDock.Size = new System.Drawing.Size(272, 47);
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
			this.menuBar1.Size = new System.Drawing.Size(270, 24);
			this.menuBar1.TabIndex = 0;
			// 
			// menuBarItem1
			// 
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
			// toolBar1
			// 
			this.toolBar1.DockLine = 1;
			this.toolBar1.Guid = new System.Guid("c7134934-dcc6-4f6f-8564-ebcb80a9d5a0");
			this.toolBar1.Location = new System.Drawing.Point(2, 24);
			this.toolBar1.Name = "toolBar1";
			this.toolBar1.Size = new System.Drawing.Size(25, 23);
			this.toolBar1.TabIndex = 1;
			this.toolBar1.Text = "toolBar1";
			// 
			// statusBar
			// 
			this.statusBar.Location = new System.Drawing.Point(0, 248);
			this.statusBar.Name = "statusBar";
			this.statusBar.Size = new System.Drawing.Size(272, 22);
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
			this.pnlClientArea.Location = new System.Drawing.Point(0, 47);
			this.pnlClientArea.Name = "pnlClientArea";
			this.pnlClientArea.Size = new System.Drawing.Size(272, 201);
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
			this.grpBoxOpenForms.Size = new System.Drawing.Size(256, 184);
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
			this.trVwOpenForms.Size = new System.Drawing.Size(240, 160);
			this.trVwOpenForms.TabIndex = 0;
			// 
			// ElectrifierAppContext
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(272, 270);
			this.Controls.Add(this.pnlClientArea);
			this.Controls.Add(this.leftSandBarDock);
			this.Controls.Add(this.rightSandBarDock);
			this.Controls.Add(this.bottomSandBarDock);
			this.Controls.Add(this.topSandBarDock);
			this.Controls.Add(this.statusBar);
			this.MinimumSize = new System.Drawing.Size(168, 168);
			this.Name = "ElectrifierAppContext";
>>>>>>> .merge-right.r7
			this.Text = "ElectrifierAppContext";
		}
		#endregion
	}
}
