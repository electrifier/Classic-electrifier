using System;
using System.Windows.Forms;
using System.Xml;

using Microsoft.WindowsAPICodePack.Controls.WindowsForms;
using Microsoft.WindowsAPICodePack.Shell;

namespace electrifier.Core.Controls.DockContents
{
    /// <summary>
    /// Summary of ShellBrowserDockContent.
    /// </summary>
    public class ShellBrowserDockContent
        : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        protected Guid Guid;
        protected ExplorerBrowser explorerBrowser;

        public ShellBrowserDockContent() : this(Guid.NewGuid()) { }

        public ShellBrowserDockContent(Guid guid) : base()
        {
            // Initialize the underlying DockControl
            this.Guid = guid;
            this.Name = "ShellBrowserDockContent." + this.Guid.ToString();
            this.Text = "ExplorerBrowser";
            this.explorerBrowser = new ExplorerBrowser()
            {
                Dock = DockStyle.Fill
            };

            this.explorerBrowser.Navigate(KnownFolders.Documents as ShellObject);
            this.Controls.Add(this.explorerBrowser);
        }
    }
}
