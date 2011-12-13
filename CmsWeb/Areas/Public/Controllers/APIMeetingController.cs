using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using CmsData;
using UtilityExtensions;
using CmsWeb.Models;
using System.Xml;
using System.IO;
using System.Net.Mail;
using CmsData.Codes;
using CmsData.API;
using System.Text;
using System.Net;
using CmsWeb.Areas.Manage.Controllers;

namespace CmsWeb.Areas.Public.Controllers
{
#if DEBUG
#else
    [RequireHttps]
#endif
    public class APIMeetingController : CmsController
    {
        [HttpGet]
        public ActionResult ExtraValues(int id, string fields)
        {
            var ret = AuthenticateDeveloper();
            if (ret.StartsWith("!"))
                return Content("<ExtraValues error=\"{0}\" />".Fmt(ret.Substring(1)));
            return Content(new APIMeeting(DbUtil.Db)
                .ExtraValues(id, fields), "text/xml");
        }
        [HttpPost]
        public ActionResult AddEditExtraValue(int meetingid, string field, string value)
        {
            var ret = AuthenticateDeveloper();
            if (ret.StartsWith("!"))
                return Content(ret.Substring(1));
            return Content(new APIMeeting(DbUtil.Db)
                .AddEditExtraValue(meetingid, field, value));
        }
        [HttpPost]
        public ActionResult DeleteExtraValue(int meetingid, string field)
        {
            var ret = AuthenticateDeveloper();
            if (ret.StartsWith("!"))
                return Content(ret.Substring(1));
            return Content(new APIMeeting(DbUtil.Db)
                .DeleteExtraValue(meetingid, field));
        }
        [HttpPost]
        public ActionResult MarkRegistered(int meetingid, int peopleid, bool registered)
        {
            var ret = AuthenticateDeveloper();
            if (ret.StartsWith("!"))
                return Content(ret.Substring(1));
            Attend.MarkRegistered(peopleid, meetingid, registered);
            return Content("ok");
        }
    }
}