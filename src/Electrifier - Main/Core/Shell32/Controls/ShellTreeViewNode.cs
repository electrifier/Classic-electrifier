//	<file>
//		<copyright see="www.electrifier.org"/>
//		<license   see="www.electrifier.org"/>
//		<owner     name="Thorsten Jung" email="taj@electrifier.org"/>
//		<version   value="$Id: ShellTreeViewNode.cs,v 1.20 2004/09/10 21:48:34 taj bender Exp $"/>
//	</file>

using System;
using System.Windows.Forms;

using Electrifier.Core.Controls;
using Electrifier.Core.Services;
using Electrifier.Core.Shell32.Services;
using Electrifier.Win32API;

namespace Electrifier.Core.Shell32.Controls {
	/// <summary>
	/// Zusammenfassung für ShellTreeViewNode.
	/// </summary>
	public class ShellTreeViewNode : ExtTreeViewNode, IShellObject {
		protected static IconManager                 iconManager      = (IconManager)ServiceManager.Services.GetService(typeof(IconManager));
		protected        BasicShellObject            basicShellObject = null;
		protected new    ShellTreeViewNodeCollection nodes            = null;

		public new ShellTreeViewNode FirstNode {
			get { return base.FirstNode as ShellTreeViewNode; }
		}

		public new ShellTreeViewNode NextNode {
			get { return base.NextNode as ShellTreeViewNode; }
		}

		public ShellTreeViewNode(ShellAPI.CSIDL shellObjectCSIDL)
			: this(PIDLManager.CreateFromCSIDL(shellObjectCSIDL), true) {}

		public ShellTreeViewNode(string shellObjectFullPath)
			: this(PIDLManager.CreateFromPathW(shellObjectFullPath), true) {}

		public ShellTreeViewNode(IntPtr shellObjectPIDL)
			: this(shellObjectPIDL, false) {}

		public ShellTreeViewNode(IntPtr shellObjectPIDL, bool pidlSelfCreated)
			: this(new BasicShellObject(shellObjectPIDL, pidlSelfCreated)) { }

		public ShellTreeViewNode(BasicShellObject shellObject) : base() {
			basicShellObject = shellObject;
			nodes            = new ShellTreeViewNodeCollection(base.Nodes);

			IsShownExpandable  = true;
			Text               = DisplayName;
			ImageIndex         = iconManager.ClosedFolderIndex;

			FileInfoUpdated += new FileInfoUpdatedHandler(IShellObject_FileInfoUpdated);
		}

		protected void IShellObject_FileInfoUpdated(object source, FileInfoUpdatedEventArgs e) {
/* TODO: RELAUNCH: Commented out due incompatibility
            if (this.imageindex != e.shfileinfo.iicon) {
                this.imageindex = e.shfileinfo.iicon;
            }
            // todo: request real openfolderindex
            // todo: only if tvif_selectedimage is set, an open image is available
            if (e.shfileinfo.iicon == iconmanager.closedfolderindex)
                this.selectedimageindex = iconmanager.openedfolderindex;
            else
                this.selectedimageindex = e.shfileinfo.iicon;
 */
		}

		public new ShellTreeViewNodeCollection Nodes {
			get {
				return nodes;
			}
		}

		public BasicShellObjectCollection GetFolderItemCollection() {
			return basicShellObject.GetFolderItemCollection();
		}

		/// <summary>
		/// Since IsExpandable property is only evaluated, when IsShownExpandable is set,
		/// the method sets IsShownExpandable temporarly to true, to force enumeration
		/// of child folder objects when <c>forceExpand</c> is set to true.
		/// This way we force the IsExpandable property to be evaluated; this is, cause the
		/// event handler of the windows api is only called, when IsShownExpandable is true.
		/// The IsShownExpandable-property after that is true when real child objects exist.
		/// When forceExpand is set to false, the standard Expand-method is called without
		/// doing magic things :-)
		/// </summary>
		public void Expand(bool forceExpand) {
			if(forceExpand) {
				IsShownExpandable = true;

				base.Expand();

				IsShownExpandable = IsExpandable;
			} else {
				base.Expand();
			}
		}

		public override bool IsExpandable {
			get {
				if(Nodes.Count == 0) {
					bool   resetTreeView = false;
					Cursor oldCursor     = null;

					try {
						if(TreeView != null) {
							resetTreeView = true;
							oldCursor     = TreeView.Cursor;

							TreeView.BeginUpdate();
							TreeView.Cursor = Cursors.WaitCursor;
						}

						// Get the collection of sub-items
						BasicShellObjectCollection collection = GetFolderItemCollection();

						if(collection.Count > 0) {
							// Add sub-items to treeview
							Nodes.AddCollection(collection);

							// Create a file info thread to gather visual info for all sub-items
							IconManager.FileInfoThread fileInfoThread = new IconManager.FileInfoThread(collection);
						}
					} finally {
						if(resetTreeView) {
							TreeView.Cursor = oldCursor;
							TreeView.EndUpdate();
						}
					}
				}

				return base.IsExpandable;
			}
		}

		public ShellTreeViewNode FindChildNodeByPIDL(IntPtr shellObjectPIDL) {
			// First of all, test whether the given PIDL anyhow derives from this node
			if(PIDLManager.IsParent(this.AbsolutePIDL, shellObjectPIDL, false)) {
				// If we have luck, just the this node itself is requested
				if(PIDLManager.IsEqual(this.AbsolutePIDL, shellObjectPIDL))
					return this;

				ShellTreeViewNode actNode = this.FirstNode;
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
			
			return null;
		}

		#region IShellObject Member
		public IntPtr AbsolutePIDL {
			get {
				return basicShellObject.AbsolutePIDL;
			}
		}

		public IntPtr RelativePIDL {
			get {
				return basicShellObject.RelativePIDL;
			}
		}

		public string DisplayName { get { return this.basicShellObject.DisplayName; } }

		public string GetDisplayNameOf(bool relativeToParentFolder, ShellAPI.SHGDN SHGDNFlags) {
			return this.basicShellObject.GetDisplayNameOf(relativeToParentFolder, SHGDNFlags);
		}

		public override WinAPI.IDataObject GetIDataObject() {
			return basicShellObject.GetIDataObject();					
		}

		public override WinAPI.IDropTarget GetIDropTarget() {
			return basicShellObject.GetIDropTarget();
		}

		public ShellAPI.IContextMenu GetIContextMenu() {
			return this.basicShellObject.GetIContextMenu();
		}

		public bool AttachFileInfoThread(IFileInfoThread fileInfoThread) {
			return basicShellObject.AttachFileInfoThread(fileInfoThread);
		}

		public void DetachFileInfoThread(IFileInfoThread fileInfoThread) {
			basicShellObject.DetachFileInfoThread(fileInfoThread);
		}

		public void UpdateFileInfo(IFileInfoThread fileInfoThread, ShellAPI.SHFILEINFO shFileInfo) {
			basicShellObject.UpdateFileInfo(fileInfoThread, shFileInfo);
		}

		public event FileInfoUpdatedHandler FileInfoUpdated {
			add {
				basicShellObject.FileInfoUpdated += value;
			}
			remove {
				basicShellObject.FileInfoUpdated -= value;
			}
		}
		#endregion
	}
}
