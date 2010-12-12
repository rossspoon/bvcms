using System;
using System.Linq;
using System.Web.Mvc;
using CmsData;
using CmsWeb.Models;
using UtilityExtensions;
using CmsWeb.Areas.Manage.Controllers;

namespace CmsWeb.Areas.OnlineReg.Controllers
{
    [ValidateInput(false)]
    public partial class OnlineRegController : CmsController
    {
#if DEBUG
        private const int INT_timeout = 1600000;
#else
        private const int INT_timeout = 60000;
#endif

        public ActionResult Index(int? id, int? div, bool? testing, int? o, int? d, string email)
        {
            if (!id.HasValue && !div.HasValue)
                return Content("no organization");
            var m = new OnlineRegModel
            {
                divid = div,
                orgid = id,
            };
            if (m.org == null && m.div == null)
                return Content("invalid registration");

            if (m.org != null)
            {
                if ((m.org.RegistrationTypeId ?? 0) == (int)CmsData.Organization.RegistrationEnum.None)
                    return Content("no registration allowed on this org");
            }
            else if (m.div != null)
                if (OnlineRegModel.UserSelectClasses(m.divid).Count() == 0)
                    return Content("no registration allowed on this div");
            m.URL = Request.Url.OriginalString;

            Session["gobackurl"] = m.URL;

            ViewData["timeout"] = INT_timeout;
            SetHeaders(m.divid ?? m.orgid ?? 0);

#if DEBUG

            m.testing = true;
            m.username = "David";
            m.password = "toby123";
#else
            m.testing = testing;
#endif
            if (Util.ValidEmail(email))
            {
                m.nologin = true;
                m.CreateList();
                m.List[0].email = email;
            }
            return View(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Cancel(int id, OnlineRegModel m)
        {
            m.List.RemoveAt(id);
            if (m.List.Count == 0)
                m.List.Add(new OnlineRegPersonModel
                {
                    divid = m.divid,
                    orgid = m.orgid,
                    LoggedIn = m.UserPeopleId.HasValue,
                });
            return View("list", m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ShowMoreInfo(int id, OnlineRegModel m)
        {
            DbUtil.Db.SetNoLock();
            var p = m.List[id];
            p.ValidateModelForFind(ModelState);
            if (p.org != null && p.Found == true)
            {
                p.IsFilled = p.org.OrganizationMembers.Count() >= p.org.Limit;
                if (p.IsFilled)
                    ModelState.AddModelError("dob", "Sorry, but registration is closed.");
                if (p.Found == true)
                    p.FillPriorInfo();
                if (!p.AnyOtherInfo())
                    p.OtherOK = true;
                return View("list", m);
            }
            if (!p.whatfamily.HasValue && (id > 0 || p.LoggedIn == true))
            {
                ModelState.AddModelError("whatfamily", "Choose a family option");
                return View("list", m);
            }
            switch (p.whatfamily)
            {
                case 1:
                    var u = DbUtil.Db.LoadPersonById(m.UserPeopleId.Value);
                    p.address = u.PrimaryAddress;
                    p.city = u.PrimaryCity;
                    p.state = u.PrimaryState;
                    p.zip = u.PrimaryZip.FmtZip();
                    break;
                case 2:
                    var pb = m.List[id - 1];
                    p.address = pb.address;
                    p.city = pb.city;
                    p.state = pb.state;
                    p.zip = pb.zip;
                    break;
                default:
#if DEBUG
                    //p.address = "235 Riveredge Cv.";
                    //p.city = "Cordova";
                    //p.state = "TN";
                    //p.zip = "38018";
                    //p.gender = 1;
                    //p.married = 10;
#endif
                    break;
            }
            p.ShowAddress = true;
            return View("list", m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult PersonFind(int id, OnlineRegModel m)
        {
            DbUtil.Db.SetNoLock();
            var p = m.List[id];
            if (m.classid.HasValue)
            {
                m.orgid = m.classid;
                p.classid = m.classid;
                p.orgid = m.classid;
            }
            p.classid = m.classid;
            p.PeopleId = null;
            p.ValidateModelForFind(ModelState);
            if (p.ManageSubscriptions())
            {
                p.OtherOK = true;
                //if (p.Found == true)
                //    return Content("/OnlineReg//{0}".Fmt(m.divid));
            }
            else if (p.org != null)
            {
                p.IsFilled = p.org.OrganizationMembers.Count() >= p.org.Limit;
                if (p.IsFilled)
                    ModelState.AddModelError("dob", "Sorry, but registration is closed.");
                if (p.Found == true)
                    p.FillPriorInfo();
                if (!p.AnyOtherInfo())
                    p.OtherOK = true;
            }
            if (p.ShowDisplay() && p.ComputesOrganizationByAge())
                p.classid = p.org.OrganizationId;
            return View("list", m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SubmitNew(int id, OnlineRegModel m)
        {
            var p = m.List[id];
            p.ValidateModelForNew(ModelState);

            if (ModelState.IsValid)
            {
                if (m.ManagingSubscriptions())
                {
                    p.IsNew = true;
                    m.ConfirmManageSubscriptions();
                    ViewData["ManagingSubscriptions"] = true;
                    ViewData["CreatedAccount"] = m.List[0].CreatingAccount;
                    DbUtil.Db.SubmitChanges();
                    ViewData["email"] = m.List[0].person.EmailAddress;
                    ViewData["orgname"] = m.div.Name;
                    ViewData["URL"] = m.URL;
                    ViewData["timeout"] = INT_timeout;
                    return View("ConfirmManageSub");
                }
                if (p.org == null && p.ComputesOrganizationByAge())
                    ModelState.AddModelError(p.ErrorTarget, "Sorry, cannot find an appropriate age group");
                else if (!p.ManageSubscriptions())
                {
                    p.IsFilled = p.org.OrganizationMembers.Count() >= p.org.Limit;
                    if (p.IsFilled)
                        ModelState.AddModelError("dob", "Sorry, that age group is filled");
                }
                p.IsNew = true;
            }
            p.IsValidForExisting = ModelState.IsValid == false;
            if (p.IsNew)
                p.FillPriorInfo();
            if (p.ShowDisplay() && p.ComputesOrganizationByAge())
                p.classid = p.org.OrganizationId;
            if (!p.AnyOtherInfo())
                p.OtherOK = ModelState.IsValid;
            return View("list", m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SubmitOtherInfo(int id, OnlineRegModel m)
        {
            var p = m.List[id];
            p.ValidateModelForOther(ModelState);
            p.OtherOK = ModelState.IsValid;
            return View("list", m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddAnotherPerson(OnlineRegModel m)
        {
            if (!ModelState.IsValid)
                return View("list", m.List);
#if DEBUG
            m.List.Add(new OnlineRegPersonModel
            {
                divid = m.divid,
                orgid = m.orgid,
                first = "Bethany",
                last = "Carroll",
                dob = "1/29/86",
                email = "davcar@pobox.com",
                phone = "9017581862".FmtFone(),
                LoggedIn = m.UserPeopleId.HasValue,
            });
#else
            m.List.Add(new OnlineRegPersonModel
            {
                divid = m.divid,
                orgid = m.orgid,
                LoggedIn = m.UserPeopleId.HasValue,
            });
#endif
            return View("list", m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CompleteRegistration(OnlineRegModel m)
        {
            DbUtil.Db.SetNoLock();
            var d = DbUtil.Db.GetDatum<OnlineRegModel>(m);
            if (m.ChoosingSlots())
                return RedirectToAction("PickSlots", new { id = d.Id });

            if (m.Amount() == 0 && !m.Terms.HasValue())
                return RedirectToAction("Confirm",
                    new
                    {
                        id = d.Id,
                        TransactionID = "zero due",
                    });

            var p = m.List[0];
            var pm = new PaymentModel
            {
                NameOnAccount = m.NameOnAccount,
                Address = p.address,
                Amount = m.Amount(),
                City = p.city,
                Email = p.email,
                Phone = p.phone.FmtFone(),
                State = p.state,
                PostalCode = p.zip,
                testing = m.testing ?? false,
                PostbackURL = Util.ServerLink("/OnlineReg/Confirm/" + d.Id),
                Misc2 = m.Header,
                Terms = Util.PickFirst(m.Terms, ""),
                _URL = m.URL,
                _timeout = INT_timeout,
                _datumid = d.Id,
                _confirm = "confirm"
            };
            pm.Misc1 = pm.NameOnAccount;

            SetHeaders(m.orgid ?? m.divid ?? 0);
            if (m.Amount() == 0 && m.Terms.HasValue())
                return View("Terms", pm);
            return View("Payment", pm);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult PayWithCoupon()
        {
            var pm = new PaymentModel();
            try
            {
                UpdateModel(pm);
                if (!pm._Coupon.HasValue())
                    return Json(new { error = "empty coupon" });
            }
            catch (Exception)
            {
                return Json(new { error = "problem coupon" });
            }
            var ed = DbUtil.Db.ExtraDatas.SingleOrDefault(e => e.Id == pm._datumid);
            var m = Util.DeSerialize<OnlineRegModel>(ed.Data.Replace("CMSWeb.Models", "CmsWeb.Models"));
            string coupon = pm._Coupon.ToUpper().Replace(" ", "");
            string admincoupon = DbUtil.Db.Setting("AdminCoupon", "ifj4ijweoij").ToUpper().Replace(" ", "");
            if (coupon == admincoupon)
                return Json(new
                {
                    confirm = "/onlinereg/{0}/{1}?TransactionID=Coupon(Admin)"
                        .Fmt(pm._confirm, pm._datumid)
                });
            var c = DbUtil.Db.Coupons.SingleOrDefault(cp => cp.Id == coupon);
            if (c == null)
                return Json(new { error = "coupon not found" });
            if (m.divid.HasValue)
            {
                if (c.DivId != m.divid)
                    return Json(new { error = "coupon division not match" });
            }
            else if (m.orgid != c.OrgId)
                return Json(new { error = "coupon org not match" });
            if (DateTime.Now.Subtract(c.Created).TotalHours > 24)
                return Json(new { error = "coupon expired" });
            if (c.Used.HasValue)
                return Json(new { error = "coupon already used" });
            if (c.Canceled.HasValue)
                return Json(new { error = "coupon canceled" });
            return Json(new
            {
                confirm = "/onlinereg/{0}/{1}?TransactionID=Coupon({2})"
                    .Fmt(pm._confirm, pm._datumid, coupon.Insert(8, " ").Insert(4, " "))
            });
        }
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
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Login(OnlineRegModel m)
        {
            var ret = AccountController.AuthenticateLogon(m.username, m.password, Session, Request);
            if (ret is string)
            {
                ModelState.AddModelError("authentication", ret.ToString());
                return View("List", m);
            }
            var user = ret as User;
            if (m.orgid == Util.CreateAccountCode)
                return Content("/Person/Index/" + Util.UserPeopleId);

            m.CreateList();
            m.UserPeopleId = user.PeopleId;
            if (m.ManagingSubscriptions())
            {
                TempData["ms"] = Util.UserPeopleId;
                return Content("/OnlineReg/ManageSubscriptions/{0}".Fmt(m.divid));
            }
            m.List[0].LoggedIn = true;
            return View("List", m);
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
        public ActionResult NoLogin(OnlineRegModel m)
        {
            m.nologin = true;
            m.CreateList();
            return View("List", m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Register(int id, OnlineRegModel m)
        {
            if (m.classid.HasValue)
                m.orgid = m.classid;
            var p = m.LoadExistingPerson(id);
            p.ValidateModelForFind(ModelState);
            if (!ModelState.IsValid)
        		return View("List", m);
            if (p.ManageSubscriptions() && p.Found == true)
            {
                p.OtherOK = true;
                return View("List", m);
            }
            if (p.org != null && p.Found == true)
            {
                p.IsFilled = p.org.OrganizationMembers.Count() >= p.org.Limit;
                if (p.IsFilled)
                    ModelState.AddModelError(p.ErrorTarget, "Sorry, but registration is closed.");
                if (p.Found == true)
                    p.FillPriorInfo();
                if (!p.AnyOtherInfo())
                    p.OtherOK = true;
                return View("list", m);
            }
            if (p.ShowDisplay() && p.org != null && p.ComputesOrganizationByAge())
                p.classid = p.org.OrganizationId;
            return View("List", m);
        }
    }
}
