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
    [System.ObsoleteAttribute("Class electrifier.Win32API.WinError is obsolete. It will be replaced and removed.")]
    public class WinError
    {
        #region HResult-Implementation as a helper struct

        /// <summary>
        /// Helper struct for HRESULT values of Windows API.
        /// 
        /// See https://msdn.microsoft.com/en-us/library/windows/desktop/aa378137(v=vs.85).aspx
        /// 
        /// The following HRESULT values are the most common. More values are contained in the header file Winerror.h.
        /// </summary>
        [StructLayout(LayoutKind.Explicit)]
        public struct HResult
        {
            [FieldOffset(0)]
            private readonly int Value;     // TODO: Convert to uint

            public static readonly HResult S_OK = new HResult(0x00000000);              // Operation successful
            public static readonly HResult E_NotImpl = new HResult(0x80004001);         // Not implemented
            public static readonly HResult E_NoInterface = new HResult(0x80004002);     // No such interface supported
            public static readonly HResult E_Pointer = new HResult(0x80004003);         // Pointer that is not valid
            public static readonly HResult E_Abort = new HResult(0x80004004);           // Operation aborted
            public static readonly HResult E_Fail = new HResult(0x80004005);            // Unspecified failure
            public static readonly HResult E_Unexpected = new HResult(0x8000FFFF);      // Unexpected failure
            public static readonly HResult E_AccessDenied = new HResult(0x80070005);    // General access denied error
            public static readonly HResult E_Handle = new HResult(0x80070006);          // Handle that is not valid
            public static readonly HResult E_OutOfMemory = new HResult(0x8007000E);     // Failed to allocate necessary memory
            public static readonly HResult E_InvalidArg = new HResult(0x80070057);      // One or more arguments are not valid

            public HResult(int value) { this.Value = value; }
            public HResult(uint value) { this.Value = unchecked((int)value); }

            public static implicit operator HResult(int value) { return (new common.Interop.WinError.HResult(value)); }
            public static implicit operator HResult(uint value) { return (new common.Interop.WinError.HResult(value)); }

            public bool Succeeded { get { return (HResult.S_OK == this.Value); } }
            public bool Failed { get { return (HResult.S_OK != this.Value); } }

            public static bool operator ==(HResult hResultA, HResult hResultB) { return (hResultA.Value == hResultB.Value); }
            public static bool operator !=(HResult hResultA, HResult hResultB) { return (hResultA.Value != hResultB.Value); }

            public void ThrowException() { Marshal.ThrowExceptionForHR(this.Value); }
            public override int GetHashCode() { return this.Value; }
            public override string ToString() { return this.Value.ToString("X8"); }

            public override bool Equals(object obj)
            {
                if (null == obj)
                    return false;

                HResult HResultObj = (HResult)obj;
                if ((object)HResultObj == null)
                    return false;

                return (this.Value == HResultObj.Value);
            }
        }

        #endregion

    }
}
