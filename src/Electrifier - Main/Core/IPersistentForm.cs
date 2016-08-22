using System;

namespace electrifier.Core
{
	/// <summary>
	/// Zusammenfassung f�r IPersistentForm.
	/// </summary>
	public interface IPersistentForm : IPersistent
	{
		void Show();
		void AttachToFormContainer(IPersistentFormContainer persistentFormContainer);
	}
}
