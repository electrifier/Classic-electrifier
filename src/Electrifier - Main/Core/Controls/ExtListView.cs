//	<file>
//		<copyright see="www.electrifier.org"/>
//		<license   see="www.electrifier.org"/>
//		<owner     name="Thorsten Jung" email="taj@electrifier.org"/>
//		<version   value="$Id: ExtTreeView.cs,v 1.8 2004/09/10 15:21:53 taj bender Exp $"/>
//	</file>

using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using Electrifier.Win32API;

namespace Electrifier.Core.Controls {
	/// <summary>
	/// Zusammenfassung für ExtListView.
	/// </summary>
	public class ExtListView : ListView {
		protected  ExtListViewItemCollection items = null;
		public new ExtListViewItemCollection Items { get { return items; } }

		public ExtListView() : base() {
			items = new ExtListViewItemCollection(this);
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

		// TODO: protected machen, und auf event reagieren!
		public void UpdateVirtualItemCount() {
			UpdateVirtualItemCount(items.Count);
		}

		protected void UpdateVirtualItemCount(int itemCount) {
			if(IsHandleCreated) {
				WinAPI.SendMessage(Handle, Win32API.WMSG.LVM_SETITEMCOUNT, itemCount, 0);
			}
		}

		protected override CreateParams CreateParams {
			get {
				CreateParams createParams = base.CreateParams;
				createParams.Style       |= (int)Win32API.LVS.OWNERDATA;

				return createParams;
			}
		}

		protected override void OnHandleCreated(EventArgs e) {
			base.OnHandleCreated(e);

			UpdateVirtualItemCount();
		}

		protected override void OnHandleDestroyed(EventArgs e) {
			// Set item count to zero, since ListViews OnHandleDestroyed-method accesses the selected items
			UpdateVirtualItemCount(0);

			base.OnHandleDestroyed(e);
		}

		protected override void WndProc(ref Message m) {
			switch(m.Msg) {
				case (int)(WinAPI.WM.NOTIFY | WinAPI.WM.REFLECT): {
					Win32API.NMHDR nmhdr = (Win32API.NMHDR)m.GetLParam(typeof(Win32API.NMHDR));

					switch(nmhdr.code) {
						case (int)WinAPI.LVN.GETDISPINFOW:
							GetDisplayInfo(ref m);
							return;
					}

					break;
				} // case (int)(WinAPI.WM.NOTIFY | WinAPI.WM.REFLECT)
				case (int)(WinAPI.WM.SETFOCUS): {
					return;
				} // case (int)(WinAPI.WM.SETFOCUS)
			} // switch(u.Msg)

			base.WndProc (ref m);
		}

		protected virtual void GetDisplayInfo(ref Message m) {
			Win32API.LVDISPINFO dispInfo = (Win32API.LVDISPINFO)m.GetLParam(typeof(Win32API.LVDISPINFO));

			if((dispInfo.item.mask & (uint)(Win32API.LVIF.TEXT | Win32API.LVIF.IMAGE | Win32API.LVIF.INDENT)) != 0) {
				// TODO: []-operator ueberladen, typ IExtListViewItem zurueckliefern!
				IExtListViewItem listViewItem = items[dispInfo.item.iItem] as IExtListViewItem;

				if((dispInfo.item.mask & (uint)Win32API.LVIF.TEXT) != 0) {
					// TODO: stringlength eceeds dispinfo.item.cchTextMax?
               Marshal.Copy(listViewItem.Text, 0, dispInfo.item.pszText, listViewItem.Text.Length);
					// TODO: SubItems: info.item.iSubItem ist der index drauf!
				}

				if((dispInfo.item.mask & (uint)Win32API.LVIF.IMAGE) != 0) {
					dispInfo.item.iImage = listViewItem.ImageIndex;
					Marshal.StructureToPtr(dispInfo, m.LParam, false);		// TODO: letzter parameter ?!? => speicherleck!
				}

				if((dispInfo.item.mask & (uint)Win32API.LVIF.INDENT) != 0) {
					dispInfo.item.iIndent = listViewItem.ItemIndent;
					Marshal.StructureToPtr(dispInfo, m.LParam, false);		// TODO: letzter parameter ?!? => speicherleck!
				}
			}
		}
	}
}
