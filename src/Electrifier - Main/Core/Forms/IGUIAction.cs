//	<file>
//		<copyright see="www.electrifier.org"/>
//		<license   see="www.electrifier.org"/>
//		<owner     name="Thorsten Jung" email="taj@electrifier.org"/>
//		<version   value="$Id: ExtTreeView.cs,v 1.8 2004/09/10 15:21:53 taj bender Exp $"/>
//	</file>

using System;

namespace Electrifier.Core.Forms {
	public class ExecutionEventArgs : EventArgs {
		private object tagData = null;
		public  object TagData { get { return tagData; } }

		public ExecutionEventArgs(object tagData) {
			this.tagData = tagData;
		}
	}

	public delegate void ExecutionEventHandler(object source, ExecutionEventArgs e);

	/// <summary>
	/// Zusammenfassung für IGUIAction.
	/// </summary>
	public interface IGUIAction {
		// Properties
		Guid Guid              { get; }
		bool IsEnabled         { get; }
//		bool IsSessionSpecific { get; }
		int  ImageIndex        { get; }

		// Methods
		object CreateExecutedEventTagData(object sender);
		void   Execute                   (object sender);
		void   Enable                    (object sender);
		void   Disable                   (object sender);

		// Events
		event ExecutionEventHandler Execution;
		event EventHandler          EnabledChanged;
	}
}
