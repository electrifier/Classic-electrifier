using System;

namespace Electrifier.Core.Controls.ActionControls {
	/// <summary>
	/// Zusammenfassung für IViewableAction.
	/// </summary>
	public interface IViewableAction : IAction {
		// Properties
		int      ImageIndex  { get; }
		string   Text        { get; }
		string   ToolTipText { get; }
		bool     BeginGroup  { get; }
	}
}
