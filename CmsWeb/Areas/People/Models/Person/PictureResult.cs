using System;
using System.Collections.Generic;
using System.Xml;
using System.Web.Mvc;
using System.Xml.Linq;
using UtilityExtensions;
using System.Linq;
using CmsData;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace CmsWeb.Areas.People.Models.Person
{
    public class PictureResult : ActionResult
    {
        int Id;
        string Size;
        public PictureResult(string size, int id)
        {
            Id = id;
            Size = size;
        }
        private class FamilyImages
        {
            public class MemberData
            {
                public int PeopleId;
                public int? SmallId;
                public int? ThumbId;
                public ImageData.Image tiny;
            }
            public List<MemberData> members;
            public ImageData.Image current;
        }
        public override void ExecuteResult(ControllerContext context)
        {
//            var fam = context.HttpContext.Items["familyimages"] as FamilyImages;
//            context.HttpContext.Response.Clear();
//		    var q = from p in DbUtil.Db.People
//		            where p.PeopleId == Id
//                    from m in p.Family.People
//		            select new FamilyImages.MemberData()
//		                       {
//                                   PeopleId = m.PeopleId,
//		                           SmallId = m.Picture.SmallId,
//		                           ThumbId = m.Picture.ThumbId,
//		                       };
//            fam = new FamilyImages()
//                          {
//                              members = q.ToList()
//                          };
//            var idlist = new Dictionary<int, ImageData.Image>();
//            foreach (var m in fam.members)
//            {
//                if (m.ThumbId.HasValue)
//                idlist.Add(m.ThumbId.Value, null);
//                if (m.PeopleId == Id && m.SmallId.HasValue)
//                    idlist.Add(m.SmallId.Value, null);
//            }
//            var i = from im in ImageData.DbUtil.Db.Images
//                    where idlist.Keys.Contains(im.Id)
//                    select im;
//            switch (Size)
//            {
//                case "small":
//                    break;
//                case "tiny":
//                    break;
//                case "medium":
//                    break;
//            }
//            var image = ImageData.DbUtil.Db.Images.SingleOrDefault(i => i.Id == Id);
//            if (image == null)
//                NoPic(context.HttpContext);
//            else
//            {
//                context.HttpContext.Response.ContentType = image.Mimetype;
//                context.HttpContext.Response.BinaryWrite(image.Bits);
//            }
//        }
//        void NoPic(System.Web.HttpContextBase context)
//        {
//            var portrait = context.Request.QueryString["portrait"].ToInt2();
//            if (portrait == 1)
//            {
//                context.Response.ContentType = "image/jpeg";
//                var u = context.Cache["unknownimage"] as Byte[];
//                if (u == null)
//                {
//                    u = File.ReadAllBytes(context.Server.MapPath("/images/unknown.jpg"));
//                    context.Cache["unknownimage"] = u;
//                }
//                context.Response.BinaryWrite(u);
//            }
//            else
//            {
//                var bmp = new Bitmap(200, 200, PixelFormat.Format24bppRgb);
//                var g = Graphics.FromImage(bmp);
//                g.Clear(Color.Bisque);
//                g.DrawString("No Image", new Font("Verdana", 22, FontStyle.Bold), SystemBrushes.WindowText, new PointF(2, 2));
//                context.Response.ContentType = "image/gif";
//                bmp.Save(context.Response.OutputStream, ImageFormat.Gif);
//            }
        }
    }
}