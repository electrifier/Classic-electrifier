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
	public class DockControlFolderBar : DockControl {
		protected ShellTreeView shellTreeView = null;

		public DockControlFolderBar() : base(){
			shellTreeView      = new ShellTreeView(ShellAPI.CSIDL.DESKTOP);

			shellTreeView.Dock = DockStyle.Fill;

			this.Controls.Add(shellTreeView);


			Guid = new System.Guid("724E77B2-8874-438c-8EE9-BB7FA198FB4A");
			Name = "ShellTreeViewDockControl";
			Text = "Folder Bar";

		}
	}
}
