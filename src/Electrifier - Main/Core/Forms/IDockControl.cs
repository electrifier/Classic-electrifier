using System;

namespace electrifier.Core.Forms.DockControls {
	/// <summary>
	/// Zusammenfassung für IDockControl.
	/// </summary>
	public interface IDockControl : IPersistent {
		void AttachToDockControlContainer(IDockControlContainer dockControlContainer);
		void DetachFromDockControlContainer();
	}
}
