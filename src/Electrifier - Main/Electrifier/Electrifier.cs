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
			SplashScreenForm splashScreen   = null;
			bool             splashIsShown  = true;
			TrayNotifyIcon   trayNotifyIcon = null;

			foreach(string arg in args) {
				if(arg.Equals("/nosplash")) {
					splashIsShown = false;
					break;
				}
			}

			// Create the splash-screen
			splashScreen = new SplashScreenForm(splashIsShown);
			splashScreen.Show();

			// Create the tray-notify icon
			trayNotifyIcon = new TrayNotifyIcon();




			ServiceManager.Services.AddService(new DesktopFolderInstance());
			ServiceManager.Services.AddService(new PIDLManager());
			ServiceManager.Services.AddService(new IconManager());

			ElectrifierForm electrifierForm = new ElectrifierForm();


			

//			ShellTreeView shtrv = new ShellTreeView(ShellAPI.CSIDL.DESKTOP);
//			shtrv.Name = "ShellTreeView";
//			shtrv.Dock = DockStyle.Left;
//			shtrv.Width = 250;
//
//			ShellListView shlsv = new ShellListView();
//			shlsv.Name = "ShellListView";
//			shlsv.Dock = DockStyle.Fill;
//			shlsv.SetBrowsingFolder(null, ShellAPI.CSIDL.DESKTOP);
//
//			Form mainForm = new Form();
//			mainForm.Controls.Add(shtrv);
//			mainForm.Controls.Add(shlsv);
//			mainForm.Width = 800;
//			mainForm.Height = 600;
//			mainForm.Text = "Electrifier SplashScreen preview release";

			trayNotifyIcon.Visible = true;
			Application.Run(electrifierForm);
			trayNotifyIcon.Visible = false;
		}
	}
}
