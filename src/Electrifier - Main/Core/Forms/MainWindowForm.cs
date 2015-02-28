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
using Electrifier.Core.Controls.ToolBars;
using Electrifier.Core.Forms.DockControls;

namespace Electrifier.Core.Forms {
	/// <summary>
	/// Zusammenfassung für MainWindowForm.
	/// </summary>
	public class MainWindowForm : System.Windows.Forms.Form, IPersistentForm, IDockControlContainer {
		protected Guid                     guid                    = Guid.NewGuid();
		public    Guid                     Guid                    { get { return guid; } }
		protected IPersistentFormContainer persistentFormContainer = null;
		public    IPersistentFormContainer PersistentFormContainer { get { return persistentFormContainer; } }
		protected ArrayList                dockControlList         = new ArrayList();
        private ToolStripContainer tscToolStripContainer;
        private MenuStrip mstMenuStrip;
        private ToolStripMenuItem mniFileToolStripMenuItem;
        private ToolStripMenuItem mniFileNewWindowToolStripMenuItem;
    
        public ArrayList DockControlList { get { return dockControlList; } }
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
			this.Text = @"electrifier";
//			documentContainer.Manager = new SandDockManager();
			Icon = AppContext.Icon;

			// Initialize form state regarding to the configuration saved
			// TODO: This is test code only; the default configuration will also be read out from XML
//			ShellBrowserDockControl shellBrowser = new ShellBrowserDockControl();
//			this.documentContainer.AddDocument(shellBrowser);
//			shellBrowser.AttachToDockControlContainer(this);


/* TODO: RELAUNCH: Commented out
            FolderBarDockControl folderBar = new FolderBarDockControl();
			folderBar.Manager = sandDockManager;
			folderBar.Open(DockLocation.Left);
			folderBar.LayoutSystem.Collapsed = true;

 */
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

		#region IPersistentForm Member
		public XmlNode CreatePersistenceInfo(XmlDocument targetXmlDocument) {
			// Create persistence information for main window Form
			XmlNode      mainWindowNode = targetXmlDocument.CreateElement(this.GetType().FullName);
			XmlAttribute guidAttr       = targetXmlDocument.CreateAttribute("Guid");
			guidAttr.Value              = guid.ToString();
			XmlAttribute leftAttr       = targetXmlDocument.CreateAttribute("Left");
			leftAttr.Value              = Left.ToString();
			XmlAttribute topAttr        = targetXmlDocument.CreateAttribute("Top");
			topAttr.Value               = Top.ToString();
			XmlAttribute widthAttr      = targetXmlDocument.CreateAttribute("Width");
			widthAttr.Value             = Width.ToString();
			XmlAttribute heightAttr     = targetXmlDocument.CreateAttribute("Height");
			heightAttr.Value            = Height.ToString();
			mainWindowNode.Attributes.Append(guidAttr);
			mainWindowNode.Attributes.Append(leftAttr);
			mainWindowNode.Attributes.Append(topAttr);
			mainWindowNode.Attributes.Append(widthAttr);
			mainWindowNode.Attributes.Append(heightAttr);

			// Append persistence information for each hosted DockControl
			XmlNode dockControlsNode = targetXmlDocument.CreateElement("DockedControls");
			foreach(IDockControl dockControl in dockControlList) {
				dockControlsNode.AppendChild(dockControl.CreatePersistenceInfo(targetXmlDocument));
			}
			mainWindowNode.AppendChild(dockControlsNode);

            /* TODO: RELAUNCH: Commented out
            // Append persistance information for SandBar and SandDock components
			mainWindowNode.AppendChild(AppContext.CreateXmlNodeFromForeignXmlDocument(targetXmlDocument,
				"SandBarManager", sandBarManager.GetLayout()));
			mainWindowNode.AppendChild(AppContext.CreateXmlNodeFromForeignXmlDocument(targetXmlDocument,
				"SandDockManager", sandDockManager.GetLayout()));
			mainWindowNode.AppendChild(AppContext.CreateXmlNodeFromForeignXmlDocument(targetXmlDocument,
				"DocumentContainer", documentContainer.Manager.GetLayout()));
            */
			return mainWindowNode;
		}

		public void ApplyPersistenceInfo(XmlNode persistenceInfo) {
			// Apply persistence information to main window Form
			guid   = new  Guid(persistenceInfo.Attributes.GetNamedItem("Guid").Value);
			Left   = int.Parse(persistenceInfo.Attributes.GetNamedItem("Left").Value);
			Top    = int.Parse(persistenceInfo.Attributes.GetNamedItem("Top").Value);
			Width  = int.Parse(persistenceInfo.Attributes.GetNamedItem("Width").Value);
			Height = int.Parse(persistenceInfo.Attributes.GetNamedItem("Height").Value);

			// Apply persistence information for each hosted DockControl
			XmlNode dockControlsNode = persistenceInfo.SelectSingleNode("DockedControls");
			foreach(XmlNode dockControlNode in dockControlsNode.ChildNodes) {
				Type dockControlType = Type.GetType(dockControlNode.LocalName);

				if((dockControlType != null ) && (dockControlType.GetInterface("IDockControl") != null)) {
					IDockControl dockControl = Activator.CreateInstance(dockControlType) as IDockControl;

					dockControl.ApplyPersistenceInfo(dockControlNode);
					dockControl.AttachToDockControlContainer(this);

					// TODO: Hard coded shit follows :-)
					if(dockControlType.Equals(typeof(ShellBrowserDockControl))) {
						ShellBrowserDockControl shbrwsr = dockControl as ShellBrowserDockControl;

						shbrwsr.BrowsingAddressChanged +=new BrowsingAddressChangedEventHandler(shbrwsr_BrowsingAddressChanged);
					}


					// TODO: decide which container!
//					documentContainer.AddDocument(dockControl as DockControl);
				} else {
					// TODO: Exception
					MessageBox.Show("Unknown DockControl type specified in configuration file");
				}
			}			

			// Apply persistance information to SandBar and SandDock components
            /* TODO: RELAUNCH: Commented out
            sandBarManager.SetLayout(AppContext.CreateXmlDocumentFromForeignXmlNode(persistenceInfo,
                "SandBarManager"));
            sandDockManager.SetLayout(AppContext.CreateXmlDocumentFromForeignXmlNode(persistenceInfo,
                "SandDockManager"));
            documentContainer.Manager.SetLayout(AppContext.CreateXmlDocumentFromForeignXmlNode(persistenceInfo,
                "DocumentContainer"));
             */
		}

