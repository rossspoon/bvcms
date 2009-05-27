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
using CmsData;
using System.Data.Linq;
using System.ComponentModel;
using UtilityExtensions;
using System.Web;
using System.Collections;

namespace CMSPresenter
{
    public class ActivityInfo
    {
        public string Name { get; set; }
        public string Username { get; set; }
        public int UserId { get; set; }
        public string Activity { get; set; }
        public string Url { get; set; }
        public DateTime Date { get; set; }
    }
    [DataObject]
    public class ActivityController
    {
        int _count;
        public int Count(int uid, int maximumRows, int startRowIndex)
        {
            return _count;
        }
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public IEnumerable<ActivityInfo> Activity(int uid, int maximumRows, int startRowIndex)
        {
            var q = from a in DbUtil.Db.ActivityLogs
                    where a.UserId == uid || uid == 0
                    select a;
            _count = q.Count();
            var q2 = from a in q
                     orderby a.ActivityDate descending
                     select new ActivityInfo
                     {
                         Activity = a.Activity,
                         Url = a.PageUrl,
                         Date = a.ActivityDate.Value,
                         Name = a.User.Name2,
                         UserId = a.UserId.Value,
                         Username = a.User.Username,
                     };
            return q2.Skip(startRowIndex).Take(maximumRows);
        }
    }
}
