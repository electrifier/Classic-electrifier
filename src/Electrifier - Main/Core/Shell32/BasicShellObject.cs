//	<file>
//		<copyright see="www.electrifier.org"/>
//		<license   see="www.electrifier.org"/>
//		<owner     name="Thorsten Jung" email="taj@electrifier.org"/>
//		<version   value="$Id: BasicShellObject.cs,v 1.13 2004/09/10 20:30:33 taj bender Exp $"/>
//	</file>

using System;
using System.Runtime.InteropServices;

using Electrifier.Core.Services;
using Electrifier.Core.Shell32.Services;
using Electrifier.Win32API;

namespace Electrifier.Core.Shell32 {
	/// <summary>
	/// Zusammenfassung für ShellObject.
	/// </summary>
	public class BasicShellObject : IShellObject {
		protected static DesktopFolderInstance desktopFolderInstance = (DesktopFolderInstance)ServiceManager.Services.GetService(typeof(DesktopFolderInstance));
		protected        IntPtr                absolutePIDL          = IntPtr.Zero;	// TODO: Release! when disposing
		protected        IntPtr                relativePIDL          = IntPtr.Zero;
		protected        string                displayName           = null;
		protected        IFileInfoThread       fileInfoThread        = null;

		public BasicShellObject(ShellAPI.CSIDL shellObjectCSIDL)
			: this(PIDLManager.CreateFromCSIDL(shellObjectCSIDL), true) {}

		public BasicShellObject(string shellObjectFullPath)
			: this(PIDLManager.CreateFromPathW(shellObjectFullPath), true) {}

		public BasicShellObject(IntPtr shellObjectPIDL)
			: this(shellObjectPIDL, false) {}

		public BasicShellObject(IntPtr shellObjectPIDL, bool pidlSelfCreated) : base() {
			IntPtr ownPIDL = (pidlSelfCreated ? shellObjectPIDL : PIDLManager.Clone(shellObjectPIDL));

			absolutePIDL = ownPIDL;
			relativePIDL = PIDLManager.FindLastID(ownPIDL);
		}

		public BasicShellObjectCollection GetFolderItemCollection() {
			// TODO: Check, whether we are a folder anyhow :-)
			return new BasicShellObjectCollection(AbsolutePIDL,
			                                      ShellAPI.SHCONTF.DefaultForTreeView,
			                                      ShellObjectPIDLComparer.SortMode.Ascending);
		}

		protected virtual string GetDisplayName() {
			return this.GetDisplayNameOf(true, ShellAPI.SHGDN.INFOLDER);
		}

		public virtual string GetDisplayNameOf(bool relativeToParentFolder, ShellAPI.SHGDN SHGDNFlags) {
			string result = null;
			ShellAPI.STRRET strRet;
			IntPtr strRetPidl = IntPtr.Zero;

			if((!relativeToParentFolder) || PIDLManager.IsDesktop(this.AbsolutePIDL)) {
				strRetPidl = AbsolutePIDL;

				desktopFolderInstance.Get.GetDisplayNameOf(strRetPidl, SHGDNFlags, out strRet);
			} else {
				IntPtr                parentPidl   = PIDLManager.CreateParentPIDL(AbsolutePIDL);
				ShellAPI.IShellFolder parentFolder = desktopFolderInstance.GetIShellFolder(parentPidl);

				strRetPidl = RelativePIDL;

				parentFolder.GetDisplayNameOf(strRetPidl, SHGDNFlags, out strRet);

				Marshal.ReleaseComObject(parentFolder);	// TODO: Put IShellInterface into disposible class
				PIDLManager.Free(parentPidl);
			}

			// TODO: Won't work using W2K
			// TODO: Converting functions into STRRET.ToString (struct)
			ShellAPI.StrRetToBSTR(ref strRet, this.AbsolutePIDL, out result);

			return result;
		}

		public virtual WinAPI.IDataObject GetIDataObject() {
			IntPtr ppv      = IntPtr.Zero;
			uint   reserved = 0;
			int    hResult  = 0;

			if(PIDLManager.IsDesktop(this.AbsolutePIDL)) {
				hResult = desktopFolderInstance.Get.GetUIObjectOf(IntPtr.Zero, 0, null, ref ShellAPI.IID_IDataObject, ref reserved, out ppv);
			} else {
				IntPtr parentPidl = PIDLManager.CreateParentPIDL(AbsolutePIDL);

				try {
					ShellAPI.IShellFolder parentFolder = desktopFolderInstance.GetIShellFolder(parentPidl);
					IntPtr[]              pidls        = { this.RelativePIDL };

					hResult = parentFolder.GetUIObjectOf(IntPtr.Zero, 1, pidls, ref ShellAPI.IID_IDataObject, ref reserved, out ppv);
				} finally {
					PIDLManager.Free(parentPidl);
				}
			}
			// TODO: Refactor the code above

//			Marshal.ThrowExceptionForHR(hResult);

			return ((ppv != IntPtr.Zero) ? (Marshal.GetObjectForIUnknown(ppv) as WinAPI.IDataObject) : null);
		}

