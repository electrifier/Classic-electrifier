/*
** 
**  electrifier
** 
**  Copyright 2017-19 Thorsten Jung, www.electrifier.org
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
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Windows.Forms;

using Vanara.PInvoke;
using Vanara.Windows.Shell;

// http://www.sky.franken.de/doxy/explorer/structIShellBrowserImpl.html



namespace electrifier.Core.Components.Controls
{

    /////// <summary>
    /////// https://carsten.familie-schumann.info/blog/einfaches-invoke-in-c/
    /////// </summary>

    public static class FormInvokeExtension
    {
        static public void UIThreadAsync(this Control control, Action code)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(code);
                return;
            }
            code.Invoke();
        }

        static public void UIThreadSync(this Control control, Action code)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(code);
                return;
            }
            code.Invoke();
        }
    }



    /// <summary>
    /// https://www.codeproject.com/Articles/28961/Full-implementation-of-IShellBrowser
    /// </summary>
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class ShellBrowser
        : UserControl
        , System.Windows.Forms.IWin32Window
        , Shell32.IShellBrowser
        , Shell32.ICommDlgBrowser3
        , Shell32.IServiceProvider
    {

        private Guid IID_IShellBrowser = new Guid("000214E2-0000-0000-C000-000000000046");

        private Guid IID_ICommDlgBrowser = new Guid("000214F1-0000-0000-C000-000000000046");
        private Guid IID_ICommDlgBrowser3 = new Guid("C8AD25A1-3294-41EE-8165-71174BD01C57");


        private static uint WM_GETISHELLBROWSER = WM_USER + 0x00000007;
        private static uint WM_USER = 0x00000400;          // See KB 157247

        /*
          IID_IServiceProvider: TGUID = '{6D5140C1-7436-11CE-8034-00AA006009FA}';
          SID_STopLevelBrowser: TGUID = '{4C96BE40-915C-11CF-99D3-00AA004AE837}';

         dfbc7e30-f9e5-455f-88f8-fa98c1e494ca => IShellView
        */


        private Shell32.IShellView shellView;
        private HWND shellViewHandle;
        private ShellFolder currentFolder;

        public ShellFolder CurrentFolder
        {
            set => this.SetCurrentFolder(value);
            get => this.currentFolder;
        }



        public ShellBrowser()
            : base()
        {
            this.InitializeComponent();



            this.Load += this.ShellBrowser_Load;
            this.Resize += this.ShellBrowser_Resize;
        }


        // Erst Handle created, dann Load!

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);


        }

        private void ShellBrowser_Load(object sender, EventArgs e)
        {
            this.SetCurrentFolder(ShellFolder.Desktop);
            // SHCreateShellFolderView?!?
        }

        private void ShellBrowser_Resize(object sender, EventArgs e)
        {
            if (this.shellView != null && this.shellViewHandle != null)
            {
                User32.MoveWindow(this.shellViewHandle, 0, 0, this.ClientRectangle.Width, this.ClientRectangle.Height, false);
            }
        }

        public void SetCurrentFolder(ShellFolder shellFolder)
        {
            // TODO: Release current folder


            if (shellFolder is null) return;    // TODO: Designer setzt den Folder auf null!

            this.currentFolder = shellFolder;

            this.BrowseObject((IntPtr)this.CurrentFolder.PIDL, Shell32.SBSP.SBSP_ABSOLUTE);


        }

        protected ShellFolder GetShellView(Shell32.PIDL pidl)
        {
            /* Vanara: https://github.com/dahall/Vanara/blob/master/Windows.Shell/ShellObjects/ShellItem.cs GetIShellFolder
                            if (ShellFolder.Desktop.PIDL.Equals(dtPidl))
                                return ShellFolder.Desktop.iShellFolder;
                            return (IShellFolder)ShellFolder.Desktop.iShellFolder.BindToObject(PIDL, null, typeof(IShellFolder).GUID);
            */

            try
            {
                var shellFolder = new ShellFolder(pidl);
                Shell32.FOLDERSETTINGS folderSettings = new Shell32.FOLDERSETTINGS(Shell32.FOLDERVIEWMODE.FVM_DETAILS, Shell32.FOLDERFLAGS.FWF_NONE);
                RECT viewRect = new RECT(this.ClientRectangle);

                this.shellView = shellFolder.GetViewObject<Shell32.IShellView>(this);

                var defaultFolderSettings = this.shellView.GetCurrentInfo();

                this.shellViewHandle = this.shellView.CreateViewWindow(null, folderSettings, this, viewRect);

                this.shellView.UIActivate(Shell32.SVUIA.SVUIA_ACTIVATE_NOFOCUS);

                return shellFolder;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show($"GetShellView failed: {ex.ToString()}");
            }

            return null;
        }

        protected override void WndProc(ref Message m)
        {
            if (WM_GETISHELLBROWSER == m.Msg)
                m.Result = Marshal.GetComInterfaceForObject(this, typeof(Shell32.IShellBrowser));
            else
                base.WndProc(ref m);
        }

        #region IShellBrowser interface =======================================================================================

        public HWND GetWindow()
        {
            AppContext.TraceDebug("GetWindow");

            return this.Handle;
        }

        public void ContextSensitiveHelp(bool fEnterMode)
        {
            //throw new NotImplementedException();
        }

        public void InsertMenusSB(HMENU hmenuShared, ref Ole32.OLEMENUGROUPWIDTHS lpMenuWidths)
        {
            //throw new NotImplementedException();
        }

        public void SetMenuSB(HMENU hmenuShared, IntPtr holemenuRes, HWND hwndActiveObject)
        {
            //throw new NotImplementedException();
        }

        public void RemoveMenusSB(HMENU hmenuShared)
        {
            //throw new NotImplementedException();
        }

        public void SetStatusTextSB(string pszStatusText)
        {
            //throw new NotImplementedException();
        }

        public void EnableModelessSB(bool fEnable)
        {
            //throw new NotImplementedException();
        }

        public void TranslateAcceleratorSB(ref MSG pmsg, ushort wID)
        {
            //throw new NotImplementedException();
        }

        public void BrowseObject(IntPtr pidl, Shell32.SBSP wFlags)
        {


            // Doppelklick: SBSP_SAMEBROWSER, SBSP_OPENMODE


            Shell32.PIDL pidlTmp = default;



            if (ShellFolder.Desktop.PIDL.Equals(pidl))                      // pidl equals Desktop
            {
                pidlTmp = new Shell32.PIDL(pidl, true, true);
            }
            else if (wFlags.HasFlag(Shell32.SBSP.SBSP_RELATIVE))            // pidl is relative to the current fodler
            {
                AppContext.TraceDebug("BrowseObject: Relative");


                //// SBSP_RELATIVE - pidl is relative from the current folder
                //if ((hr = m_currentFolder.BindToObject(pidl, IntPtr.Zero, ref NativeMethods.IID_IShellFolder,
                //                                       out folderTmpPtr)) != NativeMethods.S_OK)
                //    return hr;
                //pidlTmp = NativeMethods.Shell32.ILCombine(m_pidlAbsCurrent, pidl);
                //folderTmp = (NativeMethods.IShellFolder)Marshal.GetObjectForIUnknown(folderTmpPtr);
            }
            else if (wFlags.HasFlag(Shell32.SBSP.SBSP_PARENT))              // browse to parent folder and ignore the pidl
            {
                AppContext.TraceDebug("BrowseObject: Parent");

                //// SBSP_PARENT - Browse the parent folder (ignores the pidl)
                //pidlTmp = GetParentPidl(m_pidlAbsCurrent);
                //string pathTmp = GetDisplayName(m_desktopFolder, pidlTmp, NativeMethods.SHGNO.SHGDN_FORPARSING);
                //if (pathTmp.Equals(m_desktopPath))
                //{
                //    pidlTmp = NativeMethods.Shell32.ILClone(m_desktopPidl);
                //    folderTmp = m_desktopFolder;
                //}
                //else
                //{
                //    if ((hr = m_desktopFolder.BindToObject(pidlTmp, IntPtr.Zero, ref NativeMethods.IID_IShellFolder,
                //                                           out folderTmpPtr)) != NativeMethods.S_OK)
                //        return hr;
                //    folderTmp = (NativeMethods.IShellFolder)Marshal.GetObjectForIUnknown(folderTmpPtr);
                //}
            }
            else
            {
                Debug.Assert(wFlags.HasFlag(Shell32.SBSP.SBSP_ABSOLUTE));

                pidlTmp = new Shell32.PIDL(pidl, true, true);




                //// SBSP_ABSOLUTE - pidl is an absolute pidl (relative from desktop)
                //pidlTmp = NativeMethods.Shell32.ILClone(pidl);
                //if ((hr = m_desktopFolder.BindToObject(pidlTmp, IntPtr.Zero, ref NativeMethods.IID_IShellFolder,
                //                                       out folderTmpPtr)) != NativeMethods.S_OK)
                //    return hr;
                //folderTmp = (NativeMethods.IShellFolder)Marshal.GetObjectForIUnknown(folderTmpPtr);

            }








            this.UIThreadSync(delegate
            {
                this.GetShellView(pidlTmp);
            });
        }

        public IStream GetViewStateStream(STGM grfMode)
        {
            return default;                 //throw new NotImplementedException();
        }

        public IntPtr GetControlWindow(Shell32.FCW id)
        {
            return IntPtr.Zero;             //throw new NotImplementedException();
        }

        public IntPtr SendControlMsg(Shell32.FCW id, uint uMsg, IntPtr wParam, IntPtr lParam)
        {
            return IntPtr.Zero;             //throw new NotImplementedException();
        }

        public Shell32.IShellView QueryActiveShellView()
        {
            Marshal.AddRef(Marshal.GetIUnknownForObject(this.shellView));

            return this.shellView;
        }

        public void OnViewWindowActive(Shell32.IShellView ppshv)
        {
        }

        public void SetToolbarItems(ComCtl32.TBBUTTON[] lpButtons, uint nButtons, Shell32.FCT uFlags)
        {
        }

        #endregion ============================================================================================================



        #region ICommDlgBrowser3 ==============================================================================================

        public HRESULT OnDefaultCommand(Shell32.IShellView ppshv)
        {
            return HRESULT.E_NOTIMPL;
        }

        public HRESULT OnStateChange(Shell32.IShellView ppshv, Shell32.CDBOSC uChange)
        {
            return HRESULT.E_NOTIMPL;
        }

        public HRESULT IncludeObject(Shell32.IShellView ppshv, IntPtr pidl)
        {
            return HRESULT.E_NOTIMPL;
        }

        public HRESULT GetDefaultMenuText(Shell32.IShellView ppshv, StringBuilder pszText, int cchMax)
        {
            return HRESULT.E_NOTIMPL;
        }

        public HRESULT GetViewFlags(out Shell32.CDB2GVF pdwFlags)
        {
            pdwFlags = Shell32.CDB2GVF.CDB2GVF_SHOWALLFILES;        // Show all files, including hidden and system files

            return HRESULT.S_OK;
        }

        public HRESULT Notify(Shell32.IShellView ppshv, Shell32.CDB2N dwNotifyType)
        {
            return HRESULT.E_NOTIMPL;
        }

        public HRESULT GetCurrentFilter(StringBuilder pszFileSpec, int cchFileSpec)
        {
            return HRESULT.E_NOTIMPL;
        }

        public HRESULT OnColumnClicked(Shell32.IShellView ppshv, int iColumn)
        {
            return HRESULT.E_NOTIMPL;
        }

        public HRESULT OnPreViewCreated(Shell32.IShellView ppshv)
        {
            return HRESULT.S_OK;
        }

        #endregion ============================================================================================================



        #region IServiceProvider interface ====================================================================================

        public HRESULT QueryService(in Guid guidService, in Guid riid, out object ppvObject)
        {
            if (riid.Equals(this.IID_IShellBrowser))
            {
                // Wenn folgendes Marshalling durchgeführt wird, dann geht der "Doppelklick" nicht mehr  :(
                // ppvObject = Marshal.GetComInterfaceForObject(this, typeof(Shell32.IShellBrowser));
                ppvObject = this;

                return HRESULT.S_OK;
            }

            //if (guidService.Equals(this.IID_ICommDlgBrowser))
            //{
            //    if (riid.Equals(this.IID_ICommDlgBrowser) || riid.Equals(this.IID_ICommDlgBrowser3))
            //    {
            //        ppvObject = Marshal.GetComInterfaceForObject(this, typeof(Shell32.ICommDlgBrowser3));

            //        return HRESULT.S_OK;
            //    }
            //}

            AppContext.TraceDebug($"QueryService: {guidService}, riid: {riid}");

            ppvObject = null;

            return HRESULT.E_NOINTERFACE;
        }

        #endregion ============================================================================================================



        #region Component Designer Support ====================================================================================

        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        }

        #endregion ============================================================================================================
    }
}
