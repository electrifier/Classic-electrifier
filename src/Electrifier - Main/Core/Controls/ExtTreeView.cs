//	<file>
//		<copyright see="www.electrifier.org"/>
//		<license   see="www.electrifier.org"/>
//		<owner     name="Thorsten Jung" email="taj@electrifier.org"/>
//		<version   value="$Id: ExtTreeView.cs,v 1.8 2004/09/10 15:21:53 taj bender Exp $"/>
//	</file>

using System;
using System.Windows.Forms;

using Electrifier.Win32API;

namespace Electrifier.Core.Controls {
	/// <summary>
	/// Zusammenfassung für ExtTreeView.
	/// </summary>
	public class ExtTreeView : TreeView {
		protected ExtTreeViewNodeCollection nodes = null;

		public ExtTreeView() : base() {
			nodes = new ExtTreeViewNodeCollection(base.Nodes);

			BeforeExpand += new TreeViewCancelEventHandler(ExtTreeView_BeforeExpand);
		}

		public new ExtTreeViewNodeCollection Nodes {
			get {
				return nodes;
			}
		}

		public IntPtr SystemImageList {
			get {
				return WinAPI.SendMessage(Handle, WMSG.TVM_GETIMAGELIST, TVSIL.NORMAL, IntPtr.Zero);
			}
			set {
				WinAPI.SendMessage(Handle, WMSG.TVM_SETIMAGELIST, TVSIL.NORMAL, value);
			}
		}

		protected void ExtTreeView_BeforeExpand(object sender, TreeViewCancelEventArgs e) {
			ExtTreeViewNode node = e.Node as ExtTreeViewNode;

			if(node != null)
				e.Cancel = !node.IsExpandable;
		}
	}
}
