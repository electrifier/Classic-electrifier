//	<file>
//		<copyright see="www.electrifier.org"/>
//		<license   see="www.electrifier.org"/>
//		<owner     name="Thorsten Jung" email="taj@electrifier.org"/>
//		<version   value="$Id: ExtTreeViewNodeCollection.cs,v 1.4 2004/08/25 18:49:06 jung2t Exp $"/>
//	</file>

using System;
using System.Windows.Forms;

namespace Electrifier.Core.Controls {
	/// <summary>
	/// Zusammenfassung für ExtTreeViewNodeCollection.
	/// </summary>

	public class ExtTreeViewNodeCollection : IExtTreeViewNodeCollection {
		protected TreeNodeCollection collection;

		public ExtTreeViewNodeCollection(ExtTreeViewNodeCollection collection)
			: this(collection.collection) { }

		public ExtTreeViewNodeCollection(TreeNodeCollection treeNodeCollection) {
			collection = treeNodeCollection;
		}

		public virtual int Count {
			get{
				return collection.Count;
			}
		}

		public virtual bool IsReadOnly {
			get {
				return collection.IsReadOnly;
			}
		}

		public virtual TreeNode this[int index] {
			get{
				return collection[index];
			}
		}



		public void Add(ExtTreeViewNode node) {			
			collection.Add(node);

			node.HasBeenAddedToTreeViewBy(this);
		}

		public void AddRange(ExtTreeViewNode[] nodes) {
			collection.AddRange(nodes);

			foreach(ExtTreeViewNode node in nodes) {
				node.HasBeenAddedToTreeViewBy(this);
			}
		}
	}
}
