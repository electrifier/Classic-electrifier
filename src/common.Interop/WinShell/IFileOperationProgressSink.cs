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

namespace common.Interop.WinShell
{
    /// <summary>
    /// https://docs.microsoft.com/en-us/windows/desktop/api/shobjidl_core/nn-shobjidl_core-ifileoperationprogresssink
    /// </summary>
    [
        ComImport,
        Guid("04B0F1A7-9490-44BC-96E1-4296A31252E2"),
        InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [System.ObsoleteAttribute("Interface common.Interop.WinShell.IFileOperationProgressSink is obsolete. It will be replaced and removed.")]
    public interface IFileOperationProgressSink
    {
        void StartOperations();

        void FinishOperations(
            uint hrResult);

        void PreRenameItem(
            uint dwFlags,
            IShellItem psiItem,
            [MarshalAs(UnmanagedType.LPWStr)] string pszNewName);

        void PostRenameItem(
            uint dwFlags,
            IShellItem psiItem,
            [MarshalAs(UnmanagedType.LPWStr)] string pszNewName,
            uint hrRename,
            IShellItem psiNewlyCreated);

        void PreMoveItem(
            uint dwFlags,
            IShellItem psiItem,
            IShellItem psiDestinationFolder,
            [MarshalAs(UnmanagedType.LPWStr)] string pszNewName);

        void PostMoveItem(
            uint dwFlags,
            IShellItem psiItem,
            IShellItem psiDestinationFolder,
            [MarshalAs(UnmanagedType.LPWStr)] string pszNewName,
            uint hrMove,
            IShellItem psiNewlyCreated);

        void PreCopyItem(
            uint dwFlags,
            IShellItem psiItem,
            IShellItem psiDestinationFolder,
            [MarshalAs(UnmanagedType.LPWStr)] string pszNewName);

        void PostCopyItem(uint dwFlags,
            IShellItem psiItem,
            IShellItem psiDestinationFolder,
            [MarshalAs(UnmanagedType.LPWStr)] string pszNewName,
            uint hrCopy,
            IShellItem psiNewlyCreated);

        void PreDeleteItem(
            uint dwFlags,
            IShellItem psiItem);

        void PostDeleteItem(
            uint dwFlags,
            IShellItem psiItem,
            uint hrDelete,
            IShellItem psiNewlyCreated);

        void PreNewItem(
            uint dwFlags,
            IShellItem psiDestinationFolder,
            [MarshalAs(UnmanagedType.LPWStr)] string pszNewName);

        void PostNewItem(
            uint dwFlags,
            IShellItem psiDestinationFolder,
            [MarshalAs(UnmanagedType.LPWStr)] string pszNewName,
            [MarshalAs(UnmanagedType.LPWStr)] string pszTemplateName,
            uint dwFileAttributes,
            uint hrNew,
            IShellItem psiNewItem);

        void UpdateProgress(
            uint iWorkTotal,
            uint iWorkSoFar);

        void ResetTimer();

        void PauseTimer();

        void ResumeTimer();
    }
}
