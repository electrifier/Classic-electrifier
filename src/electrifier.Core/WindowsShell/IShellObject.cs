/*
** 
** electrifier
** 
** Copyright (c) 2017 Thorsten Jung @ electrifier.org and contributors
** 
*/

using System;

using electrifier.Core.WindowsShell.Services;
using electrifier.Win32API;

namespace electrifier.Core.WindowsShell
{
    /// <summary>
    /// 
    /// </summary>
    public class FileInfoUpdatedEventArgs : EventArgs
    {
        private IFileInfoThread fileInfoThread = null;
        public IFileInfoThread FileInfoThread { get { return this.fileInfoThread; } }
        private IconManager.FileInfoThreadResults fileInfoThreadResults;
        public IconManager.FileInfoThreadResults FileInfoThreadResults { get { return this.fileInfoThreadResults; } }
        //private ShellAPI.SHFILEINFO shFileInfo;
        //public  ShellAPI.SHFILEINFO ShFileInfo { get { return this.shFileInfo; } }

        //private IconManager.FileInfoThreadResults fileInfoThreadResults;
        //public IconManager.FileInfoThreadResults FileInfoThreadResults { get { return this.fileInfoThreadResults; } }

        public FileInfoUpdatedEventArgs(IFileInfoThread fileInfoThread, IconManager.FileInfoThreadResults fileInfoThreadResults)
        {
            this.fileInfoThread = fileInfoThread;
            this.fileInfoThreadResults = fileInfoThreadResults;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public delegate void FileInfoUpdatedHandler(object source, FileInfoUpdatedEventArgs e);

    /// <summary>
    /// Summary for IShellObject.
    /// </summary>
    public interface IShellObject
    {
        IntPtr AbsolutePIDL { get; }
        IntPtr RelativePIDL { get; }
        //		IntPtr PhysicalPIDL { get; }
        //		IntPtr PhysicalParentPIDL { get; }

        bool AttachFileInfoThread(IFileInfoThread fileInfoThread);
        void DetachFileInfoThread(IFileInfoThread fileInfoThread);
        void UpdateFileInfo(IFileInfoThread fileInfoThread, IconManager.FileInfoThreadResults fileInfoThreadResults);

        BasicShellObjectCollection GetFolderItemCollection();

        WinAPI.IDataObject GetIDataObject();
        WinAPI.IDropTarget GetIDropTarget();
        ShellAPI.IContextMenu GetIContextMenu();

        string DisplayName { get; }

        string GetDisplayNameOf(bool relativeToParentFolder, ShellAPI.SHGDN SHGDNFlags);

        event FileInfoUpdatedHandler FileInfoUpdated;
    }
}
