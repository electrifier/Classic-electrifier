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
using RibbonLib;
using RibbonLib.Controls;
using RibbonLib.Controls.Events;
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
//    [TypeDescriptionProvider(typeof(AbstractControlDescriptionProvider<ShellFolderDockContent, DockContent>))]
    public class ShellFolderDockContent :
        DockContent, // DockPanel, DockPane?
        IRibbonConsumer,
        IClipboardConsumer
    {
        #region Fields ========================================================================================================

        protected const string persistParamURI = @"URI=";
        protected const string persistParamViewMode = @"ViewMode=";

        #endregion ============================================================================================================

        #region Properties ====================================================================================================

        //public override string CurrentLocation { get => "TEST"; set => this.shellNamespaceTree.Text = value; }
        protected string currentLocation;
        public string CurrentLocation      // TODO: CurrentLocation als Object, dann für jede Art von DockContent eine explizite Konvertierung vornehmen!
        {
            get => this.currentLocation;
            set => this.currentLocation = value;
        }

        protected Shell32.PIDL CurrentFolderPidl;

        public ClipboardSelection Selection { get; }

        protected Color backColor;
        public Color BackColor
        {
            get => this.backColor;
            set
            {
                this.backColor = value;
                //this.splitter.BackColor = value;
                // TODO: Set ShellBrowser.BackColor; as soon as Vanara got an Update and has IListView-Interface
            }
        }

        #endregion ============================================================================================================


        public ShellFolderDockContent(INavigationHost navigationHost, string persistString = null)
          : base(/*navigationHost*/)
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


            this.ExplorerBrowser.Navigated += this.ExplorerBrowser_Navigated;
            //            this.ExplorerBrowser.ItemsChanged += ExplorerBrowser_ItemsChanged;

            this.Selection.PropertyChanged += this.Selection_PropertyChanged;

            if (this.CurrentFolderPidl != null)
                this.ExplorerBrowser.Navigate(new ShellItem(this.CurrentFolderPidl));
            else
                this.ExplorerBrowser.Navigate(ShellFolder.Desktop);

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

                    break;
                case nameof(Selection.CurrentClipboardAbilities):
                    this.currentClipboardAbilities = this.Selection.CurrentClipboardAbilities;
                    this.ClipboardAbilitiesChanged?.Invoke(this, new ClipboardAbilitiesChangedEventArgs(this.currentClipboardAbilities));
                    break;
                default:
                    throw new ArgumentException(nameof(e));
            }
        }

        private void ExplorerBrowser_Navigated(object sender, ExplorerBrowser.NavigatedEventArgs e)
        {
            ShellItem newLocation = e.NewLocation;

            if (null != newLocation)
            {
                Shell32.PIDL newCurrentFolderPIDL = newLocation.PIDL;
                this.CurrentLocation = this.Text =
                    newLocation.GetDisplayName(ShellItemDisplayString.DesktopAbsoluteEditing);

                this.CurrentFolderPidl = newCurrentFolderPIDL;
            }

            //this.OnNavigationOptionsChanged(EventArgs.Empty);
        }


