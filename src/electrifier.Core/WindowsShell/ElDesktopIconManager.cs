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
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Xml;
using System.Xml.Serialization;
using Vanara.PInvoke;
using Vanara.Windows.Shell;

namespace electrifier.Core.WindowsShell
{
    /// <summary>
    /// This class provides Tools for dealing with Desktop Icons, especially for getting and setting their positions.<br/>
    /// <br/>
    /// <seealso href="https://devblogs.microsoft.com/oldnewthing/?p=4933">The Old New Thing: Manipulating the positions of desktop icons (Raymond Chen)</seealso>
    /// </summary>
    public class ElDesktopIconManager
    {
        //// TODO: Nice to have: https://stackoverflow.com/questions/22284718/how-to-get-the-pidl-of-an-open-explorer-window


        [Serializable]
        public class DesktopIconLayout
        {
            public DesktopIconLayout()
            {
                this.DesktopIcons = new List<DesktopIcon>();
            }

            [XmlElement("Width")]
            public int Width { get; set; }

            [XmlElement("Height")]
            public int Height { get; set; }

            [XmlElement("SnapToGrid")]
            public bool SnapToGrid { get; set; }

            [XmlArray("DesktopIcons")]
            public List<DesktopIcon> DesktopIcons { get; }
        }

        [Serializable]
        public struct DesktopIcon
        {
            private const Shell32.SIGDN SIGDNType = Shell32.SIGDN.SIGDN_PARENTRELATIVEPARSING; //.SIGDN_NORMALDISPLAY);
            public DesktopIcon(Shell32.PIDL pidl, Point position)
            {
                if (pidl is null)
                    throw new ArgumentNullException(nameof(pidl));

                this.Name = pidl.ToString(SIGDNType);
                this.Left = position.X;
                this.Top = position.Y;
            }

            public bool Equals(Shell32.PIDL pidl)
            {
                if (pidl is null)
                    throw new ArgumentNullException(nameof(pidl));

                return this.Name.Equals(pidl.ToString(SIGDNType), StringComparison.OrdinalIgnoreCase);
            }

            [XmlAttribute]
            public string Name;
            [XmlAttribute]
            public int Left;
            [XmlAttribute]
            public int Top;
        }

        private const string defaultFileName = @"Desktop Icon Layout.eldil";
        private const Environment.SpecialFolder defaultSpecialFolder = Environment.SpecialFolder.MyDocuments;
//        public static string DefaultFullFileName { get; set; } = @Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"electrifier\Desktop Icon Positions.eldip");      // TODO: AppContext => Property for output file directory

        public static string DefaultFileName => ElDesktopIconManager.defaultFileName;
        public static string DefaultDirectoryName => Path.Combine(Environment.GetFolderPath(defaultSpecialFolder), "electrifier");


        // TODO: OSRequired = Vista!
        // TODO: See : https://docs.microsoft.com/de-de/dotnet/standard/attributes/writing-custom-attributes
        // TODO: Move Serialization into ElDesktopIconManager; Don't store only one snapshot, but append up to [UserDefined] e.g. 10 Snapshots.

        public ElDesktopIconManager()
        {
            // TODO: var shelImages = new ShellImageList() // <= Vanara

        }

        public static int SaveLayout(string fullFileName = null)
        {
            int cntSavedIcons = 0;

            try
            {
                if (string.IsNullOrEmpty(fullFileName))
                    fullFileName = Path.Combine(DefaultDirectoryName, DefaultFileName);

                // Ensure directory exists before attempting to create the file
                System.IO.FileInfo fileInfo = new System.IO.FileInfo(fullFileName);
                fileInfo.Directory.Create();

                using (StreamWriter writer = new StreamWriter(fullFileName))
                {
                    DesktopIconLayout iconLayout = ElDesktopIconManager.GetCurrentDesktopIconLayout();
                    XmlSerializer serializer = new XmlSerializer(typeof(ElDesktopIconManager.DesktopIconLayout));

                    serializer.Serialize(writer, iconLayout);
                    writer.Close();

                    cntSavedIcons = iconLayout.DesktopIcons.Count;
                }
            }
            catch (Exception)
            {
                throw;
            }

            return cntSavedIcons;
        }

        public static int RestoreLayout(string fullFileName = null)
        {
            int cntRestoredIcons = 0;

            try
            {
                if (string.IsNullOrEmpty(fullFileName))
                    fullFileName = Path.Combine(DefaultDirectoryName, DefaultFileName);

                // TODO: Using this approach is unsafe. We should provide some XML-Schema here!
                XmlSerializer serializer = new XmlSerializer(typeof(ElDesktopIconManager.DesktopIconLayout));

                using (FileStream fileStream = new FileStream(fullFileName, FileMode.Open))
                {
                    // TODO: Use XmlReader!
                    DesktopIconLayout iconLayout = (DesktopIconLayout)serializer.Deserialize(fileStream);

                    cntRestoredIcons = ApplyDesktopIconLayout(iconLayout);

                    fileStream.Close();
                }
            }
            catch (FileNotFoundException)
            {
                throw;
            }
            catch (DirectoryNotFoundException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }

            return cntRestoredIcons;
        }

