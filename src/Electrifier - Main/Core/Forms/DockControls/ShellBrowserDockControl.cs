//	<file>
//		<copyright see="www.electrifier.org"/>
//		<license   see="www.electrifier.org"/>
//		<owner     name="Thorsten Jung" email="taj@electrifier.org"/>
//		<version   value="$Id: BasicShellObject.cs,v 1.13 2004/09/10 20:30:33 taj bender Exp $"/>
//	</file>

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using TD.SandDock;

using Electrifier.Core.Shell32.Controls;
using Electrifier.Win32API;

namespace Electrifier.Core.Forms.DockControls {
	/// <summary>
	/// Zusammenfassung für ShellBrowserDockControl.
	/// </summary>
	public class ShellBrowserDockControl : DockControl, IDockControl, IPersistent {
		protected ShellTreeView         shellTreeView        = null;
		protected ShellListView         shellListView        = null;
		protected Splitter              splitter             = null;
		protected IDockControlContainer dockControlContainer = null;

		public ShellBrowserDockControl() : this(Guid.NewGuid()) { }

		public ShellBrowserDockControl(Guid guid) : base() {
			// Initialize the underlying DockControl
			Guid = guid;
			Name = "ShellBrowserDockControl." + Guid.ToString();
			Text = Name;

			// Initialize ShellTreeView
			shellTreeView              = new ShellTreeView(ShellAPI.CSIDL.DESKTOP);
			shellTreeView.Dock         = DockStyle.Left;
			shellTreeView.Size         = new Size(256, Height);
			shellTreeView.AfterSelect +=new TreeViewEventHandler(shellTreeView_AfterSelect);

			// Initialize ShellListView
			shellListView      = new ShellListView();
			shellListView.Dock = DockStyle.Fill;
			shellListView.View = View.Details;
			shellListView.Columns.Add("Name", 256, HorizontalAlignment.Left);
			shellListView.Columns.Add("Size",  80, HorizontalAlignment.Right);

			// Initialize Splitter
			splitter      = new Splitter();
			splitter.Dock = DockStyle.Left;
			splitter.Size = new Size(4, Height);

			// Add the controls from right to left
			Controls.AddRange(new Control[] { shellListView, splitter, shellTreeView });
		}

		// TODO: Dispose when closed!!!
		// http://www.divil.co.uk/net/forums/thread.aspx?id=386
		// You can use the DocumentClosing event or the Closing or Closed events of the DockControl in question. When you dispose it, the form within will also be disposed.

		private void shellTreeView_AfterSelect(object sender, TreeViewEventArgs e) {
			// TODO: TreeViewEventArgs.Node => shellTreeViewNode
			// TODO: ShellTreeView.SelectedNode => shellTreeViewNode
			this.Cursor = Cursors.WaitCursor;
			shellListView.SetBrowsingFolder(sender, (shellTreeView.SelectedNode as ShellTreeViewNode).AbsolutePIDL);
			Text = (shellTreeView.SelectedNode as ShellTreeViewNode).DisplayName;
			this.Cursor = Cursors.Default;
		}

		#region IDockControl Member
		public void AttachToDockControlContainer(IDockControlContainer dockControlContainer) {
			if(this.dockControlContainer == null) {
				this.dockControlContainer = dockControlContainer;
				dockControlContainer.AttachDockControl(this);
			} else {
				throw new InvalidOperationException("IDockControlContainer already set!");
			}
		}

		#endregion

		#region IPersistent Member

		public System.Xml.XmlNode CreatePersistenceInfo(System.Xml.XmlDocument targetXmlDocument) {
			XmlNode      dockControlNode = targetXmlDocument.CreateElement(this.GetType().FullName);
			XmlAttribute guidAttr        = targetXmlDocument.CreateAttribute("Guid");
			guidAttr.Value               = Guid.ToString();
			dockControlNode.Attributes.Append(guidAttr);

			return dockControlNode;
		}

		public void ApplyPersistenceInfo(System.Xml.XmlNode persistenceInfo) {
			Guid = new Guid(persistenceInfo.Attributes.GetNamedItem("Guid").Value);

			Name = "ShellBrowserDockControl." + Guid.ToString();		// TODO: TestCode
			Text = Name;															// TODO: TestCode
		}

		#endregion
	}
}
