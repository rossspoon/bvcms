using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
// Used for HTML Image Capture
using System.Threading;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace HelperService.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return Content("Success!");
        }

        [ValidateInput(false)]
        public ActionResult CreateWebsiteThumbnail(string sHTML)
        {
            return new FileContentResult(CaptureWebPageBytes(sHTML, 100, 150), "image/jpeg");
        }

        public static byte[] CaptureWebPageBytes(string body, int width, int height)
        {
            bool bDone = false;
            byte[] data = null;
            DateTime startDate = DateTime.Now;
            DateTime endDate = DateTime.Now;

            //sta thread to allow intiate WebBrowser
            var staThread = new Thread(delegate()
            {
                data = CaptureWebPageBytesP(body, width, height);
                bDone = true;
            });

            staThread.SetApartmentState(ApartmentState.STA);
            staThread.Start();

            while (!bDone)
            {
                endDate = DateTime.Now;
                TimeSpan tsp = endDate.Subtract(startDate);

                System.Windows.Forms.Application.DoEvents();
                if (tsp.Seconds > 50)
                {
                    break;
                }
            }
            staThread.Abort();
            return data;
        }

        static byte[] CaptureWebPageBytesP(string body, int width, int height)
        {
            byte[] data;

            using (WebBrowser web = new WebBrowser())
            {
                web.ScrollBarsEnabled = false; // no scrollbars
                web.ScriptErrorsSuppressed = true; // no errors

                web.DocumentText = body;
                while (web.ReadyState != System.Windows.Forms.WebBrowserReadyState.Complete)
                    System.Windows.Forms.Application.DoEvents();

                web.Width = web.Document.Body.ScrollRectangle.Width;
                web.Height = web.Document.Body.ScrollRectangle.Height;

                // a bitmap that we will draw to
                using (System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(web.Width, web.Height))
                {
                    // draw the web browser to the bitmap
                    web.DrawToBitmap(bmp, new Rectangle(web.Location.X, web.Location.Y, web.Width, web.Height));
                    // draw the web browser to the bitmap

                    GraphicsUnit units = GraphicsUnit.Pixel;
                    RectangleF destRect = new RectangleF(0F, 0F, width, height);
                    RectangleF srcRect = new RectangleF(0, 0, web.Width, web.Width * 1.5F);

                    Bitmap b = new Bitmap(width, height);
                    Graphics g = Graphics.FromImage((Image)b);
                    g.Clear(Color.White);
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                    g.DrawImage(bmp, destRect, srcRect, units);
                    g.Dispose();

                    using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
                    {
                        EncoderParameter qualityParam = null;
                        EncoderParameters encoderParams = null;
                        try
                        {
                            ImageCodecInfo imageCodec = null;
                            imageCodec = GetEncoderInfo("image/jpeg");

                            qualityParam = new EncoderParameter(Encoder.Quality, 100L);

                            encoderParams = new EncoderParameters(1);
                            encoderParams.Param[0] = qualityParam;
                            b.Save(stream, imageCodec, encoderParams);
                        }
                        catch (Exception)
                        {
                            throw new Exception();
                        }
                        finally
                        {
                            if (encoderParams != null) encoderParams.Dispose();
                            if (qualityParam != null) qualityParam.Dispose();
                        }
                        b.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                        stream.Position = 0;
                        data = new byte[stream.Length];
                        stream.Read(data, 0, (int)stream.Length);
                    }
                }
            }
            return data;
        }

        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }
    }
}
