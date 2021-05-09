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

using electrifier.Core.Components;
using electrifier.Core.Components.DockContents;
using electrifier.Core.WindowsShell;
using RibbonLib.Controls.Events;
using System.Windows.Forms;
using Vanara.PInvoke;

namespace electrifier.Core.Forms
{
    /// <summary>
    /// <see cref="ElApplicationWindow"/> is the Main Window of electrifier application.
    /// 
    /// This partial class file contains the implementation of the Ribbon Commands, i.e. the Ribbon Command event listeners.
    /// </summary>
    public partial class ElApplicationWindow
    {
        internal void CmdAppOpenNewShellBrowserPane_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            AppContext.TraceScope();

            DockContentFactory.CreateShellBrowser(this);
        }

        //private void TsbNewFileBrowser_ButtonClick(object sender, EventArgs e)
        //{
        //    AppContext.TraceScope();

        //    this.CreateNewShellBrowser();
        //}

        //private void TsbNewFileBrowserLeft_Click(object sender, EventArgs e)
        //{
        //    AppContext.TraceScope();

        //    this.CreateNewShellBrowser(WeifenLuo.WinFormsUI.Docking.DockAlignment.Left);
        //}

        //private void TsbNewFileBrowserRight_Click(object sender, EventArgs e)
        //{
        //    AppContext.TraceScope();

        //    this.CreateNewShellBrowser(DockAlignment.Right);
        //}

        //private void TsbNewFileBrowserTop_Click(object sender, EventArgs e)
        //{
        //    AppContext.TraceScope();

        //    this.CreateNewShellBrowser(DockAlignment.Top);
        //}

        //private void TsbNewFileBrowserBottom_Click(object sender, EventArgs e)
        //{
        //    AppContext.TraceScope();

        //    this.CreateNewShellBrowser(DockAlignment.Bottom);
        //}

        // TODO 03/02/19: TsbNewFileBrowserFloating_Click has not been used yet
        //private void TsbNewFileBrowserFloating_Click(object sender, EventArgs e)
        //{
        //    AppContext.TraceScope();

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
            AppContext.TraceScope();

            using (AboutElectrifierDialog aboutDialog = new AboutElectrifierDialog())
            {
                aboutDialog.ShowDialog();
            }
        }

        internal void CmdAppHelpAboutWindows_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            AppContext.TraceScope();

            Shell32.ShellAbout(this.Handle, @"electrifier - Windows Info", AppContext.GetDotNetFrameworkVersion(), this.Icon.Handle);
        }


        internal void CmdAppClose_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            AppContext.TraceScope();

            // Close form asynchronously since we are in a ribbon event handler, so the ribbon is still in use, and calling Close 
            // will eventually call _ribbon.DestroyFramework(), which is a big no-no, if you still use the ribbon.
            this.BeginInvoke(new MethodInvoker(this.Close));
        }




        /// <summary>
        /// TODO: [Moved from ELApplicationWindow.Ribbon.cs#Ribbon_ProcessDockContentChange]
        /// https://docs.microsoft.com/de-de/dotnet/api/system.windows.dataobject?view=netframework-4.7.2
        /// </summary>

        // => https://github.com/dahall/Vanara/blob/master/PInvoke/Shell32/Clipboard.cs ShellClipboardFormat
        //public const string CFStr_Filename = "FileNameW";
        //public const string CFStr_PreferredDropEffect = "Preferred DropEffect";
        //public const string CFStr_ShellIdList = "Shell IDList Array";
        //public const string CFStr_ShellIdListOffset = "Shell Object Offsets";

        internal void CmdSelectConditional_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            this.BeginInvoke(new MethodInvoker(delegate ()
               {
                   DockContentFactory.ToggleSelectConditionalBox(this.dpnDockPanel);
               }));
        }

        internal void CmdInvertSelection_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            // TODO: Idea: 03/01/20: Put SelectAll/None/InvertSelection into IClipboardConsumer to avoid accessing ActiveContent
            //if (this.dpnDockPanel.ActiveContent is ElShellBrowserDockContent elShellBrowserDockContent)
            //{
            //    elShellBrowserDockContent.InvertSelection();
            //}
        }

        internal void CmdBtnDesktopIconLayoutSave_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            // TODO: Error-Handling! cause of call from Ribbon, then remove invoke
            this.BeginInvoke(new MethodInvoker(delegate ()
            {
                ElDesktopIconManager.SaveLayout();
            }));
        }

        internal void CmdBtnDesktopIconLayoutRestore_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            // TODO: Error-Handling! cause of call from Ribbon, then remove invoke
            this.BeginInvoke(new MethodInvoker(delegate ()
            {
                ElDesktopIconManager.RestoreLayout();
            }));
        }
    }
}
