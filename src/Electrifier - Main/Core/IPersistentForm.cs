using System;

namespace electrifier.Core {
	/// <summary>
	/// Summary of IPersistentForm.
	/// </summary>
	public interface IPersistentForm : IPersistent {
		void Show();
		void AttachToFormContainer(IPersistentFormContainer persistentFormContainer);
	}
}
