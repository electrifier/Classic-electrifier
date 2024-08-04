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

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Vanara.PInvoke;

namespace electrifier.Core.WindowsShell
{
    static class ClipboardState
    {
        /// <summary>
        ///     Determines the DragDropEffect of the current Clipboard object.
        /// </summary>
        /// <returns>
        ///     Either <seealso cref="DragDropEffects.Move"/> or <seealso cref="DragDropEffects.Copy"/>
        ///     if succeeded or <seealso cref="DragDropEffects.None"/> if validation failed.
        /// </returns>
        public static DragDropEffects CurrentDropEffect
        {
            get
            {
                var dropEffect = DragDropEffects.None;
                var dropEffectObject = Clipboard.GetData(Shell32.ShellClipboardFormat.CFSTR_PREFERREDDROPEFFECT);

                if ((dropEffectObject is MemoryStream dropEffectMemoryStream) &&
                    dropEffectMemoryStream.CanRead && (dropEffectMemoryStream.Length > 0))
                {
                    var dropEffectByteArray = new byte[1];

                    dropEffectMemoryStream.Read(dropEffectByteArray, 0, 1);

                    dropEffect = (((dropEffectByteArray[0] & (byte)DragDropEffects.Move) != 0) ?
                        DragDropEffects.Move :
                        DragDropEffects.Copy);
                }

                return dropEffect;
            }
        }

        /// <summary>
        ///     Verify whether clipboard contains any data object.
        /// </summary>
        /// <returns>
        ///     true if a data object is present,
        ///     false if not.
        /// </returns>
        public static bool ContainsData => DataFormatNames.Any(dataformat => Clipboard.ContainsData(dataformat));

        public static IEnumerable<string> DataFormatNames { get; } = typeof(DataFormats)
            .GetFields(BindingFlags.Public | BindingFlags.Static).Select(f => f.Name);
    }
}
