//	<file>
//		<copyright see="www.electrifier.org"/>
//		<license   see="www.electrifier.org"/>
//		<owner     name="Thorsten Jung" email="taj@electrifier.org"/>
//		<version   value="$Id: BasicShellObject.cs,v 1.13 2004/09/10 20:30:33 taj bender Exp $"/>
//	</file>

using System;
using System.Drawing;
using System.Windows.Forms;
using TD.SandDock;

using Electrifier.Core.Shell32.Controls;
using Electrifier.Win32API;

namespace Electrifier.Core.Forms.DockControls {
	/// <summary>
	/// Zusammenfassung für ShellBrowserDockControl.
	/// </summary>
	public class ShellBrowserDockControl : DockControl {
		protected ShellTreeView shellTreeView = null;
		protected ShellListView shellListView = null;
		protected Splitter      splitter      = null;

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

			// TODO: Test-Code
			shellListView.SetBrowsingFolder(this, ShellAPI.CSIDL.DESKTOP);

			// Initialize Splitter
			splitter      = new Splitter();
			splitter.Dock = DockStyle.Left;
			splitter.Size = new Size(4, Height);

			// Add the controls
			Controls.AddRange(new Control[] { shellListView, splitter, shellTreeView });
		}

		// TODO: Dispose when closed!!!
		// http://www.divil.co.uk/net/forums/thread.aspx?id=386
		// You can use the DocumentClosing event or the Closing or Closed events of the DockControl in question. When you dispose it, the form within will also be disposed.

		private void shellTreeView_AfterSelect(object sender, TreeViewEventArgs e) {
			// TODO: TreeViewEventArgs.Node => shellTreeViewNode
			// TODO: ShellTreeView.SelectedNode => shellTreeViewNode
			shellListView.SetBrowsingFolder(sender, (shellTreeView.SelectedNode as ShellTreeViewNode).AbsolutePIDL);
			Text = (shellTreeView.SelectedNode as ShellTreeViewNode).DisplayName;
		}
	}
}
