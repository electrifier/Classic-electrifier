/*
** 
** electrifier
** 
** Copyright (c) 2017 Thorsten Jung @ electrifier.org and contributors
** 
*/

namespace electrifier.Core.WindowsShell.Services
{
    /// <summary>
    /// Summary for IFileInfoThread.
    /// </summary>
    public interface IFileInfoThread
    {
        void Prioritize(IShellObject sender);
        void Remove(IShellObject sender);
    }
}
