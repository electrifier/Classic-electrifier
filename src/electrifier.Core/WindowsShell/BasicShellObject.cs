/*
** 
** electrifier
** 
** Copyright (c) 2017 Thorsten Jung @ electrifier.org and contributors
** 
*/

using System;
using System.Runtime.InteropServices;

using electrifier.Core.Services;
using electrifier.Core.WindowsShell.Services;
using electrifier.Win32API;

namespace electrifier.Core.WindowsShell
{
    /// <summary>
    /// Summary for ShellObject.
    /// </summary>
    public class BasicShellObject : IShellObject
    {
        protected static DesktopFolderInstance desktopFolderInstance = (DesktopFolderInstance)ServiceManager.Services.GetService(typeof(DesktopFolderInstance));
        protected IntPtr absolutePIDL = IntPtr.Zero;    // TODO: Release! when disposing
        protected IntPtr relativePIDL = IntPtr.Zero;
        protected string displayName = null;
        protected IFileInfoThread fileInfoThread = null;

        public BasicShellObject(ShellAPI.CSIDL shellObjectCSIDL)
            : this(PIDLManager.CreateFromCSIDL(shellObjectCSIDL), true) { }

        public BasicShellObject(string shellObjectFullPath)
            : this(PIDLManager.CreateFromPathW(shellObjectFullPath), true) { }

        public BasicShellObject(IntPtr shellObjectPIDL)
            : this(shellObjectPIDL, false) { }

        public BasicShellObject(IntPtr shellObjectPIDL, bool pidlSelfCreated) : base()
        {
            IntPtr ownPIDL = (pidlSelfCreated ? shellObjectPIDL : PIDLManager.Clone(shellObjectPIDL));

            this.absolutePIDL = ownPIDL;
            this.relativePIDL = PIDLManager.FindLastID(ownPIDL);
        }

        public BasicShellObjectCollection GetFolderItemCollection()
        {
            // TODO: Check, whether we are a folder anyhow :-)
            return new BasicShellObjectCollection(this.AbsolutePIDL,
                                                  ShellAPI.SHCONTF.DefaultForTreeView,
                                                  ShellObjectPIDLComparer.SortMode.Ascending);
        }

        protected virtual string GetDisplayName()
        {
            return this.GetDisplayNameOf(true, ShellAPI.SHGDN.INFOLDER);
        }

        public virtual string GetDisplayNameOf(bool relativeToParentFolder, ShellAPI.SHGDN SHGDNFlags)
        {
            ShellAPI.STRRET strRet;
            IntPtr strRetPidl = IntPtr.Zero;

            if ((!relativeToParentFolder) || PIDLManager.IsDesktop(this.AbsolutePIDL))
            {
                strRetPidl = this.AbsolutePIDL;

                desktopFolderInstance.Get.GetDisplayNameOf(strRetPidl, SHGDNFlags, out strRet);
            }
            else
            {
                IntPtr parentPidl = PIDLManager.CreateParentPIDL(this.AbsolutePIDL);
                ShellAPI.IShellFolder parentFolder = desktopFolderInstance.GetIShellFolder(parentPidl);

                strRetPidl = this.RelativePIDL;

                parentFolder.GetDisplayNameOf(strRetPidl, SHGDNFlags, out strRet);

                Marshal.ReleaseComObject(parentFolder); // TODO: Put IShellInterface into disposible class
                PIDLManager.Free(parentPidl);
            }

            // TODO: Won't work using W2K
            // TODO: Converting functions into STRRET.ToString (struct)
            ShellAPI.StrRetToBSTR(ref strRet, this.AbsolutePIDL, out string result);

            return result;
        }

        public virtual WinAPI.IDataObject GetIDataObject()
        {
            IntPtr ppv = IntPtr.Zero;
            uint reserved = 0;
            int hResult = 0;

            if (PIDLManager.IsDesktop(this.AbsolutePIDL))
            {
                hResult = desktopFolderInstance.Get.GetUIObjectOf(IntPtr.Zero, 0, null, ref ShellAPI.IID_IDataObject, ref reserved, out ppv);
            }
            else
            {
                IntPtr parentPidl = PIDLManager.CreateParentPIDL(this.AbsolutePIDL);

                try
                {
                    ShellAPI.IShellFolder parentFolder = desktopFolderInstance.GetIShellFolder(parentPidl);
                    IntPtr[] pidls = { this.RelativePIDL };

                    hResult = parentFolder.GetUIObjectOf(IntPtr.Zero, 1, pidls, ref ShellAPI.IID_IDataObject, ref reserved, out ppv);
                }
                finally
                {
                    PIDLManager.Free(parentPidl);
                }
            }
            // TODO: Refactor the code above

            //			Marshal.ThrowExceptionForHR(hResult);

            return ((ppv != IntPtr.Zero) ? (Marshal.GetObjectForIUnknown(ppv) as WinAPI.IDataObject) : null);
        }

