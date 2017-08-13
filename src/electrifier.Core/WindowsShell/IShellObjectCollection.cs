/*
** 
** electrifier
** 
** Copyright (c) 2017 Thorsten Jung @ electrifier.org and contributors
** 
*/

namespace electrifier.Core.WindowsShell
{
    /// <summary>
    /// Summary for IShellObjectCollection.
    /// </summary>
    public interface IShellObjectCollection
    {
        int Count { get; }
        IShellObjectCollectionEnumerator GetEnumerator();
    }

    /// <summary>
    /// Summary for IShellObjectCollectionEnumerator.
    /// </summary>
    public interface IShellObjectCollectionEnumerator
    {
        bool MoveNext();
        IShellObject Current { get; }
    }
}
