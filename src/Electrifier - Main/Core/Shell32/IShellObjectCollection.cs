//	<file>
//		<copyright see="www.electrifier.org"/>
//		<license   see="www.electrifier.org"/>
//		<owner     name="Thorsten Jung" email="taj@electrifier.org"/>
//		<version   value="$Id: IShellObjectCollection.cs,v 1.3 2004/09/11 16:45:52 taj bender Exp $"/>
//	</file>

using System;

namespace Electrifier.Core.Shell32 {
	/// <summary>
	/// Zusammenfassung für IShellObjectCollection.
	/// </summary>
	public interface IShellObjectCollection {
		int Count { get; }
		IShellObjectCollectionEnumerator GetEnumerator();
	}

	/// <summary>
	/// Zusammenfassung für IShellObjectCollectionEnumerator.
	/// </summary>
	public interface IShellObjectCollectionEnumerator {
		bool MoveNext();
		IShellObject Current { get; }
	}
}
