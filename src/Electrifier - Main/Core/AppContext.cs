using System;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;

using electrifier.Core.Forms;
using electrifier.Core.Services;
using electrifier.Core.Shell32.Services;

namespace electrifier.Core {

	/// <summary>
	/// AppContext acts as a singleton which instantiates all the basic services and gets the user interface started up
	/// 
	/// See https://msdn.microsoft.com/en-us/library/ff650316.aspx for details on implementation of the singleton
	/// </summary>
	public sealed class AppContext : ApplicationContext, IPersistentFormContainer {
		// Static member variables and properties
		private static AppContext instance = null;
		public static AppContext Instance { get { return AppContext.instance; } }
		private static Icon icon = null;
		public static Icon Icon { get { return AppContext.icon; } }
		private static Bitmap logo = null;
		public static Bitmap Logo { get { return AppContext.logo; } }
		private static ArrayList openWindowList = new ArrayList();
		public static ArrayList OpenWindowList { get { return AppContext.openWindowList; } }
		private static NotifyIcon notifyIcon = new NotifyIcon();
		public static NotifyIcon NotifyIcon { get { return AppContext.notifyIcon; } }
		private static bool isPortable = false;
		public static bool IsPortable { get { return AppContext.isPortable; } }
		private static bool isIncognito = false;
		public static bool IsIncognito { get { return AppContext.isIncognito; } }
		private static string configFileName = @"electrifier.config.xml";
		private static string configFullPathFileName {
			get {
				if (AppContext.IsPortable)
					return Path.Combine(Application.StartupPath, AppContext.configFileName);
				else
					return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"electrifier", AppContext.configFileName);
			}
		}

		public const string configFileVersion = "0.1";
		public static string[] CompatibleFileVersions = {
				AppContext.configFileVersion,
			};

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
		public AppContext(string[] args, Icon appIcon, Bitmap appLogo, Form splashScreenForm) {
			bool noMainWindow = false;

			// Initialize application context
			AppContext.instance = RegisterAppContextInstance(this);
			AppContext.icon = appIcon;
			AppContext.logo = appLogo;

			// Initialize basic services
			ServiceManager.Services.AddService(new DesktopFolderInstance());
			ServiceManager.Services.AddService(new IconManager());
			ServiceManager.Services.AddService(new PIDLManager());

			// Search argument list for AppContext-related arguments
			foreach (string arg in args) {
				// NoMainWindow: Don't show main window, just reside in notify tray
				if (arg.ToLower().Equals("/nomainwindow"))
					noMainWindow = true;

				// Portable: Issue #5: Add "-portable" command switch, storing configuration in application directory instead of "LocalApplicationData"
				if (arg.ToLower().Equals("/portable"))
					AppContext.isPortable = true;

				// Incognito: Don't modify configuration file on exit, thus don't make session parameters/changes persistent
				if (arg.ToLower().Equals("/incognito"))
					AppContext.isIncognito = true;
			}

			// Create NotifyIcon
			notifyIcon.Icon = new Icon(appIcon, 16, 16);	// TODO: width & height from regisry?!?
			notifyIcon.Visible = true;

			// TODO: Action definieren, die neuen electrifier aufmacht...
			// TODO: Config auslesen, wenn gewuenscht this.OnMainFormClosed checken und
			//       application am leben erhalten (Dialog "You closed all electrifier windows.
			//       do you want to stay electrifier active in tray" blablablubb)

			// Try to load and apply the configuration used for last session
			if ((false == this.RestoreConfiguration()) || (0 == AppContext.openWindowList.Count)) {
				// Start a new session since restoring configuration failed
				MainWindowForm mainWindowForm = new MainWindowForm();
				AppContext.openWindowList.Add(mainWindowForm);
				this.MainForm = mainWindowForm;
			}

			if (false == noMainWindow)
				(AppContext.openWindowList[0] as Form).Show();

			if(noMainWindow == false) {
				// TODO: Wenn alle minimiert waren werden sie es auch diesmal sein!
				Form form = openWindowList[0] as Form;

				// TODO: Das aktive dokument wird das mainform...
				form.Show();
				MainForm = form;
			}

			// Add ThreadExit-handler to save configuration and dispose NotifyIcon when closing
			ThreadExit += new EventHandler(AppContext_ThreadExit);

			// Finally close splash screen
			splashScreenForm.Close();
			// TODO: splashScreenForm.Dispose();
		}

		private void AppContext_ThreadExit(object sender, EventArgs e) {
			// Save configuration file
			if (AppContext.IsIncognito == false)
				this.SaveConfiguration();

			// Destroy NotifyIcon
			AppContext.NotifyIcon.Visible = false;
			AppContext.NotifyIcon.Dispose();
		}

		private AppContext RegisterAppContextInstance(AppContext appContextInstance) 
		{
			// Check if another instance was already instantiated
			if(instance != null) {
				throw new InvalidOperationException("electrifier.Core.AppContext.RegisterInstance: " +
					"Tried to register a new instance although there is already one!");
			} else {
				// Everything fine, return the given instance
				return appContextInstance;
			}
		}

