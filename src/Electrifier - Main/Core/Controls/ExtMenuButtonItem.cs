//	<file>
//		<copyright see="www.electrifier.org"/>
//		<license   see="www.electrifier.org"/>
//		<owner     name="Thorsten Jung" email="taj@electrifier.org"/>
//		<version   value="$Id: ExtTreeViewNode.cs,v 1.14 2004/09/10 15:21:53 taj bender Exp $"/>
//	</file>

// Foreign uses
using System;
using TD.SandBar;

// Electrifier uses
using Electrifier.Core.Forms;

namespace Electrifier.Core.Controls {
	/// <summary>
	/// Zusammenfassung für ExtMenuBarItem.
	/// </summary>
	public class ExtMenuButtonItem : MenuButtonItem {
		private IGUIAction action = null;
		public  IGUIAction Action { get { return action; } }

		public ExtMenuButtonItem(IGUIAction action) {
			this.action            = action;
			action.EnabledChanged += new EnabledChangedEventHandler(action_EnabledChanged);
			Activate              += new EventHandler(ExtMenuButtonItem_Activate);
		}

		public void action_EnabledChanged(object sender, EnabledChangedEventArgs e) {
			Enabled = e.Enabled;
		}

		private void ExtMenuButtonItem_Activate(object sender, EventArgs e) {
			action.Execute(sender);
		}
	}
}
