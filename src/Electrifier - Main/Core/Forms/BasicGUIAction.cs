//	<file>
//		<copyright see="www.electrifier.org"/>
//		<license   see="www.electrifier.org"/>
//		<owner     name="Thorsten Jung" email="taj@electrifier.org"/>
//		<version   value="$Id: ExtTreeView.cs,v 1.8 2004/09/10 15:21:53 taj bender Exp $"/>
//	</file>

using System;
using System.Windows.Forms;

namespace Electrifier.Core.Forms {
	/// <summary>
	/// Zusammenfassung für BasicGUIAction.
	/// </summary>
	public class BasicGUIAction : IGUIAction {
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

		public BasicGUIAction(string id, bool enabled, int imageIndex, ExecutionEventHandler executionEventHandler) {
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

		public event Electrifier.Core.Forms.ExecutionEventHandler      Execution;

		public event Electrifier.Core.Forms.EnabledChangedEventHandler EnabledChanged;
		#endregion
	}
}
