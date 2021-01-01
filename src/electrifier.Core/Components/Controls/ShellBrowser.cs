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
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Windows.Forms;

using Vanara.PInvoke;
using Vanara.Windows.Shell;

// http://www.sky.franken.de/doxy/explorer/structIShellBrowserImpl.html


// TODO: ViewFlags?!? 

namespace electrifier.Core.Components.Controls
{

    public class ShellBrowserNavigationCompleteEventArgs : EventArgs
    {
        public ShellFolder CurrentFolder { get; }

        public ShellBrowserNavigationCompleteEventArgs(ShellFolder currentFolder)
        {
            this.CurrentFolder = currentFolder ?? throw new ArgumentNullException(nameof(currentFolder));
        }
    }

    /////// <summary>
    /////// https://carsten.familie-schumann.info/blog/einfaches-invoke-in-c/
    /////// </summary>

    public static class FormInvokeExtension
    {
        static public void UIThreadAsync(this Control control, Action code)
        {
            if (null == control)
                throw new ArgumentNullException(nameof(control));

            if (null == code)
                throw new ArgumentNullException(nameof(code));

            if (control.InvokeRequired)
            {
                control.BeginInvoke(code);

                return;
            }
            else
                code.Invoke();
        }

        static public void UIThreadSync(this Control control, Action code)
        {
            if (null == control)
                throw new ArgumentNullException(nameof(control));

            if (null == code)
                throw new ArgumentNullException(nameof(code));

            if (control.InvokeRequired)
            {
                control.Invoke(code);

                return;
            }
            code.Invoke();
        }
    }

    // TODO: See Interlocked class for threading issues
    // TODO: Grouping => In ShellView2 oder so?!?
    //
    // IFolderView2 interface (shobjidl_core.h)
    // 12/05/2018
    // 2 minutes to read
    //Exposes methods that retrieve information about a folder's display options, select specified items in that folder, and set the folder's view mode.

    //
    // TODO: Very handy: https://stackoverflow.com/questions/7698602/how-to-get-embedded-explorer-ishellview-to-be-browsable-i-e-trigger-browseobje
    //
    // TODO: For Keyboard handling, have a look here: https://docs.microsoft.com/en-us/windows/win32/api/oleidl/nn-oleidl-ioleinplaceactiveobject
    //
    // TODO: Creating ViewWindow using '.CreateViewWindow(' fails on Zip-Folders; I barely remember we have to react on DDE_ - Messages for this to work?!?

