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
using ComTypes = System.Runtime.InteropServices.ComTypes;

namespace common.Interop.WinShell
{
    #region Flags =============================================================================================================

    [Flags]
    public enum SHGDN : uint
    {
        NORMAL = 0x00000000,            // default (display purpose)
        INFOLDER = 0x00000001,          // displayed under a folder (relative)
        FOREDITING = 0x00001000,        // for in-place editing
        FORADDRESSBAR = 0x00004000,     // UI friendly parsing name (remove ugly stuff)
        FORPARSING = 0x00008000,        // parsing name for ParseDisplayName()
    }

    [Flags]
    public enum SHCONTF : uint
    {
        FOLDERS = 0x00000020,               // only want folders enumerated (SFGAO_FOLDER)
        NONFOLDERS = 0x00000040,            // include non folders
        INCLUDEHIDDEN = 0x00000080,         // show items normally hidden
        INIT_ON_FIRST_NEXT = 0x00000100,    // allow EnumObject() to return before validating enum
        NETPRINTERSRCH = 0x00000200,        // hint that client is looking for printers
        SHAREABLE = 0x00000400,             // hint that client is looking sharable resources (remote shares)
        STORAGE = 0x00000800,               // include all items with accessible storage and their ancestors
        //DefaultForTreeView = ShellAPI.SHCONTF.FOLDERS | ShellAPI.SHCONTF.INCLUDEHIDDEN,
        //DefaultForListView = ShellAPI.SHCONTF.FOLDERS | ShellAPI.SHCONTF.NONFOLDERS | ShellAPI.SHCONTF.INCLUDEHIDDEN,
    }

    #endregion ================================================================================================================

    /// <summary>
    /// Attributes that can be retrieved on an item (file or folder) or set of items.
    /// 
    /// See https://msdn.microsoft.com/en-us/library/windows/desktop/bb762589(v=vs.85).aspx
    /// 
    /// </summary> 
    [Flags]
    public enum SFGAO : uint
    {
        CanCopy = 0x1,
        CanMove = 0x2,
        CanLink = 0x4,
        Storage = 0x8,
        CanRename = 0x10,
        CanDelete = 0x20,
        HasPropSheet = 0x40,
        DropTarget = 0x100,
        CAPABILITYMASK = 0x177,
        Encrypted = 0x2000,
        IsSlow = 0x4000,
        Ghosted = 0x8000,
        Link = 0x10000,
        Share = 0x20000,
        ReadOnly = 0x40000,
        Hidden = 0x80000,
        DISPLAYATTRMASK = 0xFC000,
        FileSysAncestor = 0x10000000,
        Folder = 0x20000000,
        FileSystem = 0x40000000,
        HasSubfolder = 0x80000000,
        CONTENTSMASK = 0x80000000,
        Validate = 0x1000000,
        Removable = 0x2000000,
        Compressed = 0x4000000,
        Browsable = 0x8000000,
        Nonenumerated = 0x100000,
        NewContent = 0x200000,
        Stream = 0x400000,
        StorageAncestor = 0x800000,
        STORAGECAPMASK = 0x70C50008,
    }

