using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.IO;
using System.Drawing.Imaging;

namespace CmsCheckin
{
    public partial class Picture : Form
    {
        public Picture()
        {
            InitializeComponent();

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            var f = new TakePicture();
            f.ShowDialog();
            this.Close();
        }

        private void TakePic_Click(object sender, EventArgs e)
        {
            var f = new TakePicture();
            f.ShowDialog();
            this.Close();
        }

        private void Return_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Picture_Load(object sender, EventArgs e)
        {
            var wc = Util.CreateWebClient();
            var url = new Uri(new Uri(Util.ServiceUrl()), "Checkin2/FetchImage/" + Program.PeopleId);
            var bits = wc.DownloadData(url);
            var istream = new MemoryStream(bits);
            pictureBox1.Image = Image.FromStream(istream);
            istream.Close();
        }
    }
}
