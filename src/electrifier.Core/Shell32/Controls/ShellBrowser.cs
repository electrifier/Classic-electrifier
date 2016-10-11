using System;
using System.Windows.Forms;

using System.Runtime.InteropServices;

using electrifier.Core.Services;
using electrifier.Core.Shell32.Services;
using electrifier.Win32API;

namespace electrifier.Core.Shell32.Controls {

	/// <summary>
	/// Summary for ShellBrowser
	/// </summary>

	public class ShellBrowser : System.Windows.Forms.Panel, ShellAPI.IShellBrowser, WinAPI.IServiceProvider {
		protected static DesktopFolderInstance desktopFolder = ServiceManager.Services.GetService(typeof(DesktopFolderInstance)) as DesktopFolderInstance;
		protected IntPtr absolutePIDL = IntPtr.Zero;
		protected IntPtr relativePIDL = IntPtr.Zero;
		protected ShellAPI.IShellFolder shellFolder = null;
		protected ShellAPI.IShellView shellView = null;
		protected IntPtr shellViewHandle = IntPtr.Zero;

		public event BrowseShellObjectEventHandler BrowseShellObject = null;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ShellBrowser(System.ComponentModel.IContainer container) {
			container.Add(this);
			InitializeComponent();
		}

		public ShellBrowser(ShellAPI.CSIDL shellObjectCSIDL)
			: this(PIDLManager.CreateFromCSIDL(shellObjectCSIDL), true) { }

		public ShellBrowser(string shellObjectFullPath)
			: this(PIDLManager.CreateFromPathW(shellObjectFullPath), true) { }

		public ShellBrowser(IntPtr shellObjectPIDL)
			: this(shellObjectPIDL, false) { }

		private ShellBrowser(IntPtr shellObjectPIDL, bool pidlSelfCreated) : base() {
			InitializeComponent();

			// Default constructing code...
			this.NavigateTo(shellObjectPIDL, pidlSelfCreated);

		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			this.DisposeIShellView();
			this.DisposeIShellFolder();
			this.DisposePIDLs();

			if (disposing)
				if (this.components != null)
					this.components.Dispose();
			base.Dispose(disposing);
		}

		protected void DisposePIDLs() {
			if (this.absolutePIDL != IntPtr.Zero) {
				PIDLManager.Free(this.absolutePIDL);
				this.absolutePIDL = this.relativePIDL = IntPtr.Zero;
			}
		}

		protected void DisposeIShellFolder() {
			if (this.shellFolder != null) {
				lock (this.shellFolder) {
					Marshal.ReleaseComObject(this.shellFolder);
					this.shellFolder = null;
				}
			}
		}

		protected void DisposeIShellView(ShellAPI.IShellView disposingShellView) {
			if (null != disposingShellView) {
				disposingShellView.UIActivate(ShellAPI.SVUIA.DEACTIVATE);
				disposingShellView.DestroyViewWindow();
				Marshal.ReleaseComObject(disposingShellView);
			}
		}

		protected void DisposeIShellView() {
			if (this.shellView != null) {
				lock (this.shellView) {
					this.shellViewHandle = IntPtr.Zero;
					this.shellView.UIActivate(ShellAPI.SVUIA.DEACTIVATE);
					this.shellView.DestroyViewWindow();
					Marshal.ReleaseComObject(this.shellView);
					this.shellView = null;
				}
			}
		}

		private void ShellBrowser_Resize(object sender, System.EventArgs e) {
			if (this.shellViewHandle != IntPtr.Zero) {
				WinAPI.MoveWindow(this.shellViewHandle, 0, 0, this.ClientRectangle.Width, this.ClientRectangle.Height, false);
			}
		}

		protected override void WndProc(ref Message m) {
			switch ((WinAPI.WM)m.Msg) {
				case WinAPI.WM.GETISHELLBROWSER:
					// See Knowledge Base article 157247 for more details on this message
					m.Result = Marshal.GetComInterfaceForObject(this, typeof(ShellAPI.IShellBrowser));
					return;
				default:
					base.WndProc(ref m);
					return;
			}
		}

