using System;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;

using Electrifier.Core.Forms;
using Electrifier.Core.Services;
using Electrifier.Core.Shell32.Services;

namespace Electrifier.Core {
	/// <summary>
	/// Zusammenfassung für AppContext.
	/// </summary>
	public sealed class AppContext : ApplicationContext, IPersistentFormContainer {
		// Static member variables and properties
		private static AppContext instance       = null;
		public  static AppContext Instance       { get { return instance; } }
		private static Icon       icon           = null;
		public  static Icon       Icon           { get { return icon; } }
		private static Bitmap     logo           = null;
		public  static Bitmap     Logo           { get { return logo; } }
		private static ArrayList  openWindowList = new ArrayList();
		public  static ArrayList  OpenWindowList { get { return openWindowList; } }
		private static NotifyIcon notifyIcon     = new NotifyIcon();
		public  static NotifyIcon NotifyIcon     { get { return notifyIcon; } }

		/// <summary>
		/// The default constructor of the AppContext.
		/// Note that this class is only allowed to be instantiated once!
		/// Since Electrifier.exe does this, don't instantiate this class for yourself!
		/// Use its static <c>Instance</c> property instead, if you need access to its members.
		/// </summary>
		/// <param name="args">The argument list given when starting the application</param>
		/// <param name="appIcon">The icon resource used by this application</param>
		/// <param name="appLogo">The logo resource used by this application</param>
		/// <param name="splashScreenForm">The form representing the logo as splash screen</param>
		public AppContext(string[] args, Icon appIcon, Bitmap appLogo, Form splashScreenForm) {
			bool showMainWindow = true;

			// Initialize application context
			instance = RegisterAppContextInstance(this);
			icon     = appIcon;
			logo     = appLogo;

			// Initialize basic services
			ServiceManager.Services.AddService(new DesktopFolderInstance());
			ServiceManager.Services.AddService(new IconManager());
			ServiceManager.Services.AddService(new PIDLManager());

			// Search argument list for MainWindow-related arguments
			foreach(string arg in args) {
				if(arg.ToLower().Equals("/nomainwindow")) {
					showMainWindow = false;
				}
			}

			// Create NotifyIcon
			notifyIcon.Icon = new Icon(appIcon, 16, 16);	// TODO: width & height from regisry?!?
			notifyIcon.Visible = true;

//						// Open new MainWindow, if not diabled
//						if(showMainWindow) {
//							MainWindowForm mainWindowForm = new MainWindowForm();
//							mainWindowForm.Show();
//			
//							openWindowList.Add(mainWindowForm);
//			
//							// TODO: Action definieren, die neuen Electrifier aufmacht...
//							// TODO: Config auslesen, wenn gewuenscht this.OnMainFormClosed checken und
//							//       application am leben erhalten (Dialog "You closed all electrifier windows.
//							//       do you want to stay electrifier active in tray" blablablubb)
//							this.MainForm = mainWindowForm;
//						}

			// Try to load and apply the configuration used for this session
            if (!RestoreConfiguration()) {
                // Start a new session since restoring configuration failed
                MainWindowForm mainWindowForm = new MainWindowForm();
                mainWindowForm.Show();

                openWindowList.Add(mainWindowForm);
                this.MainForm = mainWindowForm;
            }

			if(showMainWindow) {
				// TODO: Wenn alle minimiert waren werden sie es auch diesmal sein!
				Form form = openWindowList[0] as Form;

				// TODO: Das aktive dokument wird das mainform...
				form.Show();
				MainForm = form;
			}

			// Add handler to save configuration before closing
			ThreadExit += new EventHandler(AppContext_ThreadExit);

			// Finally close splash screen
			splashScreenForm.Close();
		}

		private void AppContext_ThreadExit(object sender, EventArgs e) 
		{
			// Save configuration file
			if(true == true) 
			{	// TODO: uebergabeparameter zum ausschalten & exceptions checken
				SaveConfiguration();
			}

			// Destroy NotifyIcon
			AppContext.NotifyIcon.Visible = false;
			AppContext.NotifyIcon.Dispose();
		}

