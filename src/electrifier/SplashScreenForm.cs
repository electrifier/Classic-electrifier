/*
** 
**  electrifier
** 
**  Copyright (c) 2017 Thorsten Jung @ electrifier.org and contributors
** 
*/

using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

using electrifier.Win32API;				// TODO: Remove dependency for dynamic binding

namespace electrifier
{
    /// <summary>
    /// Summary for SplashScreenForm.
    /// </summary>
    public class SplashScreenForm : Form
    {
        protected Bitmap splashScreenBitmap = null;
        public Bitmap SplashScreenBitmap { get { return this.splashScreenBitmap; } }
        protected bool splashIsShown = false;
        protected bool splashIsFadedOut = false;
        protected byte opacity = 0xFF;
        public new byte Opacity {
            get { return this.opacity; }
            set {
                this.opacity = value;
                UpdateWindowLayer();
            }
        }

        public SplashScreenForm(bool splashIsShown, bool splashIsFadedOut, byte initialOpacity)
            : this(splashIsShown, splashIsFadedOut) { this.opacity = initialOpacity; }

        public SplashScreenForm(bool splashIsShown, bool splashIsFadedOut)
        {
            this.TopMost = true;
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ShowInTaskbar = false;

            if (splashIsShown)
            {
                this.splashScreenBitmap = new Bitmap(Assembly.GetEntryAssembly().
                    GetManifestResourceStream("electrifier.SplashScreenForm.png"));
                this.Size = this.splashScreenBitmap.Size;

                // Change window mode to layered window
                WinAPI.SetWindowLong(this.Handle, WinAPI.GWL.EXSTYLE, WinAPI.WS.EX_LAYERED);

                // Show the splash screen form
                this.splashIsShown = true;
                this.Show();
            }
        }

        // TODO: Fade out splash screen when closing...

        public new void Show()
        {
            if (this.splashIsShown)
            {
                base.Show();
                this.UpdateWindowLayer();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.splashScreenBitmap != null))
                this.splashScreenBitmap.Dispose();

            base.Dispose(disposing);
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == (int)WinAPI.WM.NCHITTEST)
            {
                m.Result = (IntPtr)2;       // TODO: introduce & set to constant

                return;
            }

            base.WndProc(ref m);
        }

        protected void UpdateWindowLayer()
        {
            if (!this.splashIsShown)
                return;

            IntPtr screenDC = IntPtr.Zero;
            IntPtr memoryDC = IntPtr.Zero;
            IntPtr bitmapHandle = IntPtr.Zero;
            IntPtr oldBitmapHandle = IntPtr.Zero;
            WinAPI.SIZE splashScreenSize = new WinAPI.SIZE(this.splashScreenBitmap.Size);

            try
            {
                WinAPI.POINT destinationPosition = new WinAPI.POINT(this.Left, this.Top);
                WinAPI.POINT sourcePosition = new WinAPI.POINT(0, 0);
                WinAPI.BLENDFUNCTION blendFunction =
                    new WinAPI.BLENDFUNCTION(WinAPI.AC.SRC_OVER, 0, this.opacity, WinAPI.AC.SRC_ALPHA);

                screenDC = WinAPI.GetDC(IntPtr.Zero);
                memoryDC = WinAPI.CreateCompatibleDC(screenDC);
                bitmapHandle = this.splashScreenBitmap.GetHbitmap(Color.FromArgb(0x00000000));
                oldBitmapHandle = WinAPI.SelectObject(memoryDC, bitmapHandle);

                WinAPI.UpdateLayeredWindow(this.Handle, screenDC, ref destinationPosition, ref splashScreenSize,
                    memoryDC, ref sourcePosition, 0, ref blendFunction, WinAPI.ULW.ALPHA);
            }
            finally
            {
                WinAPI.ReleaseDC(IntPtr.Zero, screenDC);

                if (bitmapHandle != IntPtr.Zero)
                {
                    WinAPI.SelectObject(memoryDC, oldBitmapHandle);
                    // TODO: Check: Windows.DeleteObject(hBitmap); // The documentation says that we have to use the Windows.DeleteObject... but since there is no such method I use the normal DeleteObject from Win32 GDI and it's working fine without any resource leak.
                    WinAPI.DeleteObject(bitmapHandle);
                }

                WinAPI.DeleteDC(memoryDC);
            }
        }
    }
}
