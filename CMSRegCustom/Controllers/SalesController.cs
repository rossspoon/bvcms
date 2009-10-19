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
            var m = new SalesModel(id);
            ViewData["header"] = "Purchase " + m.saleitem.Description;
            if (Request.HttpMethod.ToUpper() == "GET")
                return View(m);

            UpdateModel(m);
            m.testing = (m.quantity == testquantity);
            m.ValidateModel(ModelState);
            if (!ModelState.IsValid)
                return View(m);

            var transaction = m.CreateNewTransaction();
            m.SendNotice();

            TempData["tranid"] = transaction.Id;
            TempData["testing"] = m.testing;
            return RedirectToAction("Payment");

        }
        public ActionResult Payment()
        {
            if (!TempData.ContainsKey("tranid"))
                return View("Unknown");
            var m = new SalesModel { tranid = (int)TempData["tranid"] };
            m.testing = (bool)TempData["testing"];
            return View(m);
        }
        public ActionResult Confirm(int? id, string TransactionID, string OrgId)
        {
            if (!id.HasValue)
                return View("Unknown");
            if (!TransactionID.HasValue())
                return Content("error no transaction");

            var m = new SalesModel { tranid = id };

            m.testing = false;
            if ((string)DbUtil.Settings("ServiceUOrgIDTest", "") == OrgId)
                m.testing = true;
            if (m.testing)
                m.transaction.TransactionId = "Test-" + TransactionID;
            else
                m.transaction.TransactionId = TransactionID;
            DbUtil.Db.SubmitChanges();

            var p = m.person;
            m.transaction.Username = MembershipService.FetchUsernameNoCheck(
                m.person.FirstName, m.person.LastName);
            var password = MembershipService.FetchPassword();
            if (m.testing)
                m.transaction.Password = password + ".test";
            else
                m.transaction.Password = password;
           DbUtil.Db.SubmitChanges();

            var c = DbUtil.Content("SaleMessage-" + m.saleitem.Id);
            var Body = c.Body;
            Body = Body.Replace("{first}", p.NickName.HasValue() ? p.NickName : p.FirstName);
            Body = Body.Replace("{quantity}", m.transaction.Quantity.ToString());
            Body = Body.Replace("{amount}", m.transaction.Amount.ToString("C"));
            Body = Body.Replace("{description}", m.transaction.ItemDescription);
            Body = Body.Replace("{username}", m.transaction.Username);
            Body = Body.Replace("{password}", password);
            Body = Body.Replace("{download}", 
                Request.Url.Scheme + "://" + Request.Url.Authority + "/Sales/Download/" + m.saleitem.Id);

            Util.Email(m.saleitem.Email,
                 m.person.Name, m.transaction.EmailAddress, c.Title, Body);
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
        public EmptyResult ItemDelete(string id)
        {
            id = id.Substring(1);
            var o = DbUtil.Db.SaleItems.SingleOrDefault(si => si.Id == id.ToInt());
            if (o == null)
                return new EmptyResult();
            DbUtil.Db.SaleItems.DeleteOnSubmit(o);
            DbUtil.Db.SubmitChanges();
            return new EmptyResult();
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
            var c = DbUtil.Content("SaleDownload-" + id);
            ViewData["content"] = c.Body;
            ViewData["title"] = c.Title;
            return View();
        }
        private int testquantity { get { return DbUtil.Settings("ServiceUTestQuantity", "99").ToInt(); } }
    }
}
