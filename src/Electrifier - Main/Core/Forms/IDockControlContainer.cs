using System;

namespace electrifier.Core.Forms.DockControls {
	/// <summary>
	/// Zusammenfassung für IDockControlContainer.
	/// </summary>
	public interface IDockControlContainer {
		void AttachDockControl(IDockControl dockControl);
		void DetachDockControl(IDockControl dockControl);
	}
}
