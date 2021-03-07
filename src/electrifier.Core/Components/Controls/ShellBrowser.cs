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
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Windows.Forms;

using Vanara.PInvoke;
using Vanara.Windows.Shell;

using Vanara.Collections; // For History
using static Vanara.PInvoke.Shell32; // For History
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Linq;
//using static Vanara.PInvoke.User32;

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
    /// TODO: internal static readonly bool IsMinVista = Environment.OSVersion.Version.Major >= 6;     // TODO: We use one interface, afaik, that only works in vista and above: IFolderView2
    /// TODO: Use COMReleaser: https://github.com/dahall/Vanara/blob/master/Core/InteropServices/ComReleaser.cs
    /// TODO: Windows 10' Quick Access folder has a special type of grouping, can't find out how this works yet.
    ///       As soon as we would be able to get all the available properties for an particular item, we would be able found out how this grouping works.
    ///       However, it seems to be a special group, since folders are Tiles, whereas files are shown in Details mode.
    /// NOTE: The grouping is done by 'Group'. Activate it using "Group by->More->Group", and then do the grouping.
    ///       However, the Icons for 'Recent Files'-Group get lost.
    /// TODO: Maybe this interface could help, too: https://docs.microsoft.com/en-us/windows/win32/shell/shellfolderview
    /// NOTE: This could help: https://answers.microsoft.com/en-us/windows/forum/windows_10-files-winpc/windows-10-quick-access-folders-grouped-separately/ecd4be4a-1847-4327-8c44-5aa96e0120b8
    /// TODO: BorderStyle? => Additionally border like the one on the adressbar of Edge
    /// TODO: IInputObject_WinForms in Vanara\Windows.Forms\Controls\ExplorerBrowser.cs
    /// TODO: FolderViewMode and FolderFlags in BrowseObject
    /// </summary>

    /// <summary>Indicates the viewing mode of the ShellBrowser</summary>
    public enum ShellBrowserViewMode
    {
        /// <summary>Choose the best view mode for the folder</summary>
        Auto = Shell32.FOLDERVIEWMODE.FVM_AUTO,

        /// <summary>(New for Windows7)</summary>
        Content = Shell32.FOLDERVIEWMODE.FVM_CONTENT,

        /// <summary>Object names and other selected information, such as the size or date last updated, are shown.</summary>
        Details = Shell32.FOLDERVIEWMODE.FVM_DETAILS,

        /// <summary>The view should display medium-size icons.</summary>
        Icon = Shell32.FOLDERVIEWMODE.FVM_ICON,

        /// <summary>Object names are displayed in a list view.</summary>
        List = Shell32.FOLDERVIEWMODE.FVM_LIST,

        /// <summary>The view should display small icons.</summary>
        SmallIcon = Shell32.FOLDERVIEWMODE.FVM_SMALLICON,

        /// <summary>The view should display thumbnail icons.</summary>
        Thumbnail = Shell32.FOLDERVIEWMODE.FVM_THUMBNAIL,

        /// <summary>The view should display icons in a filmstrip format.</summary>
        ThumbStrip = Shell32.FOLDERVIEWMODE.FVM_THUMBSTRIP,

        /// <summary>The view should display large icons.</summary>
        Tile = Shell32.FOLDERVIEWMODE.FVM_TILE
    }

    public class ShellBrowserNavigatedEventArgs : EventArgs
    {
        public ShellFolder CurrentFolder { get; }

        public ShellBrowserNavigatedEventArgs(ShellFolder currentFolder)
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
    /// - TODO: CustomDraw, when currently no shellView available<br/>
    /// - DONE: Network folder: E_FAIL => DONE: Returning HRESULT.E_NOTIMPL from MessageSFVCB fixes this<br/>
    /// - DONE: Disk Drive (empty): E_CANCELLED_BY_USER<br/>
    /// </summary>
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    [Guid("B8B0F852-9527-4CA8-AB1D-648AE95B618E")]
    public class ShellBrowser
        : UserControl
        , IWin32Window
        , Shell32.IShellBrowser
        , Shell32.IServiceProvider
    {
        protected ShellBrowserViewHandler ViewHandler { get; private set; }

        internal FOLDERSETTINGS folderSettings = new FOLDERSETTINGS(FOLDERVIEWMODE.FVM_AUTO, defaultFolderFlags);

        private const FOLDERFLAGS defaultFolderFlags = FOLDERFLAGS.FWF_NOHEADERINALLVIEWS | FOLDERFLAGS.FWF_USESEARCHFOLDER | FOLDERFLAGS.FWF_NOWEBVIEW;
        internal const int defaultThumbnailSize = 32;

        #region Properties =====================================================================================================

        /// <inheritdoc/>
        protected override Size DefaultSize => new Size(200, 150);

        private string emptyFolderText = "This folder is empty.";

        /// <summary>The default text that is displayed when an empty folder is shown</summary>
        [Category("Appearance"), DefaultValue("This folder is empty."), Description("The default text that is displayed when an empty folder is shown.")]
        public string EmptyFolderText
        {
            get => this.emptyFolderText;
            set
            {
                this.emptyFolderText = value;

                if (this.ViewHandler.IsValid)
                    this.ViewHandler.Text = value;
            }
        }

        /// <summary>Contains the navigation history of the ShellBrowser</summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ShellNavigationHistory History { get; private set; }

        /// <summary>The set of ShellItems in the ShellBrowser</summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IReadOnlyList<ShellItem> Items { get; }

        /// <summary>The set of selected ShellItems in the ShellBrowser</summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IReadOnlyList<ShellItem> SelectedItems { get; }

        /// <summary>The size of the thumbnails in pixels.</summary>
        [Category("Appearance"), DefaultValue(defaultThumbnailSize), Description("The size of the thumbnails in pixels.")]
        public int ThumbnailSize
        {
            get => (this.ViewHandler.IsValid) ? this.ViewHandler.ThumbnailSize : defaultThumbnailSize;
            set
            {
                if (this.ViewHandler.IsValid)
                    this.ViewHandler.ThumbnailSize = value;
            }
        }

        /// <summary>The viewing mode of the ShellBrowser</summary>
        [Category("Appearance"), DefaultValue(typeof(ShellBrowserViewMode), "Auto"), Description("The viewing mode of the ShellBrowser.")]
        public ShellBrowserViewMode ViewMode
        {
            get => (ShellBrowserViewMode)this.folderSettings.ViewMode;
            set
            {
                // TODO: Set ThumbnailSize accordingly?
                this.folderSettings.ViewMode = (FOLDERVIEWMODE)value;

                if (this.ViewHandler.IsValid)
                    this.ViewHandler.ViewMode = this.folderSettings.ViewMode;
            }
        }

        #endregion ============================================================================================================

        #region Published Events ==============================================================================================

        /// <summary>Fires when the Items collection changes.</summary>
        [Category("Action"), Description("Items changed.")]
        public event EventHandler ItemsChanged;

        /// <summary>Fires when ShellBrowser has navigated to a new folder.</summary>
        [Category("Action"), Description("ShellBowser has navigated to a new folder.")]
        public event EventHandler<ShellBrowserNavigatedEventArgs> Navigated;

        /// <summary>Fires when the SelectedItems collection changes.</summary>
        [Category("Behavior"), Description("Selection changed.")]
        public event EventHandler SelectionChanged;

        #endregion ============================================================================================================


        public ShellBrowser()
            : base()
        {
            this.InitializeComponent();

            this.History = new ShellNavigationHistory();

            this.Items = new ShellItemCollection(this, SVGIO.SVGIO_ALLVIEW);
            this.SelectedItems = new ShellItemCollection(this, SVGIO.SVGIO_SELECTION);

            this.Resize += this.ShellBrowser_Resize;
        }

        protected internal virtual void ShellBrowser_Resize(object sender, EventArgs e)
        {
            this.ViewHandler?.MoveWindow(0, 0, this.ClientRectangle.Width, this.ClientRectangle.Height, false);
        }

        /// <summary>Gets the items in the ShellBrowser as an IShellItemArray</summary>
        /// <returns>An <see cref="IShellItemArray"/> instance or <see langword="null"/> if not available.</returns>
        internal IShellItemArray GetItemsArray(SVGIO opt)
        {
            try
            {
                var viewHandler = this.ViewHandler.Validated();

                if (viewHandler != null)
                    return viewHandler.FolderView2.Items<IShellItemArray>(opt);
                else
                    return null;
            }
            catch { return null; }
        }

        // TODO: ViewMode-Property, Thumbnailsize => Set ThumbnailSize for Large, ExtraLarge, etc.

        // TODO: AppContext.Trace* removal

        // TODO: Keyboard-Handling

        // TODO: BrowseObject ->Parent -> Relative


        /// <summary>Selects all items in the current view.</summary>
        public void SelectAll()
        {
            // TODO: To speed things up, try workaround #2: https://stackoverflow.com/questions/9039989/how-to-selectall-in-a-winforms-virtual-listview

            var viewHandler = this.ViewHandler.Validated();

            if (viewHandler != null)
            {
                for (var i = 0; i < viewHandler.FolderView2.ItemCount(SVGIO.SVGIO_ALLVIEW); i++)
                    viewHandler.FolderView2.SelectItem(i, SVSIF.SVSI_SELECT);
            }
        }

        /// <summary>Unselects all items in the current view.</summary>
        public void UnselectAll()
        {
            var viewHandler = this.ViewHandler.Validated();

            if (viewHandler != null)
                viewHandler.FolderView2.SelectItem(-1, SVSIF.SVSI_DESELECTOTHERS);
        }

        /// <summary>Raises the <see cref="ItemsChanged"/> event.</summary>
        protected internal virtual void OnItemsChanged() => this.ItemsChanged?.Invoke(this, EventArgs.Empty);

        /// <summary>Raises the <see cref="Navigated"/> event.</summary>
        protected internal virtual void OnNavigated(ShellFolder shellFolder)
        {
            if (null != this.Navigated)
            {
                ShellBrowserNavigatedEventArgs eventArgs = new ShellBrowserNavigatedEventArgs(shellFolder);

                this.Navigated.Invoke(this, eventArgs);
            }
        }

        /// <summary>Raises the <see cref="SelectionChanged"/> event.</summary>
        protected internal virtual void OnSelectionChanged() => this.SelectionChanged?.Invoke(this, EventArgs.Empty);

        /// <summary>
        /// Navigates to the last item in the navigation history list. This does not change the set of locations in the navigation log.
        /// </summary>
        /// <returns>True if the navigation succeeded, false if it failed for any reason.</returns>
        public bool NavigateBack()
        {
            return this.BrowseObject(IntPtr.Zero, Shell32.SBSP.SBSP_NAVIGATEBACK).Succeeded;
        }

        /// <summary>
        /// Navigates to the next item in the navigation history list. This does not change the set of locations in the navigation log.
        /// </summary>
        /// <returns>True if the navigation succeeded, false if it failed for any reason.</returns>
        public bool NavigateForward()
        {
            return this.BrowseObject(IntPtr.Zero, Shell32.SBSP.SBSP_NAVIGATEFORWARD).Succeeded;
        }



        /// <summary>
        /// Navigate within the navigation log in a specific direciton. This does not change the set of locations in the navigation log.
        /// </summary>
        /// <param name="direction">The direction to navigate within the navigation logs collection.</param>
        /// <returns>True if the navigation succeeded, false if it failed for any reason.</returns>
//        public bool NavigateFromHistory(NavigationLogDirection direction) => History.NavigateLog(direction);

        /// <summary>Navigate within the navigation log. This does not change the set of locations in the navigation log.</summary>
        /// <param name="historyIndex">An index into the navigation logs Locations collection.</param>
        /// <returns>True if the navigation succeeded, false if it failed for any reason.</returns>
        public bool NavigateToHistoryIndex(int historyIndex)
        {
            using (ShellItem shellFolder = this.History.Seek(historyIndex, SeekOrigin.Current))
            {
                if (shellFolder != null)
                    return this.BrowseObject((IntPtr)shellFolder.PIDL, SBSP.SBSP_ABSOLUTE | SBSP.SBSP_WRITENOHISTORY).Succeeded;
            }

            return false;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pidl"></param>
        /// <param name="wFlags"></param>
        /// <returns>HRESULT.STG_E_PATHNOTFOUND if path n found</returns>
        public HRESULT BrowseObject(IntPtr pidl, Shell32.SBSP wFlags)
        {
            ShellItem shellObject = null;

            //
            // The given PIDL equals Desktop, so ignore the other flags
            //
            if (ShellFolder.Desktop.PIDL.Equals(pidl))
            {
                shellObject = new ShellItem(ShellFolder.Desktop.PIDL);
            }

            //
            // SBSP_NAVIGATEBACK means the last item in the navigation history list
            //
            else if (wFlags.HasFlag(Shell32.SBSP.SBSP_NAVIGATEBACK))
            {
                if (this.History.CanSeekBackward)
                    shellObject = this.History.SeekBackward();
                else
                    return HRESULT.STG_E_PATHNOTFOUND;
            }

            //
            // SBSP_NAVIGATEFORWARD means the next item in the navigation history list
            //
            else if (wFlags.HasFlag(Shell32.SBSP.SBSP_NAVIGATEFORWARD))
            {
                if (this.History.CanSeekForward)
                    shellObject = this.History.SeekForward();
                else
                    return HRESULT.STG_E_PATHNOTFOUND;
            }

            else if (wFlags.HasFlag(Shell32.SBSP.SBSP_RELATIVE))            // pidl is relative to the current fodler
            {


                //// SBSP_RELATIVE - pidl is relative from the current folder
                //if ((hr = m_currentFolder.BindToObject(pidl, IntPtr.Zero, ref NativeMethods.IID_IShellFolder,
                //                                       out folderTmpPtr)) != NativeMethods.S_OK)
                //    return hr;
                //pidlTmp = NativeMethods.Shell32.ILCombine(m_pidlAbsCurrent, pidl);
                //folderTmp = (NativeMethods.IShellFolder)Marshal.GetObjectForIUnknown(folderTmpPtr);
            }

            else if (wFlags.HasFlag(Shell32.SBSP.SBSP_PARENT))              // browse to parent folder and ignore the pidl
            {

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

            //
            // SBSP_ABSOLUTE as the remaining option means an absolute pidl is given
            //
            else
            {
                // Remember we are not the owner of this pidl, so clone it to have our own copy on the heap.
                shellObject = new ShellItem(new PIDL(pidl, true));
            }

            this.UIThreadAsync(delegate
            {
                var viewHandler = new ShellBrowserViewHandler(this,
                    new ShellFolder(shellObject),
                    ref this.folderSettings,
                    ref this.emptyFolderText);

//                if (viewHandler.Validated() is null)
//                    return;

                // Clone the PIDL, to have our own object on the heap
                var newPIDL = new PIDL(viewHandler.ShellFolder.PIDL);

                this.History.Add(newPIDL);

                var oldViewHandler = this.ViewHandler;
                this.ViewHandler = viewHandler;
                oldViewHandler?.UIDeactivate();
                viewHandler.UIActivate();
                oldViewHandler?.DestroyView();

                this.OnNavigated(viewHandler.ShellFolder);      // TODO: Move this call above, to show folder in Adress Bar BEFORE enumerating?
                this.OnSelectionChanged();
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
            if (this.ViewHandler.Validated() != null)
            {
                Marshal.AddRef(Marshal.GetIUnknownForObject(this.ViewHandler.ShellView));
                shellView = this.ViewHandler.ShellView;

                return HRESULT.S_OK;
            }

            shellView = null;
            return HRESULT.E_PENDING;
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
                ShellBrowserViewHandler shvwHandler = this.ViewHandler.Validated();

                if (!(shvwHandler is null))
                {
                    ppvObject = Marshal.GetComInterfaceForObject(shvwHandler, typeof(Shell32.IShellFolderViewCB));

                    return HRESULT.S_OK;
                }
            }

            ppvObject = IntPtr.Zero;
            return HRESULT.E_NOINTERFACE;
        }

        #endregion ============================================================================================================



        #region ShellItemCollection ===========================================================================================

        /// <summary>
        /// Represents a collection of <see cref="ShellItem"/> attached to an <see cref="ShellBrowser"/>.
        /// </summary>
        private class ShellItemCollection : IReadOnlyList<ShellItem>
        {
            private readonly ShellBrowser shellBrowser;
            private readonly SVGIO option;

            internal ShellItemCollection(ShellBrowser shellBrowser, SVGIO opt)
            {
                this.shellBrowser = shellBrowser;
                this.option = opt;
            }

            /// <summary>Gets the number of elements in the collection.</summary>
            /// <value>Returns a <see cref="int"/> value.</value>
            public int Count
            {
                get
                {
                    var viewHandler = this.shellBrowser.ViewHandler.Validated();

                    if (viewHandler != null)
                        return viewHandler.FolderView2.ItemCount(this.option);

                    return 0;
                }
            }

            private IShellItemArray Array => this.shellBrowser.GetItemsArray(this.option);

            private IEnumerable<IShellItem> Items
            {
                get
                {
                    var array = this.Array;

                    if (array is null)
                        yield break;
                    try
                    {
                        for (uint i = 0; i < array.GetCount(); i++)
                            yield return array.GetItemAt(i);
                    }
                    finally
                    {
                        Marshal.ReleaseComObject(array);
                    }
                }
            }

            /// <summary>Gets the <see cref="ShellItem"/> at the specified index.</summary>
            /// <value>The <see cref="ShellItem"/>.</value>
            /// <param name="index">The zero-based index of the element to get.</param>
            public ShellItem this[int index]
            {
                get
                {
                    var array = Array;
                    try
                    {
                        return array is null ? null : ShellItem.Open(array.GetItemAt((uint)index));
                    }
                    catch
                    {
                        return null;
                    }
                    finally
                    {
                        if (array != null)
                            Marshal.ReleaseComObject(array);
                    }
                }
            }

            /// <summary>Returns an enumerator that iterates through the collection.</summary>
            /// <returns>An enumerator that can be used to iterate through the collection.</returns>
            public IEnumerator<ShellItem> GetEnumerator() => Items.Select(ShellItem.Open).GetEnumerator();

            /// <summary>Returns an enumerator that iterates through the collection.</summary>
            /// <returns>An enumerator that can be used to iterate through the collection.</returns>
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
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
        SFVM_ENUMERATEDITEMS = 79,                  // It seems this msg never gets sent, using Win 10 at least.
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
        : Shell32.IShellFolderViewCB
    {
        private string text;
        private int thumbnailSize = ShellBrowser.defaultThumbnailSize;

        /// <summary>
        /// <code>{"The operation was canceled by the user. (Exception from HRESULT: 0x800704C7)"}</code> is the result of a 
        /// call to <see cref="Shell32.IShellView.CreateViewWindow"/> on a Shell Item that targets a removable Disk Drive
        /// when currently no Media is present. Let's catch these to use our own error handling for this.
        /// </summary>
        internal static readonly HRESULT HRESULT_CANCELLED = new HRESULT(0x800704C7);

        #region Properties ====================================================================================================

        public ShellBrowser Owner { get; }
        public ShellFolder ShellFolder { get; private set; }
        public Shell32.IShellView ShellView { get; private set; }
        public Shell32.IFolderView2 FolderView2 { get; private set; }
        public HWND ViewWindow { get; private set; }
        public bool IsValid { get; private set; }
        public bool NoDiskInDriveError { get; }
        public COMException ValidationError { get; private set; }

        /// <summary>The default text to be used when there are no items in the view.</summary>
        public string Text
        {
            get => this.text;

            set
            {
                this.text = value;

                if (this.IsValid)
                    this.FolderView2.SetText(Shell32.FVTEXTTYPE.FVST_EMPTYTEXT, value);
            }
        }

        /// <summary>The size of the thumbnails in pixels.</summary>
        public int ThumbnailSize
        {
            get
            {
                if (this.IsValid)
                    this.FolderView2.GetViewModeAndIconSize(out _, out this.thumbnailSize);

                return this.thumbnailSize;
            }
            set
            {
                if (this.IsValid)
                {
                    this.FolderView2.GetViewModeAndIconSize(out var fvm, out _);
                    this.FolderView2.SetViewModeAndIconSize(fvm, this.thumbnailSize = value);
                }
            }
        }

        /// <summary>The viewing mode of the ShellBrowser.</summary>
        public Shell32.FOLDERVIEWMODE ViewMode
        {
            get
            {
                if (this.IsValid)
                {
                    return this.FolderView2.GetCurrentViewMode();
                }

                return Shell32.FOLDERVIEWMODE.FVM_AUTO;     // TODO!
            }
            set
            {
                if (this.IsValid)
                    this.FolderView2.SetCurrentViewMode(value);
            }
        }

        #endregion ============================================================================================================

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
            ref Shell32.FOLDERSETTINGS folderSettings,
            ref string emptyFolderText)
        {
            this.Owner = owner ?? throw new ArgumentNullException(nameof(owner));
            this.ShellFolder = shellFolder ?? throw new ArgumentNullException(nameof(shellFolder));

            // Create ShellView and FolderView2 objects, then its ViewWindow
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

                this.FolderView2 = (Shell32.IFolderView2)this.ShellView ??
                    throw new InvalidComObjectException(nameof(this.FolderView2));

                // Try to create ViewWindow and take special care of Exception
                // {"The operation was canceled by the user. (Exception from HRESULT: 0x800704C7)"}
                // cause this happens when there's no disk in a drive.
                try
                {
                    this.ViewWindow = this.ShellView.CreateViewWindow(null, folderSettings, owner, owner.ClientRectangle);

                    this.IsValid = true;

                    this.Text = emptyFolderText;
                }
                catch (COMException ex)
                {
                    // TODO: Check if the target folder IS actually a drive with removable disks in it!
                    if (HRESULT_CANCELLED.Equals(ex.ErrorCode))
                        this.NoDiskInDriveError = true;
                    throw;
                }
            }
            catch (COMException ex)
            {
                // TODO: e.g. C:\Windows\CSC => Permission denied!
                // 0x8007 0005 E_ACCESSDENIED
                this.ValidationError = ex;
            }
        }

        public void DestroyView()
        {
            this.IsValid = false;

            // TODO: Remove MessageSFVCB here!

            // Destroy ShellView's ViewWindow
            this.ViewWindow = HWND.NULL;
            this.ShellView.DestroyViewWindow();

            this.FolderView2 = null;
            this.ShellView = null;
            //this.ShellFolder = null;      // NOTE: I >>think<< this one causes RPC-Errors
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
                    this.Owner.OnSelectionChanged();
                    return HRESULT.S_OK;

                case SFVMUD.SFVM_LISTREFRESHED:
                    this.Owner.OnItemsChanged();
                    return HRESULT.S_OK;

                default:
                    //
                    // TODO: What happens when the ViewMode gets changed via Context-Menu? => Msg #33, #18
                    //
                    return HRESULT.E_NOTIMPL;
            }
        }

        /// <summary>Changes the position and dimensions of this <see cref="ViewWindow"/>.</summary>
        /// <param name="bRepaint">Force redraw</param>
        /// <returns>If the function succeeds, the return value is nonzero.</returns>
        public bool MoveWindow(int X, int Y, int nWidth, int nHeight, bool bRepaint) =>
            this.ViewWindow != HWND.NULL && User32.MoveWindow(this.ViewWindow, X, Y, nWidth, nHeight, bRepaint);

        /// <summary>Activate the ShellView of this ShellBrowser.</summary>
        /// <param name="uState">The <seealso cref="Shell32.SVUIA"/> to be set</param>
        public void UIActivate(Shell32.SVUIA uState = Shell32.SVUIA.SVUIA_ACTIVATE_NOFOCUS) => this.ShellView?.UIActivate(uState);

        /// <summary>Deactivate the ShellView of this ShellBrowser.</summary>
        public void UIDeactivate() => this.UIActivate(Shell32.SVUIA.SVUIA_DEACTIVATE);
    }

    /// <summary>
    /// Extension methods for <see cref="ShellBrowserViewHandler"/>.
    /// </summary>
    public static class ShellBrowserViewHandlerExtension
    {
        public static ShellBrowserViewHandler Validated(this ShellBrowserViewHandler shellBrowserViewHandler) =>
            ((!(shellBrowserViewHandler is null)) && shellBrowserViewHandler.IsValid) ? shellBrowserViewHandler : null;
    }


    #endregion ================================================================================================================



    #region temporary ShellNavigationHistory class

    /// <summary>The navigation log is a history of the locations visited by a shell view object.</summary>
    public class ShellNavigationHistory : IHistory<ShellItem>
    {
        private readonly History<PIDL> pidls = new History<PIDL>();

        internal ShellNavigationHistory()
        {
            pidls.CollectionChanged += (s, e) => CollectionChanged?.Invoke(this, e);
            pidls.PropertyChanged += (s, e) => PropertyChanged?.Invoke(this, e);
        }

        /// <summary>Occurs when an item is added, removed, changed, moved, or the entire list is refreshed.</summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        /// <summary>Occurs when a property value changes.</summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>Indicates the presence of items in the history that can be reached by calling <see cref="M:Vanara.Collections.IHistory`1.SeekBackward"/>.</summary>
        /// <value><see langword="true"/> if this instance can seek backward; otherwise, <see langword="false"/>.</value>
        public bool CanSeekBackward => pidls.CanSeekBackward;

        /// <summary>Indicates the presence of items in the history that can be reached by calling <see cref="M:Vanara.Collections.IHistory`1.SeekForward"/>.</summary>
        /// <value><see langword="true"/> if this instance can seek forward; otherwise, <see langword="false"/>.</value>
        public bool CanSeekForward => pidls.CanSeekForward;

        /// <summary>Gets the items in the history.</summary>
        /// <value>The number of items.</value>
        public int Count => pidls.Count;

        /// <summary>Gets the shell object in the Locations collection pointed to by CurrentLocationIndex.</summary>
        public ShellItem Current => new ShellItem(pidls.Current);

        /// <summary>
        /// Adds the specified item as the last history entry and sets the <see cref="P:Vanara.Collections.IHistory`1.Current"/> property to
        /// it's value.
        /// </summary>
        /// <param name="item">The item to add to the history.</param>
        public void Add(ShellItem item) => pidls.Add(item.PIDL, true);

        /// <summary>Clears the history of all items.</summary>
        public void Clear() => pidls.Clear();

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>A <see cref="IEnumerator{T}"/> that can be used to iterate through the collection.</returns>
        public IEnumerator<ShellItem> GetEnumerator() => pidls.Select(p => new ShellItem(p)).GetEnumerator();

        /// <summary>Gets a specified number of items starting at a location within the history.</summary>
        /// <param name="count">The maximum number of items to retrieve. The actual number of items returned may be less if not avaialable.</param>
        /// <param name="origin">The reference point within the history at which to start fetching items.</param>
        /// <returns>A read-only list of items.</returns>
        public IReadOnlyList<ShellItem> GetItems(int count, SeekOrigin origin) => (IReadOnlyList<ShellItem>)new List<ShellItem>(pidls.GetItems(count, origin).Select(ShIFromPIDL));

        /// <summary>
        /// Seeks through the history a given number of items starting at a known location within the history. This updates the <see
        /// cref="P:Vanara.Collections.IHistory`1.Current"/> property.
        /// </summary>
        /// <param name="count">The number of items to move. This value can be negative to search backwards or positive to search forwards.</param>
        /// <param name="origin">The reference point within the history at which to start seeking.</param>
        /// <returns>The value at the new current pointer position.</returns>
        public ShellItem Seek(int count, SeekOrigin origin) => ShIFromPIDL(pidls.Seek(count, origin));

        /// <summary>Seeks one position backwards.</summary>
        /// <returns>The value at the new current pointer position.</returns>
        public ShellItem SeekBackward() => ShIFromPIDL(pidls.SeekBackward());

        /// <summary>Seeks one position forwards.</summary>
        /// <returns>The value at the new current pointer position.</returns>
        public ShellItem SeekForward() => ShIFromPIDL(pidls.SeekForward());

        /// <summary>
        /// Adds the specified item as the last history entry and sets the <see cref="P:Vanara.Collections.IHistory`1.Current"/> property to
        /// it's value.
        /// </summary>
        /// <param name="item">The item to add to the history.</param>
        /// <param name="removeForwardItems">
        /// <see langword="true"/> indicates to remove all items forward of the current pointer; <see langword="false"/> leaves the history intact.
        /// </param>
        void IHistory<ShellItem>.Add(ShellItem item, bool removeForwardItems) => pidls.Add(item?.PIDL, removeForwardItems);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        internal void Add(PIDL pidl) => pidls.Add(pidl, true);

        private static ShellItem ShIFromPIDL(PIDL pidl) => pidl is null ? null : new ShellItem(pidl);
    }

    #endregion
}
