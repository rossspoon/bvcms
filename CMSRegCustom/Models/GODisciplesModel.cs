using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Web;
using CmsData;
using System.Web.Mvc;
using System.Text;
using System.Configuration;
using UtilityExtensions;
using System.Net.Mail;
using System.Web.Security;

namespace CMSRegCustom.Models
{
    public class GODisciplesModel
    {
        public string action { get; set; }

        public GODisciplesModel(string action)
        {
            this.action = action;
        }
        public GODisciplesModel(string action, int id)
            : this(action)
        {
            neworg = DbUtil.Db.Organizations.SingleOrDefault(o =>
                o.OrganizationId == id);
        }
        public int neworgid { get { return neworg.OrganizationId; } }
        public int? peopleid { get; set; }
        private Person _person;
        public Person person
        {
            get
            {
                if (_person == null)
                    _person = DbUtil.Db.People.SingleOrDefault(p => p.PeopleId == peopleid);
                return _person;
            }
        }
        public string first { get; set; }
        public string last { get; set; }
        public string dob { get; set; }
        public int? gender { get; set; }
        public DateTime birthday;
        public string phone { get; set; }
        public string homecell { get; set; }
        public string email { get; set; }
        public bool preferredEmail { get; set; }

        public bool shownew { get; set; }
        public string addr { get; set; }
        public string zip { get; set; }
        public string city { get; set; }
        public string state { get; set; }

        private DiscData.User discuser;
        private Organization neworg;
        private Organization leaderorg;
        private DiscData.Group discorg;
        public string MemberSignupUrl
        {
            get
            {
                var r = HttpContext.Current.Request;
                return "{0}://{1}/GODisciples/Disciple/{2}".Fmt(
                    r.Url.Scheme, r.Url.Authority, neworg.OrganizationId);
            }
        }
        public string CmsOrgPageUrl
        {
            get
            {
                var r = HttpContext.Current.Request;
                return "{0}://{1}/Organization.aspx?id={2}".Fmt(
                    r.Url.Scheme, r.Url.Authority, neworg.OrganizationId);
            }
        }

        public int FindMember()
        {
            first = first.Trim();
            last = last.Trim();
            var fone = Util.GetDigits(phone);
            var q = from p in DbUtil.Db.People
                    where (p.FirstName == first || p.NickName == first || p.MiddleName == first)
                    where (p.LastName == last || p.MaidenName == last)
                    where p.BirthDay == birthday.Day && p.BirthMonth == birthday.Month && p.BirthYear == birthday.Year
                    select p;
            var count = q.Count();
            if (count > 1)
                q = from p in q
                    where p.CellPhone.Contains(fone)
                            || p.WorkPhone.Contains(fone)
                            || p.Family.HomePhone.Contains(fone)
                    select p;
            count = q.Count();

            peopleid = null;
            if (count == 1)
                peopleid = q.Select(p => p.PeopleId).Single();
            return count;
        }

