using System;

namespace Electrifier.Core {
	/// <summary>
	/// Zusammenfassung f�r IPersistentFormContainer.
	/// </summary>
	public interface IPersistentFormContainer : IPersistent {
		void AttachPersistentForm(IPersistentForm persistentForm);
		void DetachPersistentForm(IPersistentForm persistentForm);
	}
}
