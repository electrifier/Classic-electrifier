using System;

// electrifier uses

namespace electrifier.Core.Controls.ActionControls {
	/// <summary>
	/// Zusammenfassung für ExtMenuBarItem.
	/// </summary>
	public class ActionMenuButtonItem {
		private IAction action = null;
		public  IAction Action { get { return action; } }

		public ActionMenuButtonItem(IViewableAction action) {
			this.action            = action;
			action.EnabledChanged += new EnabledChangedEventHandler(action_EnabledChanged);
/* TODO: RELAUNCH
            Activate              += new EventHandler(ExtMenuButtonItem_Activate);

			// TODO: Get all other properties
			Text            = action.Id.Substring(action.Id.LastIndexOf(".") + 1);
*/
		}

		public void action_EnabledChanged(object sender, EnabledChangedEventArgs e) {
/* TODO: RELAUNCH
            Enabled = e.Enabled;
*/
		}

		private void ExtMenuButtonItem_Activate(object sender, EventArgs e) {
			action.Execute(sender);
		}
	}
}
