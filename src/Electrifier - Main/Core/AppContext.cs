//	<file>
//		<copyright see="www.electrifier.org"/>
//		<license   see="www.electrifier.org"/>
//		<owner     name="Thorsten Jung" email="taj@electrifier.org"/>
//		<version   value="$Id: BasicShellObject.cs,v 1.13 2004/09/10 20:30:33 taj bender Exp $"/>
//	</file>

using System;
using System.Drawing;
using System.Windows.Forms;

namespace Electrifier.Core {
	/// <summary>
	/// Zusammenfassung für AppContext.
	/// </summary>
	public sealed class AppContext : ApplicationContext {
		// Static member variables and properties
		private static AppContext instance = null;
		public  static AppContext Instance { get { return instance; } }
		private static Icon       icon     = null;
		public  static Icon       Icon     { get { return icon; } }
		private static Bitmap     logo     = null;
		public  static Bitmap     Logo     { get { return logo; } }

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
			// Initialize the Application Context Instance
			instance = RegisterAppContextInstance(this);
			icon     = appIcon;
			logo     = appLogo;

			// Finally close splash screen
			splashScreenForm.Close();
		}

		private AppContext RegisterAppContextInstance(AppContext appContextInstance) {
			// Check if another instance was already instantiated
			if(instance != null) {
				throw new InvalidOperationException("Electrifier.Core.AppContext.RegisterInstance: " +
					"Tried to register a new instance although there is already one!");
			} else {
				// Everything fine, return the given instance
				return appContextInstance;
			}
		}
	}
}
