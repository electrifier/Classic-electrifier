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



namespace common.Interop
{
    public static partial class Shell32
    {
        #region Common COM Interface GUIDs

        /// <summary>
        /// Common Shell32 Interface GUIDs.
        /// 
        /// Some of them have been taken from <see aref="https://referencesource.microsoft.com/#PresentationFramework/src/Framework/System/Windows/Standard/ComGuids.cs"/>
        /// </summary>
        internal static partial class IID
        {
            /// <summary>IID_IEnumIDList</summary>
            public const string IEnumIdList = "000214f2-0000-0000-c000-000000000046";
            /// <summary>IID_IEnumObjects</summary>
            public const string IEnumObjects = "2c1c7e2e-2d0e-4059-831e-1e6f82335c2e";
            /// <summary>IID_IHTMLDocument2</summary>
            public const string IHtmlDocument2 = "332c4425-26cb-11d0-b483-00c04fd90119";
            /// <summary>IID_IModalWindow</summary>
            public const string IModalWindow = "b4db1657-70d7-485e-8e3e-6fcb5a5c1802";
            /// <summary>IID_IObjectArray</summary>
            public const string IObjectArray = "92ca9dcd-5622-4bba-a805-5e9f541bd8c9";
            /// <summary>IID_IObjectCollection</summary>
            public const string IObjectCollection = "5632b1a4-e38a-400a-928a-d4cd63230295";
            /// <summary>IID_IPropertyNotifySink</summary>
            public const string IPropertyNotifySink = "9bfbbc02-eff1-101a-84ed-00aa00341d07";
            /// <summary>IID_IPropertyStore</summary>
            public const string IPropertyStore = "886d8eeb-8cf2-4446-8d02-cdba1dbdcf99";
            /// <summary>IID_IServiceProvider</summary>
            public const string IServiceProvider = "6d5140c1-7436-11ce-8034-00aa006009fa";
            /// <summary>IID_IShellFolder</summary>
            public const string IShellFolder = "000214e6-0000-0000-c000-000000000046";
            /// <summary>IID_IShellLink</summary>
            public const string IShellLink = "000214f9-0000-0000-c000-000000000046";
            /// <summary>IID_IShellItem</summary>
            public const string IShellItem = "43826d1e-e718-42ee-bc55-a1e261c37bfe";
            /// <summary>IID_IShellItem2</summary>
            public const string IShellItem2 = "7e9fb0d3-919f-4307-ab2e-9b1860310c93";
            /// <summary>IID_IShellItemArray</summary>
            public const string IShellItemArray = "b63ea76d-1f85-456f-a19c-48159efa858b";
            /// <summary>IID_ITaskbarList</summary>
            public const string ITaskbarList = "56fdf342-fd6d-11d0-958a-006097c9a090";
            /// <summary>IID_ITaskbarList2</summary>
            public const string ITaskbarList2 = "602d4995-b13a-429b-a66e-1935e44f4317";
            /// <summary>IID_IUnknown</summary>
            public const string IUnknown = "00000000-0000-0000-c000-000000000046";

            #region Win7 IIDs

            /// <summary>IID_IApplicationDestinations</summary>
            public const string IApplicationDestinations = "12337d35-94c6-48a0-bce7-6a9c69d4d600";
            /// <summary>IID_IApplicationDocumentLists</summary>
            public const string IApplicationDocumentLists = "3c594f9f-9f30-47a1-979a-c9e83d3d0a06";
            /// <summary>IID_ICustomDestinationList</summary>
            public const string ICustomDestinationList = "6332debf-87b5-4670-90c0-5e57b408a49e";
            /// <summary>IID_IObjectWithAppUserModelID</summary>
            public const string IObjectWithAppUserModelId = "36db0196-9665-46d1-9ba7-d3709eecf9ed";
            /// <summary>IID_IObjectWithProgID</summary>
            public const string IObjectWithProgId = "71e806fb-8dee-46fc-bf8c-7748a8a1ae13";
            /// <summary>IID_ITaskbarList3</summary>
            public const string ITaskbarList3 = "ea1afb91-9e28-4b86-90e9-9e9f8a5eefaf";
            /// <summary>IID_ITaskbarList4</summary>
            public const string ITaskbarList4 = "c43dc798-95d1-4bea-9030-bb99e2983a1a";

            #endregion

