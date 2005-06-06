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
	/// Zusammenfassung für ShellTreeViewDockControl.
	/// </summary>
	public class FolderBarDockControl : DockControl {
		protected ShellTreeView shellTreeView = null;

		public FolderBarDockControl() : base(){
			// Initialize the underlying DockControl
			Guid = new Guid("{2B552F10-0847-44b2-A244-D595B7DDD1AE}");
			Name = "FolderBarDockControl." + Guid.ToString();
			Text = "FolderBar";

			// Initialize ShellTreeView
			shellTreeView               = new ShellTreeView(ShellAPI.CSIDL.DESKTOP);
			shellTreeView.Dock          = DockStyle.Fill;
			shellTreeView.BorderStyle   = System.Windows.Forms.BorderStyle.None;
			shellTreeView.ShowRootLines = false;

			// Add the controls
			Controls.Add(shellTreeView);
		}

		// TODO: Dispose when closed!!!
		// http://www.divil.co.uk/net/forums/thread.aspx?id=386
		// You can use the DocumentClosing event or the Closing or Closed events of the DockControl in question. When you dispose it, the form within will also be disposed.
	}
}
