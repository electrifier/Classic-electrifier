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
    // TODO: Disable header in Details view when grouping is enabled
    //
    //
    // NOTE: Very handy: https://stackoverflow.com/questions/7698602/how-to-get-embedded-explorer-ishellview-to-be-browsable-i-e-trigger-browseobje
    // NOTE: Very handy: https://stackoverflow.com/questions/54390268/getting-the-current-ishellview-user-is-interacting-with
    //
    // NOTE: For Keyboard handling, have a look here: https://docs.microsoft.com/en-us/windows/win32/api/oleidl/nn-oleidl-ioleinplaceactiveobject
    //
    // TODO: Creating ViewWindow using '.CreateViewWindow(' fails on Zip-Folders; I barely remember we have to react on DDE_ - Messages for this to work?!? -> When Implementing ICommonDlgBrowser, this works! => Fixed by removing IShellFolderViewCB

    /// <summary>
    /// 
    /// This implements the following Interfaces:
    /// ICommonDlgBrowser: To be able to browse into Zip-Folders => NO! // NOTE: 01/01/21: IShellFolderViewCB => If you provide this, navigating into zipped Folders won't work any more. Maybe you have to respond to one if it's Messages correctly.
    /// 
    /// https://www.codeproject.com/Articles/28961/Full-implementation-of-IShellBrowser
    /// 
    /// 
    /// Known Issues:
    /// - Using windows 10, the virtual Quick-Access folder doesn't get displayed properly. It has to be grouped by "Group"
    ///   (as shown in Windows Explorer UI), but I couldn't find the OLE-Property for this.
    ///   Also, if using Groups, the Frequent Files List doesn't have its Icons. Maybe we have to bind to another version
    ///   of ComCtrls to get this rendered properly - That's just an idea though, cause the Collapse-/Expand-Icons of the
    ///   Groups have the Windows Vista / Windows 7-Theme, not the Windows 10 Theme as I can see.
    /// - Keyboard input doesn't work so far.
    /// - DONE: Only Details-Mode should have column headers: (Using Shell32.FOLDERFLAGS.FWF_NOHEADERINALLVIEWS)
    ///   https://stackoverflow.com/questions/11776266/ishellview-columnheaders-not-hidden-if-autoview-does-not-choose-details
    /// </summary>
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class ShellBrowser
        : UserControl
        , IWin32Window      // See also: NativeWindow
        , Shell32.IShellBrowser
        , Shell32.IServiceProvider
        , Shell32.IShellFolderViewCB
    {

        //private Guid IID_IShellBrowser = new Guid("000214E2-0000-0000-C000-000000000046");

        //private const int WM_GETISHELLBROWSER = WM_USER + 0x00000007;
        //private const int WM_USER = 0x00000400;          // See KB 157247



        private ShellFolder currentFolder;
        private Shell32.IShellView shellView;
        private HWND shellViewHandle;

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

        protected virtual void SetCurrentFolder(ShellFolder newCurrentFolder)
        {
            // Note: The Designer may set CurrentFolder to null
            if (this.DesignMode || (newCurrentFolder is null))
                return;

            // TODO: 02/01/21: We should not allow exceptions here! When navigation fails, show a warning in the browser, call NavigationFailed event
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
                psfvcb = null, // NOTE: 01/01/21: IShellFolderViewCB => If you provide this, navigating into virtual zipped Folders won't work any more. Maybe you have to respond to one if it's Messages correctly.
                pshf = shellFolder.IShellFolder,
                psvOuter = null,
            };
            sfvCreate.cbSize = (uint)Marshal.SizeOf(sfvCreate);
            var folderSettings = new Shell32.FOLDERSETTINGS(
                Shell32.FOLDERVIEWMODE.FVM_AUTO,
                Shell32.FOLDERFLAGS.FWF_NOHEADERINALLVIEWS);


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
            //Shell32.IShellView3 shellView3 = (Shell32.IShellView3)newShellView;
            //if(shellView3 is null)
            //    MessageBox.Show("No ShellView3");




            folderview.SetText(Shell32.FVTEXTTYPE.FVST_EMPTYTEXT, "This folder\nis empty!");        // TODO: Put into property
                                                                                                    //folderview.SetViewModeAndIconSize

            

            //folderview.SetGroupBy(Ole32.PROPERTYKEY.System.PerceivedType, true);
            //folderview.SetGroupBy(Ole32.PROPERTYKEY.System.Category, true);



            // TODO: Windows 10' Quick Access folder has a special type of grouping, can't find out how this works yet.
            //       As soon as we would be able to get all the available properties for an particular item, we would be able found out how this grouping works.
            //       However, it seems to be a special group, since folders are Tiles, whereas files are shown in Details mode.
            // NOTE: The grouping is done by 'Group'. Activate it using "Group by->More->Group", and then do the grouping.
            //       However, the Icons for 'Recent Files'-Group get lost.
            // NOTE: This could help: https://answers.microsoft.com/en-us/windows/forum/windows_10-files-winpc/windows-10-quick-access-folders-grouped-separately/ecd4be4a-1847-4327-8c44-5aa96e0120b8


            //folderview.SetGroupBy(Ole32.PROPERTYKEY.System.Shell., true); ==> 01.01.21.23:33: "Group" wird gesucht...



            var test = folderview.GetCurrentViewMode();
            AppContext.TraceDebug($"ViewMode: {test}");

            // internal IFolderView2 GetFolderView2() { try { return explorerBrowserControl?.GetCurrentView<IFolderView2>(); } catch { return null; } }

            try
            {
                this.shellView = newShellView;

                // TODO: For folder "Network", we'll get an COMException here: 0x80004005,      => // NOTE: 01/01/21: since dropping ICommonDlgBrowser Interface, there's no exception anymore.
                // TODO: Zipped folders won't work at all :(                                    => // NOTE: 01/01/21: Since dropping IShellFolderViewCB, they work again
                this.shellViewHandle = newShellView.CreateViewWindow(psvPrevious: null, pfs: folderSettings, psb: this, prcView: this.ClientRectangle);

                // TODO: Take care of disk drives with removeable media; => "Cancalled by the user" COMException

                this.shellView.UIActivate(Shell32.SVUIA.SVUIA_ACTIVATE_NOFOCUS);

                return shellFolder;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show($"RecreateShellView failed: { ex }");

                throw;
            }
        }

        ///// <summary>
        ///// Override <seealso cref="UserControl.WndProc(ref Message)"/> to handle custom messages.
        ///// </summary>
        ///// <param name="message">The Window Message to process.</param>
        //protected override void WndProc(ref Message message)
        //{
        //    switch (message.Msg)
        //    {
        //        //case WM_GETISHELLBROWSER:       // 02/01/21: This Message is only needed when using ICommDlgBrowser
        //        //    message.Result = Marshal.GetComInterfaceForObject(this, typeof(Shell32.IShellBrowser));
        //        //    break;
        //        default:
        //            base.WndProc(ref message);
        //            break;
        //    }
        //}

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
            // Guid("000214E2-0000-0000-C000-000000000046")
            if (riid.Equals(typeof(Shell32.IShellBrowser).GUID))
            {
                ppvObject = Marshal.GetComInterfaceForObject(this, typeof(Shell32.IShellBrowser));

                return HRESULT.S_OK;
            }

            // Guid("2047E320-F2A9-11CE-AE65-08002B2E1262")
            if (riid.Equals(typeof(Shell32.IShellFolderViewCB).GUID))
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
