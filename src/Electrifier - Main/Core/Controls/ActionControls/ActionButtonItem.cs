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

namespace Electrifier.Core.Controls.ActionControls {
	/// <summary>
	/// Zusammenfassung für ExtButtonItem.
	/// </summary>
	public class ActionButtonItem : ButtonItem {
		private IAction action = null;
		public  IAction Action { get { return action; } }

		public ActionButtonItem(IAction action) {
			this.action = action;

			action.EnabledChanged += new EnabledChangedEventHandler(action_EnabledChanged);
		}

		public void action_EnabledChanged(object sender, EnabledChangedEventArgs e) {
			Enabled = e.Enabled;
		}
	}
}
