//	<file>
//		<copyright see="www.electrifier.org"/>
//		<license   see="www.electrifier.org"/>
//		<owner     name="Thorsten Jung" email="taj@electrifier.org"/>
//		<version   value="$Id: IService.cs,v 1.2 2004/08/09 20:50:38 jung2t Exp $"/>
//	</file>

using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Xml;

using Electrifier.Core;
using Electrifier.Core.Controls.ToolBars;
using Electrifier.Core.Forms.DockControls;

namespace Electrifier.Core.Forms {
	/// <summary>
	/// Zusammenfassung für MainWindowForm.
	/// </summary>
	public partial class MainWindowForm : Form, IPersistentForm, IDockControlContainer {
		protected Guid                     guid                    = Guid.NewGuid();
		public    Guid                     Guid                    { get { return guid; } }
		protected IPersistentFormContainer persistentFormContainer = null;
		public    IPersistentFormContainer PersistentFormContainer { get { return persistentFormContainer; } }
		protected ArrayList                dockControlList         = new ArrayList();

		public MainWindowForm() {
			InitializeComponent();

			this.Icon = AppContext.Icon;
		}

		private void shbrwsr_BrowsingAddressChanged(object source, EventArgs e) {
			ShellBrowserDockControl shbrwsr = source as ShellBrowserDockControl;

			/* TODO: RELAUNCH: Commented out
			if(shbrwsr != null) {
				this.toolBar1.Address = shbrwsr.BrowsingAddress;
			}
			*/
		}

		private void newShellBrowserToolStripMenuItem_Click(object sender, EventArgs e) {
			ShellBrowserDockControl shellBrowserDockControl = new ShellBrowserDockControl();

			if (this.dockPanel.DocumentStyle == WeifenLuo.WinFormsUI.Docking.DocumentStyle.SystemMdi) {
				shellBrowserDockControl.Show();
				shellBrowserDockControl.MdiParent = this;
			} else {
				shellBrowserDockControl.Show(this.dockPanel, WeifenLuo.WinFormsUI.Docking.DockState.Document);
			}
		}

		private void closeToolStripMenuItem_Click(object sender, EventArgs e) {
			this.Close();
		}

		#region IPersistentForm Member
		public XmlNode CreatePersistenceInfo(XmlDocument targetXmlDocument) {
			// Create persistence information for main window Form
			XmlNode      mainWindowNode = targetXmlDocument.CreateElement(this.GetType().FullName);
			XmlAttribute guidAttr       = targetXmlDocument.CreateAttribute("Guid");
			guidAttr.Value              = this.guid.ToString();
			XmlAttribute leftAttr       = targetXmlDocument.CreateAttribute("Left");
			leftAttr.Value              = this.Left.ToString();
			XmlAttribute topAttr        = targetXmlDocument.CreateAttribute("Top");
			topAttr.Value               = this.Top.ToString();
			XmlAttribute widthAttr      = targetXmlDocument.CreateAttribute("Width");
			widthAttr.Value             = this.Width.ToString();
			XmlAttribute heightAttr     = targetXmlDocument.CreateAttribute("Height");
			heightAttr.Value            = this.Height.ToString();
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

			return mainWindowNode;
		}

		public void ApplyPersistenceInfo(XmlNode persistenceInfo) {
			// Apply persistence information to main window Form
			this.guid   = new  Guid(persistenceInfo.Attributes.GetNamedItem("Guid").Value);
			this.Left   = int.Parse(persistenceInfo.Attributes.GetNamedItem("Left").Value);
			this.Top    = int.Parse(persistenceInfo.Attributes.GetNamedItem("Top").Value);
			this.Width  = int.Parse(persistenceInfo.Attributes.GetNamedItem("Width").Value);
			this.Height = int.Parse(persistenceInfo.Attributes.GetNamedItem("Height").Value);

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
	}
}