		public bool RestoreConfiguration() {
			return RestoreConfiguration(AppContext.configFullPathFileName);
		}

		public bool RestoreConfiguration(string configFileName) {
			// Open existing configuration file
			using (var configFileStream = new FileStream(configFileName, FileMode.Open)) {
				using (var xmlReader = new XmlTextReader(configFileStream) { WhitespaceHandling = WhitespaceHandling.None }) {
					xmlReader.MoveToContent();

					//xmlWriter.WriteStartElement("electrifier");
					//xmlWriter.WriteAttributeString("Content", "Configuration");
					//xmlWriter.WriteAttributeString("Version", "0.1");
					//xmlWriter.WriteAttributeString("Created", String.Format("{0:s}", DateTime.UtcNow));

					if (!xmlReader.Name.Equals("electrifier"))
						throw new Exception("XML-Format!");

					if (!xmlReader.GetAttribute("Content").Equals("Configuration"))
						throw new Exception("XML-Content!");

					var version = xmlReader.GetAttribute("FormatVersion");
					if ((null == version) || (!version.Equals(configFileVersion)))
						throw new Exception("XML-Content!");


					// Create persistence information
					//this.CreatePersistenceInfo(xmlWriter);



					//				private static bool IsFormatVersionValid(string formatVersion) {
					//	if (formatVersion == ConfigFileVersion)
					//		return true;

					//	foreach (string s in CompatibleConfigFileVersions)
					//		if (s == formatVersion)
					//			return true;

					//	return false;
					//}

					//string formatVersion = xmlIn.GetAttribute("FormatVersion");
					//if (!IsFormatVersionValid(formatVersion))
					//	throw new ArgumentException(Strings.DockPanel_LoadFromXml_InvalidFormatVersion);


					//if (!xmlReader.GetAttribute(0).Equals("Content")) {
					//	throw new Exception("XML-Format!");
					//}




				}

				////using (var xmlIn = new XmlTextReader(stream) { WhitespaceHandling = WhitespaceHandling.None })
				//using (XmlWriter xmlWriter = XmlWriter.Create(configFileStream, new XmlWriterSettings() {
				//	Encoding = System.Text.Encoding.Unicode,
				//	Indent = true,
				//	IndentChars = "  "
				//})) {
				//}
			}

			return true;
			//try {
			//	if (File.Exists(configFileName)) {
			//		XmlDocument configXml = new XmlDocument();
			//		configXml.Load(configFileName);

			//		XmlNode node = configXml.DocumentElement;
			//		string text = node.LocalName;
			//		ApplyPersistenceInfo(node);

			//		return true;
			//	}
			//} catch (Exception e) {
			//	// TODO: Add error-handling here...

			//	MessageBox.Show("AppContext.RestoreConfiguration:\nError while restoring configuration file: \n\n" + e.Message,
			//		"electrifier: We're sorry, but a runtime error occurred!", MessageBoxButtons.OK, MessageBoxIcon.Error);
			//}

			//return false;
		}

		public void SaveConfiguration() {
			SaveConfiguration(AppContext.configFullPathFileName);
		}

