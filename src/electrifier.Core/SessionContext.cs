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
using electrifier.Core.Forms;
using EntityLighter;
using Microsoft.Data.Sqlite;
using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

// TODO: By using 'sqlite3_column_int()' we could retrieve 32-bit-integers
// TODO: We should use prepared statements:
//      https://stackoverflow.com/questions/16711386/getting-int-values-from-sqlite
//      https://stackoverflow.com/questions/27383724/sqlite3-prepare-v2-sqlite3-exec


namespace electrifier.Core
{
    #region = SessionEntity ====================================================================================================

    [Table(Name = "Session")]
    public class SessionEntity
    {
        [PrimaryKey(DataType.Integer)]
        public long Id { get; protected set; }

        [Column(DataType.Text, Constraints = Constraint.NotNull)]
        public string Name { get; protected set; }

        [Column(DataType.Text)]
        public string Description { get; protected set; }

        [Column(DataType.Integer, Constraints = Constraint.NotNull, DefaultValue = "CURRENT_TIMESTAMP")]
        public DateTime DateCreated { get; protected set; }

        [Column(DataType.Integer, Constraints = Constraint.NotNull, DefaultValue = "CURRENT_TIMESTAMP")]
        public DateTime DateModified { get; protected set; }

        private SessionEntity(SqliteDataReader dataReader)
        {
            this.Id = (long)dataReader[nameof(this.Id)];
            this.Name = (string)dataReader[nameof(this.Name)];
            this.Description = (dataReader[nameof(this.Description)] is DBNull ? string.Empty : (string)dataReader[nameof(this.Description)]); // TODO: Put Null-Handling into EntityLighter
            this.DateCreated = DataContext.ConvertToDateTime((string)dataReader[nameof(this.DateCreated)]);
            this.DateModified = DataContext.ConvertToDateTime((string)dataReader[nameof(this.DateModified)]);

            //this.Properties = new PropertyCollection(dataContext);
        }

        public static SessionEntity LoadStoredSession(DataContext dataContext, long id)
        {
            EntitySet<SessionEntity> entities = LoadStoredSessions(dataContext, Where: $"Id = { id }");

            if (1 == entities.Count)
                return entities.First();

            return null;        // TODO: Remove, throw exception
        }

        public static EntitySet<SessionEntity> LoadStoredSessions(DataContext dataContext, string Where = null, string OrderBy = null)
        {
            string statement = $"SELECT Id, Name, Description, DateCreated, DateModified FROM Session";

            if (!string.IsNullOrWhiteSpace(Where))
                statement += " WHERE " + Where;

            if (!string.IsNullOrWhiteSpace(OrderBy))
                statement += " ORDER BY " + OrderBy;

            return new EntitySet<SessionEntity>(dataContext).Load(statement,
                (sqliteDataReader) =>
                {
                    return new SessionEntity(sqliteDataReader);
                });
        }
    }

    #endregion

    /*
     * In case we want to provide multi-window-support in the future, see this:
     * 
     * Siehe: https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.applicationcontext.mainform?view=netframework-4.8
     * This property determines the main Form for this context. This property can change at any time. If OnMainFormClosed is not overridden,
     * the message loop of the thread terminates when the mainForm parameter closes.
     */

    public class SessionContext
    {

    //    /// <summary>
    //    /// For correct mapping, see: https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/sql/linq/how-to-map-database-relationships
    //    /// </summary>
    //    //[Association(Storage = "properties", OtherKey = "SessionID")]
    //    [Association(OtherKey = "SessionID")]
    //    public EntitySet<SessionProperty> Properties
    //    {
    //        get;
    //        private set; //set { this.properties.Assign(value); }
    //    }


    //    /*
    ///*
    //    private EntitySet<Order> _Orders;
    //    [Association(Storage = "_Orders", OtherKey = "CustomerID")]
    //    public EntitySet<Order> Orders
    //    {
    //        get { return this._Orders; }
    //        set { this._Orders.Assign(value); }
    //    }
    // * */




    //    //[Column(DataType.Text)]
    //    //public string DockPanelLayout { get; }
    //    // -- OR, BETTER --
    //    //internal IDockContentEntity[] DockContents { get; }     // TODO: So in etwa?


        #region Properties ====================================================================================================

        public bool IsIncognito { get; }

        public string BaseDirectory { get; }

