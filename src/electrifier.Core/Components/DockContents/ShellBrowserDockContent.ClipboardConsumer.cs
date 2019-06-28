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
using System.Windows.Forms;

using Vanara.PInvoke;
using Vanara.Windows.Shell;

using electrifier.Core.WindowsShell;

namespace electrifier.Core.Components.DockContents
{
    /// <summary>
    /// ShellBrowserDockContent is electrifier's wrapper class for ExplorerBrowser Control.
    /// 
    /// This partial class file contains the <see cref="IElClipboardConsumer"/> implementation.
    /// </summary>
    public partial class ShellBrowserDockContent
      : IElClipboardConsumer
    {
        #region Fields ========================================================================================================

        private ElClipboardAbilities currentClipboardAbilities = ElClipboardAbilities.None;

        #endregion Fields =====================================================================================================

        #region Published Events ==============================================================================================

        /// <summary>
        /// Fires when the clipboard abilities have changed.
        /// </summary>
        public event EventHandler ClipboardAbilitiesChanged;        // TODO: EventArgs

        /// <summary>
        /// Raises the <see cref="ClipboardAbilitiesChanged"/> event.
        /// </summary>
        protected internal virtual void OnClipboardAbilitiesChanged() => this.ClipboardAbilitiesChanged?.Invoke(this, EventArgs.Empty);

        #endregion Published Events ===========================================================================================

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        protected void ElClipboardConsumer_SelectionChanged(object sender, EventArgs args)
        {
            var clipAbilities = this.GetClipboardAbilities();

            if (this.currentClipboardAbilities != clipAbilities)
            {
                this.currentClipboardAbilities = clipAbilities;

                this.OnClipboardAbilitiesChanged();
            }
        }

        public ElClipboardAbilities GetClipboardAbilities()
        {
            var itemCount = this.explorerBrowserControl.GetItemCount(Shell32.SVGIO.SVGIO_SELECTION);

            return ((itemCount > 0) ? (ElClipboardAbilities.CanCopy | ElClipboardAbilities.CanCut) : ElClipboardAbilities.None);
        }

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
        /// <param name="operationFlags">Additional operation flags. Defaults to AllowUndo.</param>
        private void PasteFileDropListFromClipboard(
            DragDropEffects dropEffect,
            ShellFileOperations.OperationFlags operationFlags = ShellFileOperations.OperationFlags.AllowUndo)
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
                        if (dropEffect.HasFlag(DragDropEffects.Move))
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


        //        // 31.10. 21.15: https://stackoverflow.com/questions/2077981/cut-files-to-clipboard-in-c-sharp
        //        private void CmdClipboardCopy_ExecuteEvent(object sender, Sunburst.WindowsForms.Ribbon.Controls.Events.ExecuteEventArgs e)
        //        {
        //            // https://docs.microsoft.com/en-us/dotnet/api/system.windows.clipboard.setfiledroplist?view=netframework-4.7.2
        //
        //            /* Important: https://docs.microsoft.com/en-us/windows/desktop/shell/dragdrop
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
    }
}
