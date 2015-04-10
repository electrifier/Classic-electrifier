using System;

namespace Electrifier.Core
{
	/// <summary>
	/// Zusammenfassung für IPersistentForm.
	/// </summary>
	public interface IPersistentForm : IPersistent
	{
		void Show();
		void AttachToFormContainer(IPersistentFormContainer persistentFormContainer);
	}
}
