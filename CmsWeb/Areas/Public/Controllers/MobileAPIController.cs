using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CmsData;
using CmsWeb.Models.iPhone;
using CmsWeb.MobileAPI;
using UtilityExtensions;

namespace CmsWeb.Areas.Public.Controllers
{
	public class MobileAPIController : Controller
	{
		public ActionResult Authorize()
		{
			BaseReturn br = new BaseReturn();

			if (SystemHelper.Authenticate()) return br;
			else
			{
				br.error = 1;
				br.data = "Username and password combination not found, please try again.";
				return br;
			}
		}

		public ActionResult Search(string name, string comm, string addr)
		{
			List<MobilePerson> mp = new List<MobilePerson>();
			BaseReturn br = new BaseReturn();

			var m = new SearchModel(name, comm, addr);

			br.type = 1;
			br.count = m.Count;

			foreach (var item in m.ApplySearch().OrderBy(p => p.Name2).Take(20))
			{
				mp.Add(new MobilePerson().populate(item));
			}

			br.data = JSONHelper.JsonSerializer<List<MobilePerson>>(mp);

			return br;
		}
	}

	public class BaseReturn : ActionResult
	{
		public int error = 0;
		public int type = 0;
		public int count = 0;
		public string data = "";

		public override void ExecuteResult(ControllerContext context)
		{
			context.HttpContext.Response.ContentType = "application/json";
			context.HttpContext.Response.Output.Write(JSONHelper.JsonSerializer<BaseReturn>(this));
		}
	}
}