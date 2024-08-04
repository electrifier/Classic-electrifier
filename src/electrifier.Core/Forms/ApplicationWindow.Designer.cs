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
            this.rbnRibbon = new RibbonLib.Ribbon();
            this.FormStatePersistor = new electrifier.Core.Components.FormStatePersistor(this.components);
            this.dpnDockPanel = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this.tspTopToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.StatusStrip.SuspendLayout();
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
            // rbnRibbon
            // 
            this.rbnRibbon.Location = new System.Drawing.Point(0, 0);
            this.rbnRibbon.Name = "rbnRibbon";
            this.rbnRibbon.ResourceIdentifier = null;
            this.rbnRibbon.ResourceName = "electrifier.Core.Resources.ApplicationWindow.Ribbon.ribbon";
            this.rbnRibbon.ShortcutTableResourceName = null;
            this.rbnRibbon.Size = new System.Drawing.Size(782, 122);
            this.rbnRibbon.TabIndex = 6;
            // 
            // FormStatePersistor
            // 
            this.FormStatePersistor.ClientForm = this;
            this.FormStatePersistor.LoadFormState += new System.EventHandler<electrifier.Core.Components.FormStatePersistorEventArgs>(this.FormStatePersistor_LoadFormState);
            this.FormStatePersistor.SaveFormState += new System.EventHandler<electrifier.Core.Components.FormStatePersistorEventArgs>(this.FormStatePersistor_SaveFormState);
            // 
            // dpnDockPanel
            // 
            this.dpnDockPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dpnDockPanel.DocumentStyle = WeifenLuo.WinFormsUI.Docking.DocumentStyle.DockingWindow;
            this.dpnDockPanel.Location = new System.Drawing.Point(0, 122);
            this.dpnDockPanel.Name = "dpnDockPanel";
            this.dpnDockPanel.Size = new System.Drawing.Size(782, 406);
            this.dpnDockPanel.TabIndex = 7;
            // 
            // tspTopToolStripPanel
            // 
            this.tspTopToolStripPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.tspTopToolStripPanel.Location = new System.Drawing.Point(0, 122);
            this.tspTopToolStripPanel.Name = "tspTopToolStripPanel";
            this.tspTopToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.tspTopToolStripPanel.RowMargin = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.tspTopToolStripPanel.Size = new System.Drawing.Size(782, 0);
            // 
            // ApplicationWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(782, 553);
            this.Controls.Add(this.dpnDockPanel);
            this.Controls.Add(this.tspTopToolStripPanel);
            this.Controls.Add(this.rbnRibbon);
            this.Controls.Add(this.StatusStrip);
            this.Name = "ApplicationWindow";
            this.StatusStrip.ResumeLayout(false);
            this.StatusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip StatusStrip;
        private Components.FormStatePersistor FormStatePersistor;
        private System.Windows.Forms.ToolStripStatusLabel tslItemCount;
        private System.Windows.Forms.ToolStripStatusLabel tslSelectionCount;
        private RibbonLib.Ribbon rbnRibbon;
        private System.Windows.Forms.ToolStripPanel tspTopToolStripPanel;
        private WeifenLuo.WinFormsUI.Docking.DockPanel dpnDockPanel;
    }
}