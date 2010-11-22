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
using System.Data.Linq.SqlClient;
using CMSPresenter;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Collections;
using System.Runtime.Serialization;

namespace CmsWeb.Models
{
    [Serializable]
    public class OnlineRegModel
    {
        [NonSerialized]
        private CmsData.Division _div;
        public CmsData.Division div
        {
            get
            {
                if (_div == null && divid.HasValue)
                    _div = DbUtil.Db.Divisions.SingleOrDefault(d => d.Id == divid);
                return _div;
            }
        }
        public string URL { get; set; }

        public static Organization CreateAccountOrg
        {
            get
            {
                return new Organization
                {
                    OrganizationName = "Create Account",
                    RegistrationTypeId = (int)Organization.RegistrationEnum.CreateAccount,
                    AllowOnlyOne = true,
                };
            }
        }
        [NonSerialized]
        private CmsData.Organization _org;
        public CmsData.Organization org
        {
            get
            {
                if (_org == null && orgid.HasValue)
                    if (orgid == Util.CreateAccountCode)
                        _org = CreateAccountOrg;
                    else
                        _org = DbUtil.Db.LoadOrganizationById(orgid.Value);
                return _org;
            }
        }

        public int? divid { get; set; }
        public int? orgid { get; set; }
        [OptionalField]
        private int? _Classid;
        public int? classid
        {
            get { return _Classid; }
            set { _Classid = value; }
        }

        [OptionalField]
        private string _Username;
        public string username
        {
            get { return _Username; }
            set { _Username = value; }
        }
        [OptionalField]
        private string _Password;
        public string password
        {
            get { return _Password; }
            set { _Password = value; }
        }
        [OptionalField]
        private bool _Nologin;
        public bool nologin
        {
            get { return _Nologin; }
            set { _Nologin = value; }
        }

        public bool DisplayLogin()
        {
            return (List.Count == 0 && !UserPeopleId.HasValue && !IsCreateAccount() && nologin == false);
        }

        [OptionalField]
        private int? _UserPeopleId;
        public int? UserPeopleId
        {
            get { return _UserPeopleId; }
            set { _UserPeopleId = value; }
        }
        [OptionalField]
        private Person _User;
        public Person user
        {
            get
            {
                if (_User == null && UserPeopleId.HasValue)
                    _User = DbUtil.Db.LoadPersonById(UserPeopleId.Value);
                return _User;
            }
        }
        public string LoginName
        {
            get
            {
                if (user != null)
                    return user.Name;
                return "anonymous";
            }
        }
        public string MeetingTime
        {
            get { return meeting().MeetingDate.ToString2("ddd, MMM d h:mm tt"); }
        }

