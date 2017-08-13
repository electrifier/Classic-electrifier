using System;

namespace electrifier.Core.Controls {
	/// <summary>
	/// Summary for IExtListView.
	/// </summary>
	public interface IExtListView {
		void UpdateVirtualItemCount();
		void RedrawItems(int firstIndex, int lastIndex);
	}
}
