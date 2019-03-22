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
    public enum ElClipboardAbilities
    {
        None = 0x0,
        CanCut = 0x1,
        CanCopy = 0x2,
    }

    interface IElClipboardConsumer
    {
        // Cut & Copy

        ElClipboardAbilities GetClipboardAbilities();
        /// <summary>
        /// ClipboardAbilitiesChanged is invoked by IElClipboardConsumer when CanCut or CanCopy have changed, e.g. after selection changed.
        /// </summary>
        event EventHandler ClipboardAbilitiesChanged; // TODO: CanCut/CopyChanged => ClipboardAbility

        void CutToClipboard();
        void CopyToClipboard();

        // Paste

        /// <summary>
        /// Returns the possibly paste types, the clipboard consumer can handle generally, i.e. without looking at the current state of content or selection.
        /// </summary>
        void GetSupportedClipboardPasteTypes();

        /// <summary>
        /// CanPaste is invoked by IElNavigationHost when the clipboard content has changed.
        /// </summary>
        bool CanPasteFromClipboard(); // TODO: Type of clipboard content

        ///// <summary>
        ///// CanPasteFromClipboardChanged is invoked by IElClipboardConsumer when CanPasteFromClipboard changed, e.g. after navigation to a new destination folder has completed.
        ///// </summary>
        //event EventHandler CanPasteFromClipboardChanged;

        // TODO: Type of clipboard content => GetClipboardPasteTypes();
        void PasteFromClipboard();  // TODO: The clipboard content
    }
}
