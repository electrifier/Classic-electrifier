using System;

namespace electrifier.Core {
	/// <summary>
	/// Summary of IPersistentFormContainer.
	/// </summary>
	public interface IPersistentFormContainer : IPersistent {
		void AttachPersistentForm(IPersistentForm persistentForm);
		void DetachPersistentForm(IPersistentForm persistentForm);
	}
}
