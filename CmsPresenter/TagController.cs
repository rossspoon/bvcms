/* Author: David Carroll
 * Copyright (c) 2008, 2009 Bellevue Baptist Church 
 * Licensed under the GNU General Public License (GPL v2)
 * you may not use this code except in compliance with the License.
 * You may obtain a copy of the License at http://bvcms.codeplex.com/license 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Web.Security;
using UtilityExtensions;
using CmsData;
using System.Collections;

namespace CMSPresenter
{
    public class TagController : PersonSearchController
    {
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public IEnumerable<TaggedPersonInfo> FetchPeopleList(int startRowIndex, int maximumRows, string sortExpression,
            string tag)
        {
            var tid = tag.Split(',')[0].ToInt();
            var query = ApplySearch(null, null, null, 0, tid, null, 99, 0, false);
            count = query.Count();
            query = ApplySort(query, sortExpression)
                .Skip(startRowIndex).Take(maximumRows);
            return FetchPeopleList(query);
        }
        public int Count(int startRowIndex, int maximumRows, string sortExpression, string tag)
        {
            return count;
        }
        public string SharedCount()
        {
            return Db.TagCurrent().SharedWithCountString();
        }
        public Tag NewTag(string name)
        {
            Util.CurrentTag = name;
            return Db.TagCurrent();
        }
        public void RenameTag(string name)
        {
            Db.TagCurrent().Name = name;
            Db.SubmitChanges();
            Util.CurrentTag = name;
        }
        public void DeleteTag()
        {
            var t = Db.TagCurrent();
            t.DeleteTag();
            Db.SubmitChanges();
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<CodeValueItem> UserTags(int? pid)
        {
            return (new CodeValueController()).UserTags(pid);
        }
    }
}
