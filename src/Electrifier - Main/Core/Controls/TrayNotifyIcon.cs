using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace Electrifier {
	/// <summary>
	/// Zusammenfassung für TrayNotifyIcon.
	/// </summary>
	public class TrayNotifyIcon {
		protected NotifyIcon notifyIcon      = null;
		protected Icon       electrifierIcon = null;
		public    Icon       ElectrifierIcon {
			get {
				return electrifierIcon;
			}
		}
		public    string     Text {
			get {
				return notifyIcon.Text;
			}
			set {
				notifyIcon.Text = value;
			}
		}
		public    bool       Visible {
			get {
				return notifyIcon.Visible;
			}
			set {
				notifyIcon.Visible = value;
			}
		}

		public TrayNotifyIcon() {
			notifyIcon      = new NotifyIcon();
			notifyIcon.Icon = electrifierIcon = new Icon(
				Assembly.GetEntryAssembly().GetManifestResourceStream("Electrifier.Electrifier.ico"));
		}
	}
}
