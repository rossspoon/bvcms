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
using CmsData;
using UtilityExtensions;
using CmsWeb.Models;

namespace CmsWeb.Areas.Manage.Controllers
{
    [Authorize(Roles = "Admin")]
    [ValidateInput(false)]
    public class RegSettingsController : CmsStaffController
    {
        public class RegSettingInfo
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string RegSetting { get; set; }
        }
        public ActionResult Index()
        {
            var q = from o in DbUtil.Db.Organizations
                    where o.RegSetting.Length > 0
                    orderby o.OrganizationName
                    select new RegSettingInfo
                    { 
                        Id = o.OrganizationId,
                        Name = o.OrganizationName, 
                        RegSetting = o.RegSetting, 
                    };
            return View(q);
        }
    }
}
