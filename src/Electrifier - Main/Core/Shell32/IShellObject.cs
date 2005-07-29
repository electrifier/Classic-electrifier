//	<file>
//		<copyright see="www.electrifier.org"/>
//		<license   see="www.electrifier.org"/>
//		<owner     name="Thorsten Jung" email="taj@electrifier.org"/>
//		<version   value="$Id: IShellObject.cs,v 1.7 2004/09/10 20:30:33 taj bender Exp $"/>
//	</file>

using System;

using Electrifier.Core.Shell32.Services;
using Electrifier.Win32API;

namespace Electrifier.Core.Shell32 {
	/// <summary>
	/// 
	/// </summary>
	public class FileInfoUpdatedEventArgs : EventArgs {
		private IFileInfoThread     fileInfoThread = null;
		public  IFileInfoThread     FileInfoThread { get {	return fileInfoThread; } }
		private ShellAPI.SHFILEINFO shFileInfo;
		public  ShellAPI.SHFILEINFO ShFileInfo { get { return shFileInfo; } }

		public FileInfoUpdatedEventArgs(IFileInfoThread fileInfoThread, ShellAPI.SHFILEINFO shFileInfo) {
			this.fileInfoThread = fileInfoThread;
			this.shFileInfo     = shFileInfo;
		}
	}

	/// <summary>
	/// 
	/// </summary>
	public delegate void FileInfoUpdatedHandler(object source, FileInfoUpdatedEventArgs e);

	/// <summary>
	/// Zusammenfassung für IShellObject.
	/// </summary>
	public interface IShellObject {
		IntPtr AbsolutePIDL { get; }
		IntPtr RelativePIDL { get; }
		//		IntPtr PhysicalPIDL { get; }
		//		IntPtr PhysicalParentPIDL { get; }

		bool AttachFileInfoThread(IFileInfoThread fileInfoThread);
		void DetachFileInfoThread(IFileInfoThread fileInfoThread);
		void UpdateFileInfo(IFileInfoThread fileInfoThread, ShellAPI.SHFILEINFO shFileInfo);

		BasicShellObjectCollection GetFolderItemCollection();

		WinAPI.IDataObject GetIDataObject();
		WinAPI.IDropTarget GetIDropTarget();
		ShellAPI.IContextMenu GetIContextMenu();

		string DisplayName { get; }

		string GetDisplayNameOf(bool relativeToParentFolder, ShellAPI.SHGDN SHGDNFlags);

		event FileInfoUpdatedHandler FileInfoUpdated;
	}
}