    /// <summary>
    /// https://www.codeproject.com/Articles/28961/Full-implementation-of-IShellBrowser
    /// </summary>
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class ShellBrowser
        : UserControl
        , IWin32Window
        , Shell32.IShellBrowser
        , Shell32.IServiceProvider
        , Shell32.IShellFolderViewCB
    {

        private Guid IID_IShellBrowser = new Guid("000214E2-0000-0000-C000-000000000046");

        private const int WM_GETISHELLBROWSER = WM_USER + 0x00000007;
        private const int WM_USER = 0x00000400;          // See KB 157247



        private Shell32.IShellView shellView;
        private HWND shellViewHandle;
        private ShellFolder currentFolder;

        public ShellFolder CurrentFolder
        {
            get => this.currentFolder;
            set => this.SetCurrentFolder(value);
        }



        public ShellBrowser()
            : base()
        {
            this.InitializeComponent();

            this.Resize += this.ShellBrowser_Resize;
        }

        private void ShellBrowser_Resize(object sender, EventArgs e)
        {
            if (this.shellView != null && this.shellViewHandle != null)
            {
                User32.MoveWindow(this.shellViewHandle, 0, 0, this.ClientRectangle.Width, this.ClientRectangle.Height, false);
            }
        }

        public void SetCurrentFolder(ShellFolder newCurrentFolder)
        {
            // Note: The Designer will set shellFolder to NULL
            if (this.DesignMode || (newCurrentFolder is null))
                return;

            if (null != this.currentFolder)
            {
                if (Shell32.PIDLUtil.Equals(this.currentFolder.PIDL, newCurrentFolder.PIDL))
                {
                    AppContext.TraceWarning("ShellBrowser.SetCurrentFolder(): Targeted same folder " +
                        $"'{ newCurrentFolder.GetDisplayName(ShellItemDisplayString.DesktopAbsoluteEditing) }'");
                    return;
                }

                this.currentFolder.Dispose();
            }

            this.currentFolder = newCurrentFolder;

            this.BrowseObject((IntPtr)this.CurrentFolder.PIDL, Shell32.SBSP.SBSP_ABSOLUTE);
        }


        #region Published Events ==============================================================================================


        [Category("Shell Event"), Description("ShellBowser has navigated to a new folder.")]
        public event EventHandler<ShellBrowserNavigationCompleteEventArgs> NavigationComplete;

        #endregion ============================================================================================================


        protected void OnNavigationComplete(ShellFolder shellFolder)
        {
            if (null != this.NavigationComplete)
            {
                ShellBrowserNavigationCompleteEventArgs eventArgs = new ShellBrowserNavigationCompleteEventArgs(shellFolder);

                this.NavigationComplete.Invoke(this, eventArgs);
            }
        }



        protected ShellFolder RecreateShellView(Shell32.PIDL pidl)
        {
            var shellFolder = new ShellFolder(pidl);
            var sfvCreate = new Shell32.SFV_CREATE()
            {
                psfvcb = this, // IShellFolderViewCB
                pshf = shellFolder.IShellFolder,
                psvOuter = null,
            };
            sfvCreate.cbSize = (uint)Marshal.SizeOf(sfvCreate);
            var folderSettings = new Shell32.FOLDERSETTINGS(
                Shell32.FOLDERVIEWMODE.FVM_AUTO,
                Shell32.FOLDERFLAGS.FWF_NONE);


            Shell32.SHCreateShellFolderView(ref sfvCreate, out Shell32.IShellView newShellView).ThrowIfFailed();

            /// TODO: Take care of threading here... The following has to be Invoked into the main thread.

            if (!this.shellViewHandle.IsNull)
                User32.DestroyWindow(this.shellViewHandle);

            if (this.shellView != null)
            {
                this.shellView.UIActivate(Shell32.SVUIA.SVUIA_DEACTIVATE);

                if (this.shellView.GetType().IsCOMObject)
                    Marshal.ReleaseComObject(this.shellView);
            }

            Shell32.IFolderView2 folderview = (Shell32.IFolderView2)newShellView;
            if (folderview is null)
                MessageBox.Show("No IFolderView2");


            var test = folderview.GetCurrentViewMode();
            AppContext.TraceDebug($"ViewMode: {test}");

            // internal IFolderView2 GetFolderView2() { try { return explorerBrowserControl?.GetCurrentView<IFolderView2>(); } catch { return null; } }

            try
            {
                this.shellView = newShellView;

                // TODO: For folder "Network", we'll get an COMException here: 0x80004005
                // TODO: Zipped folders won't work at all :(
                this.shellViewHandle = newShellView.CreateViewWindow(psvPrevious: null, pfs: folderSettings, psb: this, prcView: this.ClientRectangle);

                this.shellView.UIActivate(Shell32.SVUIA.SVUIA_ACTIVATE_NOFOCUS);

                return shellFolder;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show($"RecreateShellView failed: { ex }");

                throw;
            }
        }

        /// <summary>
        /// Override <seealso cref="UserControl.WndProc(ref Message)"/> to handle custom messages.
        /// </summary>
        /// <param name="message">The Window Message to process.</param>
        protected override void WndProc(ref Message message)
        {
            switch (message.Msg)
            {
                case WM_GETISHELLBROWSER:
                    message.Result = Marshal.GetComInterfaceForObject(this, typeof(Shell32.IShellBrowser));
                    break;
                default:
                    base.WndProc(ref message);
                    break;
            }
        }

        #region IShellBrowser interface =======================================================================================

        public HRESULT GetWindow(out HWND phwnd)
        {
            phwnd = this.Handle;

            return HRESULT.S_OK;
        }

        public HRESULT ContextSensitiveHelp(bool fEnterMode) => HRESULT.E_NOTIMPL;

        public HRESULT InsertMenusSB(HMENU hmenuShared, ref Ole32.OLEMENUGROUPWIDTHS lpMenuWidths) => HRESULT.E_NOTIMPL;

        public HRESULT SetMenuSB(HMENU hmenuShared, IntPtr holemenuRes, HWND hwndActiveObject) => HRESULT.E_NOTIMPL;

        public HRESULT RemoveMenusSB(HMENU hmenuShared) => HRESULT.E_NOTIMPL;

        public HRESULT SetStatusTextSB(string pszStatusText) => HRESULT.E_NOTIMPL;

        public HRESULT EnableModelessSB(bool fEnable) => HRESULT.E_NOTIMPL;

        public HRESULT TranslateAcceleratorSB(ref MSG pmsg, ushort wID) => HRESULT.E_NOTIMPL;

        public HRESULT BrowseObject(IntPtr pidl, Shell32.SBSP wFlags)
        {


            // Double Click: SBSP_SAMEBROWSER, SBSP_OPENMODE


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
                var shellFolder = this.RecreateShellView(pidlTmp);

                if (null != shellFolder)
                {
                    // TODO: Lock currentFolder cause of MultiThreading!
                    var oldFolder = this.currentFolder;
                    this.currentFolder = shellFolder;
                    oldFolder.Dispose();

                    this.OnNavigationComplete(shellFolder);
                }

                pidlTmp.Close();
            });

