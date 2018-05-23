using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
        protected Guid guid = Guid.NewGuid();
        public Guid Guid { get { return this.guid; } }

        protected LastKnownFormState lastKnownFormState;

        private const string formTitleAppendix = "electrifier"
#if (DEBUG)
            + " [debug build]"
#endif
        ;

        public Electrifier(Icon icon) : base()
        {
            AppContext.TraceScope();

            InitializeComponent();

            this.Icon = icon;
            this.Text = Electrifier.formTitleAppendix;

            this.Resize += new System.EventHandler(this.electrifier_Resize);
            this.LocationChanged += new System.EventHandler(this.electrifier_LocationChanged);
        }

        #region Command Event Listeners

        private void mniFileExit_Click(object sender, EventArgs e)
        {
            // Close form asynchronously since we are in a ribbon event 
            // handler, so the ribbon is still in use, and calling Close 
            // will eventually call _ribbon.DestroyFramework(), which is 
            // a big no-no, if you still use the ribbon.
            this.BeginInvoke(new MethodInvoker(this.Close));
        }

        private void tmiNewPanelFloating_Click(object sender, EventArgs e)
        {
            electrifier.Core.Controls.DockContents.ShellBrowserDockContent shellBrowserDockControl = new Controls.DockContents.ShellBrowserDockContent();

            if (this.dpnDockPanel.DocumentStyle == WeifenLuo.WinFormsUI.Docking.DocumentStyle.SystemMdi)
            {
                shellBrowserDockControl.Show();
                shellBrowserDockControl.MdiParent = this;
            }
            else
            {
                shellBrowserDockControl.Show(this.dpnDockPanel, WeifenLuo.WinFormsUI.Docking.DockState.Document);
            }
        }

        #endregion

        #region Event Listeners


        private void electrifier_Resize(object sender, EventArgs e)
        {
            this.lastKnownFormState.FormWindowState = this.WindowState;

            if (this.lastKnownFormState.FormWindowState == FormWindowState.Normal)
                this.lastKnownFormState.Size = this.Size;
        }

        private void electrifier_LocationChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
                this.lastKnownFormState.Location = this.Location;
        }

        #endregion
    }
}
