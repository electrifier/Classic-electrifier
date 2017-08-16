/*
** 
**  electrifier
** 
**  Copyright 2017 Thorsten Jung, www.electrifier.org
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
using System.Windows.Forms;

using Microsoft.WindowsAPICodePack.Controls.WindowsForms;
using Microsoft.WindowsAPICodePack.Shell;

namespace electrifier.Core.Controls.DockContents
{
    /// <summary>
    /// Summary of ShellBrowserDockContent.
    /// </summary>
    public class ShellBrowserDockContent
        : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        protected Guid Guid;
        protected ExplorerBrowser explorerBrowser;

        public ShellBrowserDockContent() : this(Guid.NewGuid()) { }

        public ShellBrowserDockContent(Guid guid) : base()
        {
            // Initialize the underlying DockControl
            this.Guid = guid;
            this.Name = "ShellBrowserDockContent." + this.Guid.ToString();
            this.Text = "ExplorerBrowser";
            this.explorerBrowser = new ExplorerBrowser()
            {
                Dock = DockStyle.Fill
            };

            this.explorerBrowser.Navigate(KnownFolders.Documents as ShellObject);
            this.Controls.Add(this.explorerBrowser);
        }
    }
}