        public bool? testing { get; set; }
        public string qtesting
        {
            get { return testing == true ? "?testing=true" : ""; }
        }
        public bool IsCreateAccount()
        {
            if (div == null)
                return org.RegistrationTypeId == (int)Organization.RegistrationEnum.CreateAccount;
            return false;
        }
        public static IQueryable<Organization> UserSelectClasses(int? divid)
        {
            var a = new int[] 
            { 
                (int)Organization.RegistrationEnum.UserSelectsOrganization,
                (int)Organization.RegistrationEnum.ComputeOrganizationByAge
            };
            var q = from o in DbUtil.Db.Organizations
                    where o.DivOrgs.Any(od => od.DivId == divid)
                    where o.OrganizationStatusId == (int)CmsData.Organization.OrgStatusCode.Active
                    where a.Contains(o.RegistrationTypeId.Value)
                    where o.OnLineCatalogSort != null || o.RegistrationTypeId == (int)Organization.RegistrationEnum.ComputeOrganizationByAge
                    select o;
            return q;
        }
        public bool IsEnded()
        {
            if (div != null)
                return UserSelectClasses(div.Id).Count() == 0;
            else
                return org.ClassFilled == true;
        }
        public IEnumerable<SelectListItem> Classes()
        {
            return Classes(divid, classid ?? 0);
        }
        public static IEnumerable<SelectListItem> Classes(int? divid, int id)
        {
            var q = from o in UserSelectClasses(divid)
                    let hasroom = (o.ClassFilled ?? false) == false && ((o.Limit ?? 0) == 0 || o.Limit > o.MemberCount)
                    where hasroom
                    orderby o.OnLineCatalogSort, o.OrganizationName
                    select new SelectListItem
                    {
                        Value = o.OrganizationId.ToString(),
                        Text = ClassName(o),
                        Selected = o.OrganizationId == id,
                    };
            var list = q.ToList();
            if (list.Count == 1)
                return list;
            list.Insert(0, new SelectListItem { Text = "Registration Options", Value = "0" });
            return list;
        }
        public IEnumerable<String> FilledClasses()
        {
            var q = from o in UserSelectClasses(divid)
                    let hasroom = (o.ClassFilled ?? false) == false && ((o.Limit ?? 0) == 0 || o.Limit > o.MemberCount)
                    where !hasroom
                    orderby o.OnLineCatalogSort, o.OrganizationName
                    select ClassName(o);
            return q;
        }
        private static string ClassName(CmsData.Organization o)
        {
            var lead = o.LeaderName;
            if (lead.HasValue())
                lead = ": " + lead;
            var loc = o.Location;
            if (loc.HasValue())
                loc = " ({0})".Fmt(loc);
            var dt1 = o.FirstMeetingDate;
            var dt2 = o.LastMeetingDate;
            var dt = "";
            if (dt1.HasValue && dt2.HasValue)
                dt = ", {0:MMM d}-{1:MMM d}".Fmt(dt1, dt2);
            else if (dt1.HasValue)
                dt = ", {0:MMM d}".Fmt(dt1);

            return o.OrganizationName + lead + dt + loc;
        }
        public bool UserSelectsOrganization()
        {
            return divid != null && DbUtil.Db.Organizations.Any(o => o.DivOrgs.Any(di => di.DivId == divid) &&
                    o.RegistrationTypeId == (int)CmsData.Organization.RegistrationEnum.UserSelectsOrganization);
        }
        public bool OnlyOneAllowed()
        {
            if (org != null)
                return org.AllowOnlyOne == true
                    || org.AskTickets == true
                    || org.RegistrationTypeId == (int)Organization.RegistrationEnum.ChooseSlot
                    || org.RegistrationTypeId == (int)Organization.RegistrationEnum.CreateAccount
                    || org.GiveOrgMembAccess == true;

            var q = from o in DbUtil.Db.Organizations
                    where o.DivOrgs.Any(di => di.DivId == divid)
                    where o.AllowOnlyOne == true || o.AskTickets == true
                    select o;
            return q.Count() > 0;
        }
        public bool ChoosingSlots()
        {
            if (org != null)
                return org.RegistrationTypeId == (int)Organization.RegistrationEnum.ChooseSlot;
            return false;
        }
        public string Header
        {
            get
            {
                if (div != null)
                    return div.Name;
                else
                    return org != null ? org.OrganizationName : "no organization";
            }
        }
        public string Instructions
        {
            get
            {
                if (org != null)
                    return Util.PickFirst(org.Instructions, div != null ? div.Instructions : "");
                if (div != null)
                    return div.Instructions;
                return "";
            }
        }
        public string Terms
        {
            get
            {
                if (org != null)
                    return Util.PickFirst(org.Terms, div != null ? div.Terms : "");
                if (div != null)
                    return div.Terms;
                return "";
            }
        }

