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

using electrifier.Core.Components.DockContents;
using WeifenLuo.WinFormsUI.Docking;

namespace electrifier.Core.Components
{
    /// <summary>
    /// The interface hosts of <see cref="NavigableDockContent"/>'s have to provide to communicate.
    /// 
    /// Currently only <see cref="Forms.ElApplicationWindow"/> implements this interface
    /// </summary>
    public interface INavigationHost
    {
        DockContent ActiveDockContent { get; }

//        void AddDockContent(NavigableDockContent DockContent);
        void AddDockContent(DockContent DockContent, DockState dockState);

//        void ActivateDockContent(NavigableDockContent DockContent);
//        void RemoveDockContent(NavigableDockContent DockContent);
        void RemoveDockContent(DockContent DockContent);

        // void DeactivateClient();

        // Sender => Only update ViewOptions when Sender == ActiveClient
        // event EventHandler ClientActivated;

        // event EventHandler CurrentFolderUpdated;
        // void SetCurrentFolder(INavigationClient client);
        // event EventHandler FavoritesUpdated;
        // void SetFavorites(INavigationClient client, object[] Favorites);
        // event EventHandler NavigationOptionsUpdated;
        // void OnNavigationOptionsChanged(INavigationClient client);
    }
}
