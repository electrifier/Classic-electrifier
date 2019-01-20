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
using System.Text;
using System.Windows.Forms;

using Vanara.PInvoke;
using Vanara.Windows.Shell;

namespace electrifier.Core.Components.Controls
{
    public partial class ExplorerBrowserControl
        : UserControl
        , Shell32.IServiceProvider
        , Shell32.IExplorerPaneVisibility
        , Shell32.IExplorerBrowserEvents
        , Shell32.ICommDlgBrowser3
        , IMessageFilter
    {
        #region Fields ========================================================================================================

        protected Shell32.IExplorerBrowser explorerBrowser = null;

        /// <summary>
        /// Used for this.explorerBrowser.Advise() and this.explorerBrowser.Unadvise() calls
        /// </summary>
        private uint adviseEventsCookie;

        /// <summary>
        /// Nested class ViewEvents acts as the connection to IShellView of ExplorerBrowser
        /// </summary>
        protected ExplorerBrowserControl.ViewEvents ebViewEvents;

        private readonly string propertyBagName = "electrifier.ExplorerBrowserControl";


        private const int HRESULT_CANCELLED = unchecked((int)0x800704C7);
        private const int HRESULT_RESOURCE_IN_USE = unchecked((int)0x800700AA);



        #endregion ============================================================================================================

        #region Properties ====================================================================================================

        #endregion ============================================================================================================

        #region Published Events ==============================================================================================

        /// <summary>
        /// Fires when the Items collection in ShellView has changed.
        /// </summary>
        public event EventHandler ContentsChanged;

        /// <summary>
        /// Fires when ShellView has finished enumerating files.
        /// </summary>
        public event EventHandler ViewEnumerationComplete;

        /// <summary>
        /// Fires when SelectedItems collection in ShellView has changed.
        /// </summary>
        public event EventHandler SelectionChanged;

        /// <summary>
        /// Fires when the selected item in the ShellView has changed, e.g. a rename.
        /// </summary>
        public event EventHandler SelectedItemChanged;

        #endregion ============================================================================================================

        public ExplorerBrowserControl()
        {
            this.InitializeComponent();



        }

        /// <summary>
        /// Clears the Explorer Browser of existing content, fills it with content from the specified container, and adds a new point to the
        /// Travel Log.
        /// </summary>
        /// <param name="shellItem">The shell container to navigate to.</param>
        /// <param name="category">The category of the <paramref name="shellItem"/>.</param>
        public void NavigateTo(ShellItem shellItem)
        {
            if (shellItem == null)
                throw new ArgumentNullException(nameof(shellItem));

            //if (this.explorerBrowser == null)     // TODO!
            //{
            //    antecreationNavigationTarget = new Tuple<ShellItem, ExplorerBrowserNavigationItemCategory>(shellItem, category);
            //}
            //else
            {
                try
                {
                    this.explorerBrowser.BrowseToObject(shellItem.IShellItem, Shell32.SBSP.SBSP_ABSOLUTE);
                }
                catch (COMException e)
                {
                    if (e.ErrorCode == HRESULT_RESOURCE_IN_USE || e.ErrorCode == HRESULT_CANCELLED)
                    {
                        //OnNavigationFailed(new NavigationFailedEventArgs { FailedLocation = shellItem });
                    }
                    else
                    {
                        throw new ArgumentException("Unable to browse to this shell item.", nameof(shellItem), e);
                    }
                }
                catch (Exception e)
                {
                    throw new ArgumentException("Unable to browse to this shell item.", nameof(shellItem), e);
                }
            }
        }

        protected void SetSite(Shell32.IServiceProvider sp) => (this.explorerBrowser as Shell32.IObjectWithSite)?.SetSite(sp);

        protected override void OnCreateControl()
        {
            base.OnCreateControl();

            if (false == this.DesignMode)
            {
                this.explorerBrowser = new Vanara.PInvoke.Shell32.IExplorerBrowser();

                if (this.explorerBrowser is null)
                    throw new COMException("Could not instantiate ExplorerBrowser!");

                // Set site to get notified of IExplorerPaneVisibility and ICommDlgBrowser events
                this.SetSite(this);

                // Advise of IExplorerBrowserEvents
                this.explorerBrowser.Advise(this, out this.adviseEventsCookie);

                // Create connection point to ExplorerBrowser's ShellView events
                this.ebViewEvents = new ExplorerBrowserControl.ViewEvents(this);

                this.explorerBrowser.Initialize(this.Handle, this.ClientRectangle,
                    new Shell32.FOLDERSETTINGS(Shell32.FOLDERVIEWMODE.FVM_AUTO, Shell32.FOLDERFLAGS.FWF_NONE));

                // Force an initial show frames so that IExplorerPaneVisibility works the first time it is set.
                // This also enables the control panel to be browsed to. If it is not set, then navigating to 
                // the control panel succeeds, but no items are visible in the view.
                this.explorerBrowser.SetOptions(Shell32.EXPLORER_BROWSER_OPTIONS.EBO_SHOWFRAMES);

                this.explorerBrowser.SetPropertyBag(this.propertyBagName);

                // Do initial navigation on background thread
                this.BeginInvoke(new MethodInvoker(
                    delegate
                    {
                        // TODO: On Windows 10, navigate to Quick Access by default...
                        // <seealso href="https://www.tenforums.com/tutorials/3123-clsid-key-guid-shortcuts-list-windows-10-a.html">
                        this.NavigateTo(new ShellFolder(Shell32.KNOWNFOLDERID.FOLDERID_UsersFiles));
                    }));
            }
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            if (!(this.explorerBrowser is null))
            {
                this.ebViewEvents.DisconnectShellView();

                this.explorerBrowser.Unadvise(this.adviseEventsCookie);
                this.SetSite(null);

                this.explorerBrowser.Destroy();

                Marshal.ReleaseComObject(this.explorerBrowser);
                this.explorerBrowser = null;
            }

            base.OnHandleDestroyed(e);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            this.explorerBrowser?.SetRect(IntPtr.Zero, this.ClientRectangle);

            base.OnSizeChanged(e);
        }

        /// <summary>
        /// Ask ExplorerBrowser to translate Window-Messages for proper keyboard input handling
        /// </summary>
        /// <param name="msg">A reference to the Window Message</param>
        /// <returns>True if successful, otherwise false</returns>
        bool IMessageFilter.PreFilterMessage(ref Message msg)
        {
            return ((this.explorerBrowser as Shell32.IInputObject)?.TranslateAcceleratorIO(
                new Vanara.PInvoke.MSG {
                    message = (uint)msg.Msg,
                    hwnd = msg.HWnd,
                    wParam = msg.WParam,
                    lParam = msg.LParam }).Succeeded ?? false);
        }

        #region Implemented Interface: IServiceProvider =======================================================================

        public HRESULT QueryService(ref Guid guidService, ref Guid riid, out IntPtr ppvObject)
        {
            HRESULT hr = HRESULT.E_NOINTERFACE;
            ppvObject = default;

            if (guidService.Equals(typeof(Shell32.IExplorerPaneVisibility).GUID))
            {
                ppvObject = Marshal.GetComInterfaceForObject(this, typeof(Shell32.IExplorerPaneVisibility));

                return HRESULT.S_OK;
            }
            else if (guidService.Equals(typeof(Shell32.ICommDlgBrowser).GUID))
            {
                // ICommDlgBrowserX is exposed to host the Shell Browser and control its behaviour
                //
                // TODO: According comments in ExplorerBrowser.QueryService default implementation, marshalling to
                // ICommDlgBrowser2 fails and causes exceptions.
                //
                // <see href="https://github.com/aybe/Windows-API-Code-Pack-1.1/blob/master/source/WindowsAPICodePack/Shell/ExplorerBrowser/ExplorerBrowser.cs"/>
                if (riid.Equals(typeof(Shell32.ICommDlgBrowser).GUID) || riid.Equals(typeof(Shell32.ICommDlgBrowser3).GUID))
                {
                    ppvObject = Marshal.GetComInterfaceForObject(this, typeof(Shell32.ICommDlgBrowser3));

                    return HRESULT.S_OK;
                }
            }

            return hr;
        }

        #endregion ============================================================================================================

        #region Implemented Interface: IExplorerPaneVisibility ================================================================

        public HRESULT GetPaneState(in Guid ep, out Shell32.EXPLORERPANESTATE peps)
        {
            if (ep.Equals(Shell32.IExplorerPaneVisibilityConstants.EP_NavPane))
                peps = Shell32.EXPLORERPANESTATE.EPS_DEFAULT_ON;
            else
                peps = Shell32.EXPLORERPANESTATE.EPS_DEFAULT_OFF | Shell32.EXPLORERPANESTATE.EPS_FORCE;

            return HRESULT.S_OK;
        }

        #endregion ============================================================================================================

        #region Implemented Interface: IExplorerBrowserEvents =================================================================

        public HRESULT OnNavigationPending(IntPtr pidlFolder)
        {
            return HRESULT.S_OK;
        }

        public HRESULT OnViewCreated(Shell32.IShellView psv)
        {
            this.ebViewEvents.ConnectShellView(psv);

            return HRESULT.S_OK;
        }

        public HRESULT OnNavigationComplete(IntPtr pidlFolder)
        {
            return HRESULT.S_OK;
        }

        public HRESULT OnNavigationFailed(IntPtr pidlFolder)
        {
            return HRESULT.S_OK;
        }

        #endregion ============================================================================================================

        #region Implemented Interface: ICommDlgBrowser3 =======================================================================

        public HRESULT OnDefaultCommand(Shell32.IShellView ppshv)
        {
            return HRESULT.S_FALSE;
        }

        public HRESULT OnStateChange(Shell32.IShellView ppshv, Shell32.CDBOSC uChange)
        {
            if (uChange == Shell32.CDBOSC.CDBOSC_STATECHANGE)
            {
                //this.OnSelectionChanged(); // TODO:
            }

            return HRESULT.S_OK;
        }

        public HRESULT IncludeObject(Shell32.IShellView ppshv, IntPtr pidl)
        {
            // this.OnIncludeObject(); // TODO: When returning false, the item won't get displayed!
            return HRESULT.S_OK;
        }

        public HRESULT GetDefaultMenuText(Shell32.IShellView ppshv, StringBuilder pszText, int cchMax)
        {
            return HRESULT.S_FALSE;
        }

        public HRESULT GetViewFlags(out Shell32.CDB2GVF pdwFlags)
        {
            pdwFlags = Shell32.CDB2GVF.CDB2GVF_SHOWALLFILES;

            return HRESULT.S_OK;
        }

        public HRESULT Notify(Shell32.IShellView ppshv, Shell32.CDB2N dwNotifyType)
        {
            return HRESULT.S_OK;
        }

        public HRESULT GetCurrentFilter(StringBuilder pszFileSpec, int cchFileSpec)
        {
            return HRESULT.S_OK;
        }

        public HRESULT OnColumnClicked(Shell32.IShellView ppshv, int iColumn)
        {
            return HRESULT.S_OK;
        }

        public HRESULT OnPreViewCreated(Shell32.IShellView ppshv)
        {
            return HRESULT.S_OK;
        }

        #endregion ============================================================================================================



        #region Nested class ExplorerBrowserControl.ViewEvents ================================================================

        /// <summary>
        /// 
        /// ExplorerBrowserControl.ViewEvents (Nested class)
        /// 
        /// This nested  class acts as the connection between IShellView hosted by ExplorerBrowser
        /// and ExplorerBrowserControl as its container.
        /// 
        /// It is responsible for forwarding events when IShellView is navigating.
        /// 
        /// </summary>
        [ComVisible(true)]
        [ClassInterface(ClassInterfaceType.AutoDual)]
        public class ViewEvents : IDisposable
        {
            #region Fields ====================================================================================================

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

            #endregion Fields =================================================================================================

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

        #endregion ============================================================================================================
    }
}
