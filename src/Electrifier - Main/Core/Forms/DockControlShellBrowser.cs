//	<file>
//		<copyright see="www.electrifier.org"/>
//		<license   see="www.electrifier.org"/>
//		<owner     name="Thorsten Jung" email="taj@electrifier.org"/>
//		<version   value="$Id: BasicShellObject.cs,v 1.13 2004/09/10 20:30:33 taj bender Exp $"/>
//	</file>

using System;
using System.Drawing;
using System.Windows.Forms;

using Electrifier.Core.Shell32.Controls;
using Electrifier.Win32API;
using TD.SandDock;

namespace Electrifier.Core.Forms {
	/// <summary>
	/// Zusammenfassung für ShellBrowserDockControl.
	/// </summary>
	public class DockControlShellBrowser : DockControl {
		protected ShellTreeView shellTreeView = null;
		protected ShellListView shellListView = null;
		protected Splitter      splitter      = null;

		public DockControlShellBrowser() : base() {
			shellTreeView      = new ShellTreeView(ShellAPI.CSIDL.DESKTOP);
			shellListView      = new ShellListView();
			splitter           = new Splitter();
			Control[] controls = { splitter, shellListView, shellTreeView };

//			shellTreeView      = new ShellTreeView(ShellAPI.CSIDL.DESKTOP);
			Guid = new System.Guid("816327AB-FAEE-4834-97E9-24EB9AF3E702");
			Name = "ShellBrowserDockControl";
			Text = "Desktop";

			// Initialize ShellTreeView
			shellTreeView.Dock     = DockStyle.Left;
//			shellTreeView.Location = new Point(0, 0);
			shellTreeView.Size     = new Size(228, Height);

			// Initialize ShellListView
			shellListView.Dock     = DockStyle.Fill;
//			shellListView.Location = new Point(233, 0);
//			shellListView.Size     = new Size(Width - 232, Height);
			shellListView.SetBrowsingFolder(this, ShellAPI.CSIDL.DESKTOP);

			// Initialize Splitter
			splitter.Dock     = DockStyle.Left;
//			splitter.Location = new Point(228, 0);
			splitter.Size     = new Size(5, Height);

//			this.treeView1.Dock = System.Windows.Forms.DockStyle.Left;
//			this.treeView1.ImageIndex = -1;
//			this.treeView1.Location = new System.Drawing.Point(0, 0);
//			this.treeView1.Name = "treeView1";
//			this.treeView1.SelectedImageIndex = -1;
//			this.treeView1.Size = new System.Drawing.Size(192, 359);
//			this.treeView1.TabIndex = 0;
//			// 
//			// splitter1
//			// 
//			this.splitter1.Location = new System.Drawing.Point(192, 0);
//			this.splitter1.Name = "splitter1";
//			this.splitter1.Size = new System.Drawing.Size(3, 359);
//			this.splitter1.TabIndex = 1;
//			this.splitter1.TabStop = false;
//			// 
//			// listView1
//			// 
//			this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
//			this.listView1.Location = new System.Drawing.Point(195, 0);
//			this.listView1.Name = "listView1";
//			this.listView1.Size = new System.Drawing.Size(425, 359);
//			this.listView1.TabIndex = 2;

			Controls.AddRange(controls);
		}
	}
}
