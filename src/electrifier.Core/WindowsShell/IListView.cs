using System;
using System.Drawing;
using System.Runtime.InteropServices;
using Vanara.PInvoke;


namespace electrifier.Core.WindowsShell
{
    /// <summary>
    /// <b>WARNING!</b> This is undocumented stuff!
    /// See <seealso href="https://www.geoffchappell.com/studies/windows/shell/comctl32/controls/listview/interfaces/ilistview/index.htm">Geoff Chappell, Software Analyst: IListView</seealso>
    /// 
    /// See <seealso href="https://csharp.hotexamples.com/de/examples/-/IListView/-/php-ilistview-class-examples.html">IListView-Examples</seealso>
    /// See <seealso href="https://www.codeproject.com/Articles/35197/Undocumented-List-View-Features">Undocumented List View Features</seealso>
    /// 
    /// You can receive a point to this interface by sending message
    ///   int LVM_QUERYINTERFACE = 0x10BD;
    ///   
    /// </summary>
    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("E5B16AF2-3990-4681-A609-1F060CD14269")]  // Windows 7 above
    //[Guid("2FFE2979-5928-4386-9CDB-8E1F15B72FB4")] Windows Vista
    public interface IListView
    {
        void GetWindow(out IntPtr phwnd);
        void ContextSensitiveHelp([In, MarshalAs(UnmanagedType.Bool)] bool fEnterMode);
        void GetImageList(int index, out IntPtr iList);
        void SetImageList(int index, IntPtr iList, out IntPtr iListDest);
        void GetBackgroundColor(out IntPtr colorref);
        void SetBackgroundColor(int /*IntPtr */ colorref);
        void GetTextColor(out IntPtr colorref);
        void SetTextColor(int /*IntPtr*/ colorref);
        void GetTextBackgroundColor(out IntPtr colorref);
        void SetTextBackgroundColor(int /*IntPtr*/ colorref);
        void GetHotLightColor(out IntPtr colorref);
        void SetHotLightColor(int /*IntPtr*/ colorref);
        void GetItemCount(out int count);
        void SetItemCount(int count, uint p);
        HRESULT GetItem(out IntPtr item);
        HRESULT SetItem(IntPtr item);
        HRESULT GetItemState(int iItem, ComCtl32.ListViewItemMask /* LVIF */ mask, ComCtl32.ListViewItemState /*LVIS*/ stateMask, out ComCtl32.ListViewItemState /* LVIS*/ state);
        [PreserveSig]
        HRESULT SetItemState(int iItem, ComCtl32.ListViewItemMask /*LVIF*/ mask, ComCtl32.ListViewItemState /*LVIS*/ stateMask, ComCtl32.ListViewItemState /*LVIS*/ state);
        HRESULT GetItemText(int a, int b, out IntPtr c, int d);
        HRESULT SetItemText(int a, int b, IntPtr c);
        void GetBackgroundImage(out IntPtr bitmap);
        void SetBackgroundImage(IntPtr bitmap);
        void GetFocusedColumn(out int col);
        void SetSelectionFlags(ulong a, ulong b);
        HRESULT GetSelectedColumn(out int col);
        HRESULT SetSelectedColumn(int col);
        HRESULT GetView(out uint view);
        HRESULT SetView(uint view);
        HRESULT InsertItem(IntPtr item, out int index);
        HRESULT DeleteItem(int index);
        HRESULT DeleteAllItems();
        HRESULT UpdateItem(int index);
        HRESULT GetItemRect(ComCtl32.LVITEMINDEX /*LVITEMINDEX*/ index, int a, out RECT rect);
        HRESULT GetSubItemRect(ComCtl32.LVITEMINDEX /*LVITEMINDEX*/ index, int a, int b, out RECT rect);
        HRESULT HitTestSubItem(ComCtl32.LVHITTESTINFO /*LVHITTESTINFO*/ info);
        HRESULT GetIncrSearchString(IntPtr a, int b, out int c);
        HRESULT GetItemSpacing(bool a, out int b, out int c);
        HRESULT SetIconSpacing(int a, int b, out int c, out int d);
        HRESULT GetNextItem(ComCtl32.LVITEMINDEX /*LVITEMINDEX*/ index, ulong flags, out ComCtl32.LVITEMINDEX /*LVITEMINDEX*/ result);
        HRESULT FindItem(ComCtl32.LVITEMINDEX /*LVITEMINDEX*/ index, IntPtr info, out ComCtl32.LVITEMINDEX /*LVITEMINDEX*/ item);
        HRESULT GetSelectionMark(out ComCtl32.LVITEMINDEX /*LVITEMINDEX*/ mark);
        HRESULT SetSelectionMark(ComCtl32.LVITEMINDEX /*LVITEMINDEX*/ index, out ComCtl32.LVITEMINDEX /*LVITEMINDEX*/ result);
        HRESULT GetItemPosition(ComCtl32.LVITEMINDEX /*LVITEMINDEX*/ index, out Point /*POINT*/ position);
        HRESULT SetItemPosition(int a, Point /*POINT*/ p);
        HRESULT ScrollView(int a, int b);
        [PreserveSig]
        HRESULT EnsureItemVisible(ComCtl32.LVITEMINDEX /*LVITEMINDEX*/ item, Boolean b);
        HRESULT EnsureSubItemVisible(ComCtl32.LVITEMINDEX /*LVITEMINDEX*/ item, int a);
        HRESULT EditSubItem(ComCtl32.LVITEMINDEX /*LVITEMINDEX*/ item, int a);
        HRESULT RedrawItems(int a, int b);
        HRESULT ArrangeItems(int a);
        HRESULT RecomputeItems(int a);
        HRESULT GetEditControl(out IntPtr handle);
        [PreserveSig]
        HRESULT EditLabel(ComCtl32.LVITEMINDEX /*LVITEMINDEX*/ index, IntPtr a, out IntPtr handle);
        HRESULT EditGroupLabel(int a);
        HRESULT CancelEditLabel();
        HRESULT GetEditItem(out ComCtl32.LVITEMINDEX /*LVITEMINDEX*/ item, out int a);
        HRESULT HitTest(ref ComCtl32.LVHITTESTINFO /*LVHITTESTINFO*/ result);
        HRESULT GetStringWidth(IntPtr a, out int b);
        HRESULT GetColumn(int a, out IntPtr col);
        HRESULT SetColumn(int a, ref IntPtr col);
        HRESULT GetColumnOrderArray(int a, out IntPtr b);
        HRESULT SetColumnOrderArray(int a, ref IntPtr b);
        HRESULT GetHeaderControl(out /* IntPtr*/ HWND header);
        HRESULT InsertColumn(int a, ref IntPtr b, out int c);
        HRESULT DeleteColumn(int a);
        HRESULT CreateDragImage(int a, Point /*POINT*/ b, out IntPtr c);
        HRESULT GetViewRect(out RECT rect);
        HRESULT GetClientRect(Boolean a, out RECT b);
        HRESULT GetColumnWidth(int iSubitem, out int width);
        HRESULT SetColumnWidth(int a, int b);
        HRESULT GetCallbackMask(out long a);
        HRESULT SetCallbackMask(out long b);
        HRESULT GetTopIndex(out int a);
        HRESULT GetCountPerPage(out int a);
        HRESULT GetOrigin(out Point /*POINT*/ p);
        HRESULT GetSelectedCount(out int a);
        HRESULT SortItems(bool a, IntPtr b, IntPtr c);
        HRESULT GetExtendedStyle(out IntPtr s);
        HRESULT SetExtendedStyle(long a, long b, out long c);
        HRESULT GetHoverTime(out uint a);
        HRESULT SetHoverTime(uint a, out uint b);
        HRESULT GetToolTip(out IntPtr a);
        HRESULT SetToolTip(IntPtr a, out IntPtr b);
        HRESULT GetHotItem(out ComCtl32.LVITEMINDEX /*LVITEMINDEX*/ a);
        HRESULT SetHotItem(ComCtl32.LVITEMINDEX /*LVITEMINDEX*/ a, out ComCtl32.LVITEMINDEX /*LVITEMINDEX*/ b);
        HRESULT GetHotCursor(out IntPtr a);
        HRESULT SetHotCursor(IntPtr a, out IntPtr b);
        HRESULT ApproximateViewRect(int a, out int b, out int c);
        HRESULT SetRangeObject(int a, out IntPtr b);
        HRESULT GetWorkAreas(int a, out RECT b);
        HRESULT SetWorkAreas(int a, ref RECT b);
        HRESULT GetWorkAreaCount(out IntPtr a);
        HRESULT ResetEmptyText();
        HRESULT EnableGroupView(int a);
        HRESULT IsGroupViewEnabled(out bool result);
        HRESULT SortGroups(/* PFNLVGROUPCOMPARE */IntPtr a, IntPtr b);
        HRESULT GetGroupInfo(int a, int b, out IntPtr c);
        HRESULT SetGroupInfo(int a, int b, IntPtr c);
        HRESULT GetGroupRect(bool a, int b, int c, out RECT d);
        HRESULT GetGroupState(int a, long b, out long c);
        HRESULT HasGroup(int a, out bool b);
        HRESULT InsertGroup(int index, ref ComCtl32.LVGROUP /*LVGROUP */ column, out int position);
        HRESULT RemoveGroup(int index);
        HRESULT InsertGroupSorted(IntPtr a, out int b);
        HRESULT GetGroupMetrics(out IntPtr a);
        HRESULT SetGroupMetrics(IntPtr a);
        HRESULT RemoveAllGroups();
        HRESULT GetFocusedGroup(out int a);
        HRESULT GetGroupCount(out int a);
        HRESULT SetOwnerDataCallback(IntPtr callback);
        HRESULT GetTileViewInfo(out ComCtl32.LVTILEVIEWINFO /*LVTILEVIEWINFO */ a);
        HRESULT SetTileViewInfo(ref ComCtl32.LVTILEVIEWINFO /*LVTILEVIEWINFO */ a);
        HRESULT GetTileInfo(out IntPtr a);
        HRESULT SetTileInfo(IntPtr a);
        HRESULT GetInsertMark(out IntPtr a);
        HRESULT SetInsertMark(IntPtr a);
        HRESULT GetInsertMarkRect(out RECT a);
        HRESULT GetInsertMarkColor(out IntPtr a);
        HRESULT SetInsertMarkColor(IntPtr a, out IntPtr b);
        HRESULT HitTestInsertMark(Point /*POINT*/ a, out IntPtr b);
        HRESULT SetInfoTip(IntPtr a);
        HRESULT GetOutlineColor(out IntPtr a);
        HRESULT SetOutlineColor(IntPtr a, out IntPtr b);
        HRESULT GetFrozenItem(out int a);
        HRESULT SetFrozenItem(int a, int b);
        HRESULT GetFrozenSlot(out RECT a);
        HRESULT SetFrozenSlot(int a, ref Point /*POINT*/ p);
        HRESULT GetViewMargin(out RECT a);
        HRESULT SetViewMargin(ref RECT a);
        HRESULT SetKeyboardSelected(ComCtl32.LVITEMINDEX /*LVITEMINDEX*/  index);
        HRESULT MapIndexToId(int a, out int b);
        HRESULT MapIdToIndex(int a, out int b);
        HRESULT IsItemVisible(ComCtl32.LVITEMINDEX /*LVITEMINDEX*/  a, out bool b);
        HRESULT EnableAlphaShadow(bool flag);                   /* WARNING!!!! Windows 7 only */
        HRESULT GetGroupSubsetCount(out int a);
        HRESULT SetGroupSubsetCount(int a);
        HRESULT GetVisibleSlotCount(out int a);
        HRESULT GetColumnMargin(ref RECT a);
        HRESULT SetSubItemCallback(IntPtr a);
        HRESULT GetVisibleItemRange(out ComCtl32.LVITEMINDEX /*LVITEMINDEX*/  a, out ComCtl32.LVITEMINDEX /*LVITEMINDEX*/ b);
        HRESULT SetTypeAheadFlags(uint a, uint b);
    }
}
