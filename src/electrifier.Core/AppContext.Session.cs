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
using System.IO;

namespace electrifier.Core
{
    public sealed partial class AppContext
    {
        internal class AppContextSession
        {
            public readonly string ApplicationDataPath;
            public readonly string SessionName = "Default";
            public readonly string ConfigurationFileName = "Session.Default.xml";
            public electrifier.Core.Forms.Electrifier ElectrifierForm { get; private set; }

            public AppContextSession(bool isPortable)
            {
                this.ApplicationDataPath = (isPortable ?
                    System.Windows.Forms.Application.StartupPath :
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), AppContext.AssemblyCompany));
            }

            public electrifier.Core.Forms.Electrifier CreateElectrifierForm()
            {
                this.ElectrifierForm = new electrifier.Core.Forms.Electrifier(AppContext.Icon);

                return this.ElectrifierForm;
            }

            /// <summary>
            /// Called by AppContext.AppContext()
            /// </summary>
            /// <returns>True if configuration file found and successfully loaded, False otherwise.</returns>
            public bool LoadConfiguration()
            {
                var fullFileName = Path.Combine(this.ApplicationDataPath, this.ConfigurationFileName);

                if (File.Exists(fullFileName))
                {
                    this.ElectrifierForm = new electrifier.Core.Forms.Electrifier(AppContext.Icon);
                    this.ElectrifierForm.LoadConfiguration(fullFileName);

                }

                return false;
            }


            /// <summary>
            /// Called by AppContext.AppContext_ThreadExit()
            /// </summary>
            public void SaveConfiguration()
            {
                var fullFileName = Path.Combine(this.ApplicationDataPath, this.ConfigurationFileName);

                // Create directory for configuration file, just in case it doesn't already exist
                DirectoryInfo directoryInfo = Directory.CreateDirectory(this.ApplicationDataPath);

                this.ElectrifierForm.SaveConfiguration(fullFileName);
            }
        }
    }
}
