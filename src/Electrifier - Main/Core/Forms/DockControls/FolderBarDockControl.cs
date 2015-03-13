//	<file>
//		<copyright see="www.electrifier.org"/>
//		<license   see="www.electrifier.org"/>
//		<owner     name="Thorsten Jung" email="taj@electrifier.org"/>
//		<version   value="$Id: BasicShellObject.cs,v 1.13 2004/09/10 20:30:33 taj bender Exp $"/>
//	</file>

using System;
using System.Drawing;
using System.Windows.Forms;

using WeifenLuo.WinFormsUI.Docking;

using Electrifier.Core.Shell32.Controls;
using Electrifier.Win32API;


namespace Electrifier.Core.Forms.DockControls {
	/// <summary>
	/// Zusammenfassung für ShellTreeViewDockControl.
	/// </summary>
	public class FolderBarDockControl : DockContent {
		protected ShellTreeView shellTreeView = null;
        protected Guid Guid;

		public FolderBarDockControl() : base() {
			// Initialize the underlying DockControl
			this.Guid = new Guid("{2B552F10-0847-44b2-A244-D595B7DDD1AE}");
			this.Name = "FolderBarDockControl." + Guid.ToString();
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
