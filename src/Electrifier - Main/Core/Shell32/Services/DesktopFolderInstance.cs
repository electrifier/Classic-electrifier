//	<file>
//		<copyright see="www.electrifier.org"/>
//		<license   see="www.electrifier.org"/>
//		<owner     name="Thorsten Jung" email="taj@electrifier.org"/>
//		<version   value="$Id: DesktopFolderInstance.cs,v 1.9 2004/09/10 15:21:54 taj bender Exp $"/>
//	</file>

using System;
using System.Runtime.InteropServices;

using Electrifier.Core.Services;
using Electrifier.Win32API;

namespace Electrifier.Core.Shell32.Services {
	/// <summary>
	/// Service class, acts as container for desktop folders <c>IShellFolder</c> interface.
	/// </summary>
	public class DesktopFolderInstance : AbstractService, IDisposable {
		protected ShellAPI.IShellFolder desktopFolderInstance = null;
		protected Type                  iShellFolderType      = null;

		/// <summary>
		/// Default constructor. Instantiates an instance of desktops <c>IShellFolder</c> interface.
		/// </summary>
		public DesktopFolderInstance() : base() {
			IntPtr desktopFolder     = IntPtr.Zero;
			Object shellFolderObject = null;

			iShellFolderType = Type.GetTypeFromCLSID(ShellAPI.IID_IShellFolder);

			ShellAPI.SHGetDesktopFolder(out desktopFolder);
			shellFolderObject = Marshal.GetTypedObjectForIUnknown(desktopFolder, iShellFolderType);
			desktopFolderInstance = (ShellAPI.IShellFolder)shellFolderObject;
		}

		~DesktopFolderInstance() {
			Dispose();
		}

		/// <summary>
		/// Returns the IShellFolder object of the folder specified by shellFolderCSIDL.
		/// The underlying COM-object must be released by caller after use!
		/// </summary>
		/// <param name="shellFolderCSIDL">The CSIDL-constant describing the folder</param>
		/// <returns>The IShellFolder interface or <c>null</c> if failed</returns>
		public ShellAPI.IShellFolder GetIShellFolder(ShellAPI.CSIDL shellFolderCSIDL) {
			IntPtr                pidl        = PIDLManager.CreateFromCSIDL(shellFolderCSIDL);
			ShellAPI.IShellFolder shellFolder = GetIShellFolder(pidl);

			if(!pidl.Equals(IntPtr.Zero))
				PIDLManager.Free(pidl);

			return shellFolder;
		}

		/// <summary>
		/// Returns the IShellFolder object of the folder specified by shellFolderFullPath.
		/// The underlying COM-object must be released by caller after use!
		/// </summary>
		/// <param name="shellFolderFullPath">The full qualified path string of the folder</param>
		/// <returns>The IShellFolder interface or <c>null</c> if failed</returns>
		public ShellAPI.IShellFolder GetIShellFolder(string shellFolderFullPath) {
			IntPtr                pidl        = PIDLManager.CreateFromPathW(shellFolderFullPath);
			ShellAPI.IShellFolder shellFolder = GetIShellFolder(pidl);

			if(!pidl.Equals(IntPtr.Zero))
				PIDLManager.Free(pidl);

			return shellFolder;
		}

		/// <summary>
		/// Returns the IShellFolder object of the folder specified by shellFolderPIDL.
		/// The underlying COM-object must be released by caller after use!
		/// </summary>
		/// <param name="shellFolderPIDL">The pointer to the PIDL-List describing the folder</param>
		/// <returns>The IShellFolder interface or <c>null</c> if failed</returns>
		public ShellAPI.IShellFolder GetIShellFolder(IntPtr shellFolderPIDL) {
			if(PIDLManager.IsDesktop(shellFolderPIDL)) {
				IntPtr shellFolder       = IntPtr.Zero;
				Object shellFolderObject = null;

				ShellAPI.SHGetDesktopFolder(out shellFolder);

				shellFolderObject = Marshal.GetTypedObjectForIUnknown(shellFolder, iShellFolderType);

				return (ShellAPI.IShellFolder)shellFolderObject;
			} else {
				ShellAPI.IShellFolder shellFolder = null;

				Get.BindToObject(shellFolderPIDL, IntPtr.Zero, ref ShellAPI.IID_IShellFolder, ref shellFolder);

				return shellFolder;
			}
		}

		#region Public Properties
		public ShellAPI.IShellFolder Get {
			get {
				return desktopFolderInstance;
			}
		}
		#endregion

		#region IService Interface Members
		public override void UnloadService() {
			Dispose();
			base.UnloadService();
		}
		#endregion

		#region IDisposable Interface Members
		public void Dispose() {
			if(desktopFolderInstance != null) {
				lock(desktopFolderInstance) {
					Marshal.ReleaseComObject(desktopFolderInstance);
					desktopFolderInstance = null;
				}
			}
		}
		#endregion
	}
}
