//	<file>
//		<copyright see="www.electrifier.org"/>
//		<license   see="www.electrifier.org"/>
//		<owner     name="Thorsten Jung" email="taj@electrifier.org"/>
//		<version   value="$Id: IService.cs,v 1.2 2004/08/09 20:50:38 jung2t Exp $"/>
//	</file>

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Xml;
using TD.SandDock;

using Electrifier.Core;
using Electrifier.Core.Forms.DockControls;

namespace Electrifier.Core.Forms {
	/// <summary>
	/// Zusammenfassung für MainWindowForm.
	/// </summary>
	public class MainWindowForm : System.Windows.Forms.Form, IPersistent {
		protected Guid guid = Guid.NewGuid();
		public    Guid Guid { get { return guid; } }


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
		private TD.SandDock.DockContainer leftSandDock;
		private TD.SandDock.DockContainer rightSandDock;
		private TD.SandDock.DockContainer bottomSandDock;
		private TD.SandDock.DockContainer topSandDock;
		private TD.SandBar.SandBarManager sandBarManager;
		private TD.SandDock.SandDockManager sandDockManager;
		private System.Windows.Forms.StatusBar statusBar;
		private TD.SandDock.DocumentContainer documentContainer;
		/// <summary>
		/// Erforderliche Designervariable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public MainWindowForm() {
			//
			// Erforderlich für die Windows Form-Designerunterstützung
			//
			InitializeComponent();

			//
			// TODO: Fügen Sie den Konstruktorcode nach dem Aufruf von InitializeComponent hinzu
			//
			documentContainer.Manager = new SandDockManager();
			Icon = AppContext.Icon;

			// Initialize form state regarding to the configuration saved
			ApplyFormConfiguration();
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

		protected void ApplyFormConfiguration() {
			ApplyFormConfiguration("default");
		}

		protected void ApplyFormConfiguration(string configuration) {
			// TODO: This is test code only; the default configuration will also be read out from XML
			ShellBrowserDockControl shellBrowser = new ShellBrowserDockControl();
			this.documentContainer.AddDocument(shellBrowser);

			FolderBarDockControl folderBar = new FolderBarDockControl();
			folderBar.Manager = sandDockManager;
			folderBar.Open(DockLocation.Left);
			folderBar.LayoutSystem.Collapsed = true;
		}

		#region IPersistent Member
		public XmlNode CreatePersistenceInfo(XmlDocument targetXmlDocument, string prefix, string nmspURI) {
			// Create persistence information for main window form
			XmlNode      mainWindowNode = targetXmlDocument.CreateElement(prefix, "Core.Forms.MainWindowForm", nmspURI);
			XmlAttribute guidAttr       = targetXmlDocument.CreateAttribute(prefix, "Guid", nmspURI);
			guidAttr.Value              = guid.ToString();
			XmlAttribute leftAttr       = targetXmlDocument.CreateAttribute(prefix, "Left", nmspURI);
			leftAttr.Value              = Left.ToString();
			XmlAttribute topAttr        = targetXmlDocument.CreateAttribute(prefix, "Top", nmspURI);
			topAttr.Value               = Top.ToString();
			XmlAttribute widthAttr      = targetXmlDocument.CreateAttribute(prefix, "Width", nmspURI);
			widthAttr.Value             = Width.ToString();
			XmlAttribute heightAttr     = targetXmlDocument.CreateAttribute(prefix, "Height", nmspURI);
			heightAttr.Value            = Height.ToString();
			mainWindowNode.Attributes.Append(guidAttr);
			mainWindowNode.Attributes.Append(leftAttr);
			mainWindowNode.Attributes.Append(topAttr);
			mainWindowNode.Attributes.Append(widthAttr);
			mainWindowNode.Attributes.Append(heightAttr);

			// Append persistance information for SandBar and SandDock components
			mainWindowNode.AppendChild(AppContext.CreateXmlNodeFromForeignXmlDocument(targetXmlDocument,
				prefix, "SandBarManager", nmspURI, sandBarManager.GetLayout()));
			mainWindowNode.AppendChild(AppContext.CreateXmlNodeFromForeignXmlDocument(targetXmlDocument,
				prefix, "SandDockManager", nmspURI, sandDockManager.GetLayout()));
			mainWindowNode.AppendChild(AppContext.CreateXmlNodeFromForeignXmlDocument(targetXmlDocument,
				prefix, "DocumentContainer", nmspURI, documentContainer.Manager.GetLayout()));

			return mainWindowNode;
		}

