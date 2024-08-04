using System;
using System.Drawing;
using System.Windows.Forms;

namespace electrifier.Core.Components.Controls
{
    public class ToolStripSpringComboBox : ToolStripComboBox
    {
        /// <summary>
        /// <see cref="ToolStripSpringComboBox"/> represents a ToolStripComboBox with ability to extend to full size in its containing toolbar.
        /// 
        /// <seealso href="https://docs.microsoft.com/de-de/dotnet/framework/winforms/controls/stretch-a-toolstriptextbox-to-fill-the-remaining-width-of-a-toolstrip-wf"/>
        /// </summary>
        public ToolStripSpringComboBox() : base()
        {
        }

        public override Size GetPreferredSize(Size constrainingSize)
        {
            // Use the default size if the text box is on the overflow menu or is on a vertical ToolStrip.
            if (this.IsOnOverflow || (this.Owner.Orientation == Orientation.Vertical))
            {
                return this.DefaultSize;
            }

            // Declare a variable to store the total available width as it is calculated, starting with the display width of the owning ToolStrip.
            var width = this.Owner.DisplayRectangle.Width;

            // Subtract the width of the overflow button if it is displayed. 
            if (this.Owner.OverflowButton.Visible)
            {
                width = width - this.Owner.OverflowButton.Width - this.Owner.OverflowButton.Margin.Horizontal;
            }

            // Declare a variable to maintain a count of ToolStripSpringTextBox items currently displayed in the owner ToolStrip
            var springBoxCount = 0;

            foreach (ToolStripItem item in this.Owner.Items)
            {
                // Ignore items on the overflow menu.
                if (item.IsOnOverflow)
                    continue;

                if (item is ToolStripSpringComboBox)
                {
                    // For ToolStripSpringTextBox items, increment the count and subtract the margin width from the total available width.
                    springBoxCount++;
                    width -= item.Margin.Horizontal;
                }
                else
                {
                    // For all other items, subtract the full width from the total available width.
                    width = width - item.Width - item.Margin.Horizontal;
                }
            }

            // If there are multiple ToolStripSpringTextBox items in the owning ToolStrip, divide the total available width between them. 
            if (springBoxCount > 1)
                width /= springBoxCount;

            // If the available width is less than the default width, use the default width, forcing one or more items onto the overflow menu.
            if (width < this.DefaultSize.Width)
                width = this.DefaultSize.Width;

            // Retrieve the preferred size from the base class, but change the width to the calculated width. 
            Size size = base.GetPreferredSize(constrainingSize);
            size.Width = width;

            return size;
        }
    }
}
