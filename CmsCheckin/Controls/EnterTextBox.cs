using System.Windows.Forms;

namespace CmsCheckin.Controls
{
	public class EnterTextBox : TextBox
	{
		protected override bool IsInputKey(System.Windows.Forms.Keys keyData)
		{
			if (keyData == Keys.Enter) return true;
			return base.IsInputKey(keyData);
		}
	}
}