        public string DataContextStorage { get { return $"{ this.BaseDirectory }\\electrifier.config"; } }
        public DataContext DataContext { get; }
        public GlobalConfig GlobalConfig { get; }
        public Icon ApplicationIcon { get; private set; }
        public ElApplicationWindow ApplicationWindow { get; private set; }

        public long Id => this.Session.Id;
        public string Name => this.Session.Name;

        private SessionEntity session;      // Property backing field

        public SessionEntity Session
        {
            get
            {
                return this.session ?? throw new InvalidOperationException("Currently No Session set");
            }
            private set
            {
                if (this.session != null)
                    throw new InvalidOperationException("Session already set and must not be reassigned!");

                this.session = value;
            }
        }
        public bool HasSession => this.session != null;


        public EntitySet<SessionEntity> PreviousSessions => SessionEntity.LoadStoredSessions(this.DataContext, OrderBy: "DateModified DESC");

        public PropertyCollection Properties
        {
            get;
            private set; //set { this.properties.Assign(value); }
        }

        public SessionSelector SessionSelector { get; private set; }
        public bool HasSessionSelector => this.SessionSelector != null;




        #endregion ============================================================================================================

        #region Fields ========================================================================================================


        private Form currentMainForm;

        #endregion Fields =====================================================================================================

        #region Events ========================================================================================================

        /// <summary>
        /// </summary>
        public event EventHandler<MainFormChangeEventArgs> MainFormChange;

        private void OnMainFormChange(Form newMainForm)
        {
            if (this.currentMainForm != newMainForm)
            {
                AppContext.TraceScope();

                this.MainFormChange?.Invoke(this, new MainFormChangeEventArgs(this.currentMainForm, newMainForm));

                this.currentMainForm = newMainForm;
            }
        }

        #endregion ============================================================================================================




        public SessionContext(Icon appIcon, string baseDirectory, bool isIncognito)
        {
            this.ApplicationIcon = appIcon;
            this.BaseDirectory = baseDirectory;
            this.IsIncognito = isIncognito;

            try
            {
                // TODO: Incognito => read only db
                // TODO: db may be locked by DB Browser! electrifier itself doesn't lock the DB.
                // TODO: Param: "/Incognito" - Dont' save session configuration on application exit... Thus, use In-Memeory-DB which will be loaded, but NOT saved
                this.DataContext = new EntityLighter.DataContext(this.DataContextStorage);
                this.GlobalConfig = new GlobalConfig(this.DataContext);


                this.DataContext.CreateEntityModel(typeof(ConfigurationEntity));
                this.DataContext.CreateEntityModel(typeof(SessionEntity));
                this.DataContext.CreateEntityModel(typeof(SessionProperty));

            }
            catch (Exception)
            {
                // TODO: Switch to incognito mode
                throw;
            }
        }


        // TODO: Overhaul CreateIntitalForm to avoid Cohesion with AppContext


        public void CreateInitialForm(AppContext appContext)
        {
            var test = (int)this.GlobalConfig.DefaultSession;


            if (this.GlobalConfig.DefaultSession.IsNull)
            {
                this.SessionSelector = new SessionSelector(this);
                this.SessionSelector.StartNewSession += this.SessionSelector_StartNewSession;
                this.SessionSelector.ContinueSession += this.SessionSelector_ContinueSession;

                this.OnMainFormChange(this.SessionSelector);
            }
            else
            {
                this.LoadSession((int)this.GlobalConfig.DefaultSession);
            }

            // TODO: Nochmal kucken, ob das hin und hercasten nicht doch zu fehleranfällig ist...
            // TODO: Testen mit NULL
        }

        /// <summary>
        /// Load <see cref="SessionEntity"/> with given <paramref name="sessionId"/>, build and show the main window
        /// and finally dispose <see cref="Forms.SessionSelector"/> if it was in use.
        /// </summary>
        /// <param name="sessionId"></param>
        private void LoadSession(long sessionId)
        {
            this.Session = SessionEntity.LoadStoredSession(this.DataContext, sessionId);

            // Set default properties for this session
            this.Properties = new PropertyCollection(this);

            this.ApplicationWindow = new ElApplicationWindow(this);       // Invoke()?!?  // TODO: Dispose?
            this.OnMainFormChange(this.ApplicationWindow);
            //this.ApplicationWindow.Show();

            if (this.HasSessionSelector)
            {
                this.SessionSelector.Close();
                this.SessionSelector.Dispose();
                this.SessionSelector = null;
            }
        }

