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

namespace electrifier.Core.Forms
{
    partial class ElApplicationWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.stsStatusStrip = new System.Windows.Forms.StatusStrip();
            this.tslItemCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.tslSelectionCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.tscToolStripContainer = new System.Windows.Forms.ToolStripContainer();
            this.dpnDockPanel = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this.ntsNavigation = new electrifier.Core.Components.Controls.ElNavigationToolStrip();
            this.rbnRibbon = new RibbonLib.Ribbon();
            this.fspFormStatePersistor = new electrifier.Core.Components.ElFormStatePersistor(this.components);
            this.stsStatusStrip.SuspendLayout();
            this.tscToolStripContainer.ContentPanel.SuspendLayout();
            this.tscToolStripContainer.TopToolStripPanel.SuspendLayout();
            this.tscToolStripContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // stsStatusStrip
            // 
            this.stsStatusStrip.AutoSize = false;
            this.stsStatusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.stsStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tslItemCount,
            this.tslSelectionCount});
            this.stsStatusStrip.Location = new System.Drawing.Point(0, 528);
            this.stsStatusStrip.Name = "stsStatusStrip";
            this.stsStatusStrip.Size = new System.Drawing.Size(782, 25);
            this.stsStatusStrip.TabIndex = 0;
            // 
            // tslItemCount
            // 
            this.tslItemCount.AutoSize = false;
            this.tslItemCount.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.tslItemCount.Name = "tslItemCount";
            this.tslItemCount.Size = new System.Drawing.Size(96, 19);
            // 
            // tslSelectionCount
            // 
            this.tslSelectionCount.AutoSize = false;
            this.tslSelectionCount.Name = "tslSelectionCount";
            this.tslSelectionCount.Size = new System.Drawing.Size(120, 19);
            // 
            // tscToolStripContainer
            // 
            // 
            // tscToolStripContainer.ContentPanel
            // 
            this.tscToolStripContainer.ContentPanel.AutoScroll = true;
            this.tscToolStripContainer.ContentPanel.Controls.Add(this.dpnDockPanel);
            this.tscToolStripContainer.ContentPanel.Size = new System.Drawing.Size(782, 351);
            this.tscToolStripContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tscToolStripContainer.Location = new System.Drawing.Point(0, 122);
            this.tscToolStripContainer.Name = "tscToolStripContainer";
            this.tscToolStripContainer.Size = new System.Drawing.Size(782, 406);
            this.tscToolStripContainer.TabIndex = 5;
            this.tscToolStripContainer.Text = "toolStripContainer1";
            // 
            // tscToolStripContainer.TopToolStripPanel
            // 
            this.tscToolStripContainer.TopToolStripPanel.Controls.Add(this.ntsNavigation);
            // 
            // dpnDockPanel
            // 
            this.dpnDockPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dpnDockPanel.DocumentStyle = WeifenLuo.WinFormsUI.Docking.DocumentStyle.DockingWindow;
            this.dpnDockPanel.Location = new System.Drawing.Point(0, 0);
            this.dpnDockPanel.Name = "dpnDockPanel";
            this.dpnDockPanel.Size = new System.Drawing.Size(782, 351);
            this.dpnDockPanel.TabIndex = 5;
            // 
            // ntsNavigation
            // 
            this.ntsNavigation.ActiveDockContent = null;
            this.ntsNavigation.CurrentTheme = "iTweek by Miles Ponson (32px)";
            this.ntsNavigation.Dock = System.Windows.Forms.DockStyle.None;
            this.ntsNavigation.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.ntsNavigation.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.ntsNavigation.Location = new System.Drawing.Point(0, 0);
            this.ntsNavigation.Name = "ntsNavigation";
            this.ntsNavigation.Padding = new System.Windows.Forms.Padding(8);
            this.ntsNavigation.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.ntsNavigation.Size = new System.Drawing.Size(782, 55);
            this.ntsNavigation.Stretch = true;
            this.ntsNavigation.TabIndex = 0;
            // 
            // rbnRibbon
            // 
            this.rbnRibbon.Location = new System.Drawing.Point(0, 0);
            this.rbnRibbon.Minimized = false;
            this.rbnRibbon.Name = "rbnRibbon";
            this.rbnRibbon.ResourceName = "electrifier.Core.Resources.ElApplicationWindow.Ribbon.ribbon";
            this.rbnRibbon.ShortcutTableResourceName = null;
            this.rbnRibbon.Size = new System.Drawing.Size(782, 122);
            this.rbnRibbon.TabIndex = 6;
            // 
            // fspFormStatePersistor
            // 
            this.fspFormStatePersistor.ClientForm = this;
            this.fspFormStatePersistor.FixWindowState = true;
            this.fspFormStatePersistor.FormToDesktopMargin = new System.Drawing.Size(94, 94);
            this.fspFormStatePersistor.LoadingFormState += new System.EventHandler<electrifier.Core.Components.FormStatePersistorEventArgs>(this.fspFormStatePersistor_LoadingFormState);
            this.fspFormStatePersistor.SavingFormState += new System.EventHandler<electrifier.Core.Components.FormStatePersistorEventArgs>(this.FormStatePersistor_SavingFormState);
            // 
            // ElApplicationWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(782, 553);
            this.Controls.Add(this.tscToolStripContainer);
            this.Controls.Add(this.rbnRibbon);
            this.Controls.Add(this.stsStatusStrip);
            this.Name = "ElApplicationWindow";
            this.stsStatusStrip.ResumeLayout(false);
            this.stsStatusStrip.PerformLayout();
            this.tscToolStripContainer.ContentPanel.ResumeLayout(false);
            this.tscToolStripContainer.TopToolStripPanel.ResumeLayout(false);
            this.tscToolStripContainer.TopToolStripPanel.PerformLayout();
            this.tscToolStripContainer.ResumeLayout(false);
            this.tscToolStripContainer.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.StatusStrip stsStatusStrip;
        private Components.ElFormStatePersistor fspFormStatePersistor;
        private System.Windows.Forms.ToolStripContainer tscToolStripContainer;
        private WeifenLuo.WinFormsUI.Docking.DockPanel dpnDockPanel;
        private Components.Controls.ElNavigationToolStrip ntsNavigation;
        private System.Windows.Forms.ToolStripStatusLabel tslItemCount;
        private System.Windows.Forms.ToolStripStatusLabel tslSelectionCount;
        private RibbonLib.Ribbon rbnRibbon;
    }
}