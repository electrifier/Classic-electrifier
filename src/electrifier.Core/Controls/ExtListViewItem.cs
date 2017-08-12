using System;

using electrifier.Win32API;

namespace electrifier.Core.Controls {
    /// <summary>
    /// Summary for ExtListViewItem.
    /// </summary>

    [Obsolete]
    public class ExtListViewItem : IExtListViewItem {
		private char[]       text       = null;
		public  char[]       Text       { get { return text; } }
		private int          imageIndex = -1;
		public  int          ImageIndex { get { return imageIndex; } set { SetImageIndex(value); } }
		private int          itemIndent = 0;
		public  int          ItemIndent { get { return itemIndent; } }
		private IExtListView listView   = null;
		public  IExtListView ListView   { get { return listView; } }
		private int          index      = -1;
		public  int          Index      { get { return index; } }

		public ExtListViewItem() {
		}

		public virtual WinAPI.IDataObject GetIDataObject() {
			return null;	// Don't know how to do :-)
		}

		public virtual WinAPI.IDropTarget GetIDropTarget() {
			return null;	// Don't know how to do :-)
		}

		protected void SetText(string text) {
			int textLength = text.Length;

			this.text = new char[textLength + 1];
			text.CopyTo(0, this.text, 0, textLength);
			this.text[textLength] = '\0';
		}

		protected void SetText(char[] text) {
			int textLength = text.Length;

			this.text = new char[textLength + 1];
			text.CopyTo(this.text, 0);
			this.text[textLength] = '\0';
		}

		protected void SetImageIndex(int newImageIndex) {
			if(imageIndex != newImageIndex) {
				imageIndex = newImageIndex;
				Invalidate();
			}
		}

		public void AddToIExtListView(IExtListView listView, int index) {
			if(this.listView == null) {
				this.listView = listView;
				this.index    = index;
			} else {
				throw new InvalidOperationException("Already added to an ExtListView");
			}
		}

		public void Invalidate() {
			if(listView != null) {
				listView.RedrawItems(index, index);
			}
		}
	}
}
