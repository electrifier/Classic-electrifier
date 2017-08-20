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
using System.IO;

using electrifier.Core.Forms;

namespace electrifier.Core
{
    public sealed partial class AppContext
    {
        internal class AppContextSession
        {
            public readonly string ApplicationDataPath;
            public MainWindowForm MainWindowForm { get; private set; }

            public AppContextSession(bool isPortable)
            {
                this.ApplicationDataPath = (isPortable ?
                    System.Windows.Forms.Application.StartupPath :
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"electrifier"));
            }

            public MainWindowForm PrepareForm(System.Drawing.Icon icon)
            {
                this.MainWindowForm = new MainWindowForm(icon);

                return this.MainWindowForm;
            }
        }
    }
}
