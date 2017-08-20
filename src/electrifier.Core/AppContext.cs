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
    public sealed partial class AppContext : System.Windows.Forms.ApplicationContext
    {
        public Icon Icon { get; private set; }
        public Bitmap Logo { get; private set; }
        public bool IsPortable { get; private set; }
        public bool IsIncognito { get; private set; }
        internal AppContextSession Session { get; }

        /// <summary>
        /// 
        /// AppContext is the main entry point of electrifier Application.
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

            // Initialize debug listener
            this.InitializeDebugListener();
            Debug.WriteLine("electrifier.Core.AppContext: New AppContext created. (" + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + ")");

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
            FileStream fileStream = new FileStream(Path.Combine(this.Session.ApplicationDataPath, @"electrifier.debug.log"), FileMode.Append, FileAccess.Write);

            Debug.Listeners.Add(new TextWriterTraceListener(fileStream));
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