        private void SessionSelector_StartNewSession(object sender, StartNewSessionEventArgs args)
        {
            if (args is null)
                throw new ArgumentNullException(nameof(args));

            AppContext.TraceDebug($"Creating new session {args.Name}: '{args.Description}'");

            // TODO: Check those SQL-Values!

            long sessionId = this.DataContext.CreateNewEntity(typeof(SessionEntity), (sqlCmd) =>
            {
                sqlCmd.CommandText = $"INSERT INTO Session (Name, Description) VALUES ($Name, $Description)";
                sqlCmd.Parameters.AddWithValue("$Name", args.Name);
                sqlCmd.Parameters.AddWithValue("$Description", args.Description);
            });

            this.LoadSession(sessionId);

            // Initialize new Session: Create default DockContentWindows
            DockContentFactory.CreateShellBrowser(this.ApplicationWindow);

            this.ApplicationWindow.Show();

            if (args.SetAsDefault)
                this.GlobalConfig.DefaultSession.Value = this.Session.Id;
        }

        private void SessionSelector_ContinueSession(object sender, ContinueSessionEventArgs args)
        {
            this.LoadSession(args.SessionId);

            this.ApplicationWindow.Show();

            if (args.SetAsDefault)
                this.GlobalConfig.DefaultSession.Value = this.Session.Id;
        }



        //public ElApplicationWindow Run(SessionEntity session)
        //{
        //    this.session = session;

        //    // Create main Application Window
        //    this.ApplicationWindow = new ElApplicationWindow(this);

        //    // Set default properties for this session
        //    this.Properties = new PropertyCollection(this);



        //    //TypeConverter pointConverter = TypeDescriptor.GetConverter(typeof(Point));
        //    //Point windowPos = (Point)pointConverter.ConvertFromString(this.Properties.SyncProperty("WindowPos", "0, 0"));
        //    //this.ApplicationWindow.Location = windowPos;

        //    //TypeConverter sizeConverter = TypeDescriptor.GetConverter(typeof(Size));
        //    //Size windowSize = (Size)sizeConverter.ConvertFromString(this.Properties.SyncProperty("WindowSize", "800, 600"));
        //    //this.ApplicationWindow.Size = windowSize;


        //    // Initialize session object
        //    //
        //    // TODO: Check if another instance is already running. If so, create new session with different name and fresh settings; optionally copy default session to new session settings!
        //    //
        //    // TODO: Create new default shellbrowser - for test purposes only
        //    DockContentFactory.CreateShellBrowser(this.ApplicationWindow);





        //    //////var windowPos = this.Properties.SyncProperty("WindowPos", "0, 0");
        //    //////var windowSize = this.Properties.SyncProperty("WindowSize", "800, 600");

        //    //////char[] delimiterChars = { ' ', ',' };
        //    //////var values = windowPos.Split(delimiterChars, StringSplitOptions.RemoveEmptyEntries);

        //    //////this.ApplicationWindow.StartPosition = FormStartPosition.Manual;
        //    //////this.ApplicationWindow.Location = new PointConverter()

        //    /*
        //        // Create the PointConverter.
        //        System.ComponentModel.TypeConverter converter = 
        //            System.ComponentModel.TypeDescriptor.GetConverter(typeof(Point));

        //        Point point1 = (Point) converter.ConvertFromString("200, 200");

        //        // Use the subtraction operator to get a second point.
        //        Point point2 = point1 - new Size(190, 190);

        //        // Draw a line between the two points.
        //        e.Graphics.DrawLine(Pens.Black, point1, point2);
        //    */




        //    //// Set default properties for this session

        //    //this.Properties.Add(new SessionProperty(this, "WindowPosition", "10, 10"));

        //    //this.AddSessionProperty("WindowPosition", "0, 0");      // TODO: "CenterScreen", etc...
        //    //this.AddSessionProperty("WindowSize", "800, 600");


        //    /* Unit-Test
        //                var test1 = this.Properties.SyncProperty("WindowPosition", "0, 0");
        //                var test2 = this.Properties.SyncProperty("Pos", "0, 0");
        //                this.Properties.SafeSetProperty("Pos", "-10, -10");
        //                var test3 = this.Properties.SyncProperty("Pos", "10, 10");
        //                this.Properties.SafeSetProperty("Pos", "-20, -20");
        //                var test4 = this.Properties.SyncProperty("Pos", "20, 20");
        //                this.Properties.SafeSetProperty("Pos", "-30, -30");
        //                var test5 = this.Properties.SyncProperty("Pos", "30, 30");
        //    */



