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
using System.Runtime.InteropServices;
using System.Windows.Forms;

using Vanara.PInvoke;
using Vanara.Windows.Shell;

namespace electrifier.Core.WindowsShell
{
    /// <summary>
    /// TODO: Remove <see cref="ShellFileOperations"/>, cause the multi-threaded.
    /// Use <see cref="Vanara.Windows.Forms.ShellProgressDialog"/> instead.
    /// </summary>
    internal class ElShellFileOperations : IDisposable
    {
        public ShellFileOperations ShellFileOperations { get; }

        public bool ThrowUserCancellation { get; set; }

        //public IEnumerable<ShellItem> SourceItems { get; }

        //public IEnumerable<ShellItem> TargetItems { get; }

        public ElShellFileOperations(IWin32Window owner, ShellFileOperations.OperationFlags operationFlags)
        {
            this.ShellFileOperations = new ShellFileOperations(owner.Handle)
            {
                Options = operationFlags
            };

            //shFileOperation.PostCopyItem += OnPost;
            //shFileOperation.PostMoveItem += OnPost;

            // TODO: SHFileOperationA is thread safe, ShellFileOperation is not!
        }

        public void QueueClipboardOperation(string sourceFullName, ShellFolder destinationFolder, DragDropEffects dragDropEffect)
        {
            // TODO: => QueueCopyOperation(IEnumerable[]);

            if (dragDropEffect.HasFlag(DragDropEffects.Move))
                this.ShellFileOperations.QueueMoveOperation(new ShellItem(sourceFullName), destinationFolder);
            else
                this.ShellFileOperations.QueueCopyOperation(new ShellItem(sourceFullName), destinationFolder);
        }

        public void PerformOperations()
        {
            try
            {
                this.ShellFileOperations.PerformOperations();
            }
            catch (COMException ex)
            {
                HRESULT HResult = ex.HResult;

                LogContext.Error("ShellFileOperations failed: " + HResult.ToString());

                if ((HResult.Equals(HRESULT.COPYENGINE_E_USER_CANCELLED) == false) ||
                    (HResult.Equals(HRESULT.COPYENGINE_E_USER_CANCELLED) && this.ThrowUserCancellation))
                {
                    HResult.ThrowIfFailed();
                }
            }
        }

        public void Dispose()
        {
            if (this.ShellFileOperations.QueuedOperations > 0)
            {
                // INFO: When User has cancelled the operations, this will happen naturally
                LogContext.Warn("ShellFileOperations is about to get disposed while there are still Operations in Queue!");
//                throw new Exception("ElShellFileOperations is about to get disposed while there are still Operations in Queue!");
            }

            this.ShellFileOperations.Dispose();
        }
    }
}
