using System;

namespace Electrifier.Core.Forms.DockControls {
	/// <summary>
	/// Zusammenfassung f�r IDockControlContainer.
	/// </summary>
	public interface IDockControlContainer {
		void AttachDockControl(IDockControl dockControl);
		void DetachDockControl(IDockControl dockControl);
	}
}