        private IList<OnlineRegPersonModel> list = new List<OnlineRegPersonModel>();
        public IList<OnlineRegPersonModel> List
        {
            get { return list; }
            set { list = value; }
        }
        public OnlineRegPersonModel last
        {
            get
            {
                if (list.Count > 0)
                    return list[list.Count - 1];
                return null;
            }
        }
        public class FamilyMember
        {
            public int PeopleId { get; set; }
            public string Name { get; set; }
            public int? Age { get; set; }
        }
        public IEnumerable<FamilyMember> FamilyMembers()
        {
            var family = user.Family.People.Select(p => new { p.PeopleId, p.Name2, p.Age, p.Name });
            var q = from m in family
                    join r in list on m.PeopleId equals r.PeopleId into j
                    from r in j.DefaultIfEmpty()
                    where r == null
                    orderby m.PeopleId == user.Family.HeadOfHouseholdId ? 1 :
                            m.PeopleId == user.Family.HeadOfHouseholdSpouseId ? 2 :
                            3, m.Age descending, m.Name2
                    select new FamilyMember
                    {
                        PeopleId = m.PeopleId,
                        Name = m.Name,
                        Age = m.Age
                    };
            return q;
        }
        public decimal Amount()
        {
            var amt = List.Sum(p => p.AmountToPay());
            if (org == null)
                return amt;
            if (org.MaximumFee > 0 && amt > org.MaximumFee)
                amt = org.MaximumFee.Value;
            return amt;
        }
        public decimal TotalAmount()
        {
            return List.Sum(p => p.TotalAmount());
        }
        public decimal TotalAmountDue()
        {
            return List.Sum(p => p.AmountDue());
        }
        public string NameOnAccount
        {
            get
            {
                var p = List[0];
                if (p.org.AskParents == true)
                    return p.fname.HasValue() ? p.fname : p.mname;
                return p.first + " " + p.last;
            }
        }
        private CmsData.Meeting _meeting;
        public CmsData.Meeting meeting()
        {
            if (_meeting == null)
            {
                var q = from m in DbUtil.Db.Meetings
                        where m.Organization.OrganizationId == orgid
                        where m.MeetingDate > Util.Now.AddHours(-12)
                        orderby m.MeetingDate
                        select m;
                _meeting = q.FirstOrDefault();
            }
            return _meeting;
        }
        public List<SelectListItem> ShirtSizes()
        {
            var q = from ss in DbUtil.Db.ShirtSizes
                    orderby ss.Id
                    select new SelectListItem
                    {
                        Value = ss.Code,
                        Text = ss.Description
                    };
            var list = q.ToList();
            list.Insert(0, new SelectListItem { Value = "0", Text = "(not specified)" });
            if (org != null && org.AllowLastYearShirt == true)
                list.Add(new SelectListItem { Value = "lastyear", Text = "Use shirt from last year" });
            return list;
        }

