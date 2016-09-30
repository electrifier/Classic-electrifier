using System;
using System.Windows.Forms;

namespace electrifier.Core.Controls.ActionControls {
	/// <summary>
	/// Zusammenfassung für BasicGUIAction.
	/// </summary>
	public class BasicAction : IViewableAction {
		protected string   id;
		public    string   Id          { get { return id; } }
		protected bool     enabled     = false;
		public    bool     Enabled     { get { return enabled; } }
		protected int      imageIndex  = -1;
		public    int      ImageIndex  { get { return imageIndex; } }
		protected string   text;
		public    string   Text        { get { return text; } }
		protected string   toolTipText;
		public    string   ToolTipText { get { return toolTipText; } }
		protected bool     beginGroup  = false;
		public    bool     BeginGroup  { get { return beginGroup; } }
		protected Shortcut shortcut    = Shortcut.None;
		public    Shortcut Shortcut    { get { return shortcut; } }

		public BasicAction(string id, bool enabled, int imageIndex, ExecutionEventHandler executionEventHandler) {
			this.id         = id;
			this.enabled    = enabled;
			this.imageIndex = imageIndex;
			Execution      += executionEventHandler;

			// TODO: Register to ActionManager!
			// TODO: Get texts from LocaleManager!
			// TODO: Importance from ConfigManager!
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

		public event ExecutionEventHandler      Execution;

		public event EnabledChangedEventHandler EnabledChanged;
		#endregion
	}
}
