using System;
using System.ComponentModel;
using System.Collections;
using System.Diagnostics;
using System.Windows.Forms;

using System.Runtime.InteropServices;

using Electrifier.Core.Services;
using Electrifier.Core.Shell32.Services;
using Electrifier.Win32API;

namespace Electrifier.Core.Shell32.Controls {
	/// <summary>
	/// Zusammenfassung für ShellBrowser.
	/// </summary>
	public class ShellBrowser : Panel, ShellAPI.IShellBrowser, WinAPI.IServiceProvider {
		protected static DesktopFolderInstance  desktopFolder   = (DesktopFolderInstance)ServiceManager.Services.GetService(typeof(DesktopFolderInstance));
		protected        IntPtr                 absolutePIDL    = IntPtr.Zero;
		protected        IntPtr                 relativePIDL    = IntPtr.Zero;
		protected        ShellAPI.IShellFolder  shellFolder     = null;
		protected        ShellAPI.IShellView    shellView       = null;
		protected        IntPtr                 shellViewHandle = IntPtr.Zero;

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
			: this(PIDLManager.CreateFromCSIDL(shellObjectCSIDL), true) {}
		
		public ShellBrowser(string shellObjectFullPath)
			: this(PIDLManager.CreateFromPathW(shellObjectFullPath), true) {}
		
		public ShellBrowser(IntPtr shellObjectPIDL)
			: this(shellObjectPIDL, false) {}

		private ShellBrowser(IntPtr shellObjectPIDL, bool pidlSelfCreated) : base() {
			///
			/// Erforderlich für Windows.Forms Klassenkompositions-Designerunterstützung
			///
			InitializeComponent();

			// Default constructing code...
			this.SetBrowsingFolder(shellObjectPIDL, pidlSelfCreated);

		}

		/// <summary> 
		/// Verwendete Ressourcen bereinigen.
		/// </summary>
		protected override void Dispose( bool disposing ) {
			this.DisposeIShellView();
			this.DisposeIShellFolder();
			this.DisposePIDLs();

			if(disposing)
				if(this.components != null)
					this.components.Dispose();
			base.Dispose( disposing );
		}

		protected void DisposePIDLs() {
			if(this.absolutePIDL != IntPtr.Zero) {
					PIDLManager.Free(this.absolutePIDL);
					this.absolutePIDL = this.relativePIDL = IntPtr.Zero;
			}
		}

		protected void DisposeIShellFolder() {
			if(this.shellFolder != null) {
				lock(this.shellFolder) {
					Marshal.ReleaseComObject(this.shellFolder);
					this.shellFolder = null;
				}
			}
		}

		protected void DisposeIShellView() {
			if(this.shellView != null) {
				lock(this.shellView) {
					this.shellViewHandle = IntPtr.Zero;
//					this.shellView.UIActivate(ShellAPI.SVUIA.DEACTIVATE);		// TODO: RELAUNCH: Commented out due exception
//					this.shellView.DestroyViewWindow();							// TODO: RELAUNCH: Commented out due exception
					Marshal.ReleaseComObject(this.shellView);
					this.shellView = null;
				}
			}
		}



		protected override void WndProc(ref Message m) {
			switch((WinAPI.WM)m.Msg) {
				case WinAPI.WM.GETISHELLBROWSER:
					// See Knowledge Base article 157247 for more details on this message
					m.Result = Marshal.GetComInterfaceForObject(this, typeof(ShellAPI.IShellBrowser));
					return;
				default:
					base.WndProc(ref m);
					return;
			}
		}

		public void SetBrowsingFolder(IntPtr folderPIDL) {
			this.SetBrowsingFolder(folderPIDL, false);
		}

