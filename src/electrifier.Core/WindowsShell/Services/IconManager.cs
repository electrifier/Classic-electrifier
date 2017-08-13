/*
** 
** electrifier
** 
** Copyright (c) 2017 Thorsten Jung @ electrifier.org and contributors
** 
*/

using System;
using System.Collections;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;

using electrifier.Core.Services;
using electrifier.Win32API;

namespace electrifier.Core.WindowsShell.Services
{
    /// <summary>
    /// Service class, helps dealing with system icon image lists and default icons for file classes.
    /// </summary>
    public class IconManager : AbstractService
    {
        protected IntPtr smallImageList = IntPtr.Zero;
        protected IntPtr largeImageList = IntPtr.Zero;
        protected int closedFolderIndex = -1;
        protected int openedFolderIndex = -1;

        #region Public Properties
        public IntPtr SmallImageList {
            get {
                return this.smallImageList;
            }
        }

        public IntPtr LargeImageList {
            get {
                return this.largeImageList;
            }
        }

        public int ClosedFolderIndex {
            get {
                return this.closedFolderIndex;
            }
        }

        public int OpenedFolderIndex {
            get {
                return this.openedFolderIndex;
            }
        }
        #endregion

        public IconManager() : base()
        {
            string systemPath = Environment.SystemDirectory;
            UInt32 cbFileInfo = (UInt32)Marshal.SizeOf(typeof(ShellAPI.SHFILEINFO));

            // Get small system image list together with closed folder icon index.
            this.smallImageList = ShellAPI.SHGetFileInfo(systemPath, 0, out ShellAPI.SHFILEINFO shFileInfo, cbFileInfo,
                (ShellAPI.SHGFI.SysIconIndex | ShellAPI.SHGFI.SmallIcon));
            this.closedFolderIndex = shFileInfo.iIcon;

            // TODO: Only if TVIF_SELECTEDIMAGE is set, an open image is available
            // Get large system image list together with opened folder icon index.
            this.largeImageList = ShellAPI.SHGetFileInfo(systemPath, 0, out shFileInfo, cbFileInfo,
                (ShellAPI.SHGFI.SysIconIndex | ShellAPI.SHGFI.LargeIcon | ShellAPI.SHGFI.OpenIcon));
            this.openedFolderIndex = shFileInfo.iIcon;
        }

        /// <summary>
        /// Creates a managed icon object for the given shell object
        /// </summary>
        /// <param name="absolutePIDL">The shell object's PIDL</param>
        /// <param name="large">True for large icon</param>
        /// <returns></returns>
        public static Icon GetIconFromPIDL(IntPtr absolutePIDL, bool largeIcon)
        {
            Icon iconFromPIDL = null;
            ShellAPI.SHFILEINFO shFileInfo = new ShellAPI.SHFILEINFO();
            UInt32 cbFileInfo = (UInt32)Marshal.SizeOf(typeof(ShellAPI.SHFILEINFO));

            IntPtr hImage = Win32API.ShellAPI.SHGetFileInfo(absolutePIDL, 0, out shFileInfo, cbFileInfo,
                ((largeIcon ? ShellAPI.SHGFI.LargeIcon : ShellAPI.SHGFI.SmallIcon) |
                ShellAPI.SHGFI.Icon | ShellAPI.SHGFI.PIDL /*| ShellAPI.SHGFI.UseFileAttributes*/));

            iconFromPIDL = Icon.FromHandle(shFileInfo.hIcon).Clone() as Icon;

            WinAPI.DestroyIcon(shFileInfo.hIcon);

            return iconFromPIDL;
        }

        public static /*inline*/ uint ExtractOverlayImageIndex(int iIconIndex)
        {
            return (uint)iIconIndex >> 24;
        }

        #region Sub-Class FileInfoThread

        [Flags]
        public enum FileInfoThreadParams
        {
            NONE = 0x0,
            ImageIndex = 0x1,
            SelectedImageIndex = 0x2,
            OverlayIndex = 0x4,
            IsHidden = 0x8,
            IsCompressed = 0x10,
            HasSubfolder = 0x20,
            ALL = (ImageIndex | SelectedImageIndex | OverlayIndex | IsHidden | IsCompressed | HasSubfolder),
        }

        public struct FileInfoThreadResults
        {
            public FileInfoThreadParams ValidValues;
            public int ImageIndex;
            public int SelectedImageIndex;
            public uint OverlayIndex;
            public bool IsHidden;
            public bool IsCompressed;
            public bool HasSubfolder;

            //public FileInfoThreadResults() { this.ValidValues = FileInfoThreadParams.NONE; }
        }

        public class FileInfoThread : IFileInfoThread
        {
            protected SequenceStack sequence = null;
            protected Stack excluded = new Stack();
            protected Thread thread = null;
            protected FileInfoThreadParams fileInfoThreadParams;

            #region IFileInfoThread Member
            public void Prioritize(IShellObject sender)
            {
                this.sequence.Push(sender);
            }

            public void Remove(IShellObject sender)
            {
                this.excluded.Push(sender);
            }
            #endregion

            /// <summary>
            /// Overloaded constructor. Creates a FileInfoThread which will process all the items
            /// contained in the given collection. The elements will get processed in that order,
            /// in which they are ordered in the collection.
            /// </summary>
            /// <param name="collection">The collection of ShellObjects to get info for</param>
            /// <param name="fileInfoThreadParams">The informations that should be gathered</param>
            public FileInfoThread(IShellObjectCollection collection, FileInfoThreadParams fileInfoThreadParams = FileInfoThreadParams.ALL)
                : this(new SequenceStack(collection), fileInfoThreadParams) { }

