//	<file>
//		<copyright see="www.electrifier.org"/>
//		<license   see="www.electrifier.org"/>
//		<owner     name="Thorsten Jung" email="taj@electrifier.org"/>
//		<version   value="$Id: IService.cs,v 1.2 2004/08/09 20:50:38 jung2t Exp $"/>
//	</file>

using System;

namespace Electrifier.Core.Services {
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