        //    //            var test = this.Session.Properties.Where(property => property.Key == "WindowPosition");
        //    //var test4 = this.Session.Properties.Contains("WindowPosition"); // comparer: //IEqualityComparer )
        //    //var test1 = this.Session.Properties.SingleOrDefault(p => p.Key.Equals("Windowzosition", StringComparison.InvariantCultureIgnoreCase));
        //    //var test2 = this.Session.Properties.SingleOrDefault(p => p.Key.Equals("WindowPosition", StringComparison.InvariantCultureIgnoreCase));
        //    //var test3 = this.Session.Properties.SingleOrDefault(p => p.Key.Equals("Windowposition", StringComparison.InvariantCultureIgnoreCase));

        //    ////            var test = this.Properties.Where(property => property.Key == "WindowPosition");
        //    ////            var test2 = this.Properties.SingleOrDefault(p => p.Key.Equals("WindowPosition2", StringComparison.InvariantCulture));
        //    ///




        //    //var test = this.GetSessionProperty("WindowPosition");


        //    //if (GetPropertyCount("WindowPosition") > 0)
        //    //{
        //    //    var prop = this.GetSessionProperty("WindowPosition");
        //    //}
        //    //else
        //    //{
        //    //    var prop = new SessionProperty(this, "WindowPosition", "0, 0");
        //    //    //this.Session.Properties.Add(new SessionProperty("WindowPosition", ))
        //    //}



        //    //SessionProperty propWindowPosition = new SessionProperty(this, "WindowPosition", "0, 0");











        //    //this.Name = $"Session on { DateTime.Now.DayOfWeek }";         // TODO: Put into config!

        //    //this.Id = this.DataContext.CreateNewEntity(typeof(SessionContext), (sqlCmd) =>
        //    //{
        //    //    sqlCmd.CommandText = $"INSERT INTO Session (Name) VALUES ($Name)";
        //    //    sqlCmd.Parameters.AddWithValue("$Name", this.Name);
        //    //});

        //    //// Initialize properties
        //    //this.Properties = new EntitySet<SessionProperty>(this.DataContext).Load(
        //    //    $"SELECT Id, SessionId, Key, Value FROM SessionProperty WHERE SessionID = { this.Id }", (sqliteDataReader) =>
        //    //    {
        //    //        return new SessionProperty(this, sqliteDataReader);
        //    //    });

        //    //// Set default properties for this session
        //    //this.AddSessionProperty("WindowPosition", "0, 0");      // TODO: "CenterScreen", etc...
        //    //this.AddSessionProperty("WindowSize", "800, 600");










        //    #region Test
        //    //var newProp = new SessionProperty(this, "WindowPosition", "100, 100");
        //    //this.Properties.Add(newProp);
        //    //var newProp2 = new SessionProperty(this, "WindowSize", "640, 480");
        //    //this.Properties.Add(newProp2);

        //    //var test1 = newProp.Equals(newProp);
        //    //var test2 = newProp.Equals(newProp2);
        //    //var test3 = newProp2.Equals(newProp);
        //    //var test4 = newProp2.Equals(newProp2);

        //    //var test5 = this.Properties.Contains(newProp);
        //    //var test6 = this.Properties.Contains(newProp2);

        //    //newProp2.Value = "Very new value!";
        //    ////this.Properties.Remove(newProp);
        //    ////this.Properties.Remove(newProp2);
        //    #endregion

        //    return this.ApplicationWindow;
        //}




