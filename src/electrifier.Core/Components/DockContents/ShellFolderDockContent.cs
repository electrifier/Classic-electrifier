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

using electrifier.Core.Components.Controls;
using System.ComponentModel;
using System;
using Vanara.PInvoke;
using Vanara.Windows.Shell;
using WeifenLuo.WinFormsUI.Docking;
using Vanara.Windows.Forms;

namespace electrifier.Core.Components.DockContents
{
    [TypeDescriptionProvider(typeof(AbstractControlDescriptionProvider<ShellFolderDockContent, DockContent>))]
    public class ShellFolderDockContent
      : NavigableDockContent
    {
        private System.Windows.Forms.Splitter splitter;
        private ShellBrowser shellBrowser;
        private System.Windows.Forms.StatusStrip statusStrip;
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
            this.shellNamespaceTree.DisplayPinnedItemsOnly = true;
            //            var test = KnownFolder.Desktop;
            //            var test2 = new ShellFolder(Shell32.KNOWNFOLDERID.FOLDERID_Desktop);

            // TODO: Add QuickAccess, OneDrive, This PC, Network
            //            this.shellNamespaceTree.RootItems.Add(ShellFolder.Desktop, false, true);
            //this.shellNamespaceTree.RootItems.Add(new ShellFolder(KnownFolder.Desktop), false, false);
            //this.shellNamespaceTree.RootItems.Add(KnownFolder.SkyDrive, false, false);
            //            this.shellNamespaceTree.RootItems.Add(new ShellFolder(Shell32.KNOWNFOLDERID.FOLDERID_Desktop), false, false);
            //            this.shellNamespaceTree.RootItems.Add(new ShellFolder(Shell32.KNOWNFOLDERID.), false, false);
            this.shellNamespaceTree.RootItems.Add(new ShellFolder(@"shell:::{679f85cb-0220-4080-b29b-5540cc05aab6}"), false, false);        // This is the "Quick Access"-folder, which is new to Windows 10


            this.shellNamespaceTree.RootItems.Add(new ShellFolder(Shell32.KNOWNFOLDERID.FOLDERID_Desktop), false, false);
//            this.shellNamespaceTree.RootItems.Add(new ShellFolder(Shell32.KNOWNFOLDERID.FOLDERID_AppDataDesktop), false, false);
//            this.shellNamespaceTree.RootItems.Add(new ShellFolder(Shell32.KNOWNFOLDERID.FOLDERID_PublicDesktop), false, false);
            this.shellNamespaceTree.RootItems.Add(new ShellFolder(Shell32.KNOWNFOLDERID.FOLDERID_ThisPCDesktop), false, false);     // => Is mapped to "Snyc Centre"?
            this.shellNamespaceTree.RootItems.Add(new ShellFolder(Shell32.KNOWNFOLDERID.FOLDERID_OneDrive), false, false);
            this.shellNamespaceTree.RootItems.Add(new ShellFolder(Shell32.KNOWNFOLDERID.FOLDERID_ComputerFolder), false, false);
            this.shellNamespaceTree.RootItems.Add(new ShellFolder(Shell32.KNOWNFOLDERID.FOLDERID_NetworkFolder), false, false);



        }

        private void ShellNamespaceTree_AfterSelect(object sender, EventArgs e)
        {
            ShellItem shellItem = this.shellNamespaceTree.SelectedItem;

            if (null != shellItem)
            {
                if(!this.shellBrowser.CurrentFolder.PIDL.Equals(shellItem.PIDL))
                    this.shellBrowser.CurrentFolder = (ShellFolder)shellItem;

            }
        }



        /// <summary>
        /// TODO: This is very experimental, and only works if the new folder navigated to is an direct successor node
        ///       to the currently selected node.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShellBrowser_NavigationComplete(object sender, ShellBrowserNavigationCompleteEventArgs e)
        {
            ShellFolder newCurrentFolder = e.CurrentFolder;

            if (null != newCurrentFolder)
            {
                Shell32.PIDL newCurrentFolderPIDL = newCurrentFolder.PIDL;
                ShellItem selectedTreeItem = this.shellNamespaceTree.SelectedItem;

                if ((selectedTreeItem != null) && (selectedTreeItem.PIDL.IsParentOf(newCurrentFolderPIDL, immediate: true)))
                    this.shellNamespaceTree.SelectedItem = newCurrentFolder;

                // TODO: BUG: Navigation to user folder, e.g. "Thorsten Jung", will "magically add" that folder to the tree
                //else
                //    this.shellNamespaceTree.SelectedItem = null;
            }
        }



        #region Component Designer generated code =============================================================================

        private void InitializeComponent()
        {
            this.shellNamespaceTree = new Vanara.Windows.Forms.ShellNamespaceTreeControl();
            this.splitter = new System.Windows.Forms.Splitter();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.shellBrowser = new electrifier.Core.Components.Controls.ShellBrowser();
            this.SuspendLayout();
            // 
            // shellNamespaceTree
            // 
            this.shellNamespaceTree.Dock = System.Windows.Forms.DockStyle.Left;
            this.shellNamespaceTree.Location = new System.Drawing.Point(0, 0);
            this.shellNamespaceTree.Name = "shellNamespaceTree";
            this.shellNamespaceTree.Size = new System.Drawing.Size(320, 876);
            this.shellNamespaceTree.TabIndex = 1;
            this.shellNamespaceTree.AfterSelect += new System.EventHandler(this.ShellNamespaceTree_AfterSelect);
            // 
            // splitter
            // 
            this.splitter.Location = new System.Drawing.Point(320, 0);
            this.splitter.Name = "splitter";
            this.splitter.Size = new System.Drawing.Size(6, 876);
            this.splitter.TabIndex = 4;
            this.splitter.TabStop = false;
            // 
            // statusStrip
            // 
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip.Location = new System.Drawing.Point(326, 854);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(835, 22);
            this.statusStrip.SizingGrip = false;
            this.statusStrip.TabIndex = 6;
            this.statusStrip.Text = "statusStrip1";
            // 
            // shellBrowser
            // 
            this.shellBrowser.CurrentFolder = null;
            this.shellBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.shellBrowser.Location = new System.Drawing.Point(326, 0);
            this.shellBrowser.Name = "shellBrowser";
            this.shellBrowser.Size = new System.Drawing.Size(835, 876);
            this.shellBrowser.TabIndex = 5;
            this.shellBrowser.NavigationComplete += new System.EventHandler<electrifier.Core.Components.Controls.ShellBrowserNavigationCompleteEventArgs>(this.ShellBrowser_NavigationComplete);
            // 
            // ShellFolderDockContent
            // 
            this.ClientSize = new System.Drawing.Size(1161, 876);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.shellBrowser);
            this.Controls.Add(this.splitter);
            this.Controls.Add(this.shellNamespaceTree);
            this.Name = "ShellFolderDockContent";
            this.Load += new System.EventHandler(this.ShellFolderDockContent_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion ============================================================================================================
    }
}
