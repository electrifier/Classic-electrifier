//	<file>
//		<copyright see="www.electrifier.org"/>
//		<license   see="www.electrifier.org"/>
//		<owner     name="Thorsten Jung" email="taj@electrifier.org"/>
//		<version   value="$Id: IService.cs,v 1.2 2004/08/09 20:50:38 jung2t Exp $"/>
//	</file>

using System;

namespace Electrifier.Core.Forms.DockControls {
	/// <summary>
	/// Zusammenfassung für IDockControl.
	/// </summary>
	public interface IDockControl {
		void RegisterToDockControlContainer(IDockControlContainer dockControlContainer);
	}
}
