/*
** 
**  electrifier
** 
**  Copyright 2017 Thorsten Jung, www.electrifier.org
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
using System.Windows.Forms;

namespace electrifier.Core
{
    public sealed partial class AppContext : System.Windows.Forms.ApplicationContext
    {
        public Icon Icon { get; private set; }
        public Bitmap Logo { get; private set; }
        public bool IsPortable { get; private set; }
        public bool IsIncognito { get; private set; }
        internal AppContextSession Session { get; }

        /// <summary>
        /// 
        /// <see cref="AppContext"/> is the main entry point of electrifier Application.
        /// 
        /// </summary>
        /// <param name="args">The argument list given when starting the application</param>
        /// <param name="appIcon">The icon resource used by this application</param>
        /// <param name="appLogo">The logo resource used by this application</param>
        /// <param name="splashScreenForm">The form representing the logo as splash screen</param>

        public AppContext(string[] args, Icon appIcon, Bitmap appLogo, Form splashScreenForm) : base()
        {
            this.Icon = appIcon;
            this.Logo = appLogo;

            foreach (string arg in args)
            {
                // Portable: Issue #5: Add "-portable" command switch, storing configuration in application directory instead of "LocalApplicationData"
                if (arg.ToLower().Equals("/portable"))
                    this.IsPortable = true;

                // Incognito: Don't modify configuration file on exit, thus don't make session parameters/changes persistent
                if (arg.ToLower().Equals("/incognito"))
                    this.IsIncognito = true;
            }

            // Initialize session object
            this.Session = new AppContextSession(this.IsPortable);
            this.MainForm = this.Session.PrepareForm(this.Icon);

            // Initialize trace listener
            this.InitializeTraceListener();
            AppContext.TraceScope();

            // Add ThreadExit-handler to save configuration when closing
            ThreadExit += new EventHandler(this.AppContext_ThreadExit);

            // Finally close splash screen
            splashScreenForm.Close();
            splashScreenForm.Dispose();
        }

        private void AppContext_ThreadExit(object sender, EventArgs e)
        {
            //// Save configuration file
            //if (AppContext.IsIncognito == false)
            //	this.SaveConfiguration();

            AppContext.TraceScope();
        }

        #region Debug helper members

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

            Trace.WriteLine(fullMessage);
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

            Trace.WriteLine(fullMessage);
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

            Trace.WriteLine(fullMessage);
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

            Trace.WriteLine(fullMessage);
        }

        #endregion
    }
}
