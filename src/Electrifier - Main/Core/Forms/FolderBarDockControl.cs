using System;
using System.Drawing;
using System.Windows.Forms;
using TD.SandDock;

using Electrifier.Core.Shell32.Controls;
using Electrifier.Win32API;


namespace Electrifier.Core.Forms {
	/// <summary>
	/// Zusammenfassung für ShellTreeViewDockControl.
	/// </summary>
	public class FolderBarDockControl : DockControl {
		protected ShellTreeView shellTreeView = null;

		public FolderBarDockControl() : base(){
			shellTreeView      = new ShellTreeView(ShellAPI.CSIDL.DESKTOP);

			shellTreeView.Dock = DockStyle.Fill;

			this.Controls.Add(shellTreeView);


			Guid = new System.Guid("724E77B2-8874-438c-8EE9-BB7FA198FB4A");
			Name = "ShellTreeViewDockControl";
			Text = "Folder Bar";

		}
	}
}
