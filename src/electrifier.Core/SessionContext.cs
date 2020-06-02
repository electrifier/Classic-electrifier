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

using electrifier.Core.Components;
using EntityLighter;
using Microsoft.Data.Sqlite;
using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace electrifier.Core
{
    /*
     * In case we want to provide multi-window-support in the future, see this:
     * 
     * Siehe: https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.applicationcontext.mainform?view=netframework-4.8
     * This property determines the main Form for this context. This property can change at any time. If OnMainFormClosed is not overridden,
     * the message loop of the thread terminates when the mainForm parameter closes.
     */
    [Table(Name = "Session")]
    public class SessionContext
    {
        #region ILightedEntity ================================================================================================

        /// <summary>
        /// TODO: The following implements ILightedEntity
        /// </summary>

        [PrimaryKey(DataType.Integer)]
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

        /// <summary>
        /// For correct mapping, see: https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/sql/linq/how-to-map-database-relationships
        /// </summary>
        //[Association(Storage = "properties", OtherKey = "SessionID")]
        [Association(OtherKey = "SessionID")]
        public EntitySet<SessionProperty> Properties
        {
            get;
            private set; //set { this.properties.Assign(value); }
        }


        /*
    /*
        private EntitySet<Order> _Orders;
        [Association(Storage = "_Orders", OtherKey = "CustomerID")]
        public EntitySet<Order> Orders
        {
            get { return this._Orders; }
            set { this._Orders.Assign(value); }
        }
     * */




        //[Column(DataType.Text)]
        //public string DockPanelLayout { get; }
        // -- OR, BETTER --
        //internal IDockContentEntity[] DockContents { get; }     // TODO: So in etwa?

        #endregion ============================================================================================================

        #region Properties ====================================================================================================

        public bool IsIncognito { get; }

        public string BaseDirectory { get; }

        public string DataContextStorage { get { return $"{ this.BaseDirectory }\\electrifier.config"; } }
        public DataContext DataContext { get; }





        public Icon ApplicationIcon { get; private set; }
        public Forms.ElApplicationWindow ApplicationWindow { get; private set; }




        ///// <summary>
        ///// Neuer kot, 17.05.2020
        ///// </summary>
        //public readonly List<SessionContext> KnownSessions;

        #endregion ============================================================================================================

        #region Fields ========================================================================================================


        #endregion Fields =====================================================================================================



        public SessionContext(string baseDirectory, bool isIncognito)
        {
            this.BaseDirectory = baseDirectory;
            this.IsIncognito = isIncognito;

            // TODO: Incognito => read only db
            // TODO: db may be locked by DB Browser! electrifier itself doesn't lock the DB.
            // TODO: Param: "/Incognito" - Dont' save session configuration on application exit... Thus, use In-Memeory-DB which will be loaded, but NOT saved
            this.DataContext = new EntityLighter.DataContext(this.DataContextStorage);

            //// Initialize session object
            ////
            //// TODO: Check if another instance is already running. If so, create new session with different name and fresh settings; optionally copy default session to new session settings!
            ////

//            if (!DataContext.TableExists(this))
//                this.DataContext.CreateEntityModel(typeof(SessionContext));
            this.DataContext.CreateEntityModel(typeof(SessionContext));
            this.DataContext.CreateEntityModel(typeof(SessionProperty));

            this.Name = $"Session on { DateTime.Now.DayOfWeek }";         // TODO: Put into config!

            this.Id = this.DataContext.CreateNewEntity(typeof(SessionContext), (sqlCmd) =>
            {
                sqlCmd.CommandText = $"INSERT INTO Session (Name) VALUES ($Name)";
                sqlCmd.Parameters.AddWithValue("$Name", this.Name);
            });

            ///
            /// EntitySet<SessionProperty>
            /// 
            this.Properties = new EntitySet<SessionProperty>(this.DataContext).Load(
                $"SELECT Id, SessionId, Key, Value FROM SessionProperty WHERE SessionID = { this.Id };", (sqliteDataReader) =>
            {
                return new SessionProperty(this, sqliteDataReader);
            });



            /////////// Using LINQ
            this.Properties.Add(new SessionProperty(this, "WindowPosition", "100, 100"));
            this.Properties.Add(new SessionProperty(this, "WindowSize", "640, 480"));

            //var test = this.Properties.Select("")
            //var test = this.Properties.Contains("WindowSize");




            #region Test
            //var newProp = new SessionProperty(this, "WindowPosition", "100, 100");
            //this.Properties.Add(newProp);
            //var newProp2 = new SessionProperty(this, "WindowSize", "640, 480");
            //this.Properties.Add(newProp2);

            //var test1 = newProp.Equals(newProp);
            //var test2 = newProp.Equals(newProp2);
            //var test3 = newProp2.Equals(newProp);
            //var test4 = newProp2.Equals(newProp2);

            //var test5 = this.Properties.Contains(newProp);
            //var test6 = this.Properties.Contains(newProp2);

            //newProp2.Value = "Very new value!";
            ////this.Properties.Remove(newProp);
            ////this.Properties.Remove(newProp2);
            #endregion






        }

        public Form InitializeMainForm(Icon appIcon)        // => RunSession
        {
            this.ApplicationIcon = appIcon;
            this.ApplicationWindow = new Forms.ElApplicationWindow(appIcon);






            // TODO: Create new shellbrowser - for test purposes only

            ElDockContentFactory.CreateShellBrowser(this.ApplicationWindow);

            // TODO: Load default values for session:
            // TODO: Test-Code:
            // TODO: Über die DockContentFactory erzeugen!
            //DockContentEntity dockContent = new DockContentEntity(this /* ContentType ShellBrowser als type */);

            // TODO: Zuletzt geänderte Session - nicht die zuletzt angelegte! - als Default


            // TODO: 19/04/20: As long as we don't use a session-selector, use session #1
            // ...or select(max(id)) from session...
            //                SessionEntity sessionEntity = new SessionEntity(AppContext.ElEntityStore);





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


            return this.ApplicationWindow;
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


    #region SessionProperty ===================================================================================================

    [Table(Name = "SessionProperty")]
    public class SessionProperty
      : IEquatable<SessionProperty>
    {
        public DataContext DataContext { get => this.SessionContext?.DataContext; }
        public SessionContext SessionContext { get; }

        [PrimaryKey(DataType.Integer)]
        public long Id { get; }

        [Column(DataType.Integer)] // TODO: This is the ForeignKey
        public long SessionID { get; }

        [Column(DataType.Text)]
        public string Key { get; }

        private string value;
        [Column(DataType.Text)]
        public string Value {
            get => this.value;
            set
            {
                if (this.value != value)
                {
                    // TODO: this.DataContext.UpdateEntity(this, "Value = '{ value }'" WHERE ..."); INCLUDING ERROR-Handling
                    // TODO: We have to use Transactions in general for Updates
                    if (1 == this.DataContext.ExecuteNonQuery($"UPDATE SessionProperty SET Value = '{ value }' WHERE Id = '{ this.Id }' AND SessionId = '{ this.SessionID}'"))
                        this.value = value;
                    else
                        throw new Exception("Updating SessionProperty failed!");
                }
            }
        }

        public SessionProperty(SessionContext sessionContext, string name, string value)
        {
            this.SessionContext = sessionContext ?? throw new ArgumentNullException(nameof(sessionContext));
            this.SessionID = sessionContext.Id;
            this.Key = name;
            this.value = value;

            this.Id = this.DataContext.CreateNewEntity(typeof(SessionProperty), (sqlCmd) =>
            {
                sqlCmd.CommandText = "INSERT INTO SessionProperty (SessionID, Key, Value) Values ($SessionID, $Key, $Value)";
                sqlCmd.Parameters.AddWithValue("$SessionID", SessionContext.Id);
                sqlCmd.Parameters.AddWithValue("$Key", name);
                sqlCmd.Parameters.AddWithValue("$Value", value);
            });
        }

        public SessionProperty(SessionContext sessionContext, SqliteDataReader sqliteData)
        {
            this.SessionContext = sessionContext ?? throw new ArgumentNullException(nameof(sessionContext));
            if (null == sqliteData) throw new ArgumentNullException(nameof(sqliteData));

            this.Id = (long)sqliteData[0];
            this.SessionID = (long)sqliteData[1];
            this.Key = (string)sqliteData[2];
            this.Value = (string)sqliteData[3];

        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <remarks>
        /// For objects of type <see cref="SessionProperty"/> this means that only their names are compared, not their values.
        /// </remarks>
        /// <param name="otherSession">A <see cref="SessionProperty"/> to compare with this object.</param>
        /// <returns>
        /// <c>true</c> if the current object is equal to the other parameter; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(SessionProperty otherSession)
        {
            if (null == otherSession)
                return false;

            return (0 == string.Compare(this.Key, otherSession.Key, ignoreCase: true, CultureInfo.InvariantCulture));
        }

        public override bool Equals(object otherObject)       // This compares System.Object, not System.IEquatable
        {
            if ((null == otherObject) || (!(otherObject is SessionProperty otherSession)))
                return false;

            return this.Equals(otherSession);
        }

        // TODO: operator == to compare value AND key; CompareTo-Method

        public override int GetHashCode() => this.Key.GetHashCode();
    }

    #endregion ================================================================================================================
}
