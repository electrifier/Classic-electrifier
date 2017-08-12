using System;
using System.Drawing;
using System.Windows.Forms;

using electrifier.Win32API;

namespace electrifier.Core.Controls {
	/// <summary>
	/// Summary for ExtTreeViewNode.
	/// </summary>
	public class ExtTreeViewNode : TreeNode, IExtTreeViewNode {
		protected      ExtTreeViewNodeCollection nodes         = null;
		public new     ExtTreeViewNodeCollection Nodes         { get { return this.nodes; } }
		protected      bool                      handleCreated = false;
		public virtual bool                      IsExpandable  { get { return this.Nodes.Count > 0; } }

		#region Overriden properties to ensure type strictness

		public new ExtTreeView     TreeView        { get { return base.TreeView as ExtTreeView; } }
		public new ExtTreeViewNode NextNode        { get { return base.NextNode as ExtTreeViewNode; } }
		public new ExtTreeViewNode PrevNode        { get { return base.PrevNode as ExtTreeViewNode; } }
		public new ExtTreeViewNode Parent          { get { return base.Parent as ExtTreeViewNode; } }
		public new ExtTreeViewNode PrevVisibleNode { get { return base.PrevVisibleNode as ExtTreeViewNode; } }
		public new ExtTreeViewNode NextVisibleNode { get { return base.NextVisibleNode as ExtTreeViewNode; } }

		#endregion

		public virtual WinAPI.IDataObject GetIDataObject() {
			return null;	// Don't know how to do :-)
		}

		public virtual WinAPI.IDropTarget GetIDropTarget() {
			return null;	// Don't know how to do :-)
		}


		protected      bool isShownExpandable = false;
		public virtual bool IsShownExpandable {
			get {
				return this.isShownExpandable;
			}
			set {
				if (this.isShownExpandable != value) {
					this.isShownExpandable = value;

					if (this.handleCreated)
						this.UpdateIsShownExpandable();
				}
			}
		}

		protected      bool isDropHighlited = false;
		public virtual bool IsDropHighlited {
			get { 
				return this.isDropHighlited;
			}
			set {	// TODO: See UpdateIsShownExpandable() for behaviour when not added to treeview
				if(this.isDropHighlited != value) {
					this.isDropHighlited = value;

					TVItemEx tvItemEx  = new TVItemEx();
					tvItemEx.mask      = TVIF.STATE;
					tvItemEx.hItem     = this.HTreeItem.Handle;
					tvItemEx.state     = (value ? Win32API.TVIS.DROPHILITED : 0);
					tvItemEx.stateMask = Win32API.TVIS.DROPHILITED;
			
					WinAPI.SendMessage(this.TreeView.Handle, WMSG.TVM_SETITEM, 0, ref tvItemEx);

					this.TreeView.Invalidate(this.Bounds);
					//this.TreeView.Update();
				}
			}
		}

		//public new int ImageIndex {
		//	get {
		//		return base.ImageIndex;
		//	}
		//	set {
		//		if (base.ImageIndex != value)
		//			base.ImageIndex = value;
		//	}
		//}

		/// <summary>
		/// See https://msdn.microsoft.com/en-us/library/windows/desktop/bb773456(v=vs.85).aspx
		/// 
		/// The overlay image is superimposed over the item's icon image. Bits 8 through 11 of this member
		/// specify the one-based overlay image index. If these bits are zero, the item has no overlay image.
		/// </summary>
		protected uint overlayIndex = 0;
		public virtual uint OverlayIndex {
			get {
				return this.overlayIndex;
			}
			set {
				if (this.overlayIndex != value) {
					this.overlayIndex = value;

					TVItemEx tvItemEx = new TVItemEx();
					tvItemEx.mask = Win32API.TVIF.STATE;
					tvItemEx.hItem = this.HTreeItem.Handle;
					tvItemEx.state = ((TVIS)(value << 8) & Win32API.TVIS.OVERLAYMASK);
					tvItemEx.stateMask = Win32API.TVIS.OVERLAYMASK;

					//TVItemEx tvItemEx = new TVItemEx();
					//tvItemEx.mask = Win32API.TVIF.STATE | Win32API.TVIF.IMAGE /*| Win32API.TVIF.SELECTEDIMAGE*/;
					//tvItemEx.hItem = this.HTreeItem.Handle;
					//tvItemEx.state = ((TVIS)(value << 16) & Win32API.TVIS.OVERLAYMASK);
					//tvItemEx.iImage = (int)((uint)this.ImageIndex | value << 16);
					////tvItemEx.iSelectedImage = this.SelectedImageIndex;
					//tvItemEx.stateMask = Win32API.TVIS.OVERLAYMASK;

					WinAPI.SendMessage(this.TreeView.Handle, WMSG.TVM_SETITEM, 0, ref tvItemEx);  // TVM_SETITEMW

					this.TreeView.Invalidate(this.Bounds);
					//this.TreeView.Update();
				}
				//WinAPI.SendMessage()
				/*
							try {
				// TODO: ueberpruefen, ob kopieren ueberhaupt notwendig und code auf jeden ueberabreiten!
				pItem = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(Win32API.LVITEM)));
				Marshal.StructureToPtr(lvItem, pItem, true);

				returnCode = WinAPI.SendMessage(this.Handle, Win32API.WMSG.LVM_SETITEMSTATE, 0, pItem.ToInt32());
			} finally {
				Marshal.FreeHGlobal(IntPtr.Zero);
			}
				 */

				// SetItemState(hItem,INDEXTOOVERLAYMASK(nOverlayIndex), TVIS_OVERLAYMASK);
				// #define INDEXTOOVERLAYMASK(i) ((i) << 8)

			}
		}

