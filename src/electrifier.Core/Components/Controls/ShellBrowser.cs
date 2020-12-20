using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Windows.Forms;
using Vanara.Extensions;
using Vanara.PInvoke;
using static Vanara.PInvoke.Shell32;

namespace Vanara.Windows.Shell
{
	/// <summary>A basic implementation of IShellBrowser, IOleCommandTarget and ICommDlgBrowser.</summary>
	/// <remarks>
	///   <para>This implementation used a <see cref="ShellView" /> to implement:</para>
	///   <list type="bullet">
	///     <item>BrowseObject</item>
	///     <item>GetWindow</item>
	///     <item>OnDefaultCommand</item>
	///     <item>OnStateChange</item>
	///   </list>
	/// </remarks>
	/// <seealso cref="IShellBrowser" />
	/// <seealso cref="IOleCommandTarget" />
	/// <seealso cref="Shell32.IServiceProvider" />
	/// <seealso cref="ICommDlgBrowser" />
	[ComVisible(true), ClassInterface(ClassInterfaceType.None)]
	public class ShellBrowser : UserControl, IShellBrowser, IOleCommandTarget, Shell32.IServiceProvider, ICommDlgBrowser
	{
		internal HWND shellViewWindow;
		private ShellFolder currentFolder;
		private IShellView iShellView;


		/// <summary>The <see cref="ShellView"/> instance from initialization.</summary>
		//protected ShellView shellView;		// TODO: Immer null!

		/// <summary>Occurs when a property value changes.</summary>
		[Category("Behavior"), Description("Property changed.")]
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>Fires when a navigation has been initiated, but is not yet complete.</summary>
		[Category("Action"), Description("Navigation initiated, but not complete.")]
		public event EventHandler<NavigatingEventArgs> Navigating;

		/// <summary>
		/// Fires when a navigation has been 'completed': no Navigating listener has canceled, and the ExplorerBorwser has created a new
		/// view. The view will be populated with new items asynchronously, and ItemsChanged will be fired to reflect this some time later.
		/// </summary>
		[Category("Action"), Description("Navigation complete.")]
		public event EventHandler<NavigatedEventArgs> Navigated;


		/// <summary>Initializes a new instance of the <see cref="ShellBrowser"/> class with a <see cref="ShellView"/> instance.</summary>
		/// <param name="view">The <see cref="ShellView"/> instance.</param>
		/// <exception cref="ArgumentNullException">view</exception>
		//		public ShellBrowser(ShellView view) => shellView = view ?? throw new ArgumentNullException(nameof(view));
		public ShellBrowser()
		{

		}

		/// <summary>Gets or sets the <see cref="ShellFolder"/> currently being browsed by the <see cref="ShellView"/>.</summary>
		[Category("Data"), DefaultValue(null), Description("The folder currently being browsed.")]
		public ShellFolder CurrentFolder
		{
			get => currentFolder; //??= IShellView is null ? ShellFolder.Desktop : new ShellFolder(GetFolderForView(IShellView));
			set => Navigate(value);
		}

		/// <summary>Gets the underlying <see cref="IShellView"/> instance.</summary>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public IShellView IShellView
		{
			get => iShellView;
			private set
			{
				iShellView = value;
				//Items = new ShellItemArray(GetItemArray(iShellView, SVGIO.SVGIO_ALLVIEW));
			}
		}

		/// <summary>Folder view mode.</summary>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public FOLDERVIEWMODE ViewMode => IShellView.GetCurrentInfo().ViewMode;

		/// <summary>A set of flags that indicate the options for the folder.</summary>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public FOLDERFLAGS Flags => IShellView.GetCurrentInfo().fFlags;



		/// <summary>
		/// Clears the view of existing content, fills it with content from the specified container, and adds a new item to the history.
		/// </summary>
		/// <param name="folder">The shell folder to navigate to.</param>
		public void Navigate(ShellFolder folder)
		{
			if (folder is null)
				return;

			if (!OnNavigating(folder)) return;

            ShellFolder previous = currentFolder;
            currentFolder = folder;

            try
            {
                RecreateShellView();
                //History.Add(folder.PIDL);
                OnNavigated();
            }
            catch (Exception)
            {
                currentFolder = previous;
                RecreateShellView();
                throw;
            }
            OnPropertyChanged(nameof(CurrentFolder));
        }


		/// <summary>Raises the <see cref="Navigated"/> event.</summary>
		protected internal virtual void OnNavigated() => Navigated?.Invoke(this, new NavigatedEventArgs(CurrentFolder));


		private void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

		/// <summary>Raises the <see cref="Navigating"/> event.</summary>
		protected internal virtual bool OnNavigating(ShellFolder pendingLocation)
		{
			var e = new NavigatingEventArgs(pendingLocation);
			Navigating?.Invoke(this, e);
			return !e.Cancel;
		}

