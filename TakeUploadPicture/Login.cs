using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TakeUploadPicture
{
    public partial class Signin : Form
    {
        public string Username;
        public string Password;
        public Signin()
        {
            InitializeComponent();
        }

        private void submit_Click(object sender, EventArgs e)
        {
            Username = username.Text;
            Password = password.Text;
            this.Close();
        }
    }
}
