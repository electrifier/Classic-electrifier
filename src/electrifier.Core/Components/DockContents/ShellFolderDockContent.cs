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
using Vanara.Windows.Shell;

using electrifier.Core.Components.Controls;
using WeifenLuo.WinFormsUI.Docking;
using System.ComponentModel;

namespace electrifier.Core.Components.DockContents
{
    [TypeDescriptionProvider(typeof(AbstractControlDescriptionProvider<ShellFolderDockContent, DockContent>))]
    public class ShellFolderDockContent
      : NavigableDockContent
    {
        private ShellBrowser shellBrowser;
        private Vanara.Windows.Forms.ShellNamespaceTreeControl shellNamespaceTree;

        public override string CurrentLocation { get => "TEST"; set => this.shellNamespaceTree.Text = value; }

        public override event EventHandler NavigationOptionsChanged;

        public ShellFolderDockContent(IElNavigationHost navigationHost, string persistString = null)
          : base(navigationHost)
        {
            this.InitializeComponent();


        }

        private void ShellFolderDockContent_Load(object sender, EventArgs e)
        {
            this.shellNamespaceTree.RootItems.Add(ShellFolder.Desktop, false, true);
        }

        #region Component Designer generated code =============================================================================

        private void InitializeComponent()
        {
            this.shellNamespaceTree = new Vanara.Windows.Forms.ShellNamespaceTreeControl();
            this.shellBrowser = new electrifier.Core.Components.Controls.ShellBrowser();
            this.SuspendLayout();
            // 
            // shellNamespaceTree
            // 
            this.shellNamespaceTree.Location = new System.Drawing.Point(0, 0);
            this.shellNamespaceTree.Name = "shellNamespaceTree";
            this.shellNamespaceTree.Size = new System.Drawing.Size(320, 864);
            this.shellNamespaceTree.TabIndex = 1;
            // 
            // shellBrowser
            // 
            this.shellBrowser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.shellBrowser.Location = new System.Drawing.Point(336, 12);
            this.shellBrowser.Name = "shellBrowser";
            this.shellBrowser.Size = new System.Drawing.Size(813, 852);
            this.shellBrowser.TabIndex = 2;
            // 
            // ShellFolderDockContent
            // 
            this.ClientSize = new System.Drawing.Size(1161, 876);
            this.Controls.Add(this.shellBrowser);
            this.Controls.Add(this.shellNamespaceTree);
            this.Name = "ShellFolderDockContent";
            this.Load += new System.EventHandler(this.ShellFolderDockContent_Load);
            this.ResumeLayout(false);

        }

        #endregion ============================================================================================================

    }
}
