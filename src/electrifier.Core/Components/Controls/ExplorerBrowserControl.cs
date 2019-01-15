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
        // , IMessageFilter // TODO!
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



        #endregion Fields =====================================================================================================

        #region Properties ====================================================================================================

        #endregion Properties =================================================================================================

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

        #endregion Published Events ===========================================================================================

        public ExplorerBrowserControl()
        {
            this.InitializeComponent();



        }

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
                    {   // TODO
                        this.NavigateTo(new ShellFolder(Shell32.KNOWNFOLDERID.FOLDERID_UsersFiles
                            ));
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

        protected void SetSite(Shell32.IServiceProvider sp) => (this.explorerBrowser as Shell32.IObjectWithSite)?.SetSite(sp);

        protected override void OnSizeChanged(EventArgs e)
        {
            this.explorerBrowser?.SetRect(IntPtr.Zero, this.ClientRectangle);

            base.OnSizeChanged(e);
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

            //if (this.explorerBrowser == null)
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
            else if (guidService.Equals(typeof(Shell32.ICommDlgBrowser3).GUID))
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

        #endregion Implemented Interface: IServiceProvider ====================================================================

        #region Implemented Interface: IExplorerPaneVisibility ================================================================

        public HRESULT GetPaneState(in Guid ep, out Shell32.EXPLORERPANESTATE peps)
        {
            peps = Shell32.EXPLORERPANESTATE.EPS_DONTCARE;

            return HRESULT.S_OK;
        }

        #endregion Implemented Interface: IExplorerPaneVisibility =============================================================

        #region Implemented Interface: IExplorerBrowserEvents =================================================================

        public HRESULT OnNavigationPending(IntPtr pidlFolder)
        {
            return HRESULT.S_OK;
        }

        public HRESULT OnViewCreated(Shell32.IShellView psv)
        {
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

        #endregion Implemented Interface: IExplorerBrowserEvents ==============================================================

        #region Implemented Interface: ICommDlgBrowser3 =======================================================================

        public HRESULT OnDefaultCommand(Shell32.IShellView ppshv)
        {
            return HRESULT.S_OK;
        }

        public HRESULT OnStateChange(Shell32.IShellView ppshv, Shell32.CDBOSC uChange)
        {
            return HRESULT.S_OK;
        }

        public HRESULT IncludeObject(Shell32.IShellView ppshv, IntPtr pidl)
        {
            return HRESULT.S_OK;
        }

        public HRESULT GetDefaultMenuText(Shell32.IShellView ppshv, StringBuilder pszText, int cchMax)
        {
            return HRESULT.S_OK;
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

        #endregion Implemented Interface: ICommDlgBrowser3 ====================================================================
    }
}