		public void NavigateTo(IntPtr folderPIDL, bool pidlSelfCreated = false) {
			HResult result = ((ShellAPI.IShellBrowser)this).BrowseObject(folderPIDL, (ShellAPI.SBSP.SameBrowser | ShellAPI.SBSP.Absolute));

			if (result.Failed)
				result.ThrowException();
		}

		private HResult BrowseObjectInternal(IntPtr pidl, ShellAPI.SBSP wFlags) {
			if (wFlags.HasFlag(ShellAPI.SBSP.SameBrowser)) {
				ShellAPI.IShellView oldShellView = this.shellView;

				// TODO: Check for ShellAPI.SBSP.Relative and ShellAPI.SBSP.Parent

				try {
					ShellAPI.FOLDERSETTINGS folderSettings;

					if (null != this.shellView)
						this.shellView.GetCurrentInfo(out folderSettings);
					else
						ShellAPI.FOLDERSETTINGS.LoadDefaults(out folderSettings);

					this.DisposePIDLs();
					this.DisposeIShellFolder();

					this.absolutePIDL = PIDLManager.Clone(pidl);
					this.relativePIDL = PIDLManager.FindLastID(this.absolutePIDL);
					this.shellFolder = desktopFolder.GetIShellFolder(pidl);

					if (null == this.shellFolder)
						throw new Exception("ShellBrowser:BrowseObjectInternal: 'GetIShellFolder' failed!");

					HResult hResult = ShellAPI.SHCreateShellFolderView(new ShellAPI.SFV_CREATE(this.shellFolder), out this.shellView);

					if (hResult.Succeeded) {
						Win32API.RECT viewDimensions = new Win32API.RECT(this.ClientRectangle);

						hResult = this.shellView.CreateViewWindow(oldShellView, ref folderSettings, this, ref viewDimensions, out this.shellViewHandle);

						if (hResult.Failed) {
							MessageBox.Show("ShellBrowser.BrowseObjectInternal: CreateViewWindow failed!\n" +
								"HRESULT = " + hResult.ToString(), "electrifier: Unhandled Exception!", MessageBoxButtons.OK, MessageBoxIcon.Error);
						}

						this.shellView.UIActivate(ShellAPI.SVUIA.ACTIVATE_NOFOCUS);
					} else {
						MessageBox.Show("ShellBrowser.BrowseObjectInternal: SHCreateShellFolderView failed!\n" +
							"HRESULT = " + hResult.ToString(), "electrifier: Unhandled Exception!", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
				} catch (Exception e) {
					// TODO: Error-handling?!?

					MessageBox.Show("ShellBrowser.cs: ShellBrowser.BrowseObjectInternal:\nUnknown exception!\n" + e.Message);
				} finally {
					this.DisposeIShellView(oldShellView);
				}

			} else if (wFlags.HasFlag(ShellAPI.SBSP.NewBrowser)) {

				MessageBox.Show("ShellBrowser.NavigateTo: Open in new Browser!\n",
					"electrifier: Invalid Argument", MessageBoxButtons.OK, MessageBoxIcon.Error);

			}

			if (this.BrowseShellObject != null)
				this.BrowseShellObject(this, new BrowseShellObjectEventArgs(pidl, wFlags));

			return HResult.S_OK;
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			// 
			// ShellBrowser
			// 
			this.Resize += new System.EventHandler(this.ShellBrowser_Resize);

		}
		#endregion Windows Form Designer generated code

		#region IShellBrowser Member
		HResult ShellAPI.IShellBrowser.GetWindow(out IntPtr HWND) {
			HWND = this.Handle;

			return HResult.S_OK;
		}

		HResult ShellAPI.IShellBrowser.ContextSensitiveHelp(bool fEnterMode) {
			return HResult.E_NotImpl;
		}

		HResult ShellAPI.IShellBrowser.InsertMenusSB(System.IntPtr hmenuShared, ref System.IntPtr lpMenuWidths) {
			return HResult.E_NotImpl;
		}

		HResult ShellAPI.IShellBrowser.SetMenuSB(System.IntPtr hmenuShared, System.IntPtr holemenuRes, System.IntPtr hwndActiveObject) {
			return HResult.E_NotImpl;
		}

		HResult ShellAPI.IShellBrowser.RemoveMenusSB(System.IntPtr hmenuShared) {
			return HResult.E_NotImpl;
		}

		HResult ShellAPI.IShellBrowser.SetStatusTextSB(string pszStatusText) {
			return HResult.E_NotImpl;
		}

		HResult ShellAPI.IShellBrowser.EnableModelessSB(bool fEnable) {
			return HResult.E_NotImpl;
		}

		HResult ShellAPI.IShellBrowser.TranslateAcceleratorSB(IntPtr pmsg, UInt16 wID) {
			return HResult.E_NotImpl;
		}

		HResult ShellAPI.IShellBrowser.BrowseObject(IntPtr pidl, ShellAPI.SBSP wFlags) {
			if(this.InvokeRequired) {
				this.Invoke((Action)(() => {
					this.BrowseObjectInternal(pidl, wFlags);
				}));
			} else {
				this.BrowseObjectInternal(pidl, wFlags);
			}

			return HResult.S_OK;
		}

		HResult ShellAPI.IShellBrowser.GetViewStateStream(uint /* DWORD */ grfMode, IntPtr /*IStream */ ppStrm) {
			return HResult.E_NotImpl;
		}

		HResult ShellAPI.IShellBrowser.GetControlWindow(uint id, out IntPtr phwnd) {
			phwnd = IntPtr.Zero;
			return HResult.E_NotImpl;
		}

		HResult ShellAPI.IShellBrowser.SendControlMsg(uint id, uint uMsg, uint wParam, uint lParam, IntPtr pret) {
			return HResult.E_NotImpl;
		}

		HResult ShellAPI.IShellBrowser.QueryActiveShellView([MarshalAs(UnmanagedType.Interface)] ref ShellAPI.IShellView ppshv) {
			Marshal.AddRef(Marshal.GetIUnknownForObject(ppshv = this.shellView));

			return HResult.S_OK;
		}

		HResult ShellAPI.IShellBrowser.OnViewWindowActive([MarshalAs(UnmanagedType.Interface)] ShellAPI.IShellView pshv) {
			// TODO: Now the shellview gets the focus
			//			if(this.CanSelect) {
			//				this.Select();
			//			}
			//			if(this.shellViewHandle != IntPtr.Zero) {
			//				IntPtr x = WinAPI.SetFocus(this.shellViewHandle);
			//				return 0x0;
			//			}
			return HResult.S_OK;
		}

		HResult ShellAPI.IShellBrowser.SetToolbarItems(IntPtr /* LPTBBUTTONSB */ lpButtons, uint nButtons, uint uFlags) {
			return HResult.E_NotImpl;
		}
		#endregion IShellBrowser Member

		#region IServiceProvider Member
		HResult WinAPI.IServiceProvider.QueryService(ref Guid guidService, ref Guid riid, out IntPtr ppvObject) {
			// TODO: Check for and handle IOleCommandTarget?!?

			if (riid.Equals(ShellAPI.IID_IShellBrowser)) {
				ppvObject = Marshal.GetComInterfaceForObject(this, typeof(ShellAPI.IShellBrowser));

				return HResult.S_OK;
			} else {
				ppvObject = IntPtr.Zero;

				return HResult.E_NoInterface;
			}
		}

		#endregion IServiceProvider Member
	}

	public delegate void BrowseShellObjectEventHandler(object source, BrowseShellObjectEventArgs e);

	public class BrowseShellObjectEventArgs : EventArgs {
		public BrowseShellObjectEventArgs(IntPtr shellObjectPIDL, ShellAPI.SBSP Flags) {
			this.ShellObjectPIDL = shellObjectPIDL;
			this.Flags = Flags;
		}

		public IntPtr ShellObjectPIDL = IntPtr.Zero;
		public ShellAPI.SBSP Flags = ShellAPI.SBSP.None;
	}

}
