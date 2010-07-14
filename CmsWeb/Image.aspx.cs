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
                context.Response.ContentType = image.Mimetype;
                context.Response.BinaryWrite(image.Bits);
            }
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
