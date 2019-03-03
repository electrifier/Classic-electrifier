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
using System.Collections;

using electrifier.Core.Components;
using electrifier.Core.Components.DockContents;


namespace electrifier.Core.Forms
{
    public partial class Electrifier
      : IElNavigationHost
    {
        protected internal ArrayList dockContentList = new ArrayList();
        protected internal ElNavigableDockContent activeNavigableDockContent = null;

        public void AddDockContent(ElNavigableDockContent DockContent)
        {
            AppContext.TraceScope();

            this.dockContentList.Add(DockContent);

            DockContent.Activated += this.DockContent_Activated;
            DockContent.FormClosed += this.DockContent_FormClosed;

            DockContent.NavigationOptionsChanged += this.DockContent_NavigationOptionsChanged;


            // TODO: Connect events!

            //newDockContent.ItemsChanged += this.NewDockContent_ItemsChanged;
            //newDockContent.SelectionChanged += this.NewDockContent_SelectionChanged;
            //newDockContent.NavigationLogChanged += this.NewDockContent_NavigationLogChanged;

            DockContent.Show(this.dpnDockPanel);    // DockState.Document); // TODO: Previous pane?!?
        }

        public void ActivateDockContent(ElNavigableDockContent DockContent)
        {
            AppContext.TraceScope();

            // Check if active DockContent has changed at all
            if (this.GetActiveDockContent() == DockContent)
                return;

            if(!this.dockContentList.Contains(DockContent))
                throw new ArgumentException("DockContent never has been added to NavigationHost");

            //this.activeNavigableDockContent?.Deactivate();
            this.activeNavigableDockContent = DockContent;

            // Activate the underlying DockContent if not already active
            if (!DockContent.IsActivated)
                DockContent.Activate();

            this.ntsNavigation.ActiveDockContent = DockContent;
        }

        public ElNavigableDockContent GetActiveDockContent() => this.activeNavigableDockContent;

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
        private void DockContent_Activated(/* TODO: ElNavigableDockContent */object sender, EventArgs e)
        {
            AppContext.TraceScope();

            this.ActivateDockContent(sender as ElNavigableDockContent);
        }

        private void DockContent_FormClosed(/* TODO: ElNavigableDockContent */object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            AppContext.TraceScope();

            this.RemoveDockContent(sender as ElNavigableDockContent);
        }

        private void DockContent_NavigationOptionsChanged(/* TODO: ElNavigableDockContent */object sender, EventArgs e)
        {
            AppContext.TraceScope();

            if (sender == this.GetActiveDockContent())
                this.ntsNavigation.UpdateButtonState(sender as ElNavigableDockContent);
        }

        #endregion =============================================================================================================
    }
}
