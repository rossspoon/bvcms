using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ServiceModel;
using System.Configuration;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using System.IO;
using System.Collections.Specialized;
using System.Net;
using System.Drawing.Drawing2D;

namespace TakeUploadPicture
{
    public partial class StandAloneForm : Form
    {
        private Capture cam;
        IntPtr m_ip = IntPtr.Zero;
        const int VIDEODEVICE = 0; // zero based index of video capture device to use
        const int VIDEOWIDTH = 640; // Depends on video device caps
        const int VIDEOHEIGHT = 480; // Depends on video device caps
        const int VIDEOBITSPERPIXEL = 64; // BitsPerPixel values determined by device

       public StandAloneForm()
        {
            InitializeComponent();

            try
            {
                cam = new Capture(VIDEODEVICE, VIDEOWIDTH, VIDEOHEIGHT, VIDEOBITSPERPIXEL, imageResizer2.pctCamera);
                imageResizer2.btnTakePicture.Click += new EventHandler(btnTakePicture_Click);
                imageResizer2.btnSave.Click += new EventHandler(btnUploadPicture_Click);
            }
            catch (Exception)
            {
                //Handle exception
            }
        }
        private void btnTakePicture_Click(object sender, EventArgs e)
        {
            const string STR_ShowCamera = "Show Camera";
            if (imageResizer2.btnTakePicture.Text == STR_ShowCamera)
            {
                imageResizer2.pctCamera.Visible = true;
                imageResizer2.btnTakePicture.Text = "Take Picture";
                return;
            }
            if (cam != null)
            {
                Cursor.Current = Cursors.WaitCursor;

                if (m_ip != IntPtr.Zero)
                {
                    Marshal.FreeCoTaskMem(m_ip);
                    m_ip = IntPtr.Zero;
                }

                m_ip = cam.Click();
                Bitmap b = new Bitmap(cam.Width, cam.Height, cam.Stride, PixelFormat.Format24bppRgb, m_ip);
                b.RotateFlip(RotateFlipType.RotateNoneFlipY);

                imageResizer2.BaseImage = b;
                imageResizer2.pctCamera.Visible = false;
                imageResizer2.btnTakePicture.Text = STR_ShowCamera;

                Cursor.Current = Cursors.Default;
            }

        }
        private static byte[] ConvertImageToByteArray(Image image, ImageFormat format)
        {
            byte[] b;
            try
            {

                using (var ms = new MemoryStream())
                {
                    image.Save(ms, format);
                    b = ms.ToArray();
                }
            }
            catch (Exception) { throw; }
            return b;
        }

        private void btnUploadPicture_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            var bits = ConvertImageToByteArray(imageResizer2.SaveImage(), ImageFormat.Jpeg);

            var formdata = new NameValueCollection();
            formdata.Add("PeopleId", Program.PeopleId.ToString());
            formdata.Add("Guid", Program.Guid.ToString());
            //MessageBox.Show(Program.Host);
            var url = new Uri(new Uri(Program.Host), "Checkin/UploadImage/");
            var wc = new WebClient();
            wc.Headers.Add(formdata);
            wc.UploadData(url, "POST", bits);

            cam.Dispose();
            Application.Exit();
        }

        private void StandAloneForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            cam.Dispose();
            Application.Exit();
        }


    }
}