            /// <summary>
            /// Overloaded constructor. Creates a FileInfoThread which will process the given item.
            /// </summary>
            /// <param name="shellObject">The ShellObject</param>
            /// <param name="fileInfoThreadParams">The informations that should be gathered</param>
            public FileInfoThread(IShellObject shellObject, FileInfoThreadParams fileInfoThreadParams = FileInfoThreadParams.ALL)
                : this(new SequenceStack(shellObject), fileInfoThreadParams) { }

            /// <summary>
            /// The real constructor. Creates the FileInfoThread and starts execution.
            /// </summary>
            /// <param name="sequence">The Stack containing the objects to be processed</param>
            /// <param name="fileInfoThreadParams">The informations that should be gathered</param>
            protected FileInfoThread(SequenceStack sequenceStack, FileInfoThreadParams fileInfoThreadParams)
            {
                this.sequence = sequenceStack;
                this.fileInfoThreadParams = fileInfoThreadParams;

                this.thread = new Thread(new ThreadStart(this.Process))
                {
                    IsBackground = true
                };
                this.thread.Start();
            }

            protected void Process()
            {
                FileInfoThreadResults fileInfoThreadResults = new FileInfoThreadResults();
                ShellAPI.SHFILEINFO shFileInfo;
                UInt32 cbFileInfo = (UInt32)Marshal.SizeOf(typeof(ShellAPI.SHFILEINFO));

                // Attach to every IShellObject item
                foreach (IShellObject shellObject in this.sequence)
                {
                    if (!shellObject.AttachFileInfoThread(this))
                        this.excluded.Push(shellObject);
                }

                while (this.sequence.Count > 0)
                {
                    IShellObject shellObject = this.sequence.Pop();

                    fileInfoThreadResults.ValidValues = FileInfoThreadParams.NONE;

                    if (!this.excluded.Contains(shellObject))
                    {
                        try
                        {
                            /*
							 * TODO: Setting the OverlayIndex-Flag will result in getting the correct overlay values
							 *       for e.g. OneDrive, but the ListView won't show them anymore...
							 *       Very strange indeed...
							 */
                            if (this.fileInfoThreadParams.HasFlag(FileInfoThreadParams.ImageIndex))
                            {
                                ShellAPI.SHGetFileInfo(shellObject.AbsolutePIDL, 0, out shFileInfo, cbFileInfo,
                                    (ShellAPI.SHGFI.Icon | ShellAPI.SHGFI.SmallIcon | ShellAPI.SHGFI.PIDL |
                                     ShellAPI.SHGFI.SysIconIndex | ShellAPI.SHGFI.Attributes /*| ShellAPI.SHGFI.OverlayIndex */));

                                fileInfoThreadResults.ValidValues ^= FileInfoThreadParams.ImageIndex | FileInfoThreadParams.OverlayIndex;
                                fileInfoThreadResults.ImageIndex = shFileInfo.iIcon /*& 0xFFFFFF*/;
                                fileInfoThreadResults.OverlayIndex = IconManager.ExtractOverlayImageIndex(shFileInfo.iIcon);

                                if (shFileInfo.hIcon != null)
                                    WinAPI.DestroyIcon(shFileInfo.hIcon);

                                fileInfoThreadResults.ValidValues ^= FileInfoThreadParams.IsHidden;
                                fileInfoThreadResults.IsHidden = ((shFileInfo.dwAttributes & ShellAPI.SFGAO.Hidden) != 0);

                                fileInfoThreadResults.ValidValues ^= FileInfoThreadParams.IsCompressed;
                                fileInfoThreadResults.IsCompressed = ((shFileInfo.dwAttributes & ShellAPI.SFGAO.Compressed) != 0);

                                fileInfoThreadResults.ValidValues ^= FileInfoThreadParams.HasSubfolder;
                                fileInfoThreadResults.HasSubfolder = ((shFileInfo.dwAttributes & ShellAPI.SFGAO.HasSubfolder) != 0);

                                // TODO: TODO: ShellAPI.SHGFI.Attributes komplett als solches im basicshellobject speichern,
                                // dann per "macros" auf die attribute ishidden, iscompressed, hassubfolder etc. zugreifen
                            }

                            if (this.fileInfoThreadParams.HasFlag(FileInfoThreadParams.SelectedImageIndex))
                            {
                                ShellAPI.SHGetFileInfo(shellObject.AbsolutePIDL, 0, out shFileInfo, cbFileInfo,
                                    (ShellAPI.SHGFI.Icon | ShellAPI.SHGFI.SmallIcon | ShellAPI.SHGFI.PIDL |
                                     ShellAPI.SHGFI.SysIconIndex | ShellAPI.SHGFI.OpenIcon));

                                fileInfoThreadResults.ValidValues ^= FileInfoThreadParams.SelectedImageIndex;
                                fileInfoThreadResults.SelectedImageIndex = shFileInfo.iIcon;
                            }

                            shellObject.UpdateFileInfo(this, fileInfoThreadResults);
                        }
                        finally
                        {
                            this.excluded.Push(shellObject);
                        }
                    }
                }
            }

            #region Sub-Class SequenceStack
            protected class SequenceStack : Stack
            {
                public SequenceStack(IShellObjectCollection collection)
                {
                    foreach (IShellObject shellObject in collection)
                    {
                        Push(shellObject);
                    }
                }

                public SequenceStack(IShellObject shellObject)
                {
                    Push(shellObject);
                }

                public new IShellObject Pop()
                {
                    return base.Pop() as IShellObject;
                }
            }
            #endregion
        }
        #endregion
    }
}
