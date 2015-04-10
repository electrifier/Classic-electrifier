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