		protected HTREEITEM hTreeItem = new HTREEITEM(IntPtr.Zero);
		/// <summary>
		/// Retrieve the handle to the TREEITEM structure of the node.
		/// NOTE: This property only works when the node is already added to an treeview!
		/// </summary>
 		protected HTREEITEM HTreeItem {
			get {
				if (!this.hTreeItem.IsValid) {
					// Is there a previous item?
					if (this.PrevNode != null) {
						this.hTreeItem = this.PrevNode.HTreeItem.NextNode(this.TreeView.Handle);
					} else {
						// No, so is there a parent item?
						if (this.Parent != null) {
							// TODO: Why should parents first node be the node searched for?
							this.hTreeItem = this.Parent.HTreeItem.FirstNode(this.TreeView.Handle);
						} else {
							// No, so we are the root item
							this.hTreeItem = HTREEITEM.GetRootItem(this.TreeView.Handle);
						}
					}
				}

				return this.hTreeItem;
			}
		}

		public ExtTreeViewNode() : base() {
			nodes = new ExtTreeViewNodeCollection(base.Nodes);
		}

		protected void UpdateIsShownExpandable() {
			TVItemEx tvItemEx  = new TVItemEx();
			tvItemEx.mask      = TVIF.CHILDREN;
			tvItemEx.hItem     = this.HTreeItem.Handle;
			tvItemEx.cChildren = this.isShownExpandable ? 1 : 0;
			
			WinAPI.SendMessage(this.TreeView.Handle, WMSG.TVM_SETITEM, 0, ref tvItemEx);
		}

		private void UpdateIsShownExpandable(bool causedByHasBeenAddedToTreeViewBy) {
			// By default, the node IS NOT shown expandable :-)
			if(causedByHasBeenAddedToTreeViewBy && this.isShownExpandable)
				this.UpdateIsShownExpandable();
		}

		///// <summary>
		///// Generates a Bitmap representation of this node
		///// </summary>
		///// <param name="iconDrawStyle">One of the ILD_-style constants for call to ImageList_Draw</param>
		///// <returns>The generated Bitmap</returns>
		//public Bitmap CreateNodeBitmap(Win32API.ILD iconDrawStyle) {
		//	if(this.handleCreated) {
		//		ExtTreeView treeView  = this.TreeView;
		//		Bitmap      bitmap    = new Bitmap((this.Bounds.Width + treeView.Indent), this.Bounds.Height);
		//		Graphics    graphics  = Graphics.FromImage(bitmap);
		//		IntPtr      hdcontext = graphics.GetHdc();

		//		WinAPI.ImageList_Draw(treeView.SystemImageList, this.ImageIndex, hdcontext, 0, 0, iconDrawStyle);

		//		graphics.ReleaseHdc(hdcontext);

		//		graphics.DrawString(this.Text, treeView.Font,
		//			new SolidBrush(treeView.ForeColor), (float)treeView.Indent, 1.0f);

		//		return bitmap;
		//	} else
		//		return null;
		//}

		#region IExtTreeViewNode Member
		// TODO: HandleCreated als Ersatz?!?
		public void HasBeenAddedToTreeViewBy(IExtTreeViewNodeCollection sender) {
			if(this.TreeView != null) {
				this.handleCreated = true;

				// Update properties already set but still not updated
				this.UpdateIsShownExpandable(true);
			} else {
				throw new InvalidOperationException("electrifier.Core.Controls.ExtTreeViewNode.HasBeenAddedToTreeViewBy: TreeView-property not yet initialized!");
			}
		}
		#endregion
	}
}
