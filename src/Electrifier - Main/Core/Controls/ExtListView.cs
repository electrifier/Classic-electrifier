using System;
using System.Windows.Forms;

using Electrifier.Win32API;

namespace Electrifier.Core.Controls {
	/// <summary>
	/// Zusammenfassung für ExtListView.
	/// </summary>
	public class ExtListView : ListView {
		public ExtListView() : base() {
			//
			// TODO: Fügen Sie hier die Konstruktorlogik hinzu
			//
		}

		public IntPtr SmallSystemImageList {
			get {
				return WinAPI.SendMessage(Handle, WMSG.LVM_GETIMAGELIST, LVSIL.SMALL, IntPtr.Zero);
			}
			set {
				WinAPI.SendMessage(Handle, WMSG.LVM_SETIMAGELIST, LVSIL.SMALL, value);
			}
		}

		public IntPtr LargeSystemImageList {
			get {
				return WinAPI.SendMessage(Handle, WMSG.LVM_GETIMAGELIST, LVSIL.NORMAL, IntPtr.Zero);
			}
			set {
				WinAPI.SendMessage(Handle, WMSG.LVM_SETIMAGELIST, LVSIL.NORMAL, value);
			}
		}
	}
}
