using System;
using System.Collections.Generic;
using System.Linq;
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
using CmsData.Codes;

namespace CmsWeb.Areas.Public.Controllers
{
#if DEBUG
#else
   [RequireHttps]
#endif
    [ValidateInput(false)]
    public class APIiPhoneController : CmsController
    {
        private string getUsername()
        {
            string username;
            var auth = Request.Headers["Authorization"];
            if (auth.HasValue())
            {
                var cred = System.Text.ASCIIEncoding.ASCII.GetString(
                    Convert.FromBase64String(auth.Substring(6))).Split(':');
                username = cred[0];
            }
            else
                username = Request.Headers["username"];
            return username;
        }
        private bool Authenticate(string role = null, bool checkorgmembersonly = false)
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
            var roles = CMSRoleProvider.provider;
            var ret = false;
            if (password == DbUtil.Db.Setting("ImpersonatePassword", null))
                ret = true;
            else
                ret = CMSMembershipProvider.provider.ValidateUser(username, password);
            if (ret && role.HasValue() && roles.RoleExists(role))
            {
                CmsWeb.Models.AccountModel.SetUserInfo(username, Session);
                if (!roles.IsUserInRole(username, role))
                    ret = false;
            }
            if (ret)
                if (checkorgmembersonly)
                    if (!Util2.OrgMembersOnly)
                    {
                        if (roles.IsUserInRole(username, "OrgMembersOnly"))
                        {
                            Util2.OrgMembersOnly = true;
                            DbUtil.Db.SetOrgMembersOnly();
                        }
                    }
                    else if (!Util2.OrgLeadersOnly)
                        if (roles.IsUserInRole(username, "OrgLeadersOnly"))
                        {
                            Util2.OrgLeadersOnly = true;
                            DbUtil.Db.SetOrgLeadersOnly();
                        }
            return ret;
        }
        public ActionResult FetchImage(int id)
        {
            if (!Authenticate("Access"))
                return Content("not authorized");
            var person = DbUtil.Db.People.Single(pp => pp.PeopleId == id);
            if (person.PictureId != null)
                return new CmsWeb.Models.ImageResult(person.Picture.MediumId ?? 0);
            return new CmsWeb.Models.ImageResult(0);
        }
        public ActionResult Search(string name, string comm, string addr)
        {
            if (!Authenticate())
                return Content("not authorized");
            var uname = getUsername();
            CmsWeb.Models.AccountModel.SetUserInfo(uname, Session);

            if (!Util2.OrgMembersOnly && CMSRoleProvider.provider.IsUserInRole(name, "OrgMembersOnly"))
            {
                Util2.OrgMembersOnly = true;
                DbUtil.Db.SetOrgMembersOnly();
            }
            else if (!Util2.OrgLeadersOnly && CMSRoleProvider.provider.IsUserInRole(name, "OrgLeadersOnly"))
            {
                Util2.OrgLeadersOnly = true;
                DbUtil.Db.SetOrgLeadersOnly();
            }
            var m = new SearchModel(name, comm, addr);
            return new SearchResult0(m.PeopleList(), m.Count);
        }
        public ActionResult SearchResults(string name, string comm, string addr)
        {
            if (!Authenticate() )
                return Content("not authorized");
            var uname = getUsername();
            CmsWeb.Models.AccountModel.SetUserInfo(uname, Session);
            if (!CMSRoleProvider.provider.IsUserInRole(uname, "Access"))
                return Content("not authorized");

            if (!Util2.OrgMembersOnly && CMSRoleProvider.provider.IsUserInRole(uname, "OrgMembersOnly"))
            {
                Util2.OrgMembersOnly = true;
                DbUtil.Db.SetOrgMembersOnly();
            }
            else if (!Util2.OrgLeadersOnly && CMSRoleProvider.provider.IsUserInRole(uname, "OrgLeadersOnly"))
            {
                Util2.OrgLeadersOnly = true;
                DbUtil.Db.SetOrgLeadersOnly();
            }
            var m = new SearchModel(name, comm, addr);
            return new SearchResult(m.PeopleList(), m.Count);
        }
        public ActionResult DetailResults(int id)
        {
            if (!Authenticate())
                return Content("not authorized");
            return new DetailResult(id);
        }
        public ActionResult Organizations()
        {
            if (!Authenticate())
                return Content("not authorized");
            var uname = getUsername();
            CmsWeb.Models.AccountModel.SetUserInfo(uname, Session);
            if (!CMSRoleProvider.provider.IsUserInRole(uname, "Attendance"))
                return new OrgResult(null);
            return new OrgResult(Util.UserPeopleId);
        }
        [HttpPost]
        public ActionResult RollList( int id, string datetime )
            // id = OrganizationId
            // datetime = MeetingDate
        {
            if (!Authenticate())
                return Content("not authorized");
            var uname = getUsername();
            var u = DbUtil.Db.Users.Single(uu => uu.Username == uname);
            var dt = DateTime.Parse(datetime);
            var meeting = DbUtil.Db.Meetings.SingleOrDefault(m => m.OrganizationId == id && m.MeetingDate == dt);
            if (meeting == null)
            {
                var acr = (from s in DbUtil.Db.OrgSchedules
                           where s.OrganizationId == id
                           where s.SchedTime.Value.TimeOfDay == dt.TimeOfDay
                           where s.SchedDay == (int)dt.DayOfWeek
                           select s.AttendCreditId).SingleOrDefault();
                meeting = new CmsData.Meeting
                {
                    OrganizationId = id,
                    MeetingDate = dt,
                    CreatedDate = Util.Now,
                    CreatedBy = u.UserId,
                    GroupMeetingFlag = false,
                    AttendCreditId = acr
                };
                DbUtil.Db.Meetings.InsertOnSubmit(meeting);
                DbUtil.Db.SubmitChanges();

                DbUtil.Db.EmailRedacted(DbUtil.AdminMail,
                    CMSRoleProvider.provider.GetDevelopers(),
                    "meeting created with iphone on " + Util.Host,
                    "{0} <a href='{1}'>meeting</a> created by {2}<br/>"
                        .Fmt(meeting.Organization.OrganizationName,
                        Util.ResolveServerUrl("/Meeting/Index/" + meeting.MeetingId), u.Name));
            }
            return new RollListResult(meeting);
        }
        [HttpPost]
        public ActionResult RecordAttend( int id, int PeopleId, bool Present )
            // id = MeetingId
        {
            if (!Authenticate())
                return Content("not authorized");
            Attend.RecordAttendance(PeopleId, id, Present);
            DbUtil.Db.UpdateMeetingCounters(id);
            return new EmptyResult();
        }
        [HttpPost]
        public ActionResult RecordVisit( int id, int PeopleId )
            // id = MeetingId
        {
            if (!Authenticate())
                return Content("not authorized");
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
        [HttpPost]
        public ActionResult AddPerson(int id, PersonInfo m)
        {
            if (!Authenticate())
                return Content("not authorized");

            CmsData.Family f;
            if (m.addtofamilyid > 0)
                f = DbUtil.Db.Families.First(fam => fam.People.Any(pp => pp.PeopleId == m.addtofamilyid));
            else
                f = new CmsData.Family();

            if (m.goesby == "(Null)")
                m.goesby = null;
            var position = PositionInFamily.Child;
            if (Util.Age0(m.dob) >= 18)
                if (f.People.Count(per =>
                     per.PositionInFamilyId == PositionInFamily.PrimaryAdult)
                     < 2)
                    position = PositionInFamily.PrimaryAdult;
                else
                    position = PositionInFamily.SecondaryAdult;

            var p = Person.Add(f, position,
                null, Trim(m.first), Trim(m.goesby), Trim(m.last), m.dob, false, m.gender,
                    OriginCode.Visit, null);

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
        [HttpPost]
        public ActionResult JoinUnJoinOrg(int PeopleId, int OrgId, bool Member)
        {
            if (!Authenticate())
                return Content("not authorized");
            var om = DbUtil.Db.OrganizationMembers.SingleOrDefault(m => m.PeopleId == PeopleId && m.OrganizationId == OrgId);
            if (om == null && Member)
                om = OrganizationMember.InsertOrgMembers(DbUtil.Db,
                    OrgId, PeopleId, MemberTypeCode.Member, DateTime.Now, null, false);
            else if (om != null && !Member)
                om.Drop(DbUtil.Db, addToHistory: true);
            DbUtil.Db.SubmitChanges();
            //if (om != null && om.Organization.EmailAddresses.HasValue())
            //{
            //    var smtp = Util.Smtp();
            //    Util.Email(smtp, null, om.Organization.EmailAddresses, 
            //        "cms check-in, join class " + Util.CmsHost, 
            //        "<a href='{0}/Person/Index/{1}'>{2}</a> joined {3}".Fmt( 
            //            Util.ServerLink("/Person/Index/" + om.PeopleId), 
            //            om.PeopleId, om.Person.Name, om.Organization.OrganizationName));
            //}
            return Content("OK");
        }
        private string Trim(string s)
        {
            if (s.HasValue())
                return s.Trim();
            else
                return s;
        }
        [HttpPost]
        public ActionResult RollList2(int id, string datetime)
            // id = OrganizationId
        {
            if (!Authenticate())
                return Content("not authorized");
            var uname = getUsername();
            var u = DbUtil.Db.Users.Single(uu => uu.Username == uname);
            var dt = DateTime.Parse(datetime);
            return new RollListResult(id, dt);
        }
        [HttpPost]
        public ActionResult RecordAttend2(int id, string datetime, int PeopleId, bool Present)
            // id = OrganizationId
        {
            if (!Authenticate())
                return Content("not authorized");
            var uname = getUsername();
            var dt = DateTime.Parse(datetime);
            var u = DbUtil.Db.Users.Single(uu => uu.Username == uname);
            RecordAttend2Extracted(id, PeopleId, Present, dt, u);
            return new EmptyResult();
        }
        [HttpPost]
        public ActionResult RecordVisit2(int id, string datetime, int PeopleId)
            // id = OrganizationId
        {
            if (!Authenticate())
                return Content("not authorized");
            var uname = getUsername();
            var dt = DateTime.Parse(datetime);
            var u = DbUtil.Db.Users.Single(uu => uu.Username == uname);

            RecordAttend2Extracted(id, PeopleId, true, dt, u);
            return new RollListResult(id, dt);
        }
        private static void RecordAttend2Extracted(int id, int PeopleId, bool Present, DateTime dt, User u)
        {
            var meeting = DbUtil.Db.Meetings.SingleOrDefault(m => m.OrganizationId == id && m.MeetingDate == dt);
            if (meeting == null)
            {
                var acr = (from s in DbUtil.Db.OrgSchedules
                               where s.OrganizationId == id
                               where s.SchedTime.Value.TimeOfDay == dt.TimeOfDay
                               where s.SchedDay == (int)dt.DayOfWeek
                               select s.AttendCreditId).SingleOrDefault();
                meeting = new CmsData.Meeting
                {
                    OrganizationId = id,
                    MeetingDate = dt,
                    CreatedDate = Util.Now,
                    CreatedBy = u.UserId,
                    GroupMeetingFlag = false,
                    AttendCreditId = acr,
                };
                DbUtil.Db.Meetings.InsertOnSubmit(meeting);
                DbUtil.Db.SubmitChanges();
                DbUtil.Db.EmailRedacted(DbUtil.AdminMail,
                    CMSRoleProvider.provider.GetDevelopers(),
                    "meeting created with iphone on {0}".Fmt(Util.Host),
                    "{0} <a href='{1}'>meeting</a> created by {2}<br/>"
                        .Fmt(meeting.Organization.OrganizationName,
                        Util.ResolveServerUrl("/Meeting/Index/" + meeting.MeetingId)));
            }
            Attend.RecordAttendance(PeopleId, meeting.MeetingId, Present);
            DbUtil.Db.UpdateMeetingCounters(id);
            var n = DbUtil.Db.Attends.Count(a => a.MeetingId == meeting.MeetingId && a.AttendanceFlag == true);
            if (n == 0)
            {
                DbUtil.Db.Meetings.DeleteOnSubmit(meeting);
                DbUtil.Db.SubmitChanges();
            }
        }
    }
}
