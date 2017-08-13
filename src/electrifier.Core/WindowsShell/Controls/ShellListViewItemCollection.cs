using System;
using System.Windows.Forms;

namespace electrifier.Core.WindowsShell.Controls
{
    /// <summary>
    /// Summary for ShellListViewItemCollection.
    /// </summary>

    [Obsolete]
    public class ShellListViewItemCollection : ListView.ListViewItemCollection
    {
        public ShellListViewItemCollection(ShellListView owner) : base(owner)
        {
        }
    }
}
