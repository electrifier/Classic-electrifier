using System;

namespace electrifier.Core.Controls {
	/// <summary>
	/// Zusammenfassung für IExtListView.
	/// </summary>
	public interface IExtListView {
		void UpdateVirtualItemCount();
		void RedrawItems(int firstIndex, int lastIndex);
	}
}
