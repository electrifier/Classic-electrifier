using System;
using System.Windows.Forms;

using Electrifier.Core.Services;
using Electrifier.Core.Shell32.Services;
using Electrifier.Win32API;

namespace Electrifier.Core.Shell32.Controls {
	/// <summary>
	/// Zusammenfassung für ShellListViewItem.
	/// </summary>
	public class ShellListViewItem : ListViewItem, IShellObject {
		protected static IconManager      iconManager      = (IconManager)ServiceManager.Services.GetService(typeof(IconManager));
		protected        BasicShellObject basicShellObject = null;

		public ShellListViewItem(ShellAPI.CSIDL shellObjectCSIDL)
			: this(PIDLManager.CreateFromCSIDL(shellObjectCSIDL), true) {}

		public ShellListViewItem(string shellObjectFullPath)
			: this(PIDLManager.CreateFromPathW(shellObjectFullPath), true) {}

		public ShellListViewItem(IntPtr shellObjectPIDL)
			: this(shellObjectPIDL, false) {}

		public ShellListViewItem(IntPtr shellObjectPIDL, bool pidlSelfCreated)
			: this(new BasicShellObject(shellObjectPIDL, pidlSelfCreated)) { }

		public ShellListViewItem(BasicShellObject shellObject) : base() {
			basicShellObject   = shellObject;
			Text               = DisplayName;
			ImageIndex         = iconManager.ClosedFolderIndex;
			FileInfoUpdated   += new FileInfoUpdatedHandler(IShellObject_FileInfoUpdated);
		}

		protected void IShellObject_FileInfoUpdated(object source, FileInfoUpdatedEventArgs e) {
			if(ImageIndex != e.ShFileInfo.iIcon) {
				ImageIndex = e.ShFileInfo.iIcon;
			}
		}

		#region IShellObject Member
		public IntPtr AbsolutePIDL {
			get {
				return basicShellObject.AbsolutePIDL;
			}
		}

		public IntPtr RelativePIDL {
			get {
				return basicShellObject.RelativePIDL;
			}
		}

		public bool AttachFileInfoThread(IFileInfoThread fileInfoThread) {
			return basicShellObject.AttachFileInfoThread(fileInfoThread);
		}

		public void DetachFileInfoThread(IFileInfoThread fileInfoThread) {
			basicShellObject.DetachFileInfoThread(fileInfoThread);
		}

		public void UpdateFileInfo(IFileInfoThread fileInfoThread, ShellAPI.SHFILEINFO shFileInfo) {
			basicShellObject.UpdateFileInfo(fileInfoThread, shFileInfo);
		}

		public BasicShellObjectCollection GetFolderItemCollection() {
			return basicShellObject.GetFolderItemCollection();
		}

		public string DisplayName {
			get {
				return basicShellObject.DisplayName;
			}
		}

		public string FullPathName {
			get {
				return basicShellObject.FullPathName;
			}
		}

		public event FileInfoUpdatedHandler FileInfoUpdated {
			add {
				basicShellObject.FileInfoUpdated += value;
			}
			remove {
				basicShellObject.FileInfoUpdated -= value;
			}
		}
		#endregion
	}
}
