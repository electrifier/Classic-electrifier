//	<file>
//		<copyright see="www.electrifier.org"/>
//		<license   see="www.electrifier.org"/>
//		<owner     name="Thorsten Jung" email="taj@electrifier.org"/>
//		<version   value="$Id: ExtTreeView.cs,v 1.8 2004/09/10 15:21:53 taj bender Exp $"/>
//	</file>

using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using Electrifier.Core.Services;
using Electrifier.Core.Shell32;
using Electrifier.Core.Shell32.Controls;
using Electrifier.Core.Shell32.Services;
using Electrifier.Win32API;

namespace Electrifier.Core.Controls {
	/// <summary>
	/// ExtTreeView is based on System.Windows.Forms.TreeView but provides additional functionality
	/// like enhanced Drag and Drop support and the use of the globally unique system imagelist
	/// instead of System.Windows.Forms.ImageList
	/// </summary>
	public class ExtTreeView : TreeView {
		protected static DesktopFolderInstance desktopFolderInstance = (DesktopFolderInstance)ServiceManager.Services.GetService(typeof(DesktopFolderInstance));

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

		public    ShellDragDropHelper ShellDragDropHelper { get { return this.shellDragDropHelper; } }
		protected ShellDragDropHelper shellDragDropHelper = null;

		/// <summary>
		/// System's ImageList used for rendering the node icons
		/// </summary>
		public IntPtr SystemImageList {
			get { return WinAPI.SendMessage(this.Handle, WMSG.TVM_GETIMAGELIST, TVSIL.NORMAL, IntPtr.Zero); }
			set { WinAPI.SendMessage(this.Handle, WMSG.TVM_SETIMAGELIST, TVSIL.NORMAL, value); }
		}

		#region Tree-View Control Extended Styles
		/// <summary>
		/// Tree-View Control Extended Styles
		/// <br/>See <a href="https://msdn.microsoft.com/de-de/library/windows/desktop/bb759981%28v=vs.85%29.aspx"/>
		/// </summary>
		[DllImport("user32.dll")]
		protected static extern TVS_EX SendMessage(IntPtr hWnd, Win32API.WMSG wMsg, TVS_EX wParam, TVS_EX lParam);

		[FlagsAttribute]
		protected enum TVS_EX : uint {
			None = 0,
			MULTISELECT = 0x0002,
			DOUBLEBUFFER = 0x0004,
			NOINDENTSTATE = 0x0008,
			RICHTOOLTIP = 0x0010,
			AUTOHSCROLL = 0x0020,
			FADEINOUTEXPANDOS = 0x0040,
			PARTIALCHECKBOXES = 0x0080,
			EXCLUSIONCHECKBOXES = 0x0100,
			DIMMEDCHECKBOXES = 0x0200,
			DRAWIMAGEASYNC = 0x0400,
		}

		protected TVS_EX ExtendedStyle {
			get { return ExtTreeView.SendMessage(this.Handle, WMSG.TVM_GETEXTENDEDSTYLE, TVS_EX.None, TVS_EX.None); }
			set { ExtTreeView.SendMessage(this.Handle, WMSG.TVM_SETEXTENDEDSTYLE, value, TVS_EX.None); }		// Don't know why only mask (wParam) is set, not (lParam) value, but works...
		}

		/// <summary>
		/// See < a href="http://www.programmershare.com/3772974/"/>
		/// 
		/// TODO: Using this makes the dragimages look crippled...
		/// </summary>
		public void EnableThemeStyles() {
			this.HotTracking = true;
			this.ShowLines = false;
			this.ItemHeight = this.ItemHeight + 4;			// TODO: +4 works, +5 not (and it should be 5)... Perhaps we have to use the large image list?!?

			WinAPI.SetWindowTheme(this.Handle, @"Explorer", null);
 
			this.ExtendedStyle |= TVS_EX.DOUBLEBUFFER | TVS_EX.FADEINOUTEXPANDOS;
		}
		#endregion

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
			this.shellDragDropHelper = new ShellDragDropHelper(this.Handle);

			// 
			this.BeforeExpand += new TreeViewCancelEventHandler(ExtTreeView_BeforeExpand);

			// Initialize drag and drop auto-scroll
			this.dragAutoScrollTimer.Enabled  = false;
			this.dragAutoScrollTimer.Tick    += new EventHandler(dragAutoScrollTimer_Tick);

