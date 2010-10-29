using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Data.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using CmsData;
using UtilityExtensions;
using CmsWeb.Models.iPhone;
using System.Xml;
using System.IO;

namespace CmsWeb.Areas.Main.Controllers
{
#if DEBUG
#else
   [RequireHttps]
#endif
    [ValidateInput(false)]
    public class iPhoneController : CmsController
    {
        private bool Authenticate()
        {
            string username, password;
            var auth = Request.Headers["Authorization"];
            if (auth.HasValue())
            {
                var cred = System.Text.ASCIIEncoding.ASCII.GetString(
                    Convert.FromBase64String(auth.Substring(6))).Split(':');
                username = cred[0];
                password = cred[1];
            }
            else
            {
                username = Request.Headers["username"];
                password = Request.Headers["password"];
            }
            return CMSMembershipProvider.provider.ValidateUser(username, password);
        }
        public ActionResult FetchImage(int id)
        {
            if (!Authenticate())
                return Content("not authorized");
            var person = DbUtil.Db.People.Single(pp => pp.PeopleId == id);
            if (person.PictureId != null)
                return new CmsWeb.Models.ImageResult(person.Picture.MediumId ?? 0);
            return new CmsWeb.Models.ImageResult(0);
        }
        public ActionResult Search(string name, string comm, string addr)
        {
#if DEBUG
#else
            if (!Authenticate())
                return Content("not authorized");
#endif
            var m = new SearchModel(name, comm, addr);
            return new SearchResult(m.PeopleList(), m.Count);
        }
        public ActionResult Organizations()
        {
#if DEBUG
            var uname = "david";
#else
            if (!Authenticate())
                return Content("not authorized");
            var uname = Request.Headers["username"];
#endif
            var u = DbUtil.Db.Users.Single(uu => uu.Username == uname);
            return new OrgResult(u.PeopleId.Value);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult RollList(int id, string datetime)
        {
#if DEBUG
            var uname = "david";
#else
            if (!Authenticate())
                return Content("not authorized");
            var uname = Request.Headers["username"];
#endif
            var u = DbUtil.Db.Users.Single(uu => uu.Username == uname);
            var dt = DateTime.Parse(datetime);
            var meeting = DbUtil.Db.Meetings.SingleOrDefault(m => m.OrganizationId== id && m.MeetingDate == dt);
            if (meeting == null)
            {
                meeting = new CmsData.Meeting
                {
                    OrganizationId = id,
                    MeetingDate = dt,
                    CreatedDate = Util.Now,
                    CreatedBy = u.UserId,
                    GroupMeetingFlag = false,
                };
                DbUtil.Db.Meetings.InsertOnSubmit(meeting);
                DbUtil.Db.SubmitChanges();
            }
            return new RollListResult(id, meeting);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult RecordAttend(int id, int PeopleId, bool Present)
        {
            Attend.RecordAttendance(PeopleId, id, Present);
            DbUtil.Db.UpdateMeetingCounters(id);
            return new EmptyResult();
        }
    }
}
