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
using System.IO;

namespace electrifier.Core
{
    public sealed partial class AppContext
    {
        internal class AppContextSession
        {
            #region Fields ========================================================================================================

            public readonly bool IsPortable;
            public readonly string ApplicationDataPath;
            public readonly string SessionName;

            #endregion Fields =====================================================================================================

            #region Properties ====================================================================================================

            public string ConfigurationFileName { get { return "Session." + this.SessionName + ".xml"; } }
            public electrifier.Core.Forms.Electrifier ElectrifierForm { get; private set; }

            #endregion Properties =================================================================================================

            public AppContextSession(bool isPortable, string sessionName = "Default")
            {
                this.IsPortable = isPortable;
                this.SessionName = sessionName;

                this.ApplicationDataPath = (isPortable ?
                    System.Windows.Forms.Application.StartupPath :
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), AppContext.AssemblyCompany));

                // Create Electrifier Form
                var configFullFileName = Path.Combine(this.ApplicationDataPath, this.ConfigurationFileName);

                this.ElectrifierForm = new electrifier.Core.Forms.Electrifier(AppContext.Icon);

                // TODO: If file exists, but is invalid (e.g. empty), this will crash...
                if (File.Exists(configFullFileName))
                    this.ElectrifierForm.LoadConfiguration(configFullFileName);



            }

            /// <summary>
            /// Save Electrifier Form state configuration into XML-file.
            /// 
            /// Called by AppContext.AppContext_ThreadExit()
            /// </summary>
            public void SaveConfiguration()
            {
                var fullFileName = Path.Combine(this.ApplicationDataPath, this.ConfigurationFileName);

                // Create directory for configuration file, just in case it doesn't already exist
                Directory.CreateDirectory(this.ApplicationDataPath);

                this.ElectrifierForm.SaveConfiguration(fullFileName);
            }
        }
    }
}
