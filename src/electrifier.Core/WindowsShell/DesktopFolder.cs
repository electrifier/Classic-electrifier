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
    public class DesktopFolder : IDisposable
    {
        private common.Interop.WinShell.IShellFolder desktopFolder = null;
        public common.Interop.WinShell.IShellFolder GetSHFolder => this.desktopFolder;

        public DesktopFolder()
        {
            // Retrieve IShellFolder interface of desktop folder
            var hResult = Shell32.SHGetDesktopFolder(out this.desktopFolder);

            if ((hResult.Failed) || (this.desktopFolder is null))
            {
                var errorCode = Marshal.GetLastWin32Error();
                var errorMessage = "SHGetDesktopFolder() failed: " + errorCode.ToString();

                AppContext.TraceError(errorMessage);
                throw new InvalidComObjectException(errorMessage);
            }
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

                if (!(this.desktopFolder is null))
                {
                    Marshal.ReleaseComObject(this.desktopFolder);

                    this.desktopFolder = null;        
                }

                this.disposedValue = true;
            }
        }

        // Override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        ~DesktopFolder()
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
