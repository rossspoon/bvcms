using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CmsData;
using System.Text;
using UtilityExtensions;
using System.Text.RegularExpressions;
using System.Net.Mail;

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
            var amtpaid = Amount();

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
                var om = p.Enroll(ti.TransactionId, paylink, testing, others);
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
                        if (org.GroupToJoin.HasValue())
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
                if (p.CreatingAccount == true && (p.org.GiveOrgMembAccess ?? false) == false)
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
            message = message.Replace("{first}", p0.PreferredName);
            message = message.Replace("{name}", p0.Name);
            message = message.Replace("{tickets}", List[0].ntickets.ToString());
            message = message.Replace("{division}", DivisionName);
            message = message.Replace("{org}", OrganizationName);
            message = message.Replace("{location}", Location);
            message = message.Replace("{cmshost}", DbUtil.Db.CmsHost);
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
                ti.Fund = org.ContributionFund.FundName;
                var p = List[donor.Value].person;
                var desc = "{0}; {1}; {2}, {3} {4}".Fmt(p.Name, p.PrimaryAddress, p.PrimaryCity, p.PrimaryState, p.PrimaryZip);
                //int? pid = UserPeopleId;
                //if (!pid.HasValue)
                //{
                //    string first, last;
                //    Person.NameSplit(ti.Name, out first, out last);
                //    var pds = Db.FindPerson(first, last, null, ti.Emails, ti.Phone);
                //    if (pds.Count() == 1)
                //        pid = pds.Single().PeopleId;
                //}
                PostBundleModel.PostUnattendedContribution(ti.Donate.Value, p.PeopleId, org.DonationFundId, desc);
            	var ma = re.Match(message);
                if (ma.Success)
                {
		            var v = ma.Groups["text"].Value;
                    message = re.Replace(message, v);
                }
                message = message.Replace("{donation}", ti.Donate.ToString2("N2"));
                // send donation confirmations
                Db.Email(notify.FromEmail, NotifyIds, subject + "-donation", "${0:N2} donation received from {1}".Fmt(ti.Donate, ti.Name));
            }
            else
                message = re.Replace(message, "");

            // send confirmations
            Db.Email(notify.FromEmail, p0, elist, 
                subject, message, false);
            // notify the staff
            foreach (var p in List)
            {
                Db.Email(p.person.FromEmail, 
                    Db.StaffPeopleForOrg(p.org.OrganizationId),
                    "{0}".Fmt(Header),
@"{0} has registered for {1}<br/>Feepaid: {2:C}<br/>AmountDue: {3:C}<br/>
<pre>{4}</pre>".Fmt(p.person.Name, Header, p.AmountToPay(), p.AmountDue(), p.PrepareSummaryText()));
            }
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
            var q = from o in DbUtil.Db.Organizations
                    where o.DivOrgs.Any(dd => dd.DivId == divid)
                    where o.RegistrationTypeId != (int)Organization.RegistrationEnum.None
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