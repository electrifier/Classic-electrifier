using System;

namespace electrifier.Core {
	/// <summary>
	/// Zusammenfassung für IPersistentFormContainer.
	/// </summary>
	public interface IPersistentFormContainer : IPersistent {
		void AttachPersistentForm(IPersistentForm persistentForm);
		void DetachPersistentForm(IPersistentForm persistentForm);
	}
}
