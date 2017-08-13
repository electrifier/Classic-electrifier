using System;
using System.Windows.Forms;

using electrifier.Core.Controls;

namespace electrifier.Core.WindowsShell.Controls
{
    /// <summary>
    /// Summary for ShellTreeViewNodeCollection.
    /// </summary>
    public sealed class ShellTreeViewNodeCollection : ExtTreeViewNodeCollection
    {

        public ShellTreeViewNodeCollection(ExtTreeViewNodeCollection collection)
            : base(collection) { }

        public void AddCollection(BasicShellObjectCollection collection)
        {
            ShellTreeViewNode[] nodes = new ShellTreeViewNode[collection.Count];
            int index = 0;

            foreach (BasicShellObject shellObject in collection)
            {
                nodes[index++] = new ShellTreeViewNode(shellObject);
            }

            AddRange(nodes);
        }
    }
}