            /// <summary>IID_DShellFolderViewEvents</summary>
            public const string DShellFolderViewEvents = "62112aa2-ebe4-11cf-a5fb-0020afe7292d";
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
            public const string TaskbarList = "56fdf344-fd6d-11d0-958a-006097c9a090";
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
            /// <summary>CLSID_ExplorerBrowser</summary>
            /// <remarks>IID_IExplorerBrowser</remarks>
            public const string ExplorerBrowser = "71f96385-ddd6-48d3-a0c1-ae06e8b055fb";
            /// <summary>CLSID_FileOperation</summary>
            /// <remarks>IID_IFileOperation</remarks>
            public const string FileOperation = "3ad05575-8857-4850-9277-11b85bdb8e09";
            #endregion
        }
        #endregion


        #region Constants =====================================================================================================

        /// <summary>
        /// Specifies the folder view type.
        /// 
        /// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/bb762510(v=vs.85).aspx"/>
        /// </summary>
        public enum FolderViewMode : int
        {
            /// <summary>
            /// FVM_AUTO
            /// The view should determine the best option.
            /// </summary>
            Auto = -1,
            /// <summary>
            /// FVM_FIRST
            /// The minimum constant value in FOLDERVIEWMODE, for validation purposes.
            /// </summary>
            First = 1,
            /// <summary>
            /// FVM_ICON
            /// The view should display medium-size icons.
            /// </summary>
            Icon = 1,
            /// <summary>
            /// FVM_SMALLICON
            /// The view should display small icons.
            /// </summary>
            SmallIcon = 2,
            /// <summary>
            /// FVM_LIST
            /// Object names are displayed in a list view.
            /// </summary>
            List = 3,
            /// <summary>
            /// FVM_DETAILS
            /// Object names and other selected information, such as the size or date last updated, are shown.
            /// </summary>
            Details = 4,
            /// <summary>
            /// FVM_THUMBNAIL
            /// The view should display thumbnail icons.
            /// </summary>
            Thumbnail = 5,
            /// <summary>
            /// FVM_TILE
            /// The view should display large icons.
            /// </summary>
            Tile = 6,
            /// <summary>
            /// FVM_THUMBSTRIP
            /// The view should display icons in a filmstrip format.
            /// </summary>
            Thumbstrip = 7,
            /// <summary>
            /// FVM_CONTENT
            /// Windows 7 and later. The view should display content mode.
            /// </summary>
            Content = 8,
            /// <summary>
            /// FVM_LAST:
            /// The maximum constant value in FOLDERVIEWMODE, for validation purposes.
            /// </summary>
            Last = 8
        };