        public void CreateAccount()
        {
            var p = List[0];
            if (p.IsNew)
                p.AddPerson(null, p.org.EntryPointId ?? 0);
            var person = List[0].person;
            var uname = MembershipService.FetchUsername(DbUtil.Db, person.PreferredName, person.LastName);
            var pword = MembershipService.FetchPassword(DbUtil.Db);
            var user = MembershipService.CreateUser(person.PeopleId, uname, pword);

            var gobackurl = HttpContext.Current.Session["gobackurl"] as string;
            Content c = null;
            if (gobackurl.HasValue())
                c = DbUtil.Content("CreateAccountRegistration");
            else
                c = DbUtil.Content("CreateAccountConfirmation");
            if (c == null)
                c = new Content();

            var message = Util.PickFirst(c.Body,
                    @"Hi {name},
<p>We have created an account you for in our church database.</p>
<p>This will make it easier for you to do online registrations.
Just use this account next time you register online.</p>
<p>And this will allow you to help us maintain your correct address, email and phone numbers.
Just login to {host} and you will be taken to your record where you can make corrections if needed.</p>
<p>The following are the credentials you can use. Both the username and password are system generated.
</p>
<table>
<tr><td>Username:</td><td><b>{username}</b></td></tr>
<tr><td>Password:</td><td><b>{password}</b></td></tr>
</table>
<p>Thank You</p>
");
            message = message
                .Replace("{name}", person.Name)
                .Replace("{username}", uname)
                .Replace("{password}", pword)
                .Replace("{gobackurl}", gobackurl)
                .Replace("{host}", Util.CmsHost);

            var smtp = Util.Smtp();
            Util.Email(smtp, DbUtil.AdminMail,
                uname, person.EmailAddress, "New account for " + DbUtil.Db.Host, message);
        }

        public void EnrollAndConfirm(string TransactionID)
        {
            var elist = new List<string>();
            if (UserPeopleId.HasValue)
                elist.Add(user.EmailAddress);
            var participants = new StringBuilder();
            var pids = new List<TransactionInfo.PeopleInfo>();
            for (var i = 0; i < List.Count; i++)
            {
                var p = List[i];
                if (p.IsNew)
                {
                    Person uperson = null;
                    switch (p.whatfamily)
                    {
                        case 1:
                            uperson = DbUtil.Db.LoadPersonById(UserPeopleId.Value);
                            break;
                        case 2:
                            uperson = List[i - 1].person;
                            break;
                    }
                    p.AddPerson(uperson, p.org.EntryPointId ?? 0);
                }

                if (!elist.Contains(p.email))
                    elist.Add(p.email);

                if (!p.IsNew)
                    if (p.person.EmailAddress.HasValue())
                        if (!elist.Contains(p.person.EmailAddress))
                            elist.Add(p.person.EmailAddress);
                participants.Append(p.ToString());
                pids.Add(new TransactionInfo.PeopleInfo
                {
                    name = p.person.Name,
                    pid = p.person.PeopleId,
                    amt = p.AmountToPay() + p.AmountDue()
                });
            }
            var p0 = List[0].person;
            var emails = string.Join(",", elist.ToArray());
            string paylink = string.Empty;
            var amtdue = TotalAmountDue();
            var amtpaid = Amount();

            if (amtdue > 0)
            {
                var ti = new TransactionInfo
                {
                    //URL = URL,
                    Header = Header,
                    Name = NameOnAccount,
                    Address = p0.PrimaryAddress,
                    City = p0.PrimaryCity,
                    State = p0.PrimaryState,
                    Zip = p0.PrimaryZip,
                    Phone = Util.PickFirst(p0.HomePhone, p0.CellPhone).FmtFone(),
                    testing = testing ?? false,
                    AmountDue = amtdue,
                    AmountPaid = amtpaid,
                    Email = emails,
                    Participants = participants.ToString(),
                    people = pids.ToArray(),
                    orgid = orgid.Value,
                };
                var td = DbUtil.Db.GetDatum<TransactionInfo>(ti);
                var estr = HttpUtility.UrlEncode(Util.Encrypt(td.Id.ToString()));
                paylink = Util.ResolveServerUrl("/OnlineReg/PayDue?q=" + estr);
            }

            var details = new StringBuilder("<table>");
            for (var i = 0; i < List.Count; i++)
            {
                var p = List[i];
                var others = string.Join(",", pids.Where(po => po.pid != p.PeopleId).Select(po => po.name).ToArray());
                others += "(Total paid {0:c})".Fmt(amtdue);
                var om = p.Enroll(TransactionID, paylink, testing, others);
                details.AppendFormat(@"
<tr><td colspan='2'><hr/></td></tr>
<tr><th valign='top'>{0}</th><td>
{1}
</td></tr>", i + 1, p.PrepareSummaryText());

                om.RegisterEmail = p.email;
                if (p.org.GiveOrgMembAccess == true)
                {
                    CmsData.Group g = null;
                    if (org.GroupToJoin.HasValue())
                        g = CmsData.Group.LoadByName(org.GroupToJoin);

                    if (p.person.Users.Count() == 0)
                    {
                        p.IsNew = false;
                        CreateAccount();
                    }
                    foreach (var u in p.person.Users)
                    {
                        var list = u.Roles.ToList();
                        if (!list.Contains("Access"))
                            list.Add("Access");
                        if (!list.Contains("OrgMembersOnly"))
                            list.Add("OrgMembersOnly");
                        u.Roles = list.ToArray();
                        if (org.GroupToJoin.HasValue())
                        {
                            g.SetMember(u, true);
                            u.DefaultGroup = g.Name;
                        }
                    }
                    DbUtil.Db.SubmitChanges();
                }
                OnlineRegPersonModel.CheckNotifyDiffEmails(p.person,
                    p.org.EmailAddresses,
                    p.email,
                    p.org.OrganizationName,
                    p.org.PhoneNumber);
            }
            details.Append("\n</table>\n");
            DbUtil.Db.SubmitChanges();

            string DivisionName = null;
            if (div != null)
                DivisionName = div.Name;
            else if (org != null)
                DivisionName = org.DivisionName;

            string OrganizationName = null;
            if (div != null)
                OrganizationName = "";
            else if (org != null)
                OrganizationName = org.OrganizationName;
            if (!OrganizationName.HasValue())
                OrganizationName = DivisionName;

            string EmailSubject = null;
            if (div != null)
                EmailSubject = div.EmailSubject;
            else if (org != null)
                EmailSubject = org.EmailSubject;

            string EmailMessage = null;
            if (div != null)
                EmailMessage = div.EmailMessage;
            else if (org != null)
                EmailMessage = org.EmailMessage;

            string EmailAddresses = null;
            if (div != null)
                EmailAddresses = List[0].org.EmailAddresses;
            else if (org != null)
                EmailAddresses = org.EmailAddresses;

            var subject = Util.PickFirst(EmailSubject, "no subject");
            var message = Util.PickFirst(EmailMessage, "no message");
            message = message.Replace("{first}", p0.PreferredName);
            message = message.Replace("{tickets}", List[0].ntickets.ToString());
            message = message.Replace("{division}", DivisionName);
            message = message.Replace("{org}", OrganizationName);
            message = message.Replace("{cmshost}", Util.CmsHost);
            message = message.Replace("{details}", details.ToString());
            message = message.Replace("{paid}", amtpaid.ToString("c"));
            message = message.Replace("{participants}", details.ToString());
            if (amtdue > 0)
                message = message.Replace("{paylink}", "<a href='{0}'>Click this link to pay balance of {1:C}</a>.".Fmt(paylink, amtdue));
            else
                message = message.Replace("{paylink}", "You have a zero balance.");

            var smtp = Util.Smtp();
            Util.Email(smtp, EmailAddresses, emails, subject, message);
            foreach (var p in List)
                Util.Email(smtp, p.person.EmailAddress, p.org.EmailAddresses, "{0}".Fmt(Header),
@"{0} has registered for {1}<br/>Feepaid: {2:C}<br/>AmountDue: {3:C}
<pre>{4}</pre>"
               .Fmt(p.person.Name, Header, p.AmountToPay(), p.AmountDue(), p.PrepareSummaryText()));
        }
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("orgid: {0}<br/>\n", this.orgid);
            sb.AppendFormat("divid: {0}<br/>\n", this.divid);
            sb.AppendFormat("userid: {0}<br/>\n", this.UserPeopleId);
            foreach (var li in List)
            {
                sb.AppendFormat("--------------------------------\nList: {0}<br/>\n", li.index);
                sb.Append(li.ToString());
            }
            return sb.ToString();
        }

        public OnlineRegModel()
        {

        }
        protected OnlineRegModel(SerializationInfo si, StreamingContext context)
        {
            UserPeopleId = (int?)si.GetValue("UserPeopleId", typeof(int?));
            URL = si.GetString("URL");
            classid = (int?)si.GetValue("classid", typeof(int?));
            divid = (int?)si.GetValue("divid", typeof(int?));
            nologin = si.GetBoolean("nologin");
            orgid = (int?)si.GetValue("orgid", typeof(int?));
            testing = si.GetBoolean("testing");
            username = si.GetString("username");
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }
    }
    [Serializable]
    public class TransactionInfo
    {
        public string Header { get; set; }
        public string URL { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal AmountDue { get; set; }
        public string Participants { get; set; }
        public bool testing { get; set; }
        public int orgid { get; set; }
        public PeopleInfo[] people { get; set; }
        public class PeopleInfo
        {
            public int pid { get; set; }
            public string name { get; set; }
            public decimal amt { get; set; }
        }
    }
}
