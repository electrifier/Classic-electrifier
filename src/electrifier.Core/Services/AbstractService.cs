using System;

namespace electrifier.Core.Services {
	/// <summary>
	/// Summary for AbstraceService.
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
