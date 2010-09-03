using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CmsData;
using UtilityExtensions;

namespace CmsWeb.Areas.Public.Controllers
{
    public class OptOutController : CmsController
    {
        public ActionResult UnSubscribe(string optout, string cancel)
        {
            var id = Request["id"];
            var enc = Request["enc"];
            ViewData["id"] = Request["id"];
            ViewData["enc"] = Request["enc"];
            if (!id.HasValue() && !enc.HasValue())
                return Content("unable to identify request");
            string s = null;
            try
            {
                if (id.HasValue())
                    s = Util.Decrypt(id);
                else if (enc.HasValue())
                    s = Util.DecryptFromUrl(enc);
            }
            catch (Exception ex)
            {
                return Content("unable to identify request");
            }
            if (!(s.HasValue() && s.Contains("|")))
                return Content("Unable to identify request");

            var a = s.SplitStr("|");
            ViewData["fromemail"] = a[1];
            if (Request.HttpMethod.ToUpper() == "GET")
            {
                var p = DbUtil.Db.LoadPersonById(a[0].ToInt());
                ViewData["toemail"] = p.EmailAddress;
                return View();
            }
            if (optout.HasValue() && optout.StartsWith("Yes"))
            {
                var oo = new CmsData.EmailOptOut
                {
                    ToPeopleId = a[0].ToInt(),
                    FromEmail = a[1],
                };
                DbUtil.Db.EmailOptOuts.InsertOnSubmit(oo);
                DbUtil.Db.SubmitChanges();
                return View("Confirm");
            }
            return View("Cancel");
        }
    }
}
