using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using electrifier.Core.WindowsShell;
using electrifier.Win32API;

namespace electrifier.Core.Controls
{
    /// <summary>
    /// Summary for ExtListView.
    /// </summary>
    public class ExtListView : ListView, IExtListView
    {
        protected ExtListViewItemCollection items = null;
        public new ExtListViewItemCollection Items { get { return this.items; } }

        protected ShellDragDropHelper shellDragDropHelper = null;

        public ExtListView() : base()
        {
            this.items = new ExtListViewItemCollection(this);
            this.shellDragDropHelper = new ShellDragDropHelper(this.Handle);

            // Initialize drag and drop-event handlers
            this.ItemDrag += new ItemDragEventHandler(this.ExtListView_ItemDrag);
        }

        public IntPtr SmallSystemImageList {
            get { return WinAPI.SendMessage(this.Handle, WMSG.LVM_GETIMAGELIST, LVSIL.SMALL, IntPtr.Zero); }
            set { WinAPI.SendMessage(this.Handle, WMSG.LVM_SETIMAGELIST, LVSIL.SMALL, value); }
        }

        public IntPtr NormalSystemImageList {
            get { return WinAPI.SendMessage(this.Handle, WMSG.LVM_GETIMAGELIST, LVSIL.NORMAL, IntPtr.Zero); }
            set { WinAPI.SendMessage(this.Handle, WMSG.LVM_SETIMAGELIST, LVSIL.NORMAL, value); }
        }

        public void SetItemSelectedState(int itemIndex, bool isSelected)
        {
            // Initialize appropriate LVItem-structure
            LVITEM lvItem = new LVITEM()
            {
                mask = Win32API.LVIF.STATE,
                iItem = itemIndex,
                iSubItem = 0,
                state = (isSelected ? WinAPI.LVIS.SELECTED : 0),
                stateMask = WinAPI.LVIS.SELECTED
            };

            // Send SetItemState-Message to ListView
            SetItemState(ref lvItem);
        }

        public void SetItemFocusedState(int itemIndex, bool isFocused)
        {
            // Initialize appropriate LVItem-structure
            LVITEM lvItem = new LVITEM()
            {
                mask = Win32API.LVIF.STATE,
                iItem = itemIndex,
                iSubItem = 0,
                state = (isFocused ? WinAPI.LVIS.FOCUSED : 0),
                stateMask = WinAPI.LVIS.FOCUSED
            };

            // Send SetItemState-Message to ListView
            SetItemState(ref lvItem);
        }

        protected int SetItemState(ref LVITEM lvItem)
        {
            IntPtr pItem = IntPtr.Zero;
            int returnCode;

            try
            {
                // TODO: ueberpruefen, ob kopieren ueberhaupt notwendig und code auf jeden ueberabreiten!
                pItem = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(Win32API.LVITEM)));
                Marshal.StructureToPtr(lvItem, pItem, true);

                returnCode = WinAPI.SendMessage(this.Handle, Win32API.WMSG.LVM_SETITEMSTATE, 0, pItem.ToInt32());
            }
            finally
            {
                Marshal.FreeHGlobal(IntPtr.Zero);
            }