		public void SaveConfiguration(string configFileName) {
			string newConfigFullPathFileName = configFileName + ".new";

			try {
				var newConfigFullPathFileInfo = new FileInfo(newConfigFullPathFileName);

				// Ensure the directory exists
				if (!newConfigFullPathFileInfo.Directory.Exists)
					newConfigFullPathFileInfo.Directory.Create();

				// Create new configuration file
				using (var configFileStream = new FileStream(newConfigFullPathFileName, FileMode.Create)) {
					using (var xmlWriter = XmlWriter.Create(configFileStream, new XmlWriterSettings() {
						Encoding = System.Text.Encoding.Unicode,
						Indent = true,
						IndentChars = "  "
					})) {
						xmlWriter.WriteStartDocument();
						xmlWriter.WriteComment("\n\n" +
							"\telectrifier configuration file\n\n" +
							"\tThis is a machine generated file, created by electrifier application.\n" +
							"\tFor more information about electrifier visit http://www.electrifier.org\n\n" +
							"\tDO NOT EDIT THIS FILE MANUALLY, CHANGES MAY CAUSE THE\n" +
							"\tCONTAINED CONFIGURATION INFORMATION BECOME CORRUPTED!\n\n" +
							"\tCopyright (c) 2016 Thorsten Jung @ electrifier.org and contributors.\n\n");

						// Create root xml node
						xmlWriter.WriteStartElement("electrifier");
						xmlWriter.WriteAttributeString("Content", "Configuration");
						xmlWriter.WriteAttributeString("FormatVersion", "0.1");
						xmlWriter.WriteAttributeString("Created", String.Format("{0:s}", DateTime.UtcNow));

						// Create persistence information
						this.CreatePersistenceInfo(xmlWriter);

						xmlWriter.WriteEndElement(); // electrifier
						xmlWriter.WriteEndDocument();

						xmlWriter.Close();
						configFileStream.Close();
					}
				}

				// If an old version of configuration file exists create backup and replace it by the new one
				try {
					if (File.Exists(configFileName))
						newConfigFullPathFileInfo.Replace(configFileName, (configFileName + ".bak"));
					else
						File.Move(newConfigFullPathFileName, configFileName);
				} catch (Exception e) {
					MessageBox.Show("AppContext.SaveConfiguration: Error while replacing configuration file: \n\n" + e.Message,
						"electrifier: We're sorry, but a runtime error occurred!", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			} catch (Exception e) {
				MessageBox.Show("AppContext.SaveConfiguration: Error while writing configuration file: \n\n" + e.Message,
					"electrifier: We're sorry, but a runtime error occurred!", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		/// <summary>
		/// Creates a new XmlNode-instance, containing the complete given XmlDocumentStream as child nodes
		/// </summary>
		/// <param name="targetXmlDocument"></param>
		/// <param name="localName"></param>
		/// <param name="foreignXmlDocumentStream"></param>
		/// <returns></returns>
		public static XmlNode CreateXmlNodeFromForeignXmlDocument(XmlDocument targetXmlDocument,
			string localName, string foreignXmlDocumentStream) {
			XmlDocument foreignXmlDocument = new XmlDocument();
			foreignXmlDocument.Load(new XmlTextReader(new StringReader(foreignXmlDocumentStream)));

			XmlNode xmlNode = targetXmlDocument.CreateElement(localName);
			xmlNode.AppendChild(targetXmlDocument.ImportNode(foreignXmlDocument.DocumentElement, true));

			return xmlNode;
		}

		/// <summary>
		/// Creates an new string, containing the complete given node childs as XmlDocument-string
		/// </summary>
		/// <param name="sourceXmlDocument"></param>
		/// <param name="parentNode"></param>
		/// <param name="localName">May be null</param>
		/// <returns></returns>
		public static string CreateXmlDocumentFromForeignXmlNode(XmlNode parentNode) {
			return CreateXmlDocumentFromForeignXmlNode(parentNode, null);
		}

		public static string CreateXmlDocumentFromForeignXmlNode(XmlNode parentNode,
			string optionalLocalNameChildNode) {
			StringWriter  stringWriter = new StringWriter();
			XmlTextWriter xmlWriter    = new XmlTextWriter(stringWriter);

			if(optionalLocalNameChildNode != null) {
				parentNode = parentNode.SelectSingleNode(optionalLocalNameChildNode);
			}
			parentNode.WriteContentTo(xmlWriter);

			return stringWriter.ToString();
		}


		#region IPersistentFormContainer Member
		public void CreatePersistenceInfo(System.Xml.XmlWriter xmlWriter) {
			xmlWriter.WriteStartElement("AppContext");
			xmlWriter.WriteStartElement("OpenedWindows");

			//// TODO: add global settings to appcontextnode

			// Append persistence informtion for every open window
			foreach (IPersistentForm persistentForm in AppContext.openWindowList)
				persistentForm.CreatePersistenceInfo(xmlWriter);

			xmlWriter.WriteEndElement(); // OpenedWindows
			xmlWriter.WriteEndElement(); // AppContext
		}

		public void ApplyPersistenceInfo(XmlNode persistenceInfo) {
			XmlNode appContextNode  = persistenceInfo.SelectSingleNode("AppContext");

			// Apply persistence informtion to every open window
			XmlNode openWindowsNode = appContextNode.SelectSingleNode("OpenWindows");
			foreach(XmlNode windowNode in openWindowsNode.ChildNodes) {
				Type windowFormType = Type.GetType(windowNode.LocalName);

				if((windowFormType != null ) && (windowFormType.GetInterface("IPersistentForm") != null)) {
					IPersistentForm windowForm = Activator.CreateInstance(windowFormType) as IPersistentForm;

					windowForm.ApplyPersistenceInfo(windowNode);
					windowForm.AttachToFormContainer(this);
				} else {
					// TODO: Exception
					MessageBox.Show("Unknown window type specified in configuration file");
				}
			}			
		}

		public void AttachPersistentForm(IPersistentForm persistentForm) {
			if(!AppContext.openWindowList.Contains(persistentForm)) {
				AppContext.openWindowList.Add(persistentForm);
			} else {
				throw new ArgumentException("Given Form instance already in list of hosted Forms", "persistentForm");
			}
		}

		public void DetachPersistentForm(IPersistentForm persistentForm) {
			if(AppContext.openWindowList.Contains(persistentForm)) {
				AppContext.openWindowList.Remove(persistentForm);
			} else {
				throw new ArgumentException("Given Form instance not in list of hosted Forms", "persistentForm");
			}
		}
		#endregion IPersistentFormContainer Member

	}
}
