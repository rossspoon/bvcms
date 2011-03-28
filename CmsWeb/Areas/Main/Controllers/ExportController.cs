using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CmsWeb.Areas.Main.Controllers
{
    public class ExportController : Controller
    {
        public ActionResult UpdatePeople(int id)
        {
            return new CmsWeb.Models.UpdatePeopleModel(id);
        }
    }
}
