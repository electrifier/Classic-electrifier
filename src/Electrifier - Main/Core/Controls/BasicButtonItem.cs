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

namespace Electrifier.Core.Controls {
	/// <summary>
	/// Zusammenfassung für ExtButtonItem.
	/// </summary>
	public class ExtButtonItem : ButtonItem {
		private IGUIAction action = null;
		public  IGUIAction Action { get { return action; } }

		public ExtButtonItem(IGUIAction action) {
			this.action = action;

			action.EnabledChanged += new EnabledChangedEventHandler(action_EnabledChanged);
		}

		public void action_EnabledChanged(object sender, EnabledChangedEventArgs e) {
			Enabled = e.Enabled;
		}
	}
}
