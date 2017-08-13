using System;
using System.Windows.Forms;

using electrifier.Core.WindowsShell.Controls;
using electrifier.Win32API;


namespace electrifier.Core.Controls.DockContents {
	/// <summary>
	/// Summary of ShellTreeViewDockControl.
	/// </summary>
	public class FolderBarDockContent : WeifenLuo.WinFormsUI.Docking.DockContent {
		protected ShellTreeView shellTreeView = null;
		protected Guid Guid;

		public FolderBarDockContent() : base() {
			// Initialize the underlying DockControl
			this.Guid = new Guid("{2B552F10-0847-44b2-A244-D595B7DDD1AE}");
			this.Name = "FolderBarDockContent." + this.Guid.ToString();
			this.Text = "FolderBar";

            // Initialize ShellTreeView
            this.shellTreeView = new ShellTreeView(ShellAPI.CSIDL.DESKTOP)
            {
                Dock = DockStyle.Fill,
                BorderStyle = System.Windows.Forms.BorderStyle.None,
                ShowRootLines = false
            };

            // Add the controls
            this.Controls.Add(this.shellTreeView);
		}

		// TODO: Dispose when closed!!!
	}
}
