using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Electrifier.Core {
	/// <summary>
	/// Zusammenfassung f�r ElectrifierAppContext.
	/// </summary>
	public class ElectrifierAppContext : System.Windows.Forms.Form {
		/// <summary>
		/// Erforderliche Designervariable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ElectrifierAppContext(string[] args, Icon applicationIcon, Bitmap applicationLogo, Form splashScreenForm) {
			// Enable application to use WindowsXP Visual Styles and Objekt Linking and Embedding
			Application.EnableVisualStyles();
			Application.OleRequired();

			//
			// Erforderlich f�r die Windows Form-Designerunterst�tzung
			//
			InitializeComponent();

		}

		/// <summary>
		/// Die verwendeten Ressourcen bereinigen.
		/// </summary>
		protected override void Dispose( bool disposing ) {
			if( disposing ) {
				if(components != null) {
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Vom Windows Form-Designer generierter Code
		/// <summary>
		/// Erforderliche Methode f�r die Designerunterst�tzung. 
		/// Der Inhalt der Methode darf nicht mit dem Code-Editor ge�ndert werden.
		/// </summary>
		private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			this.Size = new System.Drawing.Size(300,300);
			this.Text = "ElectrifierAppContext";
		}
		#endregion
	}
}
