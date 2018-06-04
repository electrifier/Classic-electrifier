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

namespace electrifier.Core.Forms
{
    partial class AboutElectrifier
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
            this.btnOk = new System.Windows.Forms.Button();
            this.tctTabControl = new System.Windows.Forms.TabControl();
            this.tpgVersion = new System.Windows.Forms.TabPage();
            this.lblCopyright = new System.Windows.Forms.Label();
            this.lblProductVersion = new System.Windows.Forms.Label();
            this.tbgLicense = new System.Windows.Forms.TabPage();
            this.rtbLicense = new System.Windows.Forms.RichTextBox();
            this.lblVisitElectrifierOrg = new System.Windows.Forms.LinkLabel();
            this.tctTabControl.SuspendLayout();
            this.tpgVersion.SuspendLayout();
            this.tbgLicense.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(566, 511);
            this.btnOk.Margin = new System.Windows.Forms.Padding(4);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(100, 27);
            this.btnOk.TabIndex = 30;
            this.btnOk.Text = "&OK";
            // 
            // tctTabControl
            // 
            this.tctTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tctTabControl.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tctTabControl.Controls.Add(this.tpgVersion);
            this.tctTabControl.Controls.Add(this.tbgLicense);
            this.tctTabControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tctTabControl.Location = new System.Drawing.Point(15, 14);
            this.tctTabControl.Name = "tctTabControl";
            this.tctTabControl.SelectedIndex = 0;
            this.tctTabControl.Size = new System.Drawing.Size(660, 490);
            this.tctTabControl.TabIndex = 32;
            // 
            // tpgVersion
            // 
            this.tpgVersion.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.tpgVersion.Controls.Add(this.lblCopyright);
            this.tpgVersion.Controls.Add(this.lblProductVersion);
            this.tpgVersion.Location = new System.Drawing.Point(4, 37);
            this.tpgVersion.Name = "tpgVersion";
            this.tpgVersion.Padding = new System.Windows.Forms.Padding(3);
            this.tpgVersion.Size = new System.Drawing.Size(652, 449);
            this.tpgVersion.TabIndex = 0;
            this.tpgVersion.Text = "Version";
            // 
            // lblCopyright
            // 
            this.lblCopyright.BackColor = System.Drawing.Color.Transparent;
            this.lblCopyright.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblCopyright.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCopyright.Location = new System.Drawing.Point(3, 423);
            this.lblCopyright.Margin = new System.Windows.Forms.Padding(0);
            this.lblCopyright.Name = "lblCopyright";
            this.lblCopyright.Size = new System.Drawing.Size(646, 23);
            this.lblCopyright.TabIndex = 28;
            this.lblCopyright.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // lblProductVersion
            // 
            this.lblProductVersion.BackColor = System.Drawing.Color.Transparent;
            this.lblProductVersion.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblProductVersion.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProductVersion.Location = new System.Drawing.Point(3, 3);
            this.lblProductVersion.Margin = new System.Windows.Forms.Padding(0);
            this.lblProductVersion.Name = "lblProductVersion";
            this.lblProductVersion.Size = new System.Drawing.Size(646, 23);
            this.lblProductVersion.TabIndex = 2;
            // 
            // tbgLicense
            // 
            this.tbgLicense.Controls.Add(this.rtbLicense);
            this.tbgLicense.Location = new System.Drawing.Point(4, 37);
            this.tbgLicense.Name = "tbgLicense";
            this.tbgLicense.Padding = new System.Windows.Forms.Padding(3);
            this.tbgLicense.Size = new System.Drawing.Size(652, 449);
            this.tbgLicense.TabIndex = 1;
            this.tbgLicense.Text = "License";
            // 
            // rtbLicense
            // 
            this.rtbLicense.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbLicense.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbLicense.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbLicense.Location = new System.Drawing.Point(3, 3);
            this.rtbLicense.Name = "rtbLicense";
            this.rtbLicense.ReadOnly = true;
            this.rtbLicense.Size = new System.Drawing.Size(646, 443);
            this.rtbLicense.TabIndex = 2;
            this.rtbLicense.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.RtbLicense_LinkClicked);
            // 
            // lblVisitElectrifierOrg
            // 
            this.lblVisitElectrifierOrg.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblVisitElectrifierOrg.AutoSize = true;
            this.lblVisitElectrifierOrg.Location = new System.Drawing.Point(16, 516);
            this.lblVisitElectrifierOrg.Name = "lblVisitElectrifierOrg";
            this.lblVisitElectrifierOrg.Size = new System.Drawing.Size(164, 17);
            this.lblVisitElectrifierOrg.TabIndex = 33;
            this.lblVisitElectrifierOrg.TabStop = true;
            this.lblVisitElectrifierOrg.Text = "Visit www.electrifier.org...";
            this.lblVisitElectrifierOrg.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblVisitElectrifierOrg.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LblVisitElectrifierOrg_LinkClicked);
            // 
            // AboutElectrifier
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnOk;
            this.ClientSize = new System.Drawing.Size(682, 553);
            this.Controls.Add(this.lblVisitElectrifierOrg);
            this.Controls.Add(this.tctTabControl);
            this.Controls.Add(this.btnOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(700, 580);
            this.Name = "AboutElectrifier";
            this.Padding = new System.Windows.Forms.Padding(12, 11, 12, 11);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About electrifier";
            this.tctTabControl.ResumeLayout(false);
            this.tpgVersion.ResumeLayout(false);
            this.tbgLicense.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.TabControl tctTabControl;
        private System.Windows.Forms.TabPage tpgVersion;
        private System.Windows.Forms.TabPage tbgLicense;
        private System.Windows.Forms.Label lblProductVersion;
        private System.Windows.Forms.Label lblCopyright;
        private System.Windows.Forms.RichTextBox rtbLicense;
        private System.Windows.Forms.LinkLabel lblVisitElectrifierOrg;
    }
}
