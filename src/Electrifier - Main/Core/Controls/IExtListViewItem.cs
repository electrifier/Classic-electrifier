//	<file>
//		<copyright see="www.electrifier.org"/>
//		<license   see="www.electrifier.org"/>
//		<owner     name="Thorsten Jung" email="taj@electrifier.org"/>
//		<version   value="$Id: IExtTreeViewNode.cs,v 1.1 2004/08/25 17:59:07 jung2t Exp $"/>
//	</file>

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
