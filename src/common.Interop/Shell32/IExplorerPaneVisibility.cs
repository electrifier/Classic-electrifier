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
        /// <summary>
        /// GUIDs that uniquely identify a Windows Explorer pane.
        /// 
        /// <seealso href="https://docs.microsoft.com/en-us/windows/desktop/api/shobjidl_core/nf-shobjidl_core-iexplorerpanevisibility-getpanestate"/>
        /// </summary>
        public static class RefExplorerPane
        {
            /// <summary>
            /// EP_NavPane
            /// 
            /// The pane on the left side of the Windows Explorer window that hosts the folders tree and Favorites. 
            /// </summary>
            public const string NavPane = "cb316b22-25f7-42b8-8a09-540d23a43c2f";

            /// <summary>
            /// EP_Commands
            /// 
            /// Commands module along the top of the Windows Explorer window.
            /// </summary>
            public const string Commands = "d9745868-ca5f-4a76-91cd-f5a129fbb076";

            /// <summary>
            /// EP_Commands_Organize
            /// 
            /// Organize menu within the commands module. 
            /// </summary>
            public const string Commands_Organize = "72e81700-e3ec-4660-bf24-3c3b7b648806";

            /// <summary>
            /// EP_Commands_View
            /// 
            /// View menu within the commands module.
            /// </summary>
            public const string Commands_View = "21f7c32d-eeaa-439b-bb51-37b96fd6a943";

            /// <summary>
            /// EP_DetailsPane
            /// 
            /// Pane showing metadata along the bottom of the Windows Explorer window.
            /// </summary>
            public const string DetailsPane = "43abf98b-89b8-472d-b9ce-e69b8229f019";

            /// <summary>
            /// EP_PreviewPane
            /// 
            /// Pane on the right of the Windows Explorer window that shows a large reading preview of the file.
            /// </summary>
            public const string PreviewPane = "893c63d1-45c8-4d17-be19-223be71be365";

            /// <summary>
            /// EP_QueryPane
            /// 
            /// Quick filter buttons to aid in a search.
            /// </summary>
            public const string QueryPane = "65bcde4f-4f07-4f27-83a7-1afca4df7ddd";

            /// <summary>
            /// EP_AdvQueryPane
            /// 
            /// Additional fields and options to aid in a search.
            /// </summary>
            public const string AdvQueryPane = "b4e9db8b-34ba-4c39-b5cc-16a1bd2c411c";

            /// <summary>
            /// EP_StatusBar
            /// 
            /// [Introduced in Windows 8]: A status bar that indicates the progress of some process, such as copying or downloading.
            /// </summary>
            public const string StatusBar = "65fe56ce-5cfe-4bc4-ad8a-7ae3fe7e8f7c";

            /// <summary>
            /// EP_Ribbon
            /// 
            /// [Introduced in Windows 8]: The ribbon, which is the control that replaced menus and toolbars at the top of many Microsoft applications.
            /// </summary>
            public const string Ribbon = "d27524a8-c9f2-4834-a106-df8889fd4f37";
        }



        /// <summary>
        /// Indicate flags used by <see cref="IExplorerPaneVisibility.GetPaneState(ref Guid, out ExplorerPaneState)"/>
        /// to get the current state of the given Windows Explorer pane.
        /// </summary>
        public enum ExplorerPaneState
        {
            /// <summary>
            /// EPS_DONTCARE
            /// 
            /// Do not make any modifications to the pane.
            /// </summary>
            DoNotCare = 0x00000000,
            /// <summary>
            /// EPS_DEFAULT_ON
            /// 
            /// Set the default state of the pane to "on", but respect any user-modified persisted state.
            /// </summary>
            DefaultOn = 0x00000001,
            /// <summary>
            /// EPS_DEFAULT_OFF
            /// 
            /// Set the default state of the pane to "off".
            /// </summary>
            DefaultOff = 0x00000002,
            /// <summary>
            /// EPS_STATEMASK
            /// 
            /// Unused.
            /// </summary>
            StateMask = 0x0000ffff,
            /// <summary>
            /// EPS_INITIALSTATE
            /// 
            /// Ignore any persisted state from the user, but the user can still modify the state.
            /// </summary>
            InitialState = 0x00010000,
            /// <summary>
            /// EPS_FORCE
            /// 
            /// Users cannot modify the state, that is, they do not have the ability to show or hide the given pane.
            /// This option implies EPS_INITIALSTATE.
            /// </summary>
            Force = 0x00020000
        }

        /// <summary>
        /// Used in Windows Explorer by an <see cref="IShellFolder"/> implementation to give suggestions to the view about what panes
        /// are visible. Additionally, an <see cref="IExplorerBrowser"/> host can use this interface to provide information about pane
        /// visibility. The host should implement QueryService with SID_ExplorerPaneVisibility as the service ID.
        /// The host must be in the site chain.
        ///
        /// The IExplorerPaneVisibility implementation is retrieved from the Shell folder. The Shell folder, in turn, is retrieved
        /// from the view. A namespace extension can elect to provide a custom view (IShellView) rather than using the system folder
        /// view object (DefView). In that case, the IShellView implementation must include an implementation of IFolderView::GetFolder
        /// to return the IExplorerPaneVisibility object.
        ///
        /// A namespace extension can provide a custom view by implementing IShellView itself rather than using the system folder
        /// view object (DefView). In that case, the IShellView implementation must include an implementation of
        /// IFolderView::GetFolder to make use of <see cref="IExplorerPaneVisibility"/>.
        /// 
        /// <seealso href="https://docs.microsoft.com/en-us/windows/desktop/api/shobjidl_core/nn-shobjidl_core-iexplorerpanevisibility"/>
        /// </summary>
        [ComImport, Guid("e07010ec-bc17-44c0-97b0-46c7c95b9edc"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IExplorerPaneVisibility
        {
            /// <summary>
            /// Gets the visibility state of the given Windows Explorer pane.
            /// </summary>
            /// <remarks>If the implementer does not care about the state of a given pane and therefore does not want to
            /// change it, then the implementer should return a success code for the method and EPS_DONTCARE for the peps
            /// parameter. If the method fails, it is treated as if EPS_DONTCARE was returned for the peps parameter.
            /// </remarks>
            /// <param name="explorerPane">A reference to a GUID that uniquely identifies a Windows Explorer pane.
            /// One of the <see cref="RefExplorerPane"/> constants as defined in Shlguid.h.</param>
            /// <param name="peps">When this method returns, contains the visibility state of the given Windows Explorer
            /// pane as one of the <see cref="ExplorerPaneState"/> constants.</param>
            /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
            [PreserveSig]
            WinError.HResult GetPaneState(
                ref Guid explorerPane,
                out ExplorerPaneState peps);
        };
    }
}
