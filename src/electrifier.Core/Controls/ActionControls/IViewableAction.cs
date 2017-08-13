using System;

namespace electrifier.Core.Controls.ActionControls {
	/// <summary>
	/// Summary for IViewableAction.
	/// </summary>
	public interface IViewableAction : IAction {
		// Properties
		int      ImageIndex  { get; }
		string   Text        { get; }
		string   ToolTipText { get; }
		bool     BeginGroup  { get; }
	}
}
