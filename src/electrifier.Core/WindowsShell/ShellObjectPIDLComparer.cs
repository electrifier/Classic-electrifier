/*
** 
** electrifier
** 
** Copyright (c) 2017 Thorsten Jung @ electrifier.org and contributors
** 
*/

using System;
using System.Collections;

using electrifier.Core.WindowsShell.Services;
using electrifier.Win32API;

namespace electrifier.Core.WindowsShell
{
    /// <summary>
    /// Summary for ShellObjectPIDLComparer.
    /// </summary>
    public class ShellObjectPIDLComparer : IComparer
    {
        protected SortMode sortMode;
        protected ShellAPI.IShellFolder parentFolder;

        public enum SortMode
        {
            Unsorted,
            Ascending,
            Descending,
        }

        public ShellObjectPIDLComparer(SortMode sortMode, ShellAPI.IShellFolder parentFolder)
        {
            if (!Enum.IsDefined(typeof(SortMode), sortMode))
                throw new ArgumentException("The given sortMode is not defined in electrifier.Core.Shell32.ShellObjectPIDLComparer.SortMode", "sortMode");

            this.sortMode = sortMode;
            this.parentFolder = parentFolder;
        }

        protected int StandardShellComparer(object x, object y, bool descending)
        {
            if (x != null || y != null)
            {
                if ((x is IntPtr) && (y is IntPtr))
                {
                    int result = this.parentFolder.CompareIDs(0, PIDLManager.FindLastID((IntPtr)x),
                                                            PIDLManager.FindLastID((IntPtr)y));

                    // TODO: >= 0 => SUCCEEDED
                    //	TODO: #define SCODE_CODE(sc)     ((sc) & 0xFFFF)
                    if (descending)
                        return (short)(-(result & 0xFFFF));
                    else
                        return (short)(result & 0xFFFF);
                }
                else
                {
                    throw new ArgumentException("Parameter not of type IntPtr!");
                }
            }

            return 0;
        }

        #region IComparer Member
        public int Compare(object x, object y)
        {
            switch (this.sortMode)
            {
                case SortMode.Ascending:
                    return StandardShellComparer(x, y, false);
                case SortMode.Descending:
                    return StandardShellComparer(x, y, true);
            }

            return 0;
        }
        #endregion
    }
}
