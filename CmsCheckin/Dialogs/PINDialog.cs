using System;
using System.Windows.Forms;

namespace CmsCheckin.Dialogs
{
    public partial class PINDialog : Form
    {
        private Button[] bPadButtons;

        public string sPIN = "";

        public PINDialog()
        {
            InitializeComponent();

            bPadButtons = new Button[] { N1, N2, N3, N4, N5, N6, N7, N8, N9, N0, Go, Back };

            foreach (var item in bPadButtons)
            {
                item.Click += PadClick;
            }
        }

        void PadClick(object sender, EventArgs e)
        {
            var b = sender as Button;

            if (b.Name[0] == 'N')
            {
                PIN.Text += b.Name[1];
                ResetFocus();
                return;
            }

            if (b.Name[0] == 'B')
            {
                if( PIN.Text.Length > 0 )
                    PIN.Text = PIN.Text.Substring(0, PIN.Text.Length - 1);

                ResetFocus();
                return;
            }

            if (b.Name[0] == 'G')
            {
                sPIN = PIN.Text;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        void ResetFocus()
        {
            PIN.Focus();
            PIN.Select(PIN.Text.Length, 0);
        }
    }
}
