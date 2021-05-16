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
using WeifenLuo.WinFormsUI.Docking;

namespace electrifier.Core.Forms
{
    public partial class ApplicationWindow
      : INavigationHost
    {
        private ArrayList dockContentList = new ArrayList();
        private DockContent activeDockContent;
        public DockContent ActiveDockContent { get => this.activeDockContent; }

        public void AddDockContent(DockContent dockContent, DockState dockState)
        {
            AppContext.TraceScope();

            if (null == dockContent)
                throw new ArgumentNullException(nameof(dockContent));

            this.dockContentList.Add(dockContent);

            dockContent.Activated += this.DockContent_Activated;
            dockContent.FormClosed += this.DockContent_FormClosed;

            dockContent.Show(this.dpnDockPanel, dockState);    // TODO: Previous pane?!?
        }


        public void ActivateDockContent(DockContent dockContent)
        {
            AppContext.TraceScope();

            if (null == dockContent)
                throw new ArgumentNullException(nameof(dockContent));

            // Check if active DockContent has changed at all
            if (this.ActiveDockContent == dockContent)
                return;

            if (!this.dockContentList.Contains(dockContent))
                throw new ArgumentException("DockContent never has been added to NavigationHost");

            //this.activeDockContent?.Deactivate();
            this.activeDockContent = dockContent;

            // Activate the underlying DockContent if not already active
            if (!dockContent.IsActivated)
                dockContent.Activate();



            AppContext.TraceDebug("CHANGED by Activation - NavigationHost - ViewMode: " + this.RibbonItems.ShellFolderViewMode);

        }



        public void RemoveDockContent(DockContent dockContent)
        {
            AppContext.TraceScope();

            if (null == dockContent)
                throw new ArgumentNullException(nameof(dockContent));

            // Disconnect events
            dockContent.Activated -= this.DockContent_Activated;

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
            Debug.Assert(sender is DockContent);

            this.ActivateDockContent(sender as DockContent);
        }

        private void DockContent_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            Debug.Assert(sender is DockContent);

            this.RemoveDockContent(sender as DockContent);
        }

        #endregion =============================================================================================================
    }
}
