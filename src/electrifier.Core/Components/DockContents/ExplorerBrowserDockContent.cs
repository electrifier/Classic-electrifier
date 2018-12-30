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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Vanara.PInvoke;
using WeifenLuo.WinFormsUI.Docking;

namespace electrifier.Core.Components.DockContents
{
    public class ExplorerBrowserDockContent
        : WeifenLuo.WinFormsUI.Docking.DockContent
        , Vanara.PInvoke.Shell32.IServiceProvider
        , Vanara.PInvoke.Shell32.IExplorerPaneVisibility
        , Vanara.PInvoke.Shell32.IExplorerBrowserEvents
        , Vanara.PInvoke.Shell32.ICommDlgBrowser3
    {
        #region Fields ========================================================================================================

        protected Vanara.PInvoke.Shell32.ExplorerBrowser explorerBrowser;

        #endregion Fields =====================================================================================================


        #region Properties ====================================================================================================

        public uint ItemsCount {
            //get => this.explorerBrowser.Items.Count; }
            get => 0;
        }
        public uint SelectedItemsCount {
            //get => this.explorerBrowser.SelectedItems.Count; }
            get => 0;
        }

        #endregion Properties =================================================================================================


        #region Published Events ==============================================================================================

        public delegate void ItemsChangedHandler(ExplorerBrowserDockContent sender, EventArgs eventArgs);               // TODO: EventArgs?!?
        public event ItemsChangedHandler ItemsChanged;

        public delegate void SelectionChangedHandler(ExplorerBrowserDockContent sender, EventArgs eventArgs);           // TODO: EventArgs?!?
        public event SelectionChangedHandler SelectionChanged;

        // 30.12.18 - TODO: Actually NavigationLogEventArgs seem to be self-implemented. Using a stub for now...
        public delegate void NavigationLogChangedHandler(ExplorerBrowserDockContent sender, EventArgs eventArgs);       // TODO: EventArgs?!?
        public event NavigationLogChangedHandler NavigationLogChanged;

        //public event System.EventHandler<Microsoft.WindowsAPICodePack.Controls.NavigationLogEventArgs> NavigationLogChanged {
        //    add { this.explorerBrowser.NavigationLog.NavigationLogChanged += value; }
        //    remove { this.explorerBrowser.NavigationLog.NavigationLogChanged -= value; }
        //}


        #endregion Published Events ===========================================================================================

        public ExplorerBrowserDockContent(in string persistString = null) : base()
        {

        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
        }

        public void NavigateBackward()
        {
            //this.explorerBrowser.NavigateLogLocation(NavigationLogDirection.Backward);
        }

        public void NavigateForward()
        {
            //this.explorerBrowser.NavigateLogLocation(NavigationLogDirection.Forward);
        }

        public void NavigateLogLocation(int navigationLogIndex)
        {
            //this.explorerBrowser.NavigateLogLocation(navigationLogIndex);
        }

        public void NavigateRefresh()
        {

        }


        #region Vanara.PInvoke.Shell32.IServiceProvider =======================================================================

        public HRESULT QueryService(in Guid guidService, in Guid riid, out object ppvObject)
        {
            throw new NotImplementedException();
        }

        #endregion Vanara.PInvoke.Shell32.IServiceProvider ====================================================================


        #region Vanara.PInvoke.Shell32.IExplorerPaneVisibility ================================================================

        public HRESULT GetPaneState(in Guid ep, ref Shell32.EXPLORERPANESTATE peps)
        {
            throw new NotImplementedException();
        }

        #endregion Vanara.PInvoke.Shell32.IExplorerPaneVisibility =============================================================


        #region Vanara.PInvoke.Shell32.IExplorerBrowserEvents =================================================================

        public void OnNavigationPending(Shell32.PIDL pidlFolder)
        {
            throw new NotImplementedException();
        }

        public void OnViewCreated(Shell32.IShellView psv)
        {
            throw new NotImplementedException();
        }

        public void OnNavigationComplete(Shell32.PIDL pidlFolder)
        {
            throw new NotImplementedException();
        }

        public void OnNavigationFailed(Shell32.PIDL pidlFolder)
        {
            throw new NotImplementedException();
        }

        #endregion Vanara.PInvoke.Shell32.IExplorerBrowserEvents ==============================================================


        #region Vanara.PInvoke.Shell32.ICommDlgBrowser3 =======================================================================

        public void OnDefaultCommand(Shell32.IShellView ppshv)
        {
            throw new NotImplementedException();
        }

        public void OnStateChange(Shell32.IShellView ppshv, Shell32.CDBOSC uChange)
        {
            throw new NotImplementedException();
        }

        public HRESULT IncludeObject(Shell32.IShellView ppshv, Shell32.PIDL pidl)
        {
            throw new NotImplementedException();
        }

        public void Notify(Shell32.IShellView ppshv, Shell32.CDB2N dwNotifyType)
        {
            throw new NotImplementedException();
        }

        public HRESULT GetDefaultMenuText(Shell32.IShellView ppshv, StringBuilder pszText, int cchMax)
        {
            throw new NotImplementedException();
        }

        public Shell32.CDB2GVF GetViewFlags()
        {
            throw new NotImplementedException();
        }

        public void OnColumnClicked(Shell32.IShellView ppshv, int iColumn)
        {
            throw new NotImplementedException();
        }

        public void GetCurrentFilter(StringBuilder pszFileSpec, int cchFileSpec)
        {
            throw new NotImplementedException();
        }

        public void OnPreViewCreated(Shell32.IShellView ppshv)
        {
            throw new NotImplementedException();
        }

        #endregion Vanara.PInvoke.Shell32.ICommDlgBrowser3 ====================================================================

    }
}
