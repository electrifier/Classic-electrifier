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

using Vanara.PInvoke;

namespace electrifier.Core.Components.Controls
{
    public partial class ExplorerBrowserControl
    {
        /// <summary>
        /// 
        /// ExplorerBrowserControl.ViewEvents (Nested class)
        /// 
        /// This inner class acts as the connection between IShellView hosted by ExplorerBrowser
        /// and ExplorerBrowserControl as its container.
        /// 
        /// It is responsible for forwarding events when IShellView is navigating.
        /// 
        /// </summary>
        [ComVisible(true)]
        [ClassInterface(ClassInterfaceType.AutoDual)]
        public class ViewEvents : IDisposable
        {
            #region Fields ========================================================================================================

            private readonly ExplorerBrowserControl container;

            private object viewDispatch;
            private uint viewConnectionPointCookie;

            /// <summary>
            /// View event dispatch IDs. Pulled from from reference implementation.
            /// 
            /// As far as i can see these Windows Shell-Dispatch IDs are undocumented.
            /// 
            /// <seealso href="https://github.com/aybe/Windows-API-Code-Pack-1.1/blob/master/source/WindowsAPICodePack/Shell/ExplorerBrowser/ExplorerBrowserViewEvents.cs"/>
            /// </summary>
            private static class ExplorerBrowserViewDispatchIds
            {
                internal const int ContentsChanged = 207;
                internal const int ViewEnumerationComplete = 201;
                internal const int SelectionChanged = 200;
                internal const int SelectedItemChanged = 220;
            }

            protected static Guid IID_IDispatch = new Guid("00020400-0000-0000-c000-000000000046");         // TODO
            protected static Guid IID_DShellFolderViewEvents = new Guid("62112aa2-ebe4-11cf-a5fb-0020afe7292d");    // TODO

            #endregion Fields =====================================================================================================

            internal ViewEvents(ExplorerBrowserControl container)
            {
                this.container = container;
            }

            internal void ConnectShellView(Shell32.IShellView psv)
            {
                this.DisconnectShellView();

                this.viewDispatch = psv.GetItemObject(Shell32.SVGIO.SVGIO_BACKGROUND, ViewEvents.IID_IDispatch);

                HRESULT hResult = ShlwApi.ConnectToConnectionPoint(this, ViewEvents.IID_DShellFolderViewEvents, true, this.viewDispatch,
                    ref this.viewConnectionPointCookie, out System.Runtime.InteropServices.ComTypes.IConnectionPoint ppcpOut);

                if (hResult.Failed)
                {
                    Marshal.ReleaseComObject(this.viewDispatch);
                    this.viewDispatch = null;

                    throw new COMException("ExplorerBrowserControl.ViewEvents.ConnectShellView: Shell32.ConnectToConnectionPoint() failed.", (int)hResult);
                }
            }

            internal void DisconnectShellView()
            {
                if (this.viewDispatch != null)
                {
                    ShlwApi.ConnectToConnectionPoint(this, ViewEvents.IID_DShellFolderViewEvents, false, this.viewDispatch,
                        ref this.viewConnectionPointCookie, out System.Runtime.InteropServices.ComTypes.IConnectionPoint ppcpOut);
                    this.viewConnectionPointCookie = 0;

                    Marshal.ReleaseComObject(this.viewDispatch);
                    this.viewDispatch = null;
                }
            }

            #region IDispatch event handlers ==================================================================================

            /// <summary>
            /// Items collection in ShellView has changed.
            /// 
            /// Note: Needs to be public to be accessible via AutoDual reflection
            /// </summary>
            [DispId(ExplorerBrowserViewDispatchIds.ContentsChanged)]
            public void ViewContentsChanged()
            {
                this.container.ContentsChanged?.Invoke(this, EventArgs.Empty);
            }

            /// <summary>
            /// ShellView has finished enumerating files.
            /// 
            /// Note: Needs to be public to be accessible via AutoDual reflection
            /// </summary>
            [DispId(ExplorerBrowserViewDispatchIds.ViewEnumerationComplete)]
            public void ViewEnumerationComplete()
            {
                this.container.ViewEnumerationComplete?.Invoke(this, EventArgs.Empty);
            }

            /// <summary>
            /// SelectedItems collection in ShellView has changed.
            /// 
            /// Note: Needs to be public to be accessible via AutoDual reflection
            /// </summary>
            [DispId(ExplorerBrowserViewDispatchIds.SelectionChanged)]
            public void ViewSelectionChanged()
            {
                this.container.SelectionChanged?.Invoke(this, EventArgs.Empty);
            }

            /// <summary>
            /// The selected item in the ShellView has changed, e.g. a rename.
            /// 
            /// Note: Needs to be public to be accessible via AutoDual reflection
            /// </summary>
            [DispId(ExplorerBrowserViewDispatchIds.SelectedItemChanged)]
            public void ViewSelectedItemChanged()
            {
                this.container.SelectedItemChanged?.Invoke(this, EventArgs.Empty);
            }

            #endregion IDispatch event handlers ===============================================================================

            #region IDisposable Support
            private bool disposedValue = false; // To detect redundant calls

            protected virtual void Dispose(bool disposing)
            {
                if (!this.disposedValue)
                {
                    if (disposing)
                    {
                        this.DisconnectShellView();
                    }

                    this.viewConnectionPointCookie = 0;
                    this.viewDispatch = null;

                    this.disposedValue = true;
                }
            }

            ~ViewEvents()
            {
                // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
                this.Dispose(false);
            }

            // This code added to correctly implement the disposable pattern.
            public void Dispose()
            {
                // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
                this.Dispose(true);
                GC.SuppressFinalize(this);
            }
            #endregion
        }
    }
}
