//	<file>
//		<copyright see="www.electrifier.org"/>
//		<license   see="www.electrifier.org"/>
//		<owner     name="Thorsten Jung" email="taj@electrifier.org"/>
//		<version   value="$Id: ExtTreeViewNode.cs,v 1.14 2004/09/10 15:21:53 taj bender Exp $"/>
//	</file>

using System;

namespace Electrifier.Core.Controls {
	/// <summary>
	/// Zusammenfassung für ExtListViewItem.
	/// </summary>
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
			//
			// TODO: Fügen Sie hier die Konstruktorlogik hinzu
			//
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
