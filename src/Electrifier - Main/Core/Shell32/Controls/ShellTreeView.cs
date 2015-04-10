using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using Electrifier.Core.Controls;
using Electrifier.Core.Services;
using Electrifier.Core.Shell32.Services;
using Electrifier.Win32API;

namespace Electrifier.Core.Shell32.Controls {
	/// <summary>
	/// Zusammenfassung für ShellTreeView.
	/// </summary>
	public class ShellTreeView : ExtTreeView {
		protected static IconManager                 iconManager = ServiceManager.Services.GetService(typeof(IconManager)) as IconManager;
		protected        ShellTreeViewNode           rootNode    = null;
		protected new    ShellTreeViewNodeCollection nodes       = null;
		public    new    ShellTreeViewNodeCollection Nodes       { get { return nodes; } }

		public    new    ShellTreeViewNode           SelectedNode {
			get { return base.SelectedNode as ShellTreeViewNode; }
			set { base.SelectedNode = value; }
		}

		public new ShellTreeViewNode GetNodeAt(int x, int y) {
			return base.GetNodeAt(x, y) as ShellTreeViewNode;
		}
		public new ShellTreeViewNode GetNodeAt(Point pt) {
			return base.GetNodeAt(pt) as ShellTreeViewNode;
		}

		public ShellTreeView(ShellAPI.CSIDL shellObjectCSIDL)
			: this(PIDLManager.CreateFromCSIDL(shellObjectCSIDL), true) {}

		public ShellTreeView(string shellObjectFullPath)
			: this(PIDLManager.CreateFromPathW(shellObjectFullPath), true) {}

		public ShellTreeView(IntPtr shellObjectPIDL)
			: this(shellObjectPIDL, false) {}

		public ShellTreeView(IntPtr pidl, bool pidlSelfCreated) : base() {
			// Initialize underlying ExtTreeView-component
			this.rootNode        = new ShellTreeViewNode(pidl, pidlSelfCreated);
			this.nodes           = new ShellTreeViewNodeCollection(base.Nodes);
			this.SystemImageList = iconManager.SmallImageList;
			this.HideSelection   = false;
			this.ShowRootLines   = false;

			this.Nodes.Add(rootNode);
			this.rootNode.Expand();
			if(this.rootNode.FirstNode != null) {
				this.rootNode.FirstNode.Expand();
				this.SelectedNode = rootNode.FirstNode;
			}

			// Create a file info thread to gather visual info for root item
			IconManager.FileInfoThread fileInfoThread = new IconManager.FileInfoThread(rootNode);

			this.MouseUp += new MouseEventHandler(ShellTreeView_MouseUp);

			this.BorderStyle = BorderStyle.None;

			this.EnableThemeStyles();
		}

		public ShellTreeViewNode FindNodeByPIDL(IntPtr shellObjectPIDL) {
			this.BeginUpdate();

			try {

				// First of all, test whether the given PIDL anyhow derives from our root node
				if(PIDLManager.IsParent(this.rootNode.AbsolutePIDL, shellObjectPIDL, false)) {
					// If we have luck, just the root node itself is requested
					if(PIDLManager.IsEqual(this.rootNode.AbsolutePIDL, shellObjectPIDL))
						return this.rootNode;

					ShellTreeViewNode actNode = this.rootNode.FirstNode;
					do {
						if(PIDLManager.IsEqual(actNode.AbsolutePIDL, shellObjectPIDL))
							return actNode;
						if(PIDLManager.IsParent(actNode.AbsolutePIDL, shellObjectPIDL, false)) {
							if(actNode.Nodes.Count == 0)
								actNode.Expand();

							return actNode.FindChildNodeByPIDL(shellObjectPIDL);
						}
					} while((actNode = actNode.NextNode) != null);
				}
			} finally {
				this.EndUpdate();
			}
			
			return null;
		}

		private void ShellTreeView_MouseUp(object sender, MouseEventArgs e) {
			if(e.Button == MouseButtons.Right) {
				// TODO: Refactor, refactor, refactor and move to TreeViewNode and others where appropriate
				Point             mousePoint = new Point(e.X, e.Y);
				ShellTreeViewNode node       = this.GetNodeAt(mousePoint);

				if(node != null) {
					ShellAPI.IContextMenu ctxtMenu = node.GetIContextMenu();
					IntPtr                hMenu    = WinAPI.CreatePopupMenu();

					if((ctxtMenu != null) && (hMenu != IntPtr.Zero)) {
						Point scrPoint = this.PointToScreen(mousePoint);

						ctxtMenu.QueryContextMenu(hMenu, 0, 1, 0x7FFF, ShellAPI.CMF.NORMAL | ShellAPI.CMF.EXPLORE);

						WinAPI.TPM tpmFlags = WinAPI.TPM.LEFTALIGN | WinAPI.TPM.RETURNCMD | WinAPI.TPM.RIGHTBUTTON;

						uint idCmd = WinAPI.TrackPopupMenu(hMenu, tpmFlags, scrPoint.X, scrPoint.Y, 0, this.Handle, IntPtr.Zero);

						if(idCmd != 0) {
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
				// else do treeview-contextmenu
			}
		}
	}
}
