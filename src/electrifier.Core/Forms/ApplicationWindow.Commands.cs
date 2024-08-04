using electrifier.Core.Components;
using electrifier.Core.Components.DockContents;
using electrifier.Core.WindowsShell;
using RibbonLib.Controls.Events;
using System.Windows.Forms;
using Vanara.PInvoke;

namespace electrifier.Core.Forms
{
    /// <summary>
    /// <see cref="ApplicationWindow"/> is the Main Window of electrifier application.
    /// 
    /// This partial class file contains the implementation of the Ribbon Commands, i.e. the Ribbon Command event listeners.
    /// </summary>
    public partial class ApplicationWindow
    {
        internal void CmdAppOpenNewShellBrowserPane_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            LogContext.Trace();

            DockContentFactory.CreateShellBrowser(this);
        }

        //private void TsbNewFileBrowser_ButtonClick(object sender, EventArgs e)
        //{
        //    LogContext.Trace();

        //    this.CreateNewShellBrowser();
        //}

        //private void TsbNewFileBrowserLeft_Click(object sender, EventArgs e)
        //{
        //    LogContext.Trace();

        //    this.CreateNewShellBrowser(WeifenLuo.WinFormsUI.Docking.DockAlignment.Left);
        //}

        //private void TsbNewFileBrowserRight_Click(object sender, EventArgs e)
        //{
        //    LogContext.Trace();

        //    this.CreateNewShellBrowser(DockAlignment.Right);
        //}

        //private void TsbNewFileBrowserTop_Click(object sender, EventArgs e)
        //{
        //    LogContext.Trace();

        //    this.CreateNewShellBrowser(DockAlignment.Top);
        //}

        //private void TsbNewFileBrowserBottom_Click(object sender, EventArgs e)
        //{
        //    LogContext.Trace();

        //    this.CreateNewShellBrowser(DockAlignment.Bottom);
        //}

        // TODO 03/02/19: TsbNewFileBrowserFloating_Click has not been used yet
        //private void TsbNewFileBrowserFloating_Click(object sender, EventArgs e)
        //{
        //    LogContext.Trace();

        //    var newDockContent = new Components.DockContents.ElShellBrowserDockContent();
        //    var floatWindowBounds = new Rectangle(this.Location, this.Size);

        //    floatWindowBounds.Offset((this.Width - this.ClientSize.Width), (this.Height - this.ClientSize.Height));

        //    newDockContent.ItemsChanged += this.NewDockContent_ItemsChanged;
        //    newDockContent.SelectionChanged += this.NewDockContent_SelectionChanged;
        //    //newDockContent.NavigationLogChanged += this.NewDockContent_NavigationLogChanged;

        //    newDockContent.Show(this.dpnDockPanel, floatWindowBounds);
        //}

        internal void CmdAppHelpAboutElectrifier_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            LogContext.Trace();

            using (var aboutDialog = new AboutElectrifierDialog())
            {
                aboutDialog.ShowDialog();
            }
        }

        internal void CmdAppHelpAboutWindows_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            LogContext.Trace();

            Shell32.ShellAbout(this.Handle, @"electrifier - Windows Info", AppContext.GetDotNetFrameworkVersion(), this.Icon.Handle);
        }


        internal void CmdAppClose_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            LogContext.Trace();

            // Close form asynchronously since we are in a ribbon event handler, so the ribbon is still in use, and calling Close 
            // will eventually call _ribbon.DestroyFramework(), which is a big no-no, if you still use the ribbon.
            this.BeginInvoke(new MethodInvoker(this.Close));
        }




//        /// <summary>
//        /// TODO: [Moved from ApplicationWindow.Ribbon.cs#Ribbon_ProcessDockContentChange]
//        /// https://docs.microsoft.com/de-de/dotnet/api/system.windows.dataobject?view=netframework-4.7.2
//        /// </summary>
//
//        // => https://github.com/dahall/Vanara/blob/master/PInvoke/Shell32/Clipboard.cs ShellClipboardFormat
//        //public const string CFStr_Filename = "FileNameW";
//        //public const string CFStr_PreferredDropEffect = "Preferred DropEffect";
//        //public const string CFStr_ShellIdList = "Shell IDList Array";
//        //public const string CFStr_ShellIdListOffset = "Shell Object Offsets";
//
//        internal void CmdSelectConditional_ExecuteEvent(object sender, ExecuteEventArgs e)
//        {
//            this.BeginInvoke(new MethodInvoker(delegate ()
//               {
//                   DockContentFactory.ToggleSelectConditionalBox(this.dpnDockPanel);
//               }));
//        }
//
//        internal void CmdInvertSelection_ExecuteEvent(object sender, ExecuteEventArgs e)
//        {
//            // TODO: Idea: 03/01/20: Put SelectAll/None/InvertSelection into IClipboardConsumer to avoid accessing ActiveContent
//            //if (this.dpnDockPanel.ActiveContent is ElShellBrowserDockContent elShellBrowserDockContent)
//            //{
//            //    elShellBrowserDockContent.InvertSelection();
//            //}
//        }
//
//        internal void CmdBtnDesktopIconLayoutSave_ExecuteEvent(object sender, ExecuteEventArgs e)
//        {
//            // TODO: Error-Handling! cause of call from Ribbon, then remove invoke
//            this.BeginInvoke(new MethodInvoker(delegate ()
//            {
//                ElDesktopIconManager.SaveLayout();
//            }));
//        }
//
//        internal void CmdBtnDesktopIconLayoutRestore_ExecuteEvent(object sender, ExecuteEventArgs e)
//        {
//            // TODO: Error-Handling! cause of call from Ribbon, then remove invoke
//            this.BeginInvoke(new MethodInvoker(delegate ()
//            {
//                ElDesktopIconManager.RestoreLayout();
//            }));
//        }
    }
}
