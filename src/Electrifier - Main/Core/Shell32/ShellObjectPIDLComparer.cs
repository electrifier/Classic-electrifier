//	<file>
//		<copyright see="www.electrifier.org"/>
//		<license   see="www.electrifier.org"/>
//		<owner     name="Thorsten Jung" email="taj@electrifier.org"/>
//		<version   value="$Id: ShellObjectPIDLComparer.cs,v 1.2 2004/09/10 15:21:53 taj bender Exp $"/>
//	</file>

using System;
using System.Collections;

using Electrifier.Core.Shell32.Services;
using Electrifier.Win32API;

namespace Electrifier.Core.Shell32 {
	/// <summary>
	/// Zusammenfassung für ShellObjectPIDLComparer.
	/// </summary>
	public class ShellObjectPIDLComparer : IComparer {
		protected SortMode              sortMode;
		protected ShellAPI.IShellFolder parentFolder;

		public enum SortMode {
			Unsorted,
			Ascending,
			Descending,
		}

		public ShellObjectPIDLComparer(SortMode sortMode, ShellAPI.IShellFolder parentFolder) {
			if(!Enum.IsDefined(typeof(SortMode), sortMode))
				throw new ArgumentException("The given sortMode is not defined in Electrifier.Core.Shell32.ShellObjectPIDLComparer.SortMode", "sortMode");

			this.sortMode     = sortMode;
			this.parentFolder = parentFolder;
		}

		protected int StandardShellComparer(object x, object y, bool descending) {
			if(x != null || y != null) {
				if((x is IntPtr) && (y is IntPtr)) {
					int result = parentFolder.CompareIDs(0, PIDLManager.FindLastID((IntPtr)x),
					                                        PIDLManager.FindLastID((IntPtr)y));

					// TODO: >= 0 => SUCCEEDED
					//	TODO: #define SCODE_CODE(sc)     ((sc) & 0xFFFF)
					if(descending)
						return (short)(-(result & 0xFFFF));
					else
						return (short)(result & 0xFFFF);
				} else {
					throw new ArgumentException("Parameter not of type IntPtr!");
				}
			}

			return 0;
		}

		#region IComparer Member
		public int Compare(object x, object y) {
			switch (sortMode) {
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
