//	<file>
//		<copyright see="www.electrifier.org"/>
//		<license   see="www.electrifier.org"/>
//		<owner     name="Thorsten Jung" email="taj@electrifier.org"/>
//		<version   value="$Id: ShellAPI.cs,v 1.7 2004/09/10 15:21:55 taj bender Exp $"/>
//	</file>

using System;
using System.Runtime.InteropServices;

namespace Electrifier.Win32API {
	/// <summary>
	/// Zusammenfassung für SHObjIdl.
	/// </summary>
	/// 
	public enum Test : int {
		x,
		y,
		z,
	}

	public class ShellAPI {
		public static Guid IID_IShellFolder = 
			new Guid("{000214E6-0000-0000-C000-000000000046}");
		public static Guid IID_IMalloc = 
			new Guid("{00000002-0000-0000-C000-000000000046}");

		#region Public functions imported from Shell32.dll
		#region CSIDL - Enumeration: Special Folder types
		[DllImport("Shell32.Dll")]
		public static extern Int32 SHGetFolderLocation(
			IntPtr hwndOwner,         // Handle to the owner window.
			CSIDL nFolder,            // A CSIDL value that identifies the folder to be
			// located.
			IntPtr hToken,            // Token that can be used to represent a particular
			// user.
			UInt32 dwReserved,        // Reserved.
			out IntPtr ppidl);        // Address of a pointer to an item identifier list
		// structure 
		// specifying the folder's location relative to the
		// root of the namespace 
		// (the desktop). 
		/// <summary>
		/// The CSIDL-enumeration contains the special folder types, Shell32.dll-functions
		/// "SHGetFolderLocation", "SHGetFolderPath", "SHGetSpecialFolderLocation", and
		/// "SHGetSpecialFolderPath" accept to retrieve a special folder's pathname or
		/// item ID list (PIDL). Not all of them must exist on the machine of interest.
		/// </summary>
		public enum CSIDL : int {
			DESKTOP                 = 0x0000,	// <desktop>
			INTERNET                = 0x0001,	// Internet Explorer (icon on desktop)
			PROGRAMS                = 0x0002,	// Start Menu\Programs
			CONTROLS                = 0x0003,	// My Computer\Control Panel
			PRINTERS                = 0x0004,	// My Computer\Printers
			PERSONAL                = 0x0005,	// My Documents
			FAVORITES               = 0x0006,	// <user name>\Favorites
			STARTUP                 = 0x0007,	// Start Menu\Programs\Startup
			RECENT                  = 0x0008,	// <user name>\Recent
			SENDTO                  = 0x0009,	// <user name>\SendTo
			BITBUCKET               = 0x000a,	// <desktop>\Recycle Bin
			STARTMENU               = 0x000b,	// <user name>\Start Menu
			MYDOCUMENTS             = 0x000c,	// logical "My Documents" desktop icon
			MYMUSIC                 = 0x000d,	// "My Music" folder
			MYVIDEO                 = 0x000e,	// "My Videos" folder
			DESKTOPDIRECTORY        = 0x0010,	// <user name>\Desktop
			DRIVES                  = 0x0011,	// My Computer
			NETWORK                 = 0x0012,	// Network Neighborhood (My Network Places)
			NETHOOD                 = 0x0013,	// <user name>\nethood
			FONTS                   = 0x0014,	// windows\fonts
			TEMPLATES               = 0x0015,
			COMMON_STARTMENU        = 0x0016,	// All Users\Start Menu
			COMMON_PROGRAMS         = 0x0017,	// All Users\Start Menu\Programs
			COMMON_STARTUP          = 0x0018,	// All Users\Startup
			COMMON_DESKTOPDIRECTORY = 0x0019,	// All Users\Desktop
			APPDATA                 = 0x001a,	// <user name>\Application Data
			PRINTHOOD               = 0x001b,	// <user name>\PrintHood
			LOCAL_APPDATA           = 0x001c,	// <user name>\Local Settings\Applicaiton Data (non roaming)
			ALTSTARTUP              = 0x001d,	// non localized startup
			COMMON_ALTSTARTUP       = 0x001e,	// non localized common startup
			COMMON_FAVORITES        = 0x001f,
			INTERNET_CACHE          = 0x0020,
			COOKIES                 = 0x0021,
			HISTORY                 = 0x0022,
			COMMON_APPDATA          = 0x0023,	// All Users\Application Data
			WINDOWS                 = 0x0024,	// GetWindowsDirectory()
			SYSTEM                  = 0x0025,	// GetSystemDirectory()
			PROGRAM_FILES           = 0x0026,	// C:\Program Files
			MYPICTURES              = 0x0027,	// C:\Program Files\My Pictures
			PROFILE                 = 0x0028,	// USERPROFILE
			SYSTEMX86               = 0x0029,	// x86 system directory on RISC
			PROGRAM_FILESX86        = 0x002a,	// x86 C:\Program Files on RISC
			PROGRAM_FILES_COMMON    = 0x002b,	// C:\Program Files\Common
			PROGRAM_FILES_COMMONX86 = 0x002c,	// x86 Program Files\Common on RISC
			COMMON_TEMPLATES        = 0x002d,	// All Users\Templates
			COMMON_DOCUMENTS        = 0x002e,	// All Users\Documents
			COMMON_ADMINTOOLS       = 0x002f,	// All Users\Start Menu\Programs\Administrative Tools
			ADMINTOOLS              = 0x0030,	// <user name>\Start Menu\Programs\Administrative Tools
			CONNECTIONS             = 0x0031,	// Network and Dial-up Connections
			COMMON_MUSIC            = 0x0035,	// All Users\My Music
			COMMON_PICTURES         = 0x0036,	// All Users\My Pictures
			COMMON_VIDEO            = 0x0037,	// All Users\My Video
			RESOURCES               = 0x0038,	// Resource Direcotry
			RESOURCES_LOCALIZED     = 0x0039,	// Localized Resource Direcotry
			COMMON_OEM_LINKS        = 0x003a,	// Links to All Users OEM specific apps
			CDBURN_AREA             = 0x003b,	// USERPROFILE\Local Settings\Application Data\Microsoft\CD Burning
			COMPUTERSNEARME         = 0x003d,	// Computers Near Me (computered from Workgroup membership)
			FLAG_CREATE             = 0x8000,	// combine with CSIDL_ value to force folder creation in SHGetFolderPath()
			FLAG_DONT_VERIFY        = 0x4000,	// combine with CSIDL_ value to return an unverified folder path
			FLAG_NO_ALIAS           = 0x1000,	// combine with CSIDL_ value to insure non-alias versions of the pidl
			FLAG_PER_USER_INIT      = 0x0800,	// combine with CSIDL_ value to indicate per-user init (eg. upgrade)
			FLAG_MASK               = 0xFF00,	// mask for all possible flag values
		}
		#endregion