        //        public Form InitializeMainForm(Icon appIcon)        // => RunSession
        //        {
        //            this.ApplicationIcon = appIcon;
        //            this.ApplicationWindow = new Forms.ElApplicationWindow(this, appIcon);
        //
        //            //this.ApplicationWindow.Size = new Size();
        //            //this.ApplicationWindow.Location = new Point();
        //
        ////            var test = this.Properties.Where(property => property.Key == "WindowPosition");
        ////            var test2 = this.Properties.SingleOrDefault(p => p.Key.Equals("WindowPosition2", StringComparison.InvariantCulture));
        //
        //                //< "WindowPosition" >;
        //
        //
        //
        //            // TODO: Create new shellbrowser - for test purposes only
        //            DockContentFactory.CreateShellBrowser(this.ApplicationWindow);
        //
        //
        //
        //
        //
        //
        //
        //
        //            // TODO: Load default values for session:
        //            // TODO: Test-Code:
        //
        //
        //            // TODO: Zuletzt geänderte Session - nicht die zuletzt angelegte! - als Default
        //
        //
        //            // TODO: 19/04/20: As long as we don't use a session-selector, use session #1
        //            // ...or select(max(id)) from session...
        //            //                SessionEntity sessionEntity = new SessionEntity(AppContext.ElEntityStore);
        //
        //
        //
        //
        //
        //            // TODO: If file exists, but is invalid (e.g. empty), this will crash...
        //            //                if (File.Exists(configFullFileName))
        //            //                    this.ElectrifierForm.LoadConfiguration(configFullFileName);
        //            //
        //            // TODO: 23/03/20: Using the Default-Session actually results in a file named 'Session.Default.xml',
        //            //                 which holds the DockPanelSuite-Configuration.
        //            // TODO:           We will remove this file, and use SQLite instead for storing this configuration.
        //            //                 Opening an electrifier window results in a Session-Object.
        //            // TODO:           Each Workbench can have multiple Sessions, but a Session doesn't need to have a Workbench
        //            //                 (i.e. the Default Workbench, which means a new, aka an empty Workbench).
        //            // TODO:           Sessions on the other side can derive from multiple Workbenches; thus, a Workbench is just
        //            //                 a kind of "Session-Template": e.g. The Workbench named "Listen to music" is a ShellBrowser
        //            //                 navigating to "Music", the Workbench "View my Pictures" to "Pictures",
        //            //                 "Work on Documents" to "Documents" etc...
        //            // TODO:           A running session can also import (aka add) another Workbench with ease.
        //
        //
        //            return this.ApplicationWindow;
        //        }
        //
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

    #region EventArgs =========================================================================================================

    /// <summary>
    /// Event argument for the <see cref="SessionSelector.StartNewSession"/> event.
    /// </summary>
    public class MainFormChangeEventArgs : EventArgs
    {
        public MainFormChangeEventArgs(Form oldMainForm, Form newMainForm)
        {
            this.OldMainForm = oldMainForm;
            this.NewMainForm = newMainForm;
        }

        public Form OldMainForm { get; }
        public Form NewMainForm { get; }
    }

    #endregion ================================================================================================================



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
            this.SessionContext = sessionContext ??
                throw new ArgumentNullException(nameof(sessionContext));
            if (null == sqliteData)
                throw new ArgumentNullException(nameof(sqliteData));

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
        /// <param name="otherProperty">A <see cref="SessionProperty"/> to compare with this object.</param>
        /// <returns>
        /// <c>true</c> if the current object is equal to the other parameter; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(SessionProperty otherProperty)
        {
            if (null == otherProperty)
                return false;

            return (0 == string.Compare(this.Key, otherProperty.Key, ignoreCase: true, CultureInfo.InvariantCulture));
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

    /// <summary>
    /// PropertyCollection
    /// </summary>
    public class PropertyCollection
      : EntitySet<SessionProperty>
    {
        public SessionContext SessionContext { get; }
        public StringComparison StringComparison { get; set; } = StringComparison.InvariantCultureIgnoreCase;

        public PropertyCollection(SessionContext sessionContext)
          : base(sessionContext?.DataContext ?? throw new ArgumentNullException(nameof(sessionContext)))
        {
            this.SessionContext = sessionContext;

            this.Load(
                $"SELECT Id, SessionId, Key, Value FROM SessionProperty WHERE SessionID = { this.SessionContext.Id }",
                (sqliteDataReader) =>
                {
                    return new SessionProperty(this.SessionContext, sqliteDataReader);
                });
        }


        public bool Contains(string propertyKey) => this.IndexOf(propertyKey) >= 0;

        public int IndexOf(string propertyKey)
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (entities[i].Key.Equals(propertyKey, this.StringComparison))
                    return i;
            }

            return -1;
        }

        public string SyncProperty(string propertyKey, string defaultValue)
        {
            int index = this.IndexOf(propertyKey);

            if (index >= 0)
            {
                return this.entities[index].Value;
            }
            else
            {
                this.Add(new SessionProperty(this.SessionContext, propertyKey, defaultValue));

                return defaultValue;
            }
        }

        public void SafeSetProperty(string propertyKey, string value)
        {
            int index = this.IndexOf(propertyKey);

            if (index >= 0)
            {
                this.entities[index].Value = value;
            }
            else
            {
                this.Add(new SessionProperty(this.SessionContext, propertyKey, value));
            }
        }
    }

    #endregion ================================================================================================================
}