        /// <summary>
        /// A set of flags that specify folder view options.
        /// The flags are independent of each other and can be used in any combination.
        /// 
        /// <seealso href="https://docs.microsoft.com/en-us/windows/desktop/api/shobjidl_core/ne-shobjidl_core-folderflags"/>
        /// </summary>
        [Flags]
        public enum FolderFlags : uint
        {
            /// <summary>
            /// FWF_NONE
            /// Windows 7 and later. No special view options.
            /// </summary>
            None = 0x00000000,
            /// <summary>
            /// FWF_AUTOARRANGE
            /// Automatically arrange the elements in the view.
            /// This implies LVS_AUTOARRANGE if the list-view control is used to implement the view.
            /// </summary>
            AutoArrange = 0x00000001,
            /// <summary>
            /// FWF_ABBREVIATEDNAMES
            /// Not supported.
            /// </summary>
            AbbreviatedNames = 0x00000002,
            /// <summary>
            /// FWF_SNAPTOGRID
            /// Not supported.
            /// </summary>
            SnapToGrid = 0x00000004,
            /// <summary>
            /// FWF_OWNERDATA
            /// Not supported.
            /// </summary>
            OwnerData = 0x00000008,
            /// <summary>
            /// FWF_BESTFITWINDOW
            /// Not supported.
            /// </summary>
            BestFitWindow = 0x00000010,
            /// <summary>
            /// FWF_DESKTOP
            /// Make the folder behave like the desktop. This value applies only to the desktop and is
            /// not used for typical Shell folders. This flag implies FWF_NOCLIENTEDGE and FWF_NOSCROLL.
            /// </summary>
            Desktop = 0x00000020,
            /// <summary>
            /// FWF_SINGLESEL
            /// Do not allow more than a single item to be selected. This is used in the common dialog boxes.
            /// </summary>
            SingleSelection = 0x00000040,
            /// <summary>
            /// FWF_NOSUBFOLDERS
            /// Do not show subfolders.
            /// </summary>
            NoSubfolders = 0x00000080,
            /// <summary>
            /// FWF_TRANSPARENT
            /// Draw transparently. This is used only for the desktop.
            /// </summary>
            Transparent = 0x00000100,
            /// <summary>
            /// FWF_NOCLIENTEDGE
            /// Not supported.
            /// </summary>
            NoClientEdge = 0x00000200,
            /// <summary>
            /// FWF_NOSCROLL
            /// Do not add scroll bars. This is used only for the desktop.
            /// </summary>
            NoScroll = 0x00000400,
            /// <summary>
            /// FWF_ALIGNLEFT
            /// The view should be left-aligned. This implies LVS_ALIGNLEFT if the list-view control is used to implement the view.
            /// </summary>
            AlignLeft = 0x00000800,
            /// <summary>
            /// FWF_NOICONS
            /// The view should not display icons.
            /// </summary>
            NoIcons = 0x00001000,
            /// <summary>
            /// FWF_SHOWSELALWAYS
            /// This flag is deprecated as of Windows XP and has no effect. Always show the selection.
            /// </summary>
            ShowSelectionAlways = 0x00002000,
            /// <summary>
            /// FWF_NOVISIBLE
            /// Not supported.
            /// </summary>
            NoVisible = 0x00004000,
            /// <summary>
            /// FWF_SINGLECLICKACTIVATE
            /// Not supported.
            /// </summary>
            SingleClickActivate = 0x00008000,
            /// <summary>
            /// FWF_NOWEBVIEW
            /// The view should not be shown as a web view.
            /// </summary>
            NoWebView = 0x00010000,
            /// <summary>
            /// FWF_HIDEFILENAMES
            /// The view should not display file names.
            /// </summary>
            HideFilenames = 0x00020000,
            /// <summary>
            /// FWF_CHECKSELECT
            /// Turns on the check mode for the view.
            /// </summary>
            CheckSelect = 0x00040000,
            /// <summary>
            /// FWF_NOENUMREFRESH
            /// Windows Vista and later.
            /// Do not re-enumerate the view (or drop the current contents of the view) when the view is refreshed.
            /// </summary>
            NoEnumRefresh = 0x00080000,
            /// <summary>
            /// FWF_NOGROUPING
            /// Windows Vista and later.
            /// Do not allow grouping in the view
            /// </summary>
            NoGrouping = 0x00100000,
            /// <summary>
            /// FWF_FULLROWSELECT
            /// Windows Vista and later.
            /// When an item is selected, the item and all its sub-items are highlighted.
            /// </summary>
            FullRowSelect = 0x00200000,
            /// <summary>
            /// FWF_NOFILTERS
            /// Windows Vista and later.
            /// Do not display filters in the view.
            /// </summary>
            NoFilters = 0x00400000,
            /// <summary>
            /// FWF_NOCOLUMNHEADER
            /// Windows Vista and later.
            /// Do not display a column header in the view in any view mode.
            /// </summary>
            NoColumnHeaders = 0x00800000,
            /// <summary>
            /// FWF_NOHEADERINALLVIEWS
            /// Windows Vista and later.
            /// Only show the column header in details view mode.
            /// </summary>
            NoHeaderInAllViews = 0x01000000,
            /// <summary>
            /// FWF_EXTENDEDTILES
            /// Windows Vista and later.
            /// When the view is in "tile view mode" the layout of a single item should be extended to the width of the view.
            /// </summary>
            ExtendedTiles = 0x02000000,
            /// <summary>
            /// FWF_TRICHECKSELECT
            /// Windows Vista and later.
            /// Not supported.
            /// </summary>
            TriCheckSelect = 0x04000000,
            /// <summary>
            /// FWF_AUTOCHECKSELECT
            /// Windows Vista and later.
            /// Items can be selected using checkboxes.
            /// </summary>
            AutoCheckSelect = 0x08000000,
            /// <summary>
            /// FWF_NOBROWSERVIEWSTATE
            /// Windows Vista and later.
            /// The view should not save view state in the browser.
            /// </summary>
            NoBrowserViewState = 0x10000000,
            /// <summary>
            /// FWF_SUBSETGROUPS
            /// Windows Vista and later.
            /// The view should list the number of items displayed in each group. To be used with IFolderView2::SetGroupSubsetCount.
            /// </summary>
            SubsetGroups = 0x20000000,
            /// <summary>
            /// FWF_USESEARCHFOLDER
            /// Windows Vista and later.
            /// Use the search folder for stacking and searching.
            /// </summary>
            UseSearchFolders = 0x40000000,
            /// <summary>
            /// FWF_ALLOWRTLREADING
            /// Windows Vista and later.
            /// Ensure right-to-left reading layout in a right-to-left system. Without this flag, the view displays
            /// strings from left-to-right both on systems set to left-to-right and right-to-left reading layout,
            /// which ensures that file names display correctly.
            /// </summary>
            AllowRightToLeftReading = 0x80000000,
        };

