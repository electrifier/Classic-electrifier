using System;

using electrifier.Win32API;

namespace electrifier.Core.Controls {
	/// <summary>
	/// Zusammenfassung für IExtTreeViewNode.
	/// </summary>
	public interface IExtTreeViewNode {
		void HasBeenAddedToTreeViewBy(IExtTreeViewNodeCollection sender);
		WinAPI.IDataObject GetIDataObject();
		WinAPI.IDropTarget GetIDropTarget();
	}
}
