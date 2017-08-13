using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using electrifier.Core.Controls;
using electrifier.Core.Services;
using electrifier.Core.WindowsShell.Services;
using electrifier.Win32API;

namespace electrifier.Core.WindowsShell.Controls
{
    /// <summary>
    /// Summary for ShellTreeView.
    /// </summary>

    [Obsolete("Use ExplorerBrowser instead.")]
    public class ShellTreeView : electrifier.Core.Controls.ExtTreeView
    {
        protected static IconManager iconManager = ServiceManager.Services.GetService(typeof(IconManager)) as IconManager;
        protected ShellTreeViewNode rootNode = null;
        protected new ShellTreeViewNodeCollection nodes = null;
        public new ShellTreeViewNodeCollection Nodes { get { return this.nodes; } }

        public new ShellTreeViewNode SelectedNode {
            get => base.SelectedNode as ShellTreeViewNode;
            set { base.SelectedNode = value; }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public new ShellTreeViewNode GetNodeAt(int x, int y)
        {
            return base.GetNodeAt(x, y) as ShellTreeViewNode;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public new ShellTreeViewNode GetNodeAt(Point pt)
        {
            return base.GetNodeAt(pt) as ShellTreeViewNode;
        }

        public ShellTreeView(ShellAPI.CSIDL shellObjectCSIDL)
            : this(PIDLManager.CreateFromCSIDL(shellObjectCSIDL), true) { }

        public ShellTreeView(string shellObjectFullPath)
            : this(PIDLManager.CreateFromPathW(shellObjectFullPath), true) { }

        public ShellTreeView(IntPtr shellObjectPIDL)
            : this(shellObjectPIDL, false) { }

        public ShellTreeView(IntPtr pidl, bool pidlSelfCreated) : base()
        {
            // Initialize underlying ExtTreeView-component
            this.rootNode = new ShellTreeViewNode(pidl, pidlSelfCreated);
            this.nodes = new ShellTreeViewNodeCollection(base.Nodes);
            this.SystemImageList = iconManager.SmallImageList;
            this.HideSelection = false;
            this.ShowRootLines = false;

            this.Nodes.Add(this.rootNode);
            this.rootNode.Expand();
            if (this.rootNode.FirstNode != null)
            {
                this.rootNode.FirstNode.Expand();
                this.SelectedNode = this.rootNode.FirstNode;
            }

            // Create a file info thread to gather visual info for root item
            IconManager.FileInfoThread fileInfoThread = new IconManager.FileInfoThread(this.rootNode);

            this.MouseUp += new MouseEventHandler(this.ShellTreeView_MouseUp);

            this.Font = System.Drawing.SystemFonts.IconTitleFont;

            this.BorderStyle = BorderStyle.None;

            this.EnableThemeStyles();
        }

        public ShellTreeViewNode FindNodeByPIDL(IntPtr shellObjectPIDL)
        {
            this.BeginUpdate();

            try
            {
                // Check if the root node is requested
                if (PIDLManager.IsEqual(this.rootNode.AbsolutePIDL, shellObjectPIDL))
                    return this.rootNode;

                // Then test whether the given PIDL anyhow derives from root node
                if (PIDLManager.IsParent(this.rootNode.AbsolutePIDL, shellObjectPIDL, false))
                {

                    // Now walk through the tree recursively and find the requested node
                    ShellTreeViewNode actNode = this.rootNode.FirstNode;

                    while (null != actNode)
                    {
                        if (PIDLManager.IsEqual(actNode.AbsolutePIDL, shellObjectPIDL))
                            return actNode;

                        if (PIDLManager.IsParent(actNode.AbsolutePIDL, shellObjectPIDL, false))
                        {
                            if (actNode.Nodes.Count == 0)
                                actNode.Expand();

                            return actNode.FindChildNodeByPIDL(shellObjectPIDL);
                        }

                        actNode = actNode.NextNode;
                    }
                }
            }
            finally
            {
                this.EndUpdate();
            }

            return null;
        }

        private void ShellTreeView_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                // TODO: Refactor, refactor, refactor and move to TreeViewNode and others where appropriate
                Point mousePoint = new Point(e.X, e.Y);
                ShellTreeViewNode node = this.GetNodeAt(mousePoint);

                if (node != null)
                {
                    ShellAPI.IContextMenu ctxtMenu = node.GetIContextMenu();
                    IntPtr hMenu = WinAPI.CreatePopupMenu();

                    if ((ctxtMenu != null) && (hMenu != IntPtr.Zero))
                    {
                        Point scrPoint = this.PointToScreen(mousePoint);

                        ctxtMenu.QueryContextMenu(hMenu, 0, 1, 0x7FFF, ShellAPI.CMF.NORMAL | ShellAPI.CMF.EXPLORE);

                        WinAPI.TPM tpmFlags = WinAPI.TPM.LEFTALIGN | WinAPI.TPM.RETURNCMD | WinAPI.TPM.RIGHTBUTTON;

                        uint idCmd = WinAPI.TrackPopupMenu(hMenu, tpmFlags, scrPoint.X, scrPoint.Y, 0, this.Handle, IntPtr.Zero);

                        if (idCmd != 0)
                        {
                            // TODO: Refactor
                            ShellAPI.CMINVOKECOMMANDINFO cmici = new ShellAPI.CMINVOKECOMMANDINFO();
                            cmici.cbSize = Marshal.SizeOf(typeof(ShellAPI.CMINVOKECOMMANDINFO));
                            cmici.hwnd = this.Handle;
                            cmici.lpVerb = (IntPtr)(idCmd - 1);
                            cmici.nShow = Win32API.SW.SHOWNORMAL;

                            uint hRes = ctxtMenu.InvokeCommand(ref cmici);
                        }
                    }
                }
                // TODO: else do treeview-contextmenu
            }
        }
    }
}