		protected void SetBrowsingFolder(IntPtr folderPIDL, bool pidlSelfCreated) {
			ShellAPI.FOLDERSETTINGS folderSettings;

			if(this.shellView != null)
				this.shellView.GetCurrentInfo(out folderSettings);
			else {
				folderSettings.ViewMode = ShellAPI.FOLDERVIEWMODE.DETAILS;
				folderSettings.Flags    = ShellAPI.FOLDERFLAGS.SNAPTOGRID;
			}

			this.DisposePIDLs();
			this.DisposeIShellFolder();
			this.DisposeIShellView();

			this.absolutePIDL = (pidlSelfCreated ? folderPIDL : PIDLManager.Clone(folderPIDL));
			this.relativePIDL = PIDLManager.FindLastID(this.absolutePIDL);
			this.shellFolder  = desktopFolder.GetIShellFolder(folderPIDL);

			if(this.shellFolder != null) {
				IntPtr pShellView;

				this.shellFolder.CreateViewObject(this.Handle, ref ShellAPI.IID_IShellView, out pShellView);

				if(pShellView != IntPtr.Zero) {
					this.shellView = Marshal.GetTypedObjectForIUnknown(pShellView, typeof(ShellAPI.IShellView)) as ShellAPI.IShellView;
					// TODO: Refactor the code above...

					if(this.shellView != null) {
						Win32API.RECT viewDimensions = new Win32API.RECT(this.ClientRectangle);

						this.shellView.CreateViewWindow(null, ref folderSettings, this, ref viewDimensions, out this.shellViewHandle);
						this.shellView.UIActivate(ShellAPI.SVUIA.ACTIVATE_NOFOCUS);
					}
				}
			}

			// TODO: Error-handling?!?
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
		public uint GetWindow(out IntPtr HWND) {
			HWND = this.Handle;

			return 0x0; // S_OK
		}

		public uint ContextSensitiveHelp(bool fEnterMode) {
			return 0x80004001;
		}

		public uint SetToolbarItems(System.IntPtr lpButtons, uint nButtons, uint uFlags) {
			// TODO:  Implementierung von ShellBrowser.SetToolbarItems hinzufügen
			return 0x80004001;
		}

		public uint SetMenuSB(System.IntPtr hmenuShared, System.IntPtr holemenuRes, System.IntPtr hwndActiveObject) {
			// TODO:  Implementierung von ShellBrowser.SetMenuSB hinzufügen
			return 0x80004001;
		}

		public uint BrowseObject(System.IntPtr pidl, ShellAPI.SBSP Flags) {
			if(this.BrowseShellObject != null)
				this.BrowseShellObject(this, new BrowseShellObjectEventArgs(pidl, Flags));

			return 0x0; // S_OK
		}

		public uint TranslateAcceleratorSB(uint pmsg, ushort wID) {
			// TODO:  Implementierung von ShellBrowser.TranslateAcceleratorSB hinzufügen
			return 0x00000000;
		}

		public uint SetStatusTextSB(string pszStatusText) {
			// TODO:  Implementierung von ShellBrowser.SetStatusTextSB hinzufügen
			return 0x80004001;
		}

		public uint SendControlMsg(uint id, uint uMsg, uint wParam, uint lParam, uint pret) {
			// TODO:  Implementierung von ShellBrowser.SendControlMsg hinzufügen
			return 0x80004001;
		}

		public uint GetViewStateStream(uint grfMode, out System.IntPtr ppStrm) {
			// TODO:  Implementierung von ShellBrowser.GetViewStateStream hinzufügen
			ppStrm = new System.IntPtr ();
			return 0x80004001;
		}

		public uint OnViewWindowActive(System.IntPtr pshv) {
//			if(this.CanSelect) {
//				this.Select();
//			}
//			if(this.shellViewHandle != IntPtr.Zero) {
//				IntPtr x = WinAPI.SetFocus(this.shellViewHandle);
//				return 0x0;
//			}
			// TODO:  Implementierung von ShellBrowser.OnViewWindowActive hinzufügen
			return 0x80004001;
		}

		public uint GetControlWindow(uint id, out System.IntPtr phwnd) {
			// TODO:  Implementierung von ShellBrowser.GetControlWindow hinzufügen
			phwnd = new System.IntPtr ();
			return 0x80004001;
		}

		public uint InsertMenusSB(System.IntPtr hmenuShared, ref System.IntPtr lpMenuWidths) {
			// TODO:  Implementierung von ShellBrowser.InsertMenusSB hinzufügen
			return 0x80004001;
		}

		public uint RemoveMenusSB(System.IntPtr hmenuShared) {
			// TODO:  Implementierung von ShellBrowser.RemoveMenusSB hinzufügen
			return 0x80004001;
		}

		public uint EnableModelessSB(bool fEnable) {
			// TODO:  Implementierung von ShellBrowser.EnableModelessSB hinzufügen
			return 0x80004001;
		}

		public uint QueryActiveShellView(out System.IntPtr ppshv) {
			// TODO:  Implementierung von ShellBrowser.QueryActiveShellView hinzufügen
			ppshv = new System.IntPtr ();
			return 0x80004001;
		}

		#endregion

		
		#region IServiceProvider Member

		public uint QueryService(ref Guid guidService, ref Guid riid, out System.IntPtr ppv) {
			// TODO: Very important: Check which Service/Interface-combinations are requested!
			// One combination: SID_STopWindow / IID_IShellBrowser
			if((guidService.CompareTo(ShellAPI.IID_IShellBrowser) == 0) ||
				(guidService.CompareTo(ShellAPI.IID_IShellBrowser) == 0)) {
			
				ppv = Marshal.GetComInterfaceForObject(this, typeof(ShellAPI.IShellBrowser));
				return 0x0; // S_OK
			} else
				ppv = IntPtr.Zero;
			
			// TODO: return SVC_E_UNKNOWNSERVICE instead?!? when service not recognized
			return 0x80004002;		// E_NOINTERFACE
		}

		#endregion

		private void ShellBrowser_Resize(object sender, System.EventArgs e) {
			if(this.shellViewHandle != IntPtr.Zero) {
				WinAPI.MoveWindow(this.shellViewHandle, 0, 0, this.ClientRectangle.Width, this.ClientRectangle.Height, true);
			}
		}
	}

	public delegate void BrowseShellObjectEventHandler(object source, BrowseShellObjectEventArgs e);

	public class BrowseShellObjectEventArgs : EventArgs {
		public BrowseShellObjectEventArgs(IntPtr shellObjectPIDL, ShellAPI.SBSP Flags) {
			this.ShellObjectPIDL = shellObjectPIDL;
			this.Flags           = Flags;
		}

		public IntPtr        ShellObjectPIDL = IntPtr.Zero;
		public ShellAPI.SBSP Flags           = ShellAPI.SBSP.None;
	}

}
