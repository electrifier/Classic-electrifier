using System;
using System.Drawing;
using System.Windows.Forms;

using WeifenLuo.WinFormsUI.Docking;

using electrifier.Core.Shell32.Controls;
using electrifier.Win32API;


namespace electrifier.Core.Forms.DockControls {
	/// <summary>
	/// Summary of ShellTreeViewDockControl.
	/// </summary>
	public class FolderBarDockContent : WeifenLuo.WinFormsUI.Docking.DockContent {
		protected ShellTreeView shellTreeView = null;
        protected Guid Guid;

		public FolderBarDockContent() : base() {
			// Initialize the underlying DockControl
			this.Guid = new Guid("{2B552F10-0847-44b2-A244-D595B7DDD1AE}");
			this.Name = "FolderBarDockContent." + Guid.ToString();
			this.Text = "FolderBar";

			// Initialize ShellTreeView
			this.shellTreeView               = new ShellTreeView(ShellAPI.CSIDL.DESKTOP);
			this.shellTreeView.Dock          = DockStyle.Fill;
			this.shellTreeView.BorderStyle   = System.Windows.Forms.BorderStyle.None;
			this.shellTreeView.ShowRootLines = false;

			// Add the controls
			this.Controls.Add(shellTreeView);
		}

		// TODO: Dispose when closed!!!
	}
}
