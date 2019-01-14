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

using common.Interop;


namespace electrifier.Core.Components.Controls
{
    public partial class ExplorerBrowserControl
        : UserControl
        , Shell32.IServiceProvider
        , Shell32.IExplorerPaneVisibility
        , Shell32.IExplorerBrowserEvents
        , Shell32.ICommDlgBrowser3
        //TODO: IMessageFilter
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
                this.explorerBrowser =
                    Shell32.CLSID.CoCreateInstance<Shell32.IExplorerBrowser>(Shell32.CLSID.ExplorerBrowser);

                if (this.explorerBrowser is null)
                    throw new COMException("Could not instantiate ExplorerBrowser!");

                // Set site to get notified of IExplorerPaneVisibility and ICommDlgBrowser events
                Shell32.IUnknown_SetSite(this.explorerBrowser, this);

                // Advise of IExplorerBrowserEvents
                this.explorerBrowser.Advise(this, out this.adviseEventsCookie);

                // Create connection point to ExplorerBrowser's ShellView events
                this.ebViewEvents = new ExplorerBrowserControl.ViewEvents(this);

                this.explorerBrowser.Initialize(this.Handle, this.ClientRectangle, new Shell32.FolderSettings());

                // Force an initial show frames so that IExplorerPaneVisibility works the first time it is set.
                // This also enables the control panel to be browsed to. If it is not set, then navigating to 
                // the control panel succeeds, but no items are visible in the view.
                this.explorerBrowser.SetOptions(Shell32.ExplorerBrowserOptions.ShowFrames);

                this.explorerBrowser.SetPropertyBag(this.propertyBagName);


                // Do initial navigation on background thread
                this.BeginInvoke(new MethodInvoker(
                    delegate
                    {
                        this.NavigateToFolder(@"S:\");
                    }));
            }
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            if (!(this.explorerBrowser is null))
            {
                this.ebViewEvents.DisconnectShellView();

                this.explorerBrowser.Unadvise(this.adviseEventsCookie);
                Shell32.IUnknown_SetSite(this.explorerBrowser, null);

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

        public void NavigateToFolder(in string folderPath)
        {
            Shell32.IShellItem shellItem;
            Guid guid = new Guid(Shell32.IID.IShellItem);

            shellItem = Shell32.SHCreateItemFromParsingName(folderPath, null, ref guid);

            WinError.HResult hr = this.explorerBrowser.BrowseToObject(shellItem, 0);
        }

        #region Implemented Interfaces ========================================================================================

        #region Implemented Interface: IServiceProvider =======================================================================

        public WinError.HResult QueryService(in Guid guidService, in Guid riid, out object ppvObject)
        {
            if (guidService.Equals(typeof(Shell32.IExplorerPaneVisibility).GUID))
            {
                // IExplorerPaneVisibility controls the visibility of explorer browser's panes
                ppvObject = Marshal.GetComInterfaceForObject(this, typeof(Shell32.IExplorerPaneVisibility));

                return WinError.HResult.S_OK;
            }
            else if (guidService.Equals(typeof(Shell32.IExplorerPaneVisibility).GUID))
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

                    return WinError.HResult.S_OK;
                }
            }

            ppvObject = null;
            return WinError.HResult.E_NoInterface;
        }

        #endregion Implemented Interface: IServiceProvider ====================================================================

        #region Implemented Interface: IExplorerPaneVisibility ================================================================

        public WinError.HResult GetPaneState(ref Guid explorerPane, out Shell32.ExplorerPaneState peps)
        {
            switch (explorerPane.ToString())
            {
                default:
                    peps = Shell32.ExplorerPaneState.DoNotCare;
                    break;
            }

            return WinError.HResult.S_OK;
        }

        #endregion Implemented Interface: IExplorerPaneVisibility =============================================================

        #region Implemented Interface: IExplorerBrowserEvents =================================================================

        public WinError.HResult OnNavigationPending(IntPtr pidlFolder)
        {
            return WinError.HResult.S_OK;
        }

        public WinError.HResult OnViewCreated(object psv)
        {
            this.ebViewEvents.ConnectShellView(psv as Shell32.IShellView);

            return WinError.HResult.S_OK;
        }

        public WinError.HResult OnNavigationComplete(IntPtr pidlFolder)
        {
            return WinError.HResult.S_OK;
        }

        public WinError.HResult OnNavigationFailed(IntPtr pidlFolder)
        {
            return WinError.HResult.S_OK;
        }

        #endregion Implemented Interface: IExplorerBrowserEvents ==============================================================

        #region Implemented Interface: ICommDlgBrowser3 =======================================================================

        public WinError.HResult OnDefaultCommand(IntPtr ppshv)
        {
            return WinError.HResult.S_OK;
        }

        public WinError.HResult OnStateChange(IntPtr ppshv, Shell32.CommDlgBrowserStateChange uChange)
        {
            return WinError.HResult.S_OK;
        }

        public WinError.HResult IncludeObject(IntPtr ppshv, IntPtr pidl)
        {
            return WinError.HResult.S_OK;
        }

        public WinError.HResult GetDefaultMenuText(Shell32.IShellView shellView, IntPtr buffer, int bufferMaxLength)
        {
            return WinError.HResult.S_OK;
        }

        public WinError.HResult GetViewFlags(out Shell32.CommDlgBrowserViewFlags pdwFlags)
        {
            pdwFlags = Shell32.CommDlgBrowserViewFlags.ShowAllFiles;

            return WinError.HResult.S_OK;
        }

        public WinError.HResult Notify(IntPtr pshv, Shell32.CommDlgBrowserNotifyType notifyType)
        {
            return WinError.HResult.S_OK;
        }

        public WinError.HResult GetCurrentFilter(StringBuilder pszFileSpec, int cchFileSpec)
        {
            return WinError.HResult.S_OK;
        }

        public WinError.HResult OnColumnClicked(Shell32.IShellView ppshv, int iColumn)
        {
            return WinError.HResult.S_OK;
        }

        public WinError.HResult OnPreViewCreated(Shell32.IShellView ppshv)
        {
            return WinError.HResult.S_OK;
        }

        #endregion Implemented Interface: ICommDlgBrowser3 ====================================================================

        #endregion Implemented Interfaces =====================================================================================

    }
}