		public void ApplyPersistenceInfo(XmlNode persistenceInfo) {
			// TODO:  Implementierung von MainWindowForm.ApplyPersistenceInfo hinzufügen
		}
		#endregion

		#region Vom Windows Form-Designer generierter Code
		/// <summary>
		/// Erforderliche Methode für die Designerunterstützung. 
		/// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
		/// </summary>
		private void InitializeComponent() {
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
			this.statusBar = new System.Windows.Forms.StatusBar();
			this.documentContainer = new TD.SandDock.DocumentContainer();
			this.topSandBarDock.SuspendLayout();
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
			this.bottomSandBarDock.Location = new System.Drawing.Point(0, 376);
			this.bottomSandBarDock.Manager = this.sandBarManager;
			this.bottomSandBarDock.Name = "bottomSandBarDock";
			this.bottomSandBarDock.Size = new System.Drawing.Size(824, 0);
			this.bottomSandBarDock.TabIndex = 2;
			// 
			// leftSandBarDock
			// 
			this.leftSandBarDock.Dock = System.Windows.Forms.DockStyle.Left;
			this.leftSandBarDock.Location = new System.Drawing.Point(0, 47);
			this.leftSandBarDock.Manager = this.sandBarManager;
			this.leftSandBarDock.Name = "leftSandBarDock";
			this.leftSandBarDock.Size = new System.Drawing.Size(0, 329);
			this.leftSandBarDock.TabIndex = 0;
			// 
			// rightSandBarDock
			// 
			this.rightSandBarDock.Dock = System.Windows.Forms.DockStyle.Right;
			this.rightSandBarDock.Location = new System.Drawing.Point(824, 47);
			this.rightSandBarDock.Manager = this.sandBarManager;
			this.rightSandBarDock.Name = "rightSandBarDock";
			this.rightSandBarDock.Size = new System.Drawing.Size(0, 329);
			this.rightSandBarDock.TabIndex = 1;
			// 
			// topSandBarDock
			// 
			this.topSandBarDock.Controls.Add(this.menuBar1);
			this.topSandBarDock.Controls.Add(this.toolBar1);
			this.topSandBarDock.Dock = System.Windows.Forms.DockStyle.Top;
			this.topSandBarDock.Location = new System.Drawing.Point(0, 0);
			this.topSandBarDock.Manager = this.sandBarManager;
			this.topSandBarDock.Name = "topSandBarDock";
			this.topSandBarDock.Size = new System.Drawing.Size(824, 47);
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
			this.menuBar1.Guid = new System.Guid("53385c5b-0171-4b5c-8952-6532dee0c99e");
			this.menuBar1.Location = new System.Drawing.Point(2, 0);
			this.menuBar1.Name = "menuBar1";
			this.menuBar1.Size = new System.Drawing.Size(822, 24);
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
			this.toolBar1.Guid = new System.Guid("ed97398b-31e0-472f-989f-9fd6fbf9d484");
			this.toolBar1.Location = new System.Drawing.Point(2, 24);
			this.toolBar1.Name = "toolBar1";
			this.toolBar1.Size = new System.Drawing.Size(25, 23);
			this.toolBar1.TabIndex = 1;
			this.toolBar1.Text = "toolBar1";
			// 
			// sandDockManager
			// 
			this.sandDockManager.OwnerForm = this;
			this.sandDockManager.Renderer = new TD.SandDock.Rendering.Office2003Renderer();
			// 
			// leftSandDock
			// 
			this.leftSandDock.Dock = System.Windows.Forms.DockStyle.Left;
			this.leftSandDock.Guid = new System.Guid("54e6ba6b-4e12-473f-bb62-27faa6e6a5bf");
			this.leftSandDock.LayoutSystem = new TD.SandDock.SplitLayoutSystem(250, 400);
			this.leftSandDock.Location = new System.Drawing.Point(0, 47);
			this.leftSandDock.Manager = this.sandDockManager;
			this.leftSandDock.Name = "leftSandDock";
			this.leftSandDock.Size = new System.Drawing.Size(0, 329);
			this.leftSandDock.TabIndex = 4;
			// 
			// rightSandDock
			// 
			this.rightSandDock.Dock = System.Windows.Forms.DockStyle.Right;
			this.rightSandDock.Guid = new System.Guid("e418ed79-77ed-46ad-b05a-6df7e33e3d50");
			this.rightSandDock.LayoutSystem = new TD.SandDock.SplitLayoutSystem(250, 400);
			this.rightSandDock.Location = new System.Drawing.Point(824, 47);
			this.rightSandDock.Manager = this.sandDockManager;
			this.rightSandDock.Name = "rightSandDock";
			this.rightSandDock.Size = new System.Drawing.Size(0, 329);
			this.rightSandDock.TabIndex = 5;
			// 
			// bottomSandDock
			// 
			this.bottomSandDock.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.bottomSandDock.Guid = new System.Guid("ab458e69-9139-4278-a38f-d1b3c02245b1");
			this.bottomSandDock.LayoutSystem = new TD.SandDock.SplitLayoutSystem(250, 400);
			this.bottomSandDock.Location = new System.Drawing.Point(0, 376);
			this.bottomSandDock.Manager = this.sandDockManager;
			this.bottomSandDock.Name = "bottomSandDock";
			this.bottomSandDock.Size = new System.Drawing.Size(824, 0);
			this.bottomSandDock.TabIndex = 6;
			// 
			// topSandDock
			// 
			this.topSandDock.Dock = System.Windows.Forms.DockStyle.Top;
			this.topSandDock.Guid = new System.Guid("ce745625-08ab-489f-bc70-4641d0341748");
			this.topSandDock.LayoutSystem = new TD.SandDock.SplitLayoutSystem(250, 400);
			this.topSandDock.Location = new System.Drawing.Point(0, 47);
			this.topSandDock.Manager = this.sandDockManager;
			this.topSandDock.Name = "topSandDock";
			this.topSandDock.Size = new System.Drawing.Size(824, 0);
			this.topSandDock.TabIndex = 7;
			// 
			// statusBar
			// 
			this.statusBar.Location = new System.Drawing.Point(0, 376);
			this.statusBar.Name = "statusBar";
			this.statusBar.Size = new System.Drawing.Size(824, 22);
			this.statusBar.TabIndex = 8;
			this.statusBar.Text = "statusBar1";
			// 
			// documentContainer
			// 
			this.documentContainer.Guid = new System.Guid("3a9e7f87-b0d9-4fe2-9218-692edde57bbc");
			this.documentContainer.LayoutSystem = new TD.SandDock.SplitLayoutSystem(250, 400);
			this.documentContainer.Location = new System.Drawing.Point(0, 47);
			this.documentContainer.Manager = null;
			this.documentContainer.Name = "documentContainer";
			this.documentContainer.Renderer = new TD.SandDock.Rendering.Office2003Renderer();
			this.documentContainer.Size = new System.Drawing.Size(824, 329);
			this.documentContainer.TabIndex = 9;
			// 
			// MainWindowForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(824, 398);
			this.Controls.Add(this.documentContainer);
			this.Controls.Add(this.leftSandDock);
			this.Controls.Add(this.rightSandDock);
			this.Controls.Add(this.bottomSandDock);
			this.Controls.Add(this.topSandDock);
			this.Controls.Add(this.leftSandBarDock);
			this.Controls.Add(this.rightSandBarDock);
			this.Controls.Add(this.bottomSandBarDock);
			this.Controls.Add(this.topSandBarDock);
			this.Controls.Add(this.statusBar);
			this.Name = "MainWindowForm";
			this.Text = "MainWindowForm";
			this.topSandBarDock.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion
	}
}
