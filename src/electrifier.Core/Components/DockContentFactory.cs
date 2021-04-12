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
using System;
using WeifenLuo.WinFormsUI.Docking;

namespace electrifier.Core.Components
{
    internal static class DockContentFactory
    {
        /// <summary>
        /// Deserialize given persistString as used in XML-files to create the corresponding DockContent instance.
        /// </summary>
        /// <param name="navigationHost"><see cref="INavigationHost"/> instance the new DockContent will be added to.</param>
        /// <param name="persistString">The string stored in XML-Files with DockContent type and parameters,
        ///     e.g "ElShellBrowserDockContent URI=file:///S:/%5BGit.Workspace%5D/electrifier"</param>
        /// <returns>The valid IDockContent instance, or NULL if <see cref="persistString"/> is invalid.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Unknown DockContent type.</exception>
        public static IDockContent Deserialize(INavigationHost navigationHost, string persistString)
        {
            IDockContent dockContent = default;
            var typeNameSeperatorPos = persistString.IndexOf(" ");
            string dockContentTypeName, dockContentArguments = default;

            if (typeNameSeperatorPos < 0)
                dockContentTypeName = persistString;
            else
            {
                dockContentTypeName = persistString.Substring(0, typeNameSeperatorPos);
                dockContentArguments = persistString.Substring(typeNameSeperatorPos);
            }

            switch (dockContentTypeName)
            {
                case nameof(ShellFolderDockContent):
                    dockContent = DockContentFactory.CreateShellBrowser(navigationHost, dockContentArguments);
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"Unknown DockContent of type { dockContentTypeName }");
            }

            return dockContent;
        }

        public static ShellFolderDockContent CreateShellBrowser(INavigationHost navigationHost, string persistString = null)
        {
            // ElNavigableDockContent constructor will check for navigationHost null values.
            ShellFolderDockContent shellBrowser = new ShellFolderDockContent(navigationHost, persistString);

            navigationHost.AddDockContent(shellBrowser);

            return shellBrowser;
        }
    }
}
