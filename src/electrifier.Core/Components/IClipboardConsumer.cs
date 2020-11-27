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


namespace electrifier.Core.Components
{
    [Flags]
    public enum ClipboardAbilities
    {
        None = 0x0,
        CanCut = 0x1,
        CanCopy = 0x2,
    }

    public interface IClipboardConsumer
    {
        // Cut & Copy

        ClipboardAbilities GetClipboardAbilities();
        /// <summary>
        /// Fires when the clipboard abilities (i.e. <see cref="ClipboardAbilities.CanCut"/> and / or
        /// <see cref="ClipboardAbilities.CanCopy"/>) changed, e.g. after selection has been changed.
        /// </summary>
        event EventHandler<ClipboardAbilitiesChangedEventArgs> ClipboardAbilitiesChanged;

        void CutToClipboard();
        void CopyToClipboard();

        // Paste

        /// <summary>
        /// Returns the possibly paste types, the clipboard consumer can handle generally, i.e. without looking at the current state of content or selection.
        /// </summary>
        void GetSupportedClipboardPasteTypes();

        /// <summary>
        /// CanPaste is invoked by INavigationHost when the clipboard content has changed.
        /// </summary>
        bool CanPasteFromClipboard(); // TODO: Type of clipboard content

        ///// <summary>
        ///// CanPasteFromClipboardChanged is invoked by IClipboardConsumer when CanPasteFromClipboard changed, e.g. after navigation to a new destination folder has completed.
        ///// </summary>
        //event EventHandler CanPasteFromClipboardChanged;

        // TODO: Type of clipboard content => GetClipboardPasteTypes();
        void PasteFromClipboard();  // TODO: The clipboard content
    }

    #region EventArgs =========================================================================================================

    /// <summary>
    /// The event argument for the <see cref="ClipboardAbilitiesChanged"/> event holds the new, current
    /// <see cref="ClipboardAbilities"/> of this <see cref="ShellBrowserDockContent"/>.
    /// </summary>
    public class ClipboardAbilitiesChangedEventArgs : EventArgs                 // TODO: Move into interface in future revisions of C#/VS
    {
        public ClipboardAbilitiesChangedEventArgs(ClipboardAbilities clipboardAbilities)
            => this.NewClipboardAbilities = clipboardAbilities;

        /// <summary>The new <see cref="ClipboardAbilities"/> of this <see cref="ShellBrowserDockContent"/>.</summary>
        public ClipboardAbilities NewClipboardAbilities { get; }
    }

    #endregion ================================================================================================================

}
