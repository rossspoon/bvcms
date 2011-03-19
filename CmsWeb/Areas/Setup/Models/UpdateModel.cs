/* Author: David Carroll
 * Copyright (c) 2008, 2009 Bellevue Baptist Church 
 * Licensed under the GNU General Public License (GPL v2)
 * you may not use this code except in compliance with the License.
 * You may obtain a copy of the License at http://bvcms.codeplex.com/license 
 */
using System;
using System.Collections;
using System.Linq;
using System.Web;
using UtilityExtensions;
using System.Web.Mvc;
using System.Web.Routing;
using System.Text.RegularExpressions;
using System.Threading;
using System.Data.Linq;
using CmsData;
using System.Collections.Generic;
using CMSPresenter;

namespace CmsWeb.Models
{
    public class UpdateModel
    {
        public string Tag { get; set; }
        public string Field { get; set; }
        public string NewValue { get; set; }
        public CMSPresenter.CodeValueController cv = new CodeValueController();

        public SelectList Fields()
        {
            return new SelectList(new[] { 
                "(not specified)",
                "Member Status",
                "Campus",
                "Marital Status",
                "Gender",
                "Occupation",
                "School",
                "Employer",
                "Family Position"
            }.Select(x => new { value = x, text = x }),
                "value", "text");
        }
        public IEnumerable<SelectListItem> Tags()
        {
            var tg = QueryModel.ConvertToSelect(cv.UserTags(Util.UserPeopleId), "Id").ToList();
            tg.Insert(0, new SelectListItem { Value = "0", Text = "(not specified)" });
            return tg;
        }
    }
}
