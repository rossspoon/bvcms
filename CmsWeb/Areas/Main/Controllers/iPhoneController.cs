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
using System.Web.Security;
using CmsWeb.Areas.Manage.Controllers;

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
            AccountController.SetUserInfo(name, Session);

            if (!Util2.OrgMembersOnly && CMSRoleProvider.provider.IsUserInRole(name, "OrgMembersOnly"))
            {
                Util2.OrgMembersOnly = true;
                DbUtil.Db.SetOrgMembersOnly();
            }
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
            AccountController.SetUserInfo(uname, Session);
            if (!CMSRoleProvider.provider.IsUserInRole(uname, "Attendance"))
                return new OrgResult(null);
            return new OrgResult(Util.UserPeopleId);
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
            return new RollListResult(meeting);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult RecordAttend(int id, int PeopleId, bool Present)
        {
#if DEBUG
#else
            if (!Authenticate())
                return Content("not authorized");
#endif
            Attend.RecordAttendance(PeopleId, id, Present);
            DbUtil.Db.UpdateMeetingCounters(id);
            return new EmptyResult();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult RecordVisit(int id, int PeopleId)
        {
#if DEBUG
#else
            if (!Authenticate())
                return Content("not authorized");
#endif
            Attend.RecordAttendance(PeopleId, id, true);
            DbUtil.Db.UpdateMeetingCounters(id);
            var meeting = DbUtil.Db.Meetings.Single(mm => mm.MeetingId == id);
            return new RollListResult(meeting);
        }
        public class PersonInfo
        {
            public int addtofamilyid { get; set; }
            public string addr { get; set; }
            public string zip { get; set; }
            public string first { get; set; }
            public string last { get; set; }
            public string goesby { get; set; }
            public string dob { get; set; }
            public string email { get; set; }
            public string cell { get; set; }
            public string home { get; set; }
            public int marital { get; set; }
            public int gender { get; set; }
        }
#if DEBUG
#else
        [AcceptVerbs(HttpVerbs.Post)]
#endif
        public ActionResult AddPerson(int id, PersonInfo m)
        {
#if DEBUG
#else
            if (!Authenticate())
                return Content("not authorized");
#endif

            CmsData.Family f;
            if (m.addtofamilyid > 0)
                f = DbUtil.Db.Families.First(fam => fam.People.Any(pp => pp.PeopleId == m.addtofamilyid));
            else
                f = new CmsData.Family();

            var position = (int)CmsData.Family.PositionInFamily.Child;
            if (Util.Age0(m.dob) >= 18)
                if (f.People.Count(per =>
                     per.PositionInFamilyId == (int)CmsData.Family.PositionInFamily.PrimaryAdult)
                     < 2)
                    position = (int)CmsData.Family.PositionInFamily.PrimaryAdult;
                else
                    position = (int)CmsData.Family.PositionInFamily.SecondaryAdult;

            var p = Person.Add(f, position,
                null, Trim(m.first), Trim(m.goesby), Trim(m.last), m.dob, false, m.gender,
                    (int)Person.OriginCode.Visit, null);

            var z = DbUtil.Db.ZipCodes.SingleOrDefault(zc => zc.Zip == m.zip.Zip5());
            if (!m.home.HasValue() && m.cell.HasValue())
                m.home = m.cell;
            p.Family.HomePhone = m.home.GetDigits();
            p.Family.AddressLineOne = m.addr;
            p.Family.CityName = z != null ? z.City : null;
            p.Family.StateCode = z != null ? z.State : null;
            p.Family.ZipCode = m.zip;

            p.EmailAddress = Trim(m.email);
            if (m.cell.HasValue())
                p.CellPhone = m.cell.GetDigits();
            p.MaritalStatusId = m.marital;
            p.GenderId = m.gender;
            DbUtil.Db.SubmitChanges();
            var meeting = DbUtil.Db.Meetings.Single(mm => mm.MeetingId == id);
            Attend.RecordAttendance(p.PeopleId, id, true);
            DbUtil.Db.UpdateMeetingCounters(id);
            return new RollListResult(meeting);
        }
        string Trim(string s)
        {
            if (s.HasValue())
                return s.Trim();
            else
                return s;
        }
    }
}