		public void AttachToFormContainer(IPersistentFormContainer persistentFormContainer) {
			if(this.persistentFormContainer == null) {
				this.persistentFormContainer = persistentFormContainer;
				persistentFormContainer.AttachPersistentForm(this);
			} else {
				throw new InvalidOperationException("IPersistentFormContainer already set!");
			}
		}
		#endregion

		#region Vom Windows Form-Designer generierter Code
		/// <summary>
		/// Erforderliche Methode für die Designerunterstützung. 
		/// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
		/// </summary>
		private void InitializeComponent() {
            this.tscToolStripContainer = new System.Windows.Forms.ToolStripContainer();
            this.mstMenuStrip = new System.Windows.Forms.MenuStrip();
            this.mniFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mniFileNewWindowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tscToolStripContainer.TopToolStripPanel.SuspendLayout();
            this.tscToolStripContainer.SuspendLayout();
            this.mstMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // tscToolStripContainer
            // 
            // 
            // tscToolStripContainer.ContentPanel
            // 
            this.tscToolStripContainer.ContentPanel.Size = new System.Drawing.Size(824, 374);
            this.tscToolStripContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tscToolStripContainer.Location = new System.Drawing.Point(0, 0);
            this.tscToolStripContainer.Name = "tscToolStripContainer";
            this.tscToolStripContainer.Size = new System.Drawing.Size(824, 398);
            this.tscToolStripContainer.TabIndex = 0;
            this.tscToolStripContainer.Text = "toolStripContainer1";
            // 
            // tscToolStripContainer.TopToolStripPanel
            // 
            this.tscToolStripContainer.TopToolStripPanel.Controls.Add(this.mstMenuStrip);
            // 
            // mstMenuStrip
            // 
            this.mstMenuStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.mstMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mniFileToolStripMenuItem});
            this.mstMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.mstMenuStrip.Name = "mstMenuStrip";
            this.mstMenuStrip.Size = new System.Drawing.Size(824, 24);
            this.mstMenuStrip.TabIndex = 0;
            this.mstMenuStrip.Text = "menuStrip1";
            // 
            // mniFileToolStripMenuItem
            // 
            this.mniFileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mniFileNewWindowToolStripMenuItem});
            this.mniFileToolStripMenuItem.Name = "mniFileToolStripMenuItem";
            this.mniFileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.mniFileToolStripMenuItem.Text = "&File";
            // 
            // mniFileNewWindowToolStripMenuItem
            // 
            this.mniFileNewWindowToolStripMenuItem.Name = "mniFileNewWindowToolStripMenuItem";
            this.mniFileNewWindowToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.mniFileNewWindowToolStripMenuItem.Text = "&New Window";
            this.mniFileNewWindowToolStripMenuItem.Click += new System.EventHandler(this.mniNewWindowToolStripMenuItem_Click);
            // 
            // MainWindowForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(824, 398);
            this.Controls.Add(this.tscToolStripContainer);
            this.MainMenuStrip = this.mstMenuStrip;
            this.Name = "MainWindowForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "electrifier";
            this.tscToolStripContainer.TopToolStripPanel.ResumeLayout(false);
            this.tscToolStripContainer.TopToolStripPanel.PerformLayout();
            this.tscToolStripContainer.ResumeLayout(false);
            this.tscToolStripContainer.PerformLayout();
            this.mstMenuStrip.ResumeLayout(false);
            this.mstMenuStrip.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

		#region IDockControlContainer Member
		public void AttachDockControl(IDockControl dockControl) {
			if(!dockControlList.Contains(dockControl)) {
				dockControlList.Add(dockControl);
			} else {
				throw new ArgumentException("Given DockControl instance already in list of hosted DockControls", "dockControl");
			}
		}

		public void DetachDockControl(IDockControl dockControl) {
			if(dockControlList.Contains(dockControl)) {
				dockControlList.Remove(dockControl);
			} else {
				throw new ArgumentException("Given DockControl instance not in list of hosted DockControls", "dockControl");
			}
		}
		#endregion

/* TODO: RELAUNCH: Commented out
        private void menuButtonItem1_Activate(object sender, System.EventArgs e) {
			// TODO: Experimental code :-)
			ShellBrowserDockControl shellBrowser = new ShellBrowserDockControl();
//			this.documentContainer.AddDocument(shellBrowser);
			shellBrowser.AttachToDockControlContainer(this);	
		}
 */

		private void shbrwsr_BrowsingAddressChanged(object source, EventArgs e) {
			ShellBrowserDockControl shbrwsr = source as ShellBrowserDockControl;

            /* TODO: RELAUNCH: Commented out
			if(shbrwsr != null) {
				this.toolBar1.Address = shbrwsr.BrowsingAddress;
			}
            */
		}

        private void mniNewWindowToolStripMenuItem_Click(object sender, EventArgs e) {

        }
	}
}
