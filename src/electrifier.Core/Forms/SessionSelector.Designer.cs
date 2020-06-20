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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SessionSelector));
            this.Session_WorkbenchSplitContainer = new System.Windows.Forms.SplitContainer();
            this.SessionGroupBox = new System.Windows.Forms.GroupBox();
            this.SessionListBox = new System.Windows.Forms.ListBox();
            this.WorkbenchGroupBox = new System.Windows.Forms.GroupBox();
            this.WorkbenchListBox = new System.Windows.Forms.ListBox();
            this.DialogButtonPanel = new System.Windows.Forms.Panel();
            this.DialogOkButton = new System.Windows.Forms.Button();
            this.DialogLaunchNewSessionButton = new System.Windows.Forms.Button();
            this.RememberMyChoiceCheckBox = new System.Windows.Forms.CheckBox();
            this.BetaWarningPanel = new System.Windows.Forms.Panel();
            this.BetaWarningRichTextBox = new System.Windows.Forms.RichTextBox();
            this.BetaWarningPictureBox = new System.Windows.Forms.PictureBox();
            this.FormStatePersistor = new electrifier.Core.Components.ElFormStatePersistor(this.components);
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
            this.Session_WorkbenchSplitContainer.Size = new System.Drawing.Size(758, 275);
            this.Session_WorkbenchSplitContainer.SplitterDistance = 496;
            this.Session_WorkbenchSplitContainer.SplitterWidth = 6;
            this.Session_WorkbenchSplitContainer.TabIndex = 0;
            // 
            // SessionGroupBox
            // 
            this.SessionGroupBox.Controls.Add(this.SessionListBox);
            this.SessionGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SessionGroupBox.Location = new System.Drawing.Point(0, 0);
            this.SessionGroupBox.Name = "SessionGroupBox";
            this.SessionGroupBox.Padding = new System.Windows.Forms.Padding(6);
            this.SessionGroupBox.Size = new System.Drawing.Size(496, 275);
            this.SessionGroupBox.TabIndex = 0;
            this.SessionGroupBox.TabStop = false;
            this.SessionGroupBox.Text = "Select previous Session";
            // 
            // SessionListBox
            // 
            this.SessionListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SessionListBox.FormattingEnabled = true;
            this.SessionListBox.IntegralHeight = false;
            this.SessionListBox.ItemHeight = 16;
            this.SessionListBox.Location = new System.Drawing.Point(6, 21);
            this.SessionListBox.Name = "SessionListBox";
            this.SessionListBox.Size = new System.Drawing.Size(484, 248);
            this.SessionListBox.TabIndex = 1;
            // 
            // WorkbenchGroupBox
            // 
            this.WorkbenchGroupBox.Controls.Add(this.WorkbenchListBox);
            this.WorkbenchGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.WorkbenchGroupBox.Location = new System.Drawing.Point(0, 0);
            this.WorkbenchGroupBox.Name = "WorkbenchGroupBox";
            this.WorkbenchGroupBox.Padding = new System.Windows.Forms.Padding(6);
            this.WorkbenchGroupBox.Size = new System.Drawing.Size(256, 275);
            this.WorkbenchGroupBox.TabIndex = 3;
            this.WorkbenchGroupBox.TabStop = false;
            this.WorkbenchGroupBox.Text = "Additional Workbenches";
            // 
            // WorkbenchListBox
            // 
            this.WorkbenchListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.WorkbenchListBox.FormattingEnabled = true;
            this.WorkbenchListBox.IntegralHeight = false;
            this.WorkbenchListBox.ItemHeight = 16;
            this.WorkbenchListBox.Location = new System.Drawing.Point(6, 21);
            this.WorkbenchListBox.Name = "WorkbenchListBox";
            this.WorkbenchListBox.Size = new System.Drawing.Size(244, 248);
            this.WorkbenchListBox.TabIndex = 0;
            // 
            // DialogButtonPanel
            // 
            this.DialogButtonPanel.Controls.Add(this.DialogOkButton);
            this.DialogButtonPanel.Controls.Add(this.RememberMyChoiceCheckBox);
            this.DialogButtonPanel.Controls.Add(this.DialogLaunchNewSessionButton);
            this.DialogButtonPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.DialogButtonPanel.Location = new System.Drawing.Point(0, 497);
            this.DialogButtonPanel.Name = "DialogButtonPanel";
            this.DialogButtonPanel.Size = new System.Drawing.Size(782, 56);
            this.DialogButtonPanel.TabIndex = 4;
            // 
            // DialogOkButton
            // 
            this.DialogOkButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.DialogOkButton.Location = new System.Drawing.Point(610, 12);
            this.DialogOkButton.Name = "DialogOkButton";
            this.DialogOkButton.Size = new System.Drawing.Size(160, 32);
            this.DialogOkButton.TabIndex = 5;
            this.DialogOkButton.Text = "&Ok";
            this.DialogOkButton.UseVisualStyleBackColor = true;
            // 
            // DialogLaunchNewSessionButton
            // 
            this.DialogLaunchNewSessionButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.DialogLaunchNewSessionButton.Location = new System.Drawing.Point(12, 12);
            this.DialogLaunchNewSessionButton.Name = "DialogLaunchNewSessionButton";
            this.DialogLaunchNewSessionButton.Size = new System.Drawing.Size(160, 32);
            this.DialogLaunchNewSessionButton.TabIndex = 4;
            this.DialogLaunchNewSessionButton.Text = "Launch &New Session";
            this.DialogLaunchNewSessionButton.UseVisualStyleBackColor = true;
            // 
            // RememberMyChoiceCheckBox
            // 
            this.RememberMyChoiceCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.RememberMyChoiceCheckBox.AutoSize = true;
            this.RememberMyChoiceCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.RememberMyChoiceCheckBox.Location = new System.Drawing.Point(436, 19);
            this.RememberMyChoiceCheckBox.Name = "RememberMyChoiceCheckBox";
            this.RememberMyChoiceCheckBox.Size = new System.Drawing.Size(168, 21);
            this.RememberMyChoiceCheckBox.TabIndex = 2;
            this.RememberMyChoiceCheckBox.Text = "Remember my Choice";
            this.RememberMyChoiceCheckBox.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.RememberMyChoiceCheckBox.UseVisualStyleBackColor = true;
            // 
            // BetaWarningPanel
            // 
            this.BetaWarningPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BetaWarningPanel.BackColor = System.Drawing.Color.Transparent;
            this.BetaWarningPanel.Controls.Add(this.BetaWarningRichTextBox);
            this.BetaWarningPanel.Controls.Add(this.BetaWarningPictureBox);
            this.BetaWarningPanel.Location = new System.Drawing.Point(12, 293);
            this.BetaWarningPanel.Name = "BetaWarningPanel";
            this.BetaWarningPanel.Size = new System.Drawing.Size(758, 194);
            this.BetaWarningPanel.TabIndex = 5;
            // 
            // BetaWarningRichTextBox
            // 
            this.BetaWarningRichTextBox.BackColor = System.Drawing.SystemColors.Info;
            this.BetaWarningRichTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.BetaWarningRichTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BetaWarningRichTextBox.Location = new System.Drawing.Point(200, 0);
            this.BetaWarningRichTextBox.Name = "BetaWarningRichTextBox";
            this.BetaWarningRichTextBox.Size = new System.Drawing.Size(558, 194);
            this.BetaWarningRichTextBox.TabIndex = 3;
            this.BetaWarningRichTextBox.Text = "Warning!";
            // 
            // BetaWarningPictureBox
            // 
            this.BetaWarningPictureBox.BackColor = System.Drawing.SystemColors.Info;
            this.BetaWarningPictureBox.Dock = System.Windows.Forms.DockStyle.Left;
            this.BetaWarningPictureBox.Image = ((System.Drawing.Image)(resources.GetObject("BetaWarningPictureBox.Image")));
            this.BetaWarningPictureBox.Location = new System.Drawing.Point(0, 0);
            this.BetaWarningPictureBox.MaximumSize = new System.Drawing.Size(200, 194);
            this.BetaWarningPictureBox.MinimumSize = new System.Drawing.Size(200, 194);
            this.BetaWarningPictureBox.Name = "BetaWarningPictureBox";
            this.BetaWarningPictureBox.Size = new System.Drawing.Size(200, 194);
            this.BetaWarningPictureBox.TabIndex = 2;
            this.BetaWarningPictureBox.TabStop = false;
            // 
            // FormStatePersistor
            // 
            this.FormStatePersistor.ClientForm = this;
            this.FormStatePersistor.FixWindowState = true;
            this.FormStatePersistor.FormToDesktopMargin = new System.Drawing.Size(94, 94);
            // 
            // SessionSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(782, 553);
            this.Controls.Add(this.BetaWarningPanel);
            this.Controls.Add(this.Session_WorkbenchSplitContainer);
            this.Controls.Add(this.DialogButtonPanel);
            this.Name = "SessionSelector";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SessionSelector";
            this.Session_WorkbenchSplitContainer.Panel1.ResumeLayout(false);
            this.Session_WorkbenchSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Session_WorkbenchSplitContainer)).EndInit();
            this.Session_WorkbenchSplitContainer.ResumeLayout(false);
            this.SessionGroupBox.ResumeLayout(false);
            this.WorkbenchGroupBox.ResumeLayout(false);
            this.DialogButtonPanel.ResumeLayout(false);
            this.DialogButtonPanel.PerformLayout();
            this.BetaWarningPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.BetaWarningPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Components.ElFormStatePersistor FormStatePersistor;
        private System.Windows.Forms.SplitContainer Session_WorkbenchSplitContainer;
        private System.Windows.Forms.GroupBox SessionGroupBox;
        private System.Windows.Forms.ListBox SessionListBox;
        private System.Windows.Forms.Panel DialogButtonPanel;
        private System.Windows.Forms.Button DialogOkButton;
        private System.Windows.Forms.Button DialogLaunchNewSessionButton;
        private System.Windows.Forms.CheckBox RememberMyChoiceCheckBox;
        private System.Windows.Forms.Panel BetaWarningPanel;
        private System.Windows.Forms.PictureBox BetaWarningPictureBox;
        private System.Windows.Forms.RichTextBox BetaWarningRichTextBox;
        private System.Windows.Forms.GroupBox WorkbenchGroupBox;
        private System.Windows.Forms.ListBox WorkbenchListBox;
    }
}