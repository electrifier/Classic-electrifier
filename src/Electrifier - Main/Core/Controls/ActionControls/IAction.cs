//	<file>
//		<copyright see="www.electrifier.org"/>
//		<license   see="www.electrifier.org"/>
//		<owner     name="Thorsten Jung" email="taj@electrifier.org"/>
//		<version   value="$Id: ExtTreeView.cs,v 1.8 2004/09/10 15:21:53 taj bender Exp $"/>
//	</file>

using System;
using System.Windows.Forms;

namespace Electrifier.Core.Controls {
	/// <summary>
	/// ExecutionEventArgs-class and ExecutionEventHandler delegate
	/// </summary>
	public class ExecutionEventArgs : EventArgs {
		private object tagData = null;
		public  object TagData { get { return tagData; } }

		public ExecutionEventArgs(object tagData) : base() {
			this.tagData = tagData;
		}
	}

	public delegate void ExecutionEventHandler(object source, ExecutionEventArgs e);

	/// <summary>
	/// EnabledChangedEventArgs-class and EnabledChangedEventHandler delegate
	/// </summary>
	public class EnabledChangedEventArgs : EventArgs {
		private bool enabled;
		public  bool Enabled { get { return enabled; } }

		public EnabledChangedEventArgs(bool enabled) : base() {
			this.enabled = enabled;
		}
	}

	public delegate void EnabledChangedEventHandler(object source, EnabledChangedEventArgs e);

	/// <summary>
	/// Zusammenfassung für IGUIAction.
	/// </summary>
	public interface IAction {
		// Properties
		string   Id          { get; }
		bool     Enabled     { get; }
		Shortcut Shortcut    { get; }
		// TODO: ButtonItem.importance property!

		// Methods
		void Execute(object sender);
		void Enable (object sender);
		void Disable(object sender);

		// Events
		event ExecutionEventHandler      Execution;
		event EnabledChangedEventHandler EnabledChanged;
	}
}
