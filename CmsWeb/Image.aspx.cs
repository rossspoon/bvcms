using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UtilityExtensions;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace CmsWeb
{
    public partial class Image1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var context = HttpContext.Current;
            context.Response.Clear();
            var id = context.Request.QueryString["id"].ToInt2();
            var image = ImageData.DbUtil.Db.Images.SingleOrDefault(i => i.Id == id);
            if (image == null)
                NoPic(context);
            else
            {
                var w = Page.QueryString<int?>("w");
                var h = Page.QueryString<int?>("h");
                if (w.HasValue && h.HasValue)
                {
                    context.Response.ContentType = "image/jpeg";
                    context.Response.BinaryWrite(FetchResizedImage(image, w.Value, h.Value));
                }
                else
                {
                    context.Response.ContentType = image.Mimetype;
                    context.Response.BinaryWrite(image.Bits);
                }
            }
        }
        public byte[] FetchResizedImage(ImageData.Image img, int w, int h)
        {
            var istream = new MemoryStream(img.Bits);
            var img1 = System.Drawing.Image.FromStream(istream);
            var ratio = Math.Min(w / (double)img1.Width, h / (double)img1.Height);
            if (ratio >= 1) // image is smaller than requested
                ratio = 1; // same size
            w = Convert.ToInt32(ratio * img1.Width);
            h = Convert.ToInt32(ratio * img1.Height);
            var img2 = new System.Drawing.Bitmap(img1, w, h);
            var ostream = new MemoryStream();
            img2.Save(ostream, ImageFormat.Jpeg);
            var Bits = ostream.GetBuffer();
            var Length = Bits.Length;
            img1.Dispose();
            img2.Dispose();
            istream.Close();
            ostream.Close();
            return Bits;
        }
        void NoPic(HttpContext context)
        {
            var portrait = context.Request.QueryString["portrait"].ToInt2();
            if (portrait == 1)
            {
                context.Response.ContentType = "image/jpeg";
                var u = HttpContext.Current.Cache["unknownimage"] as Byte[];
                if (u == null)
                {
                    u = File.ReadAllBytes(context.Server.MapPath("/images/unknown.jpg"));
                    HttpContext.Current.Cache["unknownimage"] = u;
                }
                context.Response.BinaryWrite(u);
            }
            else
            {
                var bmp = new Bitmap(200, 200, PixelFormat.Format24bppRgb);
                var g = Graphics.FromImage(bmp);
                g.Clear(Color.Bisque);
                g.DrawString("No Image", new Font("Verdana", 22, FontStyle.Bold), SystemBrushes.WindowText, new PointF(2, 2));
                context.Response.ContentType = "image/gif";
                bmp.Save(context.Response.OutputStream, ImageFormat.Gif);
            }
        }
    }
}
