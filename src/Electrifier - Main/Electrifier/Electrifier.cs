using System;
using System.Windows.Forms;

using Electrifier.Core;
using Electrifier.Core.Forms;
using Electrifier.Core.Services;
using Electrifier.Core.Shell32;
using Electrifier.Core.Shell32.Services;
using Electrifier.Win32API;

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
		static void Main(string[] args) {
			SplashScreenForm splashScreen  = null;
			bool             splashIsShown = true;

			foreach(string arg in args) {
				if(arg.ToLower().Equals("/nosplash")) {
					splashIsShown = false;
					break;
				}
			}

			// Create the splash-screen
			splashScreen = new SplashScreenForm(splashIsShown);
			splashScreen.Show();

			// Create the main virtual form
			// TODO: Do dynamic binding...
			ElectrifierApplicationContext appContext = new ElectrifierApplicationContext(args);

			Application.Run(electrifierMainVirtualForm);
		}
	}
}
