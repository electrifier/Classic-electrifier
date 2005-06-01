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
	/// Zusammenfassung für ExtTreeView.
	/// </summary>
	public class ExtTreeView : TreeView {
		protected  ExtTreeViewNodeCollection nodes          = null;
		public new ExtTreeViewNodeCollection Nodes          { get { return nodes; } }
		private    bool                      allowDrag      = false;
		public     bool                      AllowDrag      { get { return allowDrag; } set { allowDrag = value; } }
		private    bool                      isDragging     = false;
		public     bool                      IsDragging     { get { return isDragging; } }
		private    ExtTreeViewNode           dragTreeNode   = null;
		public     ExtTreeViewNode           DragTreeNode   { get { return dragTreeNode; } }
		private    Rectangle                 dragSourceRect = Rectangle.Empty;
		public     Rectangle                 DragSourceRect { get { return dragSourceRect; } }

		public ExtTreeView() : base() {
			nodes = new ExtTreeViewNodeCollection(base.Nodes);

			BeforeExpand += new TreeViewCancelEventHandler(ExtTreeView_BeforeExpand);
		}

		public IntPtr SystemImageList {
			get {
				return WinAPI.SendMessage(Handle, WMSG.TVM_GETIMAGELIST, TVSIL.NORMAL, IntPtr.Zero);
			}
			set {
				WinAPI.SendMessage(Handle, WMSG.TVM_SETIMAGELIST, TVSIL.NORMAL, value);
			}
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

		protected override void WndProc(ref Message m) 
		{
			switch(m.Msg) 
			{
				case (int)(WinAPI.WM.NOTIFY/* | WinAPI.WM.REFLECT*/): 
				{
					Win32API.NMHDR nmhdr = (Win32API.NMHDR)m.GetLParam(typeof(Win32API.NMHDR));

					switch(nmhdr.code) 
					{
//						case (int)WinAPI.TVN.BEGINDRAGA:
						case (int)WinAPI.TVN.BEGINDRAGW:
							OnBeginDragMessage(MouseButtons.Left, ref m);
							return;
//						case (int)WinAPI.TVN.BEGINRDRAGA:
						case (int)WinAPI.TVN.BEGINRDRAGW:
							OnBeginDragMessage(MouseButtons.Right, ref m);
							return;
					}

					break;
				} // case (int)(WinAPI.WM.NOTIFY | WinAPI.WM.REFLECT)
			} // switch(u.Msg)

			base.WndProc(ref m);
		}

		protected void OnBeginDragMessage(MouseButtons mouseButton, ref Message m) 
		{
//			try {
				Win32API.NMTREEVIEW nmTreeView = (Win32API.NMTREEVIEW)m.GetLParam(typeof(Win32API.NMTREEVIEW));
//			} catch (Exception e) {
//				string x = e.Message;
//
//			}
			OnItemDrag(new ItemDragEventArgs(mouseButton, 0));
		}

//		private void SetAllowDrag(bool value) {
//			allowDrag = value;
//
//			if(allowDrag) {
//				// Initialize Drag-Event handlers
//				MouseDown += new MouseEventHandler(ExtTreeView_MouseDown);
//				MouseMove += new MouseEventHandler(ExtTreeView_MouseMove);
//				MouseUp   += new MouseEventHandler(ExtTreeView_MouseUp);
//			} else {
//				// TODO: How to remove event handlers properly?!?
//				MouseDown -= new MouseEventHandler(ExtTreeView_MouseDown);
//				MouseMove -= new MouseEventHandler(ExtTreeView_MouseMove);
//				MouseUp   -= new MouseEventHandler(ExtTreeView_MouseUp);
//
//				ResetDragMembers();
//			}
//		}

//		/// <summary>
//		/// Reset any drag-related member variables
//		/// </summary>
//		private void ResetDragMembers() {
//			isDragging     = false;
//			dragTreeNode   = null;
//			dragSourceRect = Rectangle.Empty;
//		}

//		private void ExtTreeView_MouseDown(object sender, MouseEventArgs e) {
//			// If mouse is pointing to a valid node, initialize drags source rectangle
//			if((e.Button & (MouseButtons.Left | MouseButtons.Middle | MouseButtons.Right)) != 0) {
//				dragTreeNode = GetNodeAt(e.X, e.Y);
//
//				if(dragTreeNode != null) {
//					Size  dragSize = SystemInformation.DragSize;
//					Point srcPoint = new Point((e.X - (dragSize.Width / 2)), (e.Y - (dragSize.Height / 2)));
//					dragSourceRect = new Rectangle(srcPoint, dragSize);
//				} else {
//					dragSourceRect = Rectangle.Empty;
//				}
//			}
//		}

//		private void ExtTreeView_MouseMove(object sender, MouseEventArgs e) {
//			// Check if any mouse button was pressed while moving the mouse pointer
//			if((e.Button & (MouseButtons.Left | MouseButtons.Middle | MouseButtons.Right)) != 0) {
//				// Check if a node was selected previously and the drag source rectangle was initialized
//				if((dragTreeNode != null) && (dragSourceRect != Rectangle.Empty)) {
//					// Finally, if the mouse pointer leaved the drag source rectangle begin drag operation
//					if(!dragSourceRect.Contains(e.X, e.Y)) {
//						isDragging = true;
//
//						// TODO: Query allowed dragdropeffects from exttreeviewnode-item
//						DragDropEffects dndeffects = DoDragDrop(dragTreeNode, DragDropEffects.All);
//					}
//				}
//			}
//		}

//		private void ExtTreeView_MouseUp(object sender, MouseEventArgs e) {
//			ResetDragMembers();
//		}
	}
}
