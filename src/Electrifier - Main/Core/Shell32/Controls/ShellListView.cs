//	<file>
//		<copyright see="www.electrifier.org"/>
//		<license   see="www.electrifier.org"/>
//		<owner     name="Thorsten Jung" email="taj@electrifier.org"/>
//		<version   value="$Id: BasicShellObject.cs,v 1.13 2004/09/10 20:30:33 taj bender Exp $"/>
//	</file>

using System;
using System.Windows.Forms;

using Electrifier.Core.Controls;
using Electrifier.Core.Services;
using Electrifier.Core.Shell32.Services;
using Electrifier.Win32API;

namespace Electrifier.Core.Shell32.Controls {
	/// <summary>
	/// Zusammenfassung für ShellListView.
	/// </summary>
	public class ShellListView : ExtListView, IShellObjectCollection {
		protected static IconManager iconManager    = (IconManager)ServiceManager.Services.GetService(typeof(IconManager));
		protected        IntPtr      browsingFolder = IntPtr.Zero;

		public ShellListView() : base() {
			SmallSystemImageList = iconManager.SmallImageList;
			LargeSystemImageList = iconManager.LargeImageList;
		}

		public void SetBrowsingFolder(object sender, ShellAPI.CSIDL shellObjectCSIDL) {
			SetBrowsingFolder(sender, PIDLManager.CreateFromCSIDL(shellObjectCSIDL), true);
		}

		public void SetBrowsingFolder(object sender, string shellObjectFullPath) {
			SetBrowsingFolder(sender, PIDLManager.CreateFromPathW(shellObjectFullPath), true);
		}

		public void SetBrowsingFolder(object sender, IntPtr shellObjectPIDL) {
			SetBrowsingFolder(sender, shellObjectPIDL, false);
		}

		public void SetBrowsingFolder(object sender, IntPtr shellObjectPIDL, bool pidlSelfCreated) {
			// Update browsingFolder variable
			if(browsingFolder != IntPtr.Zero)
				PIDLManager.Free(browsingFolder);
			browsingFolder = (pidlSelfCreated ? shellObjectPIDL : PIDLManager.Clone(shellObjectPIDL));

			// Get the collection of shell-items and add them
			BasicShellObjectCollection collection = new BasicShellObjectCollection(shellObjectPIDL,
				ShellAPI.SHCONTF.DefaultForListView, ShellObjectPIDLComparer.SortMode.Ascending);
			ListViewItem[]             items      = new ListViewItem[collection.Count];
			int                        index      = 0;

			foreach(BasicShellObject shellObject in collection) {
				items[index++] = new ShellListViewItem(shellObject);
			}

			// Create a file info thread to gather visual info for all items
			IconManager.FileInfoThread fileInfoThread = new IconManager.FileInfoThread(collection);

			Items.Clear();
			Items.AddRange(items);
		}

		#region IShellObjectCollection Member
		public int Count {
			get {
				return Items.Count;
			}
		}

		public IShellObjectCollectionEnumerator GetEnumerator() {
			// TODO:  Implementierung von ShellListView.GetEnumerator hinzufügen
			return null;
		}
		#endregion
	}
}
