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
using electrifier.Core.Components.DockContents;


namespace electrifier.Core.Forms
{
    public partial class ElApplicationWindow
      : IElNavigationHost
    {
        protected internal ArrayList dockContentList = new ArrayList();
        protected internal ElNavigableDockContent activeDockContent = null;
        public ElNavigableDockContent ActiveDockContent { get => this.activeDockContent; }

        public void AddDockContent(ElNavigableDockContent DockContent)
        {
            AppContext.TraceScope();

            this.dockContentList.Add(DockContent);

            DockContent.Activated += this.DockContent_Activated;
            DockContent.FormClosed += this.DockContent_FormClosed;

            // Connect navigation options changed event
            DockContent.NavigationOptionsChanged += this.DockContent_NavigationOptionsChanged;

            // Connect clipboard consumer events
            if (DockContent is IElClipboardConsumer clipboardConsumer)
                clipboardConsumer.ClipboardAbilitiesChanged += this.rbnRibbon.ClipboardConsumer_ClipboardAbilitiesChanged;

            // TODO: Connect events!
            //newDockContent.ItemsChanged += this.NewDockContent_ItemsChanged;
            //newDockContent.SelectionChanged += this.NewDockContent_SelectionChanged;

            DockContent.Show(this.dpnDockPanel);    // DockState.Document); // TODO: Previous pane?!?
        }

        public void ActivateDockContent(ElNavigableDockContent DockContent)
        {
            AppContext.TraceScope();

            // Check if active DockContent has changed at all
            if (this.ActiveDockContent == DockContent)
                return;

            if(!this.dockContentList.Contains(DockContent))
                throw new ArgumentException("DockContent never has been added to NavigationHost");

            //this.activeDockContent?.Deactivate();
            this.activeDockContent = DockContent;

            // Activate the underlying DockContent if not already active
            if (!DockContent.IsActivated)
                DockContent.Activate();

            // Update navigation bar, i.e. its button states
            this.ntsNavigation.ActiveDockContent = DockContent;
        }

        public void RemoveDockContent(ElNavigableDockContent DockContent)
        {
            AppContext.TraceScope();

            DockContent.Activated -= this.DockContent_Activated;
            DockContent.NavigationOptionsChanged -= this.DockContent_NavigationOptionsChanged;

            this.dockContentList.Remove(DockContent);
        }

        #region DockContent event handlers =====================================================================================

        /// <summary>
        /// DockContent_Activated is called when DockContent is floating (undocked) and has been activated.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DockContent_Activated(object sender, EventArgs e)
        {
            Debug.Assert(sender is ElNavigableDockContent, "sender is not of type ElNavigableDockContent");

            this.ActivateDockContent(sender as ElNavigableDockContent);
        }

        private void DockContent_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            Debug.Assert(sender is ElNavigableDockContent, "sender is not of type ElNavigableDockContent");

            this.RemoveDockContent(sender as ElNavigableDockContent);
        }

        private void DockContent_NavigationOptionsChanged(object sender, EventArgs e)
        {
            Debug.Assert(sender is ElNavigableDockContent, "sender is not of type ElNavigableDockContent");

            if (sender.Equals(this.ActiveDockContent))
                this.ntsNavigation.UpdateButtonState(sender as ElNavigableDockContent);
        }

        #endregion =============================================================================================================
    }
}
