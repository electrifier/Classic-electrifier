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

using System;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Windows.Forms;

using Vanara.Windows.Shell;

using electrifier.Core.WindowsShell;


namespace electrifier.Core.Components.DockContents
{
    /// <summary>
    /// ShellBrowserDockContent is electrifier's wrapper class for ExplorerBrowser Control.
    /// 
    /// A reference implementation of a wrapper for ExplorerBrowser can be found
    /// <a href="https://github.com/aybe/Windows-API-Code-Pack-1.1/blob/master/source/Samples/ExplorerBrowser/CS/WinForms/ExplorerBrowserTestForm.cs">here</a>.
    /// </summary>

    public class ShellBrowserDockContent
        : ElNavigableDockContent
        , IElClipboardConsumer
    {
        #region Fields ========================================================================================================

        protected Controls.ExplorerBrowserControl explorerBrowserControl;

        protected const string persistParamURI = @"URI=";
        protected const string persistParamViewMode = @"ViewMode=";

        //private ExplorerBrowserViewMode? initialViewMode = null;

        protected System.Windows.Forms.Timer UIDecouplingTimer = new System.Windows.Forms.Timer();
        protected System.Threading.AutoResetEvent explorerBrowser_itemsChangedEvent = new System.Threading.AutoResetEvent(false);
        protected System.Threading.AutoResetEvent explorerBrowser_selectionChangedEvent = new System.Threading.AutoResetEvent(false);

        private static readonly int UIDecouplingInterval = 100;                 // Wait 100ms for UI Thread to react on Item- and Selection-Changes


        #endregion Fields =====================================================================================================

        #region Properties ====================================================================================================

        public ShellItem InitialNaviagtionTarget { get; private set; } = default;

        //public ExplorerBrowserViewMode ViewMode { get => this.explorerBrowser.ContentOptions.ViewMode; set => this.explorerBrowser.ContentOptions.ViewMode = value; }

        public int ItemsCount {
            //get => this.explorerBrowser.Items.Count; }
            get => 0;
        }
        public int SelectedItemsCount {
            //get => this.explorerBrowser.SelectedItems.Count; }
            get => 0;
        }

        //public DockContent AsDockContent { get => this as WeifenLuo.WinFormsUI.Docking.DockContent; }

        //public ExplorerBrowserNavigationLog NavigationLog { get => this.explorerBrowser.NavigationLog; }

        #endregion Properties =================================================================================================

        #region Published Events ==============================================================================================

        public delegate void ItemsChangedHandler(ShellBrowserDockContent sender, EventArgs eventArgs);               // TODO: EventArgs?!?
        public event ItemsChangedHandler ItemsChanged;

        public delegate void SelectionChangedHandler(ShellBrowserDockContent sender, EventArgs eventArgs);           // TODO: EventArgs?!?
        public event SelectionChangedHandler SelectionChanged;

        //public event System.EventHandler<Microsoft.WindowsAPICodePack.Controls.NavigationLogEventArgs> NavigationLogChanged {
        //    add { this.explorerBrowser.NavigationLog.NavigationLogChanged += value; }
        //    remove { this.explorerBrowser.NavigationLog.NavigationLogChanged -= value; }
        //}

        #endregion Published Events ===========================================================================================

        // TODO: Work on Get/Lost Focus in general!

        public ShellBrowserDockContent(IElNavigationHost navigationHost, string persistString = null)
          : base(navigationHost)
        {
            this.SuspendLayout();

            try
            {
                this.Icon = Properties.Resources.ShellBrowserDockContent;

                // Evaluate persistString
                this.EvaluatePersistString(persistString);

                // Initialize ExplorerBrowser
                this.explorerBrowserControl = new Controls.ExplorerBrowserControl(this.InitialNaviagtionTarget)
                {
                    Dock = DockStyle.Fill,
                };

                // Connect ExplorerBrowser Events
                this.explorerBrowserControl.ItemsChanged += delegate (object o, EventArgs e) { this.explorerBrowser_itemsChangedEvent.Set(); };
                this.explorerBrowserControl.SelectionChanged += delegate (object o, EventArgs e) { this.explorerBrowser_selectionChangedEvent.Set(); };
                this.explorerBrowserControl.Navigating += this.ExplorerBrowserControl_Navigating;
                this.explorerBrowserControl.Navigated += this.ExplorerBrowserControl_Navigated;
                this.explorerBrowserControl.NavigationFailed += this.ExplorerBrowserControl_NavigationFailed;
                this.explorerBrowserControl.ItemsEnumerated += this.ExplorerBrowserControl_ItemsEnumerated;

                this.Controls.Add(this.explorerBrowserControl);

                // Initialize UIDecouplingTimer
                this.UIDecouplingTimer.Tick += new EventHandler(this.UIDecouplingTimer_Tick);
                this.UIDecouplingTimer.Interval = ShellBrowserDockContent.UIDecouplingInterval;
                this.UIDecouplingTimer.Start();
            }
            finally
            {
                this.ResumeLayout();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                this.UIDecouplingTimer.Dispose();

            base.Dispose(disposing);
        }

        #region DockContent Persistence Overrides =============================================================================

        /// <summary>
        /// Override of WeifenLuo.WinFormsUI.Docking.DockContent.GetPersistString()
        /// </summary>
        /// <returns>The string describing persistence information. E.g. persistString = "ShellBrowserDockContent URI=file:///C:/Users/tajbender/Desktop";</returns>
        protected override string GetPersistString()
        {
            var sb = new StringBuilder();
            string paramFmt = " {0}{1}";

            // Append class name as identifier
            sb.Append(nameof(ShellBrowserDockContent));

            // Append URI of current location
            sb.AppendFormat(paramFmt, ShellBrowserDockContent.persistParamURI,
                WindowsShell.Tools.UrlCreateFromPath(this.explorerBrowserControl.CurrentLocation));

            // Append ViewMode
            // TODO: For any reason, this doesn't work... :(
            //sb.AppendFormat(paramFmt, ShellBrowserDockContent.persistParamViewMode, this.ViewMode);

            return sb.ToString();
        }

        /// <summary>
        /// Example: persistString = "ShellBrowserDockContent URI=file:///C:/Users/tajbender/Desktop";
        /// </summary>
        /// <param name="persistString"></param>
        protected void EvaluatePersistString(string persistString)
        {
            try
            {
                if ((null != persistString) && (persistString.Trim().Length > ShellBrowserDockContent.persistParamURI.Length))
                {
                    var args = WindowsShell.Tools.SplitArgumentString(persistString);
                    string strInitialNavigationTarget = default;
                    string strInitialViewMode = default;

                    foreach (string arg in args)
                    {
                        if (arg.StartsWith(ShellBrowserDockContent.persistParamURI))
                        {
                            strInitialNavigationTarget = WindowsShell.Tools.PathCreateFromUrl(arg.Substring(ShellBrowserDockContent.persistParamURI.Length));
                        }

                        if (arg.StartsWith(ShellBrowserDockContent.persistParamViewMode))
                        {
                            strInitialViewMode = arg.Substring(ShellBrowserDockContent.persistParamViewMode.Length);
                        }
                    }

                    // Finally, when all parameters have been parsed successfully, apply them
                    if (default != strInitialNavigationTarget)
                        this.InitialNaviagtionTarget = new ShellItem(strInitialNavigationTarget);

                    //if (null != strViewMode)
                    //{
                    //    ExplorerBrowserViewMode ebvm = ExplorerBrowserViewMode.Auto;

                    //    ebvm = (ExplorerBrowserViewMode) Enum.Parse(typeof(ExplorerBrowserViewMode), strViewMode);
                    //    this.initialViewMode = ebvm;
                    //}
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("ShellBrowserDockContent.EvaluatePersistString: Error evaluating parameters"
                    + "\n\nParameters: '" + persistString + "'"
                    + "\n\nError description: '" + e.Message + "'"
                    + "\n\nResetting to default values.");
            }
        }

        #endregion DockContent Persistence Overrides ==========================================================================

        #region DockContent Event Handler =====================================================================================

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            // TODO: When multiple Tabs are opened initially, sometimes this won't be called for some folders... Most likely if invisible!
            if (default != this.InitialNaviagtionTarget)
                this.explorerBrowserControl.NavigateTo(this.InitialNaviagtionTarget);

            //if (null != this.initialViewMode)
            //{
            //    this.ViewMode = (ExplorerBrowserViewMode)this.initialViewMode;
            //}
        }

        #endregion DockContent Event Handler ==================================================================================

        protected void UIDecouplingTimer_Tick(object sender, EventArgs e)
        {
            if (this.explorerBrowser_itemsChangedEvent.WaitOne(0)) {
                AppContext.TraceDebug("Firing of ItemsChanged event.");

                this.ItemsChanged?.Invoke(this, EventArgs.Empty);
            }

            if (this.explorerBrowser_selectionChangedEvent.WaitOne(0)) {
                AppContext.TraceDebug("Firing of SelectionChanged event.");

                this.SelectionChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        #region ExplorerBrowser Internal Events Handler ========================================================================

        protected void ExplorerBrowserControl_Navigating(object sender, Controls.ExplorerBrowserControl.NavigatingEventArgs args)
        {
        }


        protected void ExplorerBrowserControl_Navigated(object sender, Controls.ExplorerBrowserControl.NavigatedEventArgs args)
        {
            AppContext.TraceDebug("Firing of ExplorerBrowserControl_Navigated event.");

            this.OnNavigationOptionsChanged(null);  // TODO

            this.BeginInvoke(new MethodInvoker(delegate ()
            {
                this.Text = args.NewLocation.Name;

                //this.Icon = args.NewLocation.Thumbnail.SmallIcon;     // TODO: Icon-Property seems not to be thread-safe

            }));
        }

        protected void ExplorerBrowserControl_NavigationFailed(object sender, Controls.ExplorerBrowserControl.NavigationFailedEventArgs args)
        {
            AppContext.TraceError("Firing of ExplorerBrowserControl_NavigationFailed event: " + args.FailedLocation.ParsingName + args.ToString());
        }

        protected void ExplorerBrowserControl_ItemsEnumerated(object sender, EventArgs e)
        {
            AppContext.TraceDebug("Firing of ExplorerBrowserControl_ItemsEnumerated event.");

            this.explorerBrowser_itemsChangedEvent.Set();
            this.explorerBrowser_selectionChangedEvent.Set();
        }

        #endregion =============================================================================================================

        #region ElNavigableDockContent implementation ==========================================================================

        public override bool CanGoBack()
        {
            return this.explorerBrowserControl.History.CanNavigateBackward;
        }

        public override void GoBack()
        {
            this.explorerBrowserControl.History.NavigateLog(
                Components.Controls.ExplorerBrowserControl.NavigationLogDirection.Backward);
        }

        public override bool CanGoForward()
        {
            return this.explorerBrowserControl.History.CanNavigateForward;
        }

        public override void GoForward()
        {
            this.explorerBrowserControl.History.NavigateLog(
                Components.Controls.ExplorerBrowserControl.NavigationLogDirection.Forward);
        }


        public override string CurrentLocation { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }


        public override event EventHandler NavigationOptionsChanged;

        public virtual void OnNavigationOptionsChanged(EventArgs args)
        {
            this.NavigationOptionsChanged?.Invoke(this, args);
        }

        #endregion =============================================================================================================


        #region IElClipboardConsumer interface implementation ==================================================================

        public ElClipboardAbilities GetClipboardAbilities()
        {
            // TODO: Check for selection. No selection => No Abilities!
            return (ElClipboardAbilities.CanCopy | ElClipboardAbilities.CanCut);
        }

        public event EventHandler ClipboardAbilitiesChanged;


        /// <summary>
        /// Cut selected files, if any, to the clipboard.
        /// <seealso href="https://docs.microsoft.com/en-us/windows/desktop/shell/dragdrop"/>
        /// </summary>
        public void CutToClipboard()
        {
            this.PlaceFileDropListOnClipboard(DragDropEffects.Move);
        }

        /// <summary>
        /// Copy selected files, if any, to the clipboard.
        /// <seealso href="https://docs.microsoft.com/en-us/windows/desktop/shell/dragdrop"/>
        /// </summary>
        public void CopyToClipboard()
        {
            this.PlaceFileDropListOnClipboard(DragDropEffects.Copy);
        }

        private void PlaceFileDropListOnClipboard(DragDropEffects dropEffect)
        {
            AppContext.TraceScope();

            if (!(dropEffect.HasFlag(DragDropEffects.Copy) || dropEffect.HasFlag(DragDropEffects.Move)))
                throw new ArgumentException("Invalid DragDropEffect: Neither Copy nor Move flag is set.");

            if (dropEffect.HasFlag(DragDropEffects.Copy) && dropEffect.HasFlag(DragDropEffects.Move))
                throw new ArgumentException("Invalid DragDropEffect: Both Copy and Move flag are set.");

            // TODO: IFolderView can return a DataObject, too: https://docs.microsoft.com/en-us/windows/desktop/api/shobjidl_core/nf-shobjidl_core-ifolderview-items

            // Get collection of selected items, return if empty cause nothing to do then
            var selItems = this.explorerBrowserControl.SelectedItems;

            if (selItems.Count < 1)
                return;

            // Build file drop list
            StringCollection scFileDropList = new StringCollection();
            foreach (var selectedItem in selItems)
            {
                scFileDropList.Add(selectedItem.ParsingName);
            }

            // Build the data object, including the DropEffect, and place it on the clipboard
            DataObject dataObject = new DataObject();
            byte[] baDropEffect = new byte[] { (byte)dropEffect, 0, 0, 0 };
            MemoryStream msDropEffect = new MemoryStream();
            msDropEffect.Write(baDropEffect, 0, baDropEffect.Length);

            dataObject.SetFileDropList(scFileDropList);
            dataObject.SetData("Preferred DropEffect", msDropEffect);       // TODO: Use Vanaras constant

            //Clipboard.Clear();        // TODO: Do we have to call Clear before placing data on the clipboard?
            Clipboard.SetDataObject(dataObject, true);
        }

        public void GetSupportedClipboardPasteTypes()
        {
            // TODO: Not implemented yet
            throw new NotImplementedException();
        }

        public bool CanPasteFromClipboard()
        {
            if (Clipboard.ContainsFileDropList())
                return true;

            return false;
        }

        public void PasteFromClipboard()
        {
            AppContext.TraceScope();

            // Check whether clipboard contains any data object
            if (!ElClipboard.ContainsData())
            {
                AppContext.TraceWarning("No clipboard data present.");

                return;
            }

            // Check whether the clipboard contains a data format we can handle
            if (!this.CanPasteFromClipboard())
            {
                AppContext.TraceWarning("Incompatible clipboard data format present.");

                return;
            }

            // TODO: var ShellIDList = Clipboard.GetData("CFSTR_SHELLIDLIST");      // 31.10.18: We're looking for CFSTR_SHELLIDLIST or CFSTR_SHELLIDLISTOFFSET

            if (Clipboard.ContainsFileDropList())
            {
                // Determine the DragDropEffect of the current clipboard object and perform paste operation
                this.PasteFileDropListFromClipboard(ElClipboard.EvaluateDropEffect());
            }
            else
                throw new NotImplementedException("Clipboard format not supported: " + Clipboard.GetDataObject().GetFormats().ToString());
        }

        /// <summary>
        /// Paste FileDropList from the clipboard to this ShellBrowser's current folder.
        /// </summary>
        /// <param name="dropEffect">Either DragDropEffects.Copy or DragDropEffects.Move are allowed. Additional flags will be ignored.</param>
        /// <param name="operationFlags">Additional operation flags. Default is ShellFileOperations.OperationFlags.AllowUndo.</param>
        private void PasteFileDropListFromClipboard(DragDropEffects dropEffect, ShellFileOperations.OperationFlags operationFlags = ShellFileOperations.OperationFlags.AllowUndo)
        {
            AppContext.TraceScope();

            if (!(dropEffect.HasFlag(DragDropEffects.Copy) || dropEffect.HasFlag(DragDropEffects.Move)))
                throw new ArgumentException("Invalid DragDropEffect: Neither Copy nor Move flag is set.");

            if (dropEffect.HasFlag(DragDropEffects.Copy) && dropEffect.HasFlag(DragDropEffects.Move))
                throw new ArgumentException("Invalid DragDropEffect: Both Copy and Move flag are set.");

            // Get file drop list, return if empty cause nothing to do then
            var fileDropList = Clipboard.GetFileDropList();
            if (fileDropList.Count < 1)
                return;

            // Get the target folder
            string strTargetFolder = this.explorerBrowserControl.CurrentLocation;

            /* TODO:    When files are dropped to their source folder add RenameOnCollision to OperationFlags
             * WARNING: Only if files from the same folder are inserted again, then "RenameOnCollision" is needed when pasting.
             *          Otherwise, files of the same name will be overwritten (on request?)!
             **/

            using (var shTargetFolder = new ShellFolder(strTargetFolder))
            {
                using (var shFileOperation = new ShellFileOperations(this))
                {
                    shFileOperation.Options = operationFlags;

                    foreach (var strFullPathName in fileDropList)
                    {
                        if(dropEffect.HasFlag(DragDropEffects.Move))
                            shFileOperation.QueueMoveOperation(new ShellItem(strFullPathName), shTargetFolder);
                        else
                            shFileOperation.QueueCopyOperation(new ShellItem(strFullPathName), shTargetFolder);

                        // TODO: => QueueCopyOperation(IEnumerable[]);
                    }

                    shFileOperation.PerformOperations();

                    // TODO: Check if cancelled/aborted!, cause exception will be thrown: "HRESULT: 0x80270000"	System.Runtime.InteropServices.COMException
                    // https://www.hresult.info/FACILITY_SHELL/0x80270000 => COPYENGINE_E_USER_CANCELLED
                    // => ShellFileOpEventArgs.RESULT

                }
            }
        }


        //
        //
        //
        //
        //        // 31.10. 21.15: https://stackoverflow.com/questions/2077981/cut-files-to-clipboard-in-c-sharp
        //        private void CmdClipboardCopy_ExecuteEvent(object sender, Sunburst.WindowsForms.Ribbon.Controls.Events.ExecuteEventArgs e)
        //        {
        //            AppContext.TraceScope();
        //
        //            //11.11.18:
        //
        //            //common.Interop.WinShell.IShellFolder desktop = common.Interop.WinShell.DesktopFolder.Get();
        //
        //
        //
        //            //StringBuilder itemsText = new StringBuilder();
        //
        //            //foreach (ShellObject item in explorerBrowser.SelectedItems)
        //            //{
        //            //    if (item != null)
        //            //        itemsText.AppendLine("\tItem = " + item.GetDisplayName(DisplayNameType.Default));
        //            //}
        //
        //            //this.selectedItemsTextBox.Text = itemsText.ToString();
        //            //this.itemsTabControl.TabPages[1].Text = "Selected Items (Count=" + explorerBrowser.SelectedItems.Count.ToString() + ")";
        //
        //
        //            // https://docs.microsoft.com/en-us/dotnet/api/system.windows.clipboard.setfiledroplist?view=netframework-4.7.2
        //
        //            /* Wichtig:https://docs.microsoft.com/en-us/windows/desktop/shell/dragdrop
        //             * Clipboard Data Transfers
        //                The Clipboard is the simplest way to transfer Shell data. The basic procedure is similar to standard Clipboard data transfers.
        //                However, because you are transferring a pointer to a data object, not the data itself, you must use the OLE clipboard API instead
        //                of the standard clipboard API. The following procedure outlines how to use the OLE clipboard API to transfer Shell data with the Clipboard:
        //
        //                * The data source creates a data object to contain the data.
        //                * The data source calls OleSetClipboard, which places a pointer to the data object's IDataObject interface on the Clipboard.
        //                * The target calls OleGetClipboard to retrieve the pointer to the data object's IDataObject interface.
        //                * The target extracts the data by calling the IDataObject::GetData method.
        //                * With some Shell data transfers, the target might also need to call the data object's IDataObject::SetData method to provide
        //                  feedback to the data object on the outcome of the data transfer. See Handling Optimized Move Operations for an example of this type of operation.
        //             */
        //
        //
        //        }

        #endregion =============================================================================================================

    }
}
