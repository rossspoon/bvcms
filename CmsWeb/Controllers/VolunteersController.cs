/* Author: David Carroll
 * Copyright (c) 2008, 2009 Bellevue Baptist Church 
 * Licensed under the GNU General Public License (GPL v2)
 * you may not use this code except in compliance with the License.
 * You may obtain a copy of the License at http://bvcms.codeplex.com/license 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using UtilityExtensions;
using System.Web.Routing;
using CMSWeb;
using CMSWeb.Models;
using CmsData;

namespace CMSWeb.Controllers
{
    public class VolunteersController : CMSWebCommon.Controllers.CmsController
    {
        public VolunteersController()
        {
            ViewData["Title"] = "Volunteers";
        }
        public ActionResult Index(int? id)
        {
            var vols = new VolunteersModel();
            UpdateModel(vols);
            DbUtil.LogActivity("Volunteers");
            return View(vols);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public EmptyResult Delete(string id)
        {
            if (User.IsInRole("edit"))
            {
                var iid = id.Substring(1).ToInt();
                var v = DbUtil.Db.VolInterests.SingleOrDefault(vi => vi.Id == iid);
                if (v == null)
                    return new EmptyResult();
                DbUtil.Db.VolInterestInterestCodes.DeleteAllOnSubmit(v.VolInterestInterestCodes);
                DbUtil.Db.VolInterests.DeleteOnSubmit(v);
                DbUtil.Db.SubmitChanges();
            }
            return new EmptyResult();
        }
    }
}
