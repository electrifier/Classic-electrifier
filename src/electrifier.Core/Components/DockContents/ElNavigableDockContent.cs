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

namespace electrifier.Core.Components.DockContents
{
    /// <summary>
    /// Abstract class ElNavigableDockContent is the skeleton for navigation clients that
    /// are controlled by an implementer of the <see cref="IElNavigationHost"/> interface.
    /// </summary>
    public abstract class ElNavigableDockContent
      : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        // TODO: Add Clipboard functionality

        public IElNavigationHost NavigationHost { get; private set; }

        public ElNavigableDockContent(IElNavigationHost navigationHost)
            : base()
        {
            this.NavigationHost = navigationHost ??
                throw new ArgumentNullException("Instantiation of ElNavigableDockContent not allowed without given IElNavigationHost");

            //this.OnActivated TODO: Store previous DockContent for activation chain?
        }

        public virtual bool CanGoBack() { return false; }
        public virtual void GoBack() { throw new NotImplementedException(); }

        public virtual bool CanGoForward() { return false; }
        public virtual void GoForward() { throw new NotImplementedException(); }

        public virtual bool HasHistoryItems() { return false; }
        public virtual object[] HistoryItems { get; }
        public virtual void GoToHistoryItem(object HistoryItem) { throw new NotImplementedException(); }

        public virtual bool HasParentLocation() { return false; }
        public virtual void GoToParentLocation() { throw new NotImplementedException(); }

        public abstract string CurrentLocation { get; set; }

        public virtual bool CanRefresh() { return false; }
        public virtual void DoRefresh() { throw new NotImplementedException(); }

        public virtual bool HasQuickAccesItems() { return false; }
        public virtual object[] QuickAccessItems { get; }
        public virtual void GoToQuickAccesItem(object QuickAccessItem) { throw new NotImplementedException(); }

        public virtual bool CanSearchItems() { return false; }
        public virtual string CurrentSearchPattern { get; set; }
        public virtual void DoSearchItems(string SearchPattern) { throw new NotImplementedException(); }

        public abstract event EventHandler /* TODO: ElNavigableDockContent */ NavigationOptionsChanged;

        // TODO: 05/02/19 Search and Filter options will be combined!
        //public virtual ElNavOptionState CanApplyFilter() { return ElNavOptionState.Hidden; }
        //public virtual string CurrentFilterPattern { get; set; }
        //public virtual void DoApplyFilter(string FilterPattern) { throw new NotImplementedException(); }
    }
}
