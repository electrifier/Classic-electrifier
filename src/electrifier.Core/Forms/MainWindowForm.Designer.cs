namespace electrifier.Core.Forms {
    partial class MainWindowForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this._mainRibbon = new RibbonLib.Ribbon();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dockPanel = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // _mainRibbon
            // 
            this._mainRibbon.Location = new System.Drawing.Point(0, 0);
            this._mainRibbon.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this._mainRibbon.Minimized = false;
            this._mainRibbon.Name = "_mainRibbon";
            this._mainRibbon.ResourceName = "electrifier.Core.Forms.MainWindowForm.RibbonMarkup.ribbon";
            this._mainRibbon.ShortcutTableResourceName = null;
            this._mainRibbon.Size = new System.Drawing.Size(1045, 174);
            this._mainRibbon.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dockPanel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 174);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1045, 516);
            this.panel1.TabIndex = 1;
            // 
            // dockPanel
            // 
            this.dockPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dockPanel.DocumentStyle = WeifenLuo.WinFormsUI.Docking.DocumentStyle.DockingWindow;
            this.dockPanel.Location = new System.Drawing.Point(0, 0);
            this.dockPanel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dockPanel.Name = "dockPanel";
            this.dockPanel.ShowDocumentIcon = true;
            this.dockPanel.Size = new System.Drawing.Size(1045, 516);
            this.dockPanel.TabIndex = 0;
            // 
            // MainWindowForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(1045, 690);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this._mainRibbon);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "MainWindowForm";
            this.Load += new System.EventHandler(this.MainWindowForm_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private RibbonLib.Ribbon _mainRibbon;
        private System.Windows.Forms.Panel panel1;
        private WeifenLuo.WinFormsUI.Docking.DockPanel dockPanel;
    }
}