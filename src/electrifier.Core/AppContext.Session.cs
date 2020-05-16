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

using EntityLighter;
using System;
using System.Collections.Generic;
using System.IO;

namespace electrifier.Core
{
    public sealed partial class AppContext
    {
        /*
         * In case we want to provide multi-window-support in the future, see this:
         * 
         * Siehe: https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.applicationcontext.mainform?view=netframework-4.8
         * This property determines the main Form for this context. This property can change at any time. If OnMainFormClosed is not overridden,
         * the message loop of the thread terminates when the mainForm parameter closes.
         */
        internal class AppContextSession
          : ILightedEntity
        {
            #region Fields ========================================================================================================

//            public readonly bool IsPortable;
//            public readonly string Name;

            #endregion Fields =====================================================================================================

            #region Properties ====================================================================================================

            public Forms.ElApplicationWindow ApplicationWindow { get; private set; }




                        /// <summary>
                        /// TODO: The following implements ILightedEntity
                        /// </summary>
                        public DataContext DataContext { get; }
                        public string DatabaseTableName => "Session";

                        [Column(DataType.Integer, Constraints = Constraint.PrimaryKey)]
                        public long Id { get; }


                        // ===========

                        [Column(DataType.Text, Constraints = Constraint.NotNull)]
                        public string Name { get; }

                        [Column(DataType.Text)]
                        public string Description { get; }

                        [Column(DataType.Integer, Constraints = Constraint.NotNull, DefaultValue = "CURRENT_TIMESTAMP")]
                        public long DateCreated { get; }

                        [Column(DataType.Integer, Constraints = Constraint.NotNull, DefaultValue = "CURRENT_TIMESTAMP")]
                        public long DateModified { get; }

                        //internal IDockContentEntity[] DockContents { get; }     // TODO: So in etwa?

            #endregion Properties =================================================================================================

            public AppContextSession(AppContext appContext)     //bool isPortable, string sessionName = null)
//                : base(appContext.DataContext, null)
            {

                this.ApplicationWindow = new electrifier.Core.Forms.ElApplicationWindow(appContext.Icon);



                /* Code aus dem Session-Entity:
                        public SessionEntity(ElEntityStore entityStore, long? sessionId = null)
                        {
                            this.DataContext = entityStore ?? throw new ArgumentNullException(nameof(entityStore));

                            // In case the given ID is null create a whole new entity, otherwise load the exisiting one from the database
                            if (null == sessionId)
                            {
                                //var dateTime = DateTime.Now;
                                //var dayOfWeek = dateTime.DayOfWeek;
                                //DateTime.Now.ToString("U", DateTimeFormatInfo.CurrentInfo) }";       

                                this.Name = $"Session on { DateTime.Now.DayOfWeek }";       // TODO: Put into config!


                                this.Id = this.DataContext.CreateNewEntity(this, (sqlCmd) =>
                                {
                                    sqlCmd.CommandText = $"INSERT INTO Session (Name) VALUES ($Name)";
                                    sqlCmd.Parameters.AddWithValue("$Name", this.Name);
                                }
                                );
                            }
                            else
                            {
                                this.Id = (long)sessionId;

                                // TODO: Load existing values from database, BUT give it a new session ID!
                            }
                        }
                                */








                // TODO: 19/04/20: As long as we don't use a session-selector, use session #1
                // ...or select(max(id)) from session...
                //                SessionEntity sessionEntity = new SessionEntity(AppContext.ElEntityStore);



                // TODO: Load default values for session: [from sessionentity.cs]
                // TODO: Test-Code:
                // TODO: Über die DockContentFactory erzeugen!
                //DockContentEntity dockContent = new DockContentEntity(this /* ContentType ShellBrowser als type */);

                // TODO: Zuletzt geänderte Session - nicht die zuletzt angelegte! - als Default



                /////////////                this.IsPortable = isPortable;
                /////////////                this.SessionName = sessionName;
                /////////////
                /////////////                this.BaseDirectory = (isPortable ?
                /////////////                    System.Windows.Forms.Application.StartupPath :
                /////////////                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), AppContext.AssemblyCompany));
                /////////////
                /////////////                // Create ElApplicationWindow Form
                /////////////                var configFullFileName = Path.Combine(this.BaseDirectory, this.ConfigurationFileName);
                /////////////
                /////////////                this.ElectrifierForm = new electrifier.Core.Forms.ElApplicationWindow(AppContext.Icon);

                // TODO: If file exists, but is invalid (e.g. empty), this will crash...
                //                if (File.Exists(configFullFileName))
                //                    this.ElectrifierForm.LoadConfiguration(configFullFileName);
                //
                // TODO: 23/03/20: Using the Default-Session actually results in a file named 'Session.Default.xml',
                //                 which holds the DockPanelSuite-Configuration.
                // TODO:           We will remove this file, and use SQLite instead for storing this configuration.
                //                 Opening an electrifier window results in a Session-Object.
                // TODO:           Each Workbench can have multiple Sessions, but a Session doesn't need to have a Workbench
                //                 (i.e. the Default Workbench, which means a new, aka an empty Workbench).
                // TODO:           Sessions on the other side can derive from multiple Workbenches; thus, a Workbench is just
                //                 a kind of "Session-Template": e.g. The Workbench named "Listen to music" is a ShellBrowser
                //                 navigating to "Music", the Workbench "View my Pictures" to "Pictures",
                //                 "Work on Documents" to "Documents" etc...
                // TODO:           A running session can also import (aka add) another Workbench with ease.

            }

            /// <summary>
            /// Save ElApplicationWindow Form state configuration into XML-file.
            /// 
            /// Called by AppContext.AppContext_ThreadExit()
            /// </summary>
            public void SaveConfiguration()
            {
//                var fullFileName = Path.Combine(this.BaseDirectory, this.ConfigurationFileName);
//
//                // Create directory for configuration file, just in case it doesn't already exist
//                Directory.CreateDirectory(this.BaseDirectory);
//
//                this.ElectrifierForm.SaveConfiguration(fullFileName);
            }
        }
    }
}
