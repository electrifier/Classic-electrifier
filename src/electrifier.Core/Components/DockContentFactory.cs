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
        private static SelectConditionalBox selectConditionalBox;
        private static DockPanel SelectConditionalDockPanel;
        private static DockState SelectConditionalDockState;


        /// <summary>
        /// Get the instance of the currently opened <see cref="DockContents.SelectConditionalBox"/>, if there is one.
        /// </summary>
        public static SelectConditionalBox SelectConditionalBox
        {
            get => selectConditionalBox; // ?? (selectConditionalBox = new SelectConditionalBox());
            private set => selectConditionalBox = value;
        }


        //public static SelectConditionalBox CreateSelectConditionalBox() => SelectConditionalBox ?? (SelectConditionalBox = new SelectConditionalBox());


        /// <summary>
        /// Deserialize given persistString as used in XML-files to create the corresponding DockContent instance.
        /// </summary>
        /// <param name="navigationHost"><see cref="INavigationHost"/> instance the new DockContent will be added to.</param>
        /// <param name="persistString">The string stored in XML-Files with DockContent type and parameters,
        ///     e.g "ElShellBrowserDockContent URI=file:///S:/%5BGit.Workspace%5D/electrifier"</param>
        /// <returns>The valid IDockContent instance, or NULL if <see cref="persistString"/> is invalid.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Unknown DockContent type.</exception>
        public static IDockContent Deserialize(INavigationHost navigationHost, string persistString, DockPanel dockPanel)
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

            /* TODO: Remove namespace, but why has SelectConditionalBox one, whereas ShellFolderDockContent has not? */
            if ((typeNameSeperatorPos = dockContentTypeName.LastIndexOf(".")) > 0)
                dockContentTypeName = persistString.Substring(typeNameSeperatorPos + 1);

            switch (dockContentTypeName)
            {
                case nameof(ShellFolderDockContent):
                    dockContent = CreateShellBrowser(navigationHost, dockContentArguments);
                    break;
                case nameof(SelectConditionalBox):
                    dockContent = CreateSelectConditionalBox(dockPanel);
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

            //navigationHost.AddDockContent(shellBrowser);
            navigationHost.AddDockContent(shellBrowser);

            return shellBrowser;
        }

        public static SelectConditionalBox CreateSelectConditionalBox(DockPanel dockPanel)
        {
            if (SelectConditionalBox != null)
                throw new InvalidOperationException("An instance of SelectConditionalBox has already been created");

            SelectConditionalBox = new SelectConditionalBox();

            if (SelectConditionalDockPanel != null)
                SelectConditionalBox.Show(SelectConditionalDockPanel, SelectConditionalDockState);
            else
            {
                SelectConditionalBox.Show(dockPanel, dockState: DockState.DockRight);
            }

            return SelectConditionalBox;
        }

        public static void CloseSelectConditionalBox()
        {
            var dockPanel = SelectConditionalBox;

            // TODO: Test pruposes only... Just return
            if (dockPanel == null)
                throw new InvalidOperationException("No instance of SelectConditionalBox has already been created");

            SelectConditionalDockPanel = SelectConditionalBox.DockPanel;

            SelectConditionalBox = null;
            dockPanel.Close();
        }
    }
}
