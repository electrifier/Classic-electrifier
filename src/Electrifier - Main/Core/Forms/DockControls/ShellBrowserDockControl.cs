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

		public ShellBrowserDockControl() : base() {
			shellTreeView = new ShellTreeView(ShellAPI.CSIDL.DESKTOP);
			shellListView = new ShellListView();
			splitter      = new Splitter();

			// Initialize the underlying DockControl
			Guid = System.Guid.NewGuid();
			Name = "ShellBrowserDockControl." + Guid.ToString();
			Text = Name;

			// Initialize ShellTreeView
			shellTreeView.Dock = DockStyle.Left;
			shellTreeView.Size = new Size(256, Height);

			// Initialize ShellListView
			shellListView.Dock = DockStyle.Fill;
			shellListView.View = View.Details;
			shellListView.Columns.Add("Name", 256, HorizontalAlignment.Left);
			shellListView.Columns.Add("Size",  80, HorizontalAlignment.Right);

			// TODO: Test-Code
			shellListView.SetBrowsingFolder(this, ShellAPI.CSIDL.DESKTOP);

			// Initialize Splitter
			splitter.Dock = DockStyle.Left;
			splitter.Size = new Size(4, Height);

			// Add all the controls
			Controls.AddRange(new Control[] { shellListView, splitter, shellTreeView });
		}
	}
}
