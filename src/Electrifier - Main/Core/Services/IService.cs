using System;

namespace electrifier.Core.Services {
	/// <summary>
	/// This interface must be implemented by all services.
	/// </summary>
	public interface IService {
		/// <summary>
		/// This method is called when the service gets initialized.
		/// </summary>
		void InitializeService();

		/// <summary>
		/// This methid is called before the service gets unloaded.
		/// </summary>
		void UnloadService();

		event EventHandler Initialize;
		event EventHandler Unload;
	}
}
