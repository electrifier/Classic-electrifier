//	<file>
//		<copyright see="www.electrifier.org"/>
//		<license   see="www.electrifier.org"/>
//		<owner     name="Thorsten Jung" email="taj@electrifier.org"/>
//		<version   value="$Id: BasicShellObject.cs,v 1.13 2004/09/10 20:30:33 taj bender Exp $"/>
//	</file>

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
