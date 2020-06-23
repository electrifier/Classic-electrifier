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
    partial class ElAboutDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
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
            this.DialogOkButton = new System.Windows.Forms.Button();
            this.Version_LicenseTabControl = new System.Windows.Forms.TabControl();
            this.VersionTabPage = new System.Windows.Forms.TabPage();
            this.CopyrightLabel = new System.Windows.Forms.Label();
            this.ProductVersionLabel = new System.Windows.Forms.Label();
            this.LicenseTabPage = new System.Windows.Forms.TabPage();
            this.LicenseRichTextBox = new System.Windows.Forms.RichTextBox();
            this.VisitElectrifierOrgLabel = new System.Windows.Forms.LinkLabel();
            this.Version_LicenseTabControl.SuspendLayout();
            this.VersionTabPage.SuspendLayout();
            this.LicenseTabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // DialogOkButton
            // 
            this.DialogOkButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.DialogOkButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.DialogOkButton.Location = new System.Drawing.Point(566, 511);
            this.DialogOkButton.Margin = new System.Windows.Forms.Padding(4);
            this.DialogOkButton.Name = "DialogOkButton";
            this.DialogOkButton.Size = new System.Drawing.Size(100, 27);
            this.DialogOkButton.TabIndex = 30;
            this.DialogOkButton.Text = "&OK";
            // 
            // Version_LicenseTabControl
            // 
            this.Version_LicenseTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Version_LicenseTabControl.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.Version_LicenseTabControl.Controls.Add(this.VersionTabPage);
            this.Version_LicenseTabControl.Controls.Add(this.LicenseTabPage);
            this.Version_LicenseTabControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Version_LicenseTabControl.Location = new System.Drawing.Point(15, 14);
            this.Version_LicenseTabControl.Name = "Version_LicenseTabControl";
            this.Version_LicenseTabControl.SelectedIndex = 0;
            this.Version_LicenseTabControl.Size = new System.Drawing.Size(660, 490);
            this.Version_LicenseTabControl.TabIndex = 32;
            // 
            // VersionTabPage
            // 
            this.VersionTabPage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.VersionTabPage.Controls.Add(this.CopyrightLabel);
            this.VersionTabPage.Controls.Add(this.ProductVersionLabel);
            this.VersionTabPage.Location = new System.Drawing.Point(4, 37);
            this.VersionTabPage.Name = "VersionTabPage";
            this.VersionTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.VersionTabPage.Size = new System.Drawing.Size(652, 449);
            this.VersionTabPage.TabIndex = 0;
            this.VersionTabPage.Text = "Version";
            // 
            // CopyrightLabel
            // 
            this.CopyrightLabel.BackColor = System.Drawing.Color.Transparent;
            this.CopyrightLabel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.CopyrightLabel.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CopyrightLabel.Location = new System.Drawing.Point(3, 423);
            this.CopyrightLabel.Margin = new System.Windows.Forms.Padding(0);
            this.CopyrightLabel.Name = "CopyrightLabel";
            this.CopyrightLabel.Size = new System.Drawing.Size(646, 23);
            this.CopyrightLabel.TabIndex = 28;
            this.CopyrightLabel.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // ProductVersionLabel
            // 
            this.ProductVersionLabel.BackColor = System.Drawing.Color.Transparent;
            this.ProductVersionLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.ProductVersionLabel.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ProductVersionLabel.Location = new System.Drawing.Point(3, 3);
            this.ProductVersionLabel.Margin = new System.Windows.Forms.Padding(0);
            this.ProductVersionLabel.Name = "ProductVersionLabel";
            this.ProductVersionLabel.Size = new System.Drawing.Size(646, 23);
            this.ProductVersionLabel.TabIndex = 2;
            // 
            // LicenseTabPage
            // 
            this.LicenseTabPage.Controls.Add(this.LicenseRichTextBox);
            this.LicenseTabPage.Location = new System.Drawing.Point(4, 37);
            this.LicenseTabPage.Name = "LicenseTabPage";
            this.LicenseTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.LicenseTabPage.Size = new System.Drawing.Size(652, 449);
            this.LicenseTabPage.TabIndex = 1;
            this.LicenseTabPage.Text = "License";
            // 
            // LicenseRichTextBox
            // 
            this.LicenseRichTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.LicenseRichTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LicenseRichTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LicenseRichTextBox.Location = new System.Drawing.Point(3, 3);
            this.LicenseRichTextBox.Name = "LicenseRichTextBox";
            this.LicenseRichTextBox.ReadOnly = true;
            this.LicenseRichTextBox.Size = new System.Drawing.Size(646, 443);
            this.LicenseRichTextBox.TabIndex = 2;
            this.LicenseRichTextBox.Text = "";
            this.LicenseRichTextBox.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.LicenseRichTextBox_LinkClicked);
            // 
            // VisitElectrifierOrgLabel
            // 
            this.VisitElectrifierOrgLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.VisitElectrifierOrgLabel.AutoSize = true;
            this.VisitElectrifierOrgLabel.Location = new System.Drawing.Point(16, 516);
            this.VisitElectrifierOrgLabel.Name = "VisitElectrifierOrgLabel";
            this.VisitElectrifierOrgLabel.Size = new System.Drawing.Size(164, 17);
            this.VisitElectrifierOrgLabel.TabIndex = 33;
            this.VisitElectrifierOrgLabel.TabStop = true;
            this.VisitElectrifierOrgLabel.Text = "Visit www.electrifier.org...";
            this.VisitElectrifierOrgLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.VisitElectrifierOrgLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.VisitElectrifierOrgLabel_LinkClicked);
            // 
            // ElAboutDialog
            // 
            this.AcceptButton = this.DialogOkButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.DialogOkButton;
            this.ClientSize = new System.Drawing.Size(682, 553);
            this.Controls.Add(this.VisitElectrifierOrgLabel);
            this.Controls.Add(this.Version_LicenseTabControl);
            this.Controls.Add(this.DialogOkButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(700, 580);
            this.Name = "ElAboutDialog";
            this.Padding = new System.Windows.Forms.Padding(12, 11, 12, 11);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About electrifier";
            this.Version_LicenseTabControl.ResumeLayout(false);
            this.VersionTabPage.ResumeLayout(false);
            this.LicenseTabPage.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button DialogOkButton;
        private System.Windows.Forms.TabControl Version_LicenseTabControl;
        private System.Windows.Forms.TabPage VersionTabPage;
        private System.Windows.Forms.TabPage LicenseTabPage;
        private System.Windows.Forms.Label ProductVersionLabel;
        private System.Windows.Forms.Label CopyrightLabel;
        private System.Windows.Forms.RichTextBox LicenseRichTextBox;
        private System.Windows.Forms.LinkLabel VisitElectrifierOrgLabel;
    }
}
