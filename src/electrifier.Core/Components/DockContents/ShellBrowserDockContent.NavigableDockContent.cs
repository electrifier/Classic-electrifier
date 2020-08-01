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


using electrifier.Core.Components.Controls;
using System;
using Vanara.PInvoke;
using Vanara.Windows.Shell;

namespace electrifier.Core.Components.DockContents
{
    /// <summary>
    /// ElShellBrowserDockContent is electrifier's wrapper class for ExplorerBrowser Control.
    /// 
    /// This partial class file contains the <see cref="NavigableDockContent"/> implementation.
    /// </summary>
    public partial class ElShellBrowserDockContent
      : NavigableDockContent
    {
        internal string currentLocation;
/* CR13
        //public override bool CanGoBack => this.ExplorerBrowserControl.History.CanNavigateBackward;

        //public override void GoBack()
        //{
        //    this.ExplorerBrowserControl.History.NavigateLog(
        //        Components.Controls.ExplorerBrowserControl.NavigationLogDirection.Backward);
        //}

        //public override bool CanGoForward => this.ExplorerBrowserControl.History.CanNavigateForward;

        //public override void GoForward()
        //{
        //    this.ExplorerBrowserControl.History.NavigateLog(
        //        Components.Controls.ExplorerBrowserControl.NavigationLogDirection.Forward);
        //}

        //public override ElNavigableTargetItemCollection<ElNavigableTargetNavigationLogIndex> HistoryItems { get; }
*/

        public override string CurrentLocation
        {
            get => this.currentLocation;
            set => this.currentLocation = value;
        }

        // TODO: Currently obsolete as of 28/04/19. But in the future, this will open a new DockContent of approriate type,
        //       if one of the supported types is opened, e.g. text-file, md-file, icon-libraray...
        // TODO: Anyways, this should reside in AppContext
        // 
        //public void NavigateToLocation(object value)
        //{
        //    try
        //    {
        //        // Use type pattern to try to cast given value to ShellFolder
        //        // <see https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/switch >
        //        switch (value)
        //        {
        //            case ShellFolder shellFolder:
        //                this.explorerBrowserControl.NavigateTo(shellFolder);
        //                break;
        //            case string strPath:
        //                this.explorerBrowserControl.NavigateTo(new ShellFolder(strPath));
        //                break;
        //            case object obj:
        //                throw new ArgumentException("Can't cast CurrentLocation to value of type " + value.GetType().ToString());
        //            default:
        //                throw new ArgumentNullException("Given CurrentLocation is null");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new ArgumentException("Setting CurrentLocation failed. Value = " + value.ToString(), ex);
        //    }
        //}

        /// <summary>
        /// Navigate within the navigation log. This does not change the set of locations in the navigation log.
        /// </summary>
        /// <param name="historyIndex">An index into the navigation logs Locations collection.</param>
        /// <returns>True if the navigation succeeded, false if it failed for any reason.</returns>
        ///   TODO: This will be replaced by CurrentLocation(HistoryIndex historyIndex);
        //public override bool GoToHistoryItem(int historyIndex)
        //    => this.ExplorerBrowserControl.NavigateToHistoryIndex(historyIndex);

        //protected internal ElNavigableTargetItemCollection recentLocationsList = null;
        //public override ElNavigableTargetItemCollection RecentLocations => base.RecentLocations;


        //public override bool CanHaveQuickAccesItems() => true;
        //protected internal ElNavigableTargetItemCollection ntcQuickAccessItems = null;
        //public override ElNavigableTargetItemCollection QuickAccessItems { get => this.ntcQuickAccessItems; }





        public override event EventHandler NavigationOptionsChanged;

        public virtual void OnNavigationOptionsChanged(EventArgs args)
        {
            this.NavigationOptionsChanged?.Invoke(this, args);
        }

        public override bool HasShellFolderViewMode => true;

        public override Shell32.FOLDERVIEWMODE ShellFolderViewMode
        {
            get => this.ViewMode;
            set => this.ViewMode = value;
        }

        public override event EventHandler<ShellFolderViewModeChangedEventArgs> ShellFolderViewModeChanged;
    }
}
