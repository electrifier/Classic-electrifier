/*
** 
**  electrifier
** 
**  Copyright 2018 Thorsten Jung, www.electrifier.org
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
using System.Runtime.InteropServices;

namespace common.Interop.WinShell
{
    /// <summary>
    /// 
    /// See Microsoft SDKs\Windows\v7.1\Include\ShlObj.h, section "#define INTERFACE   IShellFolderViewCB"
    /// 
    /// </summary>
    public enum SFVM : uint
    {                                 // wParam             lParam
        MERGEMENU = 1,                // -                  LPQCMINFO
        INVOKECOMMAND = 2,            // idCmd              -
        GETHELPTEXT = 3,              // idCmd,cchMax       pszText
        GETTOOLTIPTEXT = 4,           // idCmd,cchMax       pszText
        GETBUTTONINFO = 5,            // -                  LPTBINFO
        GETBUTTONS = 6,               // idCmdFirst,cbtnMax LPTBBUTTON
        INITMENUPOPUP = 7,            // idCmdFirst,nIndex  hmenu
        FSNOTIFY = 14,                // LPCITEMIDLIST*     lEvent
        WINDOWCREATED = 15,           // hwnd               -
        GETDETAILSOF = 23,            // iColumn            DETAILSINFO*
        COLUMNCLICK = 24,             // iColumn            -
        QUERYFSNOTIFY = 25,           // -                  SHChangeNotifyEntry *
        DEFITEMCOUNT = 26,            // -                  UINT*
        DEFVIEWMODE = 27,             // -                  FOLDERVIEWMODE*
        UNMERGEMENU = 28,             // -                  hmenu
        UPDATESTATUSBAR = 31,         // fInitialize        -
        BACKGROUNDENUM = 32,          // -                  -
        DIDDRAGDROP = 36,             // dwEffect           IDataObject *
        SETISFV = 39,                 // -                  IShellFolderView*
        THISIDLIST = 41,              // -                  LPITMIDLIST*
        ADDPROPERTYPAGES = 47,        // -                  SFVM_PROPPAGE_DATA *
        BACKGROUNDENUMDONE = 48,      // -                  -
        GETNOTIFY = 49,               // LPITEMIDLIST*      LONG*
        GETSORTDEFAULTS = 53,         // iDirection         iParamSort
        SIZE = 57,                    // -                  -
        GETZONE = 58,                 // -                  DWORD*
        GETPANE = 59,                 // Pane ID            DWORD*
        GETHELPTOPIC = 63,            // -                  SFVM_HELPTOPIC_DATA *
        GETANIMATION = 68,            // HINSTANCE *        WCHAR *

        //
        // Undocumented values taken from WINE project,
        // see https://github.com/wine-mirror/wine/blob/master/include/shlobj.h
        //

        SELECTIONCHANGED = 8,         /* undocumented */
        DRAWMENUITEM = 9,             /* undocumented */
        MEASUREMENUITEM = 10,         /* undocumented */
        EXITMENULOOP = 11,            /* undocumented */
        VIEWRELEASE = 12,             /* undocumented */
        GETNAMELENGTH = 13,           /* undocumented */
        WINDOWCLOSING = 16,           /* undocumented */
        LISTREFRESHED = 17,           /* undocumented */
        WINDOWFOCUSED = 18,           /* undocumented */
        REGISTERCOPYHOOK = 20,        /* undocumented */
        COPYHOOKCALLBACK = 21,        /* undocumented */
        ADDINGOBJECT = 29,            /* undocumented */
        REMOVINGOBJECT = 30,          /* undocumented */
        GETCOMMANDDIR = 33,           /* undocumented */
        GETCOLUMNSTREAM = 34,         /* undocumented */
        CANSELECTALL = 35,            /* undocumented */
        ISSTRICTREFRESH = 37,         /* undocumented */
        ISCHILDOBJECT = 38,           /* undocumented */
        GETEXTVIEWS = 40,             /* undocumented */
        GET_CUSTOMVIEWINFO = 77,      /* undocumented */
        ENUMERATEDITEMS = 79,         /* undocumented */
        GET_VIEW_DATA = 80,           /* undocumented */
        GET_WEBVIEW_LAYOUT = 82,      /* undocumented */
        GET_WEBVIEW_CONTENT = 83,     /* undocumented */
        GET_WEBVIEW_TASKS = 84,       /* undocumented */
        GET_WEBVIEW_THEME = 86,       /* undocumented */
        GETDEFERREDVIEWSETTINGS = 92, /* undocumented */
    }

    /// <summary>
    /// Exposes a method that allows communication between Windows Explorer and a folder view implemented using the system folder view object
    /// (the IShellView object returned through SHCreateShellFolderView) so that the folder view can be notified of events and modify its view accordingly.
    /// 
    /// See https://msdn.microsoft.com/en-us/library/windows/desktop/bb774967(v=vs.85).aspx
    /// </summary>
    [
        ComImport,
        Guid("2047E320-F2A9-11CE-AE65-08002B2E1262"),
        InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [System.ObsoleteAttribute("Interface common.Interop.WinShell.IShellFolderViewCB is obsolete. It will be replaced and removed.")]
    public interface IShellFolderViewCB
    {
        [PreserveSig]
        common.Interop.WinError.HResult MessageSFVCB(
            SFVM uMsg,
            IntPtr wParam,
            IntPtr lParam);
    }
}
