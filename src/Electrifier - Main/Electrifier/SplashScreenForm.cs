using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

using Electrifier.Win32API;

namespace Electrifier {
	/// <summary>
	/// Zusammenfassung für SplashScreenForm.
	/// </summary>
	public class SplashScreenForm : Form {
		protected  Bitmap  splashScreenBitmap = null;
		public     Bitmap  SplashScreenBitmap { get { return splashScreenBitmap; } }
		protected  bool    splashIsShown      = false;
		protected  bool    splashIsFadedOut   = false;
		protected  byte    opacity            = 0xFF;
		public new byte    Opacity            { get { return opacity; }
			set {
				opacity = value;
				UpdateWindowLayer();
			}
		}

		public SplashScreenForm(bool splashIsShown, bool splashIsFadedOut, byte initialOpacity)
			: this(splashIsShown, splashIsFadedOut) { opacity = initialOpacity; }

		public SplashScreenForm(bool splashIsShown, bool splashIsFadedOut) {
			TopMost         = true;
			FormBorderStyle = FormBorderStyle.None;
			StartPosition   = FormStartPosition.CenterScreen;
			ShowInTaskbar   = false;

			if(splashIsShown) {
				splashScreenBitmap = new Bitmap(Assembly.GetEntryAssembly().
					GetManifestResourceStream("Electrifier.SplashScreenForm.png"));
				Size               = splashScreenBitmap.Size;

				// Change window mode to layered window
				WinAPI.SetWindowLong(Handle, WinAPI.GWL.EXSTYLE, WinAPI.WS.EX_LAYERED);

				// Show the splash screen form
				this.splashIsShown = true;
				Show();
			}
		}

		public new void Show() {
			if(splashIsShown) {
				base.Show();
				UpdateWindowLayer();
			}
		}

		protected override void Dispose(bool disposing) {
			if(disposing && (splashScreenBitmap != null))
				splashScreenBitmap.Dispose();

			base.Dispose(disposing);
		}

		protected override void WndProc(ref Message m) {
			if(m.Msg == (int)WinAPI.WM.NCHITTEST) {
				m.Result = (IntPtr)2;		// TODO: introduce & set to constant

				return;
			}

			base.WndProc (ref m);
		}

		protected void UpdateWindowLayer() {
			if(!splashIsShown)
				return;

			IntPtr      screenDC         = IntPtr.Zero;
			IntPtr      memoryDC         = IntPtr.Zero;
			IntPtr      bitmapHandle     = IntPtr.Zero;
			IntPtr      oldBitmapHandle  = IntPtr.Zero;
			WinAPI.Size splashScreenSize = new WinAPI.Size(splashScreenBitmap.Size);

			try {
				WinAPI.Point         destinationPosition = new WinAPI.Point(Left, Top);
				WinAPI.Point         sourcePosition      = new WinAPI.Point(0, 0);
				WinAPI.BLENDFUNCTION blendFunction       =
					new WinAPI.BLENDFUNCTION(WinAPI.AC.SRC_OVER, 0, opacity, WinAPI.AC.SRC_ALPHA);

				screenDC        = WinAPI.GetDC(IntPtr.Zero);
				memoryDC        = WinAPI.CreateCompatibleDC(screenDC);
				bitmapHandle    = splashScreenBitmap.GetHbitmap(Color.FromArgb(0x00000000));
				oldBitmapHandle = WinAPI.SelectObject(memoryDC, bitmapHandle);

				WinAPI.UpdateLayeredWindow(Handle, screenDC, ref destinationPosition, ref splashScreenSize,
				                           memoryDC, ref sourcePosition, 0, ref blendFunction, WinAPI.ULW.ALPHA);
			}
			finally {
				WinAPI.ReleaseDC(IntPtr.Zero, screenDC);

				if(bitmapHandle != IntPtr.Zero) {
					WinAPI.SelectObject(memoryDC, oldBitmapHandle);
					// TODO: Check: Windows.DeleteObject(hBitmap); // The documentation says that we have to use the Windows.DeleteObject... but since there is no such method I use the normal DeleteObject from Win32 GDI and it's working fine without any resource leak.
					WinAPI.DeleteObject(bitmapHandle);
				}

				WinAPI.DeleteDC(memoryDC);
			}
		}
	}
}