    [
        ComImport,
        Guid("000214E6-0000-0000-C000-000000000046"),
        InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IShellFolder
    {
        /// <summary>
        /// Translates the display name of a file object or a folder into an item identifier list.
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="pbc"></param>
        /// <param name="pszDisplayName"></param>
        /// <param name="pchEaten"></param>
        /// <param name="ppidl"></param>
        /// <param name="pdwAttributes"></param>
        void ParseDisplayName(
            [In] IntPtr hwnd,
            [In] ComTypes.IBindCtx pbc,
            [In, MarshalAs(UnmanagedType.LPWStr)] string pszDisplayName,
            [In, Out] ref int pchEaten,
            [Out] out IntPtr ppidl,
            [In, Out] ref uint pdwAttributes);

        /// <summary>
        /// Enables a client to determine the contents of a folder by creating an item identifier enumeration object and returning its IEnumIDList interface.
        /// The methods supported by that interface can then be used to enumerate the folder's contents.
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="grfFlags"></param>
        /// <returns></returns>
        IEnumIDList EnumObjects(
            [In] IntPtr hwnd,
            [In] SHCONTF grfFlags);

        /// <summary>
        /// Retrieves a handler, typically the Shell folder object that implements IShellFolder for a particular item.
        /// Optional parameters that control the construction of the handler are passed in the bind context.
        /// </summary>
        /// <param name="pidl"></param>
        /// <param name="pbc"></param>
        /// <param name="riid"></param>
        /// <returns></returns>
        [return: MarshalAs(UnmanagedType.Interface)]
        object BindToObject(
            [In] IntPtr pidl,
            [In] ComTypes.IBindCtx pbc,
            [In] ref Guid riid);

        /// <summary>
        /// Requests a pointer to an object's storage interface.
        /// </summary>
        /// <param name="pidl"></param>
        /// <param name="pbc"></param>
        /// <param name="riid"></param>
        /// <returns></returns>
        [return: MarshalAs(UnmanagedType.Interface)]
        object BindToStorage(
            [In] IntPtr pidl,
            [In] ComTypes.IBindCtx pbc,
            [In] ref Guid riid);

        /// <summary>
        /// Determines the relative order of two file objects or folders, given their item identifier lists.
        /// </summary>
        /// <param name="lParam"></param>
        /// <param name="pidl1"></param>
        /// <param name="pidl2"></param>
        /// <returns></returns>
        [PreserveSig]
        common.Interop.WinError.HResult CompareIDs(
            [In] IntPtr lParam,
            [In] IntPtr pidl1,
            [In] IntPtr pidl2);

        /// <summary>
        /// Requests an object that can be used to obtain information from or interact with a folder object.
        /// </summary>
        /// <param name="hwndOwner"></param>
        /// <param name="riid"></param>
        /// <returns></returns>
        [return: MarshalAs(UnmanagedType.Interface)]
        object CreateViewObject(
            [In] IntPtr hwndOwner,
            [In] ref Guid riid);

        /// <summary>
        /// Gets the attributes of one or more file or folder objects contained in the object represented by IShellFolder.
        /// </summary>
        /// <param name="cidl"></param>
        /// <param name="apidl"></param>
        /// <param name="rgfInOut"></param>
        void GetAttributesOf(
            [In] uint cidl,
            [In] IntPtr apidl,
            [In, Out] ref SFGAO rgfInOut);

        /// <summary>
        /// Gets an object that can be used to carry out actions on the specified file objects or folders.
        /// </summary>
        /// <param name="hwndOwner"></param>
        /// <param name="cidl"></param>
        /// <param name="apidl"></param>
        /// <param name="riid"></param>
        /// <param name="rgfReserved"></param>
        /// <returns></returns>
        [return: MarshalAs(UnmanagedType.Interface)]
        object GetUIObjectOf(
            [In] IntPtr hwndOwner,
            [In] uint cidl,
            [In, MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.SysInt, SizeParamIndex = 2)] IntPtr apidl,
            [In] ref Guid riid,
            [In, Out] ref uint rgfReserved);

        /// <summary>
        /// Retrieves the display name for the specified file object or subfolder.
        /// </summary>
        /// <param name="pidl"></param>
        /// <param name="uFlags"></param>
        /// <param name="pName"></param>
        void GetDisplayNameOf(
            [In] IntPtr pidl,
            [In] SHGDN uFlags,
            [Out] out IntPtr pName);

        /// <summary>
        /// Sets the display name of a file object or subfolder, changing the item identifier in the process.
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="pidl"></param>
        /// <param name="pszName"></param>
        /// <param name="uFlags"></param>
        /// <param name="ppidlOut"></param>
        void SetNameOf(
            [In] IntPtr hwnd,
            [In] IntPtr pidl,
            [In, MarshalAs(UnmanagedType.LPWStr)] string pszName,
            [In] SHGDN uFlags,
            [Out] out IntPtr ppidlOut);
    }
}
