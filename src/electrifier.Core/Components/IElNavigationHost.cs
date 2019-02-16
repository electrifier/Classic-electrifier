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

using electrifier.Core.Components.DockContents;

namespace electrifier.Core.Components
{
    /// <summary>
    /// The interface hosts of <see cref="ElNavigableDockContent"/>'s have to provide to communicate.
    /// 
    /// Currently only <see cref="Forms.Electrifier"/> implements this interface
    /// </summary>
    public interface IElNavigationHost
    {
        void AddDockContent(ElNavigableDockContent DockContent);
        void ActivateDockContent(ElNavigableDockContent DockContent);
        ElNavigableDockContent GetActiveDockContent();
        void RemoveDockContent(ElNavigableDockContent DockContent);

        // void DeactivateClient();

        // Sender => Only update ViewOptions when Sender == ActiveClient
        // event EventHandler ClientActivated;

        // event EventHandler CurrentFolderUpdated;
        // void SetCurrentFolder(IElNavigationClient client);
        // event EventHandler FavoritesUpdated;
        // void SetFavorites(IElNavigationClient client, object[] Favorites);
        // event EventHandler NavigationOptionsUpdated;
        // void OnNavigationOptionsChanged(IElNavigationClient client);
    }
}
