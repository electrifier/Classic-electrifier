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
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;


using electrifier.Win32API;



namespace common.Interop.WinShell
{
    #region Flags =============================================================================================================



    #endregion ================================================================================================================

    #region Common COM Interface GUIDs

    /// <summary>
    /// Common Shell32 Interface GUIDs.
    /// 
    /// Some of them have been taken from <see aref="https://referencesource.microsoft.com/#PresentationFramework/src/Framework/System/Windows/Standard/ComGuids.cs"/>
    /// </summary>
    internal static partial class IID
    {
        /// <summary>IID_IEnumIDList</summary>
        public const string EnumIdList = "000214f2-0000-0000-c000-000000000046";
        /// <summary>IID_IEnumObjects</summary>
        public const string EnumObjects = "2c1c7e2e-2d0e-4059-831e-1e6f82335c2e";
        /// <summary>IID_IHTMLDocument2</summary>
        public const string HtmlDocument2 = "332c4425-26cb-11d0-b483-00c04fd90119";
        /// <summary>IID_IModalWindow</summary>
        public const string ModalWindow = "b4db1657-70d7-485e-8e3e-6fcb5a5c1802";
        /// <summary>IID_IObjectArray</summary>
        public const string ObjectArray = "92ca9dcd-5622-4bba-a805-5e9f541bd8c9";
        /// <summary>IID_IObjectCollection</summary>
        public const string ObjectCollection = "5632b1a4-e38a-400a-928a-d4cd63230295";
        /// <summary>IID_IPropertyNotifySink</summary>
        public const string PropertyNotifySink = "9bfbbc02-eff1-101a-84ed-00aa00341d07";
        /// <summary>IID_IPropertyStore</summary>
        public const string PropertyStore = "886d8eeb-8cf2-4446-8d02-cdba1dbdcf99";
        /// <summary>IID_IServiceProvider</summary>
        public const string ServiceProvider = "6d5140c1-7436-11ce-8034-00aa006009fa";
        /// <summary>IID_IShellFolder</summary>
        public const string ShellFolder = "000214e6-0000-0000-c000-000000000046";
        /// <summary>IID_IShellLink</summary>
        public const string ShellLink = "000214f9-0000-0000-c000-000000000046";
        /// <summary>IID_IShellItem</summary>
        public const string ShellItem = "43826d1e-e718-42ee-bc55-a1e261c37bfe";
        /// <summary>IID_IShellItem2</summary>
        public const string ShellItem2 = "7e9fb0d3-919f-4307-ab2e-9b1860310c93";
        /// <summary>IID_IShellItemArray</summary>
        public const string ShellItemArray = "b63ea76d-1f85-456f-a19c-48159efa858b";
        /// <summary>IID_ITaskbarList</summary>
        public const string TaskbarList = "56fdf342-fd6d-11d0-958a-006097c9a090";
        /// <summary>IID_ITaskbarList2</summary>
        public const string TaskbarList2 = "602d4995-b13a-429b-a66e-1935e44f4317";
        /// <summary>IID_IUnknown</summary>
        public const string Unknown = "00000000-0000-0000-c000-000000000046";

        #region Win7 IIDs

        /// <summary>IID_IApplicationDestinations</summary>
        public const string ApplicationDestinations = "12337d35-94c6-48a0-bce7-6a9c69d4d600";
        /// <summary>IID_IApplicationDocumentLists</summary>
        public const string ApplicationDocumentLists = "3c594f9f-9f30-47a1-979a-c9e83d3d0a06";
        /// <summary>IID_ICustomDestinationList</summary>
        public const string CustomDestinationList = "6332debf-87b5-4670-90c0-5e57b408a49e";
        /// <summary>IID_IObjectWithAppUserModelID</summary>
        public const string ObjectWithAppUserModelId = "36db0196-9665-46d1-9ba7-d3709eecf9ed";
        /// <summary>IID_IObjectWithProgID</summary>
        public const string ObjectWithProgId = "71e806fb-8dee-46fc-bf8c-7748a8a1ae13";
        /// <summary>IID_ITaskbarList3</summary>
        public const string TaskbarList3 = "ea1afb91-9e28-4b86-90e9-9e9f8a5eefaf";
        /// <summary>IID_ITaskbarList4</summary>
        public const string TaskbarList4 = "c43dc798-95d1-4bea-9030-bb99e2983a1a";

        #endregion
    }
    #endregion

    #region Common COM Class GUIDs

    /// <summary>
    /// Common Shell32 Class GUIDs.
    /// 
    /// Some of them have been taken from <see aref="https://referencesource.microsoft.com/#PresentationFramework/src/Framework/System/Windows/Standard/ComGuids.cs"/>
    /// </summary>
    internal static partial class CLSID
    {
        public static T CoCreateInstance<T>(string clsid)
        {
            return (T)System.Activator.CreateInstance(System.Type.GetTypeFromCLSID(new System.Guid(clsid)));
        }