			// Initialize drag and drop auto-expand
			this.dragAutoExpandTimer.Enabled  = false;
			this.dragAutoExpandTimer.Tick    +=new EventHandler(dragAutoExpandTimer_Tick);

			// Initialize drag and drop-event handlers
			this.ItemDrag +=
				new ItemDragEventHandler(this.ExtTreeView_ItemDrag);
			this.ShellDragDropHelper.DropTargetEnter +=
				new DropTargetEnterEventHandler(this.ShellDragDropHelper_DropTargetEnter);
			this.ShellDragDropHelper.DropTargetOver +=
				new DropTargetOverEventHandler(this.ShellDragDropHelper_DropTargetOver);
			this.ShellDragDropHelper.DropTargetLeave += 
				new DropTargetLeaveEventHandler(this.ShellDragDropHelper_DropTargetLeave);
			this.ShellDragDropHelper.DropTargetDrop += 
				new DropTargetDropEventHandler(this.ShellDragDropHelper_DropTargetDrop);
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
				WinAPI.IDataObject dataObject = dragNode.GetIDataObject();

				this.shellDragDropHelper.PrepareDragImage(dataObject);

				this.DoDragDrop(dataObject, (DragDropEffects.Copy | DragDropEffects.Move | DragDropEffects.Link));
			}
		}

		private DragDropEffects ShellDragDropHelper_DropTargetEnter(object source, DropTargetEventArgs e) {
			DragDropEffects effects = e.Effects;

			// TODO: Override WinAPI.POINT to get Point(.NET)-instance
			ExtTreeViewNode dropTargetNode = this.GetNodeAt(this.PointToClient(new Point(e.MousePos.X, e.MousePos.Y)));

			// TODO: Very experimental code this is:
			if(dropTargetNode != null) {
				try {
					WinAPI.IDropTarget dropTarget = dropTargetNode.GetIDropTarget();

					if(dropTarget != null) {
						dropTarget.DragEnter(e.DataObject, e.KeyState, e.MousePos, ref e.Effects);
						dropTarget.DragLeave();

						effects = e.Effects;
					}

				} catch (Exception ex) {
					System.Windows.Forms.MessageBox.Show(ex.StackTrace, ex.Message);
				}
			}

			if(this.DragAutoScrollEnabled) {
				this.dragAutoScrollTimer.Stop();
				this.dragAutoScrollTimer.Interval = this.DragAutoScrollSlowInterval;
				this.dragAutoScrollTimer.Start();
			}

			return effects;
		}

		private DragDropEffects ShellDragDropHelper_DropTargetOver(object source, DropTargetEventArgs e) {
			DragDropEffects effects = e.Effects;

			// TODO: Override WinAPI.POINT to get Point(.NET)-instance
			Point           mousePos          = this.PointToClient(new Point(e.MousePos.X, e.MousePos.Y));
			ExtTreeViewNode newDropTargetNode = this.GetNodeAt(mousePos);

			if(this.dropTargetNode != newDropTargetNode) {
				// Update TreeView item states regarding the new drop target node

				if(this.dropTargetNode != null)
					this.dropTargetNode.IsDropHighlited = false;
				this.dropTargetNode = newDropTargetNode;
				if(this.dropTargetNode != null)
					this.dropTargetNode.IsDropHighlited = true;

				// TODO: Very experimental code this is:
				if(newDropTargetNode != null) {
					int hResult = 0;

					try {
						WinAPI.IDropTarget dropTarget = newDropTargetNode.GetIDropTarget();

						if(dropTarget != null) {
							hResult = dropTarget.DragEnter(e.DataObject, e.KeyState, e.MousePos, ref e.Effects);
							hResult = dropTarget.DragOver(e.KeyState, e.MousePos, ref e.Effects);
							hResult = dropTarget.DragLeave();

							effects = e.Effects;
						}
						// TODO: else e.Effects = DragDropEffects.None;
					} catch (Exception ex) {
						string body = "EXCEPTION-TYPE: " + ex.GetType().FullName +
							            "\n=> TARGET-SITE: " + ex.TargetSite.ToString() +
							            "\n=> hResult: " + hResult.ToString() +
							            "\n=> STACKTRACE:\n" + ex.StackTrace.ToString() +
							            "\n=> KEYSTATE: " + e.KeyState.ToString() +
							            "\n=> MOUSEPOS: " + e.MousePos.ToString() +
							            "\n=> EFFECTS: " + e.Effects.ToString();
						
						System.Windows.Forms.MessageBox.Show(body, ex.Message);
					}
				}

				// Initialize drag and drop auto expand members
				this.dragAutoExpandTimer.Stop();
				this.dragAutoExpandNode = newDropTargetNode;
				this.dragAutoExpandTimer.Interval = this.DragAutoExpandInterval;
				this.dragAutoExpandTimer.Start();
			}

			return effects;
		}

		private void ShellDragDropHelper_DropTargetLeave(object source, EventArgs e) {
		// TODO: DropTarget->Leave :-)
			if(this.dropTargetNode != null)
				this.dropTargetNode.IsDropHighlited = false;
			this.dragAutoScrollTimer.Stop();
			this.dragAutoExpandTimer.Stop();
		}

		private void ShellDragDropHelper_DropTargetDrop(object source, DropTargetEventArgs e) {
			// TODO: Override WinAPI.POINT to get Point(.NET)-instance
			ExtTreeViewNode dropTargetNode = this.GetNodeAt(this.PointToClient(new Point(e.MousePos.X, e.MousePos.Y)));

			if(dropTargetNode != null) {
				try {
					WinAPI.IDropTarget dropTarget = dropTargetNode.GetIDropTarget();
					DragDropEffects unused = new DragDropEffects();

					if(dropTarget != null) {
						dropTarget.DragEnter(e.DataObject, e.KeyState,e .MousePos, ref unused);
						dropTarget.DragOver(e.KeyState, e.MousePos, ref unused);
						dropTarget.Drop(e.DataObject, e.KeyState, e.MousePos, ref e.Effects);
					}
				} catch (Exception ex) {
					string body = "==> STACKTRACE:\n" + ex.StackTrace.ToString() +
						"\n==> KEYSTATE: " + e.KeyState.ToString() +
						"\n==> MOUSEPOS: " + e.MousePos.ToString() +
						"\n==> EFFECTS: " + e.Effects.ToString();
					System.Windows.Forms.MessageBox.Show(body, ex.Message);
				}
			}




			// TODO: If frop successful, then...
			if(this.dropTargetNode != null)
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
					WinAPI.SendMessage(this.Handle, WinAPI.WM.VSCROLL, WinAPI.SB.LINEUP, IntPtr.Zero);

					if(mousePos.Y <= node.Bounds.Height)
						scrollFast = true;
				} else if (mousePos.Y >= (this.ClientRectangle.Height - (2 * node.Bounds.Height))) {
					WinAPI.SendMessage(this.Handle, WinAPI.WM.VSCROLL, WinAPI.SB.LINEDOWN, IntPtr.Zero);

					if(mousePos.Y >= (this.ClientRectangle.Height - node.Bounds.Height))
						scrollFast = true;
				}

				// Do horizontal scroll if necessary
				if(mousePos.X <= (2 * node.Bounds.Height)) {
					WinAPI.SendMessage(this.Handle, WinAPI.WM.HSCROLL, WinAPI.SB.LINELEFT, IntPtr.Zero);

					if(mousePos.X <= node.Bounds.Height)
						scrollFast = true;
				} else if (mousePos.X >= (this.ClientRectangle.Width - (2 * node.Bounds.Height))) {
					WinAPI.SendMessage(this.Handle, WinAPI.WM.HSCROLL, WinAPI.SB.LINERIGHT, IntPtr.Zero);

					if(mousePos.X >= (this.ClientRectangle.Width - node.Bounds.Height))
						scrollFast = true;
				}
			}

			this.dragAutoScrollTimer.Interval =
				(scrollFast ? this.DragAutoScrollFastInterval : this.DragAutoScrollSlowInterval);
		}

		private void dragAutoExpandTimer_Tick(object sender, EventArgs e) {
			ExtTreeViewNode node = this.GetNodeAt(this.PointToClient(Control.MousePosition));

			if((this.dragAutoExpandNode != null) && (this.dragAutoExpandNode.Equals(node))) {
				// TODO: Expand-methode mit BeginInvoke aufrufen und somit Multi-Threaded zu machen :-)
				this.dragAutoExpandNode.Expand();
			}

			this.dragAutoExpandTimer.Stop();
		}

	}
}
