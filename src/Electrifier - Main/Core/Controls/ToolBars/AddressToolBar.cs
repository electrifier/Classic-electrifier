using System;

namespace electrifier.Core.Controls.ToolBars
{
	/// <summary>
	/// Zusammenfassung für AddressToolBar.
	/// </summary>
	public class AddressToolBar
	{
/* TODO: RELAUNCH: Commented out
		private ComboBoxItem comboBox = null;
*/
		public string Address {
            set { }
/* TODO: RELAUNCH: Commented out
			set { this.comboBox.ControlText = value; }
 */
		}

		public AddressToolBar()
		{
/* TODO: RELAUNCH: Commented out
			this.comboBox = new ComboBoxItem();
			this.comboBox.Text = @"Address";
			this.Items.AddRange(new TD.SandBar.ToolbarItemBase[] {
																														 this.comboBox
																													 });


			this.Stretch = true;
			this.StretchItem = this.comboBox;
 */
		}
	}
}
