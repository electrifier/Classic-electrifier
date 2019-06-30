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
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace electrifier.Core.WindowsShell
{
    class ElShellTools
    {
        protected static uint Internet_Max_URL_Length = 2048 + 32 + 3;      // INTERNET_MAX_URL_LENGTH

        #region SHLWApi.dll Imports ===================================================================================================================================================================

        /// <summary>
        /// Converts a Windows file/path to a URL. See also <see cref="PathCreateFromUrl"/> and 
        /// <a href="http://www.pinvoke.net/default.aspx/shlwapi/UrlCreateFromPath.html">PInvoke.net: UrlCreateFromPath (shlwapi)</a> for details.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="url"></param>
        /// <param name="urlLength"></param>
        /// <param name="reserved"></param>
        /// <returns></returns>
        [DllImport("shlwapi.dll", CharSet = CharSet.Auto)]
        protected static extern int UrlCreateFromPath(
            [In]      string path,
            [Out]     StringBuilder url,
            [In, Out] ref uint urlLength,
            [In]      uint reserved);

        /// <summary>
        /// Takes a (canonicalized) file URL and converts it to a Microsoft MS-DOS path. Member of the Shell Lightweight Utility API. See also <see cref="UrlCreateFromPath"/> and
        /// <a href="http://www.pinvoke.net/default.aspx/shlwapi/PathCreateFromUrl.html">PInvoke.net: PathCreateFromUrl (shlwapi)</a> for details.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="path"></param>
        /// <param name="pathLength"></param>
        /// <param name="reserved"></param>
        /// <returns></returns>
        [DllImport("shlwapi.dll", SetLastError = true)]
        protected static extern int PathCreateFromUrl(
            [In]      string url,
            [Out]     StringBuilder path,
            [In, Out] ref uint pathLength,
            [In]      uint reserved);

        #endregion SHLWApi.dll Imports ================================================================================================================================================================

        /// <summary>
        /// Normalize the given path name, i.e. the full path converted to uppercase.
        /// </summary>
        /// <param name="path"></param>
        /// <returns>The normalized path name.</returns>
        public static string NormalizePath(string path)
        {
            return Path.GetFullPath(new Uri(path).LocalPath)
                .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                .ToUpperInvariant();
        }

        /// <summary>
        /// Transform the fiven filePath into an URL.
        /// 
        /// See <see cref="https://en.wikipedia.org/wiki/File_URI_scheme"/> for details about Windows file URI scheme.
        /// </summary>
        /// <param name="FilePath">The local folder or rather FilePath to be transformed into Windows file URI scheme</param>
        /// <returns>The given FilePath encoded into Windows file URI scheme</returns>
        public static string UrlCreateFromPath(string FilePath)
        {
            uint maxLength = Internet_Max_URL_Length;
            StringBuilder Url = new StringBuilder((int)maxLength);

            ElShellTools.UrlCreateFromPath(FilePath, Url, ref maxLength, 0);

            return Url.ToString();
        }

        /// <summary>
        /// Takes a (canonicalized) file URL and converts it to a Microsoft MS-DOS path.
        /// 
        /// See <see cref="https://en.wikipedia.org/wiki/File_URI_scheme"/> for details about Windows file URI scheme.
        /// </summary>
        /// <param name="Url">The URI scheme encoded FilePath to be transformed</param>
        /// <returns>The given FilePath decoded from Windows file URI scheme</returns>
        public static string PathCreateFromUrl(string Url)
        {
            uint maxLength = Internet_Max_URL_Length;
            StringBuilder filePath = new StringBuilder((int)maxLength);

            ElShellTools.PathCreateFromUrl(Url, filePath, ref maxLength, 0);

            return filePath.ToString();
        }

        /// <summary>
        /// Split the given argument string into string array.
        /// 
        /// Taken from <see cref="https://stackoverflow.com/a/7774211"/>
        /// </summary>
        /// <param name="argumentString">The white space seperated argument list</param>
        /// <returns>Array of command line argument strings</returns>
        public static IEnumerable<string> SplitArgumentString(/*this*/ string argumentString)
        {
            if (string.IsNullOrWhiteSpace(argumentString))
                yield break;

            var sb = new StringBuilder();
            bool inQuote = false;

            foreach (char c in argumentString)
            {
                if (c == '"' && !inQuote)
                {
                    inQuote = true;
                    continue;
                }

                if (c != '"' && !(char.IsWhiteSpace(c) && !inQuote))
                {
                    sb.Append(c);
                    continue;
                }

                if (sb.Length > 0)
                {
                    var result = sb.ToString();

                    sb.Clear();
                    inQuote = false;

                    yield return result;
                }
            }

            if (sb.Length > 0)
                yield return sb.ToString();
        }
    }
}
