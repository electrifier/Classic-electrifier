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
    partial class ApplicationWindow
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
            this.StatusStrip = new System.Windows.Forms.StatusStrip();
            this.tslItemCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.tslSelectionCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.tscToolStripContainer = new System.Windows.Forms.ToolStripContainer();
            this.dpnDockPanel = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this.NavigationToolStrip = new electrifier.Core.Components.Controls.NavigationToolStrip();
            this.rbnRibbon = new RibbonLib.Ribbon();
            this.FormStatePersistor = new electrifier.Core.Components.FormStatePersistor(this.components);
            this.StatusStrip.SuspendLayout();
            this.tscToolStripContainer.ContentPanel.SuspendLayout();
            this.tscToolStripContainer.TopToolStripPanel.SuspendLayout();
            this.tscToolStripContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // StatusStrip
            // 
            this.StatusStrip.AutoSize = false;
            this.StatusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.StatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tslItemCount,
            this.tslSelectionCount});
            this.StatusStrip.Location = new System.Drawing.Point(0, 528);
            this.StatusStrip.Name = "StatusStrip";
            this.StatusStrip.Size = new System.Drawing.Size(782, 25);
            this.StatusStrip.TabIndex = 0;
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
            this.tscToolStripContainer.TopToolStripPanel.Controls.Add(this.NavigationToolStrip);
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
            // NavigationToolStrip
            // 
            this.NavigationToolStrip.ActiveDockContent = null;
            this.NavigationToolStrip.CurrentTheme = "iTweek by Miles Ponson (32px)";
            this.NavigationToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.NavigationToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.NavigationToolStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.NavigationToolStrip.Location = new System.Drawing.Point(0, 0);
            this.NavigationToolStrip.Name = "NavigationToolStrip";
            this.NavigationToolStrip.Padding = new System.Windows.Forms.Padding(8);
            this.NavigationToolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.NavigationToolStrip.Size = new System.Drawing.Size(782, 55);
            this.NavigationToolStrip.Stretch = true;
            this.NavigationToolStrip.TabIndex = 0;
            // 
            // rbnRibbon
            // 
            this.rbnRibbon.Location = new System.Drawing.Point(0, 0);
            this.rbnRibbon.Minimized = false;
            this.rbnRibbon.Name = "rbnRibbon";
            this.rbnRibbon.ResourceName = "electrifier.Core.Resources.ApplicationWindow.Ribbon.ribbon";
            this.rbnRibbon.ShortcutTableResourceName = null;
            this.rbnRibbon.Size = new System.Drawing.Size(782, 122);
            this.rbnRibbon.TabIndex = 6;
            // 
            // FormStatePersistor
            // 
            this.FormStatePersistor.ClientForm = this;
            this.FormStatePersistor.FixWindowState = true;
            this.FormStatePersistor.FormToDesktopMargin = new System.Drawing.Size(94, 94);
            this.FormStatePersistor.LoadFormState += new System.EventHandler<electrifier.Core.Components.FormStatePersistorEventArgs>(this.FormStatePersistor_LoadFormState);
            this.FormStatePersistor.SaveFormState += new System.EventHandler<electrifier.Core.Components.FormStatePersistorEventArgs>(this.FormStatePersistor_SaveFormState);
            // 
            // ApplicationWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(782, 553);
            this.Controls.Add(this.tscToolStripContainer);
            this.Controls.Add(this.rbnRibbon);
            this.Controls.Add(this.StatusStrip);
            this.Name = "ApplicationWindow";
            this.StatusStrip.ResumeLayout(false);
            this.StatusStrip.PerformLayout();
            this.tscToolStripContainer.ContentPanel.ResumeLayout(false);
            this.tscToolStripContainer.TopToolStripPanel.ResumeLayout(false);
            this.tscToolStripContainer.TopToolStripPanel.PerformLayout();
            this.tscToolStripContainer.ResumeLayout(false);
            this.tscToolStripContainer.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.StatusStrip StatusStrip;
        private Components.FormStatePersistor FormStatePersistor;
        private System.Windows.Forms.ToolStripContainer tscToolStripContainer;
        private WeifenLuo.WinFormsUI.Docking.DockPanel dpnDockPanel;
        private Components.Controls.NavigationToolStrip NavigationToolStrip;
        private System.Windows.Forms.ToolStripStatusLabel tslItemCount;
        private System.Windows.Forms.ToolStripStatusLabel tslSelectionCount;
        private RibbonLib.Ribbon rbnRibbon;
    }
}