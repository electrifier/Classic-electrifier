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
using System.Windows.Forms;

using Vanara.PInvoke;
using Vanara.Windows.Shell;

namespace electrifier.Core.Components.Controls
{
    /// <summary>
    /// TODO: See Interlocked class for threading issues
    /// FIXED TODO: Disable header in Details view when grouping is enabled
    /// NOTE: Very handy: https://stackoverflow.com/questions/7698602/how-to-get-embedded-explorer-ishellview-to-be-browsable-i-e-trigger-browseobje
    /// NOTE: Very handy: https://stackoverflow.com/questions/54390268/getting-the-current-ishellview-user-is-interacting-with
    ///
    /// NOTE: For Keyboard handling, have a look here: https://docs.microsoft.com/en-us/windows/win32/api/oleidl/nn-oleidl-ioleinplaceactiveobject
    ///
    /// FIXED TODO: Creating ViewWindow using '.CreateViewWindow(' fails on Zip-Folders; I barely remember we have to react on DDE_ - Messages for this to work?!? -> When Implementing ICommonDlgBrowser, this works!
    ///       => Fixed by removing IShellFolderViewCB ==> Fixed again by returning HRESULT.E_NOTIMPL from MessageSFVCB
    /// DOCS: https://www.codeproject.com/Articles/35197/Undocumented-List-View-Features
    /// DOCS: https://stackoverflow.com/questions/15750842/allow-selection-in-explorer-style-list-view-to-start-in-the-first-column
    /// TODO: internal static readonly bool IsMinVista = Environment.OSVersion.Version.Major >= 6;     // TODO: We use one interface, afaik, that only works in vista and above
    /// TODO: Use COMReleaser: https://github.com/dahall/Vanara/blob/master/Core/InteropServices/ComReleaser.cs
    /// TODO: Windows 10' Quick Access folder has a special type of grouping, can't find out how this works yet.
    ///       As soon as we would be able to get all the available properties for an particular item, we would be able found out how this grouping works.
    ///       However, it seems to be a special group, since folders are Tiles, whereas files are shown in Details mode.
    /// NOTE: The grouping is done by 'Group'. Activate it using "Group by->More->Group", and then do the grouping.
    ///       However, the Icons for 'Recent Files'-Group get lost.
    /// TODO: Maybe this interface could help, too: https://docs.microsoft.com/en-us/windows/win32/shell/shellfolderview
    /// NOTE: This could help: https://answers.microsoft.com/en-us/windows/forum/windows_10-files-winpc/windows-10-quick-access-folders-grouped-separately/ecd4be4a-1847-4327-8c44-5aa96e0120b8
    /// TODO: Add border like the one on the adressbar of Edge
    /// </summary>

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


