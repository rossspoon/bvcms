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
using System.Web.Mvc;

namespace CMSWeb.Models
{
    public class MinistryModel
    {
        public int? MinistryId
        {
            get
            {
                return ministry.MinistryId;
            }
            set
            {
                ministry = DbUtil.Db.Ministries.SingleOrDefault(m => m.MinistryId == value);
            }
        }
        public Ministry ministry { get; set; }

        public int InsertMinistry()
        {
            var m = new Ministry { MinistryName = "new" };
            DbUtil.Db.Ministries.InsertOnSubmit(m);
            DbUtil.Db.SubmitChanges();
            return m.MinistryId;
        }
        public void DeleteMinistry(int id)
        {
            var min = DbUtil.Db.Ministries.SingleOrDefault(m => m.MinistryId == id);
            if (min != null)
            DbUtil.Db.Ministries.DeleteOnSubmit(min);
            DbUtil.Db.SubmitChanges();
        }
    }
}
