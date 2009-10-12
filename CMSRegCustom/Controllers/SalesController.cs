using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using CmsData;
using CMSRegCustom.Models;
using UtilityExtensions;
using System.Configuration;

namespace CMSRegCustom.Controllers
{
    public class SalesController : Controller
    {
        public SalesController()
        {
            ViewData["header"] = "Purchase Item";
            ViewData["logoimg"] = "/Content/Crosses.png";
        }
        public ActionResult Item(int id, int? test)
        {
            if (test.HasValue && test > 0)
                Session["test"] = "1";
            var m = new SalesModel(id);
            ViewData["header"] = "Purchase " + m.saleitem.Description;
            if (Request.HttpMethod.ToUpper() == "GET")
                return View(m);

            UpdateModel(m);
            m.ValidateModel(ModelState);
            if (ModelState.IsValid)
            {
                var count = m.FindMember();
                if (count > 1)
                    ModelState.AddModelError("find", "More than one match, sorry");
                else if (count == 0)
                    if (!m.shownew)
                    {
                        ModelState.AddModelError("find", "Cannot find church record.");
                        m.shownew = true;
                    }
            }
            if (!ModelState.IsValid)
                return View(m);

            if (m.person == null)
                m.AddPerson();

            var transaction = new SaleTransaction
            {
                PeopleId = m.person.PeopleId,
                SaleDate = DateTime.Now,
                ItemId = m.saleitem.Id,
                ItemDescription = m.saleitem.Description,
                Quantity = m.quantity,
                Amount = m.amount,
                EmailAddress = m.email,
            };
            DbUtil.Db.SaleTransactions.InsertOnSubmit(transaction);

            DbUtil.Db.SubmitChanges();

            Util.Email2(m.email,
                    m.saleitem.Email, "Puchased Item",
@"{0}({1}) has purchased {2} {3} (check cms to confirm feepaid)</p>".Fmt(
m.person.Name, m.peopleid, m.quantity, m.Description));

            TempData["tranid"] = transaction.Id;
            return RedirectToAction("Payment");

        }
        public ActionResult Payment()
        {
            if (!TempData.ContainsKey("tranid"))
                return View("Unknown");
            var m = new SalesModel { tranid = (int)TempData["tranid"] };
            return View(m);
        }
        public ActionResult Confirm(int? id, string TransactionID)
        {
            if (!id.HasValue)
                return View("Unknown");
            var m = new SalesModel { tranid = id };
            if (!TransactionID.HasValue())
                return Content("error no transaction");

            m.transaction.TransactionId = TransactionID;
            DbUtil.Db.SubmitChanges();

            var p = m.person;
            var svc = new AccountMembershipService();
            m.transaction.Username = svc.FetchUsernameNoCheck(m.person.FirstName, m.person.LastName);
            m.transaction.Password = svc.FetchPassword();
            DbUtil.Db.SubmitChanges();

            var c = DbUtil.Db.Contents.Single(cc => cc.Name == "SaleMessage-" + m.saleitem.Id);
            c.Body = c.Body.Replace("{first}", p.NickName.HasValue() ? p.NickName : p.FirstName);
            c.Body = c.Body.Replace("{quantity}", m.transaction.Quantity.ToString());
            c.Body = c.Body.Replace("{amount}", m.transaction.Amount.ToString("C"));
            c.Body = c.Body.Replace("{description}", m.transaction.ItemDescription);
            c.Body = c.Body.Replace("{username}", m.transaction.Username);
            c.Body = c.Body.Replace("{password}", m.transaction.Password);
            c.Body = c.Body.Replace("{download}", 
                Request.Url.Scheme + "://" + Request.Url.Authority + "/Sales/Download/" + m.saleitem.Id);

            Util.Email(m.saleitem.Email,
                 m.person.Name, m.transaction.EmailAddress, c.Title, c.Body);
            return View(m);
        }
        public JsonResult CityState(string id)
        {
            var z = DbUtil.Db.ZipCodes.SingleOrDefault(zc => zc.Zip == id);
            if (z == null)
                return Json(null);
            return Json(new { city = z.City.Trim(), state = z.State });
        }
        [Authorize(Roles="Attendance")]
        public ActionResult Transactions()
        {
            var m = new SalesModel();
            return View(m.Transactions());
        }
        [Authorize(Roles="Admin")]
        public ActionResult Items()
        {
            var m = new SalesModel();
            return View(m.SaleItems());
        }
        [Authorize(Roles = "Admin")]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ItemCreate()
        {
            var o = new CmsData.SaleItem();
            DbUtil.Db.SaleItems.InsertOnSubmit(o);
            DbUtil.Db.SubmitChanges();
            return Redirect("/Sales/Items/");
        }
        [Authorize(Roles = "Admin")]
        [AcceptVerbs(HttpVerbs.Post)]
        public ContentResult ItemEdit(string id, string value)
        {
            var a = id.Split('.');
            var c = new ContentResult();
            c.Content = value;
            var o = DbUtil.Db.SaleItems.SingleOrDefault(i => i.Id == a[1].ToInt());
            if (o == null)
                return c;
            switch (a[0])
            {
                case "Description":
                    o.Description = value;
                    break;
                case "Url":
                    o.Url = value;
                    break;
                case "Email":
                    o.Email = value;
                    break;
                case "Price":
                    o.Price = decimal.Parse(value);
                    break;
                case "Available":
                    o.Available = bool.Parse(value);
                    break;
                case "DefaultItems":
                    o.DefaultItems = int.Parse(value);
                    break;
                case "MaxItems":
                    o.MaxItems = value.ToInt();
                    break;
            }
            DbUtil.Db.SubmitChanges();
            return c;
        }
        public ActionResult Download(int id, string username, string password)
        {
            ViewData["itemid"] = id;
            if (Request.HttpMethod.ToUpper() == "GET")
                 return View("Login");
            var q = from t in DbUtil.Db.SaleTransactions
                    where t.ItemId == id
                    where t.Password == password
                    where t.Username == username
                    select t;
            if (q.SingleOrDefault() == null)
            {
                ModelState.AddModelError("login", "username or password incorrect or not found");
                return View("Login");
            }
            var c = DbUtil.Db.Contents.Single(cc => cc.Name == "SaleDownload-" + id);
            ViewData["content"] = c.Body;
            ViewData["title"] = c.Title;
            return View();
        }
    }
}
