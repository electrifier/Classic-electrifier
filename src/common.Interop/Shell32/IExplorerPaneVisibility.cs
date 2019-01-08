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
        public enum ExplorerPaneState
        {
            DoNotCare = 0x00000000,
            DefaultOn = 0x00000001,
            DefaultOff = 0x00000002,
            StateMask = 0x0000ffff,
            InitialState = 0x00010000,
            Force = 0x00020000
        }

        [ComImport, Guid("e07010ec-bc17-44c0-97b0-46c7c95b9edc"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IExplorerPaneVisibility
        {
            [PreserveSig]
            WinError.HResult GetPaneState(
                ref Guid explorerPane,
                out ExplorerPaneState peps);
        };
    }
}
