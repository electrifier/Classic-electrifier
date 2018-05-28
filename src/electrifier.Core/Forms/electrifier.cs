using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace electrifier.Core.Forms
{
    /// <summary>
    /// 
    /// </summary>
    public struct LastKnownFormState
    {
        public FormWindowState FormWindowState;
        public Point Location;
        public Size Size;
    }

    /// <summary>
    /// 
    /// </summary>
    public partial class Electrifier : System.Windows.Forms.Form
    {
        #region Fields ========================================================================================================

        protected Guid guid = Guid.NewGuid();

        protected LastKnownFormState lastKnownFormState;

        private static string formTitle_Appendix = String.Format("electrifier v{0}", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString());

        #endregion ============================================================================================================

        #region Properties ====================================================================================================

        public Guid Guid { get { return this.guid; } }

        public override string Text {
            get { return base.Text; }
            set {
                base.Text = ((value.Length > 0) ? (value + " - " + Electrifier.formTitle_Appendix) : Electrifier.formTitle_Appendix);
                this.formTitle_AddDebugRemark();
            }
        }

        #endregion ============================================================================================================

        public Electrifier(Icon icon) : base()
        {
            AppContext.TraceScope();

            InitializeComponent();

            this.Icon = icon;
            this.Text = this.Text;  // Add formTitleAppendix



            this.dpnDockPanel.Theme = new WeifenLuo.WinFormsUI.Docking.VS2015LightTheme();

            this.Resize += new System.EventHandler(this.electrifier_Resize);
            this.LocationChanged += new System.EventHandler(this.electrifier_LocationChanged);
        }

        [Conditional("DEBUG")]
        private void formTitle_AddDebugRemark()
        {
            AppContext.TraceScope();

            base.Text += " [DEBUG]";
        }

        private void CreateNewFileBrowser(DockAlignment? dockAlignment = null)
        {
            var newDockContent = new Controls.DockContents.ShellBrowserDockContent();
            var activeDocumentPane = this.dpnDockPanel.ActiveDocumentPane;

            AppContext.TraceScope();

            // See https://github.com/dockpanelsuite/dockpanelsuite/issues/348: we only take care of DocumentStyle.DockingWindow
            //if (this.dpnDockPanel.DocumentStyle == WeifenLuo.WinFormsUI.Docking.DocumentStyle.SystemMdi)
            //{
            //    newDockContent.Show();
            //    newDockContent.MdiParent = this;
            //} else ...

            if ((dockAlignment != null) && (activeDocumentPane != null))
                newDockContent.Show(activeDocumentPane, dockAlignment.Value, 0.5d);
            else
                newDockContent.Show(this.dpnDockPanel, DockState.Document);

            // TODO: Add windows to open window list
            // TODO: Browse to standard folder...
        }

        #region Event Listeners ===============================================================================================

        private void mniFileExit_Click(object sender, EventArgs e)
        {
            AppContext.TraceScope();

            // Close form asynchronously since we are in a ribbon event 
            // handler, so the ribbon is still in use, and calling Close 
            // will eventually call _ribbon.DestroyFramework(), which is 
            // a big no-no, if you still use the ribbon.
            this.BeginInvoke(new MethodInvoker(this.Close));
        }

        private void mniHelpAbout_Click(object sender, EventArgs e)
        {
            AppContext.TraceScope();

            var aboutElectrifier = new AboutElectrifier();

            aboutElectrifier.ShowDialog();
        }

        private void tsbNewFileBrowser_ButtonClick(object sender, EventArgs e)
        {
            AppContext.TraceScope();

            this.CreateNewFileBrowser();
        }

        private void tsbNewFileBrowserLeft_Click(object sender, EventArgs e)
        {
            AppContext.TraceScope();

            this.CreateNewFileBrowser(DockAlignment.Left);
        }

        private void tsbNewFileBrowserRight_Click(object sender, EventArgs e)
        {
            AppContext.TraceScope();

            this.CreateNewFileBrowser(DockAlignment.Right);
        }

        private void tsbNewFileBrowserTop_Click(object sender, EventArgs e)
        {
            AppContext.TraceScope();

            this.CreateNewFileBrowser(DockAlignment.Top);
        }

        private void tsbNewFileBrowserBottom_Click(object sender, EventArgs e)
        {
            AppContext.TraceScope();

            this.CreateNewFileBrowser(DockAlignment.Bottom);
        }

        private void tsbNewFileBrowserFloating_Click(object sender, EventArgs e)
        {
            AppContext.TraceScope();

            var newDockContent = new Controls.DockContents.ShellBrowserDockContent();
            var floatWindowBounds = new Rectangle(this.Location, this.Size);

            floatWindowBounds.Offset((this.Width - this.ClientSize.Width), (this.Height - this.ClientSize.Height));

            newDockContent.Show(this.dpnDockPanel, floatWindowBounds);
        }

        private void electrifier_Resize(object sender, EventArgs e)
        {
            AppContext.TraceScope();

            this.lastKnownFormState.FormWindowState = this.WindowState;

            if (this.lastKnownFormState.FormWindowState == FormWindowState.Normal)
                this.lastKnownFormState.Size = this.Size;
        }

        private void electrifier_LocationChanged(object sender, EventArgs e)
        {
            AppContext.TraceScope();

            if (this.WindowState == FormWindowState.Normal)
                this.lastKnownFormState.Location = this.Location;
        }

        #endregion
    }
}
