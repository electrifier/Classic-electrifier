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

namespace electrifier.Core.Components.Controls
{
    public class NavigationToolStrip : System.Windows.Forms.ToolStrip
    {
        #region Fields ========================================================================================================

        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.ToolStripButton tbtNavigateBackward;
        private System.Windows.Forms.ToolStripButton tbtNavigateForward;
        private System.Windows.Forms.ToolStripDropDownButton tddNavigateRecentLocations;
        private System.Windows.Forms.ToolStripButton tbtNavigateParent;
        private Components.Controls.ToolStripSpringComboBox tcbNavigateFolder;
        private System.Windows.Forms.ToolStripButton tbtNavigateRefresh;
        private System.Windows.Forms.ToolStripDropDownButton tddNavigateQuickAccess;
        private System.Windows.Forms.ToolStripSeparator tssNavigateSeparator;
        private System.Windows.Forms.ToolStripComboBox tcbNavigateSearch;
        private System.Windows.Forms.ToolStripDropDownButton tddNavigateFilterItems;

        #endregion ============================================================================================================

        #region Published Events ==============================================================================================

        public event System.EventHandler NavigateBackwardClick {
            add { this.tbtNavigateBackward.Click += value; }
            remove { this.tbtNavigateBackward.Click -= value; }
        }

        public event System.EventHandler NavigateForwardClick {
            add { this.tbtNavigateForward.Click += value; }
            remove { this.tbtNavigateForward.Click -= value; }
        }

        public event System.EventHandler<NavigateRecentLocationsEventArgs> NavigateRecentLocationsClicked;

        public event System.EventHandler NavigateParentClick {
            add { this.tbtNavigateParent.Click += value; }
            remove { this.tbtNavigateParent.Click -= value; }
        }

        public event System.EventHandler NavigateRefreshClick {
            add { this.tbtNavigateRefresh.Click += value; }
            remove { this.tbtNavigateRefresh.Click -= value; }
        }

        public class NavigateRecentLocationsEventArgs : System.EventArgs
        {
            public int NavigationLogIndex { get; }

            public NavigateRecentLocationsEventArgs(int navigationLogIndex) { this.NavigationLogIndex = navigationLogIndex; }
        }

        #endregion Published Events ===========================================================================================

        public NavigationToolStrip()
        {
            this.InitializeComponent();

            this.tbtNavigateBackward.Enabled = false;
            this.tbtNavigateForward.Enabled = false;
            this.tbtNavigateParent.Enabled = false;         // TODO: Not implemented yet
            this.tcbNavigateFolder.Enabled = false;         // TODO: Not implemented yet
            this.tbtNavigateRefresh.Enabled = false;        // TODO: Not implemented yet
            this.tddNavigateQuickAccess.Enabled = false;    // TODO: Not implemented yet
            this.tcbNavigateSearch.Enabled = false;         // TODO: Not implemented yet
            this.tddNavigateFilterItems.Enabled = false;    // TODO: Not implemented yet

            this.tddNavigateRecentLocations.DropDownItemClicked += this.TddNavigateRecentLocations_DropDownItemClicked;

            // TODO: Associate ShellBrowserDockContent!!! 04.10.18, 01:11 => ActiveShellBrowser, events direkt hier abwickeln.
            // TODO: Associate ShellBrowserDockContent!!! 04.10.18, 01:11

        }

