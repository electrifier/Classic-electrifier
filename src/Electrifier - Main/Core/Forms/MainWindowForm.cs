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

using electrifier.Core;
using electrifier.Core.Controls.ToolBars;
using electrifier.Core.Forms.DockControls;

namespace electrifier.Core.Forms {
	public enum RibbonMarkupCommands : uint {
		//// Backstage View respectively Application Menu Items //////////////////////////////////////////////////////////////////
		cmdBtnApp_OpenNewWindow = 10000,
		cmdBtnApp_OpenNewShellBrowserPanel = 10001,
		cmdBtnApp_OpenCommandPrompt = 10002,
		cmdBtnApp_OpenWindowsPowerShell = 10003,
		cmdBtnApp_ChangeElectrifierOptions = 10010,
		cmdBtnApp_ChangeFolderAndSearchOptions = 10011,
		cmdBtnApp_Help = 10020,
		cmdBtnApp_Help_AboutElectrifier = 10021,
		cmdBtnApp_Help_AboutWindows = 10025,
		cmdBtnApp_Close = 10030,
		//// Ribbon tabs /////////////////////////////////////////////////////////////////////////////////////////////////////////
		cmdTabHome = 20000,
		cmdTabShare = 30000,
		cmdTabView = 40000,
		//// Command Group One: Clipboard ////////////////////////////////////////////////////////////////////////////////////////
		cmdGrpHomeClipboard = 20100,
		cmdBtnClipboardCut = 20101,
		cmdBtnClipboardCopy = 20102,
		cmdBtnClipboardPaste = 20103,
		//// Command Group Two: Organize /////////////////////////////////////////////////////////////////////////////////////////
		cmdGrpHomeOrganize = 20200,
		cmdBtnOrganizeMoveTo = 20201,
		cmdBtnOrganizeDelete = 20202,
		cmdBtnOrganizeRename = 20203,
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
		protected Guid guid = Guid.NewGuid();
		public Guid Guid { get { return guid; } }
		protected IPersistentFormContainer persistentFormContainer = null;
		public IPersistentFormContainer PersistentFormContainer { get { return persistentFormContainer; } }
		protected ArrayList dockControlList = new ArrayList();

		private RibbonButton _cmdBtnApp_OpenNewShellBrowserPanel;
		private RibbonButton _cmdBtnApp_Close;
		private RibbonTab _cmdTabHome;

		protected LastKnownFormState _lastKnownFormState;

		public MainWindowForm() {
			InitializeComponent();

			this.Icon = AppContext.Icon;

			this._cmdBtnApp_OpenNewShellBrowserPanel = new RibbonButton(this._mainRibbon, (uint)RibbonMarkupCommands.cmdBtnApp_OpenNewShellBrowserPanel);
			this._cmdBtnApp_OpenNewShellBrowserPanel.ExecuteEvent += new EventHandler<ExecuteEventArgs>(cmdBtnApp_OpenNewShellBrowserPanel_ExecuteEvent);
			this._cmdBtnApp_Close = new RibbonButton(this._mainRibbon, (uint)RibbonMarkupCommands.cmdBtnApp_Close);
			this._cmdBtnApp_Close.ExecuteEvent += new EventHandler<ExecuteEventArgs>(cmdBtnApp_Close_ExecuteEvent);
			this._cmdTabHome = new RibbonTab(this._mainRibbon, (uint)RibbonMarkupCommands.cmdTabHome);




			// TODO: RELAUNCH: Test-Code...
			FolderBarDockControl folderBarDockControl = new FolderBarDockControl();
			folderBarDockControl.Show(this.dockPanel, WeifenLuo.WinFormsUI.Docking.DockState.DockLeftAutoHide);

			this.Resize += new System.EventHandler(this.MainWindowForm_Resize);
			this.LocationChanged += new System.EventHandler(this.MainWindowForm_LocationChanged);
		}

