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
	/// Zusammenfassung f�r ExtTreeView.
	/// </summary>
	public class ExtTreeView : TreeView {
		/// <summary>
		/// Collection of ExtTreeViewNode items hosted by the TreeView
		/// </summary>
		protected  ExtTreeViewNodeCollection			nodes         = null;
		public new ExtTreeViewNodeCollection			Nodes         { get { return nodes; } }

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
		}

		#region Overriden member methods to ensure type strictness

		public new ExtTreeViewNode GetNodeAt(Point point) {
			return base.GetNodeAt(point) as ExtTreeViewNode;
		}

		public new ExtTreeViewNode GetNodeAt(int x, int y) {
			return base.GetNodeAt(x, y) as ExtTreeViewNode;
		}

		#endregion

		private void ExtTreeView_BeforeExpand(object sender, TreeViewCancelEventArgs e) {
			// TODO: Event ueberschreiben und casting vermeiden!
			ExtTreeViewNode node = e.Node as ExtTreeViewNode;

			if(node != null)
				e.Cancel = !node.IsExpandable;
		}

		private void ExtTreeView_ItemDrag(object sender, ItemDragEventArgs e) {
			ExtTreeViewNode dragNode = e.Item as ExtTreeViewNode;

			if(dragNode != null) {
				Point     mousePosition = this.PointToClient(Control.MousePosition);
				ImageList dragImageList = new ImageList();
				Rectangle dragBounds    = dragNode.Bounds;
				Bitmap    dragBitmap    = null;
				Graphics  dragGfx       = null;

				dragBounds.Width += this.Indent;

				try {
					// Render drag image using nodes image and text
					dragBitmap = new Bitmap(dragBounds.Width, dragBounds.Height);
					dragGfx    = Graphics.FromImage(dragBitmap);

					// TODO: Move node drawing code into node's class
					IntPtr hdc = dragGfx.GetHdc();
					WinAPI.ImageList_Draw(this.SystemImageList, dragNode.ImageIndex, hdc, 0, 0, 0);
					dragGfx.ReleaseHdc(hdc);

					dragGfx.DrawString(dragNode.Text, this.Font,
						new SolidBrush(this.ForeColor), (float)this.Indent, 1.0f);

					// Create drag image's image list
					dragImageList.ImageSize = new Size(dragBounds.Width, dragBounds.Height);
					dragImageList.Images.Add(dragBitmap);

					// Enter drag and drop loop
					WinAPI.ImageList_BeginDrag(dragImageList.Handle, 0,
						(mousePosition.X + this.Indent - dragBounds.Left), (mousePosition.Y - dragBounds.Top));
					this.DoDragDrop(dragNode, DragDropEffects.All);
					WinAPI.ImageList_EndDrag();
				}
				finally {
					dragImageList.Dispose();
					dragBitmap.Dispose();
				}
			}
		}

		private void ExtTreeView_DragEnter(object sender, DragEventArgs e) {
			WinAPI.ImageList_DragEnter(this.Handle, (e.X - this.Left), (e.Y - this.Top));
		}

		private void ExtTreeView_DragLeave(object sender, EventArgs e) {
			WinAPI.ImageList_DragLeave(this.Handle);
		}

		private void ExtTreeView_DragOver(object sender, DragEventArgs e) {
			Point pos = this.PointToClient(new Point(e.X, e.Y));
			WinAPI.ImageList_DragMove((pos.X - this.Left), (pos.Y - this.Top));
		}

		private void ExtTreeView_DragDrop(object sender, DragEventArgs e) {
			WinAPI.ImageList_DragLeave(this.Handle);
		}
	}
}
