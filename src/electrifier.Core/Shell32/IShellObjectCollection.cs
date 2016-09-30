using System;

namespace electrifier.Core.Shell32 {
	/// <summary>
	/// Zusammenfassung f�r IShellObjectCollection.
	/// </summary>
	public interface IShellObjectCollection {
		int Count { get; }
		IShellObjectCollectionEnumerator GetEnumerator();
	}

	/// <summary>
	/// Zusammenfassung f�r IShellObjectCollectionEnumerator.
	/// </summary>
	public interface IShellObjectCollectionEnumerator {
		bool MoveNext();
		IShellObject Current { get; }
	}
}