		#region Public functions dealing with PIDLs, used by PIDLManager-Service
		[DllImport("shell32.dll")]
		public static extern IntPtr ILAppendID(IntPtr pidl, IntPtr pmkid, bool fAppend);
		[DllImport("shell32.dll")]
		public static extern IntPtr ILClone(IntPtr pidl);
		[DllImport("shell32.dll")]
		public static extern IntPtr ILCloneFirst(IntPtr pidl);
		[DllImport("shell32.dll")]
		public static extern IntPtr ILCombine(IntPtr pidl1, IntPtr pidl2);
		[DllImport("shell32.dll")]
		public static extern IntPtr ILCreateFromPathW(
			[MarshalAs(UnmanagedType.LPWStr)] 
			String pwszPath);
		[DllImport("shell32.dll")]
		public static extern IntPtr ILFindChild(IntPtr pidlParent, IntPtr pidlChild);
		[DllImport("shell32.dll")]
		public static extern IntPtr ILFindLastID(IntPtr pidl);
		[DllImport("shell32.dll")]
		public static extern IntPtr ILFree(IntPtr pidl);
		[DllImport("shell32.dll")]
		public static extern IntPtr ILGetNext(IntPtr pidl);
		[DllImport("shell32.dll")]
		public static extern UInt32 ILGetSize(IntPtr pidl);
		[DllImport("shell32.dll")]
		public static extern bool ILIsEqual(IntPtr pidl1, IntPtr pidl2);
		[DllImport("shell32.dll")]
		public static extern bool ILIsParent(IntPtr pidlParent, IntPtr pidlBelow, bool fImmediate);
		[DllImport("shell32.dll")]
		public static extern bool ILRemoveLastID(IntPtr pidl);
		#endregion

