//	<file>
//		<copyright see="www.electrifier.org"/>
//		<license   see="www.electrifier.org"/>
//		<owner     name="Thorsten Jung" email="taj@electrifier.org"/>
//		<version   value="$Id: ExtTreeView.cs,v 1.8 2004/09/10 15:21:53 taj bender Exp $"/>
//	</file>

using System;
using System.Drawing;
using System.Windows.Forms;

using Electrifier.Win32API;

namespace Electrifier.Core.Controls {
	/// <summary>
	/// ExtTreeView is based on System.Windows.Forms.TreeView but provides additional functionality
	/// like enhanced Drag and Drop support and the use of the globally unique system imagelist
	/// instead of System.Windows.Forms.ImageList
	/// </summary>
	public class ExtTreeView : TreeView {
		/// <summary>
		/// Collection of ExtTreeViewNode items hosted by this TreeView
		/// </summary>
		protected  ExtTreeViewNodeCollection nodes = null;
		public new ExtTreeViewNodeCollection Nodes { get { return this.nodes; } }

		/// <summary>
		/// 
		/// </summary>
		private Timer dragAutoScrollTimer = new Timer();
		private bool  dragAutoScroll      = true;
		public  bool  DragAutoScroll      { get { return this.dragAutoScroll; } set { this.dragAutoScroll = value; } }

		/// <summary>
		/// 
		/// </summary>
		private bool dragAutoExpand = true;
		public  bool DragAutoExpand { get { return this.dragAutoExpand; } set { this.dragAutoExpand = value; } }

		/// <summary>
		/// System's ImageList used for rendering the node icons
		/// </summary>
		public IntPtr SystemImageList {
			get {
				return WinAPI.SendMessage(this.Handle, WMSG.TVM_GETIMAGELIST, TVSIL.NORMAL, IntPtr.Zero);
			}
			set {
				WinAPI.SendMessage(this.Handle, WMSG.TVM_SETIMAGELIST, TVSIL.NORMAL, value);
			}
		}

		#region Overriden members to ensure type strictness

		public new ExtTreeViewNode GetNodeAt(Point point) {
			return base.GetNodeAt(point) as ExtTreeViewNode;
		}

		public new ExtTreeViewNode GetNodeAt(int x, int y) {
			return base.GetNodeAt(x, y) as ExtTreeViewNode;
		}

		#endregion

		/// <summary>
		/// Default Constructor
		/// </summary>
		public ExtTreeView() : base() {
			this.nodes = new ExtTreeViewNodeCollection(base.Nodes);

			// 
			this.BeforeExpand += new TreeViewCancelEventHandler(ExtTreeView_BeforeExpand);

			// Initialize drag and drop-event handlers
			this.ItemDrag  += new ItemDragEventHandler(ExtTreeView_ItemDrag);
			this.DragEnter += new DragEventHandler(ExtTreeView_DragEnter);
			this.DragLeave += new EventHandler(ExtTreeView_DragLeave);
			this.DragOver  += new DragEventHandler(ExtTreeView_DragOver);
			this.DragDrop  += new DragEventHandler(ExtTreeView_DragDrop);

			// Initialize drag and drop auto-scroll
			this.dragAutoScrollTimer.Interval = 200;
			this.dragAutoScrollTimer.Enabled  = false;
			this.dragAutoScrollTimer.Tick    += new EventHandler(dragAutoScrollTimer_Tick);
		} // public ExtTreeView() : base()

		private void ExtTreeView_BeforeExpand(object sender, TreeViewCancelEventArgs e) {
			// TODO: Event ueberschreiben und casting vermeiden!
			ExtTreeViewNode node = e.Node as ExtTreeViewNode;

			if(node != null)
				e.Cancel = !node.IsExpandable;
		} // private void ExtTreeView_BeforeExpand(object sender, TreeViewCancelEventArgs e)