        public void ValidateModel(ModelStateDictionary modelState)
        {
            if (!first.HasValue())
                modelState.AddModelError("first", "first name required");
            if (!last.HasValue())
                modelState.AddModelError("last", "last name required");
            if (!Util.DateValid(dob, out birthday))
                modelState.AddModelError("dob", "valid birth date required");

            var d = phone.GetDigits().Length;
            if (d != 7 && d != 10)
                modelState.AddModelError("phone", "7 or 10 digits");
            if (!email.HasValue() || !Util.ValidEmail(email))
                modelState.AddModelError("email", "Please specify a valid email address.");
            if (shownew)
            {
                if (!gender.HasValue)
                    modelState.AddModelError("gender", "gender required");
                if (!addr.HasValue())
                    modelState.AddModelError("addr", "need address");
                if (zip.GetDigits().Length != 5)
                    modelState.AddModelError("zip", "need 5 digit zip");
                if (!city.HasValue())
                    modelState.AddModelError("city", "need city");
                if (!state.HasValue())
                    modelState.AddModelError("state", "need state");
            }
            if (modelState.IsValid)
            {
                var count = FindMember();
                if (count > 1)
                    modelState.AddModelError("find", "More than one match, sorry");
                else if (count == 0)
                    if (!shownew)
                    {
                        modelState.AddModelError("find", "Cannot find church record.");
                        shownew = true;
                    }
            }
        }
        internal void AddPerson()
        {
            var f = new Family
            {
                AddressLineOne = addr,
                CityName = city,
                StateCode = state,
                ZipCode = zip,
            };
            var p = Person.Add(f, 30,
                null, first.Trim(), null, last.Trim(), dob, false, 1,
                    DbUtil.Settings("GODisciplesOrigin", "0").ToInt(),
                    DbUtil.Settings("GODisciplesEntry", "0").ToInt());
            p.MaritalStatusId = (int)Person.MaritalStatusCode.Unknown;
            p.EmailAddress = email;
            p.CampusId = DbUtil.Settings("DefaultCampusId", "").ToInt2();
            if (p.Age >= 18)
                p.PositionInFamilyId = (int)Family.PositionInFamily.PrimaryAdult;
            switch (homecell)
            {
                case "h":
                    f.HomePhone = phone.GetDigits();
                    break;
                case "c":
                    p.CellPhone = phone.GetDigits();
                    break;
            }
            DbUtil.Db.SubmitChanges();
            peopleid = p.PeopleId;
        }
        internal void PerformLeaderSetup()
        {
            MakeUserOnCms();
            MakeDiscUser();

            // make a group on disciples with a unique name

            var startname = "{0} {1} Group".Fmt(person.FirstName, person.LastName);
            var groupname = startname;
            var i = 1;
            while (DiscData.Group.LoadByName(groupname) != null
                && DbUtil.Db.Organizations.SingleOrDefault(o => o.OrganizationName == groupname) != null)
                groupname = startname + " " + i++;

            DiscData.Group.InsertWithRolesOnSubmit(groupname);
            DiscData.DbUtil.Db.SubmitChanges();
            var g = DiscData.Group.LoadByName(groupname);
            g.SetAdmin(discuser, true);
            g.SetBlogger(discuser, true);
            g.SetMember(discuser, true);
            DiscData.DbUtil.Db.SubmitChanges();

            var leaderg = DiscData.Group.LoadByName(DbUtil.Settings("GoDisciplesLeadersGroup", "GO Disciples Leaders"));


            // add Welcome Text
            var cw = DbUtil.Content("GODisciplesGroupWelcome");
            if (cw != null)
            {
                g.WelcomeText.Title = cw.Title.Replace("{name}", groupname);
                g.WelcomeText.Body = cw.Body.Replace("{leader}", person.Name);
            }

            // make a blog on disciples
            var b = new DiscData.Blog();
            b.Title = groupname + " Blog";
            b.Name = b.Title.Replace(" ", "");
            b.Description = DbUtil.Settings("GODisciplesBlogDescription", "A Small Group Discussion");
            b.GroupId = g.Id;
            DiscData.DbUtil.Db.Blogs.InsertOnSubmit(b);
            DiscData.DbUtil.Db.SubmitChanges();

            // create a new cms org
            leaderorg = DbUtil.Db.Organizations.SingleOrDefault(o =>
                o.OrganizationId == DbUtil.Settings("GODisciplesLeadersOrgId", "0").ToInt());

            neworg = leaderorg.CloneOrg();
            neworg.OrganizationName = groupname;
            DbUtil.Db.SubmitChanges();

            // make member of leaders
            OrganizationMember.InsertOrgMembers(leaderorg.OrganizationId, person.PeopleId,
                (int)OrganizationMember.MemberTypeCode.Member,
                DateTime.Now, null, false);

            // make leader of own new group
            OrganizationMember.InsertOrgMembers(neworg.OrganizationId, person.PeopleId,
                (int)OrganizationMember.MemberTypeCode.Leader,
                DateTime.Now, null, false);
        }
        public void PerformMemberSetup()
        {
            OrganizationMember.InsertOrgMembers(neworg.OrganizationId, person.PeopleId,
                (int)OrganizationMember.MemberTypeCode.Member,
                DateTime.Now, null, false);
            MakeDiscUser();
            var g = DiscData.Group.LoadByName(neworg.OrganizationName);
            g.SetMember(discuser, true);
            DiscData.DbUtil.Db.SubmitChanges();
        }
        private string _username;
        private string username
        {
            get
            {
                if(_username == null)
                {
                    var q = from u in DbUtil.Db.Users
                            where u.PeopleId == person.PeopleId
                            orderby u.LastActivityDate descending
                            select u;
                    var user = q.FirstOrDefault();
                    if (user != null)
                        _username = user.Username;
                    else
                        _username = MembershipService.FetchUsername(
                            person.FirstName, person.LastName);
                }
                return _username;
            }
        }
        private string _password;
        private string password
        {
            get
            {
                if (_password == null)
                {
                    var q = from u in DiscData.DbUtil.Db.Users
                             where u.PeopleId == person.PeopleId
                             orderby u.LastActivityDate descending
                             select u;
                    var dser = q.SingleOrDefault();
                    if (dser != null && Membership.Provider.ValidateUser(username, dser.Password))
                        _password = dser.Password;
                    else
                        _password = MembershipService.FetchPassword();
                }
                return _password;
            }
        }
        private void MakeDiscUser()
        {
            bool userexists;
            do {
                var q3 = from u in DiscData.DbUtil.Db.Users
                         where u.Username == username
                         where u.PeopleId != person.PeopleId
                         orderby u.LastActivityDate descending
                         select u;
                userexists = q3.SingleOrDefault() != null;
                if (userexists)
                    _username = _username + "1";
            } while (userexists);

            var q2 = from u in DiscData.DbUtil.Db.Users
                     where u.PeopleId == person.PeopleId
                     orderby u.LastActivityDate descending
                     select u;
            discuser = q2.FirstOrDefault();

            if (discuser != null)
            {
                discuser.Password = password;
                discuser.Username = username; // force username to be this name
            }
            else
                discuser = DiscData.BVMembershipProvider.MakeNewUser(
                    username, password, email,
                    true, person.PeopleId);

            DiscData.DbUtil.Db.SubmitChanges();
        }
        private void MakeUserOnCms()
        {
            var user = DbUtil.Db.Users.FirstOrDefault(u => u.Username == username);
            if (user != null)
                MembershipService.ChangePassword(username, password);
            else
                user = MembershipService.CreateUser(person.PeopleId, username, password);
        }
        public void EmailLeaderNotices()
        {
            string email = DbUtil.Settings("GODisciplesMail", DbUtil.SystemEmailAddress);
            var c = DbUtil.Content("GODisciplesLeaderConfirm");
            if (c == null)
                return;
            var p = person;
            c.Body = c.Body.Replace("{first}", p.NickName.HasValue() ? p.NickName : p.FirstName);
            c.Body = c.Body.Replace("{username}", discuser.Username);
            c.Body = c.Body.Replace("{password}", discuser.Password);
            c.Body = c.Body.Replace("{groupname}", neworg.OrganizationName);
            c.Body = c.Body.Replace("{membersignupurl}", MemberSignupUrl);
            c.Body = c.Body.Replace("{cmsorgpageurl}", CmsOrgPageUrl);
            c.Body = c.Body.Replace("{minister}", DbUtil.Settings("GODisciplesMinister", "GO Disciples Team"));
            c.Body = c.Body.Replace("{disciplesurl}", DbUtil.Settings("GODisciplesURL", "http://disciples.bellevue.org"));

            var smtp = new SmtpClient();
            Util.Email(smtp, email, p.Name, p.EmailAddress, c.Title, c.Body);
            Util.Email2(smtp, p.EmailAddress, email, "new GO leader registration in cms",
                "{0}({1}) joined {2}<br/>\nand has his own {3}".Fmt(
                p.Name, p.PeopleId, leaderorg.OrganizationName, neworg.OrganizationName));
        }
        public void EmailMemberNotices()
        {
            string email = DbUtil.Settings("GODisciplesMail", DbUtil.SystemEmailAddress);
            var c = DbUtil.Content("GODisciplesConfirm");
            if (c == null)
                return;
            var p = person;
            c.Body = c.Body.Replace("{first}", p.NickName.HasValue() ? p.NickName : p.FirstName);
            c.Body = c.Body.Replace("{username}", discuser.Username);
            c.Body = c.Body.Replace("{password}", discuser.Password);
            c.Body = c.Body.Replace("{groupname}", neworg.OrganizationName);
            c.Body = c.Body.Replace("{minister}", DbUtil.Settings("GODisciplesMinister", "GO Disciples Team"));
            c.Body = c.Body.Replace("{disciplesurl}", DbUtil.Settings("GODisciplesURL", "http://disciples.bellevue.org"));

            var smtp = new SmtpClient();
            Util.Email(smtp, email, p.Name, p.EmailAddress, c.Title, c.Body);
            Util.Email2(smtp, p.EmailAddress, email, "new GO disciple registration in cms",
                "{0}({1}) joined {2}".Fmt(p.Name, p.PeopleId, neworg.OrganizationName));
            var leader = DbUtil.Db.People.Single(l => l.PeopleId == neworg.LeaderId);
            Util.Email2(smtp, p.EmailAddress, leader.EmailAddress, "new GO disciple registration",
                "{0}({1}) joined {2}".Fmt(p.Name, p.PeopleId, neworg.OrganizationName));
        }
    }
}
