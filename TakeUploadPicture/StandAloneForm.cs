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

namespace TakeUploadPicture
{
    public partial class StandAloneForm : Form
    {
        cmsws.WebServiceSoapClient wsvc = new cmsws.WebServiceSoapClient();
        private Capture cam;
        IntPtr m_ip = IntPtr.Zero;
        const int VIDEODEVICE = 0; // zero based index of video capture device to use
        const int VIDEOWIDTH = 640; // Depends on video device caps
        const int VIDEOHEIGHT = 480; // Depends on video device caps
        const int VIDEOBITSPERPIXEL = 64; // BitsPerPixel values determined by device

        public StandAloneForm()
        {
            InitializeComponent();

#if DEBUG
#else
            wsvc.Endpoint.Address = new EndpointAddress(
                ConfigurationSettings.AppSettings["serviceUrl"]);
#endif
            try
            {
                cam = new Capture(VIDEODEVICE, VIDEOWIDTH, VIDEOHEIGHT, VIDEOBITSPERPIXEL, imageResizer1.pctCamera);
                imageResizer1.btnTakePicture.Click += new EventHandler(btnTakePicture_Click);
                imageResizer1.btnSave.Click += new EventHandler(btnUploadPicture_Click);
            }
            catch (Exception)
            {
                //Handle exception
                imageResizer1.Enabled = false;
            }
        }
        private void btnTakePicture_Click(object sender, EventArgs e)
        {
            const string STR_ShowCamera = "Show Camera";
            if (imageResizer1.btnTakePicture.Text == STR_ShowCamera)
            {
                imageResizer1.pctCamera.Visible = true;
                imageResizer1.btnTakePicture.Text = "Take Picture";
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

                imageResizer1.BaseImage = b;
                imageResizer1.pctCamera.Visible = false;
                imageResizer1.btnTakePicture.Text = STR_ShowCamera;

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

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_MINIMIZED = 6;

        private void btnUploadPicture_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            var bits = ConvertImageToByteArray(imageResizer1.SaveImage(), ImageFormat.Jpeg);
            wsvc.UploadImage(Program.header, Program.PeopleId, Program.Guid, 3, "image/jpeg", bits);
            cam.Dispose();
            Application.Exit();
        }

        private void imageResizer1_Resize(object sender, EventArgs e)
        {
            if (cam != null)
            {
                cam.Dispose();
            }
            if (m_ip != IntPtr.Zero)
            {
                Marshal.FreeCoTaskMem(m_ip);
                m_ip = IntPtr.Zero;
            }
            cam = new Capture(VIDEODEVICE, VIDEOWIDTH, VIDEOHEIGHT, VIDEOBITSPERPIXEL, imageResizer1.pctCamera);
        }

        private void StandAloneForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            cam.Dispose();
            Application.Exit();
        }
       
    }
}