		/// <summary>
		/// ItemDrag-event-handler initiates an drag and drop operation
		/// </summary>
		/// <param name="sender">Sender's reference</param>
		/// <param name="e">ItemDragEventArgs information</param>
		private void ExtTreeView_ItemDrag(object sender, ItemDragEventArgs e) {
			ExtTreeViewNode dragNode = e.Item as ExtTreeViewNode;

			if(dragNode != null) {
				Point     mousePosition      = this.PointToClient(Control.MousePosition);
				Bitmap    dragBitmap         = dragNode.CreateNodeBitmap(Win32API.ILD.NORMAL);
				ImageList dragImageList      = new ImageList();
				bool      dragImageListBegun = false;

				try {
					// Create drag image's image list
					dragImageList.ImageSize = new Size(dragBitmap.Width, dragBitmap.Height);
					dragImageList.Images.Add(dragBitmap);

					// Enter drag and drop loop
					dragImageListBegun = WinAPI.ImageList_BeginDrag(dragImageList.Handle, 0,
						(mousePosition.X + this.Indent - dragNode.Bounds.Left),
						(mousePosition.Y - dragNode.Bounds.Top));

					// TODO: Gather object to be given to DoDragDrop
					// TODO: Gather Drag and Drop effects allowed for this object
					this.DoDragDrop(dragNode, DragDropEffects.All);
				} finally {
					if(dragImageListBegun)
						WinAPI.ImageList_EndDrag();

					dragImageList.Dispose();
					dragBitmap.Dispose();
				}
			}
		} // private void ExtTreeView_ItemDrag(object sender, ItemDragEventArgs e)

		private void ExtTreeView_DragEnter(object sender, DragEventArgs e) {
			WinAPI.ImageList_DragEnter(this.Handle, (e.X - this.Left), (e.Y - this.Top));

			if(this.DragAutoScroll)
				this.dragAutoScrollTimer.Enabled = true;
		} // private void ExtTreeView_DragEnter(object sender, DragEventArgs e)

		private void ExtTreeView_DragLeave(object sender, EventArgs e) {
			WinAPI.ImageList_DragLeave(this.Handle);

			if(this.dragAutoScrollTimer.Enabled)
				this.dragAutoScrollTimer.Enabled = false;
		} // private void ExtTreeView_DragLeave(object sender, EventArgs e)

		private void ExtTreeView_DragOver(object sender, DragEventArgs e) {
			Point pos = this.PointToClient(new Point(e.X, e.Y));
			WinAPI.ImageList_DragMove((pos.X - this.Left), (pos.Y - this.Top));
		} // private void ExtTreeView_DragOver(object sender, DragEventArgs e)

		private void ExtTreeView_DragDrop(object sender, DragEventArgs e) {
			WinAPI.ImageList_DragLeave(this.Handle);

			// TODO: If frop successful, then...
			if(this.dragAutoScrollTimer.Enabled)
				this.dragAutoScrollTimer.Enabled = false;
		} // private void ExtTreeView_DragDrop(object sender, DragEventArgs e)

		private void dragAutoScrollTimer_Tick(object sender, EventArgs e) {
			Point           mousePos = this.PointToClient(Control.MousePosition);
			ExtTreeViewNode node     = this.GetNodeAt(mousePos);

			// TODO: Dual-speed scrolling!?!
			// TODO: Mouse-wheel support!!!
			if(node != null) {
				// Do vertical scroll if necessary
				if(mousePos.Y <= node.Bounds.Height) {
					WinAPI.ImageList_DragShowNolock(false);

					WinAPI.SendMessage(this.Handle, WinAPI.WM.VSCROLL, WinAPI.SB.LINEUP, IntPtr.Zero);

					WinAPI.ImageList_DragShowNolock(true);
				} else if (mousePos.Y >= (this.ClientRectangle.Height - node.Bounds.Height)) {
					WinAPI.ImageList_DragShowNolock(false);

					WinAPI.SendMessage(this.Handle, WinAPI.WM.VSCROLL, WinAPI.SB.LINEDOWN, IntPtr.Zero);

					WinAPI.ImageList_DragShowNolock(true);
				}

				// Do horizontal scroll if necessary
				if(mousePos.X <= node.Bounds.Height) {
					WinAPI.ImageList_DragShowNolock(false);

					WinAPI.SendMessage(this.Handle, WinAPI.WM.HSCROLL, WinAPI.SB.LINELEFT, IntPtr.Zero);

					WinAPI.ImageList_DragShowNolock(true);
				} else if (mousePos.X >= (this.ClientRectangle.Width - node.Bounds.Height)) {
					WinAPI.ImageList_DragShowNolock(false);

					WinAPI.SendMessage(this.Handle, WinAPI.WM.HSCROLL, WinAPI.SB.LINERIGHT, IntPtr.Zero);

					WinAPI.ImageList_DragShowNolock(true);
				}
			}
		}	// private void dragAutoScrollTimer_Tick(object sender, EventArgs e)

	}
}
