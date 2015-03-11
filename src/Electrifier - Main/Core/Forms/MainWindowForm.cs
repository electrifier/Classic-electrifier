//	<file>
//		<copyright see="www.electrifier.org"/>
//		<license   see="www.electrifier.org"/>
//		<owner     name="Thorsten Jung" email="taj@electrifier.org"/>
//		<version   value="$Id: IService.cs,v 1.2 2004/08/09 20:50:38 jung2t Exp $"/>
//	</file>

using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

//using RibbonLib;
using RibbonLib.Controls;
using RibbonLib.Controls.Events;
//using RibbonLib.Interop;

using Electrifier.Core;
using Electrifier.Core.Controls.ToolBars;
using Electrifier.Core.Forms.DockControls;

namespace Electrifier.Core.Forms {
    public enum RibbonMarkupCommands : uint {
        cmdTab = 1012,
        cmdGroup = 1015,
        cmdButtonOne = 1008,
        cmdButtonTwo = 1009,
        cmdButtonThree = 1010,
    }

    public struct LastKnownFormState {
        public FormWindowState FormWindowState;
        public Point Location;
        public Size Size;        
    }

    /// <summary>
	/// Zusammenfassung für MainWindowForm.
	/// </summary>
	public partial class MainWindowForm : Form, IPersistentForm, IDockControlContainer {
		protected Guid                     guid                    = Guid.NewGuid();
		public    Guid                     Guid                    { get { return guid; } }
		protected IPersistentFormContainer persistentFormContainer = null;
		public    IPersistentFormContainer PersistentFormContainer { get { return persistentFormContainer; } }
		protected ArrayList                dockControlList         = new ArrayList();

        private RibbonTab _ribbonTab;
        private RibbonButton _ribbonButton1;

        protected LastKnownFormState lastKnownFormState;


		public MainWindowForm() {
			InitializeComponent();

			this.Icon = AppContext.Icon;

            this._ribbonTab = new RibbonTab(this.ribbon1, (uint)RibbonMarkupCommands.cmdTab);
            this._ribbonButton1 = new RibbonButton(this.ribbon1, (uint)RibbonMarkupCommands.cmdButtonOne);
            this._ribbonButton1.ExecuteEvent += new EventHandler<ExecuteEventArgs>(newShellBrowserToolStripMenuItem_Click);

            // TODO: RELAUNCH: Test-Code...
			FolderBarDockControl folderBarDockControl = new FolderBarDockControl();
			folderBarDockControl.Show(this.dockPanel, WeifenLuo.WinFormsUI.Docking.DockState.DockLeftAutoHide);

            this.Resize += new System.EventHandler(this.MainWindowForm_Resize);
            this.LocationChanged += new System.EventHandler(this.MainWindowForm_LocationChanged);
		}

        public void MainWindowForm_Resize(object sender, EventArgs e) {
            this.lastKnownFormState.FormWindowState = this.WindowState;

            if (this.lastKnownFormState.FormWindowState == FormWindowState.Normal)
                this.lastKnownFormState.Size = this.Size;
        }

        public void MainWindowForm_LocationChanged(object sender, EventArgs e) {
            if (this.WindowState == FormWindowState.Normal)
                this.lastKnownFormState.Location = this.Location;
        }


