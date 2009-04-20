using System;
using System.IO;
using System.Collections;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace GdiPlusLib
{
    public class Gdip
    {
        private static ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();

        private static bool GetCodecClsid(string filename, out Guid clsid)
        {
            clsid = Guid.Empty;
            string ext = Path.GetExtension(filename);
            if (ext == null)
                return false;
            ext = "*" + ext.ToUpper();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FilenameExtension.IndexOf(ext) >= 0)
                {
                    clsid = codec.Clsid;
                    return true;
                }
            }
            return false;
        }


        public static bool SaveDIBAs(string picname, IntPtr bminfo, IntPtr pixdat)
        {
            var sd = new SaveFileDialog();

            sd.FileName = picname;
            sd.Title = "Save bitmap as...";
            sd.Filter = "Bitmap file (*.bmp)|*.bmp|TIFF file (*.tif)|*.tif|JPEG file (*.jpg)|*.jpg|PNG file (*.png)|*.png|GIF file (*.gif)|*.gif|All files (*.*)|*.*";
            sd.FilterIndex = 1;
            if (sd.ShowDialog() != DialogResult.OK)
                return false;

            Guid clsid;
            if (!GetCodecClsid(sd.FileName, out clsid))
            {
                MessageBox.Show("Unknown picture format for extension " + Path.GetExtension(sd.FileName),
                                "Image Codec", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            var img = IntPtr.Zero;
            int st = GdipCreateBitmapFromGdiDib(bminfo, pixdat, ref img);
            if ((st != 0) || (img == IntPtr.Zero))
                return false;

            st = GdipSaveImageToFile(img, sd.FileName, ref clsid, IntPtr.Zero);
            GdipDisposeImage(img);
            return st == 0;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        internal class BITMAPINFOHEADER
        {
            public int biSize;
            public int biWidth;
            public int biHeight;
            public short biPlanes;
            public short biBitCount;
            public int biCompression;
            public int biSizeImage;
            public int biXPelsPerMeter;
            public int biYPelsPerMeter;
            public int biClrUsed;
            public int biClrImportant;
        }
        [DllImport("gdi32.dll", ExactSpelling = true)]
        internal static extern int SetDIBitsToDevice(IntPtr hdc, int xdst, int ydst,
        int width, int height, int xsrc, int ysrc, int start, int lines,
        IntPtr bitsptr, IntPtr bmiptr, int color);

        [DllImport("kernel32.dll", ExactSpelling = true)]
        internal static extern IntPtr GlobalLock(IntPtr handle);
        [DllImport("kernel32.dll", ExactSpelling = true)]
        internal static extern IntPtr GlobalFree(IntPtr handle);

        public static Bitmap GetBitmap(IntPtr ScannedImage)
        {
            BITMAPINFOHEADER bmi;
            Rectangle bmprect;
            IntPtr dibhand;
            IntPtr bmpptr;
            IntPtr pixptr;
            bmprect = new Rectangle(0, 0, 0, 0);
            dibhand = ScannedImage;
            bmpptr = GlobalLock(dibhand);
            //GetPixelInfo
            bmi = new BITMAPINFOHEADER();
            Marshal.PtrToStructure(bmpptr, bmi);

            bmprect.X = bmprect.Y = 0;
            bmprect.Width = bmi.biWidth;
            bmprect.Height = bmi.biHeight;

            if (bmi.biSizeImage == 0)
                bmi.biSizeImage = ((((bmi.biWidth * bmi.biBitCount) + 31) & ~31) >> 3) * bmi.biHeight;

            int p = bmi.biClrUsed;
            if ((p == 0) && (bmi.biBitCount <= 8))
                p = 1 << bmi.biBitCount;
            p = (p * 4) + bmi.biSize + (int)bmpptr;

            pixptr = (IntPtr)p;//GetPixelInfo(bmpptr);
            Bitmap Output = null;
            switch (bmi.biBitCount)
            {
                default:
                case 24:
                    Output = new Bitmap(bmprect.Size.Width, bmprect.Size.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                    break;
                case 32:
                    Output = new Bitmap(bmprect.Size.Width, bmprect.Size.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                    break;
            }
            if (Output == null) return Output;
            //Point scrol = AutoScrollPosition;
            Graphics grap = Graphics.FromImage((Image)Output);
            SolidBrush brbg = new SolidBrush(Color.Black);
            grap.FillRectangle(brbg, new Rectangle(0, 0, bmi.biWidth, bmi.biHeight));
            IntPtr hdc = grap.GetHdc();
            SetDIBitsToDevice(hdc, 0, 0, bmi.biWidth, bmi.biHeight, 0, 0, 0, bmi.biHeight, pixptr, bmpptr, 0);
            grap.ReleaseHdc(hdc);
            grap.Dispose();
            grap = null;
            GlobalFree(dibhand);
            dibhand = IntPtr.Zero;

            GC.Collect();
            return Output;
        }


        [DllImport("gdiplus.dll", ExactSpelling = true)]
        internal static extern int GdipCreateBitmapFromGdiDib(IntPtr bminfo, IntPtr pixdat, ref IntPtr image);

        [DllImport("gdiplus.dll", ExactSpelling = true, CharSet = CharSet.Unicode)]
        internal static extern int GdipSaveImageToFile(IntPtr image, string filename, [In] ref Guid clsid, IntPtr encparams);

        [DllImport("gdiplus.dll", ExactSpelling = true)]
        internal static extern int GdipDisposeImage(IntPtr image);

    }

} // namespace GdiPlusLib