		private AppContext RegisterAppContextInstance(AppContext appContextInstance) 
		{
			// Check if another instance was already instantiated
			if(instance != null) {
				throw new InvalidOperationException("Electrifier.Core.AppContext.RegisterInstance: " +
					"Tried to register a new instance although there is already one!");
			} else {
				// Everything fine, return the given instance
				return appContextInstance;
			}
		}

		public bool RestoreConfiguration() {
			return RestoreConfiguration("Default.Config.xml");
		}

		public bool RestoreConfiguration(string configurationFileName) {
			try {
                if (File.Exists(configurationFileName)) {
                    XmlDocument configXml = new XmlDocument();
                    configXml.Load(configurationFileName);

                    XmlNode node = configXml.DocumentElement;
                    string text = node.LocalName;
                    ApplyPersistenceInfo(node);

                    return true;
                }
			} catch(Exception e) {
				// TODO: Add error-handling here...

                System.Windows.Forms.MessageBox.Show("AppContext.RestoreConfiguration: Exception occured:" + e.Message, "electrifier: Error!");
			}

            return false;
		}

		public void SaveConfiguration() {
			SaveConfiguration("Default.Config.xml");
		}

		public void SaveConfiguration(string fileName) {
			// Create XmlDocument to store all the configuration parameters
			XmlDocument configXml = new XmlDocument();
			configXml.AppendChild(configXml.CreateComment("\n\n" +
				"Electrifier configuration file\n\n" +
				"This is a machine generated file, created by Electrifier application\n" +
				"For more information about electrifier visit http://www.electrifier.org\n\n" +
				"DO NOT EDIT THIS FILE MANUALLY, CHANGES MAY CAUSE THE\n" +
				"CONTAINED CONFIGURATION INFORMATION BECOME CORRUPTED!\n\n" +
				"Copyright (c) 2015 by electrifier.org and its owners\n\n"));

			// Create the root xml node
			XmlNode      configNode   = configXml.CreateElement("Electrifier");
			XmlAttribute fileTypeAttr = configXml.CreateAttribute("ContentDescription");
			fileTypeAttr.Value        = "Configuration";
			XmlAttribute versionAttr  = configXml.CreateAttribute("FileVersion");
			versionAttr.Value         = "0.1";
			XmlAttribute creationAttr = configXml.CreateAttribute("CreationDateTime");
			creationAttr.Value        = String.Format("{0:s}", DateTime.UtcNow);
			configNode.Attributes.Append(fileTypeAttr);
			configNode.Attributes.Append(versionAttr);
			configNode.Attributes.Append(creationAttr);

			// Append all xml node elements describing the actual configuration
			configNode.AppendChild(CreatePersistenceInfo(configXml));
			configXml.AppendChild(configNode);

			// Write the XmlDocument to file
			fileName                = Application.StartupPath + "\\" + fileName;
			XmlTextWriter xmlWriter = new System.Xml.XmlTextWriter(fileName, new UnicodeEncoding());
			xmlWriter.Formatting    = Formatting.Indented;

			configXml.Save(xmlWriter);
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
		public XmlNode CreatePersistenceInfo(XmlDocument targetXmlDocument) {
			// Create persistence information for the application context
			XmlNode appContextNode  = targetXmlDocument.CreateElement("AppContext");
			XmlNode openWindowsNode = targetXmlDocument.CreateElement("OpenWindows");

			// TODO: add global settings to appcontextnode

			// Append persistence informtion for every open window
			foreach(IPersistentForm window in openWindowList) {
				openWindowsNode.AppendChild(window.CreatePersistenceInfo(targetXmlDocument));
			}

			appContextNode.AppendChild(openWindowsNode);

			return appContextNode;
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
			if(!openWindowList.Contains(persistentForm)) {
				openWindowList.Add(persistentForm);
			} else {
				throw new ArgumentException("Given Form instance already in list of hosted Forms", "persistentForm");
			}
		}

		public void DetachPersistentForm(IPersistentForm persistentForm) {
			if(openWindowList.Contains(persistentForm)) {
				openWindowList.Remove(persistentForm);
			} else {
				throw new ArgumentException("Given Form instance not in list of hosted Forms", "persistentForm");
			}
		}
		#endregion

	}
}