        private void TddNavigateRecentLocations_DropDownItemClicked(object sender, System.Windows.Forms.ToolStripItemClickedEventArgs e)
        {
            // This should be safe, cause "DropDownItems.IndexOf(null);" returns "-1"
            int index = this.tddNavigateRecentLocations.DropDownItems.IndexOf(e.ClickedItem);

            if (-1 != index)
            {
                this.NavigateRecentLocationsClicked?.Invoke(this, new NavigateRecentLocationsEventArgs(index));
            }
            else
            {
                AppContext.TraceWarning("IndexOf failed!");
            }
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        public void UpdateNavigationLog(Microsoft.WindowsAPICodePack.Controls.NavigationLogEventArgs navigationLogEventArgs,
            Microsoft.WindowsAPICodePack.Controls.ExplorerBrowserNavigationLog navigationLog)
        {
            if (navigationLogEventArgs.CanNavigateBackwardChanged)
            {
                this.tbtNavigateBackward.Enabled = navigationLog.CanNavigateBackward;
            }

            if (navigationLogEventArgs.CanNavigateForwardChanged)
            {
                this.tbtNavigateForward.Enabled = navigationLog.CanNavigateForward;
            }

            if (navigationLogEventArgs.LocationsChanged)
            {
                this.tddNavigateRecentLocations.DropDownItems.Clear();

                foreach (Microsoft.WindowsAPICodePack.Shell.ShellObject shObj in navigationLog.Locations)
                {
                    this.tddNavigateRecentLocations.DropDownItems.Add(shObj.Name);
                }
            }
        }



        #region Component Designer generated code =============================================================================

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            this.tbtNavigateBackward = new System.Windows.Forms.ToolStripButton();
            this.tbtNavigateForward = new System.Windows.Forms.ToolStripButton();
            this.tddNavigateRecentLocations = new System.Windows.Forms.ToolStripDropDownButton();
            this.tbtNavigateParent = new System.Windows.Forms.ToolStripButton();
            this.tcbNavigateFolder = new electrifier.Core.Components.Controls.ToolStripSpringComboBox();
            this.tbtNavigateRefresh = new System.Windows.Forms.ToolStripButton();
            this.tddNavigateQuickAccess = new System.Windows.Forms.ToolStripDropDownButton();
            this.tssNavigateSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.tcbNavigateSearch = new System.Windows.Forms.ToolStripComboBox();
            this.tddNavigateFilterItems = new System.Windows.Forms.ToolStripDropDownButton();

            this.SuspendLayout();

            this.Dock = System.Windows.Forms.DockStyle.Top;
            this.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbtNavigateBackward,
            this.tbtNavigateForward,
            this.tddNavigateRecentLocations,
            this.tbtNavigateParent,
            this.tcbNavigateFolder,
            this.tbtNavigateRefresh,
            this.tddNavigateQuickAccess,
            this.tssNavigateSeparator,
            this.tcbNavigateSearch,
            this.tddNavigateFilterItems});
            this.Padding = new System.Windows.Forms.Padding(8);
            this.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.Stretch = true;
            // 
            // tbtNavigateBackward
            // 
            this.tbtNavigateBackward.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbtNavigateBackward.Image = global::electrifier.Core.Properties.Resources.Navigation_Backward_24px;
            this.tbtNavigateBackward.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbtNavigateBackward.Name = "tbtNavigateBackward";
            this.tbtNavigateBackward.Size = new System.Drawing.Size(28, 28);
            this.tbtNavigateBackward.ToolTipText = "Back to [recent folder here...]  (Alt + Left Arrow)";
            // 
            // tbtNavigateForward
            // 
            this.tbtNavigateForward.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbtNavigateForward.Image = global::electrifier.Core.Properties.Resources.Navigation_Forward_24px;
            this.tbtNavigateForward.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbtNavigateForward.Name = "tbtNavigateForward";
            this.tbtNavigateForward.Size = new System.Drawing.Size(28, 28);
            this.tbtNavigateForward.ToolTipText = "Forward to [insert folder here] (Alt + Right Arrow)";
            // 
            // tddNavigateRecentLocations
            // 
            this.tddNavigateRecentLocations.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tddNavigateRecentLocations.Image = global::electrifier.Core.Properties.Resources.DropDown_Arrow_16px;
            this.tddNavigateRecentLocations.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tddNavigateRecentLocations.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tddNavigateRecentLocations.Name = "tddNavigateRecentLocations";
            this.tddNavigateRecentLocations.ShowDropDownArrow = false;
            this.tddNavigateRecentLocations.Size = new System.Drawing.Size(20, 28);
            this.tddNavigateRecentLocations.ToolTipText = "Recent locations";
            // 
            // tbtNavigateParent
            // 
            this.tbtNavigateParent.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbtNavigateParent.Image = global::electrifier.Core.Properties.Resources.Navigation_Parent_Folder_24px;
            this.tbtNavigateParent.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbtNavigateParent.Name = "tbtNavigateParent";
            this.tbtNavigateParent.Size = new System.Drawing.Size(28, 28);
            this.tbtNavigateParent.ToolTipText = "Up to [Insert folder here]... (Alt + Up Arrow)";
            // 
            // tcbNavigateFolder
            // 
            this.tcbNavigateFolder.Name = "tcbNavigateFolder";
            // 
            // tbtNavigateRefresh
            // 
            this.tbtNavigateRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbtNavigateRefresh.Image = global::electrifier.Core.Properties.Resources.Navigation_Refresh_24px;
            this.tbtNavigateRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbtNavigateRefresh.Name = "tbtNavigateRefresh";
            this.tbtNavigateRefresh.Size = new System.Drawing.Size(28, 28);
            this.tbtNavigateRefresh.ToolTipText = "Refresh [Insert folder here]";
            // 
            // tddNavigateQuickAccess
            // 
            this.tddNavigateQuickAccess.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tddNavigateQuickAccess.Image = global::electrifier.Core.Properties.Resources.Navigation_Quick_Access_24px;
            this.tddNavigateQuickAccess.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tddNavigateQuickAccess.Name = "tddNavigateQuickAccess";
            this.tddNavigateQuickAccess.Size = new System.Drawing.Size(38, 28);
            this.tddNavigateQuickAccess.Text = "tddNavigateQuickAccess";
            // 
            // tssNavigateSeparator
            // 
            this.tssNavigateSeparator.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.tssNavigateSeparator.Name = "tssNavigateSeparator";
            this.tssNavigateSeparator.Size = new System.Drawing.Size(6, 31);
            // 
            // tcbNavigateSearch
            // 
            this.tcbNavigateSearch.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tcbNavigateSearch.ForeColor = System.Drawing.SystemColors.GrayText;
            this.tcbNavigateSearch.Name = "tcbNavigateSearch";
            this.tcbNavigateSearch.Size = new System.Drawing.Size(250, 31);
            this.tcbNavigateSearch.Text = "Enter search phrase...";
            // 
            // tddNavigateFilterItems
            // 
            this.tddNavigateFilterItems.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tddNavigateFilterItems.Image = global::electrifier.Core.Properties.Resources.Navigation_Filter_Add_24px;
            this.tddNavigateFilterItems.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tddNavigateFilterItems.Name = "tddNavigateFilterItems";
            this.tddNavigateFilterItems.Size = new System.Drawing.Size(38, 28);

            this.ResumeLayout(false);
        }

        #endregion ============================================================================================================

    }
}