		private void MainWindowForm_Load(object sender, EventArgs e) {

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
            XmlNode mainWindowNode = targetXmlDocument.CreateElement(this.GetType().FullName);
            XmlAttribute guidAttr = targetXmlDocument.CreateAttribute("Guid");
            guidAttr.Value = this.guid.ToString();
            XmlAttribute leftAttr = targetXmlDocument.CreateAttribute("Left");
            leftAttr.Value = this.lastKnownFormState.Location.X.ToString();
            XmlAttribute topAttr = targetXmlDocument.CreateAttribute("Top");
            topAttr.Value = this.lastKnownFormState.Location.Y.ToString();
            XmlAttribute widthAttr = targetXmlDocument.CreateAttribute("Width");
            widthAttr.Value = this.lastKnownFormState.Size.Width.ToString();
            XmlAttribute heightAttr = targetXmlDocument.CreateAttribute("Height");
            heightAttr.Value = this.lastKnownFormState.Size.Height.ToString();
            XmlAttribute windowStateAttr = targetXmlDocument.CreateAttribute("WindowState");
            windowStateAttr.Value = this.WindowState.ToString();
            mainWindowNode.Attributes.Append(guidAttr);
            mainWindowNode.Attributes.Append(leftAttr);
			mainWindowNode.Attributes.Append(topAttr);
			mainWindowNode.Attributes.Append(widthAttr);
			mainWindowNode.Attributes.Append(heightAttr);
            mainWindowNode.Attributes.Append(windowStateAttr);
			
			// Append persistence information for each hosted DockControl
			XmlNode dockControlsNode = targetXmlDocument.CreateElement("DockedControls");
			foreach (IDockControl dockControl in dockControlList) {
				dockControlsNode.AppendChild(dockControl.CreatePersistenceInfo(targetXmlDocument));
			}
			mainWindowNode.AppendChild(dockControlsNode);

			return mainWindowNode;
		}

		public void ApplyPersistenceInfo(XmlNode persistenceInfo) {
			// Apply persistence information to main window Form
			this.guid = new Guid(persistenceInfo.Attributes.GetNamedItem("Guid").Value);
			this.Left = int.Parse(persistenceInfo.Attributes.GetNamedItem("Left").Value);
			this.Top = int.Parse(persistenceInfo.Attributes.GetNamedItem("Top").Value);
			this.Width = int.Parse(persistenceInfo.Attributes.GetNamedItem("Width").Value);
			this.Height = int.Parse(persistenceInfo.Attributes.GetNamedItem("Height").Value);

            switch(persistenceInfo.Attributes.GetNamedItem("WindowState").Value.ToUpper()) {
                case "MAXIMIZED":
                    this.WindowState = FormWindowState.Maximized;
                    break;
                case "MINIMIZED":
                    this.WindowState = FormWindowState.Minimized;
                    break;
                default:
                    this.WindowState = FormWindowState.Normal;
                    break;
            }


			// Apply persistence information for each hosted DockControl
			XmlNode dockControlsNode = persistenceInfo.SelectSingleNode("DockedControls");
			foreach (XmlNode dockControlNode in dockControlsNode.ChildNodes) {
				Type dockControlType = Type.GetType(dockControlNode.LocalName);

				if ((dockControlType != null) && (dockControlType.GetInterface("IDockControl") != null)) {
					IDockControl dockControl = Activator.CreateInstance(dockControlType) as IDockControl;

					dockControl.ApplyPersistenceInfo(dockControlNode);
					dockControl.AttachToDockControlContainer(this);

					// TODO: Hard coded shit follows :-)
					if (dockControlType.Equals(typeof(ShellBrowserDockControl))) {
						ShellBrowserDockControl shbrwsr = dockControl as ShellBrowserDockControl;

						shbrwsr.BrowsingAddressChanged += new BrowsingAddressChangedEventHandler(shbrwsr_BrowsingAddressChanged);
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
			if (this.persistentFormContainer == null) {
				this.persistentFormContainer = persistentFormContainer;
				persistentFormContainer.AttachPersistentForm(this);
			} else {
				throw new InvalidOperationException("IPersistentFormContainer already set!");
			}
		}
		#endregion

		#region IDockControlContainer Member
		public void AttachDockControl(IDockControl dockControl) {
			if (!dockControlList.Contains(dockControl)) {
				dockControlList.Add(dockControl);
			} else {
				throw new ArgumentException("Given DockControl instance already in list of hosted DockControls", "dockControl");
			}
		}

		public void DetachDockControl(IDockControl dockControl) {
			if (dockControlList.Contains(dockControl)) {
				dockControlList.Remove(dockControl);
			} else {
				throw new ArgumentException("Given DockControl instance not in list of hosted DockControls", "dockControl");
			}
		}
		#endregion



	}
}
