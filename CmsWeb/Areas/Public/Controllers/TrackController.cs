using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CmsWeb.Areas.Public.Models;
using UtilityExtensions;
using CmsData;

namespace CmsWeb.Areas.Public.Controllers
{
	public class TrackController : Controller
	{
		public ActionResult Index(string id)
		{
			var g = id.ToGuid();
			var et = DbUtil.Db.EmailQueueTos.SingleOrDefault(e => e.Guid == g);
			if (et != null)
			{
				var r = new EmailResponse
				{
					EmailQueueId = et.Id,
					PeopleId = et.PeopleId,
					Dt = DateTime.Now,
					Type = "o",
				};
				DbUtil.Db.EmailResponses.InsertOnSubmit(r);
				DbUtil.Db.SubmitChanges();
			}
			// this is an invisible 1 pixel image
			var b = Convert.FromBase64String("R0lGODlhAQABAIAAANvf7wAAACH5BAEAAAAALAAAAAABAAEAAAICRAEAOw==");
			return new FileContentResult(b, "image/gif");
		}
		public ActionResult Key(string id)
		{
			var g = id.QuerystringToGuid();
			var et = DbUtil.Db.EmailQueueTos.SingleOrDefault(e => e.Guid == g);
			if (et != null)
			{
				var r = new EmailResponse
				{
					EmailQueueId = et.Id,
					PeopleId = et.PeopleId,
					Dt = DateTime.Now,
					Type = "o",
				};
				DbUtil.Db.EmailResponses.InsertOnSubmit(r);
				DbUtil.Db.SubmitChanges();
			}
			// this is an invisible 1 pixel image
			var b = Convert.FromBase64String("R0lGODlhAQABAIAAANvf7wAAACH5BAEAAAAALAAAAAABAAEAAAICRAEAOw==");
			return new FileContentResult(b, "image/gif");
		}
		public ActionResult Barcode(string id)
		{
            var code = new Code39(id);
			var b = code.Paint();
            var ostream = new MemoryStream();
            b.Save(ostream, ImageFormat.Png);
            var bits = ostream.GetBuffer();
            b.Dispose();
            ostream.Close();
            return new FileContentResult(bits, "image/png");
		}
	}
}
