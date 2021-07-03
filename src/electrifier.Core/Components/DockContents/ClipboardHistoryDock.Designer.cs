
namespace electrifier.Core.Components.DockContents
{
    partial class ClipboardHistoryDock
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
            System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("Today, 7/1/21", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("Yesterday, 06/30", System.Windows.Forms.HorizontalAlignment.Left);
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.lvwClipboardHistory = new System.Windows.Forms.ListView();
            this.chdPreview = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chdTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chdMove = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.BackColor = System.Drawing.Color.Transparent;
            this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.Location = new System.Drawing.Point(244, 633);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(120, 44);
            this.button1.TabIndex = 1;
            this.button1.Text = "Re&Copy";
            this.button1.UseVisualStyleBackColor = false;
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Image = global::electrifier.Core.Properties.Resources.Organise_Delete_32px;
            this.button2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button2.Location = new System.Drawing.Point(370, 633);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(120, 44);
            this.button2.TabIndex = 2;
            this.button2.Text = "&Delete";
            this.button2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.button2.UseVisualStyleBackColor = false;
            // 
            // lvwClipboardHistory
            // 
            this.lvwClipboardHistory.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvwClipboardHistory.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chdTime,
            this.chdPreview,
            this.chdMove});
            listViewGroup1.Header = "Today, 7/1/21";
            listViewGroup1.Name = "listViewGroup1";
            listViewGroup2.Header = "Yesterday, 06/30";
            listViewGroup2.Name = "listViewGroup2";
            this.lvwClipboardHistory.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2});
            this.lvwClipboardHistory.HideSelection = false;
            this.lvwClipboardHistory.Location = new System.Drawing.Point(12, 12);
            this.lvwClipboardHistory.Name = "lvwClipboardHistory";
            this.lvwClipboardHistory.Size = new System.Drawing.Size(478, 615);
            this.lvwClipboardHistory.TabIndex = 3;
            this.lvwClipboardHistory.UseCompatibleStateImageBehavior = false;
            this.lvwClipboardHistory.View = System.Windows.Forms.View.Details;
            // 
            // chdPreview
            // 
            this.chdPreview.Text = "Preview";
            this.chdPreview.Width = 160;
            // 
            // chdTime
            // 
            this.chdTime.Text = "Time";
            this.chdTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.chdTime.Width = 80;
            // 
            // chdMove
            // 
            this.chdMove.Text = "Re-Copy";
            this.chdMove.Width = 80;
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Location = new System.Drawing.Point(0, 680);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(502, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 4;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // ClipboardHistoryDock
            // 
            this.ClientSize = new System.Drawing.Size(502, 702);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.lvwClipboardHistory);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)(((WeifenLuo.WinFormsUI.Docking.DockAreas.Float | WeifenLuo.WinFormsUI.Docking.DockAreas.DockLeft) 
            | WeifenLuo.WinFormsUI.Docking.DockAreas.DockRight)));
            this.HideOnClose = true;
            this.Name = "ClipboardHistoryDock";
            this.Text = "Clipboard History";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ListView lvwClipboardHistory;
        private System.Windows.Forms.ColumnHeader chdTime;
        private System.Windows.Forms.ColumnHeader chdPreview;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ColumnHeader chdMove;
    }
}