		public void MainWindowForm_Resize(object sender, EventArgs e) {
			this._lastKnownFormState.FormWindowState = this.WindowState;

			if (this._lastKnownFormState.FormWindowState == FormWindowState.Normal)
				this._lastKnownFormState.Size = this.Size;
		}

		public void MainWindowForm_LocationChanged(object sender, EventArgs e) {
			if (this.WindowState == FormWindowState.Normal)
				this._lastKnownFormState.Location = this.Location;
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

		private void cmdBtnApp_OpenNewShellBrowserPanel_ExecuteEvent(object sender, EventArgs e) {
			ShellBrowserDockControl shellBrowserDockControl = new ShellBrowserDockControl();
			shellBrowserDockControl.AttachToDockControlContainer(this);

			if (this.dockPanel.DocumentStyle == WeifenLuo.WinFormsUI.Docking.DocumentStyle.SystemMdi) {
				shellBrowserDockControl.Show();
				shellBrowserDockControl.MdiParent = this;
			} else {
				shellBrowserDockControl.Show(this.dockPanel, WeifenLuo.WinFormsUI.Docking.DockState.Document);
			}
		}

		private void cmdBtnApp_Close_ExecuteEvent(object sender, EventArgs e) {
			// Close form asynchronously since we are in a ribbon event 
			// handler, so the ribbon is still in use, and calling Close 
			// will eventually call _ribbon.DestroyFramework(), which is 
			// a big no-no, if you still use the ribbon.
			this.BeginInvoke(new MethodInvoker(this.Close));
		}

		#region IPersistentForm Member

		public void CreatePersistenceInfo(XmlWriter xmlWriter) {
			xmlWriter.WriteStartElement(this.GetType().Name);
			xmlWriter.WriteAttributeString(@"Left", this._lastKnownFormState.Location.X.ToString());
			xmlWriter.WriteAttributeString(@"Top", this._lastKnownFormState.Location.Y.ToString());
			xmlWriter.WriteAttributeString(@"Width", this._lastKnownFormState.Size.Width.ToString());
			xmlWriter.WriteAttributeString(@"Height", this._lastKnownFormState.Size.Height.ToString());
			xmlWriter.WriteAttributeString(@"WindowState", this.WindowState.ToString());

			xmlWriter.WriteStartElement(@"DockedControls");
			this.dockPanel.SaveAsXml(xmlWriter);
			xmlWriter.WriteEndElement(); // DockedControls

			xmlWriter.WriteEndElement(); // this.GetType().Name
		}

		public void ApplyPersistenceInfo(XmlNode persistenceInfo) {
			// Apply persistence information to main window Form
			this.guid = new Guid(persistenceInfo.Attributes.GetNamedItem("Guid").Value);
			this.Left = int.Parse(persistenceInfo.Attributes.GetNamedItem("Left").Value);
			this.Top = int.Parse(persistenceInfo.Attributes.GetNamedItem("Top").Value);
			this.Width = int.Parse(persistenceInfo.Attributes.GetNamedItem("Width").Value);
			this.Height = int.Parse(persistenceInfo.Attributes.GetNamedItem("Height").Value);

			switch (persistenceInfo.Attributes.GetNamedItem("WindowState").Value.ToUpper()) {
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

					// TODO: decide which container!
					// TODO: Hard coded shit follows :-)
					if (dockControlType.Equals(typeof(ShellBrowserDockControl))) {
						ShellBrowserDockControl shellBrowserDockControl = dockControl as ShellBrowserDockControl;
						shellBrowserDockControl.BrowsingAddressChanged += new BrowsingAddressChangedEventHandler(shbrwsr_BrowsingAddressChanged);

						if (this.dockPanel.DocumentStyle == WeifenLuo.WinFormsUI.Docking.DocumentStyle.SystemMdi) {
							shellBrowserDockControl.Show();
							shellBrowserDockControl.MdiParent = this;
						} else {
							shellBrowserDockControl.Show(this.dockPanel, WeifenLuo.WinFormsUI.Docking.DockState.Document);
						}
					}
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
