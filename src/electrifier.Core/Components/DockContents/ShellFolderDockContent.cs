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
        private Vanara.Windows.Forms.ShellNamespaceTreeControl shellNamespaceTree;
        private System.Windows.Forms.Splitter splitter;
        private System.Windows.Forms.StatusStrip statusStrip;

        public ShellBrowser ShellBrowser { private set; get; }

        public override string CurrentLocation { get => "TEST"; set => this.shellNamespaceTree.Text = value; }




        public ShellFolderDockContent(INavigationHost navigationHost, string persistString = null)
          : base(navigationHost)
        {
            this.InitializeComponent();
        }

        private void ShellFolderDockContent_Load(object sender, EventArgs e)
        {
            this.Icon = Properties.Resources.ShellBrowserDockContent;

            this.shellNamespaceTree.RootItems.Add(new ShellFolder(@"shell:::{679f85cb-0220-4080-b29b-5540cc05aab6}"), false, false);        // TOOD: This is the "Quick Access"-folder, which is new to Windows 10
            this.shellNamespaceTree.RootItems.Add(new ShellFolder(Shell32.KNOWNFOLDERID.FOLDERID_OneDrive), false, false);
            this.shellNamespaceTree.RootItems.Add(new ShellFolder(Shell32.KNOWNFOLDERID.FOLDERID_ComputerFolder), false, false);
            this.shellNamespaceTree.RootItems.Add(new ShellFolder(Shell32.KNOWNFOLDERID.FOLDERID_NetworkFolder), false, false);

            this.shellNamespaceTree.SelectedItem = this.shellNamespaceTree.RootItems[0];

            this.ShellBrowser.NavigationComplete += this.ShellBrowser_NavigationComplete;
            this.ShellBrowser.ItemsChanged += this.ShellBrowser_ItemsChanged;
            this.ShellBrowser.SelectionChanged += this.ShellBrowser_SelectionChanged;
        }

        private void ShellBrowser_SelectionChanged(object sender, EventArgs e)
        {
            AppContext.TraceScope();

            if (sender != this.ShellBrowser)
                AppContext.TraceWarning("sender is not my ShellBrowser!");
        }

        private void ShellBrowser_ItemsChanged(object sender, EventArgs e)
        {
            AppContext.TraceScope();

            if (sender != this.ShellBrowser)
                AppContext.TraceWarning("sender is not my ShellBrowser!");
        }

        private void ShellNamespaceTree_AfterSelect(object sender, EventArgs e)
        {
            ShellItem shellItem = this.shellNamespaceTree.SelectedItem;

            if (null == shellItem)
                throw new NullReferenceException(nameof(shellItem));


            // TODO: 18/01/21 Remove CurrentFolder in ShellBrowser
            //this.ShellBrowser.CurrentFolder = (ShellFolder)shellItem;
            this.ShellBrowser.BrowseObject((IntPtr)shellItem.PIDL, Shell32.SBSP.SBSP_ABSOLUTE);

            //if (!shellItem.PIDL.Equals(this.shellBrowser.CurrentFolder))
            //{
            //    this.shellBrowser.CurrentFolder = (ShellFolder)shellItem;
            //}


            //if (null != shellItem)
            //{
            //    if (null == this.shellBrowser.CurrentFolder || !this.shellBrowser.CurrentFolder.PIDL.Equals(shellItem.PIDL))
            //        this.shellBrowser.CurrentFolder = (ShellFolder)shellItem;
            //}
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

                this.Text = newCurrentFolder.GetDisplayName(ShellItemDisplayString.ParentRelativeForUI);
            }

            this.OnNavigationOptionsChanged(EventArgs.Empty);
        }


        public override bool CanGoBack => this.ShellBrowser.History.CanSeekBackward;

        public override void GoBack()
        {
            this.ShellBrowser.NavigateBack();
            this.OnNavigationOptionsChanged(EventArgs.Empty);
        }

        public override bool CanGoForward => this.ShellBrowser.History.CanSeekForward;

        public override void GoForward()
        {
            this.ShellBrowser.NavigateForward();
            this.OnNavigationOptionsChanged(EventArgs.Empty);
        }



        public override ElNavigableTargetItemCollection<ElNavigableTargetNavigationLogIndex> HistoryItems { get; }

        public override event EventHandler NavigationOptionsChanged;

        public virtual void OnNavigationOptionsChanged(EventArgs args)
        {
            this.NavigationOptionsChanged?.Invoke(this, args);
        }


        #region Component Designer generated code =============================================================================

        private void InitializeComponent()
        {
            this.shellNamespaceTree = new Vanara.Windows.Forms.ShellNamespaceTreeControl();
            this.splitter = new System.Windows.Forms.Splitter();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.ShellBrowser = new electrifier.Core.Components.Controls.ShellBrowser();
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
            this.ShellBrowser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ShellBrowser.Location = new System.Drawing.Point(326, 0);
            this.ShellBrowser.Name = "shellBrowser";
            this.ShellBrowser.Size = new System.Drawing.Size(835, 851);
            this.ShellBrowser.TabIndex = 5;
            this.ShellBrowser.NavigationComplete += new System.EventHandler<electrifier.Core.Components.Controls.ShellBrowserNavigationCompleteEventArgs>(this.ShellBrowser_NavigationComplete);
            // 
            // ShellFolderDockContent
            // 
            this.ClientSize = new System.Drawing.Size(1161, 876);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.ShellBrowser);
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
