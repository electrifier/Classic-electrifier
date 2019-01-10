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
    {
        #region Fields ========================================================================================================

        protected Shell32.IExplorerBrowser explorerBrowser = null;

        /// <summary>
        /// Used for this.explorerBrowser.Advise() and this.explorerBrowser.Unadvise() calls
        /// </summary>
        private uint adviseEventsCookie;






        #endregion Fields =====================================================================================================

        #region Properties ====================================================================================================

        #endregion Properties =================================================================================================

        #region Published Events ==============================================================================================

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
                this.explorerBrowser = Shell32.CLSID.CoCreateInstance<Shell32.IExplorerBrowser>(Shell32.CLSID.ExplorerBrowser);

                if (this.explorerBrowser is null)
                    throw new Exception("Could not instantiate ExplorerBrowser!");



                Win32API.ShellAPI.IUnknown_SetSite(this.explorerBrowser, this);

                this.explorerBrowser.Advise(this, out this.adviseEventsCookie);

                var clientRect = new Win32API.RECT(this.ClientRectangle);        // implizite Konvertierung!!!!
                var folderSettings = new Win32API.ShellAPI.FOLDERSETTINGS(
                    Win32API.ShellAPI.FOLDERVIEWMODE.DETAILS, Win32API.ShellAPI.FOLDERFLAGS.None);

                this.explorerBrowser.Initialize(this.Handle, clientRect, folderSettings);

                this.explorerBrowser.SetOptions(Shell32.ExplorerBrowserOptions.ShowFrames);       // NoBorder?

                this.explorerBrowser.SetPropertyBag("electrifier.ExplorerBrowserControl");


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
                //this.viewevents.Disconnectview();         // TODO!!!

                this.explorerBrowser.Unadvise(this.adviseEventsCookie);
                Win32API.ShellAPI.IUnknown_SetSite(this.explorerBrowser, null);

                this.explorerBrowser.Destroy();

                Marshal.ReleaseComObject(this.explorerBrowser);
                this.explorerBrowser = null;
            }

            base.OnHandleDestroyed(e);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            if (!(this.explorerBrowser is null))
            {
                var rc = new Win32API.RECT(this.ClientRectangle);

                this.explorerBrowser.SetRect(IntPtr.Zero, rc);
            }

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
            peps = Shell32.ExplorerPaneState.DefaultOn;

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
