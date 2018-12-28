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
using System.Runtime.InteropServices;

namespace common.Interop
{
    [System.ObsoleteAttribute("Class electrifier.Win32API.User32 is obsolete. It will be replaced and removed.")]
    public class User32
    {
        /// <summary>
        /// Places the given window in the system-maintained clipboard format listener list.
        /// 
        /// <remark>When a window has been added to the clipboard format listener list, it is posted a WM_CLIPBOARDUPDATE message whenever the contents of the clipboard have changed.</remark>
        /// 
        /// <seealso cref="RemoveClipboardFormatListener"/>
        /// <seealso href="https://docs.microsoft.com/en-us/windows/desktop/api/winuser/nf-winuser-addclipboardformatlistener"/>
        /// </summary>
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool AddClipboardFormatListener(IntPtr hWnd);

        /// <summary>
        /// Removes the given window from the system-maintained clipboard format listener list.
        /// 
        /// <remark>When a window has been removed from the clipboard format listener list, it will no longer receive WM_CLIPBOARDUPDATE messages.</remark>
        /// 
        /// <seealso cref="AddClipboardFormatListener"/>
        /// <seealso href="https://docs.microsoft.com/en-us/windows/desktop/api/winuser/nf-winuser-removeclipboardformatlistener"/>
        /// </summary>
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool RemoveClipboardFormatListener(IntPtr hWnd);
    }
}
