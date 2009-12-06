/* Author: David Carroll
 * Copyright (c) 2008, 2009 Bellevue Baptist Church 
 * Licensed under the GNU General Public License (GPL v2)
 * you may not use this code except in compliance with the License.
 * You may obtain a copy of the License at http://bvcms.codeplex.com/license 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using UtilityExtensions;
using CmsData;
using System.ComponentModel;
using System.Collections;

namespace CMSPresenter
{
    [DataObject]
    public class VolunteerAppController
    {
        public class AppInfo
        {
            public int Id { get; set; }
            public DateTime? AppDate { get; set; }
            public int? ThumbId { get; set; }
            public int? Docid { get; set; }
            public string Uploader { get; set; }
            public bool? IsDocument { get; set; }
        }
        private CMSDataContext Db;
        public VolunteerAppController()
        {
            Db = DbUtil.Db;
        }

        private int _count;

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public IEnumerable<AppInfo> AppForms(int pid)
        {
            var q = Db.VolunteerForms.Where(f => f.PeopleId == pid);
            _count = q.Count();
            q = q.OrderBy(f => f.AppDate);
            //q = q.Skip(startRowIndex).Take(maximumRows);
            var q2 = q.Select(f =>
                   new AppInfo
                   {
                       AppDate = f.AppDate,
                       Id = f.Id,
                       ThumbId = f.SmallId,
                       Docid = f.MediumId,
                       Uploader = f.Uploader.Person.Name,
                       IsDocument = f.IsDocument,
                   });
            return q2;
        }

        public int Count(int pid)
        {
            return _count;
        }
    }
}