            return HRESULT.S_OK;
        }

        public HRESULT GetViewStateStream(STGM grfMode, out IStream stream)
        {
            stream = null;

            return HRESULT.E_NOTIMPL;
        }

        public HRESULT GetControlWindow(Shell32.FCW id, out HWND hwnd)
        {
            hwnd = HWND.NULL;

            return HRESULT.E_NOTIMPL;
        }

        public HRESULT SendControlMsg(Shell32.FCW id, uint uMsg, IntPtr wParam, IntPtr lParam, out IntPtr pret)
        {
            pret = IntPtr.Zero;

            return HRESULT.E_NOTIMPL;
        }

        public HRESULT QueryActiveShellView(out Shell32.IShellView shellView)
        {
            Marshal.AddRef(Marshal.GetIUnknownForObject(this.shellView));

            shellView = this.shellView;

            return HRESULT.S_OK;
        }

        public HRESULT OnViewWindowActive(Shell32.IShellView ppshv) => HRESULT.E_NOTIMPL;

        public HRESULT SetToolbarItems(ComCtl32.TBBUTTON[] lpButtons, uint nButtons, Shell32.FCT uFlags) => HRESULT.E_NOTIMPL;

        #endregion ============================================================================================================


        #region IServiceProvider interface ====================================================================================

        public HRESULT QueryService(in Guid guidService, in Guid riid, out IntPtr ppvObject)
        {
            if (riid.Equals(this.IID_IShellBrowser))
            {
                ppvObject = Marshal.GetComInterfaceForObject(this, typeof(Shell32.IShellBrowser));

                return HRESULT.S_OK;
            }

            if (riid.Equals(typeof(Shell32.IShellFolderViewCB).GUID)) // [Guid("2047E320-F2A9-11CE-AE65-08002B2E1262")]
            {
                ppvObject = Marshal.GetComInterfaceForObject(this, typeof(Shell32.IShellFolderViewCB));

                return HRESULT.S_OK;
            }

            ppvObject = IntPtr.Zero;
            return HRESULT.E_NOINTERFACE;
        }

        #endregion ============================================================================================================

        #region IShellFolderViewCB interface ==================================================================================

        public HRESULT MessageSFVCB(Shell32.SFVM uMsg, IntPtr wParam, IntPtr lParam, ref IntPtr plResult)
        {
            //AppContext.TraceDebug($"{uMsg}");
            //switch (uMsg)
            //{
            //    case Shell32.SFVM.SFVM_UPDATESTATUSBAR:
            //        AppContext.TraceDebug("SFVM_UPDATESTATUSBAR");
            //        break;

            //    default:
            //        break;
            //}
            return HRESULT.S_OK;
        }

        #endregion ============================================================================================================



        #region Component Designer Support ====================================================================================

        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components;

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
