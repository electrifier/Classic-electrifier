
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ClipboardHistoryDock));
            System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("Today, 7/1/21", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("Yesterday, 06/30", System.Windows.Forms.HorizontalAlignment.Left);
            this.btnDelete = new System.Windows.Forms.Button();
            this.lvwClipboardHistory = new System.Windows.Forms.ListView();
            this.chTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chPreview = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chReCopy = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ssClipboardHistory = new System.Windows.Forms.StatusStrip();
            this.ilHistoryItemImageList = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelete.Image = ((System.Drawing.Image)(resources.GetObject("btnDelete.Image")));
            this.btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnDelete.Location = new System.Drawing.Point(313, 633);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(177, 44);
            this.btnDelete.TabIndex = 2;
            this.btnDelete.Text = "&Delete";
            this.btnDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnDelete.UseVisualStyleBackColor = false;
            // 
            // lvwClipboardHistory
            // 
            this.lvwClipboardHistory.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.lvwClipboardHistory.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvwClipboardHistory.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chTime,
            this.chPreview,
            this.chReCopy});
            this.lvwClipboardHistory.FullRowSelect = true;
            this.lvwClipboardHistory.GridLines = true;
            listViewGroup1.Header = "Today, 7/1/21";
            listViewGroup1.Name = "lvGroupToday";
            listViewGroup2.Header = "Yesterday, 06/30";
            listViewGroup2.Name = "lvGroupYesterday";
            this.lvwClipboardHistory.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2});
            this.lvwClipboardHistory.HideSelection = false;
            this.lvwClipboardHistory.HotTracking = true;
            this.lvwClipboardHistory.HoverSelection = true;
            this.lvwClipboardHistory.LabelEdit = true;
            this.lvwClipboardHistory.Location = new System.Drawing.Point(12, 12);
            this.lvwClipboardHistory.Name = "lvwClipboardHistory";
            this.lvwClipboardHistory.Size = new System.Drawing.Size(478, 615);
            this.lvwClipboardHistory.SmallImageList = this.ilHistoryItemImageList;
            this.lvwClipboardHistory.TabIndex = 3;
            this.lvwClipboardHistory.UseCompatibleStateImageBehavior = false;
            this.lvwClipboardHistory.View = System.Windows.Forms.View.Details;
            // 
            // chTime
            // 
            this.chTime.Text = "Time";
            this.chTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.chTime.Width = 80;
            // 
            // chPreview
            // 
            this.chPreview.Text = "Preview";
            this.chPreview.Width = 160;
            // 
            // chReCopy
            // 
            this.chReCopy.Text = "Re-Copy";
            this.chReCopy.Width = 80;
            // 
            // ssClipboardHistory
            // 
            this.ssClipboardHistory.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.ssClipboardHistory.Location = new System.Drawing.Point(0, 680);
            this.ssClipboardHistory.Name = "ssClipboardHistory";
            this.ssClipboardHistory.Size = new System.Drawing.Size(502, 22);
            this.ssClipboardHistory.SizingGrip = false;
            this.ssClipboardHistory.TabIndex = 4;
            this.ssClipboardHistory.Text = "statusStrip1";
            // 
            // ilHistoryItemImageList
            // 
            this.ilHistoryItemImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilHistoryItemImageList.ImageStream")));
            this.ilHistoryItemImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.ilHistoryItemImageList.Images.SetKeyName(0, "sh71_TextFile.ico");
            this.ilHistoryItemImageList.Images.SetKeyName(1, "sh4_Folder.ico");
            // 
            // ClipboardHistoryDock
            // 
            this.ClientSize = new System.Drawing.Size(502, 702);
            this.Controls.Add(this.ssClipboardHistory);
            this.Controls.Add(this.lvwClipboardHistory);
            this.Controls.Add(this.btnDelete);
            this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)(((WeifenLuo.WinFormsUI.Docking.DockAreas.Float | WeifenLuo.WinFormsUI.Docking.DockAreas.DockLeft) 
            | WeifenLuo.WinFormsUI.Docking.DockAreas.DockRight)));
            this.HideOnClose = true;
            this.Name = "ClipboardHistoryDock";
            this.Text = "Clipboard History";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.ListView lvwClipboardHistory;
        private System.Windows.Forms.ColumnHeader chTime;
        private System.Windows.Forms.ColumnHeader chPreview;
        private System.Windows.Forms.StatusStrip ssClipboardHistory;
        private System.Windows.Forms.ColumnHeader chReCopy;
        private System.Windows.Forms.ImageList ilHistoryItemImageList;
    }
}