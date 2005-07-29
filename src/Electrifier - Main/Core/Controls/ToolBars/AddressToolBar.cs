using System;

using TD.SandBar;

namespace Electrifier.Core.Controls.ToolBars
{
	/// <summary>
	/// Zusammenfassung für AddressToolBar.
	/// </summary>
	public class AddressToolBar : TD.SandBar.ToolBar
	{
		private ComboBoxItem comboBox = null;

		public string Address {
			set { this.comboBox.ControlText = value; }
		}

		public AddressToolBar()
		{
			this.comboBox = new ComboBoxItem();
			this.comboBox.Text = @"Address";
			this.Items.AddRange(new TD.SandBar.ToolbarItemBase[] {
																														 this.comboBox
																													 });


			this.Stretch = true;
			this.StretchItem = this.comboBox;
		}
	}
}
