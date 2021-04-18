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

using electrifier.Core.Components.Controls;
using electrifier.Core.WindowsShell;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq.Expressions;
using System.Text;
using System.Windows.Forms;
using Vanara.PInvoke;
using Vanara.Windows.Forms;
using Vanara.Windows.Shell;
using WeifenLuo.WinFormsUI.Docking;

namespace electrifier.Core.Components.DockContents
{
    [TypeDescriptionProvider(typeof(AbstractControlDescriptionProvider<ShellFolderDockContent, DockContent>))]
    public class ShellFolderDockContent
      : NavigableDockContent
      , IClipboardConsumer
    {
        #region Fields ========================================================================================================

        private System.Windows.Forms.Splitter splitter;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel itemCountStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel selectionStatusLabel;
        private ShellNamespaceTreeControl shellNamespaceTree;

        protected const string persistParamURI = @"URI=";
        protected const string persistParamViewMode = @"ViewMode=";

        #endregion ============================================================================================================

        #region Properties ====================================================================================================

        //public override string CurrentLocation { get => "TEST"; set => this.shellNamespaceTree.Text = value; }
        protected string currentLocation;
        public override string CurrentLocation      // TODO: CurrentLocation als Object, dann für jede Art von DockContent eine explizite Konvertierung vornehmen!
        {
            get => this.currentLocation;
            set => this.currentLocation = value;
        }

        protected Shell32.PIDL CurrentFolderPidl;

        public ShellBrowser ShellBrowser { get; private set; }

        public IReadOnlyList<ShellItem> ShellItems => this.ShellBrowser.Items;



        public ClipboardSelection Selection { get; }

        protected Color backColor;
        public override Color BackColor
        {
            get => this.backColor;
            set
            {
                this.backColor = value;
                this.splitter.BackColor = value;
                // TODO: Set ShellBrowser.BackColor; as soon as Vanara got an Update and has IListView-Interface
            }
        }

        #endregion ============================================================================================================


        public ShellFolderDockContent(INavigationHost navigationHost, string persistString = null)
          : base(navigationHost)
        {
            this.InitializeComponent();

            this.Selection = new ClipboardSelection(this);

            this.BackColor = Color.FromArgb(250, 250, 250);

            //this.splitter.Cursor = Cursors.PanWest;                                               // TODO: For "Hide TreeView-Button"
            //this.shellNamespaceTree.BackColor = System.Drawing.Color.FromArgb(0xFA, 0xFA, 0xFA);  // TODO: This doesn't work, however, set to window background!



            this.EvaluatePersistString(persistString);      // TODO: Error-Handling!
        }

        private void ShellFolderDockContent_Load(object sender, EventArgs e)
        {
            this.Icon = Properties.Resources.ShellBrowserDockContent;

            this.shellNamespaceTree.RootItems.Add(new ShellFolder(@"shell:::{679f85cb-0220-4080-b29b-5540cc05aab6}"), false, false);        // TOOD: This is the "Quick Access"-folder, which is new to Windows 10
            this.shellNamespaceTree.RootItems.Add(new ShellFolder(Shell32.KNOWNFOLDERID.FOLDERID_OneDrive), false, false);
            this.shellNamespaceTree.RootItems.Add(new ShellFolder(Shell32.KNOWNFOLDERID.FOLDERID_ComputerFolder), false, false);
            this.shellNamespaceTree.RootItems.Add(new ShellFolder(Shell32.KNOWNFOLDERID.FOLDERID_NetworkFolder), false, false);

            this.ShellBrowser.Navigated += this.ShellBrowser_Navigated;
            this.ShellBrowser.ItemsChanged += this.ShellBrowser_ItemsChanged;

            this.Selection.PropertyChanged += this.Selection_PropertyChanged;

            if (this.CurrentFolderPidl != null)
                this.ShellBrowser.BrowseObject((IntPtr)this.CurrentFolderPidl, Shell32.SBSP.SBSP_ABSOLUTE);     // TODO: Overload BrowseObject in ShellFolder to accept a simple pidl without SBSP-Flags!
            else
                this.shellNamespaceTree.SelectedItem = this.shellNamespaceTree.RootItems[0];


            // Initialize status bar selection text
            this.Selection_PropertyChanged(this, new PropertyChangedEventArgs(nameof(Selection.Count)));
        }

        #region DockContent Persistence Overrides =============================================================================

        /// <summary>
        /// Override of WeifenLuo.WinFormsUI.Docking.DockContent.GetPersistString()
        /// </summary>
        /// <returns>The string describing persistence information. E.g. persistString = "ElShellBrowserDockContent URI=file:///C:/Users/tajbender/Desktop";</returns>
        protected override string GetPersistString()
        {
            var sb = new StringBuilder();
            string paramFmt = " {0}{1}";

            // Append class name as identifier
            sb.Append(nameof(ShellFolderDockContent));

            // If folder is a virtual folder, add suffix.
            string persistParamFolder = this.CurrentFolderPidl.ToString(Shell32.SIGDN.SIGDN_DESKTOPABSOLUTEPARSING);
            if (persistParamFolder.StartsWith(@"::"))
                persistParamFolder = @"shell:" + persistParamFolder;

            // Append URI of current location
            sb.AppendFormat(paramFmt,
                persistParamURI,
                ElShellTools.UrlCreateFromPath(persistParamFolder));

            // Append ViewMode
//            sb.AppendFormat(paramFmt, ShellFolderDockContent.persistParamViewMode, this.ViewMode);

            return sb.ToString();
        }

        /// <summary>
        /// Example: persistString = "ElShellBrowserDockContent URI=file:///C:/Users/tajbender/Desktop";
        /// </summary>
        /// <param name="persistString"></param>
        protected void EvaluatePersistString(string persistString)
        {
            try
            {
                if ((null != persistString) && (persistString.Trim().Length > ShellFolderDockContent.persistParamURI.Length))
                {
                    IEnumerable<string> args = ElShellTools.SplitArgumentString(persistString);
                    string strInitialNavigationTarget = default;
                    string strInitialViewMode = default;

                    foreach (string arg in args)
                    {
                        if (arg.StartsWith(ShellFolderDockContent.persistParamURI))
                        {

                            strInitialNavigationTarget = arg.Substring(ShellFolderDockContent.persistParamURI.Length);

                            if (!strInitialNavigationTarget.StartsWith(@"shell:"))
                                ElShellTools.PathCreateFromUrl(strInitialNavigationTarget);
                        }

                        if (arg.StartsWith(ShellFolderDockContent.persistParamViewMode))
                        {
                            strInitialViewMode = arg.Substring(ShellFolderDockContent.persistParamViewMode.Length);
                        }
                    }


                    // Finally, when all parameters have been parsed successfully, apply them
                    if (default != strInitialNavigationTarget)
                    {
                        using (ShellItem navigationTarget = new ShellFolder(strInitialNavigationTarget))
                        {
                            this.CurrentFolderPidl = new Shell32.PIDL((IntPtr)navigationTarget.PIDL, clone: true);
                        }
                    }
                        
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
                MessageBox.Show("ElShellBrowserDockContent.EvaluatePersistString: Error evaluating parameters"
                    + "\n\nParameters: '" + persistString + "'"
                    + "\n\nError description: '" + e.Message + "'"
                    + "\n\nResetting to default values.");
            }
        }

        #endregion DockContent Persistence Overrides ==========================================================================


        private void Selection_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Selection.Count):
                    this.selectionStatusLabel.Text = this.Selection.SelectionCountStatusBarText;
                    break;
                case nameof(Selection.CurrentClipboardAbilities):
                    this.currentClipboardAbilities = this.Selection.CurrentClipboardAbilities;
                    this.ClipboardAbilitiesChanged?.Invoke(this, new ClipboardAbilitiesChangedEventArgs(this.currentClipboardAbilities));
                    break;
                default:
                    throw new ArgumentException(nameof(e));
            }
        }

        /// <summary>
        /// Process <see cref="ShellBrowser.ItemsChanged"/> event.
        /// </summary>
        private void ShellBrowser_ItemsChanged(object sender, EventArgs e)
        {
            Debug.Assert(sender == this.ShellBrowser);

            this.itemCountStatusLabel.Text = this.ShellBrowser.Items.Count.ToString() + " items";
        }

        /// <summary>
        /// Process <see cref="ShellNamespaceTreeControl.AfterSelect"/> event.
        /// </summary>
        private void ShellNamespaceTree_AfterSelect(object sender, EventArgs e)
        {
            ShellItem shellItem = this.shellNamespaceTree.SelectedItem;

            if (null == shellItem)
                throw new NullReferenceException(nameof(shellItem));


            // TODO: 18/01/21 Remove CurrentFolder in ShellBrowser
            //this.ShellBrowser.CurrentFolder = (ShellFolder)shellItem;
            this.ShellBrowser.BrowseObject((IntPtr)shellItem.PIDL, Shell32.SBSP.SBSP_ABSOLUTE);

            //if (!shellItem.PIDL.Equals(this.shellBrowser.CurrentFolder))
            //{
            //    this.shellBrowser.CurrentFolder = (ShellFolder)shellItem;
            //}


            //if (null != shellItem)
            //{
            //    if (null == this.shellBrowser.CurrentFolder || !this.shellBrowser.CurrentFolder.PIDL.Equals(shellItem.PIDL))
            //        this.shellBrowser.CurrentFolder = (ShellFolder)shellItem;
            //}
        }



        /// <summary>
        /// TODO: This is very experimental, and only works if the new folder navigated to is an direct successor node
        ///       to the currently selected node.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShellBrowser_Navigated(object sender, ShellBrowserNavigatedEventArgs e)
        {
            ShellFolder newCurrentFolder = e.CurrentFolder;

            if (null != newCurrentFolder)
            {
                Shell32.PIDL newCurrentFolderPIDL = newCurrentFolder.PIDL;
                ShellItem selectedTreeItem = this.shellNamespaceTree.SelectedItem;

                if ((selectedTreeItem != null) && (selectedTreeItem.PIDL.IsParentOf(newCurrentFolderPIDL, immediate: true)))
                    this.shellNamespaceTree.SelectedItem = newCurrentFolder;

                // TODO: BUG: Navigation to user folder, e.g. "Thorsten Jung", will "magically add" that folder to the tree
                //else
                //    this.shellNamespaceTree.SelectedItem = null;

                this.CurrentLocation = this.Text =
                    newCurrentFolder.GetDisplayName(ShellItemDisplayString.DesktopAbsoluteEditing);

                this.CurrentFolderPidl = newCurrentFolderPIDL;
            }

            this.OnNavigationOptionsChanged(EventArgs.Empty);
        }


        public override bool CanGoBack => this.ShellBrowser.History.CanSeekBackward;

        public override void GoBack()
        {
            this.ShellBrowser.NavigateBack();
            this.OnNavigationOptionsChanged(EventArgs.Empty);
        }

        public override bool CanGoForward => this.ShellBrowser.History.CanSeekForward;

        public override void GoForward()
        {
            this.ShellBrowser.NavigateForward();
            this.OnNavigationOptionsChanged(EventArgs.Empty);
        }



        public override ElNavigableTargetItemCollection<ElNavigableTargetNavigationLogIndex> HistoryItems { get; }

        public override event EventHandler NavigationOptionsChanged;

        public virtual void OnNavigationOptionsChanged(EventArgs args) => this.NavigationOptionsChanged?.Invoke(this, args);

        public void SelectAll() => this.ShellBrowser.SelectAll();

        public void UnselectAll() => this.ShellBrowser.UnselectAll();

        public override bool HasShellFolderViewMode => true;

        public override Shell32.FOLDERVIEWMODE ShellFolderViewMode
        {
            get => (Shell32.FOLDERVIEWMODE)this.ShellBrowser.ViewMode;
            set => this.ShellBrowser.ViewMode = (ShellBrowserViewMode)value;
        }

        public bool SetSelectionState(ShellItem shellItem, Shell32.SVSIF selectionState = Shell32.SVSIF.SVSI_SELECT)
            => this.ShellBrowser.SetSelectionState(shellItem, selectionState);

        #region IClipboardConsumer ============================================================================================

        #region Published Events ==============================================================================================

        /// <summary>
        /// Fires when the clipboard abilities have changed.
        /// </summary>
        public event EventHandler<ClipboardAbilitiesChangedEventArgs> ClipboardAbilitiesChanged;

        /// <summary>
        /// Raises the <see cref="IClipboardConsumer.ClipboardAbilitiesChanged"/> event.
        /// </summary>
        //protected internal virtual void OnClipboardAbilitiesChanged(ClipboardAbilities clipboardAbilities)
        //{
        //    this.ClipboardAbilitiesChanged?.Invoke(this, new ClipboardAbilitiesChangedEventArgs(clipboardAbilities));
        //}

        #endregion ============================================================================================================

        private ClipboardAbilities currentClipboardAbilities = ClipboardAbilities.None;

        public ClipboardAbilities GetClipboardAbilities() => this.Selection.CurrentClipboardAbilities;

        public void CopyToClipboard() => this.Selection.SetClipboardDataObject(DragDropEffects.Copy);

        public void CutToClipboard() => this.Selection.SetClipboardDataObject(DragDropEffects.Move);

        public void GetSupportedClipboardPasteTypes()
        {
            // TODO: On virtual folders like 'This PC', we can't paste files... => Ask Shell if target folder can accept files

            // TODO: Not implemented yet
            throw new NotImplementedException();
        }

        // TODO: On virtual folders like 'This PC', we can't paste files... => Ask Shell if target folder can accept files
        public bool CanPasteFromClipboard() => Clipboard.ContainsFileDropList();

        public void PasteFromClipboard()
        {
            AppContext.TraceScope();

            // Check whether clipboard contains any data object
            if (!ClipboardState.ContainsData)
            {
                AppContext.TraceWarning("No clipboard data present.");

                return;
            }

            // Check whether the clipboard contains a data format we can handle
            if (!CanPasteFromClipboard())
            {
                AppContext.TraceWarning("Incompatible clipboard data format present.");

                return;
            }

            // TODO: var ShellIDList = Clipboard.GetData("CFSTR_SHELLIDLIST");      // 31.10.18: We're looking for CFSTR_SHELLIDLIST or CFSTR_SHELLIDLISTOFFSET

            if (Clipboard.ContainsFileDropList())
            {
                // Determine the DragDropEffect of the current clipboard object and perform paste operation
                this.Selection.PasteFileDropListFromClipboard(ClipboardState.CurrentDropEffect);
            }
            else
                throw new NotImplementedException("Clipboard format not supported: " + Clipboard.GetDataObject().GetFormats().ToString());
        }

        #endregion ============================================================================================================


        /// <summary>
        /// TODO: Convert to generic class, which is able to hold all clipboard-types
        /// </summary>
        public class ClipboardSelection
          : INotifyPropertyChanged
        {
            public ShellFolderDockContent Owner { get; }
            public ShellBrowser ShellBrowser { get; }

            private int count;

            public int Count
            {
                get { return this.count; }
                set { PropertyChanged.ChangeAndNotify(ref this.count, value, () => Count); }
            }

            private ClipboardAbilities currentClipboardAbilities = ClipboardAbilities.None;

            public ClipboardAbilities CurrentClipboardAbilities
            {
                get { return this.currentClipboardAbilities; }
                set { PropertyChanged.ChangeAndNotify(ref this.currentClipboardAbilities, value, () => CurrentClipboardAbilities); }
            }

            public string SelectionCountStatusBarText
            {
                /// <summary>
                /// TODO: Use recursive pattern here
                /// </summary>
                get
                {
                    //return selectionCount switch
                    //{
                    //    0 => "No Selection",
                    //    1 => "One Item selected",
                    //    _ => $"{selectionCount} items selected",
                    //};
                    switch (this.Count)
                    {
                        case 0: return "No Selection";
                        case 1: return "One Item selected";
                        default: return $"{count} items selected";
                    }
                }
            }

            public ClipboardSelection(ShellFolderDockContent owner)
            {
                this.Owner = owner ?? throw new ArgumentNullException(nameof(owner));
                this.ShellBrowser = owner.ShellBrowser;

                this.ShellBrowser.SelectionChanged += this.SelectionChanged;
            }

            public event PropertyChangedEventHandler PropertyChanged;

            public void SelectionChanged(object sender, EventArgs e)
            {
                Debug.Assert(sender == this.ShellBrowser);

                this.Count = this.ShellBrowser.SelectedItems.Count;
                this.CurrentClipboardAbilities = this.Count > 0 ?
                    (ClipboardAbilities.CanCopy | ClipboardAbilities.CanCut) : ClipboardAbilities.None;
            }

            public void SetClipboardDataObject(DragDropEffects dropEffect)
            {
                // TODO: Try / Catch
                // TODO: Extended DragDropEffects-struct with methods for checking flags
                if (!(dropEffect.HasFlag(DragDropEffects.Copy) || dropEffect.HasFlag(DragDropEffects.Move)))
                    throw new ArgumentException("Invalid DragDropEffect: Neither Copy nor Move flag is set.");

                if (dropEffect.HasFlag(DragDropEffects.Copy) && dropEffect.HasFlag(DragDropEffects.Move))
                    throw new ArgumentException("Invalid DragDropEffect: Both Copy and Move flag are set.");

                // TODO: IFolderView can return a DataObject, too: https://docs.microsoft.com/en-us/windows/desktop/api/shobjidl_core/nf-shobjidl_core-ifolderview-items

                // Get collection of selected items, return if empty cause nothing to do then
                var selItems = this.ShellBrowser.SelectedItems;

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

            /// <summary>
            /// Paste FileDropList from the clipboard to this ShellBrowser's current folder.
            /// </summary>
            /// <param name="dropEffect">Either DragDropEffects.Copy or DragDropEffects.Move are allowed. Additional flags will be ignored.</param>
            /// <param name="operationFlags">Additional operation flags. Defaults to AllowUndo.</param>
            public void PasteFileDropListFromClipboard(
                DragDropEffects dropEffect,
                ShellFileOperations.OperationFlags operationFlags = (ShellFileOperations.OperationFlags.AllowUndo | ShellFileOperations.OperationFlags.NoConfirmMkDir))
            {
                AppContext.TraceScope();

                if (!(dropEffect.HasFlag(DragDropEffects.Copy) || dropEffect.HasFlag(DragDropEffects.Move)))
                    throw new ArgumentException("Invalid DragDropEffect: Neither Copy nor Move flag is set.");

                if (dropEffect.HasFlag(DragDropEffects.Copy) && dropEffect.HasFlag(DragDropEffects.Move))
                    throw new ArgumentException("Invalid DragDropEffect: Both Copy and Move flag are set.");

                // Get file drop list, return if empty cause nothing to do then
                StringCollection fileDropList = Clipboard.GetFileDropList();
                if (fileDropList.Count < 1)
                    return;

                //operationFlags += ShellFileOperations.OperationFlags.WantMappingHandle // TODO 09/04/21: for Paste to the same folder

                /* TODO:    When files are dropped to their source folder add RenameOnCollision to OperationFlags
                 * WARNING: Only if files from the same folder are inserted again, then "RenameOnCollision" is needed when pasting.
                 *          Otherwise, files of the same name will be overwritten (on request?)!
                 *          
                 * See 'IFolderView2::IsMoveInSameFolder' for this!
                 **/

                // TODO: 09/04/21: Perform file transfer in background!
                using (var elShellFileOperations = new ElShellFileOperations(/* TODO: this */ null, operationFlags))  // TODO: 09/04/21 iwin32window handle!
                {
                    using (var destinationFolder = new ShellFolder(/*this.ShellBrowser.CurrentLocation*/ this.Owner.CurrentLocation))
                    {
                        foreach (var strFullPathName in fileDropList)
                        {
                            // TODO: => QueueCopyOperation(IEnumerable[]);
                            elShellFileOperations.QueueClipboardOperation(strFullPathName, destinationFolder, dropEffect);
                        }

                        elShellFileOperations.PerformOperations();
                    }
                }
            }

        }

        #region Component Designer generated code =============================================================================

        private void InitializeComponent()
        {
            this.shellNamespaceTree = new Vanara.Windows.Forms.ShellNamespaceTreeControl();
            this.splitter = new System.Windows.Forms.Splitter();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.itemCountStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.selectionStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.ShellBrowser = new electrifier.Core.Components.Controls.ShellBrowser();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // shellNamespaceTree
            // 
            this.shellNamespaceTree.Dock = System.Windows.Forms.DockStyle.Left;
            this.shellNamespaceTree.Location = new System.Drawing.Point(0, 0);
            this.shellNamespaceTree.Name = "shellNamespaceTree";
            this.shellNamespaceTree.Size = new System.Drawing.Size(320, 876);
            this.shellNamespaceTree.TabIndex = 1;
            this.shellNamespaceTree.AfterSelect += new System.EventHandler(this.ShellNamespaceTree_AfterSelect);
            // 
            // splitter
            // 
            this.splitter.Location = new System.Drawing.Point(320, 0);
            this.splitter.Name = "splitter";
            this.splitter.Size = new System.Drawing.Size(6, 876);
            this.splitter.TabIndex = 3;
            this.splitter.TabStop = false;
            // 
            // statusStrip
            // 
            this.statusStrip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.itemCountStatusLabel,
            this.selectionStatusLabel});
            this.statusStrip.Location = new System.Drawing.Point(326, 850);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(835, 26);
            this.statusStrip.SizingGrip = false;
            this.statusStrip.TabIndex = 4;
            this.statusStrip.Text = "statusStrip1";
            // 
            // itemCountStatusLabel
            // 
            this.itemCountStatusLabel.AutoSize = false;
            this.itemCountStatusLabel.Name = "itemCountStatusLabel";
            this.itemCountStatusLabel.Size = new System.Drawing.Size(100, 20);
            // 
            // selectionStatusLabel
            // 
            this.selectionStatusLabel.AutoSize = false;
            this.selectionStatusLabel.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.selectionStatusLabel.Name = "selectionStatusLabel";
            this.selectionStatusLabel.Size = new System.Drawing.Size(150, 20);
            // 
            // ShellBrowser
            // 
            this.ShellBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ShellBrowser.Location = new System.Drawing.Point(326, 0);
            this.ShellBrowser.Name = "ShellBrowser";
            this.ShellBrowser.Size = new System.Drawing.Size(835, 850);
            this.ShellBrowser.TabIndex = 2;
            this.ShellBrowser.Navigated += new System.EventHandler<electrifier.Core.Components.Controls.ShellBrowserNavigatedEventArgs>(this.ShellBrowser_Navigated);
            // 
            // ShellFolderDockContent
            // 
            this.ClientSize = new System.Drawing.Size(1161, 876);
            this.Controls.Add(this.ShellBrowser);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.splitter);
            this.Controls.Add(this.shellNamespaceTree);
            this.Name = "ShellFolderDockContent";
            this.Load += new System.EventHandler(this.ShellFolderDockContent_Load);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion ============================================================================================================
    }


    public static class PropertyChanged
    {
        /// <summary>
        /// Taken from <see href="https://www.wpftutorial.net/INotifyPropertyChanged.html"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handler"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="memberExpression"></param>
        /// <returns></returns>
        public static bool ChangeAndNotify<T>(
            this PropertyChangedEventHandler handler,
            ref T field,
            T value,
            Expression<Func<T>> memberExpression)
        {
            if (memberExpression == null)
                throw new ArgumentNullException(nameof(memberExpression));

            if (!(memberExpression.Body is MemberExpression body))
                throw new ArgumentException($"Lambda for {memberExpression} must return a property.");

            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;

            field = value;

            if (body.Expression is ConstantExpression vmExpression)
            {
                LambdaExpression lambda = Expression.Lambda(vmExpression);
                Delegate vmFunc = lambda.Compile();
                object sender = vmFunc.DynamicInvoke();

                handler?.Invoke(sender, new PropertyChangedEventArgs(body.Member.Name));
            }

            //field = value;
            return true;
        }
    }
}
