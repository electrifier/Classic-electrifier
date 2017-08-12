using System;
using System.Windows.Forms;

using electrifier.Core.Controls;
using electrifier.Core.Services;
using electrifier.Core.WindowsShell.Services;
using electrifier.Win32API;

namespace electrifier.Core.WindowsShell.Controls
{
    /// <summary>
    /// Summary for ShellListView.
    /// </summary>

    [Obsolete("Use ExplorerBrowser instead.")]
    public class ShellListView : electrifier.Core.Controls.ExtListView,
        electrifier.Core.WindowsShell.IShellObjectCollection
    {
        protected static IconManager iconManager = (IconManager)ServiceManager.Services.GetService(typeof(IconManager));
        protected IntPtr browsingFolder = IntPtr.Zero;

        public ShellListView() : base()
        {
            this.SmallSystemImageList = iconManager.SmallImageList;
            this.NormalSystemImageList = iconManager.LargeImageList;

            this.ItemDrag += new ItemDragEventHandler(this.ShellListView_ItemDrag);
        }

        public void SetBrowsingFolder(object sender, ShellAPI.CSIDL shellObjectCSIDL)
        {
            SetBrowsingFolder(sender, PIDLManager.CreateFromCSIDL(shellObjectCSIDL), true);
        }

        public void SetBrowsingFolder(object sender, string shellObjectFullPath)
        {
            SetBrowsingFolder(sender, PIDLManager.CreateFromPathW(shellObjectFullPath), true);
        }

        public void SetBrowsingFolder(object sender, IntPtr shellObjectPIDL)
        {
            SetBrowsingFolder(sender, shellObjectPIDL, false);
        }

        public void SetBrowsingFolder(object sender, IntPtr shellObjectPIDL, bool pidlSelfCreated)
        {
            // Update browsingFolder variable
            if (this.browsingFolder != IntPtr.Zero)
                PIDLManager.Free(this.browsingFolder);
            this.browsingFolder = (pidlSelfCreated ? shellObjectPIDL : PIDLManager.Clone(shellObjectPIDL));

            // Get the collection of shell-items and add them
            BasicShellObjectCollection collection = new BasicShellObjectCollection(shellObjectPIDL,
                ShellAPI.SHCONTF.DefaultForListView, ShellObjectPIDLComparer.SortMode.Unsorted);
            ExtListViewItem[] items = new ExtListViewItem[collection.Count];
            int index = 0;

            foreach (BasicShellObject shellObject in collection)
            {
                items[index++] = new ShellListViewItem(shellObject);
            }

            this.Items.Clear();      // TODO: Dispose all items!!!
            this.Items.AddRange(items);

            // Create a file info thread to gather visual info for all items
            //IconManager.FileInfoThread fileInfoThread = new IconManager.FileInfoThread(collection);
        }

        private void ShellListView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            // TODO: Enumerate available DragDropEffects!
            DoDragDrop(e.Item, DragDropEffects.Move);
        }

        #region IShellObjectCollection Member
        public int Count {
            get {
                return this.Items.Count;
            }
        }

        public IShellObjectCollectionEnumerator GetEnumerator()
        {
            // TODO:  Implementierung von ShellListView.GetEnumerator hinzufügen
            return null;
        }
        #endregion
    }
}
