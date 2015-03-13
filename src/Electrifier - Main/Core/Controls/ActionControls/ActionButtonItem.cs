//	<file>
//		<copyright see="www.electrifier.org"/>
//		<license   see="www.electrifier.org"/>
//		<owner     name="Thorsten Jung" email="taj@electrifier.org"/>
//		<version   value="$Id: ExtTreeViewNode.cs,v 1.14 2004/09/10 15:21:53 taj bender Exp $"/>
//	</file>

// Foreign uses
using System;

// Electrifier uses

namespace Electrifier.Core.Controls.ActionControls {
	/// <summary>
	/// Zusammenfassung für ExtButtonItem.
	/// </summary>
	public class ActionButtonItem {
		private IAction action = null;
		public  IAction Action { get { return action; } }

		public ActionButtonItem(IViewableAction action) {
			this.action = action;

			action.EnabledChanged += new EnabledChangedEventHandler(action_EnabledChanged);
		}

		public void action_EnabledChanged(object sender, EnabledChangedEventArgs e) {
/* TODO: RELAUNCH
            Enabled = e.Enabled;
 */
		}
	}
}
