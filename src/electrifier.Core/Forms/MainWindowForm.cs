/*
** 
**  electrifier
** 
**  Copyright 2017 Thorsten Jung, www.electrifier.org
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

using System;
using System.Drawing;
using System.Windows.Forms;

//using RibbonLib.Controls;
//using RibbonLib.Controls.Events;

using electrifier.Core.Controls.DockContents;

namespace electrifier.Core.Forms
{
    public enum RibbonMarkupCommands : uint
    {
        ////// Backstage View respectively Application Menu Items //////////////////////////////////////////////////////////////////
        //cmdBtnApp_OpenNewWindow = 10000,
        //cmdBtnApp_OpenNewShellBrowserPanel = 10001,
        //cmdBtnApp_OpenCommandPrompt = 10002,
        //cmdBtnApp_OpenWindowsPowerShell = 10003,
        //cmdBtnApp_ChangeElectrifierOptions = 10010,
        //cmdBtnApp_ChangeFolderAndSearchOptions = 10011,
        //cmdBtnApp_Help = 10020,
        //cmdBtnApp_Help_AboutElectrifier = 10021,
        //cmdBtnApp_Help_AboutWindows = 10025,
        //cmdBtnApp_Close = 10030,
        ////// Ribbon tabs /////////////////////////////////////////////////////////////////////////////////////////////////////////
        //cmdTabHome = 20000,
        //cmdTabShare = 30000,
        //cmdTabView = 40000,
        ////// Command Group One: Clipboard ////////////////////////////////////////////////////////////////////////////////////////
        //cmdGrpHomeClipboard = 20100,
        //cmdBtnClipboardCut = 20101,
        //cmdBtnClipboardCopy = 20102,
        //cmdBtnClipboardPaste = 20103,
        ////// Command Group Two: Organize /////////////////////////////////////////////////////////////////////////////////////////
        //cmdGrpHomeOrganize = 20200,
        //cmdBtnOrganizeMoveTo = 20201,
        //cmdBtnOrganizeDelete = 20202,
        //cmdBtnOrganizeRename = 20203,
    }

    public struct LastKnownFormState
    {
        public FormWindowState FormWindowState;
        public Point Location;
        public Size Size;
    }

    /// <summary>
    /// Summary for MainWindowForm.
    /// </summary>
    public partial class MainWindowForm : Form
    {
        protected Guid guid = Guid.NewGuid();
        public Guid Guid { get { return this.guid; } }

        private const string formTitleAppendix = "electrifier"
#if (DEBUG)
            + " [debug build]"
#endif
        ;

        //private RibbonButton _cmdBtnApp_OpenNewShellBrowserPanel;
        //private RibbonButton _cmdBtnApp_Close;
        //private RibbonTab _cmdTabHome;

        protected LastKnownFormState _lastKnownFormState;

        public MainWindowForm(Icon icon) : base()
        {
            InitializeComponent();

            this.Icon = icon;

            //this._cmdBtnApp_OpenNewShellBrowserPanel = new RibbonButton(this._mainRibbon, (uint)RibbonMarkupCommands.cmdBtnApp_OpenNewShellBrowserPanel);
            //this._cmdBtnApp_OpenNewShellBrowserPanel.ExecuteEvent += new EventHandler<ExecuteEventArgs>(CmdBtnApp_OpenNewShellBrowserPanel_ExecuteEvent);
            //this._cmdBtnApp_Close = new RibbonButton(this._mainRibbon, (uint)RibbonMarkupCommands.cmdBtnApp_Close);
            //this._cmdBtnApp_Close.ExecuteEvent += new EventHandler<ExecuteEventArgs>(CmdBtnApp_Close_ExecuteEvent);
            //this._cmdTabHome = new RibbonTab(this._mainRibbon, (uint)RibbonMarkupCommands.cmdTabHome);

            this.Resize += new System.EventHandler(this.MainWindowForm_Resize);
            this.LocationChanged += new System.EventHandler(this.MainWindowForm_LocationChanged);

            this.CmdBtnApp_OpenNewShellBrowserPanel_ExecuteEvent(this, new EventArgs());
        }

        public void MainWindowForm_Resize(object sender, EventArgs e)
        {
            this._lastKnownFormState.FormWindowState = this.WindowState;

            if (this._lastKnownFormState.FormWindowState == FormWindowState.Normal)
                this._lastKnownFormState.Size = this.Size;
        }

        public void MainWindowForm_LocationChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
                this._lastKnownFormState.Location = this.Location;
        }


        private void MainWindowForm_Load(object sender, EventArgs e)
        {

        }

        private void Shbrwsr_BrowsingAddressChanged(object source, EventArgs e)
        {
            //         ShellBrowserDockContent shbrwsr = source as ShellBrowserDockContent;

            //         /* TODO: RELAUNCH: Commented out
            //if(shbrwsr != null) {
            //	this.toolBar1.Address = shbrwsr.BrowsingAddress;
            //}
            //*/
        }

        private void CmdBtnApp_OpenNewShellBrowserPanel_ExecuteEvent(object sender, EventArgs e)
        {
            ShellBrowserDockContent shellBrowserDockControl = new ShellBrowserDockContent();

            if (this.dockPanel.DocumentStyle == WeifenLuo.WinFormsUI.Docking.DocumentStyle.SystemMdi)
            {
                shellBrowserDockControl.Show();
                shellBrowserDockControl.MdiParent = this;
            }
            else
            {
                shellBrowserDockControl.Show(this.dockPanel, WeifenLuo.WinFormsUI.Docking.DockState.Document);
            }
        }

        private void CmdBtnApp_Close_ExecuteEvent(object sender, EventArgs e)
        {
            // Close form asynchronously since we are in a ribbon event 
            // handler, so the ribbon is still in use, and calling Close 
            // will eventually call _ribbon.DestroyFramework(), which is 
            // a big no-no, if you still use the ribbon.
            this.BeginInvoke(new MethodInvoker(this.Close));
        }
    }
}
