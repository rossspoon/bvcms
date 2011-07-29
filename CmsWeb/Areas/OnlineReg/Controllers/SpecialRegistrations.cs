using System;
using System.Linq;
using System.Web.Mvc;
using CmsData;
using CmsWeb.Models;
using UtilityExtensions;
using CmsWeb.Areas.Manage.Controllers;
using CmsWeb.Areas.OnlineReg.Models.Payments;
using System.Text;

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
            return View(new SlotModel(m.List[0].PeopleId.Value, m.orgid.Value));
        }
        [AcceptVerbs(HttpVerbs.Post)]
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
                ot.Used = true;
                DbUtil.Db.SubmitChanges();
            }
            SetHeaders(m.divid);
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
            var Staff = DbUtil.Db.StaffPeopleForDiv(m.divid);

            DbUtil.Db.Email(Staff.First().FromEmail, m.person,
                "Subscription Confirmation",
@"Thank you for managing your subscriptions to {0}<br/>
You have the following subscriptions:<br/>
{1}".Fmt(m.Division.Name, m.Summary));

            DbUtil.Db.Email(m.person.FromEmail, Staff, "Subscriptions managed", @"{0} managed subscriptions to {1}<br/>
You have the following subscriptions:<br/>
{2}".Fmt(m.person.Name, m.Division.Name, m.Summary));

            SetHeaders(m.divid);
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
                     select new { p=pp, org = org, om = om }).Single();

            if(q.org == null)
                return Content("org missing, bad link");

            if (q.om == null && q.org.Limit <= q.org.MemberCount)
                return Content("sorry, maximum limit has been reached");

            var omb = q.om;
            omb = OrganizationMember.InsertOrgMembers(DbUtil.Db,
                oid, pid, 220, DateTime.Now, null, false);
            
            omb.AddToGroup(DbUtil.Db, smallgroup);
            omb.AddToGroup(DbUtil.Db, "emailid:" + emailid);
            ot.Used = true;
            DbUtil.Db.SubmitChanges();

            if (confirm == true)
            {
                var subject = Util.PickFirst(q.org.EmailSubject, "no subject");
                var msg = Util.PickFirst(q.org.EmailMessage, "no message");
                msg = OnlineRegModel.MessageReplacements(q.p, q.org.DivisionName, q.org.OrganizationName, q.org.Location, msg);
                var NotifyIds = DbUtil.Db.StaffPeopleForOrg(q.org.OrganizationId);

                DbUtil.Db.Email(NotifyIds[0].FromEmail, q.p, subject, msg); // send confirmation
                DbUtil.Db.Email(q.p.FromEmail,
                        DbUtil.Db.StaffPeopleForOrg(q.org.OrganizationId), // notify the staff
                        q.org.OrganizationName,
                        "{0} has registered for {1}".Fmt(q.p.Name, q.org.OrganizationName));
            }

            return Content(message);
        }
    }
}
