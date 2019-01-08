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

namespace common.Interop.WinShell
{
    public static partial class Shell32
    {
        public enum CommDlgBrowserStateChange
        {
            SetFocus = 0,
            KillFocus = 1,
            SelectionChange = 2,
            Rename = 3,
            StateChange = 4
        }

        public enum CommDlgBrowserNotifyType
        {
            Done = 1,
            Start = 2
        }

        [ComImport, Guid("c8ad25a1-3294-41ee-8165-71174bd01c57"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface ICommDlgBrowser3
        {
            // ICommDlgBrowser1
            [PreserveSig]
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

            // ICommDlgBrowser2
            [PreserveSig]
            WinError.HResult GetDefaultMenuText(
                IShellView shellView,
                IntPtr buffer,
                int bufferMaxLength); //should be max size = 260?

            [PreserveSig]
            WinError.HResult GetViewFlags(
                [Out] out uint pdwFlags); // CommDlgBrowser2ViewFlags 

            [PreserveSig]
            WinError.HResult Notify(
                IntPtr pshv,
                CommDlgBrowserNotifyType notifyType);

            // ICommDlgBrowser3
            [PreserveSig]
            WinError.HResult GetCurrentFilter(
                StringBuilder pszFileSpec,
                int cchFileSpec);

            [PreserveSig]
            WinError.HResult OnColumnClicked(
                IShellView ppshv,
                int iColumn);

            [PreserveSig]
            WinError.HResult OnPreViewCreated(
                Shell32.IShellView ppshv);
        }
    }
}
