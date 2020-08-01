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
using System.Text;
using System.Windows.Forms;
using electrifier.Core.Components.Controls;
using Vanara.PInvoke;
using Vanara.Windows.Shell;

namespace electrifier.Core.Components.DockContents
{
    /// <summary>
    /// ElShellBrowserDockContent is electrifier's wrapper class for ExplorerBrowser Control.
    /// 
    /// A reference implementation of a wrapper for ExplorerBrowser can be found
    /// <a href="https://github.com/aybe/Windows-API-Code-Pack-1.1/blob/master/source/Samples/ExplorerBrowser/CS/WinForms/ExplorerBrowserTestForm.cs">here</a>.
    /// </summary>
    public partial class ElShellBrowserDockContent
    {
        #region Fields ========================================================================================================

        protected const string persistParamURI = @"URI=";
        protected const string persistParamViewMode = @"ViewMode=";

        //private ExplorerBrowserViewMode? initialViewMode = null;

        #endregion Fields =====================================================================================================

        #region Properties ====================================================================================================

        public ShellItem InitialNaviagtionTarget { get; private set; } = default;

        public Shell32.FOLDERVIEWMODE ViewMode
        {
            get => Shell32.FOLDERVIEWMODE.FVM_AUTO; //this.ExplorerBrowserControl.ViewMode;
            set => value = value; //this.ExplorerBrowserControl.ViewMode = value;
        }

        //public ExplorerBrowserViewMode ViewMode { get => this.explorerBrowser.ContentOptions.ViewMode; set => this.explorerBrowser.ContentOptions.ViewMode = value; }

        //public int ItemsCount {
        //    //get => this.explorerBrowser.Items.Count; }
        //    get => 0;
        //}
        //public int SelectedItemsCount {
        //    //get => this.explorerBrowser.SelectedItems.Count; }
        //    get => 0;
        //}
//        protected ExplorerBrowserControl ExplorerBrowserControl { get; }

        //public DockContent AsDockContent { get => this as WeifenLuo.WinFormsUI.Docking.DockContent; }

        //public ExplorerBrowserNavigationLog NavigationLog { get => this.explorerBrowser.NavigationLog; }

        #endregion Properties =================================================================================================

        #region Published Events ==============================================================================================


        #endregion Published Events ===========================================================================================

        // TODO: Work on Get/Lost Focus in general!

        public ElShellBrowserDockContent(IElNavigationHost navigationHost, string persistString = null)
          : base(navigationHost)
        {
            this.SuspendLayout();

            try
            {
                this.Icon = Properties.Resources.ShellBrowserDockContent;

                // Initialize ElNavigableDockContent backing fields
//  CR13                this.HistoryItems = new ElNavigableTargetItemCollection<ElNavigableTargetNavigationLogIndex>(this);

                // Evaluate persistString
                this.EvaluatePersistString(persistString);

                // Initialize ExplorerBrowser
//                this.ExplorerBrowserControl = new Controls.ExplorerBrowserControl(this.InitialNaviagtionTarget)
//                {
//                    Dock = DockStyle.Fill,
//                };
//
//                // Connect ExplorerBrowser Events
//                this.ExplorerBrowserControl.SelectionChanged += this.ElClipboardConsumer_SelectionChanged;
//                this.ExplorerBrowserControl.Navigating += this.ExplorerBrowserControl_Navigating;
//                this.ExplorerBrowserControl.Navigated += this.ExplorerBrowserControl_Navigated;
//                this.ExplorerBrowserControl.NavigationFailed += this.ExplorerBrowserControl_NavigationFailed;
//                this.ExplorerBrowserControl.ItemsEnumerated += this.ExplorerBrowserControl_ItemsEnumerated;
//                this.ExplorerBrowserControl.ShellFolderViewModeChanged += this.ExplorerBrowserControl_ShellFolderViewModeChanged;
//                this.ExplorerBrowserControl.History.NavigationLogChanged += this.ExplorerBrowserControl_History_NavigationLogChanged;
//
//                this.Controls.Add(this.ExplorerBrowserControl);
            }
            finally
            {
                this.ResumeLayout();
            }
        }

//        private void ExplorerBrowserControl_ShellFolderViewModeChanged(object sender, ExplorerBrowserControl.ShellFolderViewModeChangedEventArgs e)
//        {
//            AppContext.TraceDebug($"CHANGED ElShellBrowserDockContent.ViewMode: Was {e.OldFolderViewMode}, is now {e.NewFolderViewMode}");
//
//            this.ShellFolderViewModeChanged?.Invoke(this, e);
//        }

        protected override void Dispose(bool disposing)
        {
//            if (null != this.ExplorerBrowserControl)
//                this.ExplorerBrowserControl.Dispose();
//
            base.Dispose(disposing);
        }

        public void SelectAll()
        {
//            this.ExplorerBrowserControl.SelectAll();
        }

        public void SelectNone()
        {
//            this.ExplorerBrowserControl.SelectNone();
        }

        public void InvertSelection()
        {
//            this.ExplorerBrowserControl.InvertSelection();
        }


        #region DockContent Persistence Overrides =============================================================================

        /// <summary>
        /// Override of WeifenLuo.WinFormsUI.Docking.DockContent.GetPersistString()
        /// </summary>
        /// <returns>The string describing persistence information. E.g. persistString = "ElShellBrowserDockContent URI=file:///C:/Users/tajbender/Desktop";</returns>
        protected override string GetPersistString()
        {
            var sb = new StringBuilder();
            string paramFmt = " {0}{1}";

            // Append class name as identifier
            sb.Append(nameof(ElShellBrowserDockContent));

            // Append URI of current location
//            sb.AppendFormat(paramFmt, ElShellBrowserDockContent.persistParamURI,
//                WindowsShell.ElShellTools.UrlCreateFromPath(this.ExplorerBrowserControl.CurrentLocation));
//
            // Append ViewMode
            // TODO: For any reason, this doesn't work... :(
            //sb.AppendFormat(paramFmt, ElShellBrowserDockContent.persistParamViewMode, this.ViewMode);

            return sb.ToString();
        }

