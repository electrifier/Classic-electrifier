using System;
using Electrifier.Win32API;

namespace Electrifier.Core.Controls {
	/// <summary>
	/// Zusammenfassung für IExtListviewItem.
	/// </summary>
	public interface IExtListViewItem {
		char[]       Text       { get; }
		int          ImageIndex { get; }
		int          ItemIndent { get; }
		int          Index      { get; }
		IExtListView ListView   { get; }
		void   AddToIExtListView(IExtListView listView, int index);
		WinAPI.IDataObject GetIDataObject();
		WinAPI.IDropTarget GetIDropTarget();
	}
}
