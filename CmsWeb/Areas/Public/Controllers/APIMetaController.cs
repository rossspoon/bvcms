using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using CmsData;
using UtilityExtensions;
using CmsWeb.Models;
using System.Xml;
using System.IO;
using System.Net.Mail;
using CmsData.Codes;
using CmsData.API;
using System.Text;
using System.Net;
using CmsWeb.Areas.Manage.Controllers;

namespace CmsWeb.Areas.Public.Controllers
{
#if DEBUG
#else
    [RequireHttps]
#endif
    public class APIMetaController : CmsController
    {
        [HttpGet]
        public ActionResult Lookups(string id)
        {
            var ret = AuthenticateDeveloper();
            if (ret.StartsWith("!"))
                return Content("<Lookups error=\"{0}\" />".Fmt(ret.Substring(1)));
            if (!id.HasValue())
                return Content("Lookups error=\"not found\">");
            var q = DbUtil.Db.ExecuteQuery<CmsWeb.Areas.Setup.Controllers.LookupController.Row>("select * from lookup." + id);
            var w = new CmsData.API.APIWriter();
            w.Start("Lookups");
            w.Attr("name", id);
            foreach(var i in q)
            {
                w.Start("Lookup");
                w.Attr("Id", i.Id);
                w.AddText(i.Description);
                w.End();
            }
            w.End();
            return Content(w.ToString(), "text/xml");
        }
		public ActionResult Cookies()
		{
			var s = Request.UserAgent;
			if (Request.Browser.Cookies == true)
				return Content("supports cookies<br>" + s);
			return Content("does not support cookies<br>" + s);
		}
    }
}