//        public override bool CanGoBack => this.ExplorerBrowser.History.CanNavigateBackward;

        //public override void GoBack()
        //{
        //    this.ExplorerBrowser.NavigateFromHistory(NavigationLogDirection.Backward);
        //    this.OnNavigationOptionsChanged(EventArgs.Empty);
        //}

        //public override bool CanGoForward => this.ExplorerBrowser.History.CanNavigateForward;

        //public override void GoForward()
        //{
        //    this.ExplorerBrowser.NavigateFromHistory(NavigationLogDirection.Forward);
        //    this.OnNavigationOptionsChanged(EventArgs.Empty);
        //}



        //public override ElNavigableTargetItemCollection<ElNavigableTargetNavigationLogIndex> HistoryItems { get; }

        //public override event EventHandler NavigationOptionsChanged;

        //public virtual void OnNavigationOptionsChanged(EventArgs args) => this.NavigationOptionsChanged?.Invoke(this, args);

        public void SelectAll() => this.ExplorerBrowser.SelectAll();

        public void UnselectAll() => this.ExplorerBrowser.UnselectAll();

        //public override bool HasShellFolderViewMode => true;

        //public override Shell32.FOLDERVIEWMODE ShellFolderViewMode
        //{
        //    get => (Shell32.FOLDERVIEWMODE)this.ExplorerBrowser.ViewMode;
        //    set => this.ExplorerBrowser.ViewMode = (ExplorerBrowserViewMode)value;
        //}

        public bool SetSelectionState(ShellItem shellItem, Shell32.SVSIF selectionState = Shell32.SVSIF.SVSI_SELECT)
        { System.Windows.Forms.MessageBox.Show("Not implemented yet."); return false; }
        //=> this.ShellBrowser.SetSelectionState(shellItem, selectionState); 

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
            public ExplorerBrowser ExplorerBrowser { get; }

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

            public ClipboardSelection(ShellFolderDockContent owner)
            {
                this.Owner = owner ?? throw new ArgumentNullException(nameof(owner));
                this.ExplorerBrowser = owner.ExplorerBrowser;

                this.ExplorerBrowser.SelectionChanged += this.SelectionChanged;
            }

            public event PropertyChangedEventHandler PropertyChanged;

            public void SelectionChanged(object sender, EventArgs e)
            {
                Debug.Assert(sender == this.ExplorerBrowser);

                this.Count = this.ExplorerBrowser.SelectedItems.Count;
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
                var selItems = this.ExplorerBrowser.SelectedItems;

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

        #region IRibbonConsumerDockPanel ======================================================================================

        public RibbonItems RibbonItems { get; private set; }

        RibbonTabBinding TabHome;
        RibbonButtonBinding BtnClipboardCut;
        RibbonButtonBinding BtnClipboardCopy;

        BaseRibbonControlBinding<BaseRibbonControl>[] ControlBindings;


        public BaseRibbonControlBinding<BaseRibbonControl>[] InitializeRibbonBinding(RibbonItems ribbonItems)
        {
            this.RibbonItems = ribbonItems ?? throw new ArgumentNullException(nameof(ribbonItems));

            this.ControlBindings = new BaseRibbonControlBinding<BaseRibbonControl>[]
            {
                this.TabHome = new RibbonTabBinding(ribbonItems.TabHome),
                this.BtnClipboardCut = new RibbonButtonBinding(ribbonItems.BtnClipboardCut, this.testribbonexecuter),
                this.BtnClipboardCopy = new RibbonButtonBinding(ribbonItems.BtnClipboardCopy, this.testribbonexecuter),
            };


            return this.ControlBindings;
        }

        public void ActivateRibbonState()
        {

            //foreach (var item in this.RibbonStateMap.RibbonStates)
            //{//item.Key.CommandID;}

        }

        public void DeactivateRibbonState()
        {

        }

        public void testribbonexecuter(object sender, ExecuteEventArgs args)
        {

        }


        #endregion ============================================================================================================


        #region Component Designer generated code =============================================================================

        private void InitializeComponent()
        {
            this.ExplorerBrowser = new Vanara.Windows.Forms.ExplorerBrowser();
            this.SuspendLayout();
            // 
            // ExplorerBrowser
            // 
            this.ExplorerBrowser.ContentFlags = ((Vanara.Windows.Forms.ExplorerBrowserContentSectionOptions)((Vanara.Windows.Forms.ExplorerBrowserContentSectionOptions.NoWebView | Vanara.Windows.Forms.ExplorerBrowserContentSectionOptions.UseSearchFolder)));
            this.ExplorerBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ExplorerBrowser.Location = new System.Drawing.Point(0, 0);
            this.ExplorerBrowser.Name = "explorerBrowser";
            this.ExplorerBrowser.NavigationFlags = Vanara.Windows.Forms.ExplorerBrowserNavigateOptions.ShowFrames;
            this.ExplorerBrowser.Size = new System.Drawing.Size(1161, 876);
            this.ExplorerBrowser.TabIndex = 5;
            // 
            // ShellFolderDockContent
            // 
            this.ClientSize = new System.Drawing.Size(1161, 876);
            this.Controls.Add(this.ExplorerBrowser);
            this.Name = "ShellFolderDockContent";
            this.Load += new System.EventHandler(this.ShellFolderDockContent_Load);
            this.ResumeLayout(false);

        }

        private ExplorerBrowser ExplorerBrowser;




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
