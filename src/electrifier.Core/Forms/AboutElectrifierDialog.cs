using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Windows.Forms;

namespace electrifier.Core.Forms
{
    // TODO: Make this a DockPanel, add Log-Viewer when log-files are enabled...
    partial class AboutElectrifierDialog
      : Form
    {
        public AboutElectrifierDialog()
        {
            LogContext.Trace();

            this.InitializeComponent();

            // Load background image of version tab from electrifier.exe embedded resource
            this.VersionTabPage.BackgroundImage = new Bitmap(Assembly.GetEntryAssembly().
                GetManifestResourceStream(@"electrifier.SplashScreenForm.png"));

            // Load license document from electrifier.Core.dll embedded resource
            this.LicenseRichTextBox.Rtf = Properties.Resources.License.ToString(CultureInfo.InvariantCulture);

            // Show additional Assembly Informations
            this.ProductVersionLabel.Text = $"{AppContext.AssemblyProduct} v{AppContext.AssemblyVersion}";
            this.CopyrightLabel.Text = AppContext.AssemblyCopyright;
        }

        private void VisitElectrifierOrgLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LogContext.Trace();

            Process.Start("http://www.electrifier.org");
        }

        private void LicenseRichTextBox_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            LogContext.Trace();

            Process.Start(e.LinkText);
        }
    }
}
