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

using electrifier.Core.Forms;
using RibbonLib.Controls;
using RibbonLib.Controls.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Vanara.Windows.Shell;
using WeifenLuo.WinFormsUI.Docking;

using System.Linq;
using electrifier.Core.Components.ShellObjects;

namespace electrifier.Core.Components.DockContents
{

    public partial class ClipboardHistoryDock
        : DockContent
        , IRibbonConsumer
        , IToggleableDockContent<ClipboardHistoryDock>
    {

        public ApplicationWindow ApplicationWindow { get; }

        //public EventHandler<ExecuteEventArgs> RibbonBtnClipboardHistory;

        public ClipboardHistoryDock(ApplicationWindow applicationWindow)
        {
            this.ApplicationWindow = applicationWindow ?? throw new ArgumentNullException(nameof(applicationWindow));

            this.InitializeComponent();
            this.InitializeRibbonBinding(this.ApplicationWindow.RibbonItems);

            NativeClipboard.ClipboardUpdate += this.NativeClipboard_ClipboardUpdate;

            //this.RibbonBtnClipboardHistory += this.RibbonBtnClipboardHistory;
            //this.BtnClipboardHistory.ExecuteEvent += this.BtnClipboardHistory_ExecuteEvent;


            //foreach (IBaseRibbonControlBinding ribbonControlBinding in this.RibbonControlBindings)
            //{
            //    ribbonControlBinding.ActivateRibbonState();
            //}
        }

        private void BtnClipboardHistory_ExecuteEvent(object sender, ExecuteEventArgs e)
        {
            LogContext.Trace($"Not yet implemented!");
        }

        //private static string GetWindowNameFromHandle(IntPtr hWnd, string defaultNameOnFailure = null)
        //{
        //    string result = defaultNameOnFailure;

        //    uint processId = 0;
        //    uint threadId = Vanara.PInvoke.User32.GetWindowThreadProcessId(hWnd, out uint lpdwProcessId);

        //    StringBuilder exeFullName = new StringBuilder(4096); // MAX_PATH

        //    uint stringlen = Vanara.PInvoke.User32.GetWindowModuleFileName(hWnd, exeFullName, (uint)exeFullName.Capacity);



        //    return result;

        //}

        // See https://devblogs.microsoft.com/oldnewthing/20210526-00/?p=105252#:~:text=%20How%20ownership%20of%20the%20Windows%20clipboard%20is,Close%C2%ADClipboard%20%28%29%20to%20indicate%20that%20you...%20More%20
        // See https://www.codeproject.com/Articles/15333/Clipboard-backup-in-C
        // See https://www.codeproject.com/reference/1091137/windows-clipboard-formats#:~:text=The%20Standard%20Clipboard%20Formats%20%5BMSDN%5D%20use%20numeric%20IDs,shell%20objects%20via%20clipboard%20and%20Drag%20%26%20Drop.

        public static readonly DataFormats.Format[] TextFormatIDs = {
            DataFormats.GetFormat(DataFormats.Html),
            DataFormats.GetFormat(DataFormats.Rtf),
            DataFormats.GetFormat(DataFormats.UnicodeText),
            DataFormats.GetFormat(DataFormats.Text),
        };

        
        public static readonly DataFormats.Format[] shellItemFormatIDs = {
            DataFormats.GetFormat(DataFormats.FileDrop),
        };


        public class ClipboardHistoryItem
        {
            public DataFormats.Format Format { get; }
            public object DataObject { get; }

            public DateTime Timestamp { get; }

            public ClipboardHistoryItem(DataFormats.Format format, object dataObject)
            {
                this.Format = format;
                this.DataObject = dataObject;
                this.Timestamp = DateTime.Now;
            }
        }

        // New:
        public readonly List<ClipboardItem> HistoryItems = new List<ClipboardItem>();     // TODO: 21/06/28 => Move into own class
//        public readonly List<ClipboardHistoryItem> HistoryItems = new List<ClipboardHistoryItem>();     // TODO: 21/06/28 => Move into own class


        private void NativeClipboard_ClipboardUpdate(object sender, EventArgs e)
        {
            LogContext.Debug($"Event NativeClipboard_ClipboardUpdate() raised. Sender is { sender.ToString() }.");

            IntPtr owner = NativeClipboard.GetClipboardOwner();      // NOT NULL
            IntPtr test = NativeClipboard.GetOpenClipboardWindow();  // null

            /*
             * https://stackoverflow.com/questions/5116429/get-window-instance-from-window-handle
             * 
            */

            //IntPtr handle = process.MainWindowHandle;

            //HwndSource hwndSource = HwndSource.FromHwnd(handle);

            //Window = hwndSource.RootVisual as Window;


            // See https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.dataformats?view=net-5.0

            // See https://dotnettutorials.net/lesson/linq-any-operator/

            try
            {
                //
                // Text
                //
                int textFormat = NativeClipboard.GetFirstFormatAvailable(TextFormatIDs.Select(fmt => fmt.Id).ToArray());

                if (textFormat > 0)
                {
                    var dataobject = new TextClipboardDataObject();
                    var clipboardobject = new ClipboardItem(DateTime.Now, dataobject);

                    this.HistoryItems.Add(clipboardobject);
                }

                //
                // ShellItems
                //
                int shellItemFormat = NativeClipboard.GetFirstFormatAvailable(shellItemFormatIDs.Select(fmt => fmt.Id).ToArray());

                if (shellItemFormat == DataFormats.GetFormat(DataFormats.FileDrop).Id) // 15
                {
                    //string fmtName = "file";  //TextFormatIDs.Single(fmt => fmt.Id == shellItemFormat).Name;

                    var dataobject = new ShellItemDataObject();
                    var clipboardobject = new ClipboardItem(DateTime.Now, dataobject);

                    this.HistoryItems.Add(clipboardobject);
                }

            }
            finally
            {
//                NativeClipboard.ClipboardUpdate += this.NativeClipboard_ClipboardUpdate;
                this.UpdateClipboardHistoryListView();
            }
        }

        protected void UpdateClipboardHistoryListView()
        {
            this.lvwClipboardHistory.Items.Clear();

            foreach (ClipboardItem clipboardItem in this.HistoryItems)
            {
                ListViewItem lvItem = new ListViewItem();   // => ClipboardHistoryItem

                lvItem.ImageIndex = clipboardItem.ImageIndex;

                lvItem.Text = clipboardItem.DisplayString;
                lvItem.SubItems.Add(clipboardItem.Timestamp.ToString("HH:mm:ss"));

                this.lvwClipboardHistory.Items.Add(lvItem);
            }
        }



        //protected void ProcessTextFormat()
        //{
        //    object dataObject;

        //    IDataObject idataObject = Clipboard.GetDataObject();

        //    //var test = idataObject.ToString();


        //    if (NativeClipboard.IsFormatAvailable(DataFormats.Rtf))
        //    {
        //        dataObject = Clipboard.GetData(DataFormats.Rtf);
        //    }
        //    else
        //        dataObject = Clipboard.GetData(DataFormats.Text);

        //    var copied = dataObject;



        //    var idataobject = Clipboard.GetDataObject();

        //}


        public new void Show() => Show(this.ApplicationWindow.DockPanel, DockState.DockRightAutoHide);





        public bool Visibilty { get => this.Visible; set => this.SetVisibility(value); }


        //public ClipboardHistoryDock GetInstance() => this.Instance ?? new ClipboardHistoryDock(this.ApplicationWindow);


        public bool SetVisibility(bool newVisibilty)
        {
            // TODO: Hide if newVisibilty == false
            this.Show();
            return true;
        }

        public bool ToggleVisibilty() => (this.Visibilty = !this.Visibilty);

        #region IRibbonConsumer ===============================================================================================
        public RibbonItems RibbonItems { get; private set; }

        private IBaseRibbonControlBinding[] RibbonControlBindings;
        public RibbonToggleButtonBinding BtnClipboardHistory { get; private set; }
        public IBaseRibbonControlBinding[] InitializeRibbonBinding(RibbonItems ribbonItems)
        {
            this.RibbonItems = ribbonItems ?? throw new ArgumentNullException(nameof(ribbonItems));

            this.RibbonControlBindings = new IBaseRibbonControlBinding[]
            {
                this.BtnClipboardHistory = new RibbonToggleButtonBinding(ribbonItems.BtnClipboardHistory, this.BtnClipboardHistory_ExecuteEvent, enabled: true, booleanValue: false),
            };

            return this.RibbonControlBindings;
        }
        public void ActivateRibbonState()
        {
            //throw new NotImplementedException();
        }

        public void DeactivateRibbonState()
        {
            //throw new NotImplementedException();
        }

        #endregion


    }
}
