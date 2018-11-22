/*
** 
**  electrifier
** 
**  Copyright 2018 Thorsten Jung, www.electrifier.org
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

using common.Interop.WinShell;

namespace electrifier.Core.WindowsShell
{
    /// <summary>
    /// IFileOperation helper class
    /// 
    /// <seealso href="https://github.com/Microsoft/Windows-classic-samples/blob/master/Samples/Win7Samples/winui/shell/appplatform/fileoperations/FileOperationSample.cpp"/>
    /// </summary>
    public class FileOperation : IDisposable
    {
        private IFileOperation shellFileOperation = null;
        private Guid iShellItemGUID = typeof(IShellItem).GUID;

        public FileOperation(
            IntPtr windowHandle,
            FileOperationFlags operationFlags = (FileOperationFlags.FOF_AllowUndo | FileOperationFlags.FOF_RenameOnCollision))
        {
            this.shellFileOperation = CLSID.CoCreateInstance<IFileOperation>(CLSID.FileOperation);

            if (this.shellFileOperation is null)
                throw new Exception("Could not instantiate FileOperation!");

            // TODO: Check for useful FileOperationFlags!
            this.shellFileOperation.SetOperationFlags(operationFlags);
            this.shellFileOperation.SetOwnerWindow(windowHandle);
        }



        public void CopyItem(
            string sourceFullPathName,
            string destinationFolder,
            string pszCopyName = null)
        {
            IShellItem shItemSource = null, shItemDestination = null;

            try
            {
                shItemSource = Shell32.SHCreateItemFromParsingName(sourceFullPathName, null, ref this.iShellItemGUID);
                shItemDestination = Shell32.SHCreateItemFromParsingName(destinationFolder, null, ref this.iShellItemGUID);

                this.shellFileOperation.CopyItem(shItemSource, shItemDestination, pszCopyName, null);
            }
            finally
            {
                if (null != shItemDestination)
                {
                    Marshal.ReleaseComObject(shItemDestination);
                    shItemDestination = null;

                    // TODO: Finally release COM-object! Copying and deleting at once doesn't work...
                }
                if (null != shItemSource)
                {
                    Marshal.ReleaseComObject(shItemSource);
                    shItemSource = null;
                }
            }
        }



        public void PerformOperations()
        {
            this.shellFileOperation.PerformOperations();
        }



        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                if (!(this.shellFileOperation is null))
                {
                    Marshal.ReleaseComObject(this.shellFileOperation);

                    this.shellFileOperation = null;
                }

                this.disposedValue = true;
            }
        }

        // Override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        ~FileOperation()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            this.Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        void IDisposable.Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            this.Dispose(true);
            // Uncomment the following line if the finalizer is overridden above.
            GC.SuppressFinalize(this);
        }
        #endregion

    }
}
