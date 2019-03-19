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
using WeifenLuo.WinFormsUI.Docking;

namespace electrifier.Core.Forms
{
    public partial class Electrifier
    {
        // => https://github.com/dahall/Vanara/blob/master/PInvoke/Shell32/Clipboard.cs ShellClipboardFormat
        //public const string CFStr_Filename = "FileNameW";
        //public const string CFStr_PreferredDropEffect = "Preferred DropEffect";
        //public const string CFStr_ShellIdList = "Shell IDList Array";
        //public const string CFStr_ShellIdListOffset = "Shell Object Offsets";

        /// <summary>
        /// TODO: https://docs.microsoft.com/de-de/dotnet/api/system.windows.dataobject?view=netframework-4.7.2
        /// </summary>
        /// <param name="activeContent"></param>
        internal void Clipboard_ActiveDockContentChanged(IDockContent activeContent)
        {
            // Check for ClipboardAbilities
            ElClipboardAbilities clipboardAbilities = ElClipboardAbilities.None;

            if (activeContent is IElClipboardConsumer clipboardConsumer)
                clipboardAbilities = clipboardConsumer.GetClipboardAbilities();

            // Update ribbon command button state
            this.cmdBtnClipboardCut.Enabled = clipboardAbilities.HasFlag(ElClipboardAbilities.CanCut);
            this.cmdBtnClipboardCopy.Enabled = clipboardAbilities.HasFlag(ElClipboardAbilities.CanCopy);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal void CmdClipboardPaste_ExecuteEvent(object sender, Sunburst.WindowsForms.Ribbon.Controls.Events.ExecuteEventArgs e)
        {
            // Only IElClipboardConsumer can paste data from clipboard
            if (this.dpnDockPanel.ActiveContent is IElClipboardConsumer clipboardConsumer)
            {
                if (clipboardConsumer.CanPasteFromClipboard())
                    clipboardConsumer.PasteFromClipboard();
            }
        }
    }
}
