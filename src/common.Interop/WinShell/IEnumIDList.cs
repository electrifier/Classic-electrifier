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

namespace common.Interop.WinShell
{
    [
        ComImport,
        Guid("000214F2-0000-0000-C000-000000000046"),
        InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IEnumIDList
    {
        [PreserveSig]
        int Next(
            int celt,
            ref IntPtr rgelt,
            out int pceltFetched);

        [PreserveSig]
        void Skip(
            int celt);

        [PreserveSig]
        int Reset();

        [PreserveSig]
        void Clone(
            ref IEnumIDList ppenum);
    };
}
