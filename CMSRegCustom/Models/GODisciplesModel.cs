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
        public int? married { get; set; }
        public DateTime birthday;
        public string phone { get; set; }
        public string homecell { get; set; }
        public string email { get; set; }
        public bool preferredemail { get; set; }

        public bool shownew { get; set; }
        public string addr { get; set; }
        public string zip { get; set; }
        public string city { get; set; }
        public string state { get; set; }

        private DiscData.User discuser;
        private Organization neworg;
        private Organization leaderorg;
        private DiscData.Group discorg;
        public string GroupDescription
        {
            get
            {
                if (neworg == null)
                    return "GO Disciples Leaders";
                else
                    return "GO Disciples, " + neworg.OrganizationName;
            }
        }
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
                if (!married.HasValue)
                    modelState.AddModelError("married", "marital status required");
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
                    else
                        AddPerson();
            }
        }
        private void AddPerson()
        {
            var f = new Family
            {
                AddressLineOne = addr,
                CityName = city,
                StateCode = state,
                ZipCode = zip,
            };
            var p = Person.Add(f, 30,
                null, first.Trim(), null, last.Trim(), dob, married.Value == 20, gender.Value,
                    DbUtil.Settings("GODisciplesOrigin", "0").ToInt(),
                    DbUtil.Settings("GODisciplesEntry", "0").ToInt());
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

            var groupname = "{0} {1} Group".Fmt(
                person.NickName.HasValue() ? person.NickName : person.FirstName,
                person.LastName);
            var g = DiscData.Group.LoadByName(groupname);
            if (!DiscData.Group.IsUserAdmin(discuser, groupname))
            {
                // make a group on disciples with a unique name
                var startname = groupname;
                var i = 1;
                while (DiscData.Group.LoadByName(groupname) != null
                    || DbUtil.Db.Organizations.SingleOrDefault(o => o.OrganizationName == groupname) != null)
                    groupname = startname + " " + i++;
                DiscData.Group.InsertWithRolesOnSubmit(groupname);
                DiscData.DbUtil.Db.SubmitChanges();
                g = DiscData.Group.LoadByName(groupname);

                // add Welcome Text
                var cw = DbUtil.Content("GODisciplesGroupWelcome");
                if (cw != null)
                {
                    g.WelcomeText.Title = cw.Title.Replace("{name}", groupname);
                    g.WelcomeText.Body = cw.Body.Replace("{leader}", person.Name);
                }
                discuser.ForceLogin = true;
                DiscData.DbUtil.Db.SubmitChanges();

                // make a blog on disciples
                var b = new DiscData.Blog();
                b.Title = groupname + " Blog";
                b.Name = b.Title.Replace(" ", "");
                b.Description = DbUtil.Settings("GODisciplesBlogDescription", "A Small Group Discussion");
                b.GroupId = g.Id;
                DiscData.DbUtil.Db.Blogs.InsertOnSubmit(b);
                DiscData.DbUtil.Db.SubmitChanges();

                // make a new first post on blog
                var firstpost = DbUtil.Content("GODisciplesFirstPost");
                var p = b.NewPost(firstpost.Title, firstpost.Body, discuser.Username, DateTime.Now);
                var cat = DiscData.DbUtil.Db.Categories.Single(ca => ca.Name == "Discipleship");
                var bc = new DiscData.BlogCategoryXref { CatId = cat.Id };
                p.BlogCategoryXrefs.Add(bc);
                DiscData.DbUtil.Db.SubmitChanges();

                // create a new cms org
                leaderorg = DbUtil.Db.Organizations.SingleOrDefault(o =>
                    o.OrganizationId == DbUtil.Settings("GODisciplesLeadersOrgId", "0").ToInt());
                neworg = leaderorg.CloneOrg();
                neworg.OrganizationName = groupname;
                DbUtil.Db.SubmitChanges();
            }
            else
            {
                leaderorg = DbUtil.Db.Organizations.SingleOrDefault(o =>
                    o.OrganizationId == DbUtil.Settings("GODisciplesLeadersOrgId", "0").ToInt());
                neworg = DbUtil.Db.Organizations.SingleOrDefault(o =>
                    o.OrganizationName == groupname);
            }


            g.SetAdmin(discuser, true);
            g.SetBlogger(discuser, true);
            g.SetMember(discuser, true);

            var leaderg = DiscData.Group.LoadByName(DbUtil.Settings("GoDisciplesLeadersGroup", "GO Disciples Leaders"));
            leaderg.SetMember(discuser, true);
            discuser.DefaultGroup = leaderg.Name;

            DiscData.DbUtil.Db.SubmitChanges();

            // make member of leaders
            OrganizationMember.InsertOrgMembers(leaderorg.OrganizationId, person.PeopleId,
                (int)OrganizationMember.MemberTypeCode.Member,
                DateTime.Now, null, false);

            // make leader of own new org
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
            discuser.DefaultGroup = g.Name;
            discuser.ForceLogin = true;
            DiscData.DbUtil.Db.SubmitChanges();
        }
        private string _username;
        private string username
        {
            get
            {
                if (_username == null)
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
                    var dser = q.FirstOrDefault();
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
            do
            {
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
            {
                discuser = DiscData.BVMembershipProvider.MakeNewUser(
                    username, password, email,
                    true, person.PeopleId);
                discuser = DiscData.DbUtil.Db.Users.Single(u => u.UserId == discuser.UserId);
                discuser.FirstName = person.FirstName;
                discuser.LastName = person.LastName;
                discuser.BirthDay = person.GetBirthdate();
            }
            DiscData.DbUtil.Db.SubmitChanges();
        }
        private void MakeUserOnCms()
        {
            const string STR_Attendance = "Attendance";
            const string STR_Staff = "Staff";
            var user = DbUtil.Db.Users.FirstOrDefault(u => u.Username == username);
            if (user != null)
            {
                MembershipService.ChangePassword(username, password);
                user = DbUtil.Db.Users.FirstOrDefault(u => u.Username == username);
                var roles = user.Roles.ToList();
                if (!roles.Contains(STR_Attendance))
                    roles.Add(STR_Attendance);
                if (!roles.Contains(STR_Staff))
                    roles.Add(STR_Staff);
                user.Roles = roles.ToArray();
            }
            else
            {
                user = MembershipService.CreateUser(person.PeopleId, username, password);
                user.Roles = new string[] { "OrgMembersOnly", STR_Attendance, STR_Staff };
            }
            DbUtil.Db.SubmitChanges();
        }
        public void EmailLeaderNotices()
        {
            string adminmail = DbUtil.Settings("GODisciplesMail", DbUtil.SystemEmailAddress);
            var c = DbUtil.Content("GODisciplesLeaderConfirm");
            if (c == null)
                return;
            var p = person;
            var Body = c.Body;
            Body = Body.Replace("{first}", p.NickName.HasValue() ? p.NickName : p.FirstName);
            Body = Body.Replace("{username}", discuser.Username);
            Body = Body.Replace("{password}", discuser.Password);
            Body = Body.Replace("{groupname}", neworg.OrganizationName);
            Body = Body.Replace("{membersignupurl}", MemberSignupUrl);
            Body = Body.Replace("{cmsorgpageurl}", CmsOrgPageUrl);
            Body = Body.Replace("{minister}", DbUtil.Settings("GODisciplesMinister", "GO Disciples Team"));
            Body = Body.Replace("{disciplesurl}", DbUtil.Settings("GODisciplesURL", "http://disciples.bellevue.org"));

            var smtp = new SmtpClient();
            Util.Email(smtp, adminmail, p.Name, email, c.Title, Body);
            Util.Email2(smtp, email, adminmail, "new GO leader registration in cms",
                "{0}({1},{2}) joined {3}\r\nand has {4} own {5}".Fmt(
                p.Name, p.PeopleId, discuser.Username, leaderorg.OrganizationName,
                p.GenderId == 2 ? "her" : "his",
                neworg.OrganizationName));

            UpdateEmailPhone(smtp, adminmail, p);
        }
        public void EmailMemberNotices()
        {
            string adminmail = DbUtil.Settings("GODisciplesMail", DbUtil.SystemEmailAddress);
            var c = DbUtil.Content("GODisciplesConfirm");
            if (c == null)
                return;
            var p = person;
            var Body = c.Body;
            Body = Body.Replace("{first}", p.NickName.HasValue() ? p.NickName : p.FirstName);
            Body = Body.Replace("{username}", discuser.Username);
            Body = Body.Replace("{password}", discuser.Password);
            Body = Body.Replace("{groupname}", neworg.OrganizationName);
            Body = Body.Replace("{minister}", DbUtil.Settings("GODisciplesMinister", "GO Disciples Team"));
            Body = Body.Replace("{disciplesurl}", DbUtil.Settings("GODisciplesURL", "http://disciples.bellevue.org"));

            var smtp = new SmtpClient();
            Util.Email(smtp, adminmail, p.Name, email, c.Title, Body);
            Util.Email2(smtp, email, adminmail, "new GO disciple registration in cms",
                "{0}({1},{2}) joined {3}".Fmt(p.Name, p.PeopleId, discuser.Username, neworg.OrganizationName));
            var q = from om in neworg.OrganizationMembers
                    where om.MemberTypeId == neworg.LeaderMemberTypeId
                    select om.Person;
            var leader = q.FirstOrDefault();
            if (leader != null)
                Util.Email2(smtp, email, leader.EmailAddress, "new GO disciple registration",
                    "{0}({1},{2}) joined {3}".Fmt(p.Name, p.PeopleId, discuser.Username, neworg.OrganizationName));
            UpdateEmailPhone(smtp, adminmail, p);
        }
        private void UpdateEmailPhone(SmtpClient smtp, string adminmail, Person p)
        {
            if (email != p.EmailAddress && preferredemail)
            {
                const string subject = "updated email address";
                const string message =
@"We have updated your email address from {0} to {1}.<br />
If this is not correct, please reply and let us know.";

                Util.Email(smtp, adminmail, p.Name, email, subject,
                    message.Fmt(p.EmailAddress, email));
                Util.Email(smtp, adminmail, p.Name, p.EmailAddress, subject,
                    message.Fmt(p.EmailAddress, email));
                p.EmailAddress = email;
            }
            if (homecell == "c" && !p.CellPhone.EndsWith(phone.GetDigits()))
            {
                const string subject = "updated cell phone";
                const string message =
@"We have updated your cell phone from {0} to {1}.<br />
If this is not correct, please reply and let us know.";
                var oldphone = p.CellPhone.FmtFone();
                if (oldphone.HasValue())
                    Util.Email(smtp, adminmail, p.Name, p.EmailAddress, subject,
                        message.Fmt(oldphone, phone.FmtFone()));
                p.CellPhone = phone;
            }
            DbUtil.Db.SubmitChanges();
        }
    }
}
