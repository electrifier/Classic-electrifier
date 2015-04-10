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
