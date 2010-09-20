using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using CmsData;
using UtilityExtensions;
using CmsWeb.Models.iPhone;
using System.Xml;
using System.IO;

namespace CmsWeb.Areas.Main.Controllers
{
#if DEBUG
#else
   [RequireHttps]
#endif
    public class iPhoneController : CmsController
    {
        private bool Authenticate()
        {
            var username = Request.Headers["username"];
            var password = Request.Headers["password"];
            return CMSMembershipProvider.provider.ValidateUser(username, password);
        }
        public ActionResult FetchImage(int id)
        {
            if (!Authenticate())
                return Content("not authorized");
            var person = DbUtil.Db.People.Single(pp => pp.PeopleId == id);
            if (person.PictureId != null)
                return new CmsWeb.Models.ImageResult(person.Picture.SmallId ?? 0);
            return new CmsWeb.Models.ImageResult(0);
        }
        public ActionResult Search(string name, string comm, string addr)
        {
            if (!Authenticate())
                return Content("not authorized");
            var m = new SearchModel(name, comm, addr);
            return new SearchResult(m.PeopleList(), m.Count);
        }
    }
}