		[DllImport("shell32.dll")]
		public static extern IntPtr SHGetFileInfo(	// TODO: IntPtr eigentlich Int
			string pszPath,
			uint dwFileAttributes,
			ref SHFILEINFO psfi,
			UInt32 cbfileInfo,
			SHGFI uFlags);

		[DllImport("shell32.dll")]
		public static extern IntPtr SHGetFileInfo(	// TODO: IntPtr eigentlich Int
			IntPtr pszPath,														// TODO: Overloaded
			uint dwFileAttributes,
			ref SHFILEINFO psfi,
			UInt32 cbfileInfo,
			SHGFI uFlags);

	
		[StructLayout(LayoutKind.Sequential)]
			public struct SHFILEINFO {
			public IntPtr hIcon;
			public int iIcon;
			public uint dwAttributes;
			
			[MarshalAs(UnmanagedType.LPStr, SizeConst = 260)]
			public string szDisplayName;
			
			[MarshalAs(UnmanagedType.LPStr, SizeConst = 80)]
			public string szTypeName;
		}
		
		public enum SHGFI {
			LargeIcon			= 0x00000000,
			SmallIcon			= 0x00000001,
			OpenIcon      = 0x00000002,
			PIDL          = 0x00000008,
			Icon					= 0x00000100,
			DisplayName			= 0x00000200,
			Typename				= 0x00000400,
			SysIconIndex		= 0x00004000,
			UseFileAttributes	= 0x00000010
		}

		// Retrieves the IShellFolder interface for the desktop folder,
		//which is the root of the Shell's namespace. 
		[DllImport("Shell32.Dll")]
		public static extern Int32 SHGetDesktopFolder(
			out IntPtr ppshf);        // Address that receives an IShellFolder interface
		// pointer for the 
		// desktop folder.

		// Retrieves a pointer to the Shell's IMalloc interface.
		[DllImport("shell32.dll")]
		public static extern Int32 SHGetMalloc(
			out IntPtr hObject);    // Address of a pointer that receives the Shell's
		// IMalloc interface pointer. 

		#endregion

		public enum SHGDN : uint {
			NORMAL             = 0x0000,		// default (display purpose)
			INFOLDER           = 0x0001,		// displayed under a folder (relative)
			FOREDITING         = 0x1000,		// for in-place editing
			FORADDRESSBAR      = 0x4000,		// UI friendly parsing name (remove ugly stuff)
			FORPARSING         = 0x8000,		// parsing name for ParseDisplayName()
		}

