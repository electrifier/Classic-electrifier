using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Electrifier.Core.Forms {
	/// <summary>
	/// Zusammenfassung für Form1.
	/// </summary>
	public class ElectrifierForm : System.Windows.Forms.Form {
		private System.Windows.Forms.StatusBar mainStatusBar;
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
		private TD.SandDock.SandDockManager sandDockManager;
		private TD.SandDock.DockContainer leftSandDock;
		private TD.SandDock.DockContainer rightSandDock;
		private TD.SandDock.DockContainer bottomSandDock;
		private TD.SandDock.DockContainer topSandDock;
		private TD.SandDock.DocumentContainer documentContainer;
		/// <summary>
		/// Erforderliche Designervariable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		protected ShellBrowserDockControl shellBrowser = null;
		protected FolderBarDockControl folderBar = null;

		public ElectrifierForm() {
			//
			// Erforderlich für die Windows Form-Designerunterstützung
			//
			InitializeComponent();

			shellBrowser = new ShellBrowserDockControl();
			documentContainer.AddDocument(shellBrowser);

			folderBar = new FolderBarDockControl();
			leftSandDock.Controls.Add(folderBar);
			this.leftSandDock.LayoutSystem = new TD.SandDock.SplitLayoutSystem(250, 400,
				System.Windows.Forms.Orientation.Horizontal, new TD.SandDock.LayoutSystemBase[] {
new TD.SandDock.ControlLayoutSystem(250, 407, new TD.SandDock.DockControl[] {
folderBar}, folderBar)});
			leftSandDock.Width = 100;


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
			this.mainStatusBar = new System.Windows.Forms.StatusBar();
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
			this.toolBar1 = new TD.SandBar.ToolBar();
			this.sandDockManager = new TD.SandDock.SandDockManager();
			this.leftSandDock = new TD.SandDock.DockContainer();
			this.rightSandDock = new TD.SandDock.DockContainer();
			this.bottomSandDock = new TD.SandDock.DockContainer();
			this.topSandDock = new TD.SandDock.DockContainer();
			this.documentContainer = new TD.SandDock.DocumentContainer();
			this.topSandBarDock.SuspendLayout();
			this.SuspendLayout();
			// 
			// mainStatusBar
			// 
			this.mainStatusBar.Location = new System.Drawing.Point(0, 432);
			this.mainStatusBar.Name = "mainStatusBar";
			this.mainStatusBar.Size = new System.Drawing.Size(880, 22);
			this.mainStatusBar.TabIndex = 0;
			this.mainStatusBar.Text = "electrifier initialized.";
			// 
			// sandBarManager
			// 
			this.sandBarManager.BottomContainer = this.bottomSandBarDock;
			this.sandBarManager.LeftContainer = this.leftSandBarDock;
			this.sandBarManager.OwnerForm = this;
			this.sandBarManager.Renderer = new TD.SandBar.WhidbeyRenderer();
			this.sandBarManager.RightContainer = this.rightSandBarDock;
			this.sandBarManager.TopContainer = this.topSandBarDock;
			// 
			// bottomSandBarDock
			// 
			this.bottomSandBarDock.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.bottomSandBarDock.Location = new System.Drawing.Point(0, 432);
			this.bottomSandBarDock.Manager = this.sandBarManager;
			this.bottomSandBarDock.Name = "bottomSandBarDock";
			this.bottomSandBarDock.Size = new System.Drawing.Size(880, 0);
			this.bottomSandBarDock.TabIndex = 3;
			// 
			// leftSandBarDock
			// 
			this.leftSandBarDock.Dock = System.Windows.Forms.DockStyle.Left;
			this.leftSandBarDock.Location = new System.Drawing.Point(0, 47);
			this.leftSandBarDock.Manager = this.sandBarManager;
			this.leftSandBarDock.Name = "leftSandBarDock";
			this.leftSandBarDock.Size = new System.Drawing.Size(0, 385);
			this.leftSandBarDock.TabIndex = 1;
			// 
			// rightSandBarDock
			// 
			this.rightSandBarDock.Dock = System.Windows.Forms.DockStyle.Right;
			this.rightSandBarDock.Location = new System.Drawing.Point(880, 47);
			this.rightSandBarDock.Manager = this.sandBarManager;
			this.rightSandBarDock.Name = "rightSandBarDock";
			this.rightSandBarDock.Size = new System.Drawing.Size(0, 385);
			this.rightSandBarDock.TabIndex = 2;
			// 
			// topSandBarDock
			// 
			this.topSandBarDock.Controls.Add(this.menuBar1);
			this.topSandBarDock.Controls.Add(this.toolBar1);
			this.topSandBarDock.Dock = System.Windows.Forms.DockStyle.Top;
			this.topSandBarDock.Location = new System.Drawing.Point(0, 0);
			this.topSandBarDock.Manager = this.sandBarManager;
			this.topSandBarDock.Name = "topSandBarDock";
			this.topSandBarDock.Size = new System.Drawing.Size(880, 47);
			this.topSandBarDock.TabIndex = 4;
			// 
			// menuBar1
			// 
			this.menuBar1.Buttons.AddRange(new TD.SandBar.ToolbarItemBase[] {
																									 this.menuBarItem1,
																									 this.menuBarItem2,
																									 this.menuBarItem3,
																									 this.menuBarItem4,
																									 this.menuBarItem5});
			this.menuBar1.Guid = new System.Guid("b85b13ec-19c1-4c0f-9959-f0eb3c0d65fd");
			this.menuBar1.Location = new System.Drawing.Point(2, 0);
			this.menuBar1.Name = "menuBar1";
			this.menuBar1.Size = new System.Drawing.Size(878, 24);
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
			this.menuBarItem3.Text = "&View";
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
			this.toolBar1.Guid = new System.Guid("eb0733ee-b705-40e9-bc6e-aa9166d4e3d4");
			this.toolBar1.Location = new System.Drawing.Point(2, 24);
			this.toolBar1.Name = "toolBar1";
			this.toolBar1.Size = new System.Drawing.Size(25, 23);
			this.toolBar1.TabIndex = 1;
			this.toolBar1.Text = "toolBar1";
			// 
			// sandDockManager
			// 
			this.sandDockManager.OwnerForm = this;
			// 
			// leftSandDock
			// 
			this.leftSandDock.Dock = System.Windows.Forms.DockStyle.Left;
			this.leftSandDock.Guid = new System.Guid("2d149c5a-ca28-421d-92bf-624a87001d87");
			this.leftSandDock.LayoutSystem = new TD.SandDock.SplitLayoutSystem(250, 400);
			this.leftSandDock.Location = new System.Drawing.Point(0, 47);
			this.leftSandDock.Manager = this.sandDockManager;
			this.leftSandDock.Name = "leftSandDock";
			this.leftSandDock.Size = new System.Drawing.Size(0, 385);
			this.leftSandDock.TabIndex = 5;
			// 
			// rightSandDock
			// 
			this.rightSandDock.Dock = System.Windows.Forms.DockStyle.Right;
			this.rightSandDock.Guid = new System.Guid("7597382f-25a9-4d99-a488-09ed9650748f");
			this.rightSandDock.LayoutSystem = new TD.SandDock.SplitLayoutSystem(250, 400);
			this.rightSandDock.Location = new System.Drawing.Point(880, 47);
			this.rightSandDock.Manager = this.sandDockManager;
			this.rightSandDock.Name = "rightSandDock";
			this.rightSandDock.Size = new System.Drawing.Size(0, 385);
			this.rightSandDock.TabIndex = 6;
			// 
			// bottomSandDock
			// 
			this.bottomSandDock.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.bottomSandDock.Guid = new System.Guid("3fefad6b-edde-4b32-9839-ae7772a862da");
			this.bottomSandDock.LayoutSystem = new TD.SandDock.SplitLayoutSystem(250, 400);
			this.bottomSandDock.Location = new System.Drawing.Point(0, 432);
			this.bottomSandDock.Manager = this.sandDockManager;
			this.bottomSandDock.Name = "bottomSandDock";
			this.bottomSandDock.Size = new System.Drawing.Size(880, 0);
			this.bottomSandDock.TabIndex = 7;
			// 
			// topSandDock
			// 
			this.topSandDock.Dock = System.Windows.Forms.DockStyle.Top;
			this.topSandDock.Guid = new System.Guid("b16e6af7-c27d-48d9-86c8-37a7518c04d4");
			this.topSandDock.LayoutSystem = new TD.SandDock.SplitLayoutSystem(250, 400);
			this.topSandDock.Location = new System.Drawing.Point(0, 47);
			this.topSandDock.Manager = this.sandDockManager;
			this.topSandDock.Name = "topSandDock";
			this.topSandDock.Size = new System.Drawing.Size(880, 0);
			this.topSandDock.TabIndex = 8;
			// 
			// documentContainer
			// 
			this.documentContainer.Cursor = System.Windows.Forms.Cursors.Default;
			this.documentContainer.Guid = new System.Guid("bb63c0c1-c39b-43d5-a399-88c23c1c038f");
			this.documentContainer.LayoutSystem = new TD.SandDock.SplitLayoutSystem(250, 400);
			this.documentContainer.Location = new System.Drawing.Point(0, 47);
			this.documentContainer.Manager = null;
			this.documentContainer.Name = "documentContainer";
			this.documentContainer.Size = new System.Drawing.Size(880, 385);
			this.documentContainer.TabIndex = 9;
			// 
			// ElectrifierForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(880, 454);
			this.Controls.Add(this.documentContainer);
			this.Controls.Add(this.leftSandDock);
			this.Controls.Add(this.rightSandDock);
			this.Controls.Add(this.bottomSandDock);
			this.Controls.Add(this.topSandDock);
			this.Controls.Add(this.leftSandBarDock);
			this.Controls.Add(this.rightSandBarDock);
			this.Controls.Add(this.bottomSandBarDock);
			this.Controls.Add(this.topSandBarDock);
			this.Controls.Add(this.mainStatusBar);
			this.Name = "ElectrifierForm";
			this.Text = "electrifier shell browser";
			this.topSandBarDock.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion
	}
}
