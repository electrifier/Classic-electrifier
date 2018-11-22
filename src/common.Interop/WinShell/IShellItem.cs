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
using System.Runtime.InteropServices.ComTypes;

using electrifier.Win32API;

namespace common.Interop.WinShell
{
    #region Flags =============================================================================================================

    /// <summary>
    /// Requests the form of an item's display name to retrieve through <see cref="IShellItem.GetDisplayName(SIGDN)">IShellItem.GetDisplayName</see> and SHGetNameFromIDList.
    /// 
    /// https://docs.microsoft.com/en-us/windows/desktop/api/shobjidl_core/ne-shobjidl_core-_sigdn
    /// </summary>
    [Flags]
    public enum SIGDN : uint
    {
        DesktopAbsoluteEditing = 0x8004C000,
        DesktopAbsoluteParsing = 0x80028000,
        FileSysPath = 0x80058000,
        NormalDisplay = 0x00000000,
        ParentRelativeEditing = 0x80031001,
        ParentRelativeForAddressbar = 0x8001C001,
        ParentRelativeParsing = 0x80018001,
        Url = 0x80068000,
    }

    /// <summary>
    /// Used to determine how to compare two Shell items.
    /// IShellItem::Compare uses this enumerated type.
    /// 
    /// <seealso href="https://docs.microsoft.com/en-us/windows/desktop/api/shobjidl_core/ne-shobjidl_core-_sichintf"/>
    /// </summary>
    [Flags]
    public enum SICHINTF : uint
    {
        Display = 0x00000000,                       // This relates to the iOrder parameter of the IShellItem::Compare interface and indicates that the comparison is based on the display in a folder view.
        AllFields = 0x80000000,                     // Exact comparison of two instances of a Shell item.
        Canonical = 0x10000000,                     // This relates to the iOrder parameter of the IShellItem::Compare interface and indicates that the comparison is based on a canonical name.
        Test_Filesyspath_If_Not_Equal = 0x20000000, // Windows 7 and later. If the Shell items are not the same, test the file system paths.
    }

    #endregion ================================================================================================================

    /// <summary>
    /// https://docs.microsoft.com/en-us/windows/desktop/api/shobjidl_core/nn-shobjidl_core-ishellitem
    /// 
    /// 
    /// </summary>
    /// <summary>
    /// Shell Namespace helper
    /// </summary>
    [
        ComImport,
        Guid("43826D1E-E718-42EE-BC55-A1E261C37BFE"),
        InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IShellItem
    {
        [return: MarshalAs(UnmanagedType.Interface)]
        object BindToHandler(
            IBindCtx pbc,
            [In] ref Guid bhid,
            [In] ref Guid riid);

        IShellItem GetParent();

        [return: MarshalAs(UnmanagedType.LPWStr)]
        string GetDisplayName(
            SIGDN sigdnName);

        SFGAO GetAttributes(
            SFGAO sfgaoMask);

        int Compare(
            IShellItem psi,
            SICHINTF hint);
    }
}
