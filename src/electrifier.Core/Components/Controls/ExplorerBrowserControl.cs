﻿/*
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

/*
** Initial conversion from Windows-API-Pack to Vanara done by David Hall,
** <see href="https://github.com/dahall/Vanara/blob/master/WIndows.Forms/Controls/ExplorerBrowser.cs">
**
** Original license information:

MIT License

Copyright (c) 2017 David Hall

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/


// Heavily leverages the work done by Microsoft on the control by the same name in WindowsVistaApiPack. Work was done to improve the designer
// experience, remove nested properties, add missing capabilities, simplify COM calls, align names to those in other mainstream controls, and
// use the Vanara libraries.

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Vanara.Extensions;
using Vanara.PInvoke;
using Vanara.Windows.Shell;
using static Vanara.PInvoke.Shell32;
using static Vanara.PInvoke.ShlwApi;
using static Vanara.PInvoke.User32_Gdi;
using IServiceProvider = Vanara.PInvoke.Shell32.IServiceProvider;

namespace electrifier.Core.Components.Controls
{
	/// <summary>
	/// Indicates the content options of the explorer browser. Typically use one, or a bitwise combination of these flags to specify how
	/// content should appear in the explorer browser control
	/// </summary>
	[Flags]
	public enum ExplorerBrowserContentSectionOptions : uint
	{
		/// <summary>No options.</summary>
		None = FOLDERFLAGS.FWF_NONE,

		/// <summary>The view should be left-aligned.</summary>
		AlignLeft = FOLDERFLAGS.FWF_ALIGNLEFT,

		/// <summary>Automatically arrange the elements in the view.</summary>
		AutoArrange = FOLDERFLAGS.FWF_AUTOARRANGE,

		/// <summary>Turns on check mode for the view</summary>
		CheckSelect = FOLDERFLAGS.FWF_CHECKSELECT,

		/// <summary>When the view is set to "Tile" the layout of a single item should be extended to the width of the view.</summary>
		ExtendedTiles = FOLDERFLAGS.FWF_EXTENDEDTILES,

		/// <summary>When an item is selected, the item and all its sub-items are highlighted.</summary>
		FullRowSelect = FOLDERFLAGS.FWF_FULLROWSELECT,

		/// <summary>The view should not display file names</summary>
		HideFileNames = FOLDERFLAGS.FWF_HIDEFILENAMES,

		/// <summary>The view should not save view state in the browser.</summary>
		NoBrowserViewState = FOLDERFLAGS.FWF_NOBROWSERVIEWSTATE,

		/// <summary>Do not display a column header in the view in any view mode.</summary>
		NoColumnHeader = FOLDERFLAGS.FWF_NOCOLUMNHEADER,

		/// <summary>Only show the column header in details view mode.</summary>
		NoHeaderInAllViews = FOLDERFLAGS.FWF_NOHEADERINALLVIEWS,

		/// <summary>The view should not display icons.</summary>
		NoIcons = FOLDERFLAGS.FWF_NOICONS,

		/// <summary>Do not show subfolders.</summary>
		NoSubfolders = FOLDERFLAGS.FWF_NOSUBFOLDERS,

		/// <summary>Navigate with a single click</summary>
		SingleClickActivate = FOLDERFLAGS.FWF_SINGLECLICKACTIVATE,

		/// <summary>Do not allow more than a single item to be selected.</summary>
		SingleSelection = FOLDERFLAGS.FWF_SINGLESEL,

		/// <summary>
		/// Make the folder behave like the desktop. This value applies only to the desktop and is not used for typical Shell folders.
		/// </summary>
		Desktop = FOLDERFLAGS.FWF_DESKTOP,

		/// <summary>Draw transparently. This is used only for the desktop.</summary>
		Transparent = FOLDERFLAGS.FWF_TRANSPARENT,

		/// <summary>Do not add scroll bars. This is used only for the desktop.</summary>
		NoScrollBars = FOLDERFLAGS.FWF_NOSCROLL,

		/// <summary>The view should not be shown as a web view.</summary>
		NoWebView = FOLDERFLAGS.FWF_NOWEBVIEW,

		/// <summary>
		/// Windows Vista and later. Do not re-enumerate the view (or drop the current contents of the view) when the view is refreshed.
		/// </summary>
		NoEnumOnRefresh = FOLDERFLAGS.FWF_NOENUMREFRESH,

		/// <summary>Windows Vista and later. Do not allow grouping in the view</summary>
		NoGrouping = FOLDERFLAGS.FWF_NOGROUPING,

		/// <summary>Windows Vista and later. Do not display filters in the view.</summary>
		NoFilters = FOLDERFLAGS.FWF_NOFILTERS,

		/// <summary>Windows Vista and later. Items can be selected using check-boxes.</summary>
		AutoCheckSelect = FOLDERFLAGS.FWF_AUTOCHECKSELECT,

		/// <summary>Windows Vista and later. The view should list the number of items displayed in each group. To be used with IFolderView2::SetGroupSubsetCount.</summary>
		SubsetGroup = FOLDERFLAGS.FWF_SUBSETGROUPS,

		/// <summary>Windows Vista and later. Use the search folder for stacking and searching.</summary>
		UseSearchFolder = FOLDERFLAGS.FWF_USESEARCHFOLDER,

		/// <summary>
		/// Windows Vista and later. Ensure right-to-left reading layout in a right-to-left system. Without this flag, the view displays
		/// strings from left-to-right both on systems set to left-to-right and right-to-left reading layout, which ensures that file names
		/// display correctly.
		/// </summary>
		AllowRtlReading = FOLDERFLAGS.FWF_ALLOWRTLREADING,
	}

	/// <summary>These flags are used with <see cref="ExplorerBrowserControl.LoadCustomItems"/>.</summary>
	[Flags]
	public enum ExplorerBrowserLoadFlags
	{
		/// <summary>No flags.</summary>
		None = EXPLORER_BROWSER_FILL_FLAGS.EBF_NONE,

		/// <summary>
		/// Causes <see cref="ExplorerBrowserControl.LoadCustomItems"/> to first populate the results folder with the contents of the parent folders
		/// of the items in the data object, and then select only the items that are in the data object.
		/// </summary>
		SelectFromDataObject = EXPLORER_BROWSER_FILL_FLAGS.EBF_SELECTFROMDATAOBJECT,

		/// <summary>
		/// Do not allow dropping on the folder. In other words, do not register a drop target for the view. Applications can then register
		/// their own drop targets.
		/// </summary>
		NoDropTarget = EXPLORER_BROWSER_FILL_FLAGS.EBF_NODROPTARGET,
	}

	/// <summary>
	/// Specifies the options that control subsequent navigation. Typically use one, or a bitwise combination of these flags to specify how
	/// the explorer browser navigates.
	/// </summary>
	[Flags]
	public enum ExplorerBrowserNavigateOptions
	{
		/// <summary>No options.</summary>
		None = EXPLORER_BROWSER_OPTIONS.EBO_NONE,

		/// <summary>Always navigate, even if you are attempting to navigate to the current folder.</summary>
		AlwaysNavigate = EXPLORER_BROWSER_OPTIONS.EBO_ALWAYSNAVIGATE,

		/// <summary>Do not navigate further than the initial navigation.</summary>
		NavigateOnce = EXPLORER_BROWSER_OPTIONS.EBO_NAVIGATEONCE,

		/// <summary>
		/// Use the following standard panes: Commands Module pane, Navigation pane, Details pane, and Preview pane. An implementer of
		/// IExplorerPaneVisibility can modify the components of the Commands Module that are shown. For more information see,
		/// IExplorerPaneVisibility::GetPaneState. If EBO_SHOWFRAMES is not set, Explorer browser uses a single view object.
		/// </summary>
		ShowFrames = EXPLORER_BROWSER_OPTIONS.EBO_SHOWFRAMES,

		/// <summary>Do not update the travel log.</summary>
		NoTravelLog = EXPLORER_BROWSER_OPTIONS.EBO_NOTRAVELLOG,

		/// <summary>Do not use a wrapper window. This flag is used with legacy clients that need the browser parented directly on themselves.</summary>
		NoWrapperWindow = EXPLORER_BROWSER_OPTIONS.EBO_NOWRAPPERWINDOW,

		/// <summary>Show WebView for SharePoint sites.</summary>
		HtmlSharePointView = EXPLORER_BROWSER_OPTIONS.EBO_HTMLSHAREPOINTVIEW,

		/// <summary>Introduced in Windows Vista. Do not draw a border around the browser window.</summary>
		NoBorder = EXPLORER_BROWSER_OPTIONS.EBO_NOBORDER,

		/// <summary>Introduced in Windows Vista. Do not persist the view state.</summary>
		NoPersistViewState = EXPLORER_BROWSER_OPTIONS.EBO_NOPERSISTVIEWSTATE,
	}

	/// <summary>Flags specifying the folder to be browsed.</summary>
	[Flags]
	public enum ExplorerBrowserNavigationItemCategory : uint
	{
		/// <summary>An absolute PIDL, relative to the desktop.</summary>
		Absolute = SBSP.SBSP_ABSOLUTE,

		/// <summary>Windows Vista and later. Navigate without the default behavior of setting focus into the new view.</summary>
		ActivateNoFocus = SBSP.SBSP_ACTIVATE_NOFOCUS,

		/// <summary>Enable auto-navigation.</summary>
		AllowAutoNavigate = SBSP.SBSP_ALLOW_AUTONAVIGATE,

		/// <summary>
		/// Microsoft Internet Explorer 6 Service Pack 2 (SP2) and later. The navigation was possibly initiated by a web page with scripting
		/// code already present on the local system.
		/// </summary>
		CallerUntrusted = SBSP.SBSP_CALLERUNTRUSTED,

		/// <summary>
		/// Windows 7 and later. Do not add a new entry to the travel log. When the user enters a search term in the search box and
		/// subsequently refines the query, the browser navigates forward but does not add an additional travel log entry.
		/// </summary>
		CreateNoHistory = SBSP.SBSP_CREATENOHISTORY,

		/// <summary>
		/// Use default behavior, which respects the view option (the user setting to create new windows or to browse in place). In most
		/// cases, calling applications should use this flag.
		/// </summary>
		Default = SBSP.SBSP_DEFBROWSER,

		/// <summary>Use the current window.</summary>
		UseCurrentWindow = SBSP.SBSP_DEFMODE,

		/// <summary>
		/// Specifies a folder tree for the new browse window. If the current browser does not match the SBSP.SBSP_EXPLOREMODE of the browse
		/// object call, a new window is opened.
		/// </summary>
		ExploreMode = SBSP.SBSP_EXPLOREMODE,

		/// <summary>
		/// Windows Internet Explorer 7 and later. If allowed by current registry settings, give the browser a destination to navigate to.
		/// </summary>
		FeedNavigation = SBSP.SBSP_FEEDNAVIGATION,

		/// <summary>Windows Vista and later. Navigate without clearing the search entry field.</summary>
		KeepSearchText = SBSP.SBSP_KEEPWORDWHEELTEXT,

		/// <summary>Navigate back, ignore the PIDL.</summary>
		NavigateBack = SBSP.SBSP_NAVIGATEBACK,

		/// <summary>Navigate forward, ignore the PIDL.</summary>
		NavigateForward = SBSP.SBSP_NAVIGATEFORWARD,

		/// <summary>Creates another window for the specified folder.</summary>
		NewWindow = SBSP.SBSP_NEWBROWSER,

		/// <summary>Suppress selection in the history pane.</summary>
		NoHistorySelect = SBSP.SBSP_NOAUTOSELECT,

		/// <summary>Do not transfer the browsing history to the new window.</summary>
		NoTransferHistory = SBSP.SBSP_NOTRANSFERHIST,

		/// <summary>
		/// Specifies no folder tree for the new browse window. If the current browser does not match the SBSP.SBSP_OPENMODE of the browse
		/// object call, a new window is opened.
		/// </summary>
		NoFolderTree = SBSP.SBSP_OPENMODE,

		/// <summary>Browse the parent folder, ignore the PIDL.</summary>
		ParentFolder = SBSP.SBSP_PARENT,

		/// <summary>Windows 7 and later. Do not make the navigation complete sound for each keystroke in the search box.</summary>
		PlayNoSound = SBSP.SBSP_PLAYNOSOUND,

		/// <summary>Enables redirection to another URL.</summary>
		Redirect = SBSP.SBSP_REDIRECT,

		/// <summary>A relative PIDL, relative to the current folder.</summary>
		Relative = SBSP.SBSP_RELATIVE,

		/// <summary>Browse to another folder with the same Windows Explorer window.</summary>
		SameWindow = SBSP.SBSP_SAMEBROWSER,

		/// <summary>Microsoft Internet Explorer 6 Service Pack 2 (SP2) and later. The navigate should allow ActiveX prompts.</summary>
		TrustedForActiveX = SBSP.SBSP_TRUSTEDFORACTIVEX,

		/// <summary>
		/// Microsoft Internet Explorer 6 Service Pack 2 (SP2) and later. The new window is the result of a user initiated action. Trust the
		/// new window if it immediately attempts to download content.
		/// </summary>
		TrustFirstDownload = SBSP.SBSP_TRUSTFIRSTDOWNLOAD,

		/// <summary>
		/// Microsoft Internet Explorer 6 Service Pack 2 (SP2) and later. The window is navigating to an untrusted, non-HTML file. If the
		/// user attempts to download the file, do not allow the download.
		/// </summary>
		UntrustedForDownload = SBSP.SBSP_UNTRUSTEDFORDOWNLOAD,

		/// <summary>Write no history of this navigation in the history Shell folder.</summary>
		WriteNoHistory = SBSP.SBSP_WRITENOHISTORY
	}

	/// <summary>Indicates the viewing mode of the explorer browser</summary>
	public enum ExplorerBrowserViewMode
	{
		/// <summary>Choose the best view mode for the folder</summary>
		Auto = FOLDERVIEWMODE.FVM_AUTO,

		/// <summary>(New for Windows7)</summary>
		Content = FOLDERVIEWMODE.FVM_CONTENT,

		/// <summary>Object names and other selected information, such as the size or date last updated, are shown.</summary>
		Details = FOLDERVIEWMODE.FVM_DETAILS,

		/// <summary>The view should display medium-size icons.</summary>
		Icon = FOLDERVIEWMODE.FVM_ICON,

		/// <summary>Object names are displayed in a list view.</summary>
		List = FOLDERVIEWMODE.FVM_LIST,

		/// <summary>The view should display small icons.</summary>
		SmallIcon = FOLDERVIEWMODE.FVM_SMALLICON,

		/// <summary>The view should display thumbnail icons.</summary>
		Thumbnail = FOLDERVIEWMODE.FVM_THUMBNAIL,

		/// <summary>The view should display icons in a filmstrip format.</summary>
		ThumbStrip = FOLDERVIEWMODE.FVM_THUMBSTRIP,

		/// <summary>The view should display large icons.</summary>
		Tile = FOLDERVIEWMODE.FVM_TILE
	}

	/// <summary>Indicates the visibility state of an ExplorerBrowser pane.</summary>
	public enum PaneVisibilityState
	{
		/// <summary>Allow the explorer browser to determine if this pane is displayed.</summary>
		Default = EXPLORERPANESTATE.EPS_DONTCARE,

		/// <summary>Hide the pane</summary>
		Hide = EXPLORERPANESTATE.EPS_DEFAULT_OFF | EXPLORERPANESTATE.EPS_FORCE,

		/// <summary>Show the pane</summary>
		Show = EXPLORERPANESTATE.EPS_DEFAULT_ON | EXPLORERPANESTATE.EPS_FORCE
	}

	/// <summary>The direction argument for Navigate</summary>
	internal enum NavigationLogDirection
	{
		/// <summary>Navigates forward through the navigation log</summary>
		Forward,

		/// <summary>Navigates backward through the travel log</summary>
		Backward
	}

	/// <summary>
	/// <c>ExplorerBrowser</c> is a browser object that can be either navigated or that can host a view of a data object. As a
	/// full-featured browser object, it also supports an automatic travel log.
	/// </summary>
	/// <seealso cref="Control" />
	/// <seealso cref="IServiceProvider" />
	/// <seealso cref="IExplorerPaneVisibility" />
	/// <seealso cref="IExplorerBrowserEvents" />
	/// <seealso cref="ICommDlgBrowser3" />
	/// <seealso cref="IMessageFilter" />
	// FIX by taj 16/01/19 - [Designer(typeof(Design.ExplorerBrowserDesigner)), DefaultProperty(nameof(Name)), DefaultEvent(nameof(SelectionChanged))]
	// FIX by taj 16/01/19 - [ToolboxItem(true), ToolboxBitmap(typeof(ExplorerBrowserControl), "ExplorerBrowser.bmp")]
	[Description("A Shell browser object that can be either navigated or that can host a view of a data object.")]
	public class ExplorerBrowserControl : UserControl, IServiceProvider, IExplorerPaneVisibility, IExplorerBrowserEvents, ICommDlgBrowser3, IMessageFilter
	{
		internal uint eventsCookie;
		internal IExplorerBrowser explorerBrowserControl;
		internal FOLDERSETTINGS folderSettings = new FOLDERSETTINGS(FOLDERVIEWMODE.FVM_AUTO, defaultFolderFlags);

		private const FOLDERFLAGS defaultFolderFlags = FOLDERFLAGS.FWF_USESEARCHFOLDER | FOLDERFLAGS.FWF_NOWEBVIEW;
		private const int defaultThumbnailSize = 32;
		private const int HRESULT_CANCELLED = unchecked((int)0x800704C7);
		private const int HRESULT_RESOURCE_IN_USE = unchecked((int)0x800700AA);

		private static readonly string defaultPropBagName = typeof(ExplorerBrowserControl).FullName;
		private static readonly Guid IID_ICommDlgBrowser = new Guid("000214F1-0000-0000-C000-000000000046");

		private Tuple<ShellItem, ExplorerBrowserNavigationItemCategory> antecreationNavigationTarget;
		private EXPLORER_BROWSER_OPTIONS options = EXPLORER_BROWSER_OPTIONS.EBO_SHOWFRAMES;
		private string propertyBagName = defaultPropBagName;
		private int thumbnailSize = defaultThumbnailSize;
		private ExplorerBrowserViewEvents viewEvents;

		/// <summary>Initializes a new instance of the <see cref="ExplorerBrowserControl"/> class.</summary>
		public ExplorerBrowserControl()
		{
			History = new ExplorerBrowserNavigationLog(this);
			Items = new ShellItemCollection(this, SVGIO.SVGIO_ALLVIEW);
			SelectedItems = new ShellItemCollection(this, SVGIO.SVGIO_SELECTION);
		}

		/// <summary>Fires when the Items collection changes.</summary>
		[Category("Action"), Description("Items changed.")]
		public event EventHandler ItemsChanged;

		/// <summary>Fires when the ExplorerBorwser view has finished enumerating files.</summary>
		[Category("Behavior"), Description("View is done enumerating files.")]
		public event EventHandler ItemsEnumerated;

		/// <summary>
		/// Fires when a navigation has been 'completed': no Navigating listener has canceled, and the ExplorerBorwser has created a new
		/// view. The view will be populated with new items asynchronously, and ItemsChanged will be fired to reflect this some time later.
		/// </summary>
		[Category("Action"), Description("Navigation complete.")]
		public event EventHandler<NavigatedEventArgs> Navigated;

		/// <summary>Fires when a navigation has been initiated, but is not yet complete.</summary>
		[Category("Action"), Description("Navigation initiated, but not complete.")]
		public event EventHandler<NavigatingEventArgs> Navigating;

		/// <summary>
		/// Fires when either a Navigating listener cancels the navigation, or if the operating system determines that navigation is not possible.
		/// </summary>
		[Category("Action"), Description("Navigation failed.")]
		public event EventHandler<NavigationFailedEventArgs> NavigationFailed;

		/// <summary>Fires when the item selected in the view has changed (i.e., a rename ). This is not the same as SelectionChanged.</summary>
		[Category("Action"), Description("Selected item has changed.")]
		public event EventHandler SelectedItemModified;

		/// <summary>Fires when the SelectedItems collection changes.</summary>
		[Category("Behavior"), Description("Selection changed.")]
		public event EventHandler SelectionChanged;

		/// <summary>The view should be left-aligned.</summary>
		[Browsable(false), DefaultValue(false), Category("Appearance"), Description("The view should be left-aligned.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool AlignLeft
		{
			get => IsContentFlagSet(ExplorerBrowserContentSectionOptions.AlignLeft);
			set => SetContentFlag(ExplorerBrowserContentSectionOptions.AlignLeft, value);
		}

		/// <summary>
		/// Ensure right-to-left reading layout in a right-to-left system. Without this flag, the view displays strings from left-to-right
		/// both on systems set to left-to-right and right-to-left reading layout, which ensures that file names display correctly.
		/// </summary>
		[Browsable(false), DefaultValue(false), Category("Appearance"), Description("Ensure right-to-left reading layout in a right-to-left system.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool AllowRtlReading
		{
			get => IsContentFlagSet(ExplorerBrowserContentSectionOptions.AllowRtlReading);
			set => SetContentFlag(ExplorerBrowserContentSectionOptions.AllowRtlReading, value);
		}

		/// <summary>Always navigate, even if you are attempting to navigate to the current folder.</summary>
		[DefaultValue(false), Category("Behavior"), Description("Always navigate, even if you are attempting to navigate to the current folder.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool AlwaysNavigate
		{
			get => IsNavFlagSet(ExplorerBrowserNavigateOptions.AlwaysNavigate);
			set => SetNavFlag(ExplorerBrowserNavigateOptions.AlwaysNavigate, value);
		}

		/// <summary>Automatically arrange the elements in the view.</summary>
		[DefaultValue(false), Category("Behavior"), Description("Automatically arrange the elements in the view.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool AutoArrange
		{
			get => IsContentFlagSet(ExplorerBrowserContentSectionOptions.AutoArrange);
			set => SetContentFlag(ExplorerBrowserContentSectionOptions.AutoArrange, value);
		}

		/// <summary>Items can be selected using check-boxes.</summary>
		[DefaultValue(false), Category("Behavior"), Description("Items can be selected using check-boxes.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool AutoCheckSelect
		{
			get => IsContentFlagSet(ExplorerBrowserContentSectionOptions.AutoCheckSelect);
			set => SetContentFlag(ExplorerBrowserContentSectionOptions.AutoCheckSelect, value);
		}

		/// <summary>Turns on check mode for the view</summary>
		[DefaultValue(false), Category("Behavior"), Description("Turns on check mode for the view")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool CheckSelect
		{
			get => IsContentFlagSet(ExplorerBrowserContentSectionOptions.CheckSelect);
			set => SetContentFlag(ExplorerBrowserContentSectionOptions.CheckSelect, value);
		}

		/// <summary>The binary representation of the ExplorerBrowser content flags</summary>
		[Browsable(false), DefaultValue(0), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public ExplorerBrowserContentSectionOptions ContentFlags
		{
			get => (ExplorerBrowserContentSectionOptions)folderSettings.fFlags;
			set
			{
				folderSettings.fFlags = (FOLDERFLAGS)value | defaultFolderFlags;
				explorerBrowserControl?.SetFolderSettings(folderSettings);
			}
		}

		/// <summary>Make the folder behave like the desktop. This applies only to the desktop and is not used for typical Shell folders.</summary>
		[Browsable(false), DefaultValue(false), Category("Appearance"), Description("Make the folder behave like the desktop.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool Desktop
		{
			get => IsContentFlagSet(ExplorerBrowserContentSectionOptions.Desktop);
			set => SetContentFlag(ExplorerBrowserContentSectionOptions.Desktop, value);
		}

		/// <summary>When the view is in "tile view mode" the layout of a single item should be extended to the width of the view.</summary>
		[DefaultValue(false), Category("Appearance"), Description("The layout of a single item should be extended to the width of the view.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool ExtendedTiles
		{
			get => IsContentFlagSet(ExplorerBrowserContentSectionOptions.ExtendedTiles);
			set => SetContentFlag(ExplorerBrowserContentSectionOptions.ExtendedTiles, value);
		}

		/// <summary>When an item is selected, the item and all its sub-items are highlighted.</summary>
		[DefaultValue(false), Category("Behavior"), Description("When an item is selected, the item and all its sub-items are highlighted.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool FullRowSelect
		{
			get => IsContentFlagSet(ExplorerBrowserContentSectionOptions.FullRowSelect);
			set => SetContentFlag(ExplorerBrowserContentSectionOptions.FullRowSelect, value);
		}

		/// <summary>The view should not display file names</summary>
		[DefaultValue(false), Category("Appearance"), Description("The view should not display file names")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool HideFileNames
		{
			get => IsContentFlagSet(ExplorerBrowserContentSectionOptions.HideFileNames);
			set => SetContentFlag(ExplorerBrowserContentSectionOptions.HideFileNames, value);
		}

		/// <summary>Contains the navigation history of the ExplorerBrowser</summary>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ExplorerBrowserNavigationLog History { get; }

		/// <summary>The set of ShellItems in the Explorer Browser</summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public IReadOnlyList<ShellItem> Items { get; }

		/// <summary>Do not navigate further than the initial navigation.</summary>
		[DefaultValue(false), Category("Behavior"), Description("Do not navigate further than the initial navigation.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool NavigateOnce
		{
			get => IsNavFlagSet(ExplorerBrowserNavigateOptions.NavigateOnce);
			set => SetNavFlag(ExplorerBrowserNavigateOptions.NavigateOnce, value);
		}

		/// <summary>The binary flags that are passed to the explorer browser control's GetOptions/SetOptions methods</summary>
		[Browsable(false), DefaultValue(0), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public ExplorerBrowserNavigateOptions NavigationFlags
		{
			get => (ExplorerBrowserNavigateOptions)(explorerBrowserControl?.GetOptions() ?? options);
			set
			{
				// Always forcing SHOWFRAMES because we handle IExplorerPaneVisibility
				options = (EXPLORER_BROWSER_OPTIONS)value | EXPLORER_BROWSER_OPTIONS.EBO_SHOWFRAMES;
				explorerBrowserControl?.SetOptions(options);
			}
		}

		/// <summary>The view should not save view state in the browser.</summary>
		[DefaultValue(false), Category("Behavior"), Description("The view should not save view state in the browser.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool NoBrowserViewState
		{
			get => IsContentFlagSet(ExplorerBrowserContentSectionOptions.NoBrowserViewState);
			set => SetContentFlag(ExplorerBrowserContentSectionOptions.NoBrowserViewState, value);
		}

		/// <summary>Do not display a column header in the view in any view mode.</summary>
		[DefaultValue(false), Category("Appearance"), Description("Do not display a column header in the view in any view mode.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool NoColumnHeader
		{
			get => IsContentFlagSet(ExplorerBrowserContentSectionOptions.NoColumnHeader);
			set => SetContentFlag(ExplorerBrowserContentSectionOptions.NoColumnHeader, value);
		}

		/// <summary>Do not re-enumerate the view (or drop the current contents of the view) when the view is refreshed.</summary>
		[DefaultValue(false), Category("Behavior"), Description("Do not re-enumerate the view (or drop the current contents of the view) when the view is refreshed.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool NoEnumOnRefresh
		{
			get => IsContentFlagSet(ExplorerBrowserContentSectionOptions.NoEnumOnRefresh);
			set => SetContentFlag(ExplorerBrowserContentSectionOptions.NoEnumOnRefresh, value);
		}

		/// <summary>Do not display filters in the view.</summary>
		[DefaultValue(false), Category("Appearance"), Description("Do not display filters in the view.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool NoFilters
		{
			get => IsContentFlagSet(ExplorerBrowserContentSectionOptions.NoFilters);
			set => SetContentFlag(ExplorerBrowserContentSectionOptions.NoFilters, value);
		}

		/// <summary>Do not allow grouping in the view.</summary>
		[DefaultValue(false), Category("Appearance"), Description("Do not allow grouping in the view.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool NoGrouping
		{
			get => IsContentFlagSet(ExplorerBrowserContentSectionOptions.NoGrouping);
			set => SetContentFlag(ExplorerBrowserContentSectionOptions.NoGrouping, value);
		}

		/// <summary>Only show the column header in details view mode.</summary>
		[DefaultValue(false), Category("Appearance"), Description("Only show the column header in details view mode.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool NoHeaderInAllViews
		{
			get => IsContentFlagSet(ExplorerBrowserContentSectionOptions.NoHeaderInAllViews);
			set => SetContentFlag(ExplorerBrowserContentSectionOptions.NoHeaderInAllViews, value);
		}

		/// <summary>The view should not display icons.</summary>
		[DefaultValue(false), Category("Appearance"), Description("The view should not display icons.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool NoIcons
		{
			get => IsContentFlagSet(ExplorerBrowserContentSectionOptions.NoIcons);
			set => SetContentFlag(ExplorerBrowserContentSectionOptions.NoIcons, value);
		}

		/// <summary>Introduced in Windows Vista. Do not persist the view state.</summary>
		[DefaultValue(false), Category("Behavior"), Description("Do not persist the view state.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool NoPersistViewState
		{
			get => IsNavFlagSet(ExplorerBrowserNavigateOptions.NoPersistViewState);
			set => SetNavFlag(ExplorerBrowserNavigateOptions.NoPersistViewState, value);
		}

		/// <summary>Do not add scroll bars. This is used only for the desktop.</summary>
		[Browsable(false), DefaultValue(false), Category("Appearance"), Description("Do not add scroll bars.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool NoScrollBars
		{
			get => IsContentFlagSet(ExplorerBrowserContentSectionOptions.NoScrollBars);
			set => SetContentFlag(ExplorerBrowserContentSectionOptions.NoScrollBars, value);
		}

		/// <summary>Do not show subfolders.</summary>
		[DefaultValue(false), Category("Appearance"), Description("Do not show subfolders.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool NoSubfolders
		{
			get => IsContentFlagSet(ExplorerBrowserContentSectionOptions.NoSubfolders);
			set => SetContentFlag(ExplorerBrowserContentSectionOptions.NoSubfolders, value);
		}

		/// <summary>Do not update the travel log.</summary>
		[DefaultValue(false), Category("Behavior"), Description("Do not update the travel log.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool NoTravelLog
		{
			get => IsNavFlagSet(ExplorerBrowserNavigateOptions.NoTravelLog);
			set => SetNavFlag(ExplorerBrowserNavigateOptions.NoTravelLog, value);
		}

		/// <summary>Do not use a wrapper window. This flag is used with legacy clients that need the browser parented directly on themselves.</summary>
		[Browsable(false), DefaultValue(false), Category("Behavior"), Description("Do not use a wrapper window.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool NoWrapperWindow
		{
			get => IsNavFlagSet(ExplorerBrowserNavigateOptions.NoWrapperWindow);
			set => SetNavFlag(ExplorerBrowserNavigateOptions.NoWrapperWindow, value);
		}

		/// <summary>Controls the visibility of the various ExplorerBrowser panes on subsequent navigation</summary>
		[Category("Appearance"), Description("Set visibility of child panes.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ExplorerBrowserPaneVisibility PaneVisibility { get; } = new ExplorerBrowserPaneVisibility();

		/// <summary>The name of the property bag used to persist changes to the ExplorerBrowser's view state.</summary>
		[Browsable(false), Category("Data"), Description("Name of the property bag used to persist changes to the ExplorerBrowser's view state")]
		public string PropertyBagName
		{
			get => propertyBagName;
			set
			{
				propertyBagName = value;
				explorerBrowserControl?.SetPropertyBag(propertyBagName);
			}
		}

		/// <summary>The set of selected ShellItems in the Explorer Browser</summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public IReadOnlyList<ShellItem> SelectedItems { get; }

		/// <summary>Navigate with a single click.</summary>
		[DefaultValue(false), Category("Behavior"), Description("Navigate with a single click.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool SingleClickActivate
		{
			get => IsContentFlagSet(ExplorerBrowserContentSectionOptions.SingleClickActivate);
			set => SetContentFlag(ExplorerBrowserContentSectionOptions.SingleClickActivate, value);
		}

		/// <summary>Do not allow more than a single item to be selected.</summary>
		[DefaultValue(false), Category("Behavior"), Description("Do not allow more than a single item to be selected.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool SingleSelection
		{
			get => IsContentFlagSet(ExplorerBrowserContentSectionOptions.SingleSelection);
			set => SetContentFlag(ExplorerBrowserContentSectionOptions.SingleSelection, value);
		}

		/// <summary>The view should list the number of items displayed in each group.</summary>
		[DefaultValue(false), Category("Appearance"), Description("The view should list the number of items displayed in each group.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool SubsetGroup
		{
			get => IsContentFlagSet(ExplorerBrowserContentSectionOptions.SubsetGroup);
			set => SetContentFlag(ExplorerBrowserContentSectionOptions.SubsetGroup, value);
		}

		/// <summary>The size of the thumbnails in pixels.</summary>
		[Category("Appearance"), DefaultValue(defaultThumbnailSize), Description("The size of the thumbnails in pixels.")]
		public int ThumbnailSize
		{
			get
			{
				var iFV2 = GetFolderView2();
				if (iFV2 is null) return thumbnailSize;
				try
				{
					iFV2.GetViewModeAndIconSize(out var fvm, out var iconSize);
					return thumbnailSize = iconSize;
				}
				finally
				{
					iFV2 = null;
				}
			}
			set
			{
				var iFV2 = GetFolderView2();
				if (iFV2 is null) return;
				try
				{
					iFV2.GetViewModeAndIconSize(out var fvm, out var iconSize);
					iFV2.SetViewModeAndIconSize(fvm, thumbnailSize = value);
				}
				finally
				{
					iFV2 = null;
				}
			}
		}

		/// <summary>Draw transparently. This is used only for the desktop.</summary>
		[Browsable(false), DefaultValue(false), Category("Appearance"), Description("Draw transparently.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool Transparent
		{
			get => IsContentFlagSet(ExplorerBrowserContentSectionOptions.Transparent);
			set => SetContentFlag(ExplorerBrowserContentSectionOptions.Transparent, value);
		}

		/// <summary>Show WebView for SharePoint sites.</summary>
		[Browsable(false), DefaultValue(false), Category("Behavior"), Description("Show WebView for SharePoint sites.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool UseHtmlSharePointView
		{
			get => IsNavFlagSet(ExplorerBrowserNavigateOptions.HtmlSharePointView);
			set => SetNavFlag(ExplorerBrowserNavigateOptions.HtmlSharePointView, value);
		}

		/// <summary>The viewing mode of the Explorer Browser</summary>
		[DefaultValue(typeof(ExplorerBrowserViewMode), "Auto"), Category("Appearance"), Description("The viewing mode of the Explorer Browser.")]
		public ExplorerBrowserViewMode ViewMode
		{
			get => (ExplorerBrowserViewMode)folderSettings.ViewMode;
			set
			{
				folderSettings.ViewMode = (FOLDERVIEWMODE)value;
				explorerBrowserControl?.SetFolderSettings(folderSettings);
			}
		}

		/// <inheritdoc/>
		protected override Size DefaultSize => new Size(200, 150);

		/// <summary>Removes all items from the results folder.</summary>
		public void ClearCustomItems() => explorerBrowserControl?.RemoveAll();

		/// <summary>Navigates to the last item in the navigation history list. This does not change the set of locations in the navigation log.</summary>
		/// <returns>True if the navigation succeeded, false if it failed for any reason.</returns>
		public bool GoBack() => History.NavigateLog(NavigationLogDirection.Backward);

		/// <summary>Navigates to the next item in the navigation history list. This does not change the set of locations in the navigation log.</summary>
		/// <returns>True if the navigation succeeded, false if it failed for any reason.</returns>
		public bool GoForward() => History.NavigateLog(NavigationLogDirection.Forward);

		/// <summary>Creates a custom folder and fills it with items.</summary>
		/// <param name="obj">
		/// An interface pointer on the source object that will fill the control. This can be an <see cref="IDataObject"/> or any object that
		/// can be used with <see cref="INamespaceWalk"/>.
		/// </param>
		/// <param name="flags">One of the <see cref="ExplorerBrowserLoadFlags"/> values.</param>
		public void LoadCustomItems(object obj, ExplorerBrowserLoadFlags flags = ExplorerBrowserLoadFlags.None) => explorerBrowserControl?.FillFromObject(obj, (EXPLORER_BROWSER_FILL_FLAGS)flags);

		/// <summary>
		/// Clears the Explorer Browser of existing content, fills it with content from the specified container, and adds a new point to the
		/// Travel Log.
		/// </summary>
		/// <param name="shellItem">The shell container to navigate to.</param>
		/// <param name="category">The category of the <paramref name="shellItem"/>.</param>
		public void Navigate(ShellItem shellItem, ExplorerBrowserNavigationItemCategory category = ExplorerBrowserNavigationItemCategory.Absolute)
		{
			if (shellItem == null)
				throw new ArgumentNullException(nameof(shellItem));

			if (explorerBrowserControl == null)
			{
				antecreationNavigationTarget = new Tuple<ShellItem, ExplorerBrowserNavigationItemCategory>(shellItem, category);
			}
			else
			{
				try
				{
					explorerBrowserControl.BrowseToObject(shellItem.IShellItem, (SBSP)category);
				}
				catch (COMException e)
				{
					if (e.ErrorCode == HRESULT_RESOURCE_IN_USE || e.ErrorCode == HRESULT_CANCELLED)
					{
						OnNavigationFailed(new NavigationFailedEventArgs { FailedLocation = shellItem });
					}
					else
					{
						throw new ArgumentException("Unable to browse to this shell item.", nameof(shellItem), e);
					}
				}
				catch (Exception e)
				{
					throw new ArgumentException("Unable to browse to this shell item.", nameof(shellItem), e);
				}
			}
		}
		/// <summary>Navigate within the navigation log. This does not change the set of locations in the navigation log.</summary>
		/// <param name="historyIndex">An index into the navigation logs Locations collection.</param>
		/// <returns>True if the navigation succeeded, false if it failed for any reason.</returns>
		public bool NavigateToHistoryIndex(int historyIndex) => History.NavigateLog(historyIndex);

		HRESULT ICommDlgBrowser3.GetCurrentFilter(StringBuilder pszFileSpec, int cchFileSpec) => HRESULT.S_OK;

		HRESULT ICommDlgBrowser3.GetDefaultMenuText(IShellView ppshv, StringBuilder pszText, int cchMax) => HRESULT.S_FALSE;

		HRESULT IExplorerPaneVisibility.GetPaneState(in Guid ep, out EXPLORERPANESTATE peps)
		{
			switch (ep)
			{
				case var a when a.Equals(IExplorerPaneVisibilityConstants.EP_AdvQueryPane):
					peps = (EXPLORERPANESTATE)PaneVisibility.AdvancedQuery;
					break;

				case var a when a.Equals(IExplorerPaneVisibilityConstants.EP_Commands):
					peps = (EXPLORERPANESTATE)PaneVisibility.Commands;
					break;

				case var a when a.Equals(IExplorerPaneVisibilityConstants.EP_Commands_Organize):
					peps = (EXPLORERPANESTATE)PaneVisibility.CommandsOrganize;
					break;

				case var a when a.Equals(IExplorerPaneVisibilityConstants.EP_Commands_View):
					peps = (EXPLORERPANESTATE)PaneVisibility.CommandsView;
					break;

				case var a when a.Equals(IExplorerPaneVisibilityConstants.EP_DetailsPane):
					peps = (EXPLORERPANESTATE)PaneVisibility.Details;
					break;

				case var a when a.Equals(IExplorerPaneVisibilityConstants.EP_NavPane):
					peps = (EXPLORERPANESTATE)PaneVisibility.Navigation;
					break;

				case var a when a.Equals(IExplorerPaneVisibilityConstants.EP_PreviewPane):
					peps = (EXPLORERPANESTATE)PaneVisibility.Preview;
					break;

				case var a when a.Equals(IExplorerPaneVisibilityConstants.EP_QueryPane):
					peps = (EXPLORERPANESTATE)PaneVisibility.Query;
					break;

				case var a when a.Equals(IExplorerPaneVisibilityConstants.EP_Ribbon):
					peps = (EXPLORERPANESTATE)PaneVisibility.Ribbon;
					break;

				case var a when a.Equals(IExplorerPaneVisibilityConstants.EP_StatusBar):
					peps = (EXPLORERPANESTATE)PaneVisibility.StatusBar;
					break;

				default:
					peps = EXPLORERPANESTATE.EPS_DONTCARE;
					break;
			}
			return HRESULT.S_OK;
		}

		HRESULT ICommDlgBrowser3.GetViewFlags(out CDB2GVF pdwFlags)
		{
			pdwFlags = CDB2GVF.CDB2GVF_SHOWALLFILES;
			return HRESULT.S_OK;
		}

		HRESULT ICommDlgBrowser3.IncludeObject(IShellView ppshv, IntPtr pidl)
		{
			OnItemsChanged();
			return HRESULT.S_OK;
		}

		//HRESULT ICommDlgBrowser.IncludeObject(IShellView ppshv, PIDL pidl) => ((ICommDlgBrowser3)this).IncludeObject(ppshv, pidl);

		HRESULT ICommDlgBrowser3.Notify(IShellView ppshv, CDB2N dwNotifyType) => HRESULT.S_OK;

		HRESULT ICommDlgBrowser3.OnColumnClicked(IShellView ppshv, int iColumn) => HRESULT.S_OK;

		HRESULT ICommDlgBrowser3.OnDefaultCommand(IShellView ppshv) => HRESULT.S_FALSE;

		//HRESULT ICommDlgBrowser.OnDefaultCommand(IShellView ppshv) => ((ICommDlgBrowser3)this).OnDefaultCommand(ppshv);

		HRESULT IExplorerBrowserEvents.OnNavigationComplete(IntPtr pidlFolder)
		{
			folderSettings.ViewMode = GetCurrentViewMode();
			OnNavigated(new NavigatedEventArgs { NewLocation = new ShellItem(pidlFolder) });
			return HRESULT.S_OK;
		}

		HRESULT IExplorerBrowserEvents.OnNavigationFailed(IntPtr pidlFolder)
		{
			OnNavigationFailed(new NavigationFailedEventArgs { FailedLocation = new ShellItem(pidlFolder) });
			return HRESULT.S_OK;
		}

		HRESULT IExplorerBrowserEvents.OnNavigationPending(IntPtr pidlFolder)
		{
			OnNavigating(new NavigatingEventArgs { PendingLocation = new ShellItem(pidlFolder) }, out var cancelled);
			return cancelled ? (HRESULT)HRESULT_CANCELLED : HRESULT.S_OK;
		}

		HRESULT ICommDlgBrowser3.OnPreViewCreated(IShellView ppshv) => HRESULT.S_OK;

		HRESULT ICommDlgBrowser3.OnStateChange(IShellView ppshv, CDBOSC uChange)
		{
			if (uChange == CDBOSC.CDBOSC_SELCHANGE)
				OnSelectionChanged();
			return HRESULT.S_OK;
		}

		//HRESULT ICommDlgBrowser.OnStateChange(IShellView ppshv, CDBOSC uChange) => ((ICommDlgBrowser3)this).OnStateChange(ppshv, uChange);

		HRESULT IExplorerBrowserEvents.OnViewCreated(IShellView psv)
		{
			viewEvents.ConnectToView((IShellView)psv);
			return HRESULT.S_OK;
		}

        bool IMessageFilter.PreFilterMessage(ref Message m)
        {
            HRESULT hr = HRESULT.S_FALSE;

            if (this.explorerBrowserControl is IInputObject io)
            {
                hr = io.TranslateAcceleratorIO(
                    new MSG { message = (uint)m.Msg, hwnd = m.HWnd, wParam = m.WParam, lParam = m.LParam });
            }

            return (hr == HRESULT.S_OK);
        }

        HRESULT IServiceProvider.QueryService(ref Guid guidService, ref Guid riid, out IntPtr ppvObject)
		{
			HRESULT hr = HRESULT.E_NOINTERFACE;
			ppvObject = default;
			if (guidService.Equals(typeof(IExplorerPaneVisibility).GUID))
			{
				//hr = InteropExtensions.QueryInterface(this, guidService, out ppvObject);
				ppvObject = Marshal.GetComInterfaceForObject(this, typeof(IExplorerPaneVisibility));
				hr = HRESULT.S_OK;
			}
			else if (guidService.Equals(IID_ICommDlgBrowser) && (riid.Equals(IID_ICommDlgBrowser) || riid.Equals(typeof(ICommDlgBrowser3).GUID)))
			{
				//hr = InteropExtensions.QueryInterface(this, riid, out ppvObject);
				ppvObject = Marshal.GetComInterfaceForObject(this, typeof(ICommDlgBrowser3));
				hr = HRESULT.S_OK;
			}
			return hr;
		}

		/// <summary>Gets the IFolderView2 interface from the explorer browser.</summary>
		/// <returns>An <see cref="IFolderView2"/> instance.</returns>
		internal IFolderView2 GetFolderView2() => explorerBrowserControl?.GetCurrentView<IFolderView2>();

		/// <summary>Gets the items in the ExplorerBrowser as an IShellItemArray</summary>
		/// <returns></returns>
		internal IShellItemArray GetItemsArray(SVGIO opt)
		{
			var iFV2 = GetFolderView2();
			if (iFV2 is null) return null;
			try
			{
				return iFV2.Items<IShellItemArray>(opt);
			}
			finally
			{
				iFV2 = null;
			}
		}

		/// <summary>Raises the <see cref="ItemsChanged"/> event.</summary>
		protected internal virtual void OnItemsChanged() => ItemsChanged?.Invoke(this, EventArgs.Empty);

		/// <summary>Raises the <see cref="ItemsEnumerated"/> event.</summary>
		protected internal virtual void OnItemsEnumerated() => ItemsEnumerated?.Invoke(this, EventArgs.Empty);

		/// <summary>Raises the <see cref="Navigated"/> event.</summary>
		protected internal virtual void OnNavigated(NavigatedEventArgs ncevent)
		{
			if (ncevent?.NewLocation == null) return;
			Navigated?.Invoke(this, ncevent);
		}

		/// <summary>Raises the <see cref="Navigating"/> event.</summary>
		protected internal virtual void OnNavigating(NavigatingEventArgs npevent, out bool cancelled)
		{
			cancelled = false;
			if (Navigating == null || npevent?.PendingLocation == null) return;
			foreach (var del in Navigating.GetInvocationList())
			{
				del.DynamicInvoke(new object[] { this, npevent });
				if (npevent.Cancel)
					cancelled = true;
			}
		}

		/// <summary>Raises the <see cref="NavigationFailed"/> event.</summary>
		protected internal virtual void OnNavigationFailed(NavigationFailedEventArgs nfevent)
		{
			if (nfevent?.FailedLocation == null) return;
			NavigationFailed?.Invoke(this, nfevent);
		}

		/// <summary>Raises the <see cref="SelectedItemModified"/> event.</summary>
		protected internal virtual void OnSelectedItemModified() => SelectedItemModified?.Invoke(this, EventArgs.Empty);

		/// <summary>Raises the <see cref="SelectionChanged"/> event.</summary>
		protected internal virtual void OnSelectionChanged() => SelectionChanged?.Invoke(this, EventArgs.Empty);

		/// <summary>Called when [create control].</summary>
		protected override void OnCreateControl()
		{
			base.OnCreateControl();

			if (!DesignMode)
			{
				explorerBrowserControl = new IExplorerBrowser();

				// hooks up IExplorerPaneVisibility and ICommDlgBrowser event notifications
				SetSite(this);

				// hooks up IExplorerBrowserEvents event notification
				explorerBrowserControl.Advise(this, out eventsCookie);

				// sets up ExplorerBrowser view connection point events
				viewEvents = new ExplorerBrowserViewEvents(this);

				explorerBrowserControl.Initialize(Handle, ClientRectangle, folderSettings);

				// Force an initial show frames so that IExplorerPaneVisibility works the first time it is set. This also enables the control
				// panel to be browsed to. If it is not set, then navigating to the control panel succeeds, but no items are visible in the view.
				explorerBrowserControl.SetOptions(options);

				// ExplorerBrowserOptions.NoBorder does not work, so we do it manually...
				RemoveWindowBorder();

				explorerBrowserControl.SetPropertyBag(propertyBagName);

				if (antecreationNavigationTarget != null)
				{
					BeginInvoke(new MethodInvoker(delegate
					{
						Navigate(antecreationNavigationTarget.Item1, antecreationNavigationTarget.Item2);
						antecreationNavigationTarget = null;
					}));
				}
			}

			Application.AddMessageFilter(this);
		}

		/// <summary>Cleans up the explorer browser events+object when the window is being taken down.</summary>
		/// <param name="e">An EventArgs that contains event data.</param>
		protected override void OnHandleDestroyed(EventArgs e)
		{
			if (explorerBrowserControl != null)
			{
				// unhook events
				viewEvents.DisconnectFromView();
				explorerBrowserControl.Unadvise(eventsCookie);
				SetSite(null);

				// destroy the explorer browser control
				explorerBrowserControl.Destroy();

				// release com reference to it
				explorerBrowserControl = null;
			}

			base.OnHandleDestroyed(e);
		}

		/// <summary>Raises the <see cref="E:Paint"/> event.</summary>
		/// <param name="pe">The <see cref="PaintEventArgs"/> instance containing the event data.</param>
		protected override void OnPaint(PaintEventArgs pe)
		{
			if (DesignMode && pe != null)
			{
				var cr = ClientRectangle;
				pe.Graphics.FillRectangle(SystemBrushes.Window, cr);
				if (VisualStyleRenderer.IsSupported)
				{
					var btn = new VisualStyleRenderer(VisualStyleElement.ScrollBar.ArrowButton.UpDisabled);
					var sz = btn.GetPartSize(pe.Graphics, ThemeSizeType.True);
					var rsb = new Rectangle(cr.X + cr.Width - sz.Width, cr.Y, sz.Width, cr.Height);
					new VisualStyleRenderer(VisualStyleElement.ScrollBar.LowerTrackVertical.Disabled).DrawBackground(pe.Graphics, rsb);
					rsb.Height = sz.Height;
					btn.DrawBackground(pe.Graphics, rsb);
					rsb.Offset(0, cr.Height - sz.Height);
					new VisualStyleRenderer(VisualStyleElement.ScrollBar.ArrowButton.DownDisabled).DrawBackground(pe.Graphics, rsb);
				}
				ControlPaint.DrawBorder(pe.Graphics, cr, SystemColors.WindowFrame, ButtonBorderStyle.Solid);

				using (var font = new Font("Segoe UI", 9))
				using (var sf = new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Near })
				{
					pe.Graphics.DrawString(nameof(ExplorerBrowserControl), font, SystemBrushes.GrayText, cr, sf);
				}
			}

			base.OnPaint(pe);
		}

		/// <summary>Sizes the native control to match the WinForms control wrapper.</summary>
		/// <param name="e">Contains information about the size changed event.</param>
		protected override void OnSizeChanged(EventArgs e)
		{
			explorerBrowserControl?.SetRect(default, ClientRectangle);
			base.OnSizeChanged(e);
		}

		private FOLDERVIEWMODE GetCurrentViewMode()
		{
			var ifv2 = GetFolderView2();
			if (ifv2 is null) return 0;
			try
			{
				return ifv2.GetCurrentViewMode();
			}
			finally
			{
				ifv2 = null;
			}
		}
		private bool IsContentFlagSet(ExplorerBrowserContentSectionOptions flag) => folderSettings.fFlags.IsFlagSet((FOLDERFLAGS)flag);

		private bool IsNavFlagSet(ExplorerBrowserNavigateOptions flag) => NavigationFlags.IsFlagSet(flag);

		/// <summary>Find the native control handle, remove its border style, then ask for a redraw.</summary>
		private void RemoveWindowBorder()
		{
			// There is an option (EBO_NOBORDER) to avoid showing a border on the native ExplorerBrowser control so we wouldn't have to
			// remove it afterwards, but:
			// 1. It's not implemented by the Windows API Code Pack
			// 2. The flag doesn't seem to work anyway (tested on 7 and 8.1) For reference: EXPLORER_BROWSER_OPTIONS https://msdn.microsoft.com/en-us/library/windows/desktop/bb762501(v=vs.85).aspx
			var hwnd = FindWindowEx(Handle, default, "ExplorerBrowserControl", default);
			var explorerBrowserStyle = (WindowStyles)GetWindowLongAuto(hwnd, WindowLongFlags.GWL_STYLE).ToInt32();
			SetWindowLong(hwnd, WindowLongFlags.GWL_STYLE, (int)explorerBrowserStyle.ClearFlags(WindowStyles.WS_CAPTION | WindowStyles.WS_BORDER));
			SetWindowPos(hwnd, default, 0, 0, 0, 0, SetWindowPosFlags.SWP_FRAMECHANGED | SetWindowPosFlags.SWP_NOMOVE | SetWindowPosFlags.SWP_NOSIZE);
		}
		private void ResetPropertyBagName() => PropertyBagName = defaultPropBagName;

		private void SetContentFlag(ExplorerBrowserContentSectionOptions flag, bool value)
		{
			folderSettings.fFlags = folderSettings.fFlags.SetFlags((FOLDERFLAGS)flag, value);
			explorerBrowserControl?.SetFolderSettings(folderSettings);
		}

		private void SetNavFlag(ExplorerBrowserNavigateOptions flag, bool value) => NavigationFlags = NavigationFlags.SetFlags(flag, value);

		private void SetSite(IServiceProvider sp) => (explorerBrowserControl as IObjectWithSite)?.SetSite(sp);

		private bool ShouldSerializePropertyBagName() => propertyBagName != defaultPropBagName;

		/// <summary>The navigation log is a history of the locations visited by the explorer browser.</summary>
		public class ExplorerBrowserNavigationLog
		{
			private ExplorerBrowserControl parent = null;

			/// <summary>The pending navigation log action. null if the user is not navigating via the navigation log.</summary>
			private PendingNavigation pendingNavigation;

			internal ExplorerBrowserNavigationLog(ExplorerBrowserControl parent)
			{
				// Hook navigation events from the parent to distinguish between navigation log induced navigation, and other navigations.
				this.parent = parent ?? throw new ArgumentNullException(nameof(parent));
				this.parent.Navigated += OnNavigated;
				this.parent.NavigationFailed += OnNavigationFailed;
			}

			/// <summary>Fires when the navigation log changes or the current navigation position changes</summary>
			public event EventHandler<NavigationLogEventArgs> NavigationLogChanged;

			/// <summary>Indicates the presence of locations in the log that can be reached by calling Navigate(Backward)</summary>
			public bool CanNavigateBackward => CurrentLocationIndex > 0;

			/// <summary>Indicates the presence of locations in the log that can be reached by calling Navigate(Forward)</summary>
			public bool CanNavigateForward => CurrentLocationIndex < Locations.Count - 1;

			/// <summary>Gets the shell object in the Locations collection pointed to by CurrentLocationIndex.</summary>
			public ShellItem CurrentLocation => CurrentLocationIndex < 0 ? null : Locations[CurrentLocationIndex];

			/// <summary>
			/// An index into the Locations collection. The ShellItem pointed to by this index is the current location of the ExplorerBrowser.
			/// </summary>
			public int CurrentLocationIndex { get; set; } = -1;

			/// <summary>The navigation log</summary>
			public List<ShellItem> Locations { get; } = new List<ShellItem>();

			/// <summary>Clears the contents of the navigation log.</summary>
			public void Clear()
			{
				if (Locations.Count == 0) return;

				var oldCanNavigateBackward = CanNavigateBackward;
				var oldCanNavigateForward = CanNavigateForward;

				Locations.Clear();
				CurrentLocationIndex = -1;

				var args = new NavigationLogEventArgs
				{
					LocationsChanged = true,
					CanNavigateBackwardChanged = oldCanNavigateBackward != CanNavigateBackward,
					CanNavigateForwardChanged = oldCanNavigateForward != CanNavigateForward
				};
				NavigationLogChanged?.Invoke(this, args);
			}

			internal bool NavigateLog(NavigationLogDirection direction)
			{
				// determine proper index to navigate to
				var locationIndex = 0;
				if (direction == NavigationLogDirection.Backward && CanNavigateBackward)
				{
					locationIndex = CurrentLocationIndex - 1;
				}
				else if (direction == NavigationLogDirection.Forward && CanNavigateForward)
				{
					locationIndex = CurrentLocationIndex + 1;
				}
				else
				{
					return false;
				}

				// initiate traversal request
				var location = Locations[locationIndex];
				pendingNavigation = new PendingNavigation(location, locationIndex);
				parent.Navigate(location);
				return true;
			}

			internal bool NavigateLog(int index)
			{
				// can't go anywhere
				if (index >= Locations.Count || index < 0) return false;

				// no need to re navigate to the same location
				if (index == CurrentLocationIndex) return false;

				// initiate traversal request
				var location = Locations[index];
				pendingNavigation = new PendingNavigation(location, index);
				parent.Navigate(location);
				return true;
			}

			private void OnNavigated(object sender, NavigatedEventArgs args)
			{
				var eventArgs = new NavigationLogEventArgs();
				var oldCanNavigateBackward = CanNavigateBackward;
				var oldCanNavigateForward = CanNavigateForward;

				if (pendingNavigation != null)
				{
					// navigation log traversal in progress

					// determine if new location is the same as the traversal request
					var shellItemsEqual = pendingNavigation.Location.IShellItem.Compare(args.NewLocation.IShellItem, SICHINTF.SICHINT_ALLFIELDS) == 0;
					if (shellItemsEqual == false)
					{
						// new location is different than traversal request, behave is if it never happened! remove history following
						// currentLocationIndex, append new item
						if (CurrentLocationIndex < Locations.Count - 1)
						{
							Locations.RemoveRange(CurrentLocationIndex + 1, Locations.Count - (CurrentLocationIndex + 1));
						}
						Locations.Add(args.NewLocation);
						CurrentLocationIndex = Locations.Count - 1;
						eventArgs.LocationsChanged = true;
					}
					else
					{
						// log traversal successful, update index
						CurrentLocationIndex = pendingNavigation.Index;
						eventArgs.LocationsChanged = false;
					}
					pendingNavigation = null;
				}
				else
				{
					// remove history following currentLocationIndex, append new item
					if (CurrentLocationIndex < Locations.Count - 1)
					{
						Locations.RemoveRange(CurrentLocationIndex + 1, Locations.Count - (CurrentLocationIndex + 1));
					}
					Locations.Add(args.NewLocation);
					CurrentLocationIndex = Locations.Count - 1;
					eventArgs.LocationsChanged = true;
				}

				// update event args
				eventArgs.CanNavigateBackwardChanged = oldCanNavigateBackward != CanNavigateBackward;
				eventArgs.CanNavigateForwardChanged = oldCanNavigateForward != CanNavigateForward;

				NavigationLogChanged?.Invoke(this, eventArgs);
			}

			private void OnNavigationFailed(object sender, NavigationFailedEventArgs args) => pendingNavigation = null;

			/// <summary>A navigation traversal request</summary>
			private class PendingNavigation
			{
				internal PendingNavigation(ShellItem location, int index)
				{
					Location = location;
					Index = index;
				}

				internal int Index { get; set; }

				internal ShellItem Location { get; set; }
			}
		}

        /// <summary>Controls the visibility of the various ExplorerBrowser panes on subsequent navigation</summary>
        // FIX by taj 16/01/19 - [TypeConverter(typeof(BetterExpandableObjectConverter)), Serializable]
        public class ExplorerBrowserPaneVisibility
		{
			/// <summary>Additional fields and options to aid in a search.</summary>
			[DefaultValue(PaneVisibilityState.Default), Category("Appearance"), Description("Additional fields and options to aid in a search.")]
			public PaneVisibilityState AdvancedQuery { get; set; } = PaneVisibilityState.Default;

			/// <summary>Commands module along the top of the Windows Explorer window.</summary>
			[DefaultValue(PaneVisibilityState.Default), Category("Appearance"), Description("Commands module along the top of the Windows Explorer window.")]
			public PaneVisibilityState Commands { get; set; } = PaneVisibilityState.Default;

			/// <summary>Organize menu within the commands module.</summary>
			[DefaultValue(PaneVisibilityState.Default), Category("Appearance"), Description("Organize menu within the commands module.")]
			public PaneVisibilityState CommandsOrganize { get; set; } = PaneVisibilityState.Default;

			/// <summary>View menu within the commands module.</summary>
			[DefaultValue(PaneVisibilityState.Default), Category("Appearance"), Description("View menu within the commands module.")]
			public PaneVisibilityState CommandsView { get; set; } = PaneVisibilityState.Default;

			/// <summary>Pane showing metadata along the bottom of the Windows Explorer window.</summary>
			[DefaultValue(PaneVisibilityState.Default), Category("Appearance"), Description("Pane showing metadata along the bottom of the Windows Explorer window.")]
			public PaneVisibilityState Details { get; set; } = PaneVisibilityState.Default;

			/// <summary>The pane on the left side of the Windows Explorer window that hosts the folders tree and Favorites.</summary>
			[DefaultValue(PaneVisibilityState.Default), Category("Appearance"), Description("The pane on the left side of the Windows Explorer window that hosts the folders tree and Favorites.")]
			public PaneVisibilityState Navigation { get; set; } = PaneVisibilityState.Default;

			/// <summary>Pane on the right of the Windows Explorer window that shows a large reading preview of the file.</summary>
			[DefaultValue(PaneVisibilityState.Default), Category("Appearance"), Description("Pane on the right of the Windows Explorer window that shows a large reading preview of the file.")]
			public PaneVisibilityState Preview { get; set; } = PaneVisibilityState.Default;

			/// <summary>Quick filter buttons to aid in a search.</summary>
			[DefaultValue(PaneVisibilityState.Default), Category("Appearance"), Description("Quick filter buttons to aid in a search.")]
			public PaneVisibilityState Query { get; set; } = PaneVisibilityState.Default;

			/// <summary>
			/// Introduced in Windows 8: The ribbon, which is the control that replaced menus and toolbars at the top of many Microsoft applications.
			/// </summary>
			[DefaultValue(PaneVisibilityState.Default), Category("Appearance"), Description("The ribbon, which is the control that replaced menus and toolbars at the top of many Microsoft applications.")]
			public PaneVisibilityState Ribbon { get; set; } = PaneVisibilityState.Default;

			/// <summary>Introduced in Windows 8: A status bar that indicates the progress of some process, such as copying or downloading.</summary>
			[DefaultValue(PaneVisibilityState.Default), Category("Appearance"), Description("A status bar that indicates the progress of some process, such as copying or downloading.")]
			public PaneVisibilityState StatusBar { get; set; } = PaneVisibilityState.Default;
		}

		/// <summary>This provides a connection point container compatible dispatch interface for hooking into the ExplorerBrowser view.</summary>
		[ComVisible(true)]
		[ClassInterface(ClassInterfaceType.AutoDual)]
		private class ExplorerBrowserViewEvents : IDisposable
		{
			private const int ContentsChanged = 207;
			private const int FileListEnumDone = 201;
			private const int SelectedItemModified = 220;
			private const int SelectionChanged = 200;
			private static readonly Guid IID_DShellFolderViewEvents = new Guid("62112AA2-EBE4-11cf-A5FB-0020AFE7292D");
			private static readonly Guid IID_IDispatch = new Guid("00020400-0000-0000-C000-000000000046");
			private ExplorerBrowserControl parent;
			private uint viewConnectionPointCookie;
			private object viewDispatch;

			/// <summary>Default constructor for ExplorerBrowserViewEvents</summary>
			public ExplorerBrowserViewEvents() : this(null) { }

			internal ExplorerBrowserViewEvents(ExplorerBrowserControl parent) => this.parent = parent;

			/// <summary>Finalizes ExplorerBrowserViewEvents</summary>
			~ExplorerBrowserViewEvents()
			{
				Dispose(false);
			}

			/// <summary>Disconnects and disposes object.</summary>
			public void Dispose()
			{
				Dispose(true);
				GC.SuppressFinalize(this);
			}

			/// <summary>The contents of the view have changed</summary>
			[DispId(ContentsChanged)]
			public void ViewContentsChanged() => parent.OnItemsChanged();

			/// <summary>The enumeration of files in the view is complete</summary>
			[DispId(FileListEnumDone)]
			public void ViewFileListEnumDone() => parent.OnItemsEnumerated();

			/// <summary>The selected item in the view has changed (not the same as the selection has changed)</summary>
			[DispId(SelectedItemModified)]
			public void ViewSelectedItemModified() => parent.OnSelectedItemModified();

			/// <summary>The view selection has changed</summary>
			[DispId(SelectionChanged)]
			public void ViewSelectionChanged() => parent.OnSelectionChanged();

			internal void ConnectToView(IShellView psv)
			{
				DisconnectFromView();
				viewDispatch = psv.GetItemObject(SVGIO.SVGIO_BACKGROUND, IID_IDispatch);
				var hr = ConnectToConnectionPoint(this, IID_DShellFolderViewEvents, true, viewDispatch, ref viewConnectionPointCookie, out var _);
				if (hr != HRESULT.S_OK)
					viewDispatch = null;
			}

			internal void DisconnectFromView()
			{
				if (viewDispatch is null) return;
				ConnectToConnectionPoint(null, IID_DShellFolderViewEvents, false, viewDispatch, ref viewConnectionPointCookie, out var _);
				viewDispatch = null;
				viewConnectionPointCookie = 0;
			}

			// These need to be public to be accessible via AutoDual reflection
			/// <summary>Disconnects and disposes object.</summary>
			/// <param name="disposed"></param>
			protected virtual void Dispose(bool disposed)
			{
				if (disposed)
				{
					DisconnectFromView();
				}
			}
		}

		/// <summary>Represents a collection of <see cref="ShellItem"/> attached to an <see cref="ExplorerBrowserControl"/>.</summary>
		private class ShellItemCollection : IReadOnlyList<ShellItem>
		{
			private readonly ExplorerBrowserControl eb;
			private readonly SVGIO option;

			internal ShellItemCollection(ExplorerBrowserControl eb, SVGIO opt)
			{
				this.eb = eb;
				option = opt;
			}

			/// <summary>Gets the number of elements in the collection.</summary>
			/// <value>Returns a <see cref="int"/> value.</value>
			public int Count => (int)Array.GetCount();

			private IShellItemArray Array => eb.GetItemsArray(option);

			private IEnumerable<IShellItem> Items
			{
				get
				{
					var array = Array;
					for (uint i = 0; i < array.GetCount(); i++)
						yield return array.GetItemAt(i);
				}
			}

			/// <summary>Gets the <see cref="ShellItem"/> at the specified index.</summary>
			/// <value>The <see cref="ShellItem"/>.</value>
			/// <param name="index">The zero-based index of the element to get.</param>
			public ShellItem this[int index]
			{
				get
				{
					try
					{
						return ShellItem.Open(Array.GetItemAt((uint)index));
					}
					catch
					{
						return null;
					}
				}
			}

			/// <summary>Returns an enumerator that iterates through the collection.</summary>
			/// <returns>An enumerator that can be used to iterate through the collection.</returns>
			public IEnumerator<ShellItem> GetEnumerator() => Items.Select(ShellItem.Open).GetEnumerator();

			/// <summary>Returns an enumerator that iterates through the collection.</summary>
			/// <returns>An enumerator that can be used to iterate through the collection.</returns>
			IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
		}
	}

	/// <summary>Event argument for The Navigated event</summary>
	public class NavigatedEventArgs : EventArgs
	{
		/// <summary>The new location of the explorer browser</summary>
		public ShellItem NewLocation { get; set; }
	}

	/// <summary>Event argument for The Navigating event</summary>
	public class NavigatingEventArgs : EventArgs
	{
		/// <summary>Set to 'True' to cancel the navigation.</summary>
		public bool Cancel { get; set; }

		/// <summary>The location being navigated to</summary>
		public ShellItem PendingLocation { get; set; }
	}

	/// <summary>Event argument for the NavigatinoFailed event</summary>
	public class NavigationFailedEventArgs : EventArgs
	{
		/// <summary>The location the browser would have navigated to.</summary>
		public ShellItem FailedLocation { get; set; }
	}

	/// <summary>The event argument for NavigationLogChangedEvent</summary>
	public class NavigationLogEventArgs : EventArgs
	{
		/// <summary>Indicates CanNavigateBackward has changed</summary>
		public bool CanNavigateBackwardChanged { get; set; }

		/// <summary>Indicates CanNavigateForward has changed</summary>
		public bool CanNavigateForwardChanged { get; set; }

		/// <summary>Indicates the Locations collection has changed</summary>
		public bool LocationsChanged { get; set; }
	}
}