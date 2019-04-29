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
using System.Collections.Generic;
using System.Windows.Forms;

using Vanara.Windows.Shell;

namespace electrifier.Core.Components.DockContents
{
    /// <summary>
    /// Abstract class <see cref="ElNavigableDockContent"/> is the skeleton for navigation clients that
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

        public virtual bool CanHaveHistoryItems() => this.HistoryItems != default;
        public virtual ElNavigableTargetItemCollection<ElNavigableTargetNavigationLogIndex> HistoryItems { get; }
        ///   TODO: This will be replaced by SetCurrentLocation(HistoryIndex historyIndex);
        public virtual bool GoToHistoryItem(int historyItemIndex) { throw new NotImplementedException(); }

        public virtual bool HasParentLocation() { return false; }
        public virtual void GoToParentLocation() { throw new NotImplementedException(); }

        public abstract string CurrentLocation { get; set; }
        public virtual bool CanHaveRecentLocations() => this.RecentLocations != default;
        public virtual ElNavigableTargetItemCollection<ElNavigableTargetShellFolder> RecentLocations { get; }

        public virtual bool CanRefresh() { return false; }
        public virtual void DoRefresh() { throw new NotImplementedException(); }

        // TODO: QuickAccess => We want to have subfolders in this collection!
        public virtual bool CanHaveQuickAccesItems() => this.QuickAccessItems != default;
        public virtual ElNavigableTargetItemCollection<ElNavigableTargetShellFolder> QuickAccessItems { get; }

        public virtual bool CanSearchItems() { return false; }
        public virtual string CurrentSearchPattern { get; set; }
        public virtual void DoSearchItems(string SearchPattern) { throw new NotImplementedException(); }

        public abstract event EventHandler /* TODO: ElNavigableDockContent */ NavigationOptionsChanged;

        // TODO: 05/02/19 Search and Filter options will be combined!
        //public virtual ElNavOptionState CanApplyFilter() { return ElNavOptionState.Hidden; }
        //public virtual string CurrentFilterPattern { get; set; }
        //public virtual void DoApplyFilter(string FilterPattern) { throw new NotImplementedException(); }
        public virtual bool CanFilterItems() { return false; }


        public virtual void OnHistoryItemClick(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem tsMenuItem)
            {
                // This should be safe, cause "DropDownItems.IndexOf(null);" returns '-1'
                int historyItemIndex = tsMenuItem.Owner.Items.IndexOf(tsMenuItem);

                if (-1 != historyItemIndex)
                    this.GoToHistoryItem(historyItemIndex);
            }
        }

        public virtual void OnRecentLocationClick(object sender, EventArgs e)
        {
        }

        public virtual void OnQuickAccessItemClick(object sender, EventArgs e)
        {
        }
    }

    /// <summary>
    /// Represents a collection of <see cref="ElNavigableTargetItem"/>s, which is used by <see cref="ElNavigableDockContent"/>
    /// for <see cref="ElNavigableDockContent.HistoryItems"/>, <see cref="ElNavigableDockContent.RecentLocations"/> and
    /// <see cref="ElNavigableDockContent.QuickAccessItems"/>.
    /// </summary>
    public class ElNavigableTargetItemCollection<T>
      : IReadOnlyList<T>
        where T : ElNavigableTargetItem, new()
    {
        public ElNavigableDockContent Owner { get; private set; }

        private readonly List<T> items = new List<T>();

        public ElNavigableTargetItemCollection(ElNavigableDockContent owner)
          : base()
        {
            this.Owner = owner ??
                throw new ArgumentNullException("Instantiation of ElNavigableTargetItemCollection not allowed without given ElNavigableDockContent");
        }

        /// <summary>
        /// Create an <see cref="ElNavigableTargetItem"/> representing the given <see cref="ShellItem"/>
        /// to add it to this <see cref="ElNavigableTargetItemCollection{T}"/>.
        /// 
        /// In dependence of its type, this will create an <see cref="ElNavigableTargetShellFolder"/>
        /// or an <see cref=""/>
        /// </summary>
        /// <param name="shellItem">The <see cref="ShellItem"/> that should be added.</param>
        public void AddNewItem(ShellItem shellItem)
        {
            T newItem = new T
            {
                ShellItem = shellItem
            };

            this.items.Add(newItem);
        }

        public void Clear() => this.items.Clear();

        // TODO: Add link to ToolStripDropDownButton, i.e. NavigationToolStrip
        // TODO: 27.04. 08:32:: Als Eigenschaft deklarieren, beim Zuweisen DropDownItems bauen "LinkedToolStripDropDownButton"
        // => So kann auch "doppeltes" Einsetzen in zwei verschiedene ToolStripDropDownButtons verhindert werden ;)

        #region Implemented Interface: IReadOnlyList<ElNavigableTargetItem> ====================================================

        /// <summary>Gets the <see cref="ElNavigableTargetItem"/> at the specified index.</summary>
        /// <value>The <see cref="ElNavigableTargetItem"/>.</value>
        /// <param name="index">The zero-based index of the element to get.</param>
        public T this[int index] => this.items[index];

        /// <summary>Gets the number of elements in the collection.</summary>
        /// <value>Returns a <see cref="int"/> value.</value>
        public int Count => this.items.Count;

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<T> GetEnumerator() => this.items.GetEnumerator();

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        #endregion =============================================================================================================
    }

    /// <summary>
    /// Abstract class <see cref="ElNavigableTargetItem"/> is the skeleton for <see cref="ElNavigableTargetShellFolder"/> and
    /// <see cref="ElNavigableTargetNavigationLogIndex"/> items, which are combined in an
    /// <see cref="ElNavigableTargetItemCollection{T}"/>.
    /// </summary>
    public abstract class ElNavigableTargetItem
    {
        protected ShellItem shellItem = default;

        public ShellItem ShellItem {
            get => this.shellItem;

            protected internal set {
                if (this.shellItem != default)
                    throw new NotSupportedException("ElNavigableTargetItem.ShellItem value can only be assigned once and never ne overwritten!");

                this.shellItem = value;
            }
        }

        public string Name => this.ShellItem?.Name;
        //bool IsNavigable();       => IsNavigable should be true for directly navigable items, but false for sub-collections containing target items themselves
        //bool IsSubfolder();
    }

    public class ElNavigableTargetShellFolder
      : ElNavigableTargetItem
    {
        public ElNavigableTargetShellFolder() { }
    }

    public class ElNavigableTargetNavigationLogIndex
      : ElNavigableTargetItem
    {
        public ElNavigableTargetNavigationLogIndex() { }
    }
}