        /// <summary>
        /// Find the desktop folder view, i.e. its IFolderView2-interface.
        /// 
        /// See also: <seealso href="https://devblogs.microsoft.com/oldnewthing/?p=4933" />
        /// </summary>
        /// <returns>Desktop's IFolderView2-Interface</returns>
        protected static Shell32.IFolderView2 FindDesktopFolderView2()
        {
            IntPtr pShellBrowser = IntPtr.Zero, pFolderView2 = IntPtr.Zero;

            try
            {
                // TODO: This is still unfinished, cause the COM-Objects aren't released yet.
                Guid IID_IFolderView2 = typeof(Shell32.IFolderView2).GUID;

                Shell32.IShellWindows shellWindows = new Shell32.IShellWindows() ??
                    throw new COMException("No IShellWindows");

                if (!(shellWindows.FindWindowSW(new Ole32.PROPVARIANT((int)Shell32.CSIDL.CSIDL_DESKTOP), new Ole32.PROPVARIANT(),
                    Shell32.ShellWindowTypeConstants.SWC_DESKTOP, out _, Shell32.ShellWindowFindWindowOptions.SWFO_NEEDDISPATCH)
                    is Shell32.IServiceProvider serviceProvider))
                    throw new COMException("No IServiceProvider");

                serviceProvider.QueryService(Shell32.SID_STopLevelBrowser, typeof(Shell32.IShellBrowser).GUID, out pShellBrowser).ThrowIfFailed();
                Shell32.IShellBrowser shellBrowser = Marshal.GetTypedObjectForIUnknown(pShellBrowser, typeof(Shell32.IShellBrowser)) as Shell32.IShellBrowser;

                Shell32.IShellView shellView = shellBrowser.QueryActiveShellView();
                ((HRESULT)Marshal.QueryInterface(Marshal.GetIUnknownForObject(shellView), ref IID_IFolderView2, out pFolderView2)).ThrowIfFailed();

                return Marshal.GetTypedObjectForIUnknown(pFolderView2, typeof(Shell32.IFolderView2)) as Shell32.IFolderView2;
            }
            finally
            {
                if (pFolderView2 != IntPtr.Zero)
                    Marshal.Release(pFolderView2);

                if (pShellBrowser != IntPtr.Zero)
                    Marshal.Release(pShellBrowser);
            }
        }

        protected static DesktopIconLayout GetCurrentDesktopIconLayout()
        {
            try
            {
                Shell32.IFolderView2 folderView2 = FindDesktopFolderView2();
                DesktopIconLayout desktopIconLayout = new DesktopIconLayout
                {
                    // TODO: Save width & height
                    SnapToGrid = folderView2.GetCurrentFolderFlags().HasFlag(Shell32.FOLDERFLAGS.FWF_SNAPTOGRID)
                };

                Shell32.IEnumIDList spEnum = folderView2.Items<Shell32.IEnumIDList>(Shell32.SVGIO.SVGIO_ALLVIEW);

                while (spEnum.Next(1, out IntPtr ptrpidl, out uint pceltFetched).Succeeded && pceltFetched == 1)
                {
                    Point itemPosition = folderView2.GetItemPosition(ptrpidl);

                    using (Shell32.PIDL pidl = new Shell32.PIDL(ptrpidl, true, true))       // Release? Set 3rd to true
                    {
                        desktopIconLayout.DesktopIcons.Add(new DesktopIcon(pidl, itemPosition));
                    }
                }

                return desktopIconLayout;
            }
            catch (Exception)
            {
                // TODO: Exception-handling
                throw;
            }
        }

        protected static int ApplyDesktopIconLayout(DesktopIconLayout iconLayout)
        {
            if (iconLayout is null)
                throw new ArgumentNullException(nameof(iconLayout));

            int matchedItemCount = 0;

            if (iconLayout.DesktopIcons.Count > 0)
            {
                Shell32.IFolderView2 folderView2 = FindDesktopFolderView2();

                // Enumerate current Desktop Icons and match them with given DesktopIconLayout
                Shell32.IEnumIDList spEnum = folderView2.Items<Shell32.IEnumIDList>(Shell32.SVGIO.SVGIO_ALLVIEW);
                int maxItemCount = folderView2.ItemCount(Shell32.SVGIO.SVGIO_ALLVIEW);

                Shell32.PIDL[] apidl = new Shell32.PIDL[maxItemCount];
                Point[] apoint = new Point[maxItemCount];

                while (spEnum.Next(1, out IntPtr ptrpidl, out uint pceltFetched).Succeeded && pceltFetched == 1)
                {
                    foreach (var layoutItem in iconLayout.DesktopIcons)
                    {
                        using (Shell32.PIDL pidl = new Shell32.PIDL(ptrpidl, true, false))       // Release? Set 3rd to true
                        {
                            if (layoutItem.Equals(pidl))
                            {
                                apidl[matchedItemCount] = ptrpidl;
                                apoint[matchedItemCount] = new Point(layoutItem.Left, layoutItem.Top);

                                matchedItemCount++;
                            }
                        }
                    }

                    // Prevent buffer overflow - TODO: Use ^-Operator?
                    if (matchedItemCount >= maxItemCount)
                        break;
                }

                // Turn off auto-arrange
                folderView2.SetCurrentFolderFlags(Shell32.FOLDERFLAGS.FWF_AUTOARRANGE, Shell32.FOLDERFLAGS.FWF_NONE);

                // Position the matched items
                folderView2.SelectAndPositionItems((uint)matchedItemCount, apidl, apoint, Shell32.SVSIF.SVSI_POSITIONITEM);

                // Turn on 'Snap To Grid'-option if necessary
                folderView2.SetCurrentFolderFlags(Shell32.FOLDERFLAGS.FWF_SNAPTOGRID,
                    (iconLayout.SnapToGrid ? Shell32.FOLDERFLAGS.FWF_SNAPTOGRID : Shell32.FOLDERFLAGS.FWF_NONE));
            }

            return matchedItemCount;
        }
    }
}
