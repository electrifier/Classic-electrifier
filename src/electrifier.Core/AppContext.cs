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
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;

using electrifier.Core.Forms;

namespace electrifier.Core
{

    /// <summary>
    /// AppContext acts as a singleton which instantiates all the basic services and gets the user interface started up
    /// 
    /// See https://msdn.microsoft.com/en-us/library/ff650316.aspx for details on implementation of the singleton
    /// </summary>
    public sealed partial class AppContext
        : System.Windows.Forms.ApplicationContext
    {
        // Static member variables and properties
        private static Icon icon = null;
        public static Icon Icon { get { return AppContext.icon; } }
        private static Bitmap logo = null;
        public static Bitmap Logo { get { return AppContext.logo; } }
        private static bool isPortable = false;
        public static bool IsPortable { get { return AppContext.isPortable; } }
        private static bool isIncognito = false;
        public static bool IsIncognito { get { return AppContext.isIncognito; } }
        internal AppContextSession Session { get; }

        /// <summary>
        /// The default constructor of the AppContext.
        /// Note that this class is only allowed to be instantiated once!
        /// Since electrifier.exe does this, don't instantiate this class for yourself!
        /// Use its static <c>Instance</c> property instead, if you need access to its members.
        /// </summary>
        /// <param name="args">The argument list given when starting the application</param>
        /// <param name="appIcon">The icon resource used by this application</param>
        /// <param name="appLogo">The logo resource used by this application</param>
        /// <param name="splashScreenForm">The form representing the logo as splash screen</param>
        public AppContext(string[] args, Icon appIcon, Bitmap appLogo, Form splashScreenForm)
        {
            // Initialize debug listener
            this.InitializeDebugListener();
            Debug.WriteLine("electrifier.Core.AppContext: New AppContext created. (" + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + ")");

            // Initialize application context
            AppContext.icon = appIcon;
            AppContext.logo = appLogo;

            // Search argument list for AppContext-related arguments
            foreach (string arg in args)
            {
                // Portable: Issue #5: Add "-portable" command switch, storing configuration in application directory instead of "LocalApplicationData"
                if (arg.ToLower().Equals("/portable"))
                    AppContext.isPortable = true;

                // Incognito: Don't modify configuration file on exit, thus don't make session parameters/changes persistent
                if (arg.ToLower().Equals("/incognito"))
                    AppContext.isIncognito = true;
            }

            // Create session object
            this.Session = new AppContextSession();
            this.MainForm = this.Session.MainWindowForm;

            // Add ThreadExit-handler to save configuration when closing
            ThreadExit += new EventHandler(this.AppContext_ThreadExit);

            // Finally close splash screen
            splashScreenForm.Close();
            splashScreenForm.Dispose();
        }

        /// <summary>
        /// InitializeDebugListener
        /// 
        /// Initializes the debug listener.
        /// Output will be written into 'C:\Users\[USER]\AppData\Local\electrifier\electrifier.debug.log'
        /// 
        /// WARNING: TODO: Currently will fail when mutltiple instances are running in debug mode!
        /// </summary>
        [Conditional("DEBUG")]
        private void InitializeDebugListener()
        {
            Debug.Listeners.Add(new TextWriterTraceListener(new FileStream((AppContext.IsPortable ?
                (Path.Combine(Application.StartupPath, @"electrifier.debug.log")) :
                (Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"electrifier", @"electrifier.debug.log"))),
                FileMode.Append, FileAccess.Write)));
        }

        private void AppContext_ThreadExit(object sender, EventArgs e)
        {
            //// Save configuration file
            //if (AppContext.IsIncognito == false)
            //	this.SaveConfiguration();

            Debug.WriteLine("electrifier.Core.AppContext: AppContext is shutting down. (" + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + ")\n");
        }
    }
}
