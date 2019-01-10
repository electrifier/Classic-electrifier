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

using electrifier.Win32API;

namespace common.Interop
{
    public static partial class Shell32
    {
        /// <summary>
        /// These flags are used with IExplorerBrowser::GetOptions and IExplorerBrowser::SetOptions.
        /// 
        /// <seealso href="https://docs.microsoft.com/en-us/windows/desktop/api/shobjidl_core/ne-shobjidl_core-explorer_browser_options"/>
        /// </summary>
        [Flags]
        public enum ExplorerBrowserOptions
        {
            /// <summary>
            /// Do not navigate further than the initial navigation.
            /// </summary>
            NavigateOnce = 0x00000001,
            /// <summary>
            /// Use the following standard panes:
            ///     Commands Module pane, 
            ///     Navigation pane, 
            ///     Details pane, 
            ///     and Preview pane. 
            /// 
            /// An implementer of IExplorerPaneVisibility can modify the components of the Commands Module that are shown. 
            /// 
            /// For more information see, IExplorerPaneVisibility::GetPaneState. If EBO_SHOWFRAMES is not set, Explorer browser uses a single view object.
            /// </summary>
            ShowFrames = 0x00000002,
            /// <summary>
            /// Always navigate, even if you are attempting to navigate to the current folder.
            /// </summary>
            AlwaysNavigate = 0x00000004,
            /// <summary>
            /// Do not update the travel log.
            /// </summary>
            NoTravelLog = 0x00000008,
            /// <summary>
            /// Do not use a wrapper window. This flag is used with legacy clients that need the browser parented directly on themselves.
            /// </summary>
            NoWrapperWindow = 0x00000010,
            /// <summary>
            /// Show WebView for sharepoint sites.
            /// </summary>
            HtmlSharepointView = 0x00000020,
            /// <summary>
            /// [Introduced in Windows Vista.]
            /// Do not draw a border around the browser window.
            /// </summary>
            NoBorder = 0x00000040,
            /// <summary>
            /// [Introduced in Windows Vista.]
            /// Do not persist the view state.
            /// </summary>
            NoPersistViewState = 0x00000080,
        }

