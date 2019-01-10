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
            private readonly int hr;

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

            public HResult(int value) { this.hr = value; }
            public HResult(uint value) { this.hr = unchecked((int)value); }

            public static implicit operator HResult(int value) { return (new HResult(value)); }
            public static implicit operator HResult(uint value) { return (new HResult(value)); }

            public bool Succeeded { get { return (this.hr >= 0); } }
            public bool Failed { get { return (this.hr < 0); } }

            public static bool operator ==(HResult hResultA, HResult hResultB) { return (hResultA.hr == hResultB.hr); }
            public static bool operator !=(HResult hResultA, HResult hResultB) { return (hResultA.hr != hResultB.hr); }

            public void ThrowException() { Marshal.ThrowExceptionForHR(this.hr); }
            public override int GetHashCode() { return this.hr; }
            public override string ToString() { return this.hr.ToString("0xX8"); }

            public override bool Equals(object obj)
            {
                if (null == obj)
                    return false;

                HResult HResultObj = (HResult)obj;
                if (((object)HResultObj) == null)
                    return false;

                return (this.hr == HResultObj.hr);
            }
        }

        #endregion

    }
}
