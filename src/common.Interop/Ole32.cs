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

using System.Runtime.InteropServices;

namespace common.Interop
{
    [System.ObsoleteAttribute("Class electrifier.Win32API.Ole32 is obsolete. It will be replaced and removed.")]
    public class Ole32
    {
        /// <summary>
        /// Allocates a block of task memory in the same way that IMalloc::Alloc does.
        /// 
        /// <seealso href="https://docs.microsoft.com/en-us/windows/desktop/api/combaseapi/nf-combaseapi-cotaskmemalloc"/>
        /// </summary>
        /// <param name="cb">The size of the memory block to be allocated, in bytes.</param>
        /// <returns>If the function succeeds, it returns the allocated memory block. Otherwise, it returns NULL.</returns>
        [DllImport("Ole32.dll", EntryPoint = "CoTaskMemAlloc")]
        public static extern System.IntPtr CoTaskMemAlloc(int cb);

        /// <summary>
        /// Frees a block of task memory previously allocated through a call to the CoTaskMemAlloc or CoTaskMemRealloc function.
        /// 
        /// <seealso href="https://docs.microsoft.com/en-us/windows/desktop/api/combaseapi/nf-combaseapi-cotaskmemfree"/>
        /// </summary>
        /// <param name="pv">A pointer to the memory block to be freed. If this parameter is NULL, the function has no effect.</param>
        [DllImport("Ole32.dll", EntryPoint = "CoTaskMemFree")]
        public static extern void CoTaskMemFree(System.IntPtr pv);
    }
}
