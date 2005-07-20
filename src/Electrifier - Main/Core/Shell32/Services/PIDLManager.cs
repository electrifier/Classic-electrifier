//	<file>
//		<copyright see="www.electrifier.org"/>
//		<license   see="www.electrifier.org"/>
//		<owner     name="Thorsten Jung" email="taj@electrifier.org"/>
//		<version   value="$Id: PIDLManager.cs,v 1.9 2004/09/10 15:21:54 taj bender Exp $"/>
//	</file>

using System;
using System.Runtime.InteropServices;

using Electrifier.Core.Services;
using Electrifier.Win32API;

namespace Electrifier.Core.Shell32.Services {
	/// <summary>
	/// Service class, acts as helper class for dealing with shell identifier lists (<c>PIDLs</c>).
	/// </summary>
	public class PIDLManager : AbstractService, IDisposable {
		protected ShellAPI.IMalloc malloc = null;

		/// <summary>
		/// Default constructor. Instantiates an instance of the <c>IMalloc</c> interface.
		/// </summary>
		public PIDLManager() : base() {
			IntPtr mallocPtr    = IntPtr.Zero;
			Type   mallocType   = System.Type.GetTypeFromCLSID(ShellAPI.IID_IMalloc);
			Object mallocObject = null;

			ShellAPI.SHGetMalloc(out mallocPtr);
			mallocObject = Marshal.GetTypedObjectForIUnknown(mallocPtr, mallocType);
			malloc = mallocObject as ShellAPI.IMalloc;
		}

		~PIDLManager() {
			Dispose();
		}

		#region Public Properties

		public ShellAPI.IMalloc Malloc {
			get { return this.malloc; }
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
			if(malloc != null) {
				lock(malloc) {
					Marshal.ReleaseComObject(malloc);
					malloc = null;
				}
			}
		}
		#endregion

		public static IntPtr AppendID(IntPtr pidl, IntPtr pmkid, bool fAppend) {
			return ShellAPI.ILAppendID(pidl, pmkid, fAppend);
		}

		public static IntPtr Clone(IntPtr pidl) {
			return ShellAPI.ILClone(pidl);			
		}

		public static IntPtr CloneFirst(IntPtr pidl) {
			return ShellAPI.ILCloneFirst(pidl);
		}

		public static IntPtr Combine(IntPtr pidl1, IntPtr pidl2) {
			return ShellAPI.ILCombine(pidl1, pidl2);
		}

		/// <summary>
		/// Wrapper function for Win32API-Call to SHGetFolderLocation. Gets a PIDL from the specified CSIDL.
		/// Call <see cref="Free">Free</see> to release the allocated memory by this object.
		/// </summary>
		/// <param name="csidl">The CSIDL constant that defines the directory searched for.</param>
		/// <returns>The PIDL as <c>IntPtr</c>. May be <c>IntPtr.Zero</c> when failed.</c></returns>
		public static IntPtr CreateFromCSIDL(ShellAPI.CSIDL csidl) {
			IntPtr pidl = IntPtr.Zero;

			ShellAPI.SHGetFolderLocation(IntPtr.Zero, csidl, IntPtr.Zero, 0, out pidl);
			return pidl;
		}

		/// <summary>
		/// Wrapper function for Win32API-Call to ILCreateFromPathW. Gets a PIDL from the specified path string.
		/// Call <see cref="Free">Free</see> to release the allocated memory by this object.
		/// </summary>
		/// <param name="pwszPath">The path string that defines the directory searched for.</param>
		/// <returns>The PIDL as <c>IntPtr</c>. May be <c>IntPtr.Zero</c> when failed.</returns>
		public static IntPtr CreateFromPathW(String pwszPath) {
			return ShellAPI.ILCreateFromPathW(pwszPath);
		}

		/// <summary>
		/// Returns a copy of the given PIDL with the last item removed.
		/// </summary>
		/// <param name="pidl">The PIDL to create the parent PIDL from</param>
		/// <returns>The PIDL as <c>IntPtr</c>. May be <c>IntPtr.Zero</c> when failed.</returns>
		public static IntPtr CreateParentPIDL(IntPtr pidl) {
			IntPtr newPidl = Clone(pidl);

			if((newPidl.Equals(IntPtr.Zero)) || (!RemoveLastID(newPidl))) {
				Free(newPidl);

				return IntPtr.Zero;
			}

			return newPidl;
		}

		public static IntPtr FindChild(IntPtr pidlParent, IntPtr pidlChild) {
			return ShellAPI.ILFindChild(pidlParent, pidlChild);
		}

		public static IntPtr FindLastID(IntPtr pidl) {
			return ShellAPI.ILFindLastID(pidl);
		}

		/// <summary>
		/// Wrapper function for Win32API-Call to ILFree. Deallocates the memory used by a PIDL.
		/// The object, this pointer (=> Pointer to Identifier List) points to gets destroyed!
		/// </summary>
		/// <param name="pidl">The PIDL which resources should be freed.</param>
		public static void Free(IntPtr pidl) {
			ShellAPI.ILFree(pidl);
		}

		public static IntPtr GetNext(IntPtr pidl) {
			return ShellAPI.ILGetNext(pidl);
		}

		public static UInt32 GetSize(IntPtr pidl) {
			return ShellAPI.ILGetSize(pidl);
		}

		public static bool RemoveLastID(IntPtr pidl) {
			return ShellAPI.ILRemoveLastID(pidl);
		}

		public static bool IsEqual(IntPtr pidl1, IntPtr pidl2) {
			return ShellAPI.ILIsEqual(pidl1, pidl2);
		}

		public static bool IsParent(IntPtr pidlParent, IntPtr pidlBelow, bool fImmediate) {
			return ShellAPI.ILIsParent(pidlParent, pidlBelow, fImmediate);
		}

		public static bool IsDesktop(IntPtr pidl) {
			return (GetSize(pidl) == 2);		// TODO: Find a nicer solution...
		}
	}
}
