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
		private TD.SandBar.ToolBarContainer leftSandBarDock;
		private TD.SandBar.ToolBarContainer rightSandBarDock;
		private TD.SandBar.ToolBarContainer bottomSandBarDock;
		private TD.SandBar.ToolBarContainer topSandBarDock;
		private TD.SandBar.MenuBar menuBar;
		private TD.SandBar.ToolBar toolBar;
		private System.Windows.Forms.StatusBar statusBar;
		private TD.SandBar.SandBarManager sandBarManager;
		private System.Windows.Forms.Panel clientAreaPanel;
		private System.Windows.Forms.TreeView trVwOpenForms;
		private System.Windows.Forms.GroupBox grpBxOpenForms;
		/// <summary>
		/// Erforderliche Designervariable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ElectrifierAppContext(string[] args, Icon applicationIcon, Bitmap applicationLogo, Form splashScreenForm) {
			// Enable application to use WindowsXP Visual Styles and Objekt Linking and Embedding
			Application.EnableVisualStyles();
			Application.OleRequired();

			//
			// Erforderlich für die Windows Form-Designerunterstützung
			//
			InitializeComponent();

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
			this.statusBar = new System.Windows.Forms.StatusBar();
			this.sandBarManager = new TD.SandBar.SandBarManager();
			this.leftSandBarDock = new TD.SandBar.ToolBarContainer();
			this.rightSandBarDock = new TD.SandBar.ToolBarContainer();
			this.bottomSandBarDock = new TD.SandBar.ToolBarContainer();
			this.topSandBarDock = new TD.SandBar.ToolBarContainer();
			this.menuBar = new TD.SandBar.MenuBar();
			this.toolBar = new TD.SandBar.ToolBar();
			this.clientAreaPanel = new System.Windows.Forms.Panel();
			this.grpBxOpenForms = new System.Windows.Forms.GroupBox();
			this.trVwOpenForms = new System.Windows.Forms.TreeView();
			this.topSandBarDock.SuspendLayout();
			this.clientAreaPanel.SuspendLayout();
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
			// leftSandBarDock
			// 
			this.leftSandBarDock.Dock = System.Windows.Forms.DockStyle.Left;
			this.leftSandBarDock.Location = new System.Drawing.Point(0, 46);
			this.leftSandBarDock.Manager = this.sandBarManager;
			this.leftSandBarDock.Name = "leftSandBarDock";
			this.leftSandBarDock.Size = new System.Drawing.Size(0, 216);
			this.leftSandBarDock.TabIndex = 1;
			// 
			// rightSandBarDock
			// 
			this.rightSandBarDock.Dock = System.Windows.Forms.DockStyle.Right;
			this.rightSandBarDock.Location = new System.Drawing.Point(288, 46);
			this.rightSandBarDock.Manager = this.sandBarManager;
			this.rightSandBarDock.Name = "rightSandBarDock";
			this.rightSandBarDock.Size = new System.Drawing.Size(0, 216);
			this.rightSandBarDock.TabIndex = 2;
			// 
			// bottomSandBarDock
			// 
			this.bottomSandBarDock.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.bottomSandBarDock.Location = new System.Drawing.Point(0, 262);
			this.bottomSandBarDock.Manager = this.sandBarManager;
			this.bottomSandBarDock.Name = "bottomSandBarDock";
			this.bottomSandBarDock.Size = new System.Drawing.Size(288, 0);
			this.bottomSandBarDock.TabIndex = 3;
			// 
			// topSandBarDock
			// 
			this.topSandBarDock.Controls.Add(this.menuBar);
			this.topSandBarDock.Controls.Add(this.toolBar);
			this.topSandBarDock.Dock = System.Windows.Forms.DockStyle.Top;
			this.topSandBarDock.Location = new System.Drawing.Point(0, 0);
			this.topSandBarDock.Manager = this.sandBarManager;
			this.topSandBarDock.Name = "topSandBarDock";
			this.topSandBarDock.Size = new System.Drawing.Size(288, 46);
			this.topSandBarDock.TabIndex = 4;
			// 
			// menuBar
			// 
			this.menuBar.Guid = new System.Guid("52a509f1-a81b-4b18-90b9-466652cfe1bf");
			this.menuBar.Location = new System.Drawing.Point(2, 0);
			this.menuBar.Name = "menuBar";
			this.menuBar.Size = new System.Drawing.Size(286, 23);
			this.menuBar.TabIndex = 0;
			// 
			// toolBar
			// 
			this.toolBar.DockLine = 1;
			this.toolBar.Guid = new System.Guid("4680e90f-86e6-4c5d-81dd-e9925f05f13d");
			this.toolBar.Location = new System.Drawing.Point(2, 23);
			this.toolBar.Name = "toolBar";
			this.toolBar.Size = new System.Drawing.Size(25, 23);
			this.toolBar.TabIndex = 1;
			this.toolBar.Text = "toolBar1";
			// 
			// clientAreaPanel
			// 
			this.clientAreaPanel.Controls.Add(this.grpBxOpenForms);
			this.clientAreaPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.clientAreaPanel.Location = new System.Drawing.Point(0, 46);
			this.clientAreaPanel.Name = "clientAreaPanel";
			this.clientAreaPanel.Size = new System.Drawing.Size(288, 194);
			this.clientAreaPanel.TabIndex = 5;
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
			this.grpBxOpenForms.Size = new System.Drawing.Size(272, 176);
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
			this.trVwOpenForms.Size = new System.Drawing.Size(256, 152);
			this.trVwOpenForms.TabIndex = 0;
			// 
			// ElectrifierAppContext
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(288, 262);
			this.Controls.Add(this.clientAreaPanel);
			this.Controls.Add(this.statusBar);
			this.Controls.Add(this.leftSandBarDock);
			this.Controls.Add(this.rightSandBarDock);
			this.Controls.Add(this.bottomSandBarDock);
			this.Controls.Add(this.topSandBarDock);
			this.Name = "ElectrifierAppContext";
			this.Text = "ElectrifierAppContext";
			this.topSandBarDock.ResumeLayout(false);
			this.clientAreaPanel.ResumeLayout(false);
			this.grpBxOpenForms.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion
	}
}
