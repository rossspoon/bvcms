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
    public class UpdateFieldsModel
    {
        public string Tag { get; set; }
        public string Field { get; set; }
        public string NewValue { get; set; }

        public SelectList Fields()
        {
            return new SelectList(new[] { 
                "(not specified)",
                "Campus",
                "Employer",
                "Family Position",
                "Gender",
                "Grade",
                "Marital Status",
                "Member Status",
                "Occupation",
                "School",
            }.Select(x => new { value = x, text = x }),
                "value", "text");
        }
        public IEnumerable<SelectListItem> Tags()
        {
            var cv = new CodeValueController();
            var tg = QueryModel.ConvertToSelect(cv.UserTags(Util.UserPeopleId), "Id").ToList();
            tg.Insert(0, new SelectListItem { Value = "0", Text = "(not specified)" });
            return tg;
        }
        public IEnumerable<TitleItems> TitleItems()
        {
            var Model = new CodeValueController();
            return new List<TitleItems>
            {
                new TitleItems { title = "Member Status Codes", items = Model.MemberStatusCodes() },
                new TitleItems { title = "Campus Codes", items = Model.AllCampuses() },
                new TitleItems { title = "Marital Status Codes", items = Model.MaritalStatusCodes() },
                new TitleItems { title = "Gender Codes", items = Model.GenderCodes() },
                new TitleItems { title = "Family Position Codes", items = Model.FamilyPositionCodes() },
            };
        }
    }
    public class TitleItems
    {
        public string title { get; set; }
        public IEnumerable<CodeValueItem> items { get; set; }
    }
}
