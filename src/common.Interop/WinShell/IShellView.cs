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

namespace common.Interop.WinShell
{
    public static partial class Shell32
    {

        public enum ShellViewGetItemObject
        {
            Background = 0x00000000,
            Selection = 0x00000001,
            AllView = 0x00000002,
            Checked = 0x00000003,
            TypeMask = 0x0000000F,
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
