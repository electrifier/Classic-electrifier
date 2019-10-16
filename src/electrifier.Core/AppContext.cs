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
using System.Reflection;
using System.Security.Permissions;
using System.Threading;
using System.Windows.Forms;

using Microsoft.Win32;

namespace electrifier.Core
{
    public sealed partial class AppContext : System.Windows.Forms.ApplicationContext
    {
        #region Properties ====================================================================================================

        public static Icon Icon { get; private set; }
        public Bitmap Logo { get; private set; }
        public bool IsPortable { get; private set; }
        public bool IsIncognito { get; private set; }
        internal AppContextSession Session { get; }
        public bool IsBetaVersion() { return true; }

        public static string AssemblyCompany {
            get {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);

                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }

        #endregion ============================================================================================================

        /// <summary>
        /// 
        /// <see cref="AppContext"/> is the main entry point of electrifier Application.
        /// 
        /// </summary>
        /// <param name="args">The argument list given when starting the application</param>
        /// <param name="appIcon">The icon resource used by this application</param>
        /// <param name="appLogo">The logo resource used by this application</param>
        /// <param name="splashScreenForm">The form representing the logo as splash screen</param>
        [SecurityPermission(SecurityAction.Demand, Flags=SecurityPermissionFlag.ControlAppDomain)]
        public AppContext(string[] args, Icon appIcon, Bitmap appLogo, Form splashScreenForm) : base()
        {
            AppContext.Icon = appIcon;
            this.Logo = appLogo;

            // Initialize Exception handlers
            Application.ThreadException += this.Application_ThreadException;
            AppDomain.CurrentDomain.UnhandledException += this.CurrentDomain_UnhandledException;

            // Parse command line paramaters
            foreach (string arg in args)
            {
                // Portable: Issue #5: Add "-portable" command switch, storing configuration in application directory instead of "LocalApplicationData"
                if (arg.ToLower().Equals("/portable"))
                    this.IsPortable = true;

                // Incognito: Don't modify configuration file on exit, thus don't make session parameters/changes persistent
                if (arg.ToLower().Equals("/incognito"))
                    this.IsIncognito = true;
            }

            // Initialize trace listener
            this.InitializeTraceListener();
            AppContext.TraceScope();

            // Initialize session object
            //
            // TODO: Check if another instance is already running. If so, create new session with different name and fresh settings; optionally copy default session to new session settings!
            //
            this.Session = new AppContextSession(this.IsPortable);
            this.MainForm = this.Session.ElectrifierForm;

            // Add ThreadExit-handler to save configuration when closing
            this.ThreadExit += new EventHandler(this.AppContext_ThreadExit);

            // TODO: Implement own MessageBox with "This is ALPHA-sign" and warning-text, perhaps use logo again
            if (this.IsBetaVersion())
            {
                if (false == Properties.Settings.Default.Disable_BETAVersion_Warning)
                {
                    // Disable TopMost-property of SplashScreenForm to ensure Beta-Warning is shown in front of it
                    splashScreenForm.TopMost = false;

                    string msgText = "Hello and welcome to electrifier!\n\n" +
                        "Please be aware that this is beta software and you should not use it in productive environments. Although carefully tested " +
                        "there will be bugs, which may result in data loss, crashes, system instability and / or rage attacks!\n\n" +
                        "No one can be held responsible for any damage that is caused by using this software!\n\n" +
                        "Use at your own risk. By continuing using this software you show that you understand the implications.\n\n" +
                        "Click Yes to see this warning again, or No to disable it forever!";

                    if (DialogResult.No == MessageBox.Show(msgText, "electrifier - Warning!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
                    {
                        Properties.Settings.Default.Disable_BETAVersion_Warning = true;
                    }
                }
            }

            // Finally close splash screen
            splashScreenForm.Close();
            splashScreenForm.Dispose();
        }

        private void AppContext_ThreadExit(object sender, EventArgs e)
        {
            AppContext.TraceScope();

            // Save configuration file
            if (false == this.IsIncognito)
                this.Session.SaveConfiguration();
        }

        private void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            AppContext.TraceError("Thread Exception:" + e.ToString());
            MessageBox.Show(e.ToString(), "D'oh! That shouldn't have happened...");
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            AppContext.TraceError("Domain Exception:" + e.ToString());
            MessageBox.Show(e.ToString(), "D'oh! That shouldn't have happened...");
        }


        #region TraceListener helper members for logging and debugging purposes ===============================================

        /// <summary>
        /// InitializeTraceListener
        /// 
        /// Initializes the trace listener.
        /// By default, output will be written into '[System.Windows.Forms.Application.StartupPath]\electrifier\electrifier.debug.log'
        /// </summary>
        [Conditional("DEBUG")]
        private void InitializeTraceListener()
        {
            string filePath = System.IO.Path.Combine(System.Windows.Forms.Application.StartupPath, @"electrifier.debug.log");

            // Ensure directory exists before attempting to create the file
            System.IO.FileInfo fileInfo = new System.IO.FileInfo(filePath);
            fileInfo.Directory.Create();

            DefaultTraceListener defaultTraceListener = new DefaultTraceListener
            {
                LogFileName = filePath
            };

            defaultTraceListener.WriteLine("\nNEW SESSION STARTED...\n");

            Trace.Listeners.Clear();
            Trace.Listeners.Add(defaultTraceListener);
        }

        [Conditional("DEBUG")]
        internal static void TraceScope(
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string filePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0)
        {
            string fullMessage = DateTime.Now.ToString("HH:mm:ss");
            fullMessage += " Enter Scope of Member " + memberName;
            fullMessage += " @ '" + filePath;

            Trace.WriteLine(fullMessage, "Scope");
        }

        [Conditional("DEBUG")]
        internal static void TraceDebug(string message,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string filePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0)
        {
            string fullMessage = DateTime.Now.ToString("HH:mm:ss");
            fullMessage += " " + message;
            fullMessage += " @ '" + filePath;
            fullMessage += "' in " + memberName;

            Trace.WriteLine(fullMessage, "Debug");
        }

        [Conditional("DEBUG")]
        internal static void TraceWarning(string message,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string filePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0)
        {
            string fullMessage = DateTime.Now.ToString("HH:mm:ss");
            fullMessage += " [-!-]: " + message;
            fullMessage += " @ '" + filePath;
            fullMessage += "' in " + memberName;

            Trace.WriteLine(fullMessage, "Warning");
        }

        [Conditional("DEBUG")]
        internal static void TraceError(string message,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string filePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0)
        {
            string fullMessage = DateTime.Now.ToString("HH:mm:ss");
            fullMessage += " ERROR: " + message;
            fullMessage += " @ '" + filePath;
            fullMessage += "', in " + memberName;
            fullMessage += "#" + lineNumber;

            Trace.WriteLine(fullMessage, "Error");
        }

        #endregion ============================================================================================================

        #region Environment helper methods ====================================================================================

        /// <summary>
        /// Beginning with .net framework 4.5, a registry value has to be evaluated to get the exact .net framekwork version.
        /// 
        /// This static method returns the exact framework version by using this technique.
        /// 
        /// <seealso href="https://docs.microsoft.com/en-us/dotnet/framework/migration-guide/how-to-determine-which-versions-are-installed"/>
        /// </summary>
        /// <param name="check64Bit">Check for 64 bit environment if true.</param>
        /// <returns>A string value representing the exact .net framework version</returns>
        public static string GetDotNetFrameworkVersion(bool check64Bit = true)
        {
            const string regSubKey = @"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full\";
            const string regValue = @"Release";
            string dotNetVersion;

            using (var regKeyLocalMachine = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32))
            {
                using (var regKeyNetDeveloperPlatform = regKeyLocalMachine.OpenSubKey(regSubKey))
                {
                    if (regKeyNetDeveloperPlatform?.GetValue(regValue) != null)
                    {
                        dotNetVersion = $".NET Framework Version: {CheckFor45PlusVersion((int)regKeyNetDeveloperPlatform.GetValue(regValue))}";

                        if (check64Bit)
                            dotNetVersion += (EnvironmentEx.Is64BitProcess ? " (64 bit)" : " (32 bit)");
                    }
                    else
                    {
                        dotNetVersion = @".NET Framework Version 4.5 or later is not detected.";
                    }
                }
            }

            return dotNetVersion;

            // Checking the version using for forward compatibility.
            string CheckFor45PlusVersion(int releaseKey)
            {
                if (releaseKey > 461814)
                    return "4.7.2 or later";
                if (releaseKey >= 461808)
                    return "4.7.2";
                if (releaseKey >= 461308)
                    return "4.7.1";
                if (releaseKey >= 460798)
                    return "4.7";
                if (releaseKey >= 394802)
                    return "4.6.2";
                if (releaseKey >= 394254)
                    return "4.6.1";
                if (releaseKey >= 393295)
                    return "4.6";
                if (releaseKey >= 379893)
                    return "4.5.2";
                if (releaseKey >= 378675)
                    return "4.5.1";
                if (releaseKey >= 378389)
                    return "4.5";
                // This code should never execute. A non-null release key should mean
                // that 4.5 or later is installed.
                return "No 4.5 or later version detected";
            }
        }

        #endregion ============================================================================================================
    }
}
