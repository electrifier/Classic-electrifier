using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace electrifier.Win32API {

	/// <summary>
	/// See https://msdn.microsoft.com/en-us/library/windows/desktop/aa378137(v=vs.85).aspx
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct HResult {

		#region The following HRESULT values are the most common. More values are contained in the header file Winerror.h.

		/// See https://msdn.microsoft.com/en-us/library/windows/desktop/aa378137(v=vs.85).aspx
		public static readonly HResult S_OK = new HResult(0x00000000);              // Operation successful
		public static readonly HResult E_NotImpl = new HResult(0x80004001);         // Not implemented
		public static readonly HResult E_NoInterface = new HResult(0x80004002);     // No such interface supported
		public static readonly HResult E_Pointer = new HResult(0x80004003);         // Pointer that is not valid
		public static readonly HResult E_Abort = new HResult(0x80004004);           // Operation aborted
		public static readonly HResult E_Fail = new HResult(0x80004005);            // Unspecified failure
		public static readonly HResult E_Unexpected = new HResult(0x8000FFFF);      // Unexpected failure
		public static readonly HResult E_AccessDenied = new HResult(0x80070005);    // General access denied error
		public static readonly HResult E_Handle = new HResult(0x80070006);          // Handle that is not valid
		public static readonly HResult E_OutOfMemory = new HResult(0x8007000E);     // Failed to allocate necessary memory
		public static readonly HResult E_InvalidArg = new HResult(0x80070057);      // One or more arguments are not valid

		#endregion

		private int value;
		public int Value { get { return this.value; } }

		public HResult(int value) { this.value = value; }
		public HResult(uint value) { this.value = unchecked((int)value); }

		public static implicit operator HResult(int value) { return (new Win32API.HResult(value)); }
		public static implicit operator HResult(uint value) { return (new Win32API.HResult(value)); }

		public bool Failed() { return (this == HResult.S_OK); }
		public bool Succeeded() { return (this != HResult.S_OK); }

		public static bool operator ==(HResult hResultA, HResult hResultB) { return (hResultA.Value == hResultB.Value); }
		public static bool operator !=(HResult hResultA, HResult hResultB) { return (hResultA.Value != hResultB.Value); }

		public override int GetHashCode() { return this.Value; }

		public override bool Equals(object obj) {
			if (null == obj)
				return false;

			HResult HResultObj = (HResult)obj;
			if((object)HResultObj == null)
				return false;

			return (this.Value == HResultObj.Value);
		}
	}

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
		TVM_SETEXTENDEDSTYLE = TV_FIRST + 44,
		TVM_GETEXTENDEDSTYLE = TV_FIRST + 45,
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

	[Flags]
	[Serializable]
	public enum MK : uint {				// Key State Masks for Mouse Messages
		NONE            = 0x0000,
		LBUTTON         = 0x0001,
		RBUTTON         = 0x0002,
		SHIFT           = 0x0004,
		CONTROL         = 0x0008,
		MBUTTON         = 0x0010,
		ALT             = 0x0020,
		ALTCONTROL      = (ALT | CONTROL),
		XBUTTON1        = 0x0020,		// _WIN32_WINNT >= 0x0500
		XBUTTON2        = 0x0040,		// _WIN32_WINNT >= 0x0500
	}

	public enum SW : uint {				// ShowWindow() Commands
		HIDE             = 0,
		SHOWNORMAL       = 1,
		NORMAL           = 1,
		SHOWMINIMIZED    = 2,
		SHOWMAXIMIZED    = 3,
		MAXIMIZE         = 3,
		SHOWNOACTIVATE   = 4,
		SHOW             = 5,
		MINIMIZE         = 6,
		SHOWMINNOACTIVE  = 7,
		SHOWNA           = 8,
		RESTORE          = 9,
		SHOWDEFAULT      = 10,
		FORCEMINIMIZE    = 11,
		MAX              = 11,
	}

	/// <summary>
	/// A helper class for representing the grfKeyState enumeration while doing
	/// drag and drop operations. It is derived from MK-enumeration.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct DragKeyState {
		public MK ModKeys;

		public DragKeyState(MK modifierKeys) { this.ModKeys = modifierKeys; }
		public static implicit operator DragKeyState(MK source) { return new DragKeyState(source); }

		public bool Alt        { get { return ((this.ModKeys & MK.ALT)        == MK.ALT);        } }
		public bool AltControl { get { return ((this.ModKeys & MK.ALTCONTROL) == MK.ALTCONTROL); } }
		public bool Control    { get { return ((this.ModKeys & MK.CONTROL)    == MK.CONTROL);    } }
		public bool Shift      { get { return ((this.ModKeys & MK.SHIFT)      == MK.SHIFT);      } }
		public bool MouseLeft  { get { return ((this.ModKeys & MK.LBUTTON)    == MK.LBUTTON);    } }
		public bool MouseRight { get { return ((this.ModKeys & MK.RBUTTON)    == MK.RBUTTON);    } }
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct LVDISPINFO {
		public NMHDR  hdr;
		public LVITEM item;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct RECT {
		public int Left;
		public int Top;
		public int Right;
		public int Bottom;

		public RECT(System.Drawing.Rectangle Rectangle) {
			this.Left   = Rectangle.Left;
			this.Top    = Rectangle.Top;
			this.Right  = Rectangle.Right;
			this.Bottom = Rectangle.Bottom;
		}
	};

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

	public enum TVIS : uint {			// TreeView Item states
		SELECTED           = 0x00000002,
		CUT                = 0x00000004,
		DROPHILITED        = 0x00000008,
		BOLD               = 0x00000010,
		EXPANDED           = 0x00000020,
		EXPANDEDONCE       = 0x00000040,
		EXPANDPARTIAL      = 0x00000080,
		OVERLAYMASK        = 0x00000F00,
		STATEIMAGEMASK     = 0x0000F000,
		USERMASK           = 0x0000F000,
	}

	/// <summary>
	/// Self-created helper object for dealing with HTREEITEM-handles of TreeView-nodes
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
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
		public WinAPI.POINT ptAction;
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


		public WinAPI.POINT ptDrag;
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
		public TVIS state;									//    UINT state;
		public TVIS stateMask;							//    UINT stateMask;
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
		public TVIS state;									//    UINT state;
		public TVIS stateMask;							//    UINT stateMask;
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
			NULL             = 0x00000000,
			CREATE           = 0x00000001,
			DESTROY          = 0x00000002,
			MOVE             = 0x00000003,
			SIZE             = 0x00000005,
			ACTIVATE         = 0x00000006,
			SETFOCUS         = 0x00000007,
			KILLFOCUS        = 0x00000008,
			NOTIFY           = 0x0000004E,
			NCHITTEST        = 0x00000084,
			NCLBUTTONDOWN    = 0x000000A1,
			HSCROLL          = 0x00000114,
			VSCROLL          = 0x00000115,
			USER             = 0x00000400,
			GETISHELLBROWSER = 0x00000007 + USER,		// See KB 157247
			REFLECT          = 0x00001c00 + USER,
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

		[Flags] public enum TPM : uint {	// Flags for TrackPopupMenu
			LEFTBUTTON      = 0x0000,
			RIGHTBUTTON     = 0x0002,
			LEFTALIGN       = 0x0000,
			CENTERALIGN     = 0x0004,
			RIGHTALIGN      = 0x0008,
			TOPALIGN        = 0x0000,		// WINVER >= 0x0400
			VCENTERALIGN    = 0x0010,		// WINVER >= 0x0400
			BOTTOMALIGN     = 0x0020,		// WINVER >= 0x0400
			HORIZONTAL      = 0x0000,		// WINVER >= 0x0400     /* Horz alignment matters more */
			VERTICAL        = 0x0040,		// WINVER >= 0x0400     /* Vert alignment matters more */
			NONOTIFY        = 0x0080,		// WINVER >= 0x0400     /* Don't send any notification msgs */
			RETURNCMD       = 0x0100,		// WINVER >= 0x0400
			RECURSE         = 0x0001,		// WINVER >= 0x0500
			HORPOSANIMATION = 0x0400,		// WINVER >= 0x0500
			HORNEGANIMATION = 0x0800,		// WINVER >= 0x0500
			VERPOSANIMATION = 0x1000,		// WINVER >= 0x0500
			VERNEGANIMATION = 0x2000,		// WINVER >= 0x0500
			NOANIMATION     = 0x4000,		// _WIN32_WINNT >= 0x0500
			LAYOUTRTL       = 0x8000,		// _WIN32_WINNT >= 0x0501
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
		public struct POINT {
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

			public POINT(Int32 x, Int32 y) {
				this.X = x;
				this.Y = y;
			}

			public POINT(POINT point) {
				this.X = point.X;
				this.Y = point.Y;
			}

			public POINT(System.Drawing.Point point) {
				this.X = point.X;
				this.Y = point.Y;
			}

			public override string ToString() {
				return "(" + this.X + "," + this.Y + ")";
			}
		}

		public static byte GetRValue(COLORREF color) { return color.Red; }
		public static byte GetGValue(COLORREF color) { return color.Green; }
		public static byte GetBValue(COLORREF color) { return color.Blue; }

		[StructLayout(LayoutKind.Sequential)]
			public struct COLORREF {
			Int32 value;					// Color value in format 0x00BBGGRR

			// Red Color Component Properties
			public byte Red {
				get {	return (byte)(this.value);	}
				set {	this.value = ((this.value & 0x00FFFF00) | (value));	}
			}

			// Green Color Component Properties
			public byte Green {
				get { return (byte)(this.value >> 8); }
				set { this.value = ((this.value & 0x00FF00FF) | (value << 8)); }
			}

			// Blue Color Component Properties
			public byte Blue {
				get { return (byte)(this.value >> 16); }
				set { this.value = ((this.value & 0x0000FFFF) | (value << 16)); }
			}

			public COLORREF(byte red, byte green, byte blue) {
				this.value = (red | (green << 8) | (blue << 16));
			}
		}

		[StructLayout(LayoutKind.Sequential)]
			public struct SIZE {
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

			public SIZE(Int32 width, Int32 height) {
				Cx = width;
				Cy = height;
			}

			public SIZE(System.Drawing.Size size) {
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
		public static extern int    SendMessage(IntPtr hWnd, WM wMsg, IntPtr wParam, IntPtr lParam);

		[DllImport("user32.dll")]
		public static extern bool   MoveWindow(IntPtr hWnd, int X, int Y, int Width, int Height, bool Repaint);

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
		public static extern IntPtr GetWindowThreadProcessId(IntPtr hWnd, out IntPtr lpdwProcessId);

		[DllImport("user32.dll")]
		public static extern bool   AttachThreadInput(IntPtr idAttach, IntPtr idAttachTo, bool Attach);

		[DllImport("user32.dll")]
		public static extern IntPtr GetDC(IntPtr hWnd);

		[DllImport("user32.dll")]
		public static extern IntPtr SetFocus(IntPtr hWnd);

		[DllImport("user32.dll")]
		public static extern IntPtr CreatePopupMenu();
		[DllImport("user32.dll")]
		public static extern uint   TrackPopupMenu(IntPtr hMenu, TPM uFlags, int x, int y, int nReserved, IntPtr hWnd, IntPtr prcRect);

		[DllImport("User32.dll")]
		public static extern int	DestroyIcon(IntPtr hIcon);

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
		public static extern bool   UpdateLayeredWindow(IntPtr hwnd, IntPtr hdcDst, ref POINT pptDst, ref SIZE psize, IntPtr hdcSrc, ref POINT pprSrc, Int32 crKey, ref BLENDFUNCTION pBlend, ULW dwFlags);

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
		[DllImport("comctl32.dll")]
		public static extern IntPtr ImageList_GetDragImage(ref POINT ppt, ref POINT pptHotspot);

		[DllImport("ole32.Dll")]
		public static extern int CoCreateInstance(ref Guid clsid,
			[MarshalAs(UnmanagedType.IUnknown)] object inner,
			Win32API.WTypes.CLSCTX context, ref Guid uuid,
			[MarshalAs(UnmanagedType.IUnknown)] out object rReturnedComObject);

		public static Guid IID_IUnknown = new Guid("00000000-0000-0000-C000-000000000046");
		/*
		 * Drag and Drop stuff
		 **/

		public enum CLIPFORMAT : uint {
			TEXT            = 1,
			BITMAP          = 2,
			METAFILEPICT    = 3,
			SYLK            = 4,
			DIF             = 5,
			TIFF            = 6,
			OEMTEXT         = 7,
			DIB             = 8,
			PALETTE         = 9,
			PENDATA         = 10,
			RIFF            = 11,
			WAVE            = 12,
			UNICODETEXT     = 13,
			ENHMETAFILE     = 14,
			HDROP           = 15,
			LOCALE          = 16,
			MAX             = 17,
			OWNERDISPLAY    = 0x0080,
			DSPTEXT         = 0x0081,
			DSPBITMAP       = 0x0082,
			DSPMETAFILEPICT = 0x0083,
			DSPENHMETAFILE  = 0x008E,
			PRIVATEFIRST    = 0x0200,
			PRIVATELAST     = 0x02FF,
			GDIOBJFIRST     = 0x0300,
			GDIOBJLAST      = 0x03FF,
		}

		public enum DVASPECT : uint {
			CONTENT   = 1,
			THUMBNAIL = 2,
			ICON      = 4,
			DOCPRINT  = 8,
		}

		public enum TYMED : uint {
			NULL     = 0,
			HGLOBAL  = 1,
			FILE     = 2,
			ISTREAM  = 4,
			ISTORAGE = 8,
			GDI      = 16,
			MFPICT   = 32,
			ENHMF    = 64,
		}

		[StructLayout(LayoutKind.Sequential)]
			public struct FORMATETC {
			public CLIPFORMAT cfFormat;
			public uint ptd;
			public DVASPECT dwAspect;
			public int lindex;
			public TYMED tymed;
		}

		[StructLayout(LayoutKind.Sequential)]
			public struct STGMEDIUM {
			public uint tymed;
			public uint hGlobal;
			public uint pUnkForRelease;
		}


		public const string GuidStr_SID_STopWindow = "49E1B500-4636-11D3-97F7-00C04F45D0B3";
		public static Guid SID_STopWindow = new Guid(GuidStr_SID_STopWindow);

		#region IServiceProvider

		public const string IIDS_IServiceProvider = "6D5140C1-7436-11CE-8034-00AA006009FA";
		public static Guid IID_IServiceProvider = new Guid(IIDS_IServiceProvider);

		[ComImport(),
			InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
			GuidAttribute(IIDS_IServiceProvider)]
		public interface IServiceProvider {
			[PreserveSig]
			HResult QueryService(ref Guid guidService, ref Guid riid, out IntPtr ppvObject);
		}

		#endregion


		[ComImport(),
			InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
			GuidAttribute("0000010e-0000-0000-C000-000000000046")]
			public interface IDataObject {
			[PreserveSig()]
			int GetData(ref FORMATETC pformatetcIn, ref STGMEDIUM pmedium);
			[PreserveSig()]
			int GetDataHere(ref FORMATETC pformatetc, ref STGMEDIUM pmedium);
			[PreserveSig()]
			int QueryGetData(ref FORMATETC pformatetc);
			[PreserveSig()]
			int GetCanonicalFormatEtc(ref FORMATETC pformatectIn, out FORMATETC pformatetcOut);
			[PreserveSig()]
			int SetData(ref FORMATETC pformatetc, ref STGMEDIUM pmedium, bool fRelease);
			[PreserveSig()]
			int EnumFormatEtc(uint dwDirection, out Object ppenumFormatEtc);
			[PreserveSig()]
			int DAdvise(ref FORMATETC pformatetc, uint advf, ref Object pAdvSink, out uint pdwConnection);
			[PreserveSig()]
			int DUnadvise(uint dwConnection);
			[PreserveSig()]
			int EnumDAdvise(out Object ppenumAdvise);
		}

		/// <summary>
		/// IDropSource - COM-Interface
		/// </summary>
		[ComImport(),
			InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
			GuidAttribute("00000121-0000-0000-C000-000000000046")]
			public interface IDropSource {
			[PreserveSig()]
			int QueryContinueDrag(bool fEscapePressed, DragKeyState KeyState);
			[PreserveSig()]
			int GiveFeedback(int dwEffect);
		}

		/// <summary>
		/// IDropTarget - COM-Interface
		/// </summary>
		[ComImport(),
			InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
			GuidAttribute("00000122-0000-0000-C000-000000000046")]
			public interface IDropTarget {
			[PreserveSig()]
			int DragEnter(IDataObject pDataObj, DragKeyState KeyState, WinAPI.POINT pt, ref DragDropEffects pdwEffect);
			[PreserveSig()]
			int DragOver(DragKeyState KeyState, WinAPI.POINT pt, ref DragDropEffects pdwEffect);
			[PreserveSig()]
			int DragLeave();
			[PreserveSig()]
			int Drop(IDataObject pDataObj, DragKeyState KeyState, WinAPI.POINT pt, ref DragDropEffects pdwEffect);
		}




        //[DllImport("ole32.dll")]              // TODO: RELAUNCH: Commented out due compatibility
        //public static extern int OleLoadFromStream(UCOMIStream pStm,ref Guid
        //    iidInterface,[MarshalAs(UnmanagedType.Interface)] out object ppvObj);

        //[DllImport("OLE32.DLL", EntryPoint="CreateStreamOnHGlobal")]
        //public static extern int CreateStreamOnHGlobal(int hGlobalMemHandle, bool
        //    fDeleteOnRelease, out UCOMIStream pOutStm);

		[DllImport("ole32.dll")]
			// TODO: What does HandleRef do?!? How benefit from? Exchange all Win32API-Handles with it?
		public static extern int RegisterDragDrop(IntPtr hwnd, IDropTarget target);
		//public static extern int RegisterDragDrop(IntPtr hwnd, IDropTarget target);

		[DllImport("user32.dll",
			 CharSet = CharSet.Auto,
			 ExactSpelling = true,
			 EntryPoint = "RegisterClipboardFormatA")]
		public static extern int RegisterClipboardFormat( string lpszFormat );

		[DllImport("uxtheme.dll", CharSet = CharSet.Auto)]
		public extern static int SetWindowTheme(IntPtr hWnd, string subAppName, string subIdList);

		private WinAPI() {
			// No instantion allowed.
		}
	}
}
