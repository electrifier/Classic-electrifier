//	<file>
//		<copyright see="www.electrifier.org"/>
//		<license   see="www.electrifier.org"/>
//		<owner     name="Thorsten Jung" email="taj@electrifier.org"/>
//		<version   value="$Id: WinAPI.cs,v 1.9 2004/09/11 23:02:07 taj bender Exp $"/>
//	</file>

using System;
using System.Runtime.InteropServices;

namespace Electrifier.Win32API {
	public enum WMSG : uint {
		/**
		 * ListView-Control Messages
		 */
		LVM_FIRST						= 0x00001000,
		LVM_GETIMAGELIST		= LVM_FIRST +  2,
		LVM_SETIMAGELIST		= LVM_FIRST +  3,
		LVM_REDRAWITEMS			= LVM_FIRST + 21,
		LVM_SETITEMSTATE		= LVM_FIRST + 43,
		LVM_SETITEMCOUNT		= LVM_FIRST + 47,

		/**
		 * TreeView-Control Messages
		 */
		TV_FIRST						= 0x00001100,
		TVM_INSERTITEMA			= TV_FIRST +  0,
		TVM_DELETEITEM			= TV_FIRST +  1,
		TVM_EXPAND					= TV_FIRST +  2,
		TVM_GETITEMRECT			= TV_FIRST +  4,
		TVM_GETCOUNT				= TV_FIRST +  5,
		TVM_GETINDENT				= TV_FIRST +  6,
		TVM_SETINDENT				= TV_FIRST +  7,
		TVM_GETIMAGELIST		= TV_FIRST +  8,
		TVM_SETIMAGELIST		= TV_FIRST +  9,
		TVM_GETNEXTITEM			= TV_FIRST + 10,
		TVM_CREATEDRAGIMAGE	= TV_FIRST + 18,
		TVM_INSERTITEMW     = TV_FIRST + 50,
		TVM_SETITEM         = TV_FIRST + 63,
	}

	public enum ILD : uint {		// ImageList_Draw fStyle-enumeration
		NORMAL              = 0x00000000,
		TRANSPARENT         = 0x00000001,
		MASK                = 0x00000010,
		IMAGE               = 0x00000020,
		ROP                 = 0x00000040,
		BLEND25             = 0x00000002,
		BLEND50             = 0x00000004,
		OVERLAYMASK         = 0x00000F00,
		PRESERVEALPHA       = 0x00001000,
		SCALE               = 0x00002000,
		DPISCALE            = 0x00004000,
		SELECTED            = BLEND50,
		FOCUS               = BLEND25,
		BLEND               = BLEND50,
	}


	public enum TVIF : uint {			// TVItemEx flags
		TEXT          = 0x0001,
		IMAGE         = 0x0002,
		PARAM         = 0x0004,
		STATE         = 0x0008,
		HANDLE        = 0x0010,
		SELECTEDIMAGE = 0x0020,
		CHILDREN      = 0x0040,
		INTEGRAL      = 0x0080,			// WIN32_IE >= 0x0400
	}

