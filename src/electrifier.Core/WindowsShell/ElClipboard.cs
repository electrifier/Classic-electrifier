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

using System.Linq;
using System.Windows.Forms;

namespace electrifier.Core.WindowsShell
{
    static class ElClipboard
    {
        /// <summary>
        /// Determines the DragDropEffect of the current Clipboard object.
        /// </summary>
        /// <returns>Either DragDropEffects.Move or DragDropEffects.Copy if succeeded, DragDropEffects.None if validation failed.</returns>
        public static DragDropEffects EvaluateDropEffect()
        {
            var dropEffect = DragDropEffects.None;
            var objDropEffect = Clipboard.GetData(Vanara.PInvoke.Shell32.ShellClipboardFormat.CFSTR_PREFERREDDROPEFFECT);

            if ((objDropEffect is System.IO.MemoryStream msDropEffect) &&
                (msDropEffect.CanRead) &&
                (msDropEffect.Length > 0))
            {
                var baDropEffect = new byte[1];

                msDropEffect.Read(baDropEffect, 0, 1);

                dropEffect = (((baDropEffect[0] & (byte)DragDropEffects.Move) != 0) ? DragDropEffects.Move : DragDropEffects.Copy);
            }

            return dropEffect;
        }

        /// <summary>
        /// Verify whether clipboard contains any data object.
        /// </summary>
        /// <returns>true if a data object is present, false if not.</returns>
        public static bool ContainsData()
        {
            var dataFormats = typeof(DataFormats).GetFields(
                System.Reflection.BindingFlags.Public |
                System.Reflection.BindingFlags.Static).Select(f => f.Name);
            var containsData = dataFormats.Any(x => Clipboard.ContainsData(x));

            return containsData;
        }
    }
}