        /// <summary>
        /// Example: persistString = "ElShellBrowserDockContent URI=file:///C:/Users/tajbender/Desktop";
        /// </summary>
        /// <param name="persistString"></param>
        protected void EvaluatePersistString(string persistString)
        {
            try
            {
                if ((null != persistString) && (persistString.Trim().Length > ElShellBrowserDockContent.persistParamURI.Length))
                {
                    var args = WindowsShell.ElShellTools.SplitArgumentString(persistString);
                    string strInitialNavigationTarget = default;
                    string strInitialViewMode = default;

                    foreach (string arg in args)
                    {
                        if (arg.StartsWith(ElShellBrowserDockContent.persistParamURI))
                        {
                            strInitialNavigationTarget = WindowsShell.ElShellTools.PathCreateFromUrl(arg.Substring(ElShellBrowserDockContent.persistParamURI.Length));
                        }

                        if (arg.StartsWith(ElShellBrowserDockContent.persistParamViewMode))
                        {
                            strInitialViewMode = arg.Substring(ElShellBrowserDockContent.persistParamViewMode.Length);
                        }
                    }

                    // Finally, when all parameters have been parsed successfully, apply them
                    if (default != strInitialNavigationTarget)
                        this.InitialNaviagtionTarget = new ShellItem(strInitialNavigationTarget);

                    //if (null != strViewMode)
                    //{
                    //    ExplorerBrowserViewMode ebvm = ExplorerBrowserViewMode.Auto;

                    //    ebvm = (ExplorerBrowserViewMode) Enum.Parse(typeof(ExplorerBrowserViewMode), strViewMode);
                    //    this.initialViewMode = ebvm;
                    //}
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("ElShellBrowserDockContent.EvaluatePersistString: Error evaluating parameters"
                    + "\n\nParameters: '" + persistString + "'"
                    + "\n\nError description: '" + e.Message + "'"
                    + "\n\nResetting to default values.");
            }
        }

        #endregion DockContent Persistence Overrides ==========================================================================

        #region DockContent Event Handler =====================================================================================

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            // TODO: When multiple Tabs are opened initially, sometimes this won't be called for some folders... Most likely if invisible!
            // HACK: 13/05/19: Commented out, since InitialNaviagtionTarget is already set at construction.
            //if (default != this.InitialNaviagtionTarget)
            //    this.explorerBrowserControl.NavigateTo(this.InitialNaviagtionTarget);

            //if (null != this.initialViewMode)
            //{
            //    this.ViewMode = (ExplorerBrowserViewMode)this.initialViewMode;
            //}
        }

        #endregion DockContent Event Handler ==================================================================================


//        #region ExplorerBrowser Internal Events Handler ========================================================================
//
//        protected void ExplorerBrowserControl_Navigating(object sender, Controls.ExplorerBrowserControl.NavigatingEventArgs args)
//        {
//        }
//
//
//        protected void ExplorerBrowserControl_Navigated(object sender, Controls.ExplorerBrowserControl.NavigatedEventArgs args)
//        {
//            AppContext.TraceDebug("Firing of ExplorerBrowserControl_Navigated event.");
//
//            this.BeginInvoke(new MethodInvoker(delegate ()
//            {
//                this.Text = args.NewLocation.Name;
//                this.currentLocation = args.NewLocation.GetDisplayName(ShellItemDisplayString.DesktopAbsoluteEditing);
//
//                //args.NewLocation.ViewInExplorer(); // TODO: Nice to have :) However, shows parent with item selected
//                //this.Icon = args.NewLocation.Thumbnail.SmallIcon;     // TODO: Icon-Property seems not to be thread-safe
//
//                this.OnNavigationOptionsChanged(null);  // TODO: Set some args
//
//            }));
//        }
//
//        protected void ExplorerBrowserControl_NavigationFailed(object sender, Controls.ExplorerBrowserControl.NavigationFailedEventArgs args)
//        {
//            AppContext.TraceError("Firing of ExplorerBrowserControl_NavigationFailed event: " + args.FailedLocation.ParsingName + args.ToString());
//        }
//
//        protected void ExplorerBrowserControl_ItemsEnumerated(object sender, EventArgs args)
//        {
//            AppContext.TraceDebug("Firing of ExplorerBrowserControl_ItemsEnumerated event.");
//        }
//
//        private void ExplorerBrowserControl_History_NavigationLogChanged(object sender, Controls.ExplorerBrowserControl.NavigationLogEventArgs args)
//        {
//            // HACK: args.CanNavigateBackwardChanged / args.CanNavigateForwardChanged is currently ignored...
//
//            if (args.LocationsChanged)
//            {
//                this.HistoryItems.Clear();
//
//                // TODO: Instead of "copying" the HistoryItems, just point to ExplorerBrowserControl's History.Locations
//                // TODO: Convert HistoryItems to ShellItems-Array, add explicit conversion ShellItems->HistoryItems
//                foreach (var location in this.ExplorerBrowserControl.History.Locations)
//                {
//                    this.HistoryItems.AddNewItem(location);
//
//                    // TODO: delete the follwoing check and its warning
//                    if (!location.IsFolder)
//                        AppContext.TraceWarning("History.Location is not a folder!");
//                }
//
//                // TODO: Update ToolStripDropDownButton!
//            }
//        }
//
//        #endregion =============================================================================================================
    }
}