		public virtual WinAPI.IDropTarget GetIDropTarget() {
			IntPtr                ppv          = IntPtr.Zero;
			uint                  reserved     = 0;
			int                   hResult      = 0;

			if(PIDLManager.IsDesktop(this.AbsolutePIDL)) {
				hResult = desktopFolderInstance.Get.GetUIObjectOf(IntPtr.Zero, 0, null, ref ShellAPI.IID_IDropTarget, ref reserved, out ppv);
			} else {
				IntPtr parentPidl = PIDLManager.CreateParentPIDL(AbsolutePIDL);

				try {
					ShellAPI.IShellFolder parentFolder = desktopFolderInstance.GetIShellFolder(parentPidl);
					IntPtr[]              pidls        = { this.RelativePIDL };

					hResult = parentFolder.GetUIObjectOf(IntPtr.Zero, 1, pidls, ref ShellAPI.IID_IDropTarget, ref reserved, out ppv);
				} finally {
					PIDLManager.Free(parentPidl);
				}
			}
			// TODO: Refactor the code above

//			Marshal.ThrowExceptionForHR(hResult);
			return ((ppv != IntPtr.Zero) ? (Marshal.GetObjectForIUnknown(ppv) as WinAPI.IDropTarget) : null);
		}

		public virtual ShellAPI.IContextMenu GetIContextMenu() {
			IntPtr                ppv          = IntPtr.Zero;
			uint                  reserved     = 0;
			int                   hResult      = 0;

			if(PIDLManager.IsDesktop(this.AbsolutePIDL)) {
				hResult = desktopFolderInstance.Get.GetUIObjectOf(IntPtr.Zero, 0, null, ref ShellAPI.IID_IContextMenu, ref reserved, out ppv);
			} else {
				IntPtr parentPidl = PIDLManager.CreateParentPIDL(AbsolutePIDL);

				try {
					ShellAPI.IShellFolder parentFolder = desktopFolderInstance.GetIShellFolder(parentPidl);
					IntPtr[]              pidls        = { this.RelativePIDL };

					hResult = parentFolder.GetUIObjectOf(IntPtr.Zero, 1, pidls, ref ShellAPI.IID_IContextMenu, ref reserved, out ppv);
				} finally {
					PIDLManager.Free(parentPidl);
				}
			}
			// TODO: Refactor the code above

			//			Marshal.ThrowExceptionForHR(hResult);
			return ((ppv != IntPtr.Zero) ? (Marshal.GetObjectForIUnknown(ppv) as ShellAPI.IContextMenu) : null);
		}


		#region IShellObject Member
		public IntPtr AbsolutePIDL {
			get {
				return absolutePIDL;
			}
		}

		public IntPtr RelativePIDL {
			get {
				return relativePIDL;
			}
		}

		public string DisplayName { get { return GetDisplayName(); } }


		public bool AttachFileInfoThread(IFileInfoThread fileInfoThread) {
			if(this.fileInfoThread == null) {
				this.fileInfoThread = fileInfoThread;
				return true;
			} else {
				return false;
			}
		}

		public void DetachFileInfoThread(IFileInfoThread fileInfoThread) {
			if(this.fileInfoThread != fileInfoThread) {
				throw new ArgumentException("Electrifier.Core.Shell32.BasicShellObject.DetachFileInfoThread " +
					"was called with invalid IFileInfoThread instance");
			} else {
				this.fileInfoThread = null;
			}
		}

		public void UpdateFileInfo(IFileInfoThread fileInfoThread, ShellAPI.SHFILEINFO shFileInfo) {
			if(this.fileInfoThread != fileInfoThread) {
				throw new ArgumentException("Electrifier.Core.Shell32.BasicShellObject.UpdateFileInfo " +
					"was called with invalid IFileInfoThread instance");
			} else {
				if(FileInfoUpdated != null) {
					FileInfoUpdatedEventArgs args = new FileInfoUpdatedEventArgs(fileInfoThread, shFileInfo);

					FileInfoUpdated(this, args);
				}
			}
		}

		public event FileInfoUpdatedHandler FileInfoUpdated;
		#endregion
	}
}
