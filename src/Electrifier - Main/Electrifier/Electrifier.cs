//	<file>
//		<copyright see="www.electrifier.org"/>
//		<license   see="www.electrifier.org"/>
//		<owner     name="Thorsten Jung" email="taj@electrifier.org"/>
//		<version   value="$Id: BasicShellObject.cs,v 1.13 2004/09/10 20:30:33 taj bender Exp $"/>
//	</file>

using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

using Electrifier.Core;

namespace Electrifier {
	/// <summary>
	/// The class <c>ElectrifierMainEntryPoint</c> acts, as suggested, as the main entry point.
	/// The given arguments are evaluated, the core services started and the main configuration
	/// is read out; also, the splash is shown here.
	/// </summary>
	class ElectrifierMainEntryPoint {
		/// <summary>
		/// The Main method is the entry point of the electrifier application.
		/// </summary>
		[STAThread]
		static int Main(string[] args) {
			SplashScreenForm splashScreen     = null;
			bool             splashIsShown    = true;
			bool             splashIsFadedOut = true;
			Icon             applicationIcon  = null;

			// TODO: Check if first instance
			// Enable application to use Windows XP Visual Styles
			Application.EnableVisualStyles();
			Application.DoEvents();

			// Search argument list for splasscreen-related arguments
			foreach(string arg in args) {
				if(arg.ToLower().Equals("/nosplash")) {
					splashIsShown = false;
				}

				if(arg.ToLower().Equals("/nosplashfadeout")) {
					splashIsFadedOut = false;
				}
			}

			// Create the splash-screen
			splashScreen = new SplashScreenForm(splashIsShown, splashIsFadedOut);

			// Get the application icon
			applicationIcon = new Icon(Assembly.GetEntryAssembly().
				GetManifestResourceStream("Electrifier.Electrifier.ico"));

			// Create an electrifier application context form and run as application
			// TODO: Do dynamic binding...
			AppContext appContext = new AppContext(args, applicationIcon, splashScreen.SplashScreenBitmap, splashScreen);
			Application.Run(appContext);

			// Free used resources
			applicationIcon.Dispose();
			splashScreen.Dispose();

			return 0;		// TODO: Return returncode
		}
	}
}