        /// <summary>
        /// IExplorerBrowser is a browser object that can be either navigated or that can host a view of a data object. As a
        /// full-featured browser object, it also supports an automatic travel log.
        /// 
        /// The Shell provides a default implementation of IExplorerBrowser as CLSID_ExplorerBrowser. Typically, a developer
        /// does not need to provide a custom implemention of this interface.
        /// 
        /// The Windows Software Development Kit(SDK) provides full samples that demonstrate the use of and interaction with
        /// IExplorerBrowser. Download the Explorer Browser Search Sample and the Explorer Browser Custom Contents Sample.
        /// 
        /// <seealso href="https://docs.microsoft.com/en-us/windows/desktop/api/shobjidl_core/nn-shobjidl_core-iexplorerbrowser"/>
        /// </summary>
        [ComImport, Guid("dfd3b6b5-c10c-4be9-85f6-a66969f402f6"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IExplorerBrowser
        {
            /// <summary>
            /// Prepares the browser to be navigated.
            /// </summary>
            /// <param name="hwndParent">A handle to the owner window or control.</param>
            /// <param name="prc">A pointer to a RECT containing the coordinates of the bounding rectangle 
            /// the browser will occupy. The coordinates are relative to hwndParent. If this parameter is NULL,
            /// then method IExplorerBrowser::SetRect should subsequently be called.</param>
            /// <param name="pfs">A pointer to a FOLDERSETTINGS structure that determines how the folder will be
            /// displayed in the view. If this parameter is NULL, then method IExplorerBrowser::SetFolderSettings
            /// should be called, otherwise, the default view settings for the folder are used.</param>
            /// <returns></returns>
            void Initialize(
                IntPtr hwndParent,
                in electrifier.Win32API.RECT prc,
                in ShellAPI.FOLDERSETTINGS pfs);

            /// <summary>
            /// Destroys the browser.
            /// </summary>
            /// <returns></returns>
            void Destroy();

            /// <summary>
            /// Sets the size and position of the view windows created by the browser.
            /// </summary>
            /// <param name="phdwp">A pointer to a DeferWindowPos handle. This paramater can be NULL.</param>
            /// <param name="rcBrowser">The coordinates that the browser will occupy.</param>
            /// <returns></returns>
            void SetRect(
                IntPtr phdwp,
                electrifier.Win32API.RECT rcBrowser);

            /// <summary>
            /// Sets the name of the property bag.
            /// </summary>
            /// <param name="pszPropertyBag">A pointer to a constant, null-terminated, Unicode string that contains
            /// the name of the property bag. View state information that is specific to the application of the 
            /// client is stored (persisted) using this name.</param>
            /// <returns></returns>
            void SetPropertyBag(
                [In, MarshalAs(UnmanagedType.LPWStr)] string pszPropertyBag);

            /// <summary>
            /// Sets the default empty text.
            /// </summary>
            /// <param name="pszEmptyText">A pointer to a constant, null-terminated, Unicode string that contains 
            /// the empty text.</param>
            /// <returns></returns>
            void SetEmptyText(
                [In, MarshalAs(UnmanagedType.LPWStr)] string pszEmptyText);

            /// <summary>
            /// Sets the folder settings for the current view.
            /// </summary>
            /// <param name="pfs">A pointer to a FOLDERSETTINGS structure that contains the folder settings 
            /// to be applied.</param>
            /// <returns></returns>
            [PreserveSig]
            WinError.HResult SetFolderSettings(
                ShellAPI.FOLDERSETTINGS pfs);

            /// <summary>
            /// Initiates a connection with IExplorerBrowser for event callbacks.
            /// </summary>
            /// <param name="psbe">A pointer to the IExplorerBrowserEvents interface of the object to be 
            /// advised of IExplorerBrowser events</param>
            /// <param name="pdwCookie">When this method returns, contains a token that uniquely identifies 
            /// the event listener. This allows several event listeners to be subscribed at a time.</param>
            /// <returns></returns>
            [PreserveSig]
            WinError.HResult Advise(
                [In] IExplorerBrowserEvents psbe,
                out uint pdwCookie);

            /// <summary>
            /// Terminates an advisory connection.
            /// </summary>
            /// <param name="dwCookie">A connection token previously returned from IExplorerBrowser::Advise.
            /// Identifies the connection to be terminated.</param>
            /// <returns></returns>
            [PreserveSig]
            WinError.HResult Unadvise(
                [In] uint dwCookie);

            /// <summary>
            /// Sets the current browser options.
            /// </summary>
            /// <param name="dwFlag">One or more EXPLORER_BROWSER_OPTIONS flags to be set.</param>
            /// <returns></returns>
            void SetOptions(
                [In] ExplorerBrowserOptions dwFlag);

            /// <summary>
            /// Gets the current browser options.
            /// </summary>
            /// <param name="pdwFlag">When this method returns, contains the current EXPLORER_BROWSER_OPTIONS 
            /// for the browser.</param>
            /// <returns></returns>
            void GetOptions(
                out ExplorerBrowserOptions pdwFlag);

            /// <summary>
            /// Browses to a pointer to an item identifier list (PIDL)
            /// </summary>
            /// <param name="pidl">A pointer to a const ITEMIDLIST (item identifier list) that specifies an object's 
            /// location as the destination to navigate to. This parameter can be NULL.</param>
            /// <param name="uFlags">A flag that specifies the category of the pidl. This affects how 
            /// navigation is accomplished</param>
            /// <returns></returns>
            void BrowseToIDList(
                [In] IntPtr pidl,
                [In] uint uFlags);

            /// <summary>
            /// Browse to an object
            /// </summary>
            /// <param name="punk">A pointer to an object to browse to. If the object cannot be browsed, 
            /// an error value is returned.</param>
            /// <param name="uFlags">A flag that specifies the category of the pidl. This affects how 
            /// navigation is accomplished. </param>
            /// <returns></returns>
            [PreserveSig]
            WinError.HResult BrowseToObject(
                [In, MarshalAs(UnmanagedType.IUnknown)] object punk,
                [In] uint uFlags);

            /// <summary>
            /// Creates a results folder and fills it with items.
            /// </summary>
            /// <param name="punk">An interface pointer on the source object that will fill the IResultsFolder</param>
            /// <param name="dwFlags">One of the EXPLORER_BROWSER_FILL_FLAGS</param>
            /// <returns></returns>
            void FillFromObject(
                [In, MarshalAs(UnmanagedType.IUnknown)] object punk,
                int dwFlags);

            /// <summary>
            /// Removes all items from the results folder.
            /// </summary>
            /// <returns></returns>
            void RemoveAll();

            /// <summary>
            /// Gets an interface for the current view of the browser.
            /// </summary>
            /// <param name="riid">A reference to the desired interface ID.</param>
            /// <param name="ppv">When this method returns, contains the interface pointer requested in riid. 
            /// This will typically be IShellView or IShellView2. </param>
            /// <returns></returns>
            [PreserveSig]
            WinError.HResult GetCurrentView(
                in Guid riid,
                out IntPtr ppv);
        }
    }
}
