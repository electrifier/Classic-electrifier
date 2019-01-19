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

using common.Interop;

namespace common.Interop.WinShell
{
    #region Flags =============================================================================================================

    /// <summary>
    /// Pararmeters used for IFileOperation::SetOperationFlags
    /// 
    /// <see href="https://docs.microsoft.com/en-us/windows/desktop/api/shobjidl_core/nf-shobjidl_core-ifileoperation-setoperationflags"/>
    /// </summary>
    [Flags]
    public enum FileOperationFlags : uint
    {
        FOF_MultiDestFiles = 0x00000001,
        FOF_ConfirmMouse = 0x00000002,
        FOF_Silent = 0x00000004,
        FOF_RenameOnCollision = 0x00000008,
        FOF_NoConfirmation = 0x00000010,
        FOF_WantMappingHandle = 0x00000020,
        FOF_AllowUndo = 0x00000040,
        FOF_FilesOnly = 0x00000080,
        FOF_SimpleProgress = 0x00000100,
        FOF_NoConfirmMkDir = 0x00000200,
        FOF_NoErrorUI = 0x00000400,
        FOF_NoCopySecurityAttribs = 0x00000800,
        FOF_NoRecursion = 0x00001000,
        FOF_No_Connected_Elements = 0x00002000,
        FOF_WantNukeWarning = 0x00004000,
        FOF_NoRecurseReparse = 0x00008000,
        FOFX_NoSkipJunctions = 0x00010000,
        FOFX_PreferHardlink = 0x00020000,
        FOFX_ShowElevationPrompt = 0x00040000,
        FOFX_EarlyFailure = 0x00100000,
        FOFX_PreserveFileExtensions = 0x00200000,
        FOFX_KeepNewerFile = 0x00400000,
        FOFX_NoCopyHooks = 0x00800000,
        FOFX_NoMinimizeBox = 0x01000000,
        FOFX_MoveAclsAcrossVolumes = 0x02000000,
        FOFX_DontDisplaySourcePath = 0x04000000,
        FOFX_DontDisplayDestPath = 0x08000000,
        FOFX_RecycleOnDelete = 0x00080000,
    }

    #endregion ================================================================================================================

    /// <summary>
    /// https://docs.microsoft.com/en-us/windows/desktop/api/shobjidl_core/nn-shobjidl_core-ifileoperation
    /// </summary>
    [
        ComImport,
        Guid("947AAB5F-0A5C-4C13-B4D6-4BF7836FC9F8"),
        InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IFileOperation
    {
        uint Advise(
            IFileOperationProgressSink pfops);

        void Unadvise(
            uint dwCookie);

        void SetOperationFlags(
            FileOperationFlags dwOperationFlags);

        void SetProgressMessage(
            [MarshalAs(UnmanagedType.LPWStr)] string pszMessage);

        void SetProgressDialog(
            [MarshalAs(UnmanagedType.Interface)] object popd);

        void SetProperties(
            [MarshalAs(UnmanagedType.Interface)] object pproparray);

        void SetOwnerWindow(
            IntPtr hwndParent);

        void ApplyPropertiesToItem(
            IShellItem psiItem);

        void ApplyPropertiesToItems(
            [MarshalAs(UnmanagedType.Interface)] object punkItems);

        void RenameItem(
            IShellItem psiItem,
            [MarshalAs(UnmanagedType.LPWStr)] string pszNewName,
            IFileOperationProgressSink pfopsItem);

        void RenameItems(
            [MarshalAs(UnmanagedType.Interface)] object pUnkItems,
            [MarshalAs(UnmanagedType.LPWStr)] string pszNewName);

        void MoveItem(
            IShellItem psiItem,
            IShellItem psiDestinationFolder,
            [MarshalAs(UnmanagedType.LPWStr)] string pszNewName,
            IFileOperationProgressSink pfopsItem);

        void MoveItems(
            [MarshalAs(UnmanagedType.Interface)] object punkItems,
            IShellItem psiDestinationFolder);

        void CopyItem(
            IShellItem psiItem,
            IShellItem psiDestinationFolder,
            [MarshalAs(UnmanagedType.LPWStr)] string pszCopyName,
            IFileOperationProgressSink pfopsItem);

        void CopyItems(
            [MarshalAs(UnmanagedType.Interface)] object punkItems,
            IShellItem psiDestinationFolder);

        void DeleteItem(
            IShellItem psiItem,
            IFileOperationProgressSink pfopsItem);

        void DeleteItems(
            [MarshalAs(UnmanagedType.Interface)] object punkItems);

        common.Interop.WinError.HResult NewItem(
            IShellItem psiDestinationFolder,
            WinNT.File_Attribute /*FileAttributes*/ dwFileAttributes,
            [MarshalAs(UnmanagedType.LPWStr)] string pszName,
            [MarshalAs(UnmanagedType.LPWStr)] string pszTemplateName,
            IFileOperationProgressSink pfopsItem);

        void PerformOperations();

        [return: MarshalAs(UnmanagedType.Bool)]
        bool GetAnyOperationsAborted();
    }
}