		private void RecreateShellView()
		{
//			if (IShellView != null)	// taj: why?!?
			{
				CreateShellView();
				OnNavigated();
			}
		}

		//private static IShellView CreateViewObject(ShellFolder folder, HWND owner) =>		// 20.12. taj
		//	folder?.iShellFolder.CreateViewObject<IShellView>(owner);
		private static IShellView CreateViewObject(ShellFolder folder, HWND owner)
		{
			return folder?.IShellFolder.CreateViewObject<IShellView>(owner);
		}

		private void CreateShellView()
		{
			IShellView prev = IShellView;
			IShellView = CreateViewObject(CurrentFolder, Handle);

			iShellView = IShellView; // taj, HACK
			//shellView = (ShellView)IShellView;	// 20.12.

			try
			{
				var fsettings = new FOLDERSETTINGS(FOLDERVIEWMODE.FVM_AUTO, FOLDERFLAGS.FWF_AUTOARRANGE); /*prev?.GetCurrentInfo() ?? new FOLDERSETTINGS(ViewMode, Flags);*/		// BUG! => viewmode und flags kommen ja vom prev.
				shellViewWindow = IShellView.CreateViewWindow(prev, fsettings, /*Browser*/ this, ClientRectangle);
			}
			catch (COMException ex)
			{
				// If the operation was cancelled by the user (for example because an empty removable media drive was selected, then
				// "Cancel" pressed in the resulting dialog) convert the exception into something more meaningfil.
				if (ex.ErrorCode == unchecked((int)0x800704C7U))
				{
					throw new OperationCanceledException("User cancelled.", ex);
				}
			}

			if (prev != null)
			{
				prev.UIActivate(SVUIA.SVUIA_DEACTIVATE);
				prev.DestroyViewWindow();
				Marshal.ReleaseComObject(prev);
			}

			IShellView.UIActivate(SVUIA.SVUIA_ACTIVATE_NOFOCUS);

			if (DesignMode) User32.EnableWindow(shellViewWindow, false);
		}


		/// SNIP: Old Code following =====================================================================================================================================================================================================================================





		/// <summary>Gets or sets the progress bar associated with the view.</summary>
		/// <value>The progress bar.</value>
		public ProgressBar ProgressBar { get; set; }

#if NETFRAMEWORK || NETCOREAPP3_0
		/// <summary>Gets or sets the status bar associated with the view.</summary>
		/// <value>The status bar.</value>
		public StatusBar StatusBar { get; set; }

		/// <summary>Gets or sets the tool bar associated with the view.</summary>
		/// <value>The tool bar.</value>
		public ToolBar ToolBar { get; set; }
#endif

		/// <summary>Gets or sets the TreeView associated with the view.</summary>
		/// <value>The TreeView.</value>
		public TreeView TreeView { get; set; }

		/// <inheritdoc/>
		public virtual HRESULT BrowseObject(IntPtr pidl, SBSP wFlags)
		{
			switch (wFlags)
			{
				case var f when f.IsFlagSet(SBSP.SBSP_NAVIGATEBACK):
					//shellView.NavigateBack(); // 20.12.
					break;
				case var f when f.IsFlagSet(SBSP.SBSP_NAVIGATEFORWARD):
					//shellView.NavigateForward();	// 20.12.
					break;
				case var f when f.IsFlagSet(SBSP.SBSP_PARENT):
					//shellView.NavigateParent();	// 20.12.
					break;
				case var f when f.IsFlagSet(SBSP.SBSP_RELATIVE):
					//if (ShellItem.Open(shellView.CurrentFolder.IShellFolder, pidl) is ShellFolder sf) 20.12.
					//	shellView.Navigate(sf);
					break;
				default:
					//shellView.Navigate(new ShellFolder(pidl));		// 20.12.

					break;
			}
			return HRESULT.S_OK;
		}

		/// <inheritdoc/>
		public virtual HRESULT ContextSensitiveHelp(bool fEnterMode) => HRESULT.E_NOTIMPL;

		/// <inheritdoc/>
		public virtual HRESULT EnableModelessSB(bool fEnable) => HRESULT.E_NOTIMPL;

		/// <inheritdoc/>
		public virtual HRESULT Exec(in Guid pguidCmdGroup, uint nCmdID, uint nCmdexecopt, in object pvaIn, ref object pvaOut) => HRESULT.E_NOTIMPL;

