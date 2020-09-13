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
        private System.Windows.Forms.Splitter splitter1;
        private ShellBrowser shellBrowser1;
        private System.Windows.Forms.StatusStrip statusStrip1;
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
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.shellBrowser1 = new electrifier.Core.Components.Controls.ShellBrowser();
            this.SuspendLayout();
            // 
            // shellNamespaceTree
            // 
            this.shellNamespaceTree.Dock = System.Windows.Forms.DockStyle.Left;
            this.shellNamespaceTree.Location = new System.Drawing.Point(0, 0);
            this.shellNamespaceTree.Name = "shellNamespaceTree";
            this.shellNamespaceTree.Size = new System.Drawing.Size(320, 876);
            this.shellNamespaceTree.TabIndex = 1;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(320, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(6, 876);
            this.splitter1.TabIndex = 4;
            this.splitter1.TabStop = false;
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Location = new System.Drawing.Point(326, 854);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(835, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 6;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // shellBrowser1
            // 
            this.shellBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.shellBrowser1.Location = new System.Drawing.Point(326, 0);
            this.shellBrowser1.Name = "shellBrowser1";
            this.shellBrowser1.Size = new System.Drawing.Size(835, 876);
            this.shellBrowser1.TabIndex = 5;
            // 
            // ShellFolderDockContent
            // 
            this.ClientSize = new System.Drawing.Size(1161, 876);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.shellBrowser1);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.shellNamespaceTree);
            this.Name = "ShellFolderDockContent";
            this.Load += new System.EventHandler(this.ShellFolderDockContent_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion ============================================================================================================

    }
}
