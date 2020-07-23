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
using System.Collections.Generic;

using electrifier.Core.Components.Controls.Extensions;
using electrifier.Core.Components.DockContents;
using electrifier.Core.Components.DockContents.Extensions;

namespace electrifier.Core.Components.Controls
{
    public class ElNavigationToolStrip
        : System.Windows.Forms.ToolStrip
        , IElThemedControl
    {
        #region Fields =========================================================================================================

        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.ToolStripButton btnGoBack;
        private System.Windows.Forms.ToolStripButton btnGoForward;
        private System.Windows.Forms.ToolStripDropDownButton ddnHistoryItems;
        private System.Windows.Forms.ToolStripButton btnGoToParentLocation;
        private ElToolStripSpringComboBox cbbCurrentFolder;
        private System.Windows.Forms.ToolStripButton btnRefresh;
        private System.Windows.Forms.ToolStripDropDownButton ddnQuickAccessItems;
        private System.Windows.Forms.ToolStripSeparator ssoSeparator;
        private System.Windows.Forms.ToolStripComboBox cbbSearchPattern;
        private System.Windows.Forms.ToolStripDropDownButton cbbFilterPattern;

        private enum ImageListIndex
        {
            btnGoBack = 0,
            btnGoForward,
            btnGoToParentLocation,
            btnRefresh,
            ddnQuickAccessItems,
            cbbFilterPattern_Add,
            cbbFilterPattern_Remove,
        }

        #endregion =============================================================================================================

        #region Properties =====================================================================================================

        private ElNavigableDockContent activeDockContent = null;

        public ElNavigableDockContent ActiveDockContent {
            get => this.activeDockContent;
            set => this.UpdateButtonState(this.activeDockContent = value);
        }

        #endregion =============================================================================================================

        #region Implemented Interface: IElThemedControl ========================================================================

        // Remember the theme resource name is build internally, so don't add its ".png"-file extension here :)
        public string DefaultTheme => "iTweek by Miles Ponson (32px)";
        //public string DefaultTheme => "Microsoft Windows (24px)";

        public string ThemeResourceNamespace { get => this.GetThemeResourceNamespace(); }

        public IEnumerable<string> GetAvailableThemes() { return this.EnumerateAvailableThemes(); }

        private string currentTheme = null;
        public string CurrentTheme {
            get => this.currentTheme;
            set {
                this.ImageList = this.LoadThemeImageListFromResource(this.currentTheme = value);
                this.ImageScalingSize = this.ImageList.ImageSize;
            }
        }

        #endregion =============================================================================================================

        public ElNavigationToolStrip()
        {
            this.InitializeComponent();
            this.UpdateButtonState(null);

            this.btnGoBack.Click += this.BtnGoBack_Click;
            this.btnGoForward.Click += this.BtnGoForward_Click;

            // Set current theme to default theme to get the ImageList populated
            this.CurrentTheme = this.DefaultTheme;
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

        /// <summary>
        /// Call GoBack() on ActiveDockContent.
        /// </summary>
        /// <param name="sender">Not used.</param>
        /// <param name="e">Not used.</param>
        private void BtnGoBack_Click(object sender, EventArgs e)
        {
            if (this.ActiveDockContent is null)
                AppContext.TraceWarning("ActiveDockContent is null in BtnGoBack_Click()");

            this.ActiveDockContent?.GoBack();
        }

        /// <summary>
        /// Call GoForward() on ActiveDockContent.
        /// </summary>
        /// <param name="sender">Not used.</param>
        /// <param name="e">Not used.</param>
        private void BtnGoForward_Click(object sender, EventArgs e)
        {
            if (this.ActiveDockContent is null)
                AppContext.TraceWarning("ActiveDockContent is null in BtnGoForward_Click()");

            this.ActiveDockContent?.GoForward();
        }

        // TODO: Replace this whole procedure through appropriate event handlers!!!!! 28.04.19
        public void UpdateButtonState(ElNavigableDockContent navigableDockContent)
        {
            this.SuspendLayout();

            try
            {
                // TODO: The following exception is thrown for test purposes only!
                if (this.ActiveDockContent != navigableDockContent)
                    throw new ArgumentException("ElNavigationToolStrip.UpdateButtonState: navigableDockContent does not match ActiveDockContent");

                if (null != navigableDockContent)
                {
                    this.btnGoBack.Enabled = navigableDockContent.CanGoBack;
                    this.btnGoForward.Enabled = navigableDockContent.CanGoForward;
                    this.ddnHistoryItems.Enabled = navigableDockContent.CanHaveHistoryItems;
                    this.ddnHistoryItems.DropDownItems.Rebuild(navigableDockContent.HistoryItems, navigableDockContent.OnHistoryItemClick);      // TODO: Only do this when the item collection has changed!
                    this.btnGoToParentLocation.Enabled = navigableDockContent.HasParentLocation;
                    this.cbbCurrentFolder.Enabled = true;
                    this.cbbCurrentFolder.Text = navigableDockContent.CurrentLocation;
                    this.btnRefresh.Enabled = navigableDockContent.CanRefresh;
                    this.ddnQuickAccessItems.Enabled = navigableDockContent.CanHaveQuickAccesItems;
                    this.cbbSearchPattern.Enabled = navigableDockContent.CanSearchItems;
                    this.cbbFilterPattern.Enabled = navigableDockContent.CanFilterItems;
                }
                else
                {
                    this.btnGoBack.Enabled = false;
                    this.btnGoForward.Enabled = false;
                    this.ddnHistoryItems.Enabled = false;
                    this.ddnHistoryItems.DropDownItems.Clear();
                    this.btnGoToParentLocation.Enabled = false;
                    this.cbbCurrentFolder.Enabled = false;
                    this.cbbCurrentFolder.Text = "";
                    this.btnRefresh.Enabled = false;
                    this.ddnQuickAccessItems.Enabled = false;
                    this.cbbSearchPattern.Enabled = false;
                    this.cbbFilterPattern.Enabled = false;
                }
            }
            finally
            {
                this.ResumeLayout();
            }
        }




        #region Component Designer generated code ==============================================================================

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            this.btnGoBack = new System.Windows.Forms.ToolStripButton();
            this.btnGoForward = new System.Windows.Forms.ToolStripButton();
            this.ddnHistoryItems = new System.Windows.Forms.ToolStripDropDownButton();
            this.btnGoToParentLocation = new System.Windows.Forms.ToolStripButton();
            this.cbbCurrentFolder = new electrifier.Core.Components.Controls.ElToolStripSpringComboBox();
            this.btnRefresh = new System.Windows.Forms.ToolStripButton();
            this.ddnQuickAccessItems = new System.Windows.Forms.ToolStripDropDownButton();
            this.ssoSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.cbbSearchPattern = new System.Windows.Forms.ToolStripComboBox();
            this.cbbFilterPattern = new System.Windows.Forms.ToolStripDropDownButton();

            this.SuspendLayout();

            this.Dock = System.Windows.Forms.DockStyle.Top;
            this.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.btnGoBack,
                this.btnGoForward,
                this.ddnHistoryItems,
                this.btnGoToParentLocation,
                this.cbbCurrentFolder,
                this.btnRefresh,
                this.ddnQuickAccessItems,
                this.ssoSeparator,
                this.cbbSearchPattern,
                this.cbbFilterPattern });
            this.Padding = new System.Windows.Forms.Padding(8);
            this.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.Stretch = true;
            // 
            // btnGoBack
            // 
            this.btnGoBack.Name = "btnGoBack";
            this.btnGoBack.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnGoBack.ImageIndex = (int)ImageListIndex.btnGoBack;
            this.btnGoBack.ToolTipText = "Back to [recent folder here...]  (Alt + Left Arrow)";
            // 
            // btnGoForward
            // 
            this.btnGoForward.Name = "btnGoForward";
            this.btnGoForward.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnGoForward.ImageIndex = (int)ImageListIndex.btnGoForward;
            this.btnGoForward.ToolTipText = "Forward to [insert folder here] (Alt + Right Arrow)";
            // 
            // ddnHistoryItems
            // 
            this.ddnHistoryItems.Name = "ddnHistoryItems";
            this.ddnHistoryItems.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ddnHistoryItems.Image = global::electrifier.Core.Properties.Resources.DropDown_Arrow_16px;
            this.ddnHistoryItems.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ddnHistoryItems.ShowDropDownArrow = false;
            this.ddnHistoryItems.ToolTipText = "Recent locations";
            // 
            // btnGoToParentLocation
            // 
            this.btnGoToParentLocation.Name = "btnGoToParentLocation";
            this.btnGoToParentLocation.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnGoToParentLocation.ImageIndex = (int)ImageListIndex.btnGoToParentLocation;
            this.btnGoToParentLocation.ToolTipText = "Up to [Insert folder here]... (Alt + Up Arrow)";
            // 
            // cbbCurrentFolder
            // 
            this.cbbCurrentFolder.Name = "cbbCurrentFolder";
            // 
            // btnRefresh
            // 
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnRefresh.ImageIndex = (int)ImageListIndex.btnRefresh;
            this.btnRefresh.ToolTipText = "Refresh current folder view";
            // 
            // ddnQuickAccessItems
            // 
            this.ddnQuickAccessItems.Name = "ddnQuickAccessItems";
            this.ddnQuickAccessItems.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ddnQuickAccessItems.ImageIndex = (int)ImageListIndex.ddnQuickAccessItems;
            // 
            // ssoSeparator
            // 
            this.ssoSeparator.Name = "ssoSeparator";
            this.ssoSeparator.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            // 
            // cbbSearchFilter
            // 
            this.cbbSearchPattern.Name = "cbbSearchFilter";
            this.cbbSearchPattern.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbbSearchPattern.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cbbSearchPattern.Size = new System.Drawing.Size(250, 31);
            this.cbbSearchPattern.Text = "Enter search phrase...";
            // 
            // cbbFilterPattern
            // 
            this.cbbFilterPattern.Name = "cbbFilterPattern";
            this.cbbFilterPattern.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.cbbFilterPattern.ImageIndex = (int)ImageListIndex.cbbFilterPattern_Add;

            this.ResumeLayout(false);
        }

        #endregion =============================================================================================================

    }
}
