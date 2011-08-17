using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CmsData;
using System.Text;
using UtilityExtensions;
using System.Text.RegularExpressions;
using System.Net.Mail;
using CmsData.Codes;

namespace CmsWeb.Models
{
    public partial class OnlineRegModel
    {
        public void EnrollAndConfirm()
        {
            var Db = DbUtil.Db;
            var ti = Transaction;
            // make a list of email addresses
            var elist = new List<MailAddress>();
            if (UserPeopleId.HasValue)
            {
                if (user.SendEmailAddress1 ?? true)
                    Util.AddGoodAddress(elist, user.FromEmail);
                if (user.SendEmailAddress2 ?? false)
                    Util.AddGoodAddress(elist, user.FromEmail2);
            }
            var participants = new StringBuilder();
            for (var i = 0; i < List.Count; i++)
            {
                var p = List[i];
                if (p.IsNew)
                {
                    Person uperson = null;
                    switch (p.whatfamily)
                    {
                        case 1:
                            uperson = Db.LoadPersonById(UserPeopleId.Value);
                            break;
                        case 2:
                            if (i > 0)
                                uperson = List[i - 1].person;
                            break;
                    }
                    p.AddPerson(uperson, p.org.EntryPointId ?? 0);
                }

                Util.AddGoodAddress(elist, p.fromemail);
                participants.Append(p.ToString());
            }
            var p0 = List[0].person;
            if (this.user != null)
                p0 = user;

            //var emails = string.Join(",", elist.ToArray());
            string paylink = string.Empty;
            var amtdue = TotalAmountDue();
            var amtpaid = ti.Amt ?? 0;

            var pids2 = new List<TransactionPerson>();
            foreach (var p in List)
                pids2.Add(new TransactionPerson
                {
                    PeopleId = p.PeopleId.Value,
                    Amt = p.AmountToPay() + p.AmountDue(),
                    OrgId = orgid,
                });

            ti.Emails = Util.EmailAddressListToString(elist);
            ti.Participants = participants.ToString();
            ti.TransactionDate = DateTime.Now;
            ti.TransactionPeople.AddRange(pids2);

            var estr = HttpUtility.UrlEncode(Util.Encrypt(ti.Id.ToString()));
            paylink = Util.ResolveServerUrl("/OnlineReg/PayAmtDue?q=" + estr);

            var pids = pids2.Select(pp => pp.PeopleId);

            var details = new StringBuilder("<table>");
            for (var i = 0; i < List.Count; i++)
            {
                var p = List[i];

                var q = from pp in Db.People
                        where pids.Contains(pp.PeopleId)
                        where pp.PeopleId != p.PeopleId
                        select pp.Name;
                var others = string.Join(",", q.ToArray());

                others += "(Total due {0:c})".Fmt(amtdue);
                var om = p.Enroll(ti, paylink, testing, others);
                details.AppendFormat(@"
<tr><td colspan='2'><hr/></td></tr>
<tr><th valign='top'>{0}</th><td>
{1}
</td></tr>", i + 1, p.PrepareSummaryText(ti));

                om.RegisterEmail = p.email;
                if (p.setting.GiveOrgMembAccess == true)
                {
                    CmsData.Group g = null;
                    if (p.setting.GroupToJoin.HasValue())
                        g = CmsData.Group.LoadByName(p.setting.GroupToJoin);

                    if (p.person.Users.Count() == 0)
                    {
                        p.IsNew = false;
                        p.CreateAccount();
                    }
                    foreach (var u in p.person.Users)
                    {
                        var list = u.Roles.ToList();
                        if (!list.Contains("Access"))
                            list.Add("Access");
                        if (!list.Contains("OrgMembersOnly"))
                            list.Add("OrgMembersOnly");
                        u.SetRoles(Db, list.ToArray(), false);
                        if (p.setting.GroupToJoin.HasValue())
                        {
                            g.SetMember(u, true);
                            u.DefaultGroup = g.Name;
                        }
                    }
                    Db.SubmitChanges();
                }
                OnlineRegPersonModel.CheckNotifyDiffEmails(p.person,
                    Db.StaffEmailForOrg(p.org.OrganizationId),
                    p.fromemail,
                    p.org.OrganizationName,
                    p.org.PhoneNumber);
                if (p.CreatingAccount == true && p.setting.GiveOrgMembAccess == false)
                    p.CreateAccount();
            }
            details.Append("\n</table>\n");
            Db.SubmitChanges();

            string DivisionName = null;
            if (div != null)
                DivisionName = div.Name;
            else if (org != null)
                DivisionName = org.DivisionName;

            string OrganizationName = null;
            if (org != null)
                OrganizationName = org.OrganizationName;
            else if (div != null)
                OrganizationName = DivisionName;

            if (!OrganizationName.HasValue())
                OrganizationName = DivisionName;

            string EmailSubject = null;
            string EmailMessage = null;

            if (org != null && settings[orgid.Value].Body.ToString().HasValue())
            {
                var os = settings[orgid.Value];
                EmailSubject = Util.PickFirst(os.Subject, org.EmailSubject, "no subject");
                EmailMessage = Util.PickFirst(os.Body.ToString(), org.EmailMessage, "no body");
            }
            else if (div != null)
            {
                EmailSubject = div.EmailSubject;
                EmailMessage = div.EmailMessage;
            }

            List<Person> NotifyIds = null;
            if (div != null)
                NotifyIds = Db.StaffPeopleForDiv(div.Id);
            else if (org != null)
                NotifyIds = Db.StaffPeopleForOrg(org.OrganizationId);
            var notify = NotifyIds[0];

            string Location = null;
            if (div != null)
                Location = List[0].org.Location;
            else if (org != null)
                Location = org.Location;

            var subject = Util.PickFirst(EmailSubject, "no subject");
            var message = Util.PickFirst(EmailMessage, "no message");

            message = MessageReplacements(p0, DivisionName, OrganizationName, Location, message);

            message = message.Replace("{tickets}", List[0].ntickets.ToString());
            message = message.Replace("{details}", details.ToString());
            message = message.Replace("{paid}", amtpaid.ToString("c"));
            message = message.Replace("{participants}", details.ToString());
            if (amtdue > 0)
                message = message.Replace("{paylink}", "<a href='{0}'>Click this link to pay balance of {1:C}</a>.".Fmt(paylink, amtdue));
            else
                message = message.Replace("{paylink}", "You have a zero balance.");

            var re = new Regex(@"\{donation(?<text>.*)donation\}", RegexOptions.Singleline | RegexOptions.Multiline);
            if (ti.Donate > 0)
            {
                var p = List[donor.Value];
                ti.Fund = p.setting.DonationFund();
                var desc = "{0}; {1}; {2}, {3} {4}".Fmt(
                    p.person.Name, 
                    p.person.PrimaryAddress, 
                    p.person.PrimaryCity, 
                    p.person.PrimaryState, 
                    p.person.PrimaryZip);
                PostBundleModel.PostUnattendedContribution(
                    ti.Donate.Value, 
                    p.PeopleId, 
                    p.setting.DonationFundId, 
                    desc);
            	var ma = re.Match(message);
                if (ma.Success)
                {
		            var v = ma.Groups["text"].Value;
                    message = re.Replace(message, v);
                }
                message = message.Replace("{donation}", ti.Donate.ToString2("N2"));
                // send donation confirmations
                Db.Email(notify.FromEmail, NotifyIds, subject + "-donation", 
                    "${0:N2} donation received from {1}".Fmt(ti.Donate, ti.Name));
            }
            else
                message = re.Replace(message, "");

            // send confirmations
            Db.Email(notify.FromEmail, p0, elist, 
                subject, message, false);
            // notify the staff
            foreach (var p in List)
            {
                Db.Email(Util.PickFirst(p.person.FromEmail, notify.FromEmail), 
                    Db.StaffPeopleForOrg(p.org.OrganizationId), Header,
@"{0} has registered for {1}<br/>Feepaid: {2:C}<br/>AmountDue: {3:C}<br/>
<pre>{4}</pre>".Fmt(p.person.Name, Header, amtpaid, p.AmountDue(), p.PrepareSummaryText(ti)));
            }
        }
        public static string MessageReplacements(Person p, string DivisionName, string OrganizationName, string Location, string message)
        {
             message = message.Replace("{first}", p.PreferredName);
             message = message.Replace("{name}", p.Name);
             message = message.Replace("{division}", DivisionName);
             message = message.Replace("{org}", OrganizationName);
             message = message.Replace("{location}", Location);
             message = message.Replace("{cmshost}", DbUtil.Db.CmsHost);
            return message;
        }
        public void UseCoupon(string TransactionID)
        {
            string matchcoupon = @"Coupon\((?<coupon>[^)]*)\)";
            if (Regex.IsMatch(TransactionID, matchcoupon, RegexOptions.IgnoreCase))
            {
                var match = Regex.Match(TransactionID, matchcoupon, RegexOptions.IgnoreCase);
                var coup = match.Groups["coupon"];
                var coupon = "";
                if (coup != null)
                    coupon = coup.Value.Replace(" ", "");
                if (coupon != "Admin")
                {
                    var c = DbUtil.Db.Coupons.SingleOrDefault(cp => cp.Id == coupon);
                    if (c != null)
                    {
                        c.RegAmount = Amount();
                        c.Used = DateTime.Now;
                        c.PeopleId = List[0].PeopleId;
                    }
                }
            }
        }
        public void ConfirmManageSubscriptions()
        {
            var p = List[0];
            if (p.IsNew)
                p.AddPerson(null, EntryPointForDiv());
            if (p.CreatingAccount == true)
                p.CreateAccount();
            p.SendOneTimeLink(
                DbUtil.Db.StaffPeopleForDiv(divid.Value).First().FromEmail,
                Util.ServerLink("/OnlineReg/ManageSubscriptions/"));
        }
        public int EntryPointForDiv()
        {
            var q = from o in GetOrgsInDiv()
                    where o.RegistrationTypeId != RegistrationEnum.None
                    where o.EntryPointId > 0
                    select o.EntryPointId;
            return q.FirstOrDefault() ?? 0;
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
    }
}