	public enum LVIF : uint {			// ListView ItemMask
		TEXT        = 0x0001,
		IMAGE       = 0x0002,
		PARAM       = 0x0004,
		STATE       = 0x0008,
		INDENT      = 0x0010,
		NORECOMPUTE = 0x0800,
		GROUPID     = 0x0100,		// WIN32_WINNT >= 0x0501
		COLUMNS     = 0x0200,		// WIN32_WINNT >= 0x0501
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct LVDISPINFO {
		public NMHDR  hdr;
		public LVITEM item;
	}

	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Unicode)]
	public struct LVITEM {
		public LVIF        mask;
		public int         iItem;
		public int         iSubItem;
		public WinAPI.LVIS state;
		public WinAPI.LVIS stateMask;
		public IntPtr      pszText;
		public int         cchTextMax;
		public int         iImage;
		public IntPtr      lParam;
		public int         iIndent;
		public int         iGroupId;			// WIN32_WINNT >= 0x0501
		public uint        cColumns;			// WIN32_WINNT >= 0x0501
		public IntPtr      puColumns;			// WIN32_WINNT >= 0x0501
	}

	public enum TVSIL : uint {			// TreeView SetImageList-message constants
		NORMAL = 0,
		STATE  = 2,
	}

	public enum LVS : uint {			// ListView style constants
		ICON                = 0x0000,
		REPORT              = 0x0001,
		SMALLICON           = 0x0002,
		LIST                = 0x0003,
		TYPEMASK            = 0x0003,
		SINGLESEL           = 0x0004,
		SHOWSELALWAYS       = 0x0008,
		SORTASCENDING       = 0x0010,
		SORTDESCENDING      = 0x0020,
		SHAREIMAGELISTS     = 0x0040,
		NOLABELWRAP         = 0x0080,
		AUTOARRANGE         = 0x0100,
		EDITLABELS          = 0x0200,
		OWNERDATA           = 0x1000,
		NOSCROLL            = 0x2000,
		TYPESTYLEMASK       = 0xfc00,
		ALIGNTOP            = 0x0000,
		ALIGNLEFT           = 0x0800,
		ALIGNMASK           = 0x0c00,
		OWNERDRAWFIXED      = 0x0400,
		NOCOLUMNHEADER      = 0x4000,
		NOSORTHEADER        = 0x8000,
	}

	public enum LVSIL : uint {			// ListView SetImageList-message constants
		NORMAL = 0,
		SMALL  = 1,
		STATE  = 2,
	}

	public enum TVGN : uint {			// TreeView GetNextItem-message constants
		ROOT            = 0x0000,
		NEXT            = 0x0001,
		PREVIOUS        = 0x0002,
		PARENT          = 0x0003,
		CHILD           = 0x0004,
		FIRSTVISIBLE    = 0x0005,
		NEXTVISIBLE     = 0x0006,
		PREVIOUSVISIBLE = 0x0007,
		DROPHILITE      = 0x0008,
		CARET           = 0x0009,
		LASTVISIBLE     = 0x000A,		// WIN32_IE >= 0x0400
	}

	/// <summary>
	/// Self-created helper object for dealing with HTREEITEM-handles of TreeView-nodes
	/// </summary>
	public struct HTREEITEM {
		private IntPtr handle;
		public  IntPtr Handle  { get { return handle; } }
		public  bool   IsValid { get { return !handle.Equals(IntPtr.Zero); } }

		public HTREEITEM(IntPtr handle) {
			this.handle = handle;
		}

		public HTREEITEM NextNode(IntPtr treeViewHandle) {
			return new HTREEITEM(
				WinAPI.SendMessage(treeViewHandle, WMSG.TVM_GETNEXTITEM, TVGN.NEXT, this.handle));
		}
		public HTREEITEM PrevNode(IntPtr treeViewHandle) {
			return new HTREEITEM(
				WinAPI.SendMessage(treeViewHandle, WMSG.TVM_GETNEXTITEM, TVGN.PREVIOUS, this.handle));
		}
		public HTREEITEM FirstNode(IntPtr treeViewHandle) {
			return new HTREEITEM(
				WinAPI.SendMessage(treeViewHandle, WMSG.TVM_GETNEXTITEM, TVGN.CHILD, this.handle));
		}
		public HTREEITEM Parent(IntPtr treeViewHandle) {
			return new HTREEITEM(
				WinAPI.SendMessage(treeViewHandle, WMSG.TVM_GETNEXTITEM, TVGN.PARENT, this.handle));
		}
		public static HTREEITEM GetRootItem(IntPtr treeViewHandle) {
			return new HTREEITEM(
				WinAPI.SendMessage(treeViewHandle, WMSG.TVM_GETNEXTITEM, TVGN.ROOT, IntPtr.Zero));
		}
	}

	[StructLayout(LayoutKind.Sequential)]	// Notify-Message Header
	public struct NMHDR {
		public IntPtr hwndFrom;
		public int    idFrom;
		public int    code;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct NMLISTVIEW {				// ListView Notify-Message
		public NMHDR        hdr;
		public int          iItem;
		public int          iSubItem;
		public uint         uNewState;
		public uint         uOldState;
		public uint         uChanged;
		public WinAPI.Point ptAction;
		public IntPtr       lParam;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct NMTREEVIEW {
		public NMHDR        hdr;
		public UInt32       action;
//		public IntPtr       itemOld;
//		public IntPtr       itemNew;
		public IntPtr x11;
		public IntPtr x12;
		public IntPtr x13;
		public IntPtr x14;
		public IntPtr x21;
		public IntPtr x22;
		public IntPtr x23;
		public IntPtr x24;


		public WinAPI.Point ptDrag;
	}
/*
typedef struct tagNMTREEVIEW {
    NMHDR hdr;
    UINT action;
    TVITEM itemOld;
    TVITEM itemNew;
    POINT ptDrag;
} NMTREEVIEW, *LPNMTREEVIEW;
 * */
	[StructLayout(LayoutKind.Sequential)]
	public struct TVItem {				//TODO: Marshaling pruefen (typedef struct tagTVITEMEX)
		public TVIF   mask;									//    UINT mask;
		public IntPtr hItem;									//    HTREEITEM hItem;
		public UInt32 state;									//    UINT state;
		public UInt32 stateMask;							//    UINT stateMask;
		[MarshalAs(UnmanagedType.LPWStr)]
		public string pszText;								//    LPTSTR pszText;
		public Int32  cchTextMax;							//    int cchTextMax;
		public Int32  iImage;								//    int iImage;
		public Int32  iSelectedImage;						//    int iSelectedImage;
		public Int32  cChildren;							//    int cChildren;
		public Int32  lParam;								//    LPARAM lParam;
//		public Int32  iIntegral;							//    int iIntegral;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct TVItemEx {				//TODO: Marshaling pruefen (typedef struct tagTVITEMEX)
		public TVIF   mask;									//    UINT mask;
		public IntPtr hItem;									//    HTREEITEM hItem;
		public UInt32 state;									//    UINT state;
		public UInt32 stateMask;							//    UINT stateMask;
		[MarshalAs(UnmanagedType.LPStr)]
		public string pszText;								//    LPTSTR pszText;
		public Int32  cchTextMax;							//    int cchTextMax;
		public Int32  iImage;								//    int iImage;
		public Int32  iSelectedImage;						//    int iSelectedImage;
		public Int32  cChildren;							//    int cChildren;
		public Int32  lParam;								//    LPARAM lParam;
		public Int32  iIntegral;							//    int iIntegral;
	}

	public class WinAPI {
		public enum WM : int {							// General Windows Messages
			SETFOCUS      = 0x00000007,
			KILLFOCUS     = 0x00000008,
			NOTIFY        = 0x0000004E,
			NCHITTEST     = 0x00000084,
			NCLBUTTONDOWN = 0x000000A1,
			HSCROLL       = 0x00000114,
			VSCROLL       = 0x00000115,
			USER          = 0x00000400,
			REFLECT       = USER + 0x1c00,
		}

		public enum SB : int {								// Scroll bar related constants
			/*
			 * Scroll Bar Constants
			 */
			HORZ            = 0,
			VERT            = 1,
			CTL             = 2,
			BOTH            = 3,

			/*
			 * Scroll Bar Commands
			 */
			LINEUP          = 0,
			LINELEFT        = 0,
			LINEDOWN        = 1,
			LINERIGHT       = 1,
			PAGEUP          = 2,
			PAGELEFT        = 2,
			PAGEDOWN        = 3,
			PAGERIGHT       = 3,
			THUMBPOSITION   = 4,
			THUMBTRACK      = 5,
			TOP             = 6,
			LEFT            = 6,
			BOTTOM          = 7,
			RIGHT           = 7,
			ENDSCROLL       = 8,
		}

		public enum LVN : int {							// ListView Notify Messages
			FIRST          = (0-100),
			LAST           = (0-199),
			BEGINDRAG		= FIRST -  9,
			BEGINRDRAG		= FIRST - 11,
			GETDISPINFOA   = FIRST - 50,
			GETDISPINFOW   = FIRST - 77,
			SETDISPINFOA   = FIRST - 51,
			SETDISPINFOW   = FIRST - 78,
			ODCACHEHINT    = FIRST - 13,
			ODFINDITEMW    = FIRST - 79,
		}

		public enum TVN : int {
			FIRST           =      (0-400),       // treeview
			LAST            =     (0-499),
			BEGINDRAGA      =    FIRST-7,
			BEGINDRAGW      =    FIRST-56,
			BEGINRDRAGA     =    FIRST-8,
			BEGINRDRAGW     =    FIRST-57,

		}

		public enum LVIS : uint {
			FOCUSED        = 0x0001,
			SELECTED       = 0x0002,
			CUT            = 0x0004,
			DROPHILITED    = 0x0008,
			GLOW           = 0x0010,
			ACTIVATING     = 0x0020,
			OVERLAYMASK    = 0x0F00,
			STATEIMAGEMASK = 0xF000,
		}

		public enum GWL : int {			// Window field offsets for GetWindowLong()
			WNDPROC    = -4,
			HINSTANCE  = -6,
			HWNDPARENT = -8,
			STYLE      = -16,
			EXSTYLE    = -20,
			USERDATA   = -21,
			ID         = -12,
		}

		public enum WS : uint {			// Window Styles
			OVERLAPPED          = 0x00000000,
			POPUP               = 0x80000000,
			CHILD               = 0x40000000,
			MINIMIZE            = 0x20000000,
			VISIBLE             = 0x10000000,
			DISABLED            = 0x08000000,
			CLIPSIBLINGS        = 0x04000000,
			CLIPCHILDREN        = 0x02000000,
			MAXIMIZE            = 0x01000000,
			CAPTION             = 0x00C00000,
			BORDER              = 0x00800000,
			DLGFRAME            = 0x00400000,
			VSCROLL             = 0x00200000,
			HSCROLL             = 0x00100000,
			SYSMENU             = 0x00080000,
			THICKFRAME          = 0x00040000,
			GROUP               = 0x00020000,
			TABSTOP             = 0x00010000,
			MINIMIZEBOX         = 0x00020000,
			MAXIMIZEBOX         = 0x00010000,
			TILED               = OVERLAPPED,
			ICONIC              = MINIMIZE,
			SIZEBOX             = THICKFRAME,
			TILEDWINDOW         = OVERLAPPEDWINDOW,
			OVERLAPPEDWINDOW    = OVERLAPPED | CAPTION | SYSMENU | THICKFRAME | MINIMIZEBOX | MAXIMIZEBOX,
			POPUPWINDOW         = POPUP | BORDER | SYSMENU,
			CHILDWINDOW         = CHILD,
			EX_DLGMODALFRAME    = 0x00000001,
			EX_NOPARENTNOTIFY   = 0x00000004,
			EX_TOPMOST          = 0x00000008,
			EX_ACCEPTFILES      = 0x00000010,
			EX_TRANSPARENT      = 0x00000020,
			EX_MDICHILD         = 0x00000040,
			EX_TOOLWINDOW       = 0x00000080,
			EX_WINDOWEDGE       = 0x00000100,
			EX_CLIENTEDGE       = 0x00000200,
			EX_CONTEXTHELP      = 0x00000400,
			EX_RIGHT            = 0x00001000,
			EX_LEFT             = 0x00000000,
			EX_RTLREADING       = 0x00002000,
			EX_LTRREADING       = 0x00000000,
			EX_LEFTSCROLLBAR    = 0x00004000,
			EX_RIGHTSCROLLBAR   = 0x00000000,
			EX_CONTROLPARENT    = 0x00010000,
			EX_STATICEDGE       = 0x00020000,
			EX_APPWINDOW        = 0x00040000,
			EX_OVERLAPPEDWINDOW = EX_WINDOWEDGE | EX_CLIENTEDGE,
			EX_PALETTEWINDOW    = EX_WINDOWEDGE | EX_TOOLWINDOW | EX_TOPMOST,
			EX_LAYERED          = 0x00080000,
			EX_NOINHERITLAYOUT  = 0x00100000,
			EX_LAYOUTRTL        = 0x00400000,
			EX_COMPOSITED       = 0x02000000,
			EX_NOACTIVATE       = 0x08000000
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct Point {
			public Int32 X;
			public Int32 Vertical {
				get {
					return X;
				}
				set {
					X = value;
				}
			}

			public Int32 Y;
			public Int32 Horizontal {
				get {
					return Y;
				}
				set {
					Y = value;
				}
			}

			public Point(Int32 x, Int32 y) {
				X = x;
				Y = y;
			}
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct Size {
			public Int32 Cx;
			public Int32 Width {
				get {
					return Cx;
				}
				set {
					Cx = value;
				}
			}

			public Int32 Cy;
			public Int32 Height {
				get {
					return Cy;
				}
				set {
					Cy = value;
				}
			}

			public Size(Int32 width, Int32 height) {
				Cx = width;
				Cy = height;
			}

			public Size(System.Drawing.Size size) {
				Cx = size.Width;
				Cy = size.Height;
			}
		}

		public enum AC : byte {
			SRC_OVER  = 0x00,
			SRC_ALPHA = 0x01,
		}

		public enum ULW : uint {
			COLORKEY = 0x00000001,
			ALPHA    = 0x00000002,
			OPAQUE   = 0x00000004,
		}

		[StructLayout(LayoutKind.Sequential, Pack=1)]
		public struct BLENDFUNCTION {
			public AC   BlendOp;
			public byte BlendFlags;
			public byte SourceConstantAlpha;
			public AC   AlphaFormat;

			public BLENDFUNCTION(AC blendOp, byte blendFlags, byte sourceConstantAlpha, AC alphaFormat) {
				BlendOp             = blendOp;
				BlendFlags          = blendFlags;
				SourceConstantAlpha = sourceConstantAlpha;
				AlphaFormat         = alphaFormat;
			}
		}

		[DllImport("user32.dll")]
		public static extern int    SendMessage(IntPtr hWnd, WMSG wMsg, int wParam, int lParam);
		[DllImport("user32.dll")]		// TVM_GETNEXTITEM-overloading
		public static extern IntPtr SendMessage(IntPtr hWnd, WMSG wMsg, TVGN wParam, IntPtr lParam);
		[DllImport("user32.dll")]		// TVM_SETIMAGELIST/TVM_GETIMAGELIST-overloading
		public static extern IntPtr SendMessage(IntPtr hWnd, WMSG wMsg, TVSIL wParam, IntPtr lParam);
		[DllImport("user32.dll")]		// LVM_SETIMAGELIST/LVM_GETIMAGELIST-overloading
		public static extern IntPtr SendMessage(IntPtr hWnd, WMSG wMsg, LVSIL wParam, IntPtr lParam);
//		[DllImport("user32.dll")]   // LVM_SETITEMSTATE/LVM_GETITEMSTATE-overloading
//		public static extern int    SendMessage(IntPtr hWnd, WMSG wMsg, int wParam, ref LVITEM lParam);
		[DllImport("user32.dll")]		// TVM_CREATEDRAGIMAGE-overloading
		public static extern IntPtr SendMessage(IntPtr hWnd, WMSG wMsg, int wParam, IntPtr lParam);
		[DllImport("user32.dll")]		// WM_
		public static extern int    SendMessage(IntPtr hWnd, WMSG wMsg, int wParam, ref TVItemEx lParam);
		[DllImport("user32.dll")]		// WM_VSCROLL/WM_HSCROLL-overloading
		public static extern int    SendMessage(IntPtr hWnd, WM wMsg, SB wParam, IntPtr lParam);

		[DllImport("user32.dll")]
		public static extern Int32  SetWindowLong(IntPtr hWnd, GWL nIndex, WS dwNewLong);

		[DllImport("user32.dll")]
		public static extern IntPtr GetDC(IntPtr hWnd);
		[DllImport("gdi32.dll")]
		public static extern IntPtr CreateCompatibleDC(IntPtr hDC);
		[DllImport("user32.dll")]
		public static extern int    ReleaseDC(IntPtr hWnd, IntPtr hDC);
		[DllImport("gdi32.dll")]
		public static extern bool   DeleteDC(IntPtr hdc);
		[DllImport("gdi32.dll")]
		public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);
		[DllImport("gdi32.dll")]
		public static extern bool   DeleteObject(IntPtr hObject);
		[DllImport("user32.dll")]
		public static extern bool   UpdateLayeredWindow(IntPtr hwnd, IntPtr hdcDst, ref Point pptDst, ref Size psize, IntPtr hdcSrc, ref Point pprSrc, Int32 crKey, ref BLENDFUNCTION pBlend, ULW dwFlags);

		[DllImport("comctl32.dll")]
		public static extern bool		ImageList_BeginDrag(IntPtr himlTrack, int iTrack, int dxHotspot, int dyHotspot);
		[DllImport("comctl32.dll")]
		public static extern bool		ImageList_DragMove(int x, int y);
		[DllImport("comctl32.dll")]
		public static extern void		ImageList_EndDrag();
		[DllImport("comctl32.dll")]
		public static extern bool		ImageList_DragEnter(IntPtr hwndLock, int x, int y);
		[DllImport("comctl32.dll")]
		public static extern bool		ImageList_DragLeave(IntPtr hwndLock);
		[DllImport("comctl32.dll")]
		public static extern bool		ImageList_DragShowNolock(bool fShow);
		[DllImport("comctl32.dll")]
		public static extern bool		ImageList_Draw(IntPtr himl, int i, IntPtr hdcDst, int x, int y, ILD fStyle);

		
		
		private WinAPI() {
			// This class can't be instantiated directly
		}
	}
}
