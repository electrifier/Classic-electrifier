//	<file>
//		<copyright see="www.electrifier.org"/>
//		<license   see="www.electrifier.org"/>
//		<owner     name="Thorsten Jung" email="taj@electrifier.org"/>
//		<version   value="$Id: IconManager.cs,v 1.9 2004/09/10 21:48:34 taj bender Exp $"/>
//	</file>

using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Threading;

using Electrifier.Core.Services;
using Electrifier.Win32API;

namespace Electrifier.Core.Shell32.Services {
	/// <summary>
	/// Service class, helps dealing with system icon image lists and default icons for file classes.
	/// </summary>
	public class IconManager : AbstractService {
		protected IntPtr smallImageList    = IntPtr.Zero;
		protected IntPtr largeImageList    = IntPtr.Zero;
		protected int    closedFolderIndex = -1;
		protected int    openedFolderIndex = -1;

		#region Public Properties
		public IntPtr SmallImageList {
			get {
				return smallImageList;
			}
		}

		public IntPtr LargeImageList {
			get {
				return largeImageList;
			}
		}

		public int ClosedFolderIndex {
			get {
				return closedFolderIndex;
			}
		}

		public int OpenedFolderIndex {
			get {
				return openedFolderIndex;
			}
		}
		#endregion

		public IconManager() : base() {
			string              systemPath = Environment.SystemDirectory;
			ShellAPI.SHFILEINFO shFileInfo = new ShellAPI.SHFILEINFO();
			UInt32              cbFileInfo = (UInt32)Marshal.SizeOf(shFileInfo);

			// Get small system image list together with closed folder icon index.
			smallImageList = ShellAPI.SHGetFileInfo(systemPath, 0, ref shFileInfo, cbFileInfo,
				(ShellAPI.SHGFI.SysIconIndex | ShellAPI.SHGFI.SmallIcon));
			closedFolderIndex = shFileInfo.iIcon;

			// Get large system image list together with opened folder icon index.
			largeImageList = ShellAPI.SHGetFileInfo(systemPath, 0, ref shFileInfo, cbFileInfo,
				(ShellAPI.SHGFI.SysIconIndex | ShellAPI.SHGFI.LargeIcon | ShellAPI.SHGFI.OpenIcon));
			openedFolderIndex = shFileInfo.iIcon;
		}

		#region Sub-Class FileInfoThread
		public class FileInfoThread : IFileInfoThread {
			protected SequenceStack sequence = null;
			protected Stack         excluded = new Stack();
			protected Thread        thread   = null;

			#region IFileInfoThread Member
			public void Prioritize(IShellObject sender) {
				sequence.Push(sender);
			}

			public void Remove(IShellObject sender) {
				excluded.Push(sender);
			}
			#endregion

			/// <summary>
			/// Overloaded constructor. Creates a FileInfoThread which will process all the items
			/// contained in the given collection. The elements will get processed in that order,
			/// in which they are ordered in the collection.
			/// </summary>
			/// <param name="collection">The collection of ShellObjects to get info for</param>
			public FileInfoThread(IShellObjectCollection collection)
				: this(new SequenceStack(collection)) { }

			/// <summary>
			/// Overloaded constructor. Creates a FileInfoThread which will process the item given.
			/// </summary>
			/// <param name="shellObject">The ShellObject</param>
			public FileInfoThread(IShellObject shellObject)
				: this(new SequenceStack(shellObject)) { }

			/// <summary>
			/// The real constructor. Creates the FileInfoThread and starts execution.
			/// </summary>
			/// <param name="sequence">The Stack containing the objects to be processed</param>
			protected FileInfoThread(SequenceStack sequenceStack) {
				sequence            = sequenceStack;
				thread              = new Thread(new ThreadStart(Process));
				thread.IsBackground = true;

				thread.Start();
			}

			protected void Process() {
				ShellAPI.SHFILEINFO shFileInfo = new ShellAPI.SHFILEINFO();
				UInt32              cbFileInfo = (UInt32)Marshal.SizeOf(shFileInfo);

				// Attach to every IShellObject item
				foreach(IShellObject shellObject in sequence) {
					if(!shellObject.AttachFileInfoThread(this))
						excluded.Push(shellObject);
				}

				while(sequence.Count > 0) {
					IShellObject shellObject = sequence.Pop();

					if(!excluded.Contains(shellObject)){
						try {
							ShellAPI.SHGetFileInfo(shellObject.AbsolutePIDL, 0, ref shFileInfo, cbFileInfo,
								(ShellAPI.SHGFI.PIDL | ShellAPI.SHGFI.SysIconIndex));

							shellObject.UpdateFileInfo(this, shFileInfo);
						} finally {
							excluded.Push(shellObject);
						}
					}
				}
			}

			#region Sub-Class SequenceStack
			protected class SequenceStack : Stack {
				public SequenceStack(IShellObjectCollection collection) {
					foreach(IShellObject shellObject in collection) {
						Push(shellObject);
					}
				}

				public SequenceStack(IShellObject shellObject) {
					Push(shellObject);
				}

				public new IShellObject Pop() {
					return base.Pop() as IShellObject;
				}
			}
			#endregion
		}
		#endregion
	}
}
