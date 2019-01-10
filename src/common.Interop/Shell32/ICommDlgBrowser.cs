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
using System.Text;

namespace common.Interop
{
    public static partial class Shell32
    {
        public enum CommDlgBrowserStateChange
        {
            /// <summary>
            /// The focus has been set to the view.
            /// </summary>
            SetFocus = 0,
            /// <summary>
            /// The view has lost the focus.
            /// </summary>
            KillFocus = 1,
            /// <summary>
            /// The selection has changed.
            /// </summary>
            SelectionChange = 2,
            /// <summary>
            /// An item has been renamed.
            /// </summary>
            Rename = 3,
            /// <summary>
            /// An item has been checked or unchecked.
            /// </summary>
            StateChange = 4
        }

        public enum CommDlgBrowserNotifyType
        {
            Done = 1,
            Start = 2
        }

        [Flags]
        public enum CommDlgBrowserViewFlags
        {
            /// <summary>
            /// All files, including hidden and system files, should be shown. In Windows XP, this is the only recognized flag.
            /// </summary>
            ShowAllFiles = 0x00000001,
            /// <summary>
            /// This browser is designated to choose a file to save.
            /// </summary>
            IsFileSave = 0x00000002,
            /// <summary>
            /// Not used.
            /// </summary>
            AllowPreviewPane = 0x00000004,
            /// <summary>
            /// Do not show a "select" verb on an item's context menu.
            /// </summary>
            NoSelectVerb = 0x00000008,
            /// <summary>
            /// IncludeObject should not be called.
            /// </summary>
            NoIncludeItem = 0x00000010,
            /// <summary>
            /// This browser is designated to pick folders.
            /// </summary>
            IsFolderPicker = 0x00000020,
            /// <summary>
            /// Windows 7 and later. Displays a UAC shield on the selected item when CDB2GVF_NOSELECTVERB is not specified.
            /// </summary>
            AddShield = 0x00000040,
        }

        [ComImport, Guid("000214f1-0000-0000-c000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface ICommDlgBrowser
        {
            // Note: Not implemented yet, only GUID is used.
        }

        [ComImport, Guid("10339516-2894-11d2-9039-00c04f8eeb3e"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface ICommDlgBrowser2
        {
            // Note: Not implemented yet, only GUID is used.
        }

        [ComImport, Guid("c8ad25a1-3294-41ee-8165-71174bd01c57"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface ICommDlgBrowser3
        {
            [PreserveSig]                                   // ICommDlgBrowser1
            WinError.HResult OnDefaultCommand(
                IntPtr ppshv);

            [PreserveSig]
            WinError.HResult OnStateChange(
                IntPtr ppshv,
                CommDlgBrowserStateChange uChange);

            [PreserveSig]
            WinError.HResult IncludeObject(
                IntPtr ppshv,
                IntPtr pidl);

            [PreserveSig]                                   // ICommDlgBrowser2
            WinError.HResult GetDefaultMenuText(
                IShellView shellView,
                IntPtr buffer,
                int bufferMaxLength);

            [PreserveSig]
            WinError.HResult GetViewFlags(
                out CommDlgBrowserViewFlags pdwFlags);

            [PreserveSig]
            WinError.HResult Notify(
                IntPtr pshv,
                CommDlgBrowserNotifyType notifyType);

            [PreserveSig]                                   // ICommDlgBrowser3
            WinError.HResult GetCurrentFilter(
                StringBuilder pszFileSpec,
                int cchFileSpec);

            [PreserveSig]
            WinError.HResult OnColumnClicked(
                IShellView ppshv,
                int iColumn);

            [PreserveSig]
            WinError.HResult OnPreViewCreated(
                IShellView ppshv);
        }
    }
}
