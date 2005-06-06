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
		private Timer dragAutoScrollTimer        = new Timer();
		private bool  dragAutoScrollEnabled      = true;
		public  bool  DragAutoScrollEnabled      { get { return this.dragAutoScrollEnabled; } set { this.dragAutoScrollEnabled = value; } }
		private int   dragAutoScrollSlowInterval = 200;
		public  int   DragAutoScrollSlowInterval { get { return this.dragAutoScrollSlowInterval; } set { this.dragAutoScrollSlowInterval = value; } }
		private int   dragAutoScrollFastInterval = 100;
		public  int   DragAutoScrollFastInterval { get { return this.dragAutoScrollFastInterval; } set { this.dragAutoScrollFastInterval = value; } }

		/// <summary>
		/// 
		/// </summary>
		private Timer           dragAutoExpandTimer    = new Timer();
		private bool            dragAutoExpandEnabled  = true;
		public  bool            DragAutoExpandEnabled  { get { return this.dragAutoExpandEnabled; } set { this.dragAutoExpandEnabled = value; } }
		private int             dragAutoExpandInterval = 750;
		public  int             DragAutoExpandInterval { get { return this.dragAutoExpandInterval; } set { this.dragAutoExpandInterval = value; } }
		private ExtTreeViewNode dragAutoExpandNode     = null;

		protected ExtTreeViewNode dropTargetNode = null;

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
			this.dragAutoScrollTimer.Enabled  = false;
			this.dragAutoScrollTimer.Tick    += new EventHandler(dragAutoScrollTimer_Tick);

			// Initialize drag and drop auto-expand
			this.dragAutoExpandTimer.Enabled  = false;
			this.dragAutoExpandTimer.Tick    +=new EventHandler(dragAutoExpandTimer_Tick);
		}

		private void ExtTreeView_BeforeExpand(object sender, TreeViewCancelEventArgs e) {
			// TODO: Event ueberschreiben und casting vermeiden!
			ExtTreeViewNode node = e.Node as ExtTreeViewNode;

			if(node != null)
				e.Cancel = !node.IsExpandable;
		}

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
		}

		private void ExtTreeView_DragEnter(object sender, DragEventArgs e) {
			WinAPI.ImageList_DragEnter(this.Handle, (e.X - this.Left), (e.Y - this.Top));

			// TODO: Call to ImageList_GetDragImage ?!?

			if(this.DragAutoScrollEnabled) {
				this.dragAutoScrollTimer.Stop();
				this.dragAutoScrollTimer.Interval = this.DragAutoScrollSlowInterval;
				this.dragAutoScrollTimer.Start();
			}
		}

		private void ExtTreeView_DragLeave(object sender, EventArgs e) {
			WinAPI.ImageList_DragLeave(this.Handle);

			this.dropTargetNode.IsDropHighlited = false;
			this.dragAutoScrollTimer.Stop();
			this.dragAutoExpandTimer.Stop();
		}

		private void ExtTreeView_DragOver(object sender, DragEventArgs e) {
			Point           mousePos          = this.PointToClient(new Point(e.X, e.Y));
			ExtTreeViewNode newDropTargetNode = this.GetNodeAt(mousePos);

			if(this.dropTargetNode != newDropTargetNode) {
				// Update TreeView item states regarding the new drop target node
				WinAPI.ImageList_DragShowNolock(false);

				if(this.dropTargetNode != null)
					this.dropTargetNode.IsDropHighlited = false;
				this.dropTargetNode = newDropTargetNode;
				this.dropTargetNode.IsDropHighlited = true;

				WinAPI.ImageList_DragShowNolock(true);

				// Initialize drag and drop auto expand members
				this.dragAutoExpandTimer.Stop();
				this.dragAutoExpandNode = newDropTargetNode;
				this.dragAutoExpandTimer.Interval = this.DragAutoExpandInterval;
				this.dragAutoExpandTimer.Start();
			}

			WinAPI.ImageList_DragMove((mousePos.X - this.Left), (mousePos.Y - this.Top));
		}

		private void ExtTreeView_DragDrop(object sender, DragEventArgs e) {
			WinAPI.ImageList_DragLeave(this.Handle);

			// TODO: If frop successful, then...
			this.dropTargetNode.IsDropHighlited = false;
			this.dragAutoScrollTimer.Stop();
			this.dragAutoExpandTimer.Stop();
		}

		/// <summary>
		/// All the autoscroll-related behaviour is managed within this method
		/// </summary>
		/// <param name="sender">Senders reference</param>
		/// <param name="e">Event arguments</param>
		private void dragAutoScrollTimer_Tick(object sender, EventArgs e) {
			Point           mousePos   = this.PointToClient(Control.MousePosition);
			ExtTreeViewNode node       = this.GetNodeAt(mousePos);
			bool            scrollFast = false;

			// TODO: Mouse-wheel support!!!
			if(node != null) {
				// Do vertical scroll if necessary
				if(mousePos.Y <= (2 * node.Bounds.Height)) {
					WinAPI.ImageList_DragShowNolock(false);

					WinAPI.SendMessage(this.Handle, WinAPI.WM.VSCROLL, WinAPI.SB.LINEUP, IntPtr.Zero);

					WinAPI.ImageList_DragShowNolock(true);

					if(mousePos.Y <= node.Bounds.Height)
						scrollFast = true;
				} else if (mousePos.Y >= (this.ClientRectangle.Height - (2 * node.Bounds.Height))) {
					WinAPI.ImageList_DragShowNolock(false);

					WinAPI.SendMessage(this.Handle, WinAPI.WM.VSCROLL, WinAPI.SB.LINEDOWN, IntPtr.Zero);

					WinAPI.ImageList_DragShowNolock(true);

					if(mousePos.Y >= (this.ClientRectangle.Height - node.Bounds.Height))
						scrollFast = true;
				}

				// Do horizontal scroll if necessary
				if(mousePos.X <= (2 * node.Bounds.Height)) {
					WinAPI.ImageList_DragShowNolock(false);

					WinAPI.SendMessage(this.Handle, WinAPI.WM.HSCROLL, WinAPI.SB.LINELEFT, IntPtr.Zero);

					WinAPI.ImageList_DragShowNolock(true);

					if(mousePos.X <= node.Bounds.Height)
						scrollFast = true;
				} else if (mousePos.X >= (this.ClientRectangle.Width - (2 * node.Bounds.Height))) {
					WinAPI.ImageList_DragShowNolock(false);

					WinAPI.SendMessage(this.Handle, WinAPI.WM.HSCROLL, WinAPI.SB.LINERIGHT, IntPtr.Zero);

					WinAPI.ImageList_DragShowNolock(true);

					if(mousePos.X >= (this.ClientRectangle.Width - node.Bounds.Height))
						scrollFast = true;
				}
			}

			this.dragAutoScrollTimer.Interval =
				(scrollFast ? this.DragAutoScrollFastInterval : this.DragAutoScrollSlowInterval);
		}

		private void dragAutoExpandTimer_Tick(object sender, EventArgs e) {
			Point           mousePos = this.PointToClient(Control.MousePosition);
			ExtTreeViewNode node     = this.GetNodeAt(mousePos);

			if(this.dragAutoExpandNode.Equals(node)) {
				// TODO: Expand-methode mit BeginInvoke aufrufen und somit Multi-Threaded zu machen :-)
				if(!node.IsExpanded)
					node.Expand();
			}
		}
	}
}
