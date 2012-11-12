using System;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Security;
using CmsData;
using CmsData.Registration;
using CmsWeb.Models;
using UtilityExtensions;
using System.Text;
using System.Collections.Generic;
using CmsData.Codes;
using CmsWeb.Areas.Manage.Controllers;
using System.Diagnostics;
using CmsWeb;

namespace CmsWeb.Areas.OnlineReg.Controllers
{
	[ValidateInput(false)]
	public partial class OnlineRegController : CmsController
	{
#if DEBUG
		private int INT_timeout = 1600000;
#else
        private int INT_timeout = DbUtil.Db.Setting("RegTimeout", "180000").ToInt();
#endif

		// Main page
		public ActionResult Index(int? id, int? div, bool? testing, int? o, int? d, string email, bool? nologin, bool? login, string registertag, bool? showfamily)
		{
			Util.NoCache(Response);
			if (!id.HasValue && !div.HasValue)
				return Content("no organization");
			var m = new OnlineRegModel
			{
				divid = div,
				orgid = id,
			};
			if (m.org == null && m.div == null && m.masterorg == null)
				return Content("invalid registration");

			if (m.masterorg != null)
			{
				if (!OnlineRegModel.UserSelectClasses(m.masterorg).Any())
					return Content("no classes available on this org");
			}
			else if (m.org != null)
			{
				if ((m.org.RegistrationTypeId ?? 0) == RegistrationTypeCode.None)
					return Content("no registration allowed on this org");
			}
			else if (m.div != null)
			{
				if (!OnlineRegModel.UserSelectClasses(m.divid).Any())
					return Content("no registration allowed on this div");
			}
			m.URL = Request.Url.OriginalString;

			SetHeaders(m);

#if DEBUG2

			m.testing = true;
			m.username = "David";
#else
            m.testing = testing;
#endif
			if (Util.ValidEmail(email) || login != true)
				m.nologin = true;

			if (m.nologin)
				m.CreateList();
			else
				m.List = new List<OnlineRegPersonModel>();

			if (Util.ValidEmail(email))
				m.List[0].email = email;

			var pid = 0;
			if (registertag.HasValue())
			{
				var guid = registertag.ToGuid();
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
				var oid = a[0].ToInt();
				var emailid = a[2].ToInt();
				pid = a[1].ToInt();
				m.registertag = registertag;
			}
			else if (User.Identity.IsAuthenticated)
			{
				pid = Util.UserPeopleId ?? 0;
			}

			if (pid > 0)
			{
				m.List = new List<OnlineRegPersonModel>();
				m.UserPeopleId = pid;
				OnlineRegPersonModel p = null;
				if (showfamily != true)
				{
					p = m.LoadExistingPerson(pid);
					p.LoggedIn = true;
					if (m.masterorg == null && !m.divid.HasValue)
						m.List.Add(p);
					p.ValidateModelForFind(ModelState, m);
				}
				if (!ModelState.IsValid)
					return View(m);
				if (m.masterorg != null && m.masterorg.RegistrationTypeId == RegistrationTypeCode.ManageSubscriptions2)
				{
					TempData["ms"] = m.UserPeopleId;
					return Redirect("/OnlineReg/ManageSubscriptions/{0}".Fmt(m.masterorgid));
				}
				if (m.org != null && m.org.RegistrationTypeId == RegistrationTypeCode.ManageGiving)
				{
					TempData["mg"] = m.UserPeopleId;
					return Redirect("/OnlineReg/ManageGiving/{0}".Fmt(m.orgid));
				}
				if (m.org != null && m.org.RegistrationTypeId == RegistrationTypeCode.OnlinePledge)
				{
					TempData["mp"] = m.UserPeopleId;
					return Redirect("/OnlineReg/ManagePledge/{0}".Fmt(m.orgid));
				}
				if (m.org != null && m.org.RegistrationTypeId == RegistrationTypeCode.ChooseSlot)
				{
					TempData["ps"] = m.UserPeopleId;
					return Redirect("/OnlineReg/ManageVolunteer/{0}".Fmt(m.orgid));
				}
				if (showfamily != true && p.org != null && p.Found == true)
				{
					p.IsFilled = p.org.OrganizationMembers.Count() >= p.org.Limit;
					if (p.IsFilled)
						ModelState.AddModelError(m.GetNameFor(mm => mm.List[0].Found), "Sorry, but registration is closed.");
					if (p.Found == true)
						p.FillPriorInfo();
					CheckSetFee(m, p);
					return View(m);
				}
				return View(m);
			}
			return View(m);
		}
		// authenticate user
		[HttpPost]
		public ActionResult Login(OnlineRegModel m)
		{
			var ret = AccountModel.AuthenticateLogon(m.username, m.password, Session, Request);
			if (ret is string)
			{
				ModelState.AddModelError("authentication", ret.ToString());
				return View("Flow/List", m);
			}
			Session["OnlineRegLogin"] = true;
			var user = ret as User;
			if (m.orgid == Util.CreateAccountCode)
				return Content("/Person/Index/" + Util.UserPeopleId);

			m.CreateList();
			m.UserPeopleId = user.PeopleId;

			if (m.ManagingSubscriptions())
			{
				TempData["ms"] = Util.UserPeopleId;
				if (m.masterorgid.HasValue && m.masterorg.RegistrationTypeId == RegistrationTypeCode.ManageSubscriptions2)
					return Content("/OnlineReg/ManageSubscriptions/{0}".Fmt(m.masterorgid));
				return Content("/OnlineReg/ManageSubscriptions/{0}".Fmt(m.divid));
			}
			if (m.ChoosingSlots())
			{
				TempData["ps"] = Util.UserPeopleId;
				return Content("/OnlineReg/ManageVolunteer/{0}".Fmt(m.orgid));
			}
			if (m.OnlinePledge())
			{
				TempData["mp"] = Util.UserPeopleId;
				return Content("/OnlineReg/ManagePledge/{0}".Fmt(m.orgid));
			}
			if (m.ManageGiving())
			{
				TempData["mg"] = Util.UserPeopleId;
				return Content("/OnlineReg/ManageGiving/{0}".Fmt(m.orgid));
			}
			m.List[0].LoggedIn = true;
			return View("Flow/List", m);
		}
		// Register without logging in
		[HttpPost]
		public ActionResult NoLogin(OnlineRegModel m)
		{
			m.nologin = true;
			m.CreateList();
			return View("Flow/List", m);
		}
		[HttpPost]
		public ActionResult YesLogin(OnlineRegModel m)
		{
			m.nologin = false;
			m.List = new List<OnlineRegPersonModel>();
			return View("Flow/List", m);
		}
		[HttpPost]
		public ActionResult Register(int id, OnlineRegModel m)
		{
			if (m.classid.HasValue)
				m.orgid = m.classid;
			var p = m.LoadExistingPerson(id);
			int index = m.List.Count - 1;
			m.List[index] = p;
			p.ValidateModelForFind(ModelState, m);
			if (!ModelState.IsValid)
				return View("Flow/List", m);
			if (p.ManageSubscriptions() && p.Found == true)
			{
				//p.OtherOK = true;
				return View("Flow/List", m);
			}
			if (p.org != null && p.Found == true)
			{
				p.IsFilled = p.org.OrganizationMembers.Count() >= p.org.Limit;
				if (p.IsFilled)
					ModelState.AddModelError(m.GetNameFor(mm => mm.List[m.List.IndexOf(p)].Found), "Sorry, but registration is closed.");
				if (p.Found == true)
					p.FillPriorInfo();
				//if (!p.AnyOtherInfo())
				//p.OtherOK = true;
				return View("Flow/List", m);
			}
			if (p.ShowDisplay() && p.org != null && p.ComputesOrganizationByAge())
				p.classid = p.org.OrganizationId;
			return View("Flow/List", m);
		}
		[HttpPost]
		public ActionResult Cancel(int id, OnlineRegModel m)
		{
			m.List.RemoveAt(id);
			if (m.List.Count == 0)
				m.List.Add(new OnlineRegPersonModel
				{
					divid = m.divid,
					orgid = m.orgid,
					masterorgid = m.masterorgid,
					LoggedIn = m.UserPeopleId.HasValue,
				});
			return View("Flow/List", m);
		}
		[HttpPost]
		public ActionResult ShowMoreInfo(int id, OnlineRegModel m)
		{
			DbUtil.Db.SetNoLock();
			var p = m.List[id];
			p.ValidateModelForFind(ModelState, m);
			if (p.org != null && p.Found == true)
			{
				p.IsFilled = p.org.OrganizationMembers.Count() >= p.org.Limit;
				if (p.IsFilled)
					ModelState.AddModelError(m.GetNameFor(mm => mm.List[id].dob), "Sorry, but registration is closed.");
				if (p.Found == true)
					p.FillPriorInfo();
				return View("Flow/List", m);
			}
			if (!p.whatfamily.HasValue && (id > 0 || p.LoggedIn == true))
			{
				ModelState.AddModelError(m.GetNameFor(mm => mm.List[id].whatfamily), "Choose a family option");
				return View("Flow/List", m);
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
			return View("Flow/List", m);
		}
		[HttpPost]
		public ActionResult PersonFind(int id, OnlineRegModel m)
		{
			if (id >= m.List.Count)
				return View("Flow/List", m);
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
			p.ValidateModelForFind(ModelState, m);
			if (p.ManageSubscriptions()
				|| p.OnlinePledge()
				|| p.ManageGiving()
				|| m.ChoosingSlots())
			{
				p.OtherOK = true;
			}
			else if (p.org != null)
			{
				p.IsFilled = p.org.OrganizationMembers.Count() >= p.org.Limit;
				if (p.IsFilled)
					ModelState.AddModelError(m.GetNameFor(mm => mm.List[id].dob), "Sorry, but registration is closed.");
				if (p.Found == true)
					p.FillPriorInfo();
			}
			if (p.org != null && p.ShowDisplay() && p.ComputesOrganizationByAge())
				p.classid = p.org.OrganizationId;

			CheckSetFee(m, p);

			return View("Flow/List", m);
		}
		// Set suggested giving fee for an indidividual person
		private static void CheckSetFee(OnlineRegModel m, OnlineRegPersonModel p)
		{
			if (m.OnlineGiving() && p.setting.ExtraValueFeeName.HasValue())
			{
				var f = CmsWeb.Models.OnlineRegPersonModel.Funds().SingleOrDefault(ff => ff.Text == p.setting.ExtraValueFeeName);
				var evamt = p.person.GetExtra(p.setting.ExtraValueFeeName).ToDecimal();
				if (f != null && evamt > 0)
					p.FundItem[f.Value.ToInt()] = evamt;
			}
		}
		[HttpPost]
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
					if (m.masterorgid != null)
						ViewData["orgname"] = m.masterorg.OrganizationName;
					else
						ViewData["orgname"] = m.div.Name;
					ViewData["URL"] = m.URL;
					ViewData["timeout"] = INT_timeout;
					return View("ConfirmManageSub");
				}
				if (m.OnlinePledge())
				{
					p.IsNew = true;
					m.ConfirmManagePledge();
					ViewData["CreatedAccount"] = m.List[0].CreatingAccount;
					DbUtil.Db.SubmitChanges();
					ViewData["email"] = m.List[0].person.EmailAddress;
					ViewData["orgname"] = m.org.OrganizationName;
					ViewData["URL"] = m.URL;
					ViewData["timeout"] = INT_timeout;
					SetHeaders(m);
					return View("ConfirmManagePledge");
				}
				if (m.ManageGiving())
				{
					p.IsNew = true;
					m.ConfirmManageGiving();
					ViewData["CreatedAccount"] = m.List[0].CreatingAccount;
					DbUtil.Db.SubmitChanges();
					ViewData["email"] = m.List[0].person.EmailAddress;
					ViewData["orgname"] = m.org.OrganizationName;
					ViewData["URL"] = m.URL;
					ViewData["timeout"] = INT_timeout;
					SetHeaders(m);
					return View("ConfirmManageGiving");
				}
				if (p.org == null && p.ComputesOrganizationByAge())
					ModelState.AddModelError(m.GetNameFor(mm => mm.List[id].Found), "Sorry, cannot find an appropriate age group");
				else if (!p.ManageSubscriptions())
				{
					p.IsFilled = p.org.OrganizationMembers.Count() >= p.org.Limit;
					if (p.IsFilled)
						ModelState.AddModelError(m.GetNameFor(mm => mm.List[id].dob), "Sorry, that age group is filled");
				}
				p.IsNew = true;
			}
			p.IsValidForExisting = ModelState.IsValid == false;
			if (p.IsNew)
				p.FillPriorInfo();
			if (p.org != null && p.ShowDisplay() && p.ComputesOrganizationByAge())
				p.classid = p.org.OrganizationId;
			//if (!p.AnyOtherInfo())
			//    p.OtherOK = ModelState.IsValid;
			return View("Flow/List", m);
		}
		[HttpPost]
		public ActionResult SubmitOtherInfo(int id, OnlineRegModel m)
		{
			if (m.List.Count <= id)
				return Content("<p style='color:red'>error: cannot find person on submit other info</p>");
			m.List[id].ValidateModelForOther(ModelState);
			return View("Flow/List", m);
		}
		[HttpPost]
		public ActionResult AddAnotherPerson(OnlineRegModel m)
		{
			m.ParseSettings();
			if (!ModelState.IsValid)
				return View("Flow/List", m);
#if DEBUG2
            m.List.Add(new OnlineRegPersonModel
            {
                divid = m.divid,
                orgid = m.orgid,
                masterorgid = m.masterorgid,
                first = "Bethany",
                last = "Carroll",
                //bmon = 1,
                //bday = 29,
                //byear = 1987,
                dob = "1/29/87",
                email = "davcar@pobox.com",
                phone = "9017581862".FmtFone(),
                LoggedIn = m.UserPeopleId.HasValue,
            });
#else
			m.List.Add(new OnlineRegPersonModel
			{
				divid = m.divid,
				orgid = m.orgid,
				masterorgid = m.masterorgid,
				LoggedIn = m.UserPeopleId.HasValue,
			});
#endif
			return View("Flow/List", m);
		}
		[HttpPost]
		public ActionResult AskDonation(OnlineRegModel m)
		{
			if (m.List.Count == 0)
				return Content("Can't find any registrants");
			SetHeaders(m);
			return View(m);
		}
		[HttpPost]
		public ActionResult CompleteRegistration(OnlineRegModel m)
		{
			if (m.AskDonation() && !m.donor.HasValue && m.donation > 0)
			{
				SetHeaders(m);
				ModelState.AddModelError("donation",
					"Please indicate a donor or clear the donation amount");
				return View("AskDonation", m);
			}

			if (m.List.Count == 0)
				return Content("Can't find any registrants");
			DbUtil.LogActivity("Online Registration: {0} ({1})".Fmt(m.Header, m.NameOnAccount));
			if (!m.last.IsNew && m.last.Found != true)
				m.List.Remove(m.last);

			var d = new ExtraDatum { Stamp = Util.Now };
			d.Data = Util.Serialize<OnlineRegModel>(m);
			DbUtil.Db.ExtraDatas.InsertOnSubmit(d);
			DbUtil.Db.SubmitChanges();

			if (m.Amount() == 0 && (m.donation ?? 0) == 0 && !m.Terms.HasValue())
				return RedirectToAction("Confirm",
					new
					{
						id = d.Id,
						TransactionID = "zero due",
					});

			var terms = Util.PickFirst(m.Terms, "");
			if (terms.HasValue())
				ViewData["Terms"] = terms;

			SetHeaders(m);
			if (m.Amount() == 0 && m.Terms.HasValue())
			{
				return View("Terms", new PaymentModel
					{
						Terms = m.Terms,
						_URL = m.URL,
						_timeout = INT_timeout,
						PostbackURL = Util.ServerLink("/OnlineReg/Confirm/" + d.Id),
					});
			}

			ViewBag.timeout = INT_timeout;
			ViewBag.Url = m.URL;

			var om =
				DbUtil.Db.OrganizationMembers.SingleOrDefault(
					mm => mm.OrganizationId == m.orgid && mm.PeopleId == m.List[0].PeopleId);
			m.ParseSettings();

			if (om != null && m.settings[m.orgid.Value].AllowReRegister == false)
            {
	            return Content("You are already registered it appears");
            }

			var pf = PaymentForm.CreatePaymentForm(m);
			pf.DatumId = d.Id;
			pf.FormId = Guid.NewGuid();
			if (OnlineRegModel.GetTransactionGateway() == "serviceu")
					return View("Payment", pf);
			return View("ProcessPayment", pf);
		}
		[HttpPost]
		public JsonResult CityState(string id)
		{
			var z = DbUtil.Db.ZipCodes.SingleOrDefault(zc => zc.Zip == id);
			if (z == null)
				return Json(null);
			return Json(new { city = z.City.Trim(), state = z.State });
		}
		private Dictionary<int, Settings> _settings;
		public Dictionary<int, Settings> settings
		{
			get
			{
				if (_settings == null)
					_settings = HttpContext.Items["RegSettings"] as Dictionary<int, Settings>;
				return _settings;
			}
		}
		public class CurrentRegistration
		{
			public string OrgName { get; set; }
			public int OrgId { get; set; }
			public string RegType { get; set; }
		}
		public ActionResult Current()
		{
			var q = from o in DbUtil.Db.Organizations
					where o.LastMeetingDate == null || o.LastMeetingDate < DateTime.Today
					where o.RegistrationTypeId > 0
					where o.OrganizationStatusId == 30
					where !(o.RegistrationClosed ?? false)
					select new { o.FullName2, o.OrganizationId, o.RegistrationTypeId, o.LastMeetingDate, o.OrgPickList };
			var list = q.ToList();
			var q2 = from i in list
					 where !list.Any(ii => (ii.OrgPickList ?? "0").Split(',').Contains(i.OrganizationId.ToString()))
					 orderby i.OrganizationId
					 select new CurrentRegistration
							{
								OrgName = i.FullName2,
								OrgId = i.OrganizationId,
								RegType = RegistrationTypeCode.Lookup(i.RegistrationTypeId)
							};
			return View(q2);
		}
		public ActionResult Timeout(string ret)
		{
			FormsAuthentication.SignOut();
			Session.Abandon();
			ViewBag.Url = ret;
			return View();
		}
	}
}
