//	<file>
//		<copyright see="www.electrifier.org"/>
//		<license   see="www.electrifier.org"/>
//		<owner     name="Thorsten Jung" email="taj@electrifier.org"/>
//		<version   value="$Id: ExtTreeViewNode.cs,v 1.14 2004/09/10 15:21:53 taj bender Exp $"/>
//	</file>

using System;
using System.Windows.Forms;

using Electrifier.Win32API;

namespace Electrifier.Core.Controls {
	/// <summary>
	/// Zusammenfassung für ExtTreeViewNode.
	/// </summary>
	public class ExtTreeViewNode : TreeNode, IExtTreeViewNode {
		protected      ExtTreeViewNodeCollection nodes             = null;
		public new     ExtTreeViewNodeCollection Nodes             { get { return nodes; } }
		protected      bool                      handleCreated     = false;
		public virtual bool                      IsExpandable      { get { return Nodes.Count > 0; } }

		#region Overriden properties to ensure type strictness

		public new ExtTreeViewNode NextNode        { get { return base.NextNode as ExtTreeViewNode; } }
		public new ExtTreeViewNode PrevNode        { get { return base.PrevNode as ExtTreeViewNode; } }
		public new ExtTreeViewNode Parent          { get { return base.Parent as ExtTreeViewNode; } }
		public new ExtTreeViewNode PrevVisibleNode { get { return base.PrevVisibleNode as ExtTreeViewNode; } }
		public new ExtTreeViewNode NextVisibleNode { get { return base.NextVisibleNode as ExtTreeViewNode; } }

		#endregion

		protected      bool isShownExpandable = false;
		public virtual bool IsShownExpandable {
			get {
				return isShownExpandable;
			}
			set {
				if(isShownExpandable != value) {
					isShownExpandable = value;

					if(handleCreated) {
						UpdateIsShownExpandable();
					}
				}
			}
		}

		protected HTREEITEM hTreeItem = new HTREEITEM(IntPtr.Zero);
		/// <summary>
		/// Retrieve the handle to the TREEITEM structure of the node.
		/// NOTE: This property only works when the node is already added to an treeview!
		/// </summary>
		protected HTREEITEM HTreeItem {
			get {
				if(!hTreeItem.IsValid) {
					if(PrevNode != null) {              // Is there a previous item?
						hTreeItem = PrevNode.HTreeItem.NextNode(TreeView.Handle);
					} else {
						if(Parent != null) {             // No, so is there a parent item?
							hTreeItem = Parent.HTreeItem.FirstNode(TreeView.Handle);
						} else {                         // No, so we are the root item
							hTreeItem = HTREEITEM.GetRootItem(TreeView.Handle);
						}
					}
				}

				return hTreeItem;
			}
		}

		public ExtTreeViewNode() : base() {
			nodes = new ExtTreeViewNodeCollection(base.Nodes);
		}

		protected void UpdateIsShownExpandable() {
			TVItemEx tvItemEx  = new TVItemEx();
			tvItemEx.mask      = TVIF.CHILDREN;
			tvItemEx.hItem     = HTreeItem.Handle;
			tvItemEx.cChildren = isShownExpandable ? 1 : 0;
			
			WinAPI.SendMessage(TreeView.Handle, WMSG.TVM_SETITEM, 0, ref tvItemEx);
		}

		private void UpdateIsShownExpandable(bool causedByHasBeenAddedToTreeViewBy) {
			// By default, the node IS NOT shown expandable :-)
			if(causedByHasBeenAddedToTreeViewBy && isShownExpandable) {
				UpdateIsShownExpandable();
			}
		}

		#region IExtTreeViewNode Member
		// TODO: HandleCreated als Ersatz?!?
		public void HasBeenAddedToTreeViewBy(IExtTreeViewNodeCollection sender) {
			if(TreeView != null) {
				handleCreated = true;

				// Update properties already set but still not updated
				UpdateIsShownExpandable(true);
			} else {
				throw new InvalidOperationException("Electrifier.Core.Controls.ExtTreeViewNode.HasBeenAddedToTreeViewBy: TreeView-property not yet initialized!");
			}
		}
		#endregion
	}
}
