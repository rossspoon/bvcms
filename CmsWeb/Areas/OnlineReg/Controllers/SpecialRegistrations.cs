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
                om = OrganizationMember.InsertOrgMembers(oid, id, 220, Util.Now, null, false);
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
                Guid guid;
                if (!Guid.TryParse(id, out guid))
                    return Content("invalid link");
                var ot = DbUtil.Db.OneTimeLinks.SingleOrDefault(oo => oo.Id == guid);
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
            var smtp = Util.Smtp();
            Emailer.Email(smtp, m.org.EmailAddresses, m.person, "Commitment confirmation",
@"Thank you for committing to {0}. You have the following slots:<br/>
{1}".Fmt(m.org.OrganizationName, slots));
            Util.Email(smtp, m.person.FromEmail, m.org.EmailAddresses, "commitment received for " + m.org.OrganizationName,
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
            var smtp = Util.Smtp();
            var StaffEmail = ManageSubsModel.StaffEmail(m.divid);

            Emailer.Email(smtp, StaffEmail, m.person,
                "Subscription Confirmation",
@"Thank you for managing your subscriptions to {0}<br/>
You have the following subscriptions:<br/>
{1}".Fmt(m.Division.Name, m.Summary));

            Util.Email(smtp, m.person.FromEmail, StaffEmail,
                "Subscriptions managed",
@"{0} managed subscriptions to {1}<br/>
You have the following subscriptions:<br/>
{2}".Fmt(m.person.Name, m.Division.Name, m.Summary));

            SetHeaders(m.divid);
            return View(m);
        }
        public ActionResult VoteLink(string id, string smallgroup)
        {
            if (!id.HasValue())
                return Content("bad link");

            Guid guid;
            if (!Guid.TryParse(id, out guid))
                return Content("invalid link");
            var ot = DbUtil.Db.OneTimeLinks.SingleOrDefault(oo => oo.Id == guid);
            if (ot == null)
                return Content("invalid link");
            if (ot.Used)
                return Content("link used");
            if (ot.Expires.HasValue && ot.Expires < DateTime.Now)
                return Content("link expired");
            var a = ot.Querystring.Split(',');
            var oid = a[0].ToInt();
            var pid = a[1].ToInt();
            var om = OrganizationMember.InsertOrgMembers(oid, pid, 220, DateTime.Now, null, false);
            om.AddToGroup(DbUtil.Db, smallgroup);
            ot.Used = true;
            DbUtil.Db.SubmitChanges();

            return Content("Thanks for your response");
        }
    }
}
