using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Electrifier.Win32API {

	public class ShellAPI {

		[DllImport("Shell32.Dll")]
		public static extern Int32 SHGetFolderLocation(
			IntPtr hwndOwner,
			CSIDL nFolder,
			IntPtr hToken,
			uint dwReserved,
			out IntPtr ppidl);

		/// <summary>
		/// The CSIDL-enumeration contains the special folder types, Shell32.dll-functions
		/// "SHGetFolderLocation", "SHGetFolderPath", "SHGetSpecialFolderLocation", and
		/// "SHGetSpecialFolderPath" accept to retrieve a special folder's pathname or
		/// item ID list (PIDL). Not all of them must exist on the machine of interest.
		/// </summary>
		public enum CSIDL : uint {
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
			BITBUCKET               = 0x000A,	// <desktop>\Recycle Bin
			STARTMENU               = 0x000B,	// <user name>\Start Menu
			MYDOCUMENTS             = 0x000C,	// logical "My Documents" desktop icon
			MYMUSIC                 = 0x000D,	// "My Music" folder
			MYVIDEO                 = 0x000E,	// "My Videos" folder
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
			APPDATA                 = 0x001A,	// <user name>\Application Data
			PRINTHOOD               = 0x001B,	// <user name>\PrintHood
			LOCAL_APPDATA           = 0x001C,	// <user name>\Local Settings\Applicaiton Data (non roaming)
			ALTSTARTUP              = 0x001D,	// non localized startup
			COMMON_ALTSTARTUP       = 0x001E,	// non localized common startup
			COMMON_FAVORITES        = 0x001F,
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
			PROGRAM_FILESX86        = 0x002A,	// x86 C:\Program Files on RISC
			PROGRAM_FILES_COMMON    = 0x002B,	// C:\Program Files\Common
			PROGRAM_FILES_COMMONX86 = 0x002C,	// x86 Program Files\Common on RISC
			COMMON_TEMPLATES        = 0x002D,	// All Users\Templates
			COMMON_DOCUMENTS        = 0x002E,	// All Users\Documents
			COMMON_ADMINTOOLS       = 0x002F,	// All Users\Start Menu\Programs\Administrative Tools
			ADMINTOOLS              = 0x0030,	// <user name>\Start Menu\Programs\Administrative Tools
			CONNECTIONS             = 0x0031,	// Network and Dial-up Connections
			COMMON_MUSIC            = 0x0035,	// All Users\My Music
			COMMON_PICTURES         = 0x0036,	// All Users\My Pictures
			COMMON_VIDEO            = 0x0037,	// All Users\My Video
			RESOURCES               = 0x0038,	// Resource Direcotry
			RESOURCES_LOCALIZED     = 0x0039,	// Localized Resource Direcotry
			COMMON_OEM_LINKS        = 0x003A,	// Links to All Users OEM specific apps
			CDBURN_AREA             = 0x003B,	// USERPROFILE\Local Settings\Application Data\Microsoft\CD Burning
			COMPUTERSNEARME         = 0x003D,	// Computers Near Me (computered from Workgroup membership)
			FLAG_CREATE             = 0x8000,	// combine with CSIDL_ value to force folder creation in SHGetFolderPath()
			FLAG_DONT_VERIFY        = 0x4000,	// combine with CSIDL_ value to return an unverified folder path
			FLAG_NO_ALIAS           = 0x1000,	// combine with CSIDL_ value to insure non-alias versions of the pidl
			FLAG_PER_USER_INIT      = 0x0800,	// combine with CSIDL_ value to indicate per-user init (eg. upgrade)
			FLAG_MASK               = 0xFF00,	// mask for all possible flag values
		}

		[Flags]
			public enum CMF : uint {			// QueryContextMenu uFlags
			NORMAL              = 0x00000000,
			DEFAULTONLY         = 0x00000001,
			VERBSONLY           = 0x00000002,
			EXPLORE             = 0x00000004,
			NOVERBS             = 0x00000008,
			CANRENAME           = 0x00000010,
			NODEFAULT           = 0x00000020,
			INCLUDESTATIC       = 0x00000040,
			EXTENDEDVERBS       = 0x00000100,     // rarely used verbs
			RESERVED            = 0xffff0000,     // View specific
		}

		// All the PIDL-functions
		[DllImport("shell32.dll")]
		public static extern IntPtr ILAppendID(IntPtr pidl, IntPtr pmkid, bool fAppend);
		[DllImport("shell32.dll")]
		public static extern IntPtr ILClone(IntPtr pidl);
		[DllImport("shell32.dll")]
		public static extern IntPtr ILCloneFirst(IntPtr pidl);
		[DllImport("shell32.dll")]
		public static extern IntPtr ILCombine(IntPtr pidl1, IntPtr pidl2);
		[DllImport("shell32.dll")]
		public static extern IntPtr ILCreateFromPathW([MarshalAs(UnmanagedType.LPWStr)] String pwszPath);
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
		public static extern bool   ILIsEqual(IntPtr pidl1, IntPtr pidl2);
		[DllImport("shell32.dll")]
		public static extern bool   ILIsParent(IntPtr pidlParent, IntPtr pidlBelow, bool fImmediate);
		[DllImport("shell32.dll")]
		public static extern bool   ILRemoveLastID(IntPtr pidl);

		[DllImport("shell32.dll")]
		public static extern IntPtr SHGetFileInfo(	// TODO: IntPtr eigentlich Int
			string pszPath,
			uint dwFileAttributes,
			out SHFILEINFO psfi,
			UInt32 cbfileInfo,
			SHGFI uFlags);

		[DllImport("shell32.dll")]
		public static extern IntPtr SHGetFileInfo(	// TODO: IntPtr eigentlich Int
			IntPtr pszPath,														// TODO: Overloaded
			uint dwFileAttributes,
			out SHFILEINFO psfi,
			UInt32 cbfileInfo,
			SHGFI uFlags);


		[StructLayout(LayoutKind.Sequential)]
			public struct SHFILEINFO {
			public IntPtr hIcon;
			public int iIcon;
			public SFGAO dwAttributes;
			[MarshalAs(UnmanagedType.LPStr, SizeConst = 260)]
			public string szDisplayName;
			[MarshalAs(UnmanagedType.LPStr, SizeConst = 80)]
			public string szTypeName;
		}

		[StructLayout(LayoutKind.Sequential)]
			public struct SHDRAGIMAGE {
			public WinAPI.SIZE     sizeDragImage;
			public WinAPI.POINT    ptOffset;
			public IntPtr          hbmpDragImage;
			public WinAPI.COLORREF crColorKey;
		}

		[StructLayout(LayoutKind.Sequential)]
			public struct FOLDERSETTINGS {
			public ShellAPI.FOLDERVIEWMODE ViewMode;
			public ShellAPI.FOLDERFLAGS    Flags;

			public FOLDERSETTINGS(FOLDERVIEWMODE ViewMode, FOLDERFLAGS Flags) {
				this.ViewMode = ViewMode;
				this.Flags    = Flags;
			}
		};

		[StructLayout(LayoutKind.Sequential)]
			public struct CMINVOKECOMMANDINFO {
			public int cbSize;
			public uint fMask;
			public IntPtr hwnd;
			public IntPtr /* LPCSTR */ lpVerb;
			public IntPtr /* LPCSTR */ lpParameters;
			public IntPtr /* LPCSTR */lpDirectory;
			public Win32API.SW nShow;
			public uint dwHotKey;
			public IntPtr hIcon;
		}

		public enum FOLDERVIEWMODE : uint {
			FIRST      = 1,
			ICON       = 1,
			SMALLICON  = 2,
			LIST       = 3,
			DETAILS    = 4,
			THUMBNAIL  = 5,
			TILE       = 6,
			THUMBSTRIP = 7,
			LAST       = 7,
		};

		public enum FOLDERFLAGS : uint {
			AUTOARRANGE	        = 0x00000001,
			ABBREVIATEDNAMES	  = 0x00000002,
			SNAPTOGRID	        = 0x00000004,
			OWNERDATA	          = 0x00000008,
			BESTFITWINDOW	      = 0x00000010,
			DESKTOP	            = 0x00000020,
			SINGLESEL	          = 0x00000040,
			NOSUBFOLDERS	      = 0x00000080,
			TRANSPARENT	        = 0x00000100,
			NOCLIENTEDGE	      = 0x00000200,
			NOSCROLL	          = 0x00000400,
			ALIGNLEFT	          = 0x00000800,
			NOICONS	            = 0x00001000,
			SHOWSELALWAYS	      = 0x00002000,
			NOVISIBLE	          = 0x00004000,
			SINGLECLICKACTIVATE	= 0x00008000,
			NOWEBVIEW	          = 0x00010000,
			HIDEFILENAMES	      = 0x00020000,
			CHECKSELECT	        = 0x00040000,
		};

		public enum SVUIA : uint {
			DEACTIVATE	      = 0,
			ACTIVATE_NOFOCUS	= 1,
			ACTIVATE_FOCUS	  = 2,
			INPLACEACTIVATE	  = 3,
		};

		/// <summary>
		/// Flags for SHGetFileInfo function
		/// 
		/// See https://msdn.microsoft.com/en-us/library/windows/desktop/bb762179(v=vs.85).aspx
		/// 
		/// The flags that specify the file information to retrieve. This parameter can be a combination of the following values.
		///
		/// SHGFI_ADDOVERLAYS (0x000000020)
		/// 	Version 5.0. Apply the appropriate overlays to the file's icon. The SHGFI_ICON flag must also be set.
		///
		/// SHGFI_ATTR_SPECIFIED (0x000020000)
		/// 	Modify SHGFI_ATTRIBUTES to indicate that the dwAttributes member of the SHFILEINFO structure at psfi contains the specific attributes that are desired. These attributes are passed to IShellFolder::GetAttributesOf. If this flag is not specified, 0xFFFFFFFF is passed to IShellFolder::GetAttributesOf, requesting all attributes. This flag cannot be specified with the SHGFI_ICON flag.
		///
		/// SHGFI_ATTRIBUTES (0x000000800)
		/// 	Retrieve the item attributes. The attributes are copied to the dwAttributes member of the structure specified in the psfi parameter. These are the same attributes that are obtained from IShellFolder::GetAttributesOf.
		///
		/// SHGFI_DISPLAYNAME (0x000000200)
		/// 	Retrieve the display name for the file, which is the name as it appears in Windows Explorer. The name is copied to the szDisplayName member of the structure specified in psfi. The returned display name uses the long file name, if there is one, rather than the 8.3 form of the file name. Note that the display name can be affected by settings such as whether extensions are shown.
		///
		/// SHGFI_EXETYPE (0x000002000)
		/// 	Retrieve the type of the executable file if pszPath identifies an executable file. The information is packed into the return value. This flag cannot be specified with any other flags.
		///
		/// SHGFI_ICON (0x000000100)
		/// 	Retrieve the handle to the icon that represents the file and the index of the icon within the system image list. The handle is copied to the hIcon member of the structure specified by psfi, and the index is copied to the iIcon member.
		///
		/// SHGFI_ICONLOCATION (0x000001000)
		/// 	Retrieve the name of the file that contains the icon representing the file specified by pszPath, as returned by the IExtractIcon::GetIconLocation method of the file's icon handler. Also retrieve the icon index within that file. The name of the file containing the icon is copied to the szDisplayName member of the structure specified by psfi. The icon's index is copied to that structure's iIcon member.
		///
		/// SHGFI_LARGEICON (0x000000000)
		/// 	Modify SHGFI_ICON, causing the function to retrieve the file's large icon. The SHGFI_ICON flag must also be set.
		///
		/// SHGFI_LINKOVERLAY (0x000008000)
		/// 	Modify SHGFI_ICON, causing the function to add the link overlay to the file's icon. The SHGFI_ICON flag must also be set.
		///
		/// SHGFI_OPENICON (0x000000002)
		/// 	Modify SHGFI_ICON, causing the function to retrieve the file's open icon. Also used to modify SHGFI_SYSICONINDEX, causing the function to return the handle to the system image list that contains the file's small open icon. A container object displays an open icon to indicate that the container is open. The SHGFI_ICON and/or SHGFI_SYSICONINDEX flag must also be set.
		///
		/// SHGFI_OVERLAYINDEX (0x000000040)
		/// 	Version 5.0. Return the index of the overlay icon. The value of the overlay index is returned in the upper eight bits of the iIcon member of the structure specified by psfi. This flag requires that the SHGFI_ICON be set as well.
		///
		/// SHGFI_PIDL (0x000000008)
		/// 	Indicate that pszPath is the address of an ITEMIDLIST structure rather than a path name.
		///
		/// SHGFI_SELECTED (0x000010000)
		/// 	Modify SHGFI_ICON, causing the function to blend the file's icon with the system highlight color. The SHGFI_ICON flag must also be set.
		///
		/// SHGFI_SHELLICONSIZE (0x000000004)
		/// 	Modify SHGFI_ICON, causing the function to retrieve a Shell-sized icon. If this flag is not specified the function sizes the icon according to the system metric values. The SHGFI_ICON flag must also be set.
		///
		/// SHGFI_SMALLICON (0x000000001)
		/// 	Modify SHGFI_ICON, causing the function to retrieve the file's small icon. Also used to modify SHGFI_SYSICONINDEX, causing the function to return the handle to the system image list that contains small icon images. The SHGFI_ICON and/or SHGFI_SYSICONINDEX flag must also be set.
		///
		/// SHGFI_SYSICONINDEX (0x000004000)
		/// 	Retrieve the index of a system image list icon. If successful, the index is copied to the iIcon member of psfi. The return value is a handle to the system image list. Only those images whose indices are successfully copied to iIcon are valid. Attempting to access other images in the system image list will result in undefined behavior.
		///
		/// SHGFI_TYPENAME (0x000000400)
		/// 	Retrieve the string that describes the file's type. The string is copied to the szTypeName member of the structure specified in psfi.
		///
		/// SHGFI_USEFILEATTRIBUTES (0x000000010)
		/// 	Indicates that the function should not attempt to access the file specified by pszPath. Rather, it should act as if the file specified by pszPath exists with the file attributes passed in dwFileAttributes. This flag cannot be combined with the SHGFI_ATTRIBUTES, SHGFI_EXETYPE, or SHGFI_PIDL flags.
		/// </summary>

		[Flags]
		public enum SHGFI : uint {
			Icon = 0x100,
			DisplayName = 0x200,
			TypeName = 0x400,
			Attributes = 0x800,
			IconLocation = 0x1000,
			ExeType = 0x2000,
			SysIconIndex = 0x4000,
			LinkOverlay = 0x8000,
			Selected = 0x10000,
			Attr_Specified = 0x20000,
			LargeIcon = 0x0,
			SmallIcon = 0x1,
			OpenIcon = 0x2,
			ShellIconSize = 0x4,
			PIDL = 0x8,
			UseFileAttributes = 0x10,
			AddOverlays = 0x20,
			OverlayIndex = 0x40,
		}

		/// <summary>
		/// Flags for IShellBrowser::BrowseObject method
		/// 
		/// See https://msdn.microsoft.com/en-us/library/windows/desktop/bb775113(v=vs.85).aspx
		/// 
		/// Flags specifying the folder to be browsed. It can be zero or one or more of the following values.
		/// 
		/// These flags specify whether another window is to be created.
		/// 
		/// SBSP_DEFBROWSER (0x0000)
		///		Use default behavior, which respects the view option (the user setting to create new windows or to browse in place). In most cases, calling applications should use this flag.
		/// SBSP_SAMEBROWSER
		///		Browse to another folder with the same Windows Explorer window.
		/// SBSP_NEWBROWSER
		///		Creates another window for the specified folder.
		/// 
		/// The following flags specify the mode. These values are ignored if SBSP_SAMEBROWSER is specified or if SBSP_DEFBROWSER is specified and the user has selected Browse In Place.
		/// 
		/// SBSP_DEFMODE
		///		Use the current window.
		/// SBSP_OPENMODE
		///		Specifies no folder tree for the new browse window. If the current browser does not match the SBSP_OPENMODE of the browse object call, a new window is opened.
		/// SBSP_EXPLOREMODE
		///		Specifies a folder tree for the new browse window. If the current browser does not match the SBSP_EXPLOREMODE of the browse object call, a new window is opened.
		/// SBSP_HELPMODE
		///		Not supported. Do not use.
		/// SBSP_NOTRANSFERHIST
		///		Do not transfer the browsing history to the new window.
		/// 
		/// The following flags specify the category of the pidl parameter.
		/// 
		/// SBSP_ABSOLUTE
		///		An absolute PIDL, relative to the desktop.
		/// SBSP_RELATIVE
		///		A relative PIDL, relative to the current folder.
		/// SBSP_PARENT
		///		Browse the parent folder, ignore the PIDL.
		/// SBSP_NAVIGATEBACK
		///		Navigate back, ignore the PIDL.
		/// SBSP_NAVIGATEFORWARD
		///		Navigate forward, ignore the PIDL.
		/// SBSP_ALLOW_AUTONAVIGATE (0x00010000)
		///		Enable auto-navigation.
		/// 
		/// The following flags specify mode.
		/// 
		/// SBSP_KEEPSAMETEMPLATE (0x00020000)
		///		Windows Vista and later. Not supported. Do not use.
		/// SBSP_KEEPWORDWHEELTEXT (0x00040000)
		///		Windows Vista and later. Navigate without clearing the search entry field.
		/// SBSP_ACTIVATE_NOFOCUS (0x00080000)
		///		Windows Vista and later. Navigate without the default behavior of setting focus into the new view.
		///	
		/// The following flags control how history is manipulated as a result of navigation.
		/// 
		/// SBSP_CALLERUNTRUSTED (0x00800000)
		///		Microsoft Internet Explorer 6 Service Pack 2 (SP2) and later. The navigation was possibly initiated by a webpage with scripting code already present on the local system.
		/// SBSP_TRUSTFIRSTDOWNLOAD (0x01000000)
		///		Microsoft Internet Explorer 6 Service Pack 2 (SP2) and later. The new window is the result of a user initiated action. Trust the new window if it immediately attempts to download content.
		/// SBSP_UNTRUSTEDFORDOWNLOAD (0x02000000)
		///		Microsoft Internet Explorer 6 Service Pack 2 (SP2) and later. The window is navigating to an untrusted, non-HTML file. If the user attempts to download the file, do not allow the download.
		/// SBSP_NOAUTOSELECT
		///		Suppress selection in the history pane.
		/// SBSP_WRITENOHISTORY
		///		Write no history of this navigation in the history Shell folder.
		/// SBSP_CREATENOHISTORY (0x00100000)
		///		Windows 7 and later. Do not add a new entry to the travel log. When the user enters a search term in the search box and subsequently refines the query, the browser navigates forward but does not add an additional travel log entry.
		/// SBSP_TRUSTEDFORACTIVEX (0x10000000)
		///		Microsoft Internet Explorer 6 Service Pack 2 (SP2) and later. The navigate should allow ActiveX prompts.
		/// SBSP_FEEDNAVIGATION (0x20000000)
		///		Windows Internet Explorer 7 and later. If allowed by current registry settings, give the browser a destination to navigate to.
		/// SBSP_REDIRECT (0x40000000)
		///		Enables redirection to another URL.
		/// SBSP_INITIATEDBYHLINKFRAME (0x80000000)
		/// SBSP_PLAYNOSOUND (0x00200000)
		///		Windows 7 and later. Do not make the navigation complete sound for each keystroke in the search box.
		/// </summary>

		[Flags]
		public enum SBSP : uint {
			None = 0x0,
			DefBrowser = 0x0,
			SameBrowser = 0x1,
			NewBrowser = 0x2,
			DefMode = 0x0,
			OpenMode = 0x10,
			ExploreMode = 0x20,
			//HelpMode = 0x40, // Not supported. Do not use. (IEUNIX : Help window uses this.)
			NoTransferHist = 0x80,
			Absolute = 0x0,
			Relative = 0x1000,
			Parent = 0x2000,
			NavigateBack = 0x4000,
			NavigateForward = 0x8000,
			Allow_AutoNavigate = 0x10000,
			NoAutoSelect = 0x4000000,
			WriteNoHistory = 0x8000000,
			Redirect = 0x40000000,
			InitiatedByHLinkFrame = 0x80000000,
		}

		/// <summary>
		/// Attributes that can be retrieved on an item (file or folder) or set of items.
		/// 
		/// See https://msdn.microsoft.com/en-us/library/windows/desktop/bb762589(v=vs.85).aspx
		/// 
		/// 
		/// </summary> 

		[Flags]
		public enum SFGAO : uint {
			CanCopy = 0x1,
			CanMove = 0x2,
			CanLink = 0x4,
			Storage = 0x8,
			CanRename = 0x10,
			CanDelete = 0x20,
			HasPropSheet = 0x40,
			DropTarget = 0x100,
			CAPABILITYMASK = 0x177,
			Encrypted = 0x2000,
			IsSlow = 0x4000,
			Ghosted = 0x8000,
			Link = 0x10000,
			Share = 0x20000,
			ReadOnly = 0x40000,
			Hidden = 0x80000,
			DISPLAYATTRMASK = 0xFC000,
			FileSysAncestor = 0x10000000,
			Folder = 0x20000000,
			FileSystem = 0x40000000,
			HasSubfolder = 0x80000000,
			CONTENTSMASK = 0x80000000,
			Validate = 0x1000000,
			Removable = 0x2000000,
			Compressed = 0x4000000,
			Browsable = 0x8000000,
			Nonenumerated = 0x100000,
			NewContent = 0x200000,
			Stream = 0x400000,
			StorageAncestor = 0x800000,
			STORAGECAPMASK = 0x70C50008,
		}

		[DllImport("shell32.dll")]
		public static extern Int32 SHGetDesktopFolder(
			// TODO: [MarshalAs(UnmanagedType.IUnknown)]
			out IntPtr ppshf);

		[DllImport("shell32.dll")]
		public static extern Int32 SHGetMalloc(
			// TODO: [MarshalAs(UnmanagedType.IUnknown)]
			out IntPtr hObject);


		public enum SHGDN : uint {
			NORMAL             = 0x0000,		// default (display purpose)
			INFOLDER           = 0x0001,		// displayed under a folder (relative)
			FOREDITING         = 0x1000,		// for in-place editing
			FORADDRESSBAR      = 0x4000,		// UI friendly parsing name (remove ugly stuff)
			FORPARSING         = 0x8000,		// parsing name for ParseDisplayName()
		}

		/// <summary>
		/// System interface id for IMalloc interface
		/// </summary>
		public const string GuidString_IMalloc = "00000002-0000-0000-C000-000000000046";
		public static Guid IID_IMalloc = new Guid(GuidString_IMalloc);

		[ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("00000002-0000-0000-C000-000000000046")]
			public interface IMalloc {
			[PreserveSig] IntPtr Alloc(uint cb);
			[PreserveSig] IntPtr Realloc(IntPtr pv, uint cb);
			[PreserveSig] void Free(IntPtr pv);
			[PreserveSig] uint GetSize(IntPtr pv);
			[PreserveSig] Int16 DidAlloc(IntPtr pv);
			[PreserveSig] void HeapMinimize();
		}


		public const string IIDS_IContextMenu = "000214E4-0000-0000-C000-000000000046";
		public static Guid IID_IContextMenu = new Guid(IIDS_IContextMenu);

		[ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid(IIDS_IContextMenu)]
			public interface IContextMenu {
			[PreserveSig] uint QueryContextMenu(IntPtr hmenu, uint indexMenu, uint idCmdFirst, uint idCmdLast, CMF uFlags);
			[PreserveSig] uint InvokeCommand(ref CMINVOKECOMMANDINFO CMInvokeCommandInfo);
			[PreserveSig] uint GetCommandString(IntPtr idCmd, uint uType, uint pwReserved,
				[MarshalAs(UnmanagedType.LPStr)] string pszName, uint cchMax);
		}



		public const string IIDS_IOleWindow = "00000114-0000-0000-C000-000000000046";
		public static Guid IID_IOleWindow = new Guid(IIDS_IOleWindow);

		[ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid(IIDS_IOleWindow)]
			public interface IOleWindow {
			[PreserveSig] uint GetWindow(out IntPtr HWND);
			[PreserveSig] uint ContextSensitiveHelp(bool EnterMode);
		}
                                              
		public const string IIDS_IShellBrowser = "000214E2-0000-0000-C000-000000000046";
		public static Guid IID_IShellBrowser = new Guid(IIDS_IShellBrowser);

		[ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid(IIDS_IShellBrowser)]
			public interface IShellBrowser : IOleWindow {
			// IOleWindow-Member
			[PreserveSig] new uint GetWindow(out IntPtr HWND);
			[PreserveSig] new uint ContextSensitiveHelp(bool EnterMode);
			// End of IOleWindow-Member

			[PreserveSig] uint InsertMenusSB(IntPtr hmenuShared, ref IntPtr /* LPOLEMENUGROUPWIDTHS */ lpMenuWidths);
			[PreserveSig] uint SetMenuSB(IntPtr hmenuShared, IntPtr /* HOLEMENU */ holemenuRes, IntPtr hwndActiveObject);
			[PreserveSig] uint RemoveMenusSB(IntPtr hmenuShared);
			[PreserveSig] uint SetStatusTextSB([MarshalAs(UnmanagedType.LPWStr)] String pszStatusText);
			[PreserveSig] uint EnableModelessSB(bool fEnable);
			[PreserveSig] uint TranslateAcceleratorSB(uint pmsg, UInt16 wID);
			[PreserveSig] uint BrowseObject(IntPtr pidl, SBSP Flags);
			[PreserveSig] uint GetViewStateStream(uint /* DWORD */ grfMode, out IntPtr ppStrm);
			[PreserveSig] uint GetControlWindow(uint id, out IntPtr phwnd);
			[PreserveSig] uint SendControlMsg(uint id, uint uMsg, uint wParam, uint lParam, uint pret);
			[PreserveSig] uint QueryActiveShellView(out IntPtr ppshv);
			[PreserveSig] uint OnViewWindowActive(IntPtr pshv);
			[PreserveSig] uint SetToolbarItems(IntPtr /* LPTBBUTTONSB */ lpButtons, uint nButtons, uint uFlags);
		}

		public const string IIDS_IShellView = "000214E3-0000-0000-C000-000000000046";
		public static Guid IID_IShellView = new Guid(IIDS_IShellView);

		[ComImport,
			InterfaceType(ComInterfaceType.InterfaceIsIUnknown), 
			Guid(IIDS_IShellView)]
			public interface IShellView : IOleWindow {
			// IOleWindow-Member
			[PreserveSig] new uint GetWindow(out IntPtr HWND);
			[PreserveSig] new uint ContextSensitiveHelp(bool EnterMode);
			// End of IOleWindow-Member

			[PreserveSig] uint TranslateAcceleratorA(IntPtr Msg);
			[PreserveSig] uint EnableModeless(bool Enable);
			[PreserveSig] uint UIActivate(SVUIA State);
			[PreserveSig] uint Refresh();
			[PreserveSig] uint CreateViewWindow(IShellView psvPrevious, ref FOLDERSETTINGS pfs,
			                     IShellBrowser psb, ref RECT prcView, out IntPtr phWnd);
			[PreserveSig] uint DestroyViewWindow();
			[PreserveSig] uint GetCurrentInfo(out FOLDERSETTINGS pfs);
			[PreserveSig] uint AddPropertySheetPages(uint dwReserved, IntPtr /*LPFNSVADDPROPSHEETPAGE */ pfn, IntPtr /* LPARAM */ lparam);
			[PreserveSig] uint SaveViewState();
			[PreserveSig] uint SelectItem(IntPtr pidlItem, IntPtr /* SVSIF */ uFlags);
			[PreserveSig] uint GetItemObject(uint uItem, ref Guid riid, IntPtr ppv);
		};

		[StructLayout(LayoutKind.Explicit)]
			public struct STRRET {
			[FieldOffset(0)] public UInt32 uType;							// One of the STRRET_* values
			[FieldOffset(4)] public IntPtr pOleStr;						// must be freed by caller of GetDisplayNameOf
			[FieldOffset(4)] public IntPtr pStr;								// NOT USED
			[FieldOffset(4)] public UInt32 uOffset;						// Offset into SHITEMID
			[FieldOffset(4)] public IntPtr cStr;								// Buffer to fill in (ANSI)
		}

		public enum STRRET_TYPE {
			WSTR   = 0x0000,									// Use STRRET.pOleStr
			OFFSET = 0x0001,									// Use STRRET.uOffset to Ansi
			CSTR   = 0x0002,									// Use STRRET.cStr
		}

		[ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("000214F2-0000-0000-C000-000000000046")]
			public interface IEnumIDList {
			[PreserveSig]
			int Next(
				int celt,
				ref IntPtr rgelt,
				out int pceltFetched);

			[PreserveSig]
			void Skip(
				int celt);

			[PreserveSig]
			int Reset();

			[PreserveSig]
			void Clone(
				ref IEnumIDList ppenum);
		};


		/// <summary>
		/// System interface id for IDropTargetHelper interface
		/// </summary>
		public static Guid CLSID_DragDropHelper  = new Guid("{4657278A-411B-11d2-839A-00C04FD918D0}");

		public static Guid IID_IDropTargetHelper = new Guid("{4657278B-411B-11d2-839A-00C04FD918D0}");
		public static Guid IID_IDragSourceHelper = new Guid("{DE5BF786-477A-11D2-839D-00C04FD918D0}");
		public static Guid IID_IDataObject       = new Guid("{0000010e-0000-0000-C000-000000000046}");
		public static Guid IID_IDropTarget       = new Guid("{00000122-0000-0000-C000-000000000046}");

		[ComImport]																							// IDropTargetHelper
			[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
			[Guid("4657278B-411B-11d2-839A-00C04FD918D0")]
			public interface IDropTargetHelper {
			[PreserveSig]		// TODO: pDataObject       => IDataObject*
				// TODO: Int32 DragEnter() => HRESULT DragEnter()
				// TODO: Int32 dwEffect    => DWORD dwEffect
			Int32 DragEnter(IntPtr hwndTarget, WinAPI.IDataObject pDataObject, ref WinAPI.POINT ppt, DragDropEffects dwEffect);
			[PreserveSig]		// TODO: Int32 DragLeave() => HRESULT DragLeave()
			Int32 DragLeave();
			[PreserveSig]		// TODO: Int32 DragOver()  => HRESULT DragOver()
				// TODO: Int32 dwEffect    => DWORD dwEffect
			Int32 DragOver(ref WinAPI.POINT ppt, DragDropEffects dwEffect);
			[PreserveSig]		// TODO: pDataObject       => IDataObject*
				// TODO: Int32 Drop()      => HRESULT Drop()
				// TODO: Int32 dwEffect    => DWORD dwEffect
			Int32 Drop(IntPtr pDataObject, ref WinAPI.POINT ppt, DragDropEffects dwEffect);
			[PreserveSig]		// TODO: Int32 Show()      => HRESULT Show()
			Int32 Show(bool fShow);
		}

		[ComImport]																							// IDragSourceHelper
			[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
			[Guid("DE5BF786-477A-11D2-839D-00C04FD918D0")]
			public interface IDragSourceHelper {
			[PreserveSig]		// TODO: Int32 InitializeFromBitmap()      => HRESULT InitializeFromBitmap()
			int InitializeFromBitmap(SHDRAGIMAGE pshdi, WinAPI.IDataObject pDataObject);
			[PreserveSig]		// TODO: Int32 InitializeFromWindow()      => HRESULT InitializeFromWindow()
			int InitializeFromWindow(IntPtr hwnd, ref WinAPI.POINT ppt,	WinAPI.IDataObject pDataObject);
		}




		/// <summary>
		/// System interface id for IShellFolder interface
		/// </summary>
		public const string IIDS_IShellFolder = "000214E6-0000-0000-C000-000000000046";
		public static Guid IID_IShellFolder = new Guid(IIDS_IShellFolder);

		[ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid(IIDS_IShellFolder)]
			public interface IShellFolder {
			[PreserveSig]
			Int32 ParseDisplayName(
				IntPtr hwnd,
				IntPtr pbc,
				[MarshalAs(UnmanagedType.LPWStr)]
				String pszDisplayName,
				ref UInt32 pchEaten,
				out IntPtr ppidl,
				ref UInt32 pdwAttributes);

			[PreserveSig]
			Int32 EnumObjects(
				IntPtr hwnd,
				SHCONTF grfFlags,
				ref IEnumIDList ppenumIDList);

			[PreserveSig]
			Int32 BindToObject(
				IntPtr pidl,
				IntPtr pbc,
				ref Guid riid,
				ref IShellFolder ppv);

			[PreserveSig]
			Int32 BindToStorage(
				IntPtr pidl,
				IntPtr pbc,
				ref Guid riid,
				out IntPtr ppv);

			[PreserveSig]
			Int32 CompareIDs(
				Int32 lParam,
				IntPtr pidl1,
				IntPtr pidl2);

			[PreserveSig]
			Int32 CreateViewObject(
				IntPtr hwndOwner,
				ref Guid riid,
				out IntPtr ppv);

			[PreserveSig]
			Int32 GetAttributesOf(
				UInt32 cidl,
				[MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0)]
				IntPtr[] apidl,
				ref UInt32 rgfInOut);

			[PreserveSig]
			Int32 GetUIObjectOf(
				IntPtr hwndOwner,
				UInt32 cidl,
				[MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0)]
				IntPtr[] apidl,
				ref Guid riid,
				ref UInt32 rgfReserved,
				//				out object ppv);
				//			// TODO:	[MarshalAs(UnmanagedType.IUnknown)]
				out IntPtr ppv);

			[PreserveSig]
			Int32 GetDisplayNameOf(
				IntPtr pidl,
				SHGDN uFlags,
				out STRRET pName);

			[PreserveSig]
			Int32 SetNameOf(
				IntPtr hwnd,
				IntPtr pidl,
				[MarshalAs(UnmanagedType.LPWStr)]
				String pszName,
				UInt32 uFlags,
				out IntPtr ppidlOut);
		}

		// TODO: Warning! the following will NOT work using w2k
		// Accepts a STRRET structure returned by
		// ShellFolder::GetDisplayNameOf that contains or points to a string, and then
		// returns that string as a BSTR.
		[DllImport("shlwapi.dll")]
		public static extern Int32 StrRetToBSTR(
			ref STRRET pstr,
			IntPtr pidl,
			[MarshalAs(UnmanagedType.BStr)]
			out String pbstr);

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

		private ShellAPI() {
			// No instantion allowed.
		}
	}
}
