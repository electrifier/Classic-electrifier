using System;
using System.Windows.Forms;

using Electrifier.Core.Controls;
using Electrifier.Core.Services;
using Electrifier.Core.Shell32.Services;
using Electrifier.Win32API;

namespace Electrifier.Core.Shell32.Controls {
	/// <summary>
	/// Zusammenfassung für ShellListViewItem.
	/// </summary>
	public class ShellListViewItem : ExtListViewItem, IShellObject {
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
			ImageIndex         = iconManager.ClosedFolderIndex;
			FileInfoUpdated   += new FileInfoUpdatedHandler(IShellObject_FileInfoUpdated);
			SetText(DisplayName);
		}

		protected void IShellObject_FileInfoUpdated(object source, FileInfoUpdatedEventArgs e) {
			if (this.ImageIndex != e.FileInfoThreadResults.ImageIndex) {
				this.ImageIndex = e.FileInfoThreadResults.ImageIndex;
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

		public void UpdateFileInfo(IFileInfoThread fileInfoThread, IconManager.FileInfoThreadResults fileInfoThreadResults) {
			basicShellObject.UpdateFileInfo(fileInfoThread, fileInfoThreadResults);
		}

		public BasicShellObjectCollection GetFolderItemCollection() {
			return basicShellObject.GetFolderItemCollection();
		}

		public override WinAPI.IDataObject GetIDataObject() {
			return basicShellObject.GetIDataObject();					
		}

		public override WinAPI.IDropTarget GetIDropTarget() {
			return basicShellObject.GetIDropTarget();
		}
		
		public ShellAPI.IContextMenu GetIContextMenu() {
			return this.basicShellObject.GetIContextMenu();
		}

		public string DisplayName { get { return this.basicShellObject.DisplayName; } }

		public string GetDisplayNameOf(bool relativeToParentFolder, ShellAPI.SHGDN SHGDNFlags) {
			return this.basicShellObject.GetDisplayNameOf(relativeToParentFolder, SHGDNFlags);
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