		/// <inheritdoc/>
		public virtual HRESULT GetControlWindow(FCW id, out HWND phwnd)
		{
			/* taj 20.12.
			phwnd = id switch
			{
				FCW.FCW_PROGRESS => CheckAndLoad(ProgressBar),
#if NETFRAMEWORK || NETCOREAPP3_0
				FCW.FCW_STATUS => CheckAndLoad(StatusBar),
				FCW.FCW_TOOLBAR => CheckAndLoad(ToolBar),
#endif
				FCW.FCW_TREE => CheckAndLoad(TreeView),
				_ => HWND.NULL,
			};
			return phwnd.IsNull ? HRESULT.E_NOTIMPL : HRESULT.S_OK;

			static HWND CheckAndLoad(Control c) => c != null && c.IsHandleCreated ? c.Handle : HWND.NULL;
			*/
			phwnd = HWND.NULL;
			return HRESULT.E_NOTIMPL;
		}

		/// <inheritdoc/>
		public virtual HRESULT GetViewStateStream(STGM grfMode, out IStream ppStrm)
		{
			ppStrm = null;
			return HRESULT.E_NOTIMPL;
		}

		/// <inheritdoc/>
		public virtual HRESULT GetWindow(out HWND phwnd)
		{
			phwnd = this.Handle;	/* taj shellView.shellViewWindow */
			return HRESULT.S_OK;
		}

		/// <inheritdoc/>
		public virtual HRESULT IncludeObject(IShellView ppshv, IntPtr pidl) => HRESULT.S_OK; //=> shellView.IncludeItem(pidl) ? HRESULT.S_OK : HRESULT.S_FALSE; taj 20.12.

		/// <inheritdoc/>
		public virtual HRESULT InsertMenusSB(HMENU hmenuShared, ref Ole32.OLEMENUGROUPWIDTHS lpMenuWidths) => HRESULT.E_NOTIMPL;

		/// <inheritdoc/>
		public virtual HRESULT OnDefaultCommand(IShellView ppshv)
		{
			/* 20.12.
			var selected = shellView.SelectedItems;

			if (selected.Length > 0 && selected[0].IsFolder)
			{
				try { shellView.Navigate(selected[0] is ShellFolder f ? f : selected[0].Parent); }
				catch { }
			}
			else
			{
				shellView.OnDoubleClick(EventArgs.Empty);
			}
			*/
			return HRESULT.S_OK;
		}

		/// <inheritdoc/>
		public virtual HRESULT OnStateChange(IShellView ppshv, CDBOSC uChange)
		{
			//if (uChange == CDBOSC.CDBOSC_SELCHANGE)	// 20.12.
			//	shellView.OnSelectionChanged();
			return HRESULT.S_OK;
		}

		/// <inheritdoc/>
		public virtual HRESULT OnViewWindowActive(IShellView ppshv) => HRESULT.E_NOTIMPL;

		/// <inheritdoc/>
		public virtual HRESULT QueryActiveShellView(out IShellView ppshv)
		{
			ppshv = null;
			return HRESULT.E_NOTIMPL;
		}

		/// <inheritdoc/>
		public virtual HRESULT QueryService(in Guid guidService, in Guid riid, out IntPtr ppvObject)
		{
			var lriid = riid;
			var ii = GetType().GetInterfaces().FirstOrDefault(i => i.IsCOMObject && i.GUID == lriid);
			if (ii is null)
			{
				ppvObject = IntPtr.Zero;
				return HRESULT.E_NOINTERFACE;
			}

			ppvObject = Marshal.GetComInterfaceForObject(this, ii);
			return HRESULT.S_OK;
		}

		/// <inheritdoc/>
		public virtual HRESULT QueryStatus(in Guid pguidCmdGroup, uint cCmds, OLECMD[] prgCmds, OLECMDTEXT pCmdText) => HRESULT.E_NOTIMPL;

		/// <inheritdoc/>
		public virtual HRESULT RemoveMenusSB(HMENU hmenuShared) => HRESULT.E_NOTIMPL;

		/// <inheritdoc/>
		public virtual HRESULT SendControlMsg(FCW id, uint uMsg, IntPtr wParam, IntPtr lParam, out IntPtr pret)
		{
			pret = default;
			return HRESULT.E_NOTIMPL;
		}

		/// <inheritdoc/>
		public virtual HRESULT SetMenuSB(HMENU hmenuShared, IntPtr holemenuRes, HWND hwndActiveObject) => HRESULT.E_NOTIMPL;

		/// <inheritdoc/>
		public virtual HRESULT SetStatusTextSB(string pszStatusText) => HRESULT.E_NOTIMPL;

		/// <inheritdoc/>
		public virtual HRESULT SetToolbarItems(ComCtl32.TBBUTTON[] lpButtons, uint nButtons, FCT uFlags) => HRESULT.E_NOTIMPL;

		/// <inheritdoc/>
		public virtual HRESULT TranslateAcceleratorSB(ref MSG pmsg, ushort wID) => HRESULT.E_NOTIMPL;
	}
}