/*
** 
**  electrifier
** 
**  Copyright 2018 Thorsten Jung, www.electrifier.org
**  
**  Licensed under the Apache License, Version 2.0 (the "License");
**  you may not use this file except in compliance with the License.
**  You may obtain a copy of the License at
**  
**      http://www.apache.org/licenses/LICENSE-2.0
**  
**  Unless required by applicable law or agreed to in writing, software
**  distributed under the License is distributed on an "AS IS" BASIS,
**  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
**  See the License for the specific language governing permissions and
**  limitations under the License.
**
*/

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace common.Interop
{
    public static partial class Shell32
    {
        /// <summary>
        /// Used with the IFolderView::Items, IFolderView::ItemCount, and IShellView::GetItemObject
        /// methods to restrict or control the items in their collections.
        /// </summary>
        public enum ShellViewGetItemObject
        {
            /// <summary>
            /// SVGIO_BACKGROUND
            /// Refers to the background of the view. It is used with IID_IContextMenu to get a shortcut menu for the view background 
            /// and with IID_IDispatch to get a dispatch interface that represents the ShellFolderView object for the view.
            /// </summary>
            Background = 0x00000000,
            /// <summary>
            /// SVGIO_SELECTION
            /// Refers to the currently selected items. Used with IID_IDataObject to retrieve a data object that represents the selected items.
            /// </summary>
            Selection = 0x00000001,
            /// <summary>
            /// SVGIO_ALLVIEW
            /// Used in the same way as SVGIO_SELECTION but refers to all items in the view.
            /// </summary>
            AllView = 0x00000002,
            /// <summary>
            /// SVGIO_CHECKED
            /// Used in the same way as SVGIO_SELECTION but refers to checked items in views where checked mode is supported.
            /// For more details on checked mode, see FOLDERFLAGS.
            /// </summary>
            Checked = 0x00000003,
            /// <summary>
            /// SVGIO_TYPE_MASK	0x0000000F. Masks all bits but those corresponding to the _SVGIO flags.
            /// </summary>
            TypeMask = 0x0000000F,
            /// <summary>
            /// SVGIO_FLAG_VIEWORDER
            /// Returns the items in the order they appear in the view. If this flag is not set, the selected item will be listed first.
            /// </summary>
            ViewOrderFlag = unchecked((int)0x80000000)
        }

        [ComImport, Guid("000214e3-0000-0000-c000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IShellView
        {
            // IOleWindow
            [PreserveSig]
            WinError.HResult GetWindow(
                out IntPtr phwnd);

            [PreserveSig]
            WinError.HResult ContextSensitiveHelp(
                bool fEnterMode);

            // IShellView
            [PreserveSig]
            WinError.HResult TranslateAccelerator(
                IntPtr pmsg);

            [PreserveSig]
            WinError.HResult EnableModeless(
                bool fEnable);

            [PreserveSig]
            WinError.HResult UIActivate(
                uint uState);

            [PreserveSig]
            WinError.HResult Refresh();

            [PreserveSig]
            WinError.HResult CreateViewWindow(
                [MarshalAs(UnmanagedType.IUnknown)]
                object psvPrevious,
                IntPtr pfs,
                [MarshalAs(UnmanagedType.IUnknown)]
                object psb,
                IntPtr prcView,
                out IntPtr phWnd);

            [PreserveSig]
            WinError.HResult DestroyViewWindow();

            [PreserveSig]
            WinError.HResult GetCurrentInfo(
                out IntPtr pfs);

            [PreserveSig]
            WinError.HResult AddPropertySheetPages(
                uint dwReserved,
                IntPtr pfn,
                uint lparam);

            [PreserveSig]
            WinError.HResult SaveViewState();

            [PreserveSig]
            WinError.HResult SelectItem(
                IntPtr pidlItem,
                uint uFlags);

            [PreserveSig]
            WinError.HResult GetItemObject(
                ShellViewGetItemObject uItem,
                ref Guid riid,
                [MarshalAs(UnmanagedType.IUnknown)]
                out object ppv);
        }
    }
}