            return returnCode;
        }

        protected override CreateParams CreateParams {
            get {
                CreateParams createParams = base.CreateParams;
                createParams.Style |= (int)Win32API.LVS.OWNERDATA;

                return createParams;
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            this.UpdateVirtualItemCount();
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            // Set item count to zero, since ListViews OnHandleDestroyed-method accesses the selected items
            this.UpdateVirtualItemCount(0);

            base.OnHandleDestroyed(e);
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case (int)(WinAPI.WM.NOTIFY | WinAPI.WM.REFLECT):
                    {
                        Win32API.NMHDR nmhdr = (Win32API.NMHDR)m.GetLParam(typeof(Win32API.NMHDR));

                        switch (nmhdr.code)
                        {
                            case (int)WinAPI.LVN.GETDISPINFOW:
                                GetDisplayInfo(ref m);
                                return;
                            case (int)WinAPI.LVN.BEGINDRAG:
                                OnBeginDragMessage(MouseButtons.Left, ref m);
                                return;
                            case (int)WinAPI.LVN.BEGINRDRAG:
                                OnBeginDragMessage(MouseButtons.Right, ref m);
                                return;
                        }

                        break;
                    } // case (int)(WinAPI.WM.NOTIFY | WinAPI.WM.REFLECT)

                case (int)(WinAPI.WM.SETFOCUS):
                    {
                        SetItemFocusedState(0, true);
                        // TODO: Only test-code!
                        try
                        {
                            base.WndProc(ref m);
                        }
                        catch (System.ArgumentOutOfRangeException) { }
                        return;
                    } // case (int)(WinAPI.WM.SETFOCUS)
            } // switch(u.Msg)

            base.WndProc(ref m);
        }

        protected virtual void GetDisplayInfo(ref Message m)
        {
            Win32API.LVDISPINFO dispInfo = (Win32API.LVDISPINFO)m.GetLParam(typeof(Win32API.LVDISPINFO));

            if ((dispInfo.item.mask & (Win32API.LVIF.TEXT | Win32API.LVIF.IMAGE | Win32API.LVIF.INDENT)) != 0)
            {
                // TODO: []-operator ueberladen, typ IExtListViewItem zurueckliefern!
                IExtListViewItem listViewItem = this.items[dispInfo.item.iItem] as IExtListViewItem;

                if ((dispInfo.item.mask & Win32API.LVIF.TEXT) != 0)
                {
                    // TODO: stringlength eceeds dispinfo.item.cchTextMax?
                    Marshal.Copy(listViewItem.Text, 0, dispInfo.item.pszText, listViewItem.Text.Length);
                    // TODO: SubItems: info.item.iSubItem ist der index drauf!
                }

                if ((dispInfo.item.mask & Win32API.LVIF.IMAGE) != 0)
                {
                    dispInfo.item.iImage = listViewItem.ImageIndex;
                    Marshal.StructureToPtr(dispInfo, m.LParam, false);      // TODO: letzter parameter ?!? => speicherleck!
                }

                if ((dispInfo.item.mask & Win32API.LVIF.INDENT) != 0)
                {
                    dispInfo.item.iIndent = listViewItem.ItemIndent;
                    Marshal.StructureToPtr(dispInfo, m.LParam, false);      // TODO: letzter parameter ?!? => speicherleck!
                }
            }
        }

        protected void OnBeginDragMessage(MouseButtons mouseButton, ref Message m)
        {
            Win32API.NMLISTVIEW nmListView = (Win32API.NMLISTVIEW)m.GetLParam(typeof(Win32API.NMLISTVIEW));

            // TODO: What about multi-selection?!?
            this.OnItemDrag(new ItemDragEventArgs(mouseButton, this.Items[nmListView.iItem]));
        }

        #region IExtListView Member

        // TODO: protected machen, und auf event reagieren!
        public void UpdateVirtualItemCount()
        {
            this.UpdateVirtualItemCount(this.items.Count);
        }

        protected void UpdateVirtualItemCount(int itemCount)
        {
            if (this.IsHandleCreated)
            {
                WinAPI.SendMessage(this.Handle, Win32API.WMSG.LVM_SETITEMCOUNT, itemCount, 0);
            }
        }

        public void RedrawItems(int firstIndex, int lastIndex)
        {
            if (this.IsHandleCreated)
            {
                WinAPI.SendMessage(this.Handle, Win32API.WMSG.LVM_REDRAWITEMS, firstIndex, lastIndex);
            }
        }

        #endregion

        private void ExtListView_ItemDrag(object sender, ItemDragEventArgs e)
        {

            if (e.Item is ExtListViewItem dragItem)
            {
                WinAPI.IDataObject dataObject = dragItem.GetIDataObject();

                this.shellDragDropHelper.PrepareDragImage(dataObject);

                this.DoDragDrop(dataObject, (DragDropEffects.Copy | DragDropEffects.Move | DragDropEffects.Link));
            }
        }
    }
}
