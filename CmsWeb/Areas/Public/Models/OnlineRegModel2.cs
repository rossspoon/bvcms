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

namespace CmsWeb.Models
{
    [Serializable]
    public class OnlineRegModel2
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
        public bool IsEnded()
        {
            if (div != null)
                return OnlineRegPersonModel2.UserSelectClasses(div.Id).Count() == 0;
            else
                return org.ClassFilled == true;
        }
        public bool OnlyOneAllowed()
        {
            if (org != null)
                return org.AllowOnlyOne == true
                    || org.AskTickets == true
                    || org.RegistrationTypeId == (int)Organization.RegistrationEnum.ChooseSlot
                    || org.RegistrationTypeId == (int)Organization.RegistrationEnum.CreateAccount;
            var q = from o in DbUtil.Db.Organizations
                    where o.DivisionId == divid
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

        private IList<OnlineRegPersonModel2> list = new List<OnlineRegPersonModel2>();
        public IList<OnlineRegPersonModel2> List
        {
            get { return list; }
            set { list = value; }
        }
        public OnlineRegPersonModel2 last
        {
            get
            {
                if (list.Count > 0)
                    return list[list.Count - 1];
                return null;
            }
        }
        public string username { get; set; }
        public string password { get; set; }

        public int? UserPeopleId { get; set; }
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
        public string MeetingTime
        {
            get { return meeting().MeetingDate.ToString2("ddd, MMM d h:mm tt"); }
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
            var uname = MembershipService.FetchUsername(person.PreferredName, person.LastName);
            var pword = MembershipService.FetchPassword();
            var user = MembershipService.CreateUser(person.PeopleId, uname, pword);
            var smtp = Util.Smtp();
            DbUtil.Email(smtp, DbUtil.Settings("AdminMail", DbUtil.SystemEmailAddress),
                uname, person.EmailAddress,
                    "New account for " + Util.Host,
                    @"Hi {1},
<p>You now have an account in our church database.</p>
<p>This will make it easier for you to do online registrations.
Just use this account next time you register online.</p>
<p>And this will allow you to help us maintain your correct address, email and phone numbers.
Just login to {0} and you will be taken to your record where you can make corrections if needed.</p>
<p>The following are the credentials you can use. Both the username and password are system generated.
</p>
<table>
<tr><td>Username:</td><td><b>{2}</b></td></tr>
<tr><td>Password:</td><td><b>{3}</b></td></tr>
</table>
<p>Thank You</p>
".Fmt(Util.CmsHost, person.Name, uname, pword));
        }
        public void EnrollAndConfirm(string TransactionID)
        {
            var elist = new List<string>();
            var participants = new StringBuilder();
            var pids = new List<TransactionInfo.PeopleInfo>();
            var p0 = List[0];
            for (var i = 0; i < List.Count; i++)
            {
                var p = List[i];
                if (p.IsNew)
                    p.AddPerson(i == 0 ? null : p0.person, p.org.EntryPointId ?? 0);

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
                    Address = p0.address,
                    City = p0.city,
                    State = p0.state,
                    Zip = p0.zip,
                    Phone = p0.phone.FmtFone(),
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
                var om = p.Enroll(TransactionID, paylink, testing, others);
                details.AppendFormat(@"
<tr><td colspan='2'><hr/></td></tr>
<tr><th valign='top'>{0}</th><td>
{1}
</td></tr>", i + 1, p.PrepareSummaryText());

                om.RegisterEmail = p.email;
                OnlineRegPersonModel2.CheckNotifyDiffEmails(p.person,
                    p.org.EmailAddresses,
                    p.email,
                    p.org.OrganizationName,
                    p.org.PhoneNumber);
            }
            details.Append("\n</table>\n");
            DbUtil.Db.SubmitChanges();

            var o = org;
            if (o == null)
                o = p0.org;
            var subject = Util.PickFirst(o.EmailSubject, o.Division != null ? o.Division.EmailSubject : null, "no subject");
            var message = Util.PickFirst(o.EmailMessage, o.Division != null ? o.Division.EmailMessage : null, "no message");
            message = message.Replace("{first}", p0.first);
            message = message.Replace("{tickets}", p0.ntickets.ToString());
            message = message.Replace("{division}", o.DivisionName);
            message = message.Replace("{org}", o.OrganizationName);
            message = message.Replace("{cmshost}", Util.CmsHost);
            message = message.Replace("{details}", details.ToString());
            message = message.Replace("{paid}", amtpaid.ToString("c"));
            message = message.Replace("{participants}", details.ToString());
            if (amtdue > 0)
                message = message.Replace("{paylink}", "<a href='{0}'>Click this link to pay balance of {1:C}</a>.".Fmt(paylink, amtdue));
            else
                message = message.Replace("{paylink}", "You have a zero balance.");

            var smtp = Util.Smtp();
            DbUtil.Email2(smtp, o.EmailAddresses, emails, subject, message);
            DbUtil.Email2(smtp, emails, o.EmailAddresses,
                "{0}".Fmt(Header),
                @"{0} has registered {1} participant for {2}<br/>Feepaid: {3:C}<br/>AmountDue: {4:C}
<pre>{5}</pre>"
                .Fmt(NameOnAccount, List.Count, Header, amtpaid, amtdue, details.ToString()));
        }
    }
}
