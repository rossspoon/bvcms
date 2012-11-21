using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Text;
using System.IO;
using CmsWeb.Models;
using CmsData;
using UtilityExtensions;

namespace CmsWeb.Areas.Finance.Controllers
{
	[Authorize(Roles = "Finance")]
	public class QuickBooksController : Controller
	{
        public static Dictionary<string, QuickBooksHelper> helpers = new Dictionary<string, QuickBooksHelper>();

		public ActionResult Index()
		{
			return View();
		}

		public ActionResult RequestOAuthToken()
		{
            QuickBooksHelper qbh = new QuickBooksHelper(Request);
            string authLink = qbh.RequestOAuthToken();

            helpers[makeKey()] = qbh;

            return Redirect(authLink);
		}

		public ActionResult RequestAccessToken()
		{
            QuickBooksHelper qbh = helpers[makeKey()];
            qbh.RequestAccessToken(Request["realmId"], Request["oauth_verifier"]);

            // TODO: Change response based on results
            return View("Index");
		}

		public ActionResult Disconnect()
		{
            QuickBooksHelper qbh = new QuickBooksHelper(Request);
            bool complete = qbh.Disconnect();

            // TODO: Change response based on results
			return View("Index");
		}

        public string makeKey()
        {
            return Util.CmsHost2 + "-" + Util.UserId;
        }
	}
}