using System;
using System.Linq;
using System.Web.Mvc;
using CmsData;
using CmsWeb.Models;
using UtilityExtensions;
using CmsWeb.Areas.Manage.Controllers;
using System.Text;
using System.Collections.Generic;
using CmsData.Codes;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace CmsWeb.Areas.OnlineReg.Controllers
{
    public partial class OnlineRegController : CmsController
    {
        public ActionResult PickSlots(int? id)
        {
            if (!id.HasValue)
                return View("Unknown");

            var ed = DbUtil.Db.ExtraDatas.SingleOrDefault(e => e.Id == id);
            if (ed == null)
                return Content("no pending confirmation found");
            var m = Util.DeSerialize<OnlineRegModel>(ed.Data);
            m.ParseSettings();
            return View(new SlotModel(m.List[0].PeopleId.Value, m.orgid.Value));
        }
        [HttpPost]
        public ActionResult ToggleSlot(int id, int oid, string slot, bool ck)
        {
            var m = new SlotModel(id, oid);
            var om = m.org.OrganizationMembers.SingleOrDefault(mm => mm.PeopleId == id);
            if (om == null)
                om = OrganizationMember.InsertOrgMembers(DbUtil.Db,
                    oid, id, 220, Util.Now, null, false);
            if (ck)
                om.AddToGroup(DbUtil.Db, slot);
            else
                om.RemoveFromGroup(DbUtil.Db, slot);
            DbUtil.DbDispose();
            m = new SlotModel(id, oid);
            var slotinfo = m.NewSlot(slot);
            if (slotinfo.slot == null)
                return new EmptyResult();
            ViewData["returnval"] = slotinfo.status;
            return View("PickSlot", slotinfo);
        }
        public ActionResult ManageSubscriptions(string id)
        {
            if (!id.HasValue())
                return Content("bad link");
            ManageSubsModel m = null;
            var td = TempData["ms"];
            if (td != null)
                m = new ManageSubsModel(td.ToInt(), id.ToInt());
            else
            {
                var guid = id.ToGuid();
                if (guid == null)
                    return Content("invalid link");
                var ot = DbUtil.Db.OneTimeLinks.SingleOrDefault(oo => oo.Id == guid.Value);
                if (ot == null)
                    return Content("invalid link");
                if (ot.Used)
                    return Content("link used");
                if (ot.Expires.HasValue && ot.Expires < DateTime.Now)
                    return Content("link expired");
                var a = ot.Querystring.Split(',');
                m = new ManageSubsModel(a[1].ToInt(), a[0].ToInt());
                id = a[0];
                ot.Used = true;
                DbUtil.Db.SubmitChanges();
            }
            SetHeaders(id.ToInt());
            DbUtil.LogActivity("Manage Subs: {0} ({1})".Fmt(m.Description(), m.person.Name));
            return View(m);
        }
        public ActionResult ManagePledge(string id)
        {
            if (!id.HasValue())
                return Content("bad link");
            ManagePledgesModel m = null;
            var td = TempData["mp"];
            if (td != null)
                m = new ManagePledgesModel(td.ToInt(), id.ToInt());
            else
            {
                var guid = id.ToGuid();
                if (guid == null)
                    return Content("invalid link");
                var ot = DbUtil.Db.OneTimeLinks.SingleOrDefault(oo => oo.Id == guid.Value);
                if (ot == null)
                    return Content("invalid link");
                if (ot.Used)
                    return Content("link used");
                if (ot.Expires.HasValue && ot.Expires < DateTime.Now)
                    return Content("link expired");
                var a = ot.Querystring.Split(',');
                m = new ManagePledgesModel(a[1].ToInt(), a[0].ToInt());
                ot.Used = true;
                DbUtil.Db.SubmitChanges();
            }
            SetHeaders(m.orgid);
            DbUtil.LogActivity("Manage Pledge: {0} ({1})".Fmt(m.Organization.OrganizationName, m.person.Name));
            return View(m);
        }
        [HttpGet]
        public ActionResult ManageGiving(string id, bool? testing)
        {
            if (!id.HasValue())
                return Content("bad link");
            ManageGivingModel m = null;
            var td = TempData["mg"];
            if (td != null)
                m = new ManageGivingModel(td.ToInt(), id.ToInt());
            else
            {
                var guid = id.ToGuid();
                if (guid == null)
                    return Content("invalid link");
                var ot = DbUtil.Db.OneTimeLinks.SingleOrDefault(oo => oo.Id == guid.Value);
                if (ot == null)
                    return Content("invalid link");
#if DEBUG
#else
                if (ot.Used)
                    return Content("link used");
#endif
                if (ot.Expires.HasValue && ot.Expires < DateTime.Now)
                    return Content("link expired");
                var a = ot.Querystring.Split(',');
                m = new ManageGivingModel(a[1].ToInt(), a[0].ToInt());
                ot.Used = true;
                DbUtil.Db.SubmitChanges();
            }
            if (!m.testing)
                m.testing = testing ?? false;
            SetHeaders(m.orgid);
            DbUtil.LogActivity("Manage Giving: {0} ({1})".Fmt(m.Organization.OrganizationName, m.person.Name));
            return View(m);
        }
        [HttpPost]
        public ActionResult ManageGiving(ManageGivingModel m)
        {
            SetHeaders(m.orgid);
            m.ValidateModel(ModelState);
            if (!ModelState.IsValid)
                return View(m);
            try
            {
                var gateway = OnlineRegModel.GetTransactionGateway();
                if (gateway == "AuthorizeNet")
                {
                    var au = new AuthorizeNet(DbUtil.Db, m.testing);
                    au.AddUpdateCustomerProfile(m.pid,
                        m.SemiEvery,
                        m.Day1,
                        m.Day2,
                        m.EveryN,
                        m.Period,
                        m.StartWhen,
                        m.StartWhen,
                        m.Type,
                        m.Cardnumber,
                        m.Expires,
                        m.Cardcode,
                        m.Routing,
                        m.Account,
                        m.testing);
                }
                else if (gateway == "SagePayments")
                {
                    var sg = new CmsData.SagePayments(DbUtil.Db, m.testing);
                    sg.storeVault(m.pid, 
                        m.SemiEvery,
                        m.Day1,
                        m.Day2,
                        m.EveryN,
                        m.Period,
                        m.StartWhen,
                        m.StartWhen,
                        m.Type,
                        m.Cardnumber,
                        m.Expires,
                        m.Cardcode,
                        m.Routing,
                        m.Account,
                        m.testing);
                }
                else
                    throw new Exception("ServiceU not supported");

                var q = from ra in DbUtil.Db.RecurringAmounts
                        where ra.PeopleId == m.pid
                        select ra;
                DbUtil.Db.RecurringAmounts.DeleteAllOnSubmit(q);
                DbUtil.Db.SubmitChanges();
                foreach (var c in m.FundItemsChosen())
                {
                    var ra = new RecurringAmount
                    {
                        PeopleId = m.pid,
                        FundId = c.fundid,
                        Amt = c.amt
                    };
                    DbUtil.Db.RecurringAmounts.InsertOnSubmit(ra);
                }
                DbUtil.Db.SubmitChanges();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("form", ex.Message);
            }
            if (!ModelState.IsValid)
                return View(m);
            TempData["managegiving"] = m;
            return Redirect("ConfirmRecurringGiving");
        }
        public ActionResult ConfirmRecurringGiving()
        {
            var m = TempData["managegiving"] as ManageGivingModel;

            var staff = DbUtil.Db.StaffPeopleForOrg(m.orgid)[0];
            var text = m.setting.Body.Replace("{church}", DbUtil.Db.Setting("NameOfChurch", "church"));
            text = text.Replace("{amt}", m.Total().ToString("N2"));
            text = text.Replace("{name}", m.person.Name);
            text = text.Replace("{account}", "");
            text = text.Replace("{email}", m.person.EmailAddress);
            text = text.Replace("{phone}", m.person.HomePhone.FmtFone());
            text = text.Replace("{contact}", staff.Name);
            text = text.Replace("{contactemail}", staff.EmailAddress);
            text = text.Replace("{contactphone}", m.Organization.PhoneNumber.FmtFone());
            var re = new Regex(@"(?<b>.*?)<!--ITEM\sROW\sSTART-->.(?<row>.*?)\s*<!--ITEM\sROW\sEND-->(?<e>.*)", RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace);
            var match = re.Match(text);
            var b = match.Groups["b"].Value;
            var row = match.Groups["row"].Value.Replace("{funditem}", "{0}").Replace("{itemamt}", "{1:N2}");
            var e = match.Groups["e"].Value;
            var sb = new StringBuilder(b);
            foreach (var g in m.FundItemsChosen())
                sb.AppendFormat(row, g.desc, g.amt);

            Util.SendMsg(Util.SysFromEmail, Util.Host, Util.TryGetMailAddress(DbUtil.Db.StaffEmailForOrg(m.orgid)),
                m.setting.Subject, sb.ToString(),
                Util.EmailAddressListFromString(m.person.FromEmail), 0, m.pid);
            Util.SendMsg(Util.SysFromEmail, Util.Host, Util.TryGetMailAddress(m.person.FromEmail),
                "Managed Giving",
                "Managed giving for {0} ({1})".Fmt(m.person.Name, m.pid),
                Util.EmailAddressListFromString(DbUtil.Db.StaffEmailForOrg(m.orgid)),
                0, m.pid);

            SetHeaders(m.orgid);
            return View(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ConfirmSlots(int id, int orgid)
        {
            var m = new SlotModel(id, orgid);
            var slots = string.Join("<br />\n", m.MySlots());
            var Db = DbUtil.Db;
            Db.Email(Db.StaffEmailForOrg(m.org.OrganizationId),
                m.person, "Commitment confirmation",
@"Thank you for committing to {0}. You have the following slots:<br/>
{1}".Fmt(m.org.OrganizationName, slots));
            Db.Email(m.person.FromEmail,
                Db.PeopleFromPidString(m.org.NotifyIds),
                "commitment received for " + m.org.OrganizationName,
                "{0} committed to:<br/>\n{1}".Fmt(m.org.OrganizationName, slots));
            return RedirectToAction("ConfirmSlots", new { id = m.org.OrganizationId });
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult ConfirmSlots(int id)
        {
            var org = DbUtil.Db.LoadOrganizationById(id);
            ViewData["Organization"] = org.OrganizationName;
            SetHeaders(org.OrganizationId);
            return View();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ConfirmSubscriptions(ManageSubsModel m)
        {
            m.UpdateSubscriptions();
            List<Person> Staff = null;
            if (m.masterorgid != null)
                Staff = DbUtil.Db.StaffPeopleForOrg(m.masterorgid.Value);
            else
                Staff = DbUtil.Db.StaffPeopleForDiv(m.divid.Value);

            DbUtil.Db.Email(Staff.First().FromEmail, m.person,
                "Subscription Confirmation",
@"Thank you for managing your subscriptions to {0}<br/>
You have the following subscriptions:<br/>
{1}".Fmt(m.Description(), m.Summary));

            DbUtil.Db.Email(m.person.FromEmail, Staff, "Subscriptions managed", @"{0} managed subscriptions to {1}<br/>
You have the following subscriptions:<br/>
{2}".Fmt(m.person.Name, m.Description(), m.Summary));

            SetHeaders(m.divid ?? m.masterorgid.Value);
            return View(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ConfirmPledge(ManagePledgesModel m)
        {
            var Staff = DbUtil.Db.StaffPeopleForOrg(m.orgid);

            //OrganizationMember.InsertOrgMembers(DbUtil.Db, m.orgid, m.pid, 220, DateTime.Now, null, false);

            var desc = "{0}; {1}; {2}, {3} {4}".Fmt(
                m.person.Name,
                m.person.PrimaryAddress,
                m.person.PrimaryCity,
                m.person.PrimaryState,
                m.person.PrimaryZip);

            m.person.PostUnattendedContribution(DbUtil.Db,
                m.pledge ?? 0,
                m.setting.DonationFundId,
                desc, pledge: true);

            var pi = m.GetPledgeInfo();
            var body = m.setting.Body;
            body = body.Replace("{amt}", pi.Pledged.ToString("N2"));
            body = body.Replace("{org}", m.Organization.OrganizationName);
            body = body.Replace("{first}", m.person.PreferredName);
            DbUtil.Db.EmailRedacted(Staff.First().FromEmail, m.person,
                m.setting.Subject, body);

            DbUtil.Db.Email(m.person.FromEmail, Staff, "Online Pledge", @"{0} made a pledge to {1}".Fmt(m.person.Name, m.Organization.OrganizationName));

            SetHeaders(m.orgid);
            return View(m);
        }
        public ActionResult VoteLink(string id, string smallgroup, string message, bool? confirm)
        {
            if (!id.HasValue())
                return Content("bad link");

            var guid = id.ToGuid();
            if (guid == null)
                return Content("invalid link");
            var ot = DbUtil.Db.OneTimeLinks.SingleOrDefault(oo => oo.Id == guid.Value);
            if (ot == null)
                return Content("invalid link");
            if (ot.Used)
                return Content("link used");
            if (ot.Expires.HasValue && ot.Expires < DateTime.Now)
                return Content("link expired");
            var a = ot.Querystring.Split(',');
            var oid = a[0].ToInt();
            var pid = a[1].ToInt();
            var emailid = a[2].ToInt();
            var pre = a[3];
            var q = (from pp in DbUtil.Db.People
                     where pp.PeopleId == pid
                     let org = DbUtil.Db.Organizations.SingleOrDefault(oo => oo.OrganizationId == oid)
                     let om = DbUtil.Db.OrganizationMembers.SingleOrDefault(oo => oo.OrganizationId == oid && oo.PeopleId == pid)
                     select new { p = pp, org = org, om = om }).Single();

            if (q.org == null)
                return Content("org missing, bad link");

            if (q.om == null && q.org.Limit <= q.org.MemberCount)
                return Content("sorry, maximum limit has been reached");

            if (q.om == null && (q.org.RegistrationClosed == true || q.org.OrganizationStatusId == OrgStatusCode.Inactive))
                return Content("sorry, registration has been closed");

            var setting = new RegSettings(q.org.RegSetting, DbUtil.Db, oid);
            if (IsSmallGroupFilled(setting, oid, smallgroup))
                return Content("sorry, maximum limit has been reached for " + smallgroup);

            var omb = q.om;
            omb = OrganizationMember.InsertOrgMembers(DbUtil.Db,
                oid, pid, 220, DateTime.Now, null, false);

            omb.AddToGroup(DbUtil.Db, smallgroup);
            omb.AddToGroup(DbUtil.Db, "emailid:" + emailid);
            ot.Used = true;
            DbUtil.Db.SubmitChanges();
            DbUtil.LogActivity("Votelink: {0}".Fmt(q.org.OrganizationName));

            if (confirm == true)
            {
                var subject = Util.PickFirst(setting.Subject, "no subject");
                var msg = Util.PickFirst(setting.Body, "no message");
                msg = OnlineRegModel.MessageReplacements(q.p, q.org.DivisionName, q.org.OrganizationName, q.org.Location, msg);
                msg = msg.Replace("{details}", smallgroup);
                var NotifyIds = DbUtil.Db.StaffPeopleForOrg(q.org.OrganizationId);

                DbUtil.Db.Email(NotifyIds[0].FromEmail, q.p, subject, msg); // send confirmation
                DbUtil.Db.Email(q.p.FromEmail,
                        DbUtil.Db.StaffPeopleForOrg(q.org.OrganizationId), // notify the staff
                        q.org.OrganizationName,
                        "{0} has registered for {1}<br>{2}".Fmt(q.p.Name, q.org.OrganizationName, smallgroup));
            }

            return Content(message);
        }
		[ValidateInput(false)]
        public ActionResult RegisterLink(string id)
        {
            if (!id.HasValue())
                return Content("bad link");
			if (!Request.Browser.Cookies)
				return Content(Request.UserAgent + "<br>Your browser must support cookies");

            var guid = id.ToGuid();
            if (guid == null)
                return Content("invalid link");
            var ot = DbUtil.Db.OneTimeLinks.SingleOrDefault(oo => oo.Id == guid.Value);
            if (ot == null)
                return Content("invalid link");
            if (ot.Used)
                return Content("link used");
            if (ot.Expires.HasValue && ot.Expires < DateTime.Now)
                return Content("link expired");
            var a = ot.Querystring.Split(',');
            var oid = a[0].ToInt();
            var pid = a[1].ToInt();
            var emailid = a[2].ToInt();
            var q = (from pp in DbUtil.Db.People
                     where pp.PeopleId == pid
                     let org = DbUtil.Db.Organizations.SingleOrDefault(oo => oo.OrganizationId == oid)
                     let om = DbUtil.Db.OrganizationMembers.SingleOrDefault(oo => oo.OrganizationId == oid && oo.PeopleId == pid)
                     select new { p = pp, org = org, om = om }).Single();

            if (q.org == null)
                return Content("org missing, bad link");

            if (q.om == null && q.org.Limit <= q.org.MemberCount)
                return Content("sorry, maximum limit has been reached");

            if (q.om == null && (q.org.RegistrationClosed == true || q.org.OrganizationStatusId == OrgStatusCode.Inactive))
                return Content("sorry, registration has been closed");

            var omb = q.om;
            return Redirect("/OnlineReg/Index/{0}?registertag={1}".Fmt(oid, id));
        }
        private bool IsSmallGroupFilled(RegSettings setting, int orgid, string sg)
        {
            return IsSmallGroupFilled(setting.Dropdown1, orgid, sg)
                || IsSmallGroupFilled(setting.Dropdown2, orgid, sg)
                || IsSmallGroupFilled(setting.Dropdown3, orgid, sg)
                || IsSmallGroupFilled(setting.Checkboxes, orgid, sg)
                || IsSmallGroupFilled(setting.Checkboxes2, orgid, sg);
        }
        private bool IsSmallGroupFilled(List<CmsData.RegSettings.MenuItem> list, int orgid, string sg)
        {
            var i = list.SingleOrDefault(dd => string.Compare(dd.SmallGroup, sg, true) == 0);
            if (i != null && i.Limit > 0)
            {
                var cnt = DbUtil.Db.OrganizationMembers.Count(mm => mm.OrganizationId == orgid && mm.OrgMemMemTags.Any(mt => mt.MemberTag.Name == sg));
                if (cnt >= i.Limit)
                    return true;
            }
            return false;
        }
    }
}