		[ComImport]
			[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
			[Guid("00000002-0000-0000-C000-000000000046")]
			public interface IMalloc {
			// Allocates a block of memory.
			// Return value: a pointer to the allocated memory block.
			[PreserveSig]
			IntPtr Alloc(
				UInt32 cb);        // Size, in bytes, of the memory block to be allocated. 
        
			// Changes the size of a previously allocated memory block.
			// Return value:  Reallocated memory block 
			[PreserveSig]
			IntPtr Realloc(
				IntPtr pv,         // Pointer to the memory block to be reallocated.
				UInt32 cb);        // Size of the memory block (in bytes) to be reallocated.

			// Frees a previously allocated block of memory.
			[PreserveSig]
			void Free(
				IntPtr pv);        // Pointer to the memory block to be freed.

			// This method returns the size (in bytes) of a memory block previously
			// allocated with IMalloc::Alloc or IMalloc::Realloc.
			// Return value: The size of the allocated memory block in bytes 
			[PreserveSig]
			UInt32 GetSize(
				IntPtr pv);        // Pointer to the memory block for which the size
			// is requested.

			// This method determines whether this allocator was used to allocate
			// the specified block of memory.
			// Return value: 1 - allocated 0 - not allocated by this IMalloc instance. 
			[PreserveSig]
			Int16 DidAlloc(
				IntPtr pv);        // Pointer to the memory block

			// This method minimizes the heap as much as possible by releasing unused
			// memory to the operating system, 
			// coalescing adjacent free blocks and committing free pages.
			[PreserveSig]
			void HeapMinimize();
		}


		#region STRRET - Structure: String Return structure
		[StructLayout(LayoutKind.Explicit)]
			public struct STRRET {
			[FieldOffset(0)]
			public UInt32 uType;							// One of the STRRET_* values
			[FieldOffset(4)]
			public IntPtr pOleStr;						// must be freed by caller of GetDisplayNameOf
			[FieldOffset(4)]
			public IntPtr pStr;								// NOT USED
			[FieldOffset(4)]
			public UInt32 uOffset;						// Offset into SHITEMID
			[FieldOffset(4)]
			public IntPtr cStr;								// Buffer to fill in (ANSI)
		}
		#endregion

		#region STRRET_TYPE - Enumeration: STRRET - Types
		public enum STRRET_TYPE {
			WSTR   = 0x0000,									// Use STRRET.pOleStr
			OFFSET = 0x0001,									// Use STRRET.uOffset to Ansi
			CSTR   = 0x0002,									// Use STRRET.cStr
		}
		#endregion

		[ComImportAttribute()]
			[GuidAttribute("000214F2-0000-0000-C000-000000000046")]
			[InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
			//helpstring("IEnumIDList interface")
			public interface IEnumIDList {
			[PreserveSig]
			int Next(
				int celt,
				ref IntPtr rgelt,
				out int pceltFetched);

			void Skip(
				int celt);

			int Reset();

			void Clone(
				ref IEnumIDList ppenum);
		};

		#region IShellFolder - COM - Interface: Shell Folder interface
		[ComImport]
			[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
			[Guid("000214E6-0000-0000-C000-000000000046")]
			public interface IShellFolder {
			// Translates a file object's or folder's display name into an item identifier list.
			// Return value: error code, if any
			[PreserveSig]
			Int32 ParseDisplayName( 
				IntPtr hwnd,										// Optional window handle
				IntPtr pbc,											// Optional bind context that controls the
				// parsing operation. This parameter is 
				// normally set to NULL. 
				[MarshalAs(UnmanagedType.LPWStr)] 
				String pszDisplayName,					// Null-terminated UNICODE string with the
				// display name.
				ref UInt32 pchEaten,						// Pointer to a ULONG value that receives the
				// number of characters of the 
				// display name that was parsed.
				out IntPtr ppidl,								// Pointer to an ITEMIDLIST pointer that receives
				// the item identifier list for 
				// the object.
				ref UInt32 pdwAttributes);			// Optional parameter that can be used to
			// query for file attributes.
			// this can be values from the SFGAO enum
        
			// Allows a client to determine the contents of a folder by creating an item
			// identifier enumeration object and returning its IEnumIDList interface.
			// Return value: error code, if any
			[PreserveSig]
			Int32 EnumObjects( 
				IntPtr hwnd,										// If user input is required to perform the
				// enumeration, this window handle 
				// should be used by the enumeration object as
				// the parent window to take 
				// user input.
				SHCONTF grfFlags,									// Flags indicating which items to include in the
				// enumeration. For a list 
				// of possible values, see the SHCONTF enum. 
				ref IEnumIDList ppenumIDList);				// Address that receives a pointer to the
			//				out IntPtr ppenumIDList);				// Address that receives a pointer to the
			// IEnumIDList interface of the 
			// enumeration object created by this method. 

			// Retrieves an IShellFolder object for a subfolder.
			// Return value: error code, if any
			[PreserveSig]
			Int32 BindToObject( 
				IntPtr pidl,										// Address of an ITEMIDLIST structure (PIDL)
				// that identifies the subfolder.
				IntPtr pbc,											// Optional address of an IBindCtx interface on
				// a bind context object to be 
				// used during this operation.
				ref Guid riid,											// Identifier of the interface to return. 
				ref IShellFolder ppv);								// Address that receives the interface pointer.

			// Requests a pointer to an object's storage interface. 
			// Return value: error code, if any
			[PreserveSig]
			Int32 BindToStorage( 
				IntPtr pidl,										// Address of an ITEMIDLIST structure that
				// identifies the subfolder relative 
				// to its parent folder. 
				IntPtr pbc,											// Optional address of an IBindCtx interface on a
				// bind context object to be 
				// used during this operation.
				Guid riid,											// Interface identifier (IID) of the requested
				// storage interface.
				out IntPtr ppv);								// Address that receives the interface pointer specified by riid.
        
			// Determines the relative order of two file objects or folders, given their
			// item identifier lists. Return value: If this method is successful, the
			// CODE field of the HRESULT contains one of the following values (the code
			// can be retrived using the helper function GetHResultCode): Negative A
			// negative return value indicates that the first item should precede
			// the second (pidl1 < pidl2). 

			// Positive A positive return value indicates that the first item should
			// follow the second (pidl1 > pidl2).  Zero A return value of zero
			// indicates that the two items are the same (pidl1 = pidl2). 
			[PreserveSig]
			Int32 CompareIDs( 
				Int32 lParam,										// Value that specifies how the comparison
				// should be performed. The lower 
				// Sixteen bits of lParam define the sorting rule.
				// The upper sixteen bits of 
				// lParam are used for flags that modify the
				// sorting rule. values can be from 
				// the SHCIDS enum
				IntPtr pidl1,										// Pointer to the first item's ITEMIDLIST structure.
				IntPtr pidl2);									// Pointer to the second item's ITEMIDLIST structure.

			// Requests an object that can be used to obtain information from or interact
			// with a folder object.
			// Return value: error code, if any
			[PreserveSig]
			Int32 CreateViewObject( 
				IntPtr hwndOwner,								// Handle to the owner window.
				Guid riid,											// Identifier of the requested interface. 
				out IntPtr ppv);								// Address of a pointer to the requested interface. 

			// Retrieves the attributes of one or more file objects or subfolders. 
			// Return value: error code, if any
			[PreserveSig]
			Int32 GetAttributesOf( 
				UInt32 cidl,										// Number of file objects from which to retrieve
				// attributes. 
				[MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0)]
				IntPtr[] apidl,									// Address of an array of pointers to ITEMIDLIST
				// structures, each of which 
				// uniquely identifies a file object relative to
				// the parent folder.
				ref UInt32 rgfInOut);						// Address of a single ULONG value that, on entry,
			// contains the attributes that 
			// the caller is requesting. On exit, this value
			// contains the requested 
			// attributes that are common to all of the
			// specified objects. this value can
			// be from the SFGAO enum
       
			// Retrieves an OLE interface that can be used to carry out actions on the
			// specified file objects or folders.
			// Return value: error code, if any
			[PreserveSig]
			Int32 GetUIObjectOf( 
				IntPtr hwndOwner,								// Handle to the owner window that the client
				// should specify if it displays 
				// a dialog box or message box.
				UInt32 cidl,										// Number of file objects or subfolders specified
				// in the apidl parameter. 
				IntPtr[] apidl,									// Address of an array of pointers to ITEMIDLIST
				// structures, each of which 
				// uniquely identifies a file object or subfolder
				// relative to the parent folder.
				Guid riid,											// Identifier of the COM interface object to return.
				ref UInt32 rgfReserved,					// Reserved. 
				out IntPtr ppv);								// Pointer to the requested interface.

			// Retrieves the display name for the specified file object or subfolder. 
			// Return value: error code, if any
			[PreserveSig]
			Int32 GetDisplayNameOf(
				IntPtr pidl,										// Address of an ITEMIDLIST structure (PIDL)
				// that uniquely identifies the file 
				// object or subfolder relative to the parent folder. 
				SHGDN uFlags,									// Flags used to request the type of display name
				// to return. For a list of 
				// possible values, see the SHGNO enum. 
				out STRRET pName);							// Address of a STRRET structure in which to
			// return the display name.
        
			// Sets the display name of a file object or subfolder, changing the item
			// identifier in the process.
			// Return value: error code, if any
			[PreserveSig]
			Int32 SetNameOf( 
				IntPtr hwnd,										// Handle to the owner window of any dialog or
				// message boxes that the client 
				// displays.
				IntPtr pidl,										// Pointer to an ITEMIDLIST structure that uniquely
				// identifies the file object
				// or subfolder relative to the parent folder. 
				[MarshalAs(UnmanagedType.LPWStr)] 
				String pszName,									// Pointer to a null-terminated string that
				// specifies the new display name. 
				UInt32 uFlags,									// Flags indicating the type of name specified by
				// the lpszName parameter. For a list of possible
				// values, see the description of the SHGNO enum. 
				out IntPtr ppidlOut);						// Address of a pointer to an ITEMIDLIST structure
			// which receives the new ITEMIDLIST. 
		}
		#endregion

		// TODO: Warning! the following will NOT work using w2k
		// Accepts a STRRET structure returned by
		// ShellFolder::GetDisplayNameOf that contains or points to a string, and then
		// returns that string as a BSTR.
		[DllImport("shlwapi.dll")]
		public static extern Int32 StrRetToBSTR(
			ref STRRET pstr,    // Pointer to a STRRET structure.
			IntPtr pidl,        // Pointer to an ITEMIDLIST uniquely identifying a file
			// object or subfolder relative
			// to the parent folder.
			[MarshalAs(UnmanagedType.BStr)]
			out String pbstr);    // Pointer to a variable of type BSTR that contains the
		// converted string.

		public enum SHCONTF : uint {
			FOLDERS             = 0x0020,   // only want folders enumerated (SFGAO_FOLDER)
			NONFOLDERS          = 0x0040,   // include non folders
			INCLUDEHIDDEN       = 0x0080,   // show items normally hidden
			INIT_ON_FIRST_NEXT  = 0x0100,   // allow EnumObject() to return before validating enum
			NETPRINTERSRCH      = 0x0200,   // hint that client is looking for printers
			SHAREABLE           = 0x0400,   // hint that client is looking sharable resources (remote shares)
			STORAGE             = 0x0800,   // include all items with accessible storage and their ancestors
			DefaultForTreeView  = ShellAPI.SHCONTF.FOLDERS | ShellAPI.SHCONTF.INCLUDEHIDDEN,
			DefaultForListView  = ShellAPI.SHCONTF.FOLDERS | ShellAPI.SHCONTF.NONFOLDERS | ShellAPI.SHCONTF.INCLUDEHIDDEN,
		}

		public ShellAPI() {
			//
			// TODO: Fügen Sie hier die Konstruktorlogik hinzu
			//
		}
	}
}
