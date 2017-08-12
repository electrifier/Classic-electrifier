using System;
using System.Windows.Forms;

using electrifier.Core.Controls;
using electrifier.Core.Services;
using electrifier.Core.WindowsShell.Services;
using electrifier.Win32API;

namespace electrifier.Core.WindowsShell.Controls
{
    /// <summary>
    /// Summary for ShellListViewItem.
    /// </summary>
    public class ShellListViewItem : ExtListViewItem, IShellObject
    {
        protected static IconManager iconManager = (IconManager)ServiceManager.Services.GetService(typeof(IconManager));
        protected BasicShellObject basicShellObject = null;

        public ShellListViewItem(ShellAPI.CSIDL shellObjectCSIDL)
            : this(PIDLManager.CreateFromCSIDL(shellObjectCSIDL), true) { }

        public ShellListViewItem(string shellObjectFullPath)
            : this(PIDLManager.CreateFromPathW(shellObjectFullPath), true) { }

        public ShellListViewItem(IntPtr shellObjectPIDL)
            : this(shellObjectPIDL, false) { }

        public ShellListViewItem(IntPtr shellObjectPIDL, bool pidlSelfCreated)
            : this(new BasicShellObject(shellObjectPIDL, pidlSelfCreated)) { }

        public ShellListViewItem(BasicShellObject shellObject) : base()
        {
            this.basicShellObject = shellObject;
            this.ImageIndex = iconManager.ClosedFolderIndex;
            FileInfoUpdated += new FileInfoUpdatedHandler(this.IShellObject_FileInfoUpdated);
            SetText(this.DisplayName);
        }

        protected void IShellObject_FileInfoUpdated(object source, FileInfoUpdatedEventArgs e)
        {
            if (this.ImageIndex != e.FileInfoThreadResults.ImageIndex)
            {
                this.ImageIndex = e.FileInfoThreadResults.ImageIndex;
            }
        }

        #region IShellObject Member
        public IntPtr AbsolutePIDL {
            get {
                return this.basicShellObject.AbsolutePIDL;
            }
        }

        public IntPtr RelativePIDL {
            get {
                return this.basicShellObject.RelativePIDL;
            }
        }

        public bool AttachFileInfoThread(IFileInfoThread fileInfoThread)
        {
            return this.basicShellObject.AttachFileInfoThread(fileInfoThread);
        }

        public void DetachFileInfoThread(IFileInfoThread fileInfoThread)
        {
            this.basicShellObject.DetachFileInfoThread(fileInfoThread);
        }

        public void UpdateFileInfo(IFileInfoThread fileInfoThread, IconManager.FileInfoThreadResults fileInfoThreadResults)
        {
            this.basicShellObject.UpdateFileInfo(fileInfoThread, fileInfoThreadResults);
        }

        public BasicShellObjectCollection GetFolderItemCollection()
        {
            return this.basicShellObject.GetFolderItemCollection();
        }

        public override WinAPI.IDataObject GetIDataObject()
        {
            return this.basicShellObject.GetIDataObject();
        }

        public override WinAPI.IDropTarget GetIDropTarget()
        {
            return this.basicShellObject.GetIDropTarget();
        }

        public ShellAPI.IContextMenu GetIContextMenu()
        {
            return this.basicShellObject.GetIContextMenu();
        }

        public string DisplayName { get { return this.basicShellObject.DisplayName; } }

        public string GetDisplayNameOf(bool relativeToParentFolder, ShellAPI.SHGDN SHGDNFlags)
        {
            return this.basicShellObject.GetDisplayNameOf(relativeToParentFolder, SHGDNFlags);
        }

        public event FileInfoUpdatedHandler FileInfoUpdated {
            add {
                this.basicShellObject.FileInfoUpdated += value;
            }
            remove {
                this.basicShellObject.FileInfoUpdated -= value;
            }
        }
        #endregion
    }
}