        /// <summary>CLSID_TaskbarList</summary>
        /// <remarks>IID_ITaskbarList</remarks>
        public const string TaskbarList = "56FDF344-FD6D-11d0-958A-006097C9A090";
        /// <summary>CLSID_EnumerableObjectCollection</summary>
        /// <remarks>IID_IEnumObjects.</remarks>
        public const string EnumerableObjectCollection = "2d3468c1-36a7-43b6-ac24-d3f02fd9607a";
        /// <summary>CLSID_ShellLink</summary>
        /// <remarks>IID_IShellLink</remarks>
        public const string ShellLink = "00021401-0000-0000-C000-000000000046";

        #region Win7 CLSIDs

        /// <summary>CLSID_DestinationList</summary>
        /// <remarks>IID_ICustomDestinationList</remarks>
        public const string DestinationList = "77f10cf0-3db5-4966-b520-b7c54fd35ed6";
        /// <summary>CLSID_ApplicationDestinations</summary>
        /// <remarks>IID_IApplicationDestinations</remarks>
        public const string ApplicationDestinations = "86c14003-4d6b-4ef3-a7b4-0506663b2e68";
        /// <summary>CLSID_ApplicationDocumentLists</summary>
        /// <remarks>IID_IApplicationDocumentLists</remarks>
        public const string ApplicationDocumentLists = "86bec222-30f2-47e0-9f25-60d11cd75c28";

        #endregion

        #region Various CLSIDs added by tajbender
        /// <summary>CLSID_FileOperation</summary>
        /// <remarks>IID_IFileOperation</remarks>
        public const string FileOperation = "3ad05575-8857-4850-9277-11b85bdb8e09";
        #endregion
    }
    #endregion



    public class Shell32
    {
        ///// <summary>
        ///// Creates and initializes a Shell item object from a parsing name.
        ///// 
        ///// <seealso href="https://docs.microsoft.com/en-us/windows/desktop/api/shobjidl_core/nf-shobjidl_core-shcreateitemfromparsingname"/>
        ///// </summary>
        ///// <param name="pszPath">A pointer to a display name.</param>
        ///// <param name="pbc">Optional. A pointer to a bind context used to pass parameters as inputs and outputs to the parsing function. These passed parameters are often specific to the data sourc
        ///// and are documented by the data source owners. For example, the file system data source accepts the name being parsed (as a WIN32_FIND_DATA structure), using the STR_FILE_SYS_BIND_DATA
        ///// bind context parameter.
        ///// STR_PARSE_PREFER_FOLDER_BROWSING can be passed to indicate that URLs are parsed using the file system data source when possible.Construct a bind context object using CreateBindCtx and
        ///// populate the values using IBindCtx::RegisterObjectParam. See Bind Context String Keys for a complete list of these.See the Parsing With Parameters Sample for an example of the use of
        ///// this parameter.
        ///// If no data is being passed to or received from the parsing function, this value can be NULL.</param>
        ///// <param name="riid">A reference to the IID of the interface to retrieve through ppv, typically IID_IShellItem or IID_IShellItem2.</param>
        ///// <param name="ppv">When this method returns successfully, contains the interface pointer requested in riid. This is typically IShellItem or IShellItem2.</param>
        ///// 
        ///// <returns>If this function succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>

        //[DllImport("shell32.dll", CharSet = CharSet.Unicode)]
        //public static extern WinError.HResult SHCreateItemFromParsingName(
        //    [MarshalAs(UnmanagedType.LPWStr)] string pszPath,
        //    IntPtr pbc,
        //    [MarshalAs(UnmanagedType.LPStruct)] Guid riid,
        //    [MarshalAs(UnmanagedType.Interface, IidParameterIndex = 2)] out IShellItem ppv);


        [DllImport("shell32.dll",
            SetLastError = true,
            CharSet = CharSet.Unicode,
            PreserveSig = false)]
        [return: MarshalAs(UnmanagedType.Interface)]
        public static extern IShellItem SHCreateItemFromParsingName(
            [MarshalAs(UnmanagedType.LPWStr)] string pszPath,
            System.Runtime.InteropServices.ComTypes.IBindCtx pbc,
            ref Guid riid);

        /// <summary>
        /// Retrieves the IShellFolder interface for the desktop folder, which is the root of the Shell's namespace.
        /// 
        /// <seealso href="https://docs.microsoft.com/en-us/windows/desktop/api/shlobj_core/nf-shlobj_core-shgetdesktopfolder"/>
        /// </summary>
        /// 
        /// <param name="ppshf">When this method returns, receives an IShellFolder interface pointer for the desktop folder. The
        /// calling application is responsible for eventually freeing the interface by calling its IUnknown::Release method.</param>
        /// 
        /// <returns>If this function succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [DllImport("shell32.dll")]
        public static extern WinError.HResult SHGetDesktopFolder(
            out WinShell.IShellFolder ppshf);

        [ObsoleteAttribute("SHGetMalloc interface is deprecated. Use CoTaskMemAlloc instead!")]
        [DllImport("shell32.dll")]
        public static extern WinError.HResult SHGetMalloc(
            out IntPtr hObject);        // TODO: [MarshalAs(UnmanagedType.IUnknown)]
    }
}
