/* Author: David Carroll
 * Copyright (c) 2008, 2009 Bellevue Baptist Church 
 * Licensed under the GNU General Public License (GPL v2)
 * you may not use this code except in compliance with the License.
 * You may obtain a copy of the License at http://bvcms.codeplex.com/license 
 */
using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using CmsData;
using UtilityExtensions;
using System.Drawing;
using System.Drawing.Imaging;

namespace CMSWeb
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class Image : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
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
                context.Response.WriteFile(context.Server.MapPath("~/images/unknown.jpg"));
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

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
