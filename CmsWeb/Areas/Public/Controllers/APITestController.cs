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
    public class APITestController : CmsController
    {
        public ActionResult Index()
        {
            return View(ApiTestInfo.testplan());
        }
        public ActionResult Init(string script, string uname, string pword)
        {
            Session["APIuname"] = uname;
            Session["APIpword"] = pword;
            Session["APIinit"] = script;
            var valid = CMSMembershipProvider.provider.ValidateUser(uname, pword);
            if (valid)
            {
                var roles = CMSRoleProvider.provider;
                if (!roles.IsUserInRole(uname, "Developer"))
                    valid = false;
            }
            if (!valid)
                return Content("Not a Valid Developer");
            return Content("Authentication Initialized");
        }
        public ActionResult Test(ApiTestInfo api)
        {
            api.args.Add("uname", (string)Session["APIuname"] );
            api.args.Add("pword", (string)Session["APIpword"] );
            var init = (string)Session["APIinit"];
            return Content(APIFunctions.TestAPI(init, api.script, api.args));
        }
    }
}