    /// <summary>
    /// Encapsulates a <see cref="Shell32.IShellBrowser"/>-Implementation within an <see cref="UserControl"/>.<br/>
    /// <br/>
    /// Implements the following Interfaces:<br/>
    /// - <seealso cref="IWin32Window"/><br/>
    /// - <seealso cref="Shell32.IShellBrowser"/><br/>
    /// - <seealso cref="Shell32.IShellFolderViewCB"/><br/>
    /// - <seealso cref="Shell32.IServiceProvider"/><br/>
    /// <br/>
    /// For more Information on used techniques see:<br/>
    /// - <seealso href="https://www.codeproject.com/Articles/28961/Full-implementation-of-IShellBrowser"/><br/>
    /// <br/>
    /// <br/>
    /// Known Issues:<br/>
    /// - Using windows 10, the virtual Quick-Access folder doesn't get displayed properly. It has to be grouped by "Group"
    ///   (as shown in Windows Explorer UI), but I couldn't find the OLE-Property for this.
    ///   Also, if using Groups, the Frequent Files List doesn't have its Icons. Maybe we have to bind to another version
    ///   of ComCtrls to get this rendered properly - That's just an idea though, cause the Collapse-/Expand-Icons of the
    ///   Groups have the Windows Vista / Windows 7-Theme, not the Windows 10 Theme as I can see.<br/>
    /// - Keyboard input doesn't work so far.<br/>
    /// - DONE: Only Details-Mode should have column headers: (Using Shell32.FOLDERFLAGS.FWF_NOHEADERINALLVIEWS)<br/>
    ///   https://stackoverflow.com/questions/11776266/ishellview-columnheaders-not-hidden-if-autoview-does-not-choose-details
    /// - TODO: CustomDraw, when currently no shellView available, draw white background to avoid flicker while navigating?!? => OR: Release shellViewWindow AFTER creating the new window...<br/>
    /// - DONE: Network folder: E_FAIL => DONE: Returning HRESULT.E_NOTIMPL from MessageSFVCB fixes this<br/>
    /// - DONE: Disk Drive (empty): E_CANCELLED_BY_USER<br/>
    /// </summary>
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class ShellBrowser
        : UserControl
        , IWin32Window
        , Shell32.IShellBrowser
        , Shell32.IServiceProvider
    {
        private ShellFolder currentFolder;
        private Shell32.IShellView shellView;
        private HWND shellViewWindow;
        private Shell32.IFolderView2 folderView2;

        #region Properties =====================================================================================================

        /// <summary>Contains the navigation history of the ExplorerBrowser</summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ShellNavigationHistory History { get; private set; }


        //private string emptyFolderText = "This folder\nis empty.";
        //[Category("Appearance"), Description("The default text that is displayed when an empty folder is shown.")]
        //public string EmptyFolderText
        //{
        //    get => this.emptyFolderText;
        //    set => this.SetEmptyFolderText(value);
        //}

        public ShellFolder CurrentFolder
        {
            get => this.currentFolder;
            set => this.SetCurrentFolder(value);
        }

        #endregion ============================================================================================================

        #region Published Events ==============================================================================================

        [Category("Shell Event"), Description("ShellBowser has navigated to a new folder.")]
        public event EventHandler<ShellBrowserNavigationCompleteEventArgs> NavigationComplete;

        #endregion ============================================================================================================


        public ShellBrowser()
            : base()
        {
            this.InitializeComponent();

            //this.History = new ShellNavigationHistory();

            this.Resize += this.ShellBrowser_Resize;
        }

        private void ShellBrowser_Resize(object sender, EventArgs e)
        {
            if (this.shellView != null && this.shellViewWindow != null)
            {
                User32.MoveWindow(this.shellViewWindow, 0, 0, this.ClientRectangle.Width, this.ClientRectangle.Height, false);
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

        //protected virtual void SetEmptyFolderText(string value)     // TODO!
        //{
        //    this.emptyFolderText = value;
        //    this.folderView2?.SetText(Shell32.FVTEXTTYPE.FVST_EMPTYTEXT, value);
        //}

        protected void OnNavigationComplete(ShellFolder shellFolder)
        {
            if (null != this.NavigationComplete)
            {
                ShellBrowserNavigationCompleteEventArgs eventArgs = new ShellBrowserNavigationCompleteEventArgs(shellFolder);

                this.NavigationComplete.Invoke(this, eventArgs);
            }
        }

        /// <summary>
        /// Raises the <see cref="Navigating"/> event.
        /// </summary>
        protected internal virtual void OnNavigating(ShellBrowserViewHandler eventargs, out bool cancelled)
        {
            cancelled = false;

            //return eventargs;       // For fluid design pattern?!?

            //if (Navigating is null || npevent?.PendingLocation is null) return;
            //foreach (var del in Navigating.GetInvocationList())
            //{
            //    del.DynamicInvoke(new object[] { this, npevent });
            //    if (npevent.Cancel)
            //        cancelled = true;
            //}
        }

        protected internal virtual void DoNavigating(ShellBrowserViewHandler args)
        {
            if (!args.IsBrowsable)
                return;

            this.shellView = args.ShellView;        // => set -> Release old ones...
            this.folderView2 = args.FolderView2;    // 
            this.shellViewWindow = args.ViewWindow; //

            args.ShellView.UIActivate(Shell32.SVUIA.SVUIA_ACTIVATE_NOFOCUS);

            // TODO: Set msg hook: MessageSFVCB...
            // TODO: Marshal.AddRef() on COM-Objects
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
            Shell32.PIDL pidlTmp = default;

            // TODO: if(Shell32.SBSP.SBSP_NAVIGATEFORWARD)
            // TODO: if(Shell32.SBSP.SBSP_NAVIGATEBACK)



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








            this.UIThreadSync(delegate  //TODO: Check if asynchronous processing is possible
            {
                var newBrowserViewHandler = new ShellBrowserViewHandler(this, new ShellFolder(pidlTmp));

                this.OnNavigating(newBrowserViewHandler, out bool cancelled);

                if (!cancelled)
                {
                    this.DoNavigating(newBrowserViewHandler);
                }
                else
                    newBrowserViewHandler.Dispose();


                //using (var navigatingArgs = new ShellBrowserViewHandler(this, new ShellFolder(pidlTmp)))
                //{
                //    this.OnNavigating(navigatingArgs, out bool cancelled);

                //    if (!cancelled)
                //        this.DoNavigating(navigatingArgs);
                //}

                /*
                var shellFolder = this.RecreateShellView(pidlTmp);

                if (null != shellFolder)
                {
                    // TODO: Lock currentFolder cause of MultiThreading!
                    var oldFolder = this.currentFolder;
                    this.currentFolder = shellFolder;
                    oldFolder.Dispose();

                    this.OnNavigationComplete(shellFolder);
                }
                */

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

        /// <summary>
        /// <see cref="Shell32.IServiceProvider"/>-Interface Implementation for <see cref="ShellBrowser"/>.<br/>
        /// <br/>
        /// Responds to the following Interfaces:<br/>
        /// - <see cref="Shell32.IShellBrowser"/><br/>
        /// - <see cref="Shell32.IShellFolderViewCB"/><br/>
        /// </summary>
        /// <param name="guidService">The service's unique identifier (SID).</param>
        /// <param name="riid">The IID of the desired service interface.</param>
        /// <param name="ppvObject">When this method returns, contains the interface pointer requested riid. If successful,
        /// the calling application is responsible for calling IUnknown::Release using this value when the service is no
        /// longer needed. In the case of failure, this value is NULL.</param>
        /// <returns>
        ///  <see cref="HRESULT.S_OK"/> or<br/>
        ///  <see cref="HRESULT.E_NOINTERFACE"/>
        /// </returns>
        HRESULT Shell32.IServiceProvider.QueryService(in Guid guidService, in Guid riid, out IntPtr ppvObject)
        {
            // IShellBrowser: Guid("000214E2-0000-0000-C000-000000000046")
            if (riid.Equals(typeof(Shell32.IShellBrowser).GUID))
            {
                ppvObject = Marshal.GetComInterfaceForObject(this, typeof(Shell32.IShellBrowser));

                return HRESULT.S_OK;
            }

            // IShellFolderViewCB: Guid("2047E320-F2A9-11CE-AE65-08002B2E1262")
            if (riid.Equals(typeof(Shell32.IShellFolderViewCB).GUID))
            {
                ppvObject = Marshal.GetComInterfaceForObject(this, typeof(Shell32.IShellFolderViewCB));

                return HRESULT.S_OK;
            }

            ppvObject = IntPtr.Zero;
            return HRESULT.E_NOINTERFACE;
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

    #region SFVMUD: ShellFolderView undocumented Messages =====================================================================

#pragma warning disable CA1707 // Identifiers should not contain underscores

    /// <summary>
    /// Undocumented Flags used by <see cref="Shell32.IShellFolderViewCB.MessageSFVCB"/> Callback Handler.
    /// </summary>
    public enum SFVMUD
    {
        SFVM_SELECTIONCHANGED = 8,
        SFVM_DRAWMENUITEM = 9,
        SFVM_MEASUREMENUITEM = 10,
        SFVM_EXITMENULOOP = 11,
        SFVM_VIEWRELEASE = 12,
        SFVM_GETNAMELENGTH = 13,

        SFVM_WINDOWCLOSING = 16,
        SFVM_LISTREFRESHED = 17,
        SFVM_WINDOWFOCUSED = 18,
        SFVM_REGISTERCOPYHOOK = 20,
        SFVM_COPYHOOKCALLBACK = 21,

        SFVM_ADDINGOBJECT = 29,
        SFVM_REMOVINGOBJECT = 30,

        SFVM_GETCOMMANDDIR = 33,
        SFVM_GETCOLUMNSTREAM = 34,
        SFVM_CANSELECTALL = 35,

        SFVM_ISSTRICTREFRESH = 37,
        SFVM_ISCHILDOBJECT = 38,

        SFVM_GETEXTVIEWS = 40,

        SFVM_GET_CUSTOMVIEWINFO = 77,
        SFVM_ENUMERATEDITEMS = 79,
        SFVM_GET_VIEW_DATA = 80,
        SFVM_GET_WEBVIEW_LAYOUT = 82,
        SFVM_GET_WEBVIEW_CONTENT = 83,
        SFVM_GET_WEBVIEW_TASKS = 84,
        SFVM_GET_WEBVIEW_THEME = 86,
        SFVM_GETDEFERREDVIEWSETTINGS = 92,
    }

#pragma warning restore CA1707 // Identifiers should not contain underscores

    #endregion ================================================================================================================

    #region ShellBrowserViewHandler ===========================================================================================

    /// <summary>
    /// Encapsulates an <see cref="Shell32.IShellFolderViewCB">IShellFolderViewCB</see>-Implementation within an
    /// <see cref="IDisposable"/>-Object. Beside that it's implemented as a Wrapper-Object that is responsible for
    /// creating and disposing the following objects aka Interface-Instances:<br/>
    /// - <seealso cref="Vanara.Windows.Shell.ShellFolder"/><br/>
    /// - <seealso cref="Shell32.IShellView"/><br/>
    /// - <seealso cref="Shell32.IFolderView2"/><br/>
    /// <br/>
    /// While doing that, it also handles some common error cases:<br/>
    /// - When there's no disk in a disk drive<br/>
    /// <br/>
    /// Implements the following Interfaces:<br/>
    /// - <seealso cref="Shell32.IShellFolderViewCB"/><br/>
    /// <br/>
    /// This class make use of some <see cref="SFVMUD">undocumented Messages</see> in its
    /// <see cref="Shell32.IShellFolderViewCB.MessageSFVCB"/> Callback Handler.<br/>
    /// <br/>
    /// For more Information on these see:<br/>
    /// - Google Drive Shell Extension: <seealso
    ///   href="https://github.com/google/google-drive-shell-extension/blob/master/DriveFusion/ShellFolderViewCBHandler.cpp">
    ///    ShellFolderViewCBHandler.cpp
    ///   </seealso><br/>
    /// - ReactOS: <seealso
    ///   href="https://doxygen.reactos.org/d2/dbb/IShellFolderViewCB_8cpp.html">
    ///    IShellFolderViewCB.cpp File Reference
    ///   </seealso>, <seealso
    ///   href="https://doxygen.reactos.org/d2/dbb/IShellFolderViewCB_8cpp_source.html">
    ///    IShellFolderViewCB.cpp
    ///   </seealso>
    /// </summary>
    public class ShellBrowserViewHandler
        : IDisposable
        , Shell32.IShellFolderViewCB
    {
        private bool isDisposed;
        public ShellBrowser Owner { get; }
        public ShellFolder ShellFolder { get; private set; }
        public Shell32.IShellView ShellView { get; private set; }
        public Shell32.IFolderView2 FolderView2 { get; private set; }
        public HWND ViewWindow { get; }
        public bool IsBrowsable { get; } = true;
        public bool NoDiskInDriveError { get; }
        public COMException ValidationError { get; private set; }

        /// <summary>
        /// <code>{"The operation was canceled by the user. (Exception from HRESULT: 0x800704C7)"}</code> is the result of a 
        /// call to <see cref="Shell32.IShellView.CreateViewWindow"/> on a Shell Item that targets a removable Disk Drive
        /// when currently no Media is present. Let's catch these to use our own error handling for this.
        /// </summary>
        internal static readonly HRESULT HResultOperationCanceled = new HRESULT(0x800704C7);

        /// <summary>
        /// Create an instance of <see cref="ShellBrowserViewHandler"/> to handle Callback messages for the given ShellFolder.
        /// </summary>
        /// <param name="owner">The <see cref="ShellBrowser"/> that is owner of this instance.</param>
        /// <param name="shellFolder"></param>
        /// <param name="folderViewMode"></param>
        /// <param name="folderFlags"></param>
        public ShellBrowserViewHandler(
            ShellBrowser owner,
            ShellFolder shellFolder,
            Shell32.FOLDERVIEWMODE folderViewMode = Shell32.FOLDERVIEWMODE.FVM_AUTO,
            Shell32.FOLDERFLAGS folderFlags = Shell32.FOLDERFLAGS.FWF_NOHEADERINALLVIEWS)
        {
            this.Owner = owner ?? throw new ArgumentNullException(nameof(owner));
            this.ShellFolder = shellFolder ?? throw new ArgumentNullException(nameof(shellFolder));

            // Try to create ShellView and FolderView2 objects, then its ViewWindow
            try
            {
                var sfvCreate = new Shell32.SFV_CREATE()
                {
                    cbSize = (uint)Marshal.SizeOf<Shell32.SFV_CREATE>(),
                    psfvcb = this,
                    pshf = shellFolder.IShellFolder,
                    psvOuter = null,
                };

                Shell32.SHCreateShellFolderView(ref sfvCreate, out Shell32.IShellView shellView).ThrowIfFailed();

                this.ShellView = shellView ??
                    throw new InvalidComObjectException(nameof(this.ShellView));

                var folderSettings = new Shell32.FOLDERSETTINGS(folderViewMode, folderFlags);
                this.FolderView2 = (Shell32.IFolderView2)this.ShellView ??                     // @dahall: Is this safe, or should I use the Marshaller for this?
                    throw new InvalidComObjectException(nameof(this.FolderView2));

                // Try to create ViewWindow and take special care of Exception
                // {"The operation was canceled by the user. (Exception from HRESULT: 0x800704C7)"}
                // cause this happens when there's no disk in a drive.
                try
                {
                    this.ViewWindow = this.ShellView.CreateViewWindow(null, folderSettings, owner, owner.ClientRectangle);
                }
                catch (COMException ex)
                {
                    if (HResultOperationCanceled.Equals(ex.ErrorCode))
                        this.NoDiskInDriveError = true;
                    throw;
                }
            }
            catch (COMException ex)
            {
                this.ValidationError = ex;
                this.IsBrowsable = false;
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.isDisposed)
                return;

            // Dispose managed resources
            if (disposing)
            {
                if (null != this.ShellFolder)
                {
                    this.ShellFolder.Dispose();
                    this.ShellFolder = null;
                }
            }

            // Dispose unmanaged resources
            if (null != this.ShellView)
            {
                this.ShellView.UIActivate(Shell32.SVUIA.SVUIA_DEACTIVATE);
                this.ShellView.DestroyViewWindow();
                Marshal.ReleaseComObject(this.ShellView);
                this.ShellView = null;
            }

            if (null != this.FolderView2)
            {
                Marshal.ReleaseComObject(this.FolderView2);
                this.FolderView2 = null;
            }

            this.isDisposed = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uMsg"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <param name="plResult"></param>
        /// <returns></returns>
        HRESULT Shell32.IShellFolderViewCB.MessageSFVCB(Shell32.SFVM uMsg, IntPtr wParam, IntPtr lParam, ref IntPtr plResult)
        {
            switch ((SFVMUD)uMsg)
            {
                case SFVMUD.SFVM_SELECTIONCHANGED:
                    AppContext.TraceDebug("...Selection Changed");
                    return HRESULT.S_OK;
                case SFVMUD.SFVM_LISTREFRESHED:
                    AppContext.TraceDebug("...List Refreshed");
                    return HRESULT.S_OK;
                default:
                    return HRESULT.E_NOTIMPL;
            }
        }
    }

    #endregion ================================================================================================================
}
