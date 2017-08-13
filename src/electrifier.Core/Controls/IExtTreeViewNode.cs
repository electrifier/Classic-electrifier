using System;

using electrifier.Win32API;

namespace electrifier.Core.Controls {
	/// <summary>
	/// Summary for IExtTreeViewNode.
	/// </summary>
	public interface IExtTreeViewNode {
		void HasBeenAddedToTreeViewBy(IExtTreeViewNodeCollection sender);
		WinAPI.IDataObject GetIDataObject();
		WinAPI.IDropTarget GetIDropTarget();
	}
}
