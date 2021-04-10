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
using System.Collections;
using System.Diagnostics;

using electrifier.Core.Components;
using electrifier.Core.Components.Controls;
using electrifier.Core.Components.DockContents;


namespace electrifier.Core.Forms
{
    public partial class ElApplicationWindow
      : INavigationHost
    {
        private ArrayList dockContentList = new ArrayList();
        private NavigableDockContent activeDockContent;
        public NavigableDockContent ActiveDockContent { get => this.activeDockContent; }

        public void AddDockContent(NavigableDockContent dockContent)
        {
            AppContext.TraceScope();

            if (null == dockContent)
                throw new ArgumentNullException(nameof(dockContent));

            this.dockContentList.Add(dockContent);

            dockContent.Activated += this.DockContent_Activated;
            dockContent.FormClosed += this.DockContent_FormClosed;

            // Connect navigation options changed event
            dockContent.NavigationOptionsChanged += this.DockContent_NavigationOptionsChanged;
            if (dockContent.HasShellFolderViewMode)
                dockContent.ShellFolderViewModeChanged += this.DockContent_ShellFolderViewModeChanged;

            // Connect clipboard consumer events
            if (dockContent is IClipboardConsumer clipboardConsumer)
                clipboardConsumer.ClipboardAbilitiesChanged += this.RibbonItems.ClipboardAbilitiesChanged;


            // TODO: Connect events!
            //newDockContent.ItemsChanged += this.NewDockContent_ItemsChanged;
            //newDockContent.SelectionChanged += this.NewDockContent_SelectionChanged;

            dockContent.Show(this.dpnDockPanel);    // DockState.Document); // TODO: Previous pane?!?
        }

        public void ActivateDockContent(NavigableDockContent dockContent)
        {
            AppContext.TraceScope();

            if (null == dockContent)
                throw new ArgumentNullException(nameof(dockContent));

            // Check if active DockContent has changed at all
            if (this.ActiveDockContent == dockContent)
                return;

            if(!this.dockContentList.Contains(dockContent))
                throw new ArgumentException("DockContent never has been added to NavigationHost");

            //this.activeDockContent?.Deactivate();
            this.activeDockContent = dockContent;

            // Activate the underlying DockContent if not already active
            if (!dockContent.IsActivated)
                dockContent.Activate();


            if (dockContent is ShellFolderDockContent shellBolderDockContent)
                this.RibbonItems.ShellFolderViewMode = shellBolderDockContent.ShellFolderViewMode;
            else
                this.RibbonItems.ShellFolderViewMode = /* TODO: None */ Vanara.PInvoke.Shell32.FOLDERVIEWMODE.FVM_AUTO;

            AppContext.TraceDebug("CHANGED by Activation - NavigationHost - ViewMode: " + this.RibbonItems.ShellFolderViewMode);

            // Update navigation bar, i.e. its button states
            this.NavigationToolStrip.ActiveDockContent = dockContent;
        }

        public void RemoveDockContent(NavigableDockContent dockContent)
        {
            AppContext.TraceScope();

            if (null == dockContent)
                throw new ArgumentNullException(nameof(dockContent));

            // Disconnect events
            dockContent.Activated -= this.DockContent_Activated;
            dockContent.ShellFolderViewModeChanged -= this.DockContent_ShellFolderViewModeChanged;
            dockContent.NavigationOptionsChanged -= this.DockContent_NavigationOptionsChanged;

            this.dockContentList.Remove(dockContent);
        }

        #region DockContent event handlers =====================================================================================

        /// <summary>
        /// DockContent_Activated is called when DockContent is floating (undocked) and has been activated.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DockContent_Activated(object sender, EventArgs e)
        {
            Debug.Assert(sender is NavigableDockContent, "sender is not of type NavigableDockContent");

            this.ActivateDockContent(sender as NavigableDockContent);
        }

        private void DockContent_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            Debug.Assert(sender is NavigableDockContent, "sender is not of type NavigableDockContent");

            this.RemoveDockContent(sender as NavigableDockContent);
        }

        private void DockContent_NavigationOptionsChanged(object sender, EventArgs e)
        {
            Debug.Assert(sender is NavigableDockContent, "sender is not of type NavigableDockContent");

            if (sender.Equals(this.ActiveDockContent))
                this.NavigationToolStrip.UpdateButtonState(sender as NavigableDockContent);
        }

        // TODO: 18/11/19: ShellFolderViewMode should be placed into its own Interface => Class are fast, interfaces are slow!
        private void DockContent_ShellFolderViewModeChanged(object sender, ShellFolderViewModeChangedEventArgs e)
        {
            Debug.Assert(sender is NavigableDockContent, "sender is not of type NavigableDockContent");

            AppContext.TraceDebug("CHANGED by DockContent_ShellFolderViewModeChanged - NavigationHost - ViewMode: " + e.NewFolderViewMode);

            if (sender.Equals(this.ActiveDockContent))
                this.RibbonItems.ShellFolderViewMode = e.NewFolderViewMode;
        }

        #endregion =============================================================================================================
    }
}
