/*
** 
** electrifier
** 
** Copyright (c) 2017 Thorsten Jung @ electrifier.org and contributors
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
using electrifier.Core.Services;
using electrifier.Core.WindowsShell.Services;

namespace electrifier.Core {

	/// <summary>
	/// AppContext acts as a singleton which instantiates all the basic services and gets the user interface started up
	/// 
	/// See https://msdn.microsoft.com/en-us/library/ff650316.aspx for details on implementation of the singleton
	/// </summary>
	public sealed partial class AppContext : ApplicationContext, IPersistentFormContainer {
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

			// Initialize debug listener
			Debug.Listeners.Add(new TextWriterTraceListener(new FileStream((AppContext.IsPortable ?
				(Path.Combine(Application.StartupPath, @"electrifier.debug.log")) :
				(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"electrifier", @"electrifier.debug.log"))),
				FileMode.Append, FileAccess.Write)));
			Debug.AutoFlush = true;
			Debug.WriteLine("electrifier.Core.AppContext: New AppContext created. (" + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + ")");

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
			notifyIcon.Icon = new Icon(appIcon, 16, 16);	// TODO: width & height from registry?!?
			notifyIcon.Visible = true;

			// TODO: Action definieren, die neuen electrifier aufmacht...
			// TODO: Config auslesen, wenn gewuenscht this.OnMainFormClosed checken und
			//       application am leben erhalten (Dialog "You closed all electrifier windows.
			//       do you want to stay electrifier active in tray" blablablubb)

			// Try to load and apply the configuration used for last session

			// TODO: 11.08.2017 -> Wrong XML-Format: Crash!
			if ((false == this.RestoreConfiguration()) || (0 == AppContext.openWindowList.Count)) {
				// Start a new session since restoring configuration failed
				MainWindowForm mainWindowForm = new MainWindowForm();
				AppContext.openWindowList.Add(mainWindowForm);
				this.MainForm = mainWindowForm;

				// TODO: cmdBtnApp_OpenNewShellBrowserPanel_ExecuteEvent
			}

			if (false == noMainWindow)
				(AppContext.openWindowList[0] as Form).Show();

			if(noMainWindow == false) {
				// TODO: Wenn alle minimiert waren werden sie es auch diesmal sein!
				Form form = openWindowList[0] as Form;

				// TODO: Das aktive dokument wird das mainform...
				form.Show();
				this.MainForm = form;
			}

			// Add ThreadExit-handler to save configuration and dispose NotifyIcon when closing
			ThreadExit += new EventHandler(this.AppContext_ThreadExit);

			// Finally close splash screen
			splashScreenForm.Close();
			splashScreenForm.Dispose();
		}

		private void AppContext_ThreadExit(object sender, EventArgs e) {
			// Save configuration file
			if (AppContext.IsIncognito == false)
				this.SaveConfiguration();

			// Destroy NotifyIcon
			AppContext.NotifyIcon.Visible = false;
			AppContext.NotifyIcon.Dispose();

			Debug.WriteLine("electrifier.Core.AppContext: AppContext is shutting down. (" + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + ")\n");
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
	}
}
