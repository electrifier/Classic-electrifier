//	<file>
//		<copyright see="www.electrifier.org"/>
//		<license   see="www.electrifier.org"/>
//		<owner     name="Thorsten Jung" email="taj@electrifier.org"/>
//		<version   value="$Id: ExtTreeView.cs,v 1.8 2004/09/10 15:21:53 taj bender Exp $"/>
//	</file>

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
