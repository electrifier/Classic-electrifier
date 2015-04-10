using System;

using Electrifier.Win32API;

namespace Electrifier.Core.Controls {
	/// <summary>
	/// Zusammenfassung für IExtTreeViewNode.
	/// </summary>
	public interface IExtTreeViewNode {
		void HasBeenAddedToTreeViewBy(IExtTreeViewNodeCollection sender);
		WinAPI.IDataObject GetIDataObject();
		WinAPI.IDropTarget GetIDropTarget();
	}
}
