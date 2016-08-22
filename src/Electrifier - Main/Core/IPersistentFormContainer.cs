using System;

namespace electrifier.Core {
	/// <summary>
	/// Zusammenfassung f�r IPersistentFormContainer.
	/// </summary>
	public interface IPersistentFormContainer : IPersistent {
		void AttachPersistentForm(IPersistentForm persistentForm);
		void DetachPersistentForm(IPersistentForm persistentForm);
	}
}
