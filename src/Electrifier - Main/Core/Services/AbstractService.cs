//	<file>
//		<copyright see="www.electrifier.org"/>
//		<license   see="www.electrifier.org"/>
//		<owner     name="Thorsten Jung" email="taj@electrifier.org"/>
//		<version   value="$Id: AbstractService.cs,v 1.1 2004/08/10 16:46:24 jung2t Exp $"/>
//	</file>

using System;

namespace Electrifier.Core.Services {
	/// <summary>
	/// Zusammenfassung für AbstraceService.
	/// </summary>
	public class AbstractService : IService {
		public virtual void InitializeService() {
			OnInitialize(EventArgs.Empty);
		}

		public virtual void UnloadService() {
			OnUnload(EventArgs.Empty);
		}

		protected virtual void OnInitialize(EventArgs e) {
			if(Initialize != null) {
				Initialize(this, e);
			}
		}

		protected virtual void OnUnload(EventArgs e) {
			if(Unload != null) {
				Unload(this, e);
			}
		}

		public event EventHandler Initialize;
		public event EventHandler Unload;
	}
}
