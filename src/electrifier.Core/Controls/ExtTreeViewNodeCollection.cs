using System;
using System.Windows.Forms;

namespace electrifier.Core.Controls {
	/// <summary>
	/// Summary for ExtTreeViewNodeCollection.
	/// </summary>
	public class ExtTreeViewNodeCollection : IExtTreeViewNodeCollection {
		protected TreeNodeCollection collection;

		public ExtTreeViewNodeCollection(ExtTreeViewNodeCollection collection) {
			this.collection = collection.collection;
		}

		public ExtTreeViewNodeCollection(TreeNodeCollection treeNodeCollection) {
			this.collection = treeNodeCollection;
		}

		public virtual int Count {
			get{ return this.collection.Count; }
		}

		public virtual bool IsReadOnly {
			get { return this.collection.IsReadOnly; }
		}

		public virtual ExtTreeViewNode this[int index] {
			get{ return this.collection[index] as ExtTreeViewNode; }
		}

		public void Add(ExtTreeViewNode node) {			
			this.collection.Add(node);

			node.HasBeenAddedToTreeViewBy(this);
		}

		public void AddRange(ExtTreeViewNode[] nodes) {
			this.collection.AddRange(nodes);

			foreach(ExtTreeViewNode node in nodes) {
				node.HasBeenAddedToTreeViewBy(this);
			}
		}
	}
}
