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
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace electrifier
{
    /// <summary>
    /// SplashScreenForm, electrifier's splash screen.
    /// </summary>
    public class SplashScreenForm : Form
    {
        public Bitmap SplashScreenBitmap { get; private set; }
        protected bool splashIsShown = false;
        protected bool splashIsFadedOut = false;
        protected byte opacity = 0xFF;
        public new byte Opacity {
            get => this.opacity;
            set {
                this.opacity = value;
                this.UpdateWindowLayer();
            }
        }

        public SplashScreenForm(bool splashIsShown, bool splashIsFadedOut, byte initialOpacity)
            : this(splashIsShown, splashIsFadedOut) => this.opacity = initialOpacity;

        public SplashScreenForm(bool splashIsShown, bool splashIsFadedOut)
        {
            this.TopMost = true;
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ShowInTaskbar = false;

            if (splashIsShown)
            {
                this.SplashScreenBitmap = new Bitmap(Assembly.GetEntryAssembly().
                    GetManifestResourceStream("electrifier.SplashScreenForm.png"));
                this.Size = this.SplashScreenBitmap.Size;

                // Change window mode to layered window
                User32.SetWindowLong(this.Handle, User32.GWL.EXSTYLE, User32.WS.EX_LAYERED);

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
            if (disposing && (this.SplashScreenBitmap != null))
                this.SplashScreenBitmap.Dispose();

            base.Dispose(disposing);
        }

        /// <summary>
        /// WM_NCHITTEST message
        /// 
        /// Sent to a window in order to determine what part of the window corresponds to a particular screen coordinate.
        /// </summary>
        protected readonly int NCHITTEST = 0x84;

        /// <summary>
        /// The return value of the DefWindowProc function is one of the following values, indicating the position of
        /// the cursor hot spot.
        /// 
        /// HTCAPTION = In a title bar.
        /// </summary>
        protected readonly IntPtr HTCAPTION = (IntPtr)2;

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == this.NCHITTEST)
            {
                m.Result = this.HTCAPTION;

                return;
            }

            base.WndProc(ref m);
        }

        protected void UpdateWindowLayer()
        {
            if (!this.splashIsShown)
                return;

            var screenDC = IntPtr.Zero;
            var memoryDC = IntPtr.Zero;
            var bitmapHandle = IntPtr.Zero;
            var oldBitmapHandle = IntPtr.Zero;
            var splashScreenSize = new WinDef.SIZE(this.SplashScreenBitmap.Size);

            try
            {
                var destinationPosition = new WinDef.POINT(this.Left, this.Top);
                var sourcePosition = new WinDef.POINT(0, 0);
                var blendFunction =
                    new User32.BLENDFUNCTION(User32.AC.SRC_OVER, 0, this.opacity, User32.AC.SRC_ALPHA);

                screenDC = User32.GetDC(IntPtr.Zero);
                memoryDC = Gdi32.CreateCompatibleDC(screenDC);
                bitmapHandle = this.SplashScreenBitmap.GetHbitmap(Color.FromArgb(0x00000000));
                oldBitmapHandle = Gdi32.SelectObject(memoryDC, bitmapHandle);

                User32.UpdateLayeredWindow(this.Handle, screenDC, ref destinationPosition, ref splashScreenSize,
                    memoryDC, ref sourcePosition, 0, ref blendFunction, User32.ULW.ALPHA);
            }
            finally
            {
                User32.ReleaseDC(IntPtr.Zero, screenDC);

                if (bitmapHandle != IntPtr.Zero)
                {
                    Gdi32.SelectObject(memoryDC, oldBitmapHandle);
                    Gdi32.DeleteObject(bitmapHandle);
                }

                Gdi32.DeleteDC(memoryDC);
            }
        }
    }

    /// <summary>
    /// Gdi32 contains the P/Invoke imports for GDI32.dll
    /// </summary>
    public static class Gdi32
    {
        /// <summary>
        /// The CreateCompatibleDC function creates a memory device context (DC) compatible with the specified device.
        /// </summary>
        /// <param name="hDC">A handle to an existing DC. If this handle is NULL, the function creates a memory DC compatible
        /// with the application's current screen.</param>
        /// <returns>If the function succeeds, the return value is the handle to a memory DC.
        /// If the function fails, the return value is NULL.</returns>
        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleDC(IntPtr hDC);

        /// <summary>
        /// The DeleteDC function deletes the specified device context (DC).
        /// </summary>
        /// <param name="hdc">A handle to the device context.</param>
        /// <returns>If the function succeeds, the return value is nonzero.
        /// If the function fails, the return value is zero.</returns>
        [DllImport("gdi32.dll")]
        public static extern bool DeleteDC(IntPtr hdc);

        /// <summary>
        /// The DeleteObject function deletes a logical pen, brush, font, bitmap, region, or palette, freeing all system
        /// resources associated with the object. After the object is deleted, the specified handle is no longer valid.
        /// </summary>
        /// <param name="hObject">A handle to a logical pen, brush, font, bitmap, region, or palette.</param>
        /// <returns>If the function succeeds, the return value is nonzero.
        /// If the specified handle is not valid or is currently selected into a DC, the return value is zero.</returns>
        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        /// <summary>
        /// The SelectObject function selects an object into the specified device context (DC).
        /// The new object replaces the previous object of the same type.
        /// </summary>
        /// <param name="hDC">A handle to the DC.</param>
        /// <param name="hObject">A handle to the object to be selected.</param>
        /// <returns>If the selected object is not a region and the function succeeds, the return value is a handle
        /// to the object being replaced.</returns>
        [DllImport("gdi32.dll")]
        public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);
    }

    /// <summary>
    /// User32 contains the P/Invoke imports for User32.dll
    /// </summary>
    public static class User32
    {
        /// <summary>
        /// The GetDC function retrieves a handle to a device context (DC) for the client area of a specified window or for
        /// the entire screen. You can use the returned handle in subsequent GDI functions to draw in the DC.
        /// The device context is an opaque data structure, whose values are used internally by GDI.
        /// </summary>
        /// <param name="hWnd">
        /// A handle to the window whose DC is to be retrieved.
        /// If this value is NULL, GetDC retrieves the DC for the entire screen.
        /// </param>
        /// <returns>
        /// If the function succeeds, the return value is a handle to the DC for the specified window's client area.
        /// If the function fails, the return value is NULL.
        /// </returns>
        [DllImport("user32.dll")]
        public static extern IntPtr GetDC(IntPtr hWnd);

        /// <summary>
        /// The ReleaseDC function releases a device context (DC), freeing it for use by other applications.
        /// The effect of the ReleaseDC function depends on the type of DC. It frees only common and window DCs.
        /// It has no effect on class or private DCs.
        /// </summary>
        /// <param name="hWnd">A handle to the window whose DC is to be released.</param>
        /// <param name="hDC">A handle to the DC to be released.</param>
        /// <returns>
        /// The return value indicates whether the DC was released. If the DC was released, the return value is 1.
        /// If the DC was not released, the return value is zero.</returns>
        [DllImport("user32.dll")]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        /// <summary>
        /// Valid 'nIndex' parameter values for GetWindowLong and SetWindowLong.
        /// 
        /// The zero-based offset to the value to be set. Valid values are in the range zero through
        /// the number of bytes of extra window memory, minus the size of an integer.
        /// </summary>
        /// <remarks>
        ///   <b>This is only an excerpt of those values used in electrifier!</b>
        /// </remarks>
        public enum GWL : int
        {
            /// <summary>
            /// Sets a new extended window style.
            /// </summary>
            EXSTYLE = -20,
        }

        /// <summary>
        /// Window Styles
        /// The following are the window styles.
        /// After the window has been created, these styles cannot be modified, except as noted.
        /// </summary>
        /// <remarks>
        ///   <b>This is only an excerpt of those values used in electrifier!</b>
        /// </remarks>
        public enum WS : uint
        {
            /// <summary>
            /// The window is a layered window. This style cannot be used if the window has a class style of either
            /// CS_OWNDC or CS_CLASSDC.
            /// 
            /// Windows 8: The WS_EX_LAYERED style is supported for top-level windows and child windows.
            /// Previous Windows versions support WS_EX_LAYERED only for top-level windows.
            /// </summary>
            EX_LAYERED = 0x00080000,
        }

        /// <summary>
        /// Changes an attribute of the specified window. The function also sets the
        /// 32-bit (long) value at the specified offset into the extra window memory.
        /// </summary>
        /// <remarks>
        ///   <see href="https://docs.microsoft.com/en-us/windows/desktop/api/winuser/nf-winuser-setwindowlonga"/>
        /// </remarks>
        /// <param name="hWnd">A handle to the window and, indirectly, the class to which the window belongs.</param>
        /// <param name="nIndex">The zero-based offset to the value to be set. Valid values are in the range zero through
        /// the number of bytes of extra window memory, minus the size of an integer.</param>
        /// <param name="dwNewLong">The replacement value.</param>
        /// <returns>
        /// If the function succeeds, the return value is the previous value of the specified 32-bit integer.
        /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
        /// </returns>
        [DllImport("user32.dll")]
        public static extern Int32 SetWindowLong(IntPtr hWnd, GWL nIndex, WS dwNewLong);

        /// <summary>
        /// The BLENDFUNCTION structure controls blending by specifying
        /// the blending functions for source and destination bitmaps.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct BLENDFUNCTION
        {
            /// <summary>
            /// The source blend operation. Currently, the only source and destination blend operation
            /// that has been defined is AC_SRC_OVER. For details, see the following Remarks section.
            /// </summary>
            public AC BlendOp;
            /// <summary>
            /// Must be zero.
            /// </summary>
            public byte BlendFlags;
            /// <summary>
            /// Specifies an alpha transparency value to be used on the entire source bitmap.
            /// The SourceConstantAlpha value is combined with any per-pixel alpha values in the source bitmap.
            /// If you set SourceConstantAlpha to 0, it is assumed that your image is transparent.
            /// Set the SourceConstantAlpha value to 255 (opaque) when you only want to use per-pixel alpha values.
            /// </summary>
            public byte SourceConstantAlpha;
            /// <summary>
            /// This member controls the way the source and destination bitmaps are interpreted.
            /// AlphaFormat has the following value. <see cref="AC"/>
            /// </summary>
            public AC AlphaFormat;

            public BLENDFUNCTION(AC blendOp, byte blendFlags, byte sourceConstantAlpha, AC alphaFormat)
            {
                this.BlendOp = blendOp;
                this.BlendFlags = blendFlags;
                this.SourceConstantAlpha = sourceConstantAlpha;
                this.AlphaFormat = alphaFormat;
            }
        }

        public enum AC : byte
        {
            SRC_OVER = 0x00,
            /// <summary>
            /// This flag is set when the bitmap has an Alpha channel (that is, per-pixel alpha).
            /// Note that the APIs use premultiplied alpha, which means that the red, green and blue channel values in
            /// the bitmap must be premultiplied with the alpha channel value. For example, if the alpha channel value
            /// is x, the red, green and blue channels must be multiplied by x and divided by 0xff prior to the call.
            /// </summary>
            SRC_ALPHA = 0x01,
        }

        public enum ULW : uint
        {
            /// <summary>
            /// Use crKey as the transparency color.
            /// </summary>
            COLORKEY = 1,
            /// <summary>
            /// Use pblend as the blend function.
            /// If the display mode is 256 colors or less, the effect of this value is the same as the effect of ULW_OPAQUE.
            /// </summary>
            ALPHA = 2,
            /// <summary>
            /// Draw an opaque layered window.
            /// </summary>
            OPAQUE = 4,
        }

        /// <summary>
        /// Updates the position, size, shape, content, and translucency of a layered window.
        /// </summary>
        /// <param name="hwnd">A handle to a layered window. A layered window is created by specifying WS_EX_LAYERED when
        /// creating the window with the CreateWindowEx function.</param>
        /// <param name="hdcDst">A handle to a DC for the screen. This handle is obtained by specifying NULL when calling
        /// the function. It is used for palette color matching when the window contents are updated.
        /// If hdcDst is NULL, the default palette will be used.</param>
        /// <param name="pptDst">A pointer to a structure that specifies the new screen position of the layered window.
        /// If the current position is not changing, pptDst can be NULL.</param>
        /// <param name="psize">A pointer to a structure that specifies the new size of the layered window.
        /// If the size of the window is not changing, psize can be NULL. If hdcSrc is NULL, psize must be NULL.</param>
        /// <param name="hdcSrc">A handle to a DC for the surface that defines the layered window. This handle can be
        /// obtained by calling the CreateCompatibleDC function.
        /// If the shape and visual context of the window are not changing, hdcSrc can be NULL.</param>
        /// <param name="pprSrc">A pointer to a structure that specifies the location of the layer in the device context.
        /// If hdcSrc is NULL, pptSrc should be NULL.</param>
        /// <param name="crKey">A structure that specifies the color key to be used when composing the layered window.
        /// To generate a COLORREF, use the RGB macro.</param>
        /// <param name="pBlend">A pointer to a structure that specifies the transparency value to be used when composing
        /// the layered window.</param>
        /// <param name="dwFlags">This parameter can be one of the following values: <see cref="ULW"/>.
        /// If hdcSrc is NULL, dwFlags should be zero.</param>
        /// <returns>If the function succeeds, the return value is nonzero.
        /// If the function fails, the return value is zero.To get extended error information, call GetLastError.</returns>
        [DllImport("user32.dll")]
        public static extern bool UpdateLayeredWindow(IntPtr hwnd, IntPtr hdcDst, ref WinDef.POINT pptDst,
            ref WinDef.SIZE psize, IntPtr hdcSrc, ref WinDef.POINT pprSrc, int crKey, ref BLENDFUNCTION pBlend, ULW dwFlags);
    }

    /// <summary>
    /// WinDef contains the P/Invoke imports for Windows GDI
    /// </summary>
    public static class WinDef
    {
        /// <summary>
        /// The POINT structure defines the x- and y- coordinates of a point.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public Int32 X;
            public Int32 Horizontal {
                get { return this.X; }
                set { this.X = value; }
            }

            public Int32 Y;
            public Int32 Vertical {
                get { return this.Y; }
                set { this.Y = value; }
            }

            public POINT(Int32 x, Int32 y)
            {
                this.X = x;
                this.Y = y;
            }

            public POINT(POINT point)
            {
                this.X = point.X;
                this.Y = point.Y;
            }

            public POINT(System.Drawing.Point point)
            {
                this.X = point.X;
                this.Y = point.Y;
            }

            public override string ToString()
            {
                return "(" + this.X + "," + this.Y + ")";
            }
        }

        /// <summary>
        /// The SIZE structure defines the width and height of a rectangle.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct SIZE
        {
            public Int32 Cx;
            public Int32 Width {
                get {
                    return this.Cx;
                }
                set {
                    this.Cx = value;
                }
            }

            public Int32 Cy;
            public Int32 Height {
                get {
                    return this.Cy;
                }
                set {
                    this.Cy = value;
                }
            }

            public SIZE(Int32 width, Int32 height)
            {
                this.Cx = width;
                this.Cy = height;
            }

            public SIZE(System.Drawing.Size size)
            {
                this.Cx = size.Width;
                this.Cy = size.Height;
            }
        }
    }
}
