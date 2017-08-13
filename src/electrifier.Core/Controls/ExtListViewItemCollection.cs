using System;
using System.Collections;

using electrifier.Win32API;

namespace electrifier.Core.Controls {
	/// <summary>
	/// Summary for ExtListViewItemCollection.
	/// </summary>
	public class ExtListViewItemCollection : ArrayList, ICollection, IList, IEnumerable {
		protected IExtListView owner = null;
		public    IExtListView Owner { get { return owner; } }

		public ExtListViewItemCollection(IExtListView owner) {
			this.owner = owner;
		}

		public override int Add(object item) {
			if(item != null) {
				if(item.GetType().GetInterface("IExtListView") != null) {
					return Add(item as IExtListViewItem);
				} else {
					throw new ArrayTypeMismatchException("All objects must implement interface IExtListView");
				}
			} else {
				return -1;
			}
		}

		public virtual int Add(IExtListViewItem item) {
			int index = AddWithoutUpdatingItemCount(item);
			owner.UpdateVirtualItemCount();
			return index;
		}

		public virtual int AddWithoutUpdatingItemCount(IExtListViewItem item) {
			if(item != null) {
				int index = base.Add(item);
				item.AddToIExtListView(owner, index);
				return index;
			} else {
				return -1;
			}
		}

		public override void AddRange(ICollection collection) {
			foreach(object obj in collection) {
				if((obj != null) && (obj.GetType().GetInterface("IExtListView") == null)) {
					throw new ArrayTypeMismatchException("All objects must implement interface IExtListView");
				}
			}

			AddRange(collection as IExtListViewItem[]);
		}

		public virtual void AddRange(IExtListViewItem[] items) {
			foreach(IExtListViewItem item in items) {
				if(item != null) {
					AddWithoutUpdatingItemCount(item);
				}
			}

			Owner.UpdateVirtualItemCount();
		}

		public override void Clear() {
			base.Clear();
			Owner.UpdateVirtualItemCount();
		}
	}
}
