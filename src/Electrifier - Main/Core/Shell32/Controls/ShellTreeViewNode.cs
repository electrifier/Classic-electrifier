using System;
using System.Windows.Forms;

using electrifier.Core.Controls;
using electrifier.Core.Services;
using electrifier.Core.Shell32.Services;
using electrifier.Win32API;

namespace electrifier.Core.Shell32.Controls {
	/// <summary>
	/// Zusammenfassung für ShellTreeViewNode.
	/// </summary>
	public class ShellTreeViewNode : ExtTreeViewNode, IShellObject {
		protected static IconManager                 iconManager      = ServiceManager.Services.GetService(typeof(IconManager)) as IconManager;
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

		public ShellTreeViewNode(BasicShellObject basicShellObject)
			: base() {
			this.basicShellObject = basicShellObject;
			this.nodes = new ShellTreeViewNodeCollection(base.Nodes);

			this.Text = DisplayName;
			this.IsShownExpandable = true;
			this.ImageIndex = iconManager.ClosedFolderIndex;
			this.SelectedImageIndex = iconManager.OpenedFolderIndex;

			FileInfoUpdated += new FileInfoUpdatedHandler(IShellObject_FileInfoUpdated);
		}

		protected void IShellObject_FileInfoUpdated(object source, FileInfoUpdatedEventArgs e) {
			if (e.FileInfoThreadResults.ValidValues == IconManager.FileInfoThreadParams.NONE)
				return;

			base.TreeView.BeginInvoke((MethodInvoker)(() => {		// TODO: InvokeRequired
				if (e.FileInfoThreadResults.ValidValues.HasFlag(IconManager.FileInfoThreadParams.ImageIndex))
					this.ImageIndex = e.FileInfoThreadResults.ImageIndex;

				if (e.FileInfoThreadResults.ValidValues.HasFlag(IconManager.FileInfoThreadParams.SelectedImageIndex))
					this.SelectedImageIndex = e.FileInfoThreadResults.SelectedImageIndex;

				if (e.FileInfoThreadResults.ValidValues.HasFlag(IconManager.FileInfoThreadParams.OverlayIndex)) {
					this.OverlayIndex = e.FileInfoThreadResults.OverlayIndex;

					uint x;
					if (this.OverlayIndex != 0)
						x = this.OverlayIndex;

					//if (this.OverlayIndex == 0)
					//	this.OverlayIndex = (uint)this.ImageIndex & 0xF;

				}

				if (e.FileInfoThreadResults.ValidValues.HasFlag(IconManager.FileInfoThreadParams.HasSubfolder)) {
					this.IsShownExpandable = e.FileInfoThreadResults.HasSubfolder;
					// TODO: Namen in FileInfoThreadParams und FileInfoThreadResults angleichen!
				}

				if (e.FileInfoThreadResults.ValidValues.HasFlag(IconManager.FileInfoThreadParams.IsHidden)) {
					//this.IsVisible = e.FileInfoThreadResults.IsHidden;
					//this.
				}
			}));
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
			// Check if this node itself is requested
			if (PIDLManager.IsEqual(this.AbsolutePIDL, shellObjectPIDL))
				return this;

			// Then test whether the given PIDL anyhow derives from this node
			if (PIDLManager.IsParent(this.AbsolutePIDL, shellObjectPIDL, false)) {

				// Now walk through the tree recursively and find the requested node
				ShellTreeViewNode actNode = this.FirstNode;
				
				do {
					if (PIDLManager.IsEqual(actNode.AbsolutePIDL, shellObjectPIDL))
						return actNode;

					if (PIDLManager.IsParent(actNode.AbsolutePIDL, shellObjectPIDL, false)) {
						if (actNode.Nodes.Count == 0)
							actNode.Expand();

						return actNode.FindChildNodeByPIDL(shellObjectPIDL);
					}
				} while ((actNode = actNode.NextNode) != null);

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

		public void UpdateFileInfo(IFileInfoThread fileInfoThread, IconManager.FileInfoThreadResults fileInfoThreadResults) {
			basicShellObject.UpdateFileInfo(fileInfoThread, fileInfoThreadResults);
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
