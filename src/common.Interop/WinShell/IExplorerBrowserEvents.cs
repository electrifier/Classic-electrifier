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
        [ComImport, Guid("361bbdc7-e6ee-4e13-be58-58e2240c810f"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IExplorerBrowserEvents
        {
            [PreserveSig]
            WinError.HResult OnNavigationPending(
                IntPtr pidlFolder);

            [PreserveSig]
            WinError.HResult OnViewCreated(
                [MarshalAs(UnmanagedType.IUnknown)]
                object psv);

            [PreserveSig]
            WinError.HResult OnNavigationComplete(
                IntPtr pidlFolder);

            [PreserveSig]
            WinError.HResult OnNavigationFailed(
                IntPtr pidlFolder);
        }
    }
}
