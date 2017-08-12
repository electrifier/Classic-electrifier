using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace electrifier {
	/// <summary>
	/// The class <c>ElectrifierMainEntryPoint</c> acts, as suggested, as the main entry point.
	/// The given arguments are evaluated, the core services started and the main configuration
	/// is read out; also, the splash is shown here.
	/// </summary>
	class ElectrifierMainEntryPoint {

		/// <summary>
		/// System Error Codes (0-499)
		/// 
		/// See https://msdn.microsoft.com/de-de/library/ms681382(v=vs.85)
		/// 
		/// ERROR_INVALID_FUNCTION = 1 (0x1)
		///		Incorrect function.
		///
		/// ERROR_FILE_NOT_FOUND = 2 (0x2)
		///		The system cannot find the file specified.
		/// </summary>
		public enum SystemErrorCodes {
			ERROR_INVALID_FUNCTION = 1,
			ERROR_FILE_NOT_FOUND = 2,
		}

		/// <summary>
		/// The Main method is the entry point of the electrifier application.
		/// </summary>
		[STAThread]
		static void Main(string[] args) {
			bool splashIsShown = true;
			bool splashIsFadedOut = true;
			Icon applicationIcon = null;
			SplashScreenForm splashScreen = null;
			ApplicationContext appContext = null;

            // Never show the splash-screen while debugging
            if (Debugger.IsAttached)
                splashIsShown = false;

            // TODO: Check if first instance
            // Enable application to use Visual Styles
            Application.EnableVisualStyles();
			Application.DoEvents();

			string electrifierCoreDLLFullPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) +
				Path.DirectorySeparatorChar + @"electrifier.core.dll";

			// Search argument list for splash-screen-related arguments
			foreach (string arg in args) {
				if (arg.ToLower().Equals("/nosplash")) {
					splashIsShown = false;
				}

				if (arg.ToLower().Equals("/nosplashfadeout")) {
					splashIsFadedOut = false;
				}
			}

			// Create an electrifier application context by loading electrifier.core.dll
			try {
				// Get the application icon
				applicationIcon = new Icon(Assembly.GetEntryAssembly().
					GetManifestResourceStream("electrifier.electrifier.ico"));

				// Create the splash-screen
				splashScreen = new SplashScreenForm(splashIsShown, splashIsFadedOut);

				// Create an instance of the application context
				appContext = Activator.CreateInstance(
					Assembly.LoadFile(electrifierCoreDLLFullPath).GetType(@"electrifier.Core.AppContext"),
					new Object[] { args, applicationIcon, splashScreen.SplashScreenBitmap, splashScreen }) as ApplicationContext;

				// Run electrifier applicaton context
				try {
					Application.Run(appContext);
				} catch (Exception ex) {
					MessageBox.Show("Error while running electrifier.\n\n" + ex.Message,
						"electrifier: Runtime Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

					Environment.ExitCode = (int)SystemErrorCodes.ERROR_INVALID_FUNCTION;
				}
			} catch (Exception ex) {
				MessageBox.Show("Unable to load '" + electrifierCoreDLLFullPath + "'.\n\n" +
					"Please reinstall electrifier application.\n\n" + ex.Message,
					"electrifier: Critical Startup Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

				Environment.ExitCode = (int)SystemErrorCodes.ERROR_FILE_NOT_FOUND;
			}

			// Free used resources
			if (null != splashScreen)
				splashScreen.Dispose();
			if (null != applicationIcon)
				applicationIcon.Dispose();
		}
	}
}
