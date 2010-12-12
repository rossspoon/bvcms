using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CmsData;
using System.Text;
using UtilityExtensions;
using System.Text.RegularExpressions;

namespace CmsWeb.Models
{
    public partial class OnlineRegModel
    {
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
                            if (i > 0)
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
                        p.CreateAccount();
                    }
                    foreach (var u in p.person.Users)
                    {
                        var list = u.Roles.ToList();
                        if (!list.Contains("Access"))
                            list.Add("Access");
                        if (!list.Contains("OrgMembersOnly"))
                            list.Add("OrgMembersOnly");
                        u.SetRoles(DbUtil.Db, list.ToArray());
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
                if (p.CreatingAccount == true && p.org.GiveOrgMembAccess == false)
                    p.CreateAccount();
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
        public void UseCoupon(string TransactionID)
        {
            string matchcoupon = @"Coupon\((?<coupon>.*)\)";
            if (Regex.IsMatch(TransactionID, matchcoupon, RegexOptions.IgnoreCase))
            {
                var coupon = Regex.Match(TransactionID, matchcoupon, RegexOptions.IgnoreCase)
                        .Groups["coupon"].Value.Replace(" ", "");
                if (coupon != "Admin")
                {
                    var c = DbUtil.Db.Coupons.Single(cp => cp.Id == coupon);
                    c.RegAmount = Amount();
                    c.Used = DateTime.Now;
                    c.PeopleId = List[0].PeopleId.Value;
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
                    ManageSubsModel.StaffEmail(divid.Value), 
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