namespace electrifier.Core.Forms
{
    partial class SessionSelector
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SessionSelector));
            this.Session_WorkbenchSplitContainer = new System.Windows.Forms.SplitContainer();
            this.SessionGroupBox = new System.Windows.Forms.GroupBox();
            this.CreateSessionDescriptionLabel = new System.Windows.Forms.Label();
            this.CreateSessionNameLabel = new System.Windows.Forms.Label();
            this.CreateSessionDescriptionTextBox = new System.Windows.Forms.TextBox();
            this.CreateSessionNameTextBox = new System.Windows.Forms.TextBox();
            this.ContinueSessionListView = new System.Windows.Forms.ListView();
            this.SessionNameColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SessionDescriptionColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SessionCreatedColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SessionModifiedColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ContinueSessionRadioButton = new System.Windows.Forms.RadioButton();
            this.CreateSessionRadioButton = new System.Windows.Forms.RadioButton();
            this.WorkbenchGroupBox = new System.Windows.Forms.GroupBox();
            this.WorkbenchListBox = new System.Windows.Forms.ListBox();
            this.DialogButtonPanel = new System.Windows.Forms.Panel();
            this.DialogOkButton = new System.Windows.Forms.Button();
            this.DialogRememberChoiceCheckBox = new System.Windows.Forms.CheckBox();
            this.BetaWarningPanel = new System.Windows.Forms.Panel();
            this.BetaWarningRichTextBox = new System.Windows.Forms.RichTextBox();
            this.BetaWarningPictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.Session_WorkbenchSplitContainer)).BeginInit();
            this.Session_WorkbenchSplitContainer.Panel1.SuspendLayout();
            this.Session_WorkbenchSplitContainer.Panel2.SuspendLayout();
            this.Session_WorkbenchSplitContainer.SuspendLayout();
            this.SessionGroupBox.SuspendLayout();
            this.WorkbenchGroupBox.SuspendLayout();
            this.DialogButtonPanel.SuspendLayout();
            this.BetaWarningPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BetaWarningPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // Session_WorkbenchSplitContainer
            // 
            this.Session_WorkbenchSplitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Session_WorkbenchSplitContainer.Location = new System.Drawing.Point(12, 12);
            this.Session_WorkbenchSplitContainer.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Session_WorkbenchSplitContainer.Name = "Session_WorkbenchSplitContainer";
            // 
            // Session_WorkbenchSplitContainer.Panel1
            // 
            this.Session_WorkbenchSplitContainer.Panel1.Controls.Add(this.SessionGroupBox);
            // 
            // Session_WorkbenchSplitContainer.Panel2
            // 
            this.Session_WorkbenchSplitContainer.Panel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.Session_WorkbenchSplitContainer.Panel2.Controls.Add(this.WorkbenchGroupBox);
            this.Session_WorkbenchSplitContainer.Size = new System.Drawing.Size(757, 335);
            this.Session_WorkbenchSplitContainer.SplitterDistance = 510;
            this.Session_WorkbenchSplitContainer.SplitterWidth = 8;
            this.Session_WorkbenchSplitContainer.TabIndex = 0;
            this.Session_WorkbenchSplitContainer.TabStop = false;
            // 
            // SessionGroupBox
            // 
            this.SessionGroupBox.Controls.Add(this.CreateSessionDescriptionLabel);
            this.SessionGroupBox.Controls.Add(this.CreateSessionNameLabel);
            this.SessionGroupBox.Controls.Add(this.CreateSessionDescriptionTextBox);
            this.SessionGroupBox.Controls.Add(this.CreateSessionNameTextBox);
            this.SessionGroupBox.Controls.Add(this.ContinueSessionListView);
            this.SessionGroupBox.Controls.Add(this.ContinueSessionRadioButton);
            this.SessionGroupBox.Controls.Add(this.CreateSessionRadioButton);
            this.SessionGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SessionGroupBox.Location = new System.Drawing.Point(0, 0);
            this.SessionGroupBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.SessionGroupBox.Name = "SessionGroupBox";
            this.SessionGroupBox.Padding = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.SessionGroupBox.Size = new System.Drawing.Size(510, 335);
            this.SessionGroupBox.TabIndex = 0;
            this.SessionGroupBox.TabStop = false;
            this.SessionGroupBox.Text = "Session";
            // 
            // CreateSessionDescriptionLabel
            // 
            this.CreateSessionDescriptionLabel.AutoSize = true;
            this.CreateSessionDescriptionLabel.Location = new System.Drawing.Point(53, 87);
            this.CreateSessionDescriptionLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.CreateSessionDescriptionLabel.Name = "CreateSessionDescriptionLabel";
            this.CreateSessionDescriptionLabel.Size = new System.Drawing.Size(83, 17);
            this.CreateSessionDescriptionLabel.TabIndex = 7;
            this.CreateSessionDescriptionLabel.Text = "Description:";
            // 
            // CreateSessionNameLabel
            // 
            this.CreateSessionNameLabel.AutoSize = true;
            this.CreateSessionNameLabel.Location = new System.Drawing.Point(53, 55);
            this.CreateSessionNameLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.CreateSessionNameLabel.Name = "CreateSessionNameLabel";
            this.CreateSessionNameLabel.Size = new System.Drawing.Size(49, 17);
            this.CreateSessionNameLabel.TabIndex = 6;
            this.CreateSessionNameLabel.Text = "Name:";
            // 
            // CreateSessionDescriptionTextBox
            // 
            this.CreateSessionDescriptionTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CreateSessionDescriptionTextBox.Location = new System.Drawing.Point(183, 84);
            this.CreateSessionDescriptionTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.CreateSessionDescriptionTextBox.Name = "CreateSessionDescriptionTextBox";
            this.CreateSessionDescriptionTextBox.Size = new System.Drawing.Size(318, 22);
            this.CreateSessionDescriptionTextBox.TabIndex = 5;
            // 
            // CreateSessionNameTextBox
            // 
            this.CreateSessionNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CreateSessionNameTextBox.Location = new System.Drawing.Point(183, 52);
            this.CreateSessionNameTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.CreateSessionNameTextBox.Name = "CreateSessionNameTextBox";
            this.CreateSessionNameTextBox.Size = new System.Drawing.Size(318, 22);
            this.CreateSessionNameTextBox.TabIndex = 4;
            // 
            // ContinueSessionListView
            // 
            this.ContinueSessionListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ContinueSessionListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.SessionNameColumnHeader,
            this.SessionDescriptionColumnHeader,
            this.SessionCreatedColumnHeader,
            this.SessionModifiedColumnHeader});
            this.ContinueSessionListView.FullRowSelect = true;
            this.ContinueSessionListView.HideSelection = false;
            this.ContinueSessionListView.Location = new System.Drawing.Point(8, 144);
            this.ContinueSessionListView.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ContinueSessionListView.MinimumSize = new System.Drawing.Size(84, 78);
            this.ContinueSessionListView.MultiSelect = false;
            this.ContinueSessionListView.Name = "ContinueSessionListView";
            this.ContinueSessionListView.Size = new System.Drawing.Size(493, 181);
            this.ContinueSessionListView.TabIndex = 3;
            this.ContinueSessionListView.UseCompatibleStateImageBehavior = false;
            this.ContinueSessionListView.View = System.Windows.Forms.View.Details;
            this.ContinueSessionListView.SelectedIndexChanged += new System.EventHandler(this.ContinueSessionListView_SelectedIndexChanged);
            this.ContinueSessionListView.DoubleClick += new System.EventHandler(this.ContinueSessionListView_DoubleClick);
            // 
            // SessionNameColumnHeader
            // 
            this.SessionNameColumnHeader.Text = "Name";
            this.SessionNameColumnHeader.Width = 80;
            // 
            // SessionDescriptionColumnHeader
            // 
            this.SessionDescriptionColumnHeader.Text = "Description";
            this.SessionDescriptionColumnHeader.Width = 120;
            // 
            // SessionCreatedColumnHeader
            // 
            this.SessionCreatedColumnHeader.Text = "Created";
            this.SessionCreatedColumnHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.SessionCreatedColumnHeader.Width = 80;
            // 
            // SessionModifiedColumnHeader
            // 
            this.SessionModifiedColumnHeader.Text = "Modified";
            this.SessionModifiedColumnHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.SessionModifiedColumnHeader.Width = 80;
            // 
            // ContinueSessionRadioButton
            // 
            this.ContinueSessionRadioButton.AutoSize = true;
            this.ContinueSessionRadioButton.Location = new System.Drawing.Point(8, 118);
            this.ContinueSessionRadioButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ContinueSessionRadioButton.Name = "ContinueSessionRadioButton";
            this.ContinueSessionRadioButton.Size = new System.Drawing.Size(89, 21);
            this.ContinueSessionRadioButton.TabIndex = 2;
            this.ContinueSessionRadioButton.TabStop = true;
            this.ContinueSessionRadioButton.Text = "Continue:";
            this.ContinueSessionRadioButton.UseVisualStyleBackColor = true;
            this.ContinueSessionRadioButton.CheckedChanged += new System.EventHandler(this.ContinueSessionRadioButton_CheckedChanged);
            // 
            // CreateSessionRadioButton
            // 
            this.CreateSessionRadioButton.AutoSize = true;
            this.CreateSessionRadioButton.Location = new System.Drawing.Point(8, 25);
            this.CreateSessionRadioButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.CreateSessionRadioButton.Name = "CreateSessionRadioButton";
            this.CreateSessionRadioButton.Size = new System.Drawing.Size(146, 21);
            this.CreateSessionRadioButton.TabIndex = 1;
            this.CreateSessionRadioButton.TabStop = true;
            this.CreateSessionRadioButton.Text = "Start new Session:";
            this.CreateSessionRadioButton.UseVisualStyleBackColor = true;
            this.CreateSessionRadioButton.CheckedChanged += new System.EventHandler(this.CreateSessionRadioButton_CheckedChanged);
            // 
            // WorkbenchGroupBox
            // 
            this.WorkbenchGroupBox.Controls.Add(this.WorkbenchListBox);
            this.WorkbenchGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.WorkbenchGroupBox.Enabled = false;
            this.WorkbenchGroupBox.Location = new System.Drawing.Point(0, 0);
            this.WorkbenchGroupBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.WorkbenchGroupBox.Name = "WorkbenchGroupBox";
            this.WorkbenchGroupBox.Padding = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.WorkbenchGroupBox.Size = new System.Drawing.Size(239, 335);
            this.WorkbenchGroupBox.TabIndex = 3;
            this.WorkbenchGroupBox.TabStop = false;
            this.WorkbenchGroupBox.Text = "Additional Workbenches";
            // 
            // WorkbenchListBox
            // 
            this.WorkbenchListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.WorkbenchListBox.FormattingEnabled = true;
            this.WorkbenchListBox.IntegralHeight = false;
            this.WorkbenchListBox.ItemHeight = 16;
            this.WorkbenchListBox.Location = new System.Drawing.Point(8, 25);
            this.WorkbenchListBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.WorkbenchListBox.Name = "WorkbenchListBox";
            this.WorkbenchListBox.Size = new System.Drawing.Size(222, 301);
            this.WorkbenchListBox.TabIndex = 4;
            // 
            // DialogButtonPanel
            // 
            this.DialogButtonPanel.Controls.Add(this.DialogOkButton);
            this.DialogButtonPanel.Controls.Add(this.DialogRememberChoiceCheckBox);
            this.DialogButtonPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.DialogButtonPanel.Location = new System.Drawing.Point(0, 496);
            this.DialogButtonPanel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.DialogButtonPanel.Name = "DialogButtonPanel";
            this.DialogButtonPanel.Size = new System.Drawing.Size(781, 57);
            this.DialogButtonPanel.TabIndex = 4;
            // 
            // DialogOkButton
            // 
            this.DialogOkButton.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.DialogOkButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.DialogOkButton.Location = new System.Drawing.Point(611, 12);
            this.DialogOkButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.DialogOkButton.Name = "DialogOkButton";
            this.DialogOkButton.Size = new System.Drawing.Size(160, 32);
            this.DialogOkButton.TabIndex = 6;
            this.DialogOkButton.Text = "&Ok";
            this.DialogOkButton.UseVisualStyleBackColor = true;
            this.DialogOkButton.Click += new System.EventHandler(this.DialogOkButton_Click);
            // 
            // DialogRememberChoiceCheckBox
            // 
            this.DialogRememberChoiceCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.DialogRememberChoiceCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.DialogRememberChoiceCheckBox.Location = new System.Drawing.Point(20, 21);
            this.DialogRememberChoiceCheckBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.DialogRememberChoiceCheckBox.Name = "DialogRememberChoiceCheckBox";
            this.DialogRememberChoiceCheckBox.Size = new System.Drawing.Size(585, 17);
            this.DialogRememberChoiceCheckBox.TabIndex = 5;
            this.DialogRememberChoiceCheckBox.Text = "&Remember this Choice";
            this.DialogRememberChoiceCheckBox.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.DialogRememberChoiceCheckBox.UseVisualStyleBackColor = true;
            // 
            // BetaWarningPanel
            // 
            this.BetaWarningPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BetaWarningPanel.BackColor = System.Drawing.Color.Transparent;
            this.BetaWarningPanel.Controls.Add(this.BetaWarningRichTextBox);
            this.BetaWarningPanel.Controls.Add(this.BetaWarningPictureBox);
            this.BetaWarningPanel.Location = new System.Drawing.Point(12, 352);
            this.BetaWarningPanel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.BetaWarningPanel.Name = "BetaWarningPanel";
            this.BetaWarningPanel.Size = new System.Drawing.Size(757, 135);
            this.BetaWarningPanel.TabIndex = 5;
            // 
            // BetaWarningRichTextBox
            // 
            this.BetaWarningRichTextBox.BackColor = System.Drawing.SystemColors.Info;
            this.BetaWarningRichTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.BetaWarningRichTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BetaWarningRichTextBox.Location = new System.Drawing.Point(200, 0);
            this.BetaWarningRichTextBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.BetaWarningRichTextBox.Name = "BetaWarningRichTextBox";
            this.BetaWarningRichTextBox.ReadOnly = true;
            this.BetaWarningRichTextBox.Size = new System.Drawing.Size(557, 135);
            this.BetaWarningRichTextBox.TabIndex = 0;
            this.BetaWarningRichTextBox.TabStop = false;
            this.BetaWarningRichTextBox.Text = resources.GetString("BetaWarningRichTextBox.Text");
            // 
            // BetaWarningPictureBox
            // 
            this.BetaWarningPictureBox.BackColor = System.Drawing.SystemColors.Info;
            this.BetaWarningPictureBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("BetaWarningPictureBox.BackgroundImage")));
            this.BetaWarningPictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.BetaWarningPictureBox.Dock = System.Windows.Forms.DockStyle.Left;
            this.BetaWarningPictureBox.Location = new System.Drawing.Point(0, 0);
            this.BetaWarningPictureBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.BetaWarningPictureBox.Name = "BetaWarningPictureBox";
            this.BetaWarningPictureBox.Size = new System.Drawing.Size(200, 135);
            this.BetaWarningPictureBox.TabIndex = 2;
            this.BetaWarningPictureBox.TabStop = false;
            // 
            // SessionSelector
            // 
            this.AcceptButton = this.DialogOkButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(781, 553);
            this.Controls.Add(this.BetaWarningPanel);
            this.Controls.Add(this.Session_WorkbenchSplitContainer);
            this.Controls.Add(this.DialogButtonPanel);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "SessionSelector";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "What do you want to electrify today?";
            this.Session_WorkbenchSplitContainer.Panel1.ResumeLayout(false);
            this.Session_WorkbenchSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Session_WorkbenchSplitContainer)).EndInit();
            this.Session_WorkbenchSplitContainer.ResumeLayout(false);
            this.SessionGroupBox.ResumeLayout(false);
            this.SessionGroupBox.PerformLayout();
            this.WorkbenchGroupBox.ResumeLayout(false);
            this.DialogButtonPanel.ResumeLayout(false);
            this.BetaWarningPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.BetaWarningPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Components.ElFormStatePersistor FormStatePersistor;
        private System.Windows.Forms.SplitContainer Session_WorkbenchSplitContainer;
        private System.Windows.Forms.GroupBox SessionGroupBox;
        private System.Windows.Forms.Panel DialogButtonPanel;
        private System.Windows.Forms.Button DialogOkButton;
        private System.Windows.Forms.CheckBox DialogRememberChoiceCheckBox;
        private System.Windows.Forms.Panel BetaWarningPanel;
        private System.Windows.Forms.PictureBox BetaWarningPictureBox;
        private System.Windows.Forms.RichTextBox BetaWarningRichTextBox;
        private System.Windows.Forms.GroupBox WorkbenchGroupBox;
        private System.Windows.Forms.ListBox WorkbenchListBox;
        private System.Windows.Forms.RadioButton CreateSessionRadioButton;
        private System.Windows.Forms.RadioButton ContinueSessionRadioButton;
        private System.Windows.Forms.ListView ContinueSessionListView;
        private System.Windows.Forms.ColumnHeader SessionNameColumnHeader;
        private System.Windows.Forms.ColumnHeader SessionDescriptionColumnHeader;
        private System.Windows.Forms.ColumnHeader SessionCreatedColumnHeader;
        private System.Windows.Forms.ColumnHeader SessionModifiedColumnHeader;
        private System.Windows.Forms.Label CreateSessionDescriptionLabel;
        private System.Windows.Forms.Label CreateSessionNameLabel;
        private System.Windows.Forms.TextBox CreateSessionDescriptionTextBox;
        private System.Windows.Forms.TextBox CreateSessionNameTextBox;
    }
}