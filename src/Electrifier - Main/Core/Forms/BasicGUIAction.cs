//	<file>
//		<copyright see="www.electrifier.org"/>
//		<license   see="www.electrifier.org"/>
//		<owner     name="Thorsten Jung" email="taj@electrifier.org"/>
//		<version   value="$Id: ExtTreeView.cs,v 1.8 2004/09/10 15:21:53 taj bender Exp $"/>
//	</file>

using System;

namespace Electrifier.Core.Forms {
	/// <summary>
	/// Zusammenfassung für BasicGUIAction.
	/// </summary>
	public class BasicGUIAction : IGUIAction {
		protected Guid guid       = Guid.Empty;
		public    Guid Guid       { get { return guid; } }
		protected bool enabled    = false;
		public    bool Enabled    { get { return enabled; } }
		protected int  imageIndex = -1;
		public    int  ImageIndex { get { return imageIndex; } }

		public BasicGUIAction(Guid guid, bool enabled, int imageIndex, ExecutionEventHandler executionEventHandler) {
			this.guid       = guid;
			this.enabled    = enabled;
			this.imageIndex = imageIndex;
			Execution      += executionEventHandler;

			// TODO: Register by ActionManager!
		}

		protected virtual void OnExecution(object source, ExecutionEventArgs e) {
			if(Execution != null) {
				Execution(source, e);
			}
		}

		protected virtual void OnEnabledChanged(object source, EnabledChangedEventArgs e) {
			if(EnabledChanged != null) {
				EnabledChanged(source, e);
			}
		}

		#region IGUIAction Member
		public void Execute(object sender) {
			OnExecution(sender, new ExecutionEventArgs(null));
		}

		public void Enable(object sender) {
			enabled = true;

			OnEnabledChanged(sender, new EnabledChangedEventArgs(true));
		}

		public void Disable(object sender) {
			enabled = false;

			OnEnabledChanged(sender, new EnabledChangedEventArgs(true));
		}

		public event Electrifier.Core.Forms.ExecutionEventHandler      Execution;

		public event Electrifier.Core.Forms.EnabledChangedEventHandler EnabledChanged;
		#endregion
	}
}
