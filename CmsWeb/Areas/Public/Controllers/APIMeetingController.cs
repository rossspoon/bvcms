using System.Web.Mvc;
using CmsData;
using UtilityExtensions;
using CmsData.API;

namespace CmsWeb.Areas.Public.Controllers
{
    public class APIMeetingController : CmsController
    {
        [HttpGet]
        public ActionResult ExtraValues(int id, string fields)
        {
            var ret = AuthenticateDeveloper();
            if (ret.StartsWith("!"))
                return Content("<ExtraValues error=\"{0}\" />".Fmt(ret.Substring(1)));
			DbUtil.LogActivity("APIMeeting ExtraValues {0}, {1}".Fmt(id, fields));
            return Content(new APIMeeting(DbUtil.Db)
                .ExtraValues(id, fields), "text/xml");
        }
        [HttpPost]
        public ActionResult AddEditExtraValue(int meetingid, string field, string value)
        {
            var ret = AuthenticateDeveloper();
            if (ret.StartsWith("!"))
                return Content(ret.Substring(1));
			DbUtil.LogActivity("APIMeeting AddEditExtraValue {0}, {1}".Fmt(meetingid, field));
            return Content(new APIMeeting(DbUtil.Db)
                .AddEditExtraValue(meetingid, field, value));
        }
        [HttpPost]
        public ActionResult DeleteExtraValue(int meetingid, string field)
        {
            var ret = AuthenticateDeveloper();
            if (ret.StartsWith("!"))
                return Content(ret.Substring(1));
			DbUtil.LogActivity("APIMeeting DeleteExtraValue {0}, {1}".Fmt(meetingid, field));
            return Content(new APIMeeting(DbUtil.Db)
                .DeleteExtraValue(meetingid, field));
        }
        [HttpPost]
        public ActionResult MarkRegistered(int meetingid, int peopleid, bool registered)
        {
            var ret = AuthenticateDeveloper();
            if (ret.StartsWith("!"))
                return Content(ret.Substring(1));
			DbUtil.LogActivity("APIMeeting MarkRegistered {0}, {1}".Fmt(meetingid, peopleid));
            Attend.MarkRegistered(DbUtil.Db, peopleid, meetingid, registered);
            return Content("ok");
        }
    }
}