        public virtual WinAPI.IDropTarget GetIDropTarget()
        {
            IntPtr ppv = IntPtr.Zero;
            uint reserved = 0;
            int hResult = 0;

            if (PIDLManager.IsDesktop(this.AbsolutePIDL))
            {
                hResult = desktopFolderInstance.Get.GetUIObjectOf(IntPtr.Zero, 0, null, ref ShellAPI.IID_IDropTarget, ref reserved, out ppv);
            }
            else
            {
                IntPtr parentPidl = PIDLManager.CreateParentPIDL(this.AbsolutePIDL);

                try
                {
                    ShellAPI.IShellFolder parentFolder = desktopFolderInstance.GetIShellFolder(parentPidl);
                    IntPtr[] pidls = { this.RelativePIDL };

                    hResult = parentFolder.GetUIObjectOf(IntPtr.Zero, 1, pidls, ref ShellAPI.IID_IDropTarget, ref reserved, out ppv);
                }
                finally
                {
                    PIDLManager.Free(parentPidl);
                }
            }
            // TODO: Refactor the code above

            //			Marshal.ThrowExceptionForHR(hResult);
            return ((ppv != IntPtr.Zero) ? (Marshal.GetObjectForIUnknown(ppv) as WinAPI.IDropTarget) : null);
        }

        public virtual ShellAPI.IContextMenu GetIContextMenu()
        {
            IntPtr ppv = IntPtr.Zero;
            uint reserved = 0;
            int hResult = 0;

            if (PIDLManager.IsDesktop(this.AbsolutePIDL))
            {
                hResult = desktopFolderInstance.Get.GetUIObjectOf(IntPtr.Zero, 0, null, ref ShellAPI.IID_IContextMenu, ref reserved, out ppv);
            }
            else
            {
                IntPtr parentPidl = PIDLManager.CreateParentPIDL(this.AbsolutePIDL);

                try
                {
                    ShellAPI.IShellFolder parentFolder = desktopFolderInstance.GetIShellFolder(parentPidl);
                    IntPtr[] pidls = { this.RelativePIDL };

                    hResult = parentFolder.GetUIObjectOf(IntPtr.Zero, 1, pidls, ref ShellAPI.IID_IContextMenu, ref reserved, out ppv);
                }
                finally
                {
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
                return this.absolutePIDL;
            }
        }

        public IntPtr RelativePIDL {
            get {
                return this.relativePIDL;
            }
        }

        public string DisplayName { get { return GetDisplayName(); } }


        public bool AttachFileInfoThread(IFileInfoThread fileInfoThread)
        {
            if (this.fileInfoThread == null)
            {
                this.fileInfoThread = fileInfoThread;
                return true;
            }
            else
            {
                return false;
            }
        }

        public void DetachFileInfoThread(IFileInfoThread fileInfoThread)
        {
            if (this.fileInfoThread != fileInfoThread)
            {
                throw new ArgumentException("electrifier.Core.Shell32.BasicShellObject.DetachFileInfoThread " +
                    "was called with invalid IFileInfoThread instance");
            }
            else
            {
                this.fileInfoThread = null;
            }
        }

        public void UpdateFileInfo(IFileInfoThread fileInfoThread, IconManager.FileInfoThreadResults fileInfoThreadResults)
        {
            if (this.fileInfoThread != fileInfoThread)
            {
                throw new ArgumentException("electrifier.Core.Shell32.BasicShellObject.UpdateFileInfo " +
                    "was called with invalid IFileInfoThread instance");
            }
            else
            {
                if (FileInfoUpdated != null)
                {
                    FileInfoUpdatedEventArgs args = new FileInfoUpdatedEventArgs(fileInfoThread,
                        fileInfoThreadResults);

                    FileInfoUpdated(this, args);
                }
            }
        }

        public event FileInfoUpdatedHandler FileInfoUpdated;
        #endregion
    }
}
