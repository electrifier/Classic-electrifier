using System;
using System.Windows.Forms;

using System.Runtime.InteropServices;

using electrifier.Core.Services;
using electrifier.Core.Shell32.Services;
using electrifier.Win32API;

namespace electrifier.Core.Shell32.Controls {
	/// <summary>
	/// Zusammenfassung für ShellBrowser.
	/// </summary>

	public class ShellBrowser : Panel, ShellAPI.IShellBrowser, WinAPI.IServiceProvider {
		protected static DesktopFolderInstance desktopFolder = ServiceManager.Services.GetService(typeof(DesktopFolderInstance)) as DesktopFolderInstance;
		protected IntPtr absolutePIDL = IntPtr.Zero;
		protected IntPtr relativePIDL = IntPtr.Zero;
		protected ShellAPI.IShellFolder shellFolder = null;
		protected ShellAPI.IShellView shellView = null;
		protected IntPtr shellViewHandle = IntPtr.Zero;

		public event BrowseShellObjectEventHandler BrowseShellObject = null;

		/// <summary>
		/// Erforderliche Designervariable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ShellBrowser(System.ComponentModel.IContainer container) {
			///
			/// Erforderlich für Windows.Forms Klassenkompositions-Designerunterstützung
			///
			container.Add(this);
			InitializeComponent();

			//
			// TODO: Fügen Sie den Konstruktorcode nach dem Aufruf von InitializeComponent hinzu
			//
		}

		public ShellBrowser(ShellAPI.CSIDL shellObjectCSIDL)
			: this(PIDLManager.CreateFromCSIDL(shellObjectCSIDL), true) { }

		public ShellBrowser(string shellObjectFullPath)
			: this(PIDLManager.CreateFromPathW(shellObjectFullPath), true) { }

		public ShellBrowser(IntPtr shellObjectPIDL)
			: this(shellObjectPIDL, false) { }

		private ShellBrowser(IntPtr shellObjectPIDL, bool pidlSelfCreated) : base() {
			///
			/// Erforderlich für Windows.Forms Klassenkompositions-Designerunterstützung
			///
			InitializeComponent();

			// Default constructing code...
			this.NavigateTo(shellObjectPIDL, pidlSelfCreated);

		}

		/// <summary> 
		/// Verwendete Ressourcen bereinigen.
		/// </summary>
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

		protected void DisposeIShellView() {
			if (this.shellView != null) {
				lock (this.shellView) {
					this.shellViewHandle = IntPtr.Zero;
					//					this.shellView.UIActivate(ShellAPI.SVUIA.DEACTIVATE);		// TODO: RELAUNCH: Commented out due exception
					//					this.shellView.DestroyViewWindow();							// TODO: RELAUNCH: Commented out due exception
					Marshal.ReleaseComObject(this.shellView);
					this.shellView = null;
				}
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
			try {
				ShellAPI.FOLDERSETTINGS folderSettings;

				if (this.shellView != null)
					this.shellView.GetCurrentInfo(out folderSettings);
				else {
					folderSettings.ViewMode = ShellAPI.FOLDERVIEWMODE.DETAILS;
					folderSettings.Flags = ShellAPI.FOLDERFLAGS.SNAPTOGRID;
				}

				this.DisposePIDLs();
				this.DisposeIShellFolder();
				this.DisposeIShellView();

				this.absolutePIDL = (pidlSelfCreated ? folderPIDL : PIDLManager.Clone(folderPIDL));
				this.relativePIDL = PIDLManager.FindLastID(this.absolutePIDL);
				this.shellFolder = desktopFolder.GetIShellFolder(folderPIDL);

				if (this.shellFolder != null) {
					IntPtr pShellView;

					int hResultVO = this.shellFolder.CreateViewObject(this.Handle, ref ShellAPI.IID_IShellView, out pShellView);

					if (pShellView != IntPtr.Zero) {
						this.shellView = Marshal.GetTypedObjectForIUnknown(pShellView, typeof(ShellAPI.IShellView)) as ShellAPI.IShellView;
						// TODO: Refactor the code above...

						if (this.shellView != null) {
							Win32API.RECT viewDimensions = new Win32API.RECT(this.ClientRectangle);

							// TODO: Issue #1, CreateViewWindow fails for e.g. Control Panel (ReturnCode = 1),
							// so check ReturnCode (OLELastError or something like that) and check why it fails!

							// TODO: CreateViewWindow returns HRESULT
							// TODO: Set previous (First parameter)
							uint hResult = this.shellView.CreateViewWindow(null, ref folderSettings, this, ref viewDimensions, out this.shellViewHandle);
							if (hResult != 0) {
								MessageBox.Show("ShellBrowser.NavigateTo: CreateViewWindow failed!\n" +
									"HRESULT = " + hResult, "electrifier: Unhandled Exception!", MessageBoxButtons.OK, MessageBoxIcon.Error);
							}

							this.shellView.UIActivate(ShellAPI.SVUIA.ACTIVATE_NOFOCUS);
						}
					}
				}
			} catch (Exception e) {
				// TODO: Error-handling?!?

				MessageBox.Show("ShellBrowser.cs: ShellBrowser.NavigateTo:\nUnknown exception!\n" + e.Message);
			}
		}

		#region Vom Komponenten-Designer generierter Code
		/// <summary>
		/// Erforderliche Methode für die Designerunterstützung. 
		/// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
		/// </summary>
		private void InitializeComponent() {
			// 
			// ShellBrowser
			// 
			this.Resize += new System.EventHandler(this.ShellBrowser_Resize);

		}
		#endregion

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
			if (wFlags.HasFlag(ShellAPI.SBSP.SameBrowser)) {
				this.Invoke((Action)(() => {        // TODO: InvokeRequired
					this.NavigateTo(pidl, false);

					if (this.BrowseShellObject != null)
						this.BrowseShellObject(this, new BrowseShellObjectEventArgs(pidl, wFlags));
				}));
			} else {
				// TODO: Check all the other possible flags!

				MessageBox.Show("ShellBrowser.cs: BrowseObject:\nFlag is unknown!");
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
			return HResult.E_NotImpl;
		}

		HResult ShellAPI.IShellBrowser.OnViewWindowActive([MarshalAs(UnmanagedType.Interface)] ShellAPI.IShellView pshv) {
			//			if(this.CanSelect) {
			//				this.Select();
			//			}
			//			if(this.shellViewHandle != IntPtr.Zero) {
			//				IntPtr x = WinAPI.SetFocus(this.shellViewHandle);
			//				return 0x0;
			//			}
			return HResult.E_NotImpl;
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

				return HResult.E_NoInterface;   // TODO: return SVC_E_UNKNOWNSERVICE instead?!? when service not recognized
			}
		}

		#endregion IServiceProvider Member

		private void ShellBrowser_Resize(object sender, System.EventArgs e) {
			if (this.shellViewHandle != IntPtr.Zero) {
				WinAPI.MoveWindow(this.shellViewHandle, 0, 0, this.ClientRectangle.Width, this.ClientRectangle.Height, true);
			}
		}
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