        #endregion Constants ==================================================================================================

        #region Structures ====================================================================================================

        /// <summary>
        /// Contains folder view information.
        /// 
        /// <seealso href="https://docs.microsoft.com/en-us/windows/desktop/api/shobjidl_core/ns-shobjidl_core-foldersettings"/>
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct FolderSettings
        {
            public FolderViewMode ViewMode;
            public FolderFlags Flags;

            public FolderSettings(FolderViewMode ViewMode = FolderViewMode.Auto, FolderFlags Flags = FolderFlags.AutoArrange)
            {
                this.ViewMode = ViewMode;
                this.Flags = Flags;
            }
        };

        #endregion Structures =================================================================================================


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


        [DllImport("shell32.dll", SetLastError = true, CharSet = CharSet.Unicode, PreserveSig = false)]
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
            out Shell32.IShellFolder ppshf);

        [ObsoleteAttribute("SHGetMalloc interface is deprecated. Use CoTaskMemAlloc instead!")]
        [DllImport("shell32.dll")]
        public static extern WinError.HResult SHGetMalloc(
            [MarshalAs(UnmanagedType.IUnknown)] out object hObject);

        /// <summary>
        /// Sets the specified object's site by calling its IObjectWithSite::SetSite method.
        /// </summary>
        /// <remarks>
        /// This function calls the specified object's IUnknown::QueryInterface method to obtain a pointer to the object's
        /// IObjectWithSite interface.
        /// If successful, the function calls IObjectWithSite::SetSite to set or change the site.
        /// </remarks>
        /// <param name="punk">A pointer to the IUnknown interface of the object whose site is to be changed.</param>
        /// <param name="punkSite">A pointer to the IUnknown interface of the new site.</param>
        /// <returns>Returns S_OK if the site was successfully set, or a COM error code otherwise.</returns>
        [DllImport("shlwapi.dll", SetLastError = true)]
        public static extern WinError.HResult IUnknown_SetSite(
            [In, MarshalAs(UnmanagedType.IUnknown)] object punk,
            [In, MarshalAs(UnmanagedType.IUnknown)] object punkSite);

        /// <summary>
        /// [This function is available through Windows XP and Windows Server 2003. It might be altered or unavailable in subsequent versions of Windows.]
        /// 
        /// Establishes or terminates a connection between a client's sink and a connection point container.
        /// </summary>
        /// <param name="punk">A pointer to the IUnknown interface of the object to be connected to the connection point container.
        /// If you set fConnect to FALSE to indicate that you are disconnecting the object, this parameter is ignored and can be set to NULL.</param>
        /// <param name="riidEvent">The IID of the interface on the connection point container whose connection point object is being requested.</param>
        /// <param name="fConnect">TRUE if a connection is being established; FALSE if a connection is being broken.</param>
        /// <param name="punkTarget">A pointer to the connection point container's IUnknown interface.</param>
        /// <param name="pdwCookie">A connection token. If you set fConnect to TRUE to make a new connection, this parameter receives a token that uniquely
        /// identifies the connection. If you set fConnect to FALSE to break a connection, this parameter must point to the token that you received when
        /// you called ConnectToConnectionPoint to establish the connection.</param>
        /// <param name="ppcpOut">A pointer to the connection point container's IConnectionPoint interface, if the operation was successful. The
        /// calling application must release this pointer when it is no longer needed. If the request is unsuccessful, the pointer receives NULL.
        /// This parameter is optional and can be NULL.</param>
        /// <returns>If this function succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [DllImport("shlwapi.dll", SetLastError = true)]
        internal static extern WinError.HResult ConnectToConnectionPoint(
            [In, MarshalAs(UnmanagedType.IUnknown)] object punk,
            ref Guid riidEvent,
            [MarshalAs(UnmanagedType.Bool)] bool fConnect,
            [In, MarshalAs(UnmanagedType.IUnknown)] object punkTarget,
            ref uint pdwCookie,
            ref IntPtr ppcpOut);

    }
}
