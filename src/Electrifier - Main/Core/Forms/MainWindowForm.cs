using System;
using System.Collections;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using System.Xml;

//using RibbonLib;
using RibbonLib.Controls;
using RibbonLib.Controls.Events;
//using RibbonLib.Interop;
using WeifenLuo.WinFormsUI.Docking;

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
	public partial class MainWindowForm : Form, IPersistentForm {
		protected Guid guid = Guid.NewGuid();
		public Guid Guid { get { return guid; } }
		protected IPersistentFormContainer persistentFormContainer = null;
		public IPersistentFormContainer PersistentFormContainer { get { return persistentFormContainer; } }
		protected ArrayList dockControlList = new ArrayList();

		private RibbonButton _cmdBtnApp_OpenNewShellBrowserPanel;
		private RibbonButton _cmdBtnApp_Close;
		private RibbonTab _cmdTabHome;

		protected LastKnownFormState _lastKnownFormState;

		public MainWindowForm() : base() {
			InitializeComponent();

			this.Icon = AppContext.Icon;

			this._cmdBtnApp_OpenNewShellBrowserPanel = new RibbonButton(this._mainRibbon, (uint)RibbonMarkupCommands.cmdBtnApp_OpenNewShellBrowserPanel);
			this._cmdBtnApp_OpenNewShellBrowserPanel.ExecuteEvent += new EventHandler<ExecuteEventArgs>(cmdBtnApp_OpenNewShellBrowserPanel_ExecuteEvent);
			this._cmdBtnApp_Close = new RibbonButton(this._mainRibbon, (uint)RibbonMarkupCommands.cmdBtnApp_Close);
			this._cmdBtnApp_Close.ExecuteEvent += new EventHandler<ExecuteEventArgs>(cmdBtnApp_Close_ExecuteEvent);
			this._cmdTabHome = new RibbonTab(this._mainRibbon, (uint)RibbonMarkupCommands.cmdTabHome);

			this.Resize += new System.EventHandler(this.MainWindowForm_Resize);
			this.LocationChanged += new System.EventHandler(this.MainWindowForm_LocationChanged);
		}

		public MainWindowForm(XmlTextReader xmlReader) : this() {
			this.ApplyPersistenceInfo(xmlReader);
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
			ShellBrowserDockContent shbrwsr = source as ShellBrowserDockContent;

			/* TODO: RELAUNCH: Commented out
			if(shbrwsr != null) {
				this.toolBar1.Address = shbrwsr.BrowsingAddress;
			}
			*/
		}

		private void cmdBtnApp_OpenNewShellBrowserPanel_ExecuteEvent(object sender, EventArgs e) {
			ShellBrowserDockContent shellBrowserDockControl = new ShellBrowserDockContent();
//			shellBrowserDockControl.AttachToDockControlContainer(this);

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

			this.dockPanel.SaveAsXml(xmlWriter);

			xmlWriter.WriteEndElement(); // this.GetType().Name
		}

		public void ApplyPersistenceInfo(XmlTextReader xmlReader) {
			this.Left = Convert.ToInt32(xmlReader.GetAttribute(@"Left"), CultureInfo.InvariantCulture);
			this.Top = Convert.ToInt32(xmlReader.GetAttribute(@"Top"), CultureInfo.InvariantCulture);
			this.Width = Convert.ToInt32(xmlReader.GetAttribute(@"Width"), CultureInfo.InvariantCulture);
			this.Height = Convert.ToInt32(xmlReader.GetAttribute(@"Height"), CultureInfo.InvariantCulture);

			switch (xmlReader.GetAttribute(@"WindowState").ToUpper()) {
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

			this.dockPanel.LoadFromXml(xmlReader, new DeserializeDockContent(GetContentFromPersistString));
		}

		private IDockContent GetContentFromPersistString(string persistString) {
			if (persistString == typeof(ShellBrowserDockContent).ToString())
				return new ShellBrowserDockContent();
			if (persistString == typeof(FolderBarDockContent).ToString())
				return new FolderBarDockContent();

			return null;
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

	}
}
