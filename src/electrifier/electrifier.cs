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
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace electrifier
{
    /// <summary>
    /// The class <c>ElectrifierMainEntryPoint</c> acts, as suggested, as the main entry point.
    /// The given arguments are evaluated, the core services started and the main configuration
    /// is read out; also, the splash is shown here.
    /// </summary>
    class ElectrifierMainEntryPoint
    {

        /// <summary>
        /// System Error Codes (0-499)
        /// 
        /// See https://msdn.microsoft.com/de-de/library/ms681382(v=vs.85)
        /// 
        /// ERROR_INVALID_FUNCTION = 1 (0x1)
        ///		Incorrect function.
        ///
        /// ERROR_FILE_NOT_FOUND = 2 (0x2)
        ///		The system cannot find the file specified.
        /// </summary>
        internal enum SystemErrorCodes
        {
            ERROR_INVALID_FUNCTION = 1,
            ERROR_FILE_NOT_FOUND = 2,
        }

        private static readonly string elCoreDLLFileName = @"electrifier.Core.dll";
        private static readonly string appIconResourceID = @"electrifier.electrifier.ico";
        private static readonly string appContextTypeID = @"electrifier.Core.AppContext";

        /// <summary>
        /// This Main method is the entry point of the electrifier application.<br/>
        /// <br/>
        /// Since we use COM to interop with Shell Objects, we have to use Single Thread ApartmentState.<br/>See
        /// <seealso href="https://docs.microsoft.com/en-us/archive/blogs/jfoscoding/why-is-stathread-required">
        /// Why is STAThread required?</seealso> for more information on this topic.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            bool splashIsShown = true;
            bool splashIsFadedOut = true;
            Icon applicationIcon = null;
            SplashScreenForm splashScreen = null;

            // Catch all unhandled exceptions, exception-handlers will be configured in electrifier.Core.dll
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

            // Never show splash-screen when debugging
            if (Debugger.IsAttached)
                splashIsShown = false;

            // TODO: Check if first instance
            // Enable application to use Visual Styles
            Application.EnableVisualStyles();
            Application.DoEvents();

            // Search argument list for splash-screen-related arguments
            foreach (string arg in args)
            {
                if (arg.ToLower().Equals("/nosplash"))
                {
                    splashIsShown = false;
                }

                if (arg.ToLower().Equals("/nosplashfadeout"))
                {
                    splashIsFadedOut = false;
                }
            }

            // Create an electrifier application context by loading and running electrifier.core.dll
            try
            {
                // Create the splash-screen
                splashScreen = new SplashScreenForm(splashIsShown, splashIsFadedOut);

                Assembly entryAssembly = Assembly.GetEntryAssembly();
                string electrifierCoreDLLFullPath = Path.Combine(
                    Path.GetDirectoryName(entryAssembly.Location),
                    ElectrifierMainEntryPoint.elCoreDLLFileName);

                // Get the application icon
                applicationIcon = new Icon(entryAssembly.GetManifestResourceStream(appIconResourceID));

                // Create the instance of the application context
                ApplicationContext appContext = Activator.CreateInstance(
                    Assembly.LoadFile(electrifierCoreDLLFullPath).GetType(appContextTypeID),
                    args: new object[] { args, applicationIcon, splashScreen.SplashScreenBitmap, splashScreen }) as ApplicationContext;

                // Run electrifier applicaton context
                try
                {
                    Application.Run(appContext);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        $"Error while running electrifier.\n\n" +
                        $"Please try to reinstall electrifier application.\n\n" +
                        $"Type of Exception: '{ ex.Message }'\n\n" +
                        $"Inner Exception: '{ ex.InnerException.Message }'\n\n" +
                        $"{ ex.StackTrace }",
                        caption: @"electrifier: Runtime Error",
                        buttons: MessageBoxButtons.OK,
                        icon: MessageBoxIcon.Error);

                    Environment.ExitCode = (int)SystemErrorCodes.ERROR_INVALID_FUNCTION;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Unable to launch '{ ElectrifierMainEntryPoint.elCoreDLLFileName }'.\n\n" +
                    $"Please try to reinstall electrifier application.\n\n" +
                    $"Type of Exception: '{ ex.Message }'\n\n" +
                    $"Inner Exception: '{ ex.InnerException.Message }'\n\n" +
                    $"{ ex.StackTrace }",
                    caption: @"electrifier: Critical Startup Error",
                    buttons: MessageBoxButtons.OK,
                    icon: MessageBoxIcon.Error);

                Environment.ExitCode = (int)SystemErrorCodes.ERROR_FILE_NOT_FOUND;
            }
            finally
            {
                // Free used resources
                if (null != splashScreen)
                    splashScreen.Dispose();
                if (null != applicationIcon)
                    applicationIcon.Dispose();
            }
        }
    }
}
