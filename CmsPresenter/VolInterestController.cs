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
    [DataObject]
    public class VolInterestController
    {
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public IEnumerable<CmsData.View.VolInterestView> FetchList(int startRowIndex, int maximumRows)
        {
            var query = from i in DbUtil.Db.ViewVolInterestViews 
                        orderby i.Opportunity, i.Created descending 
                        select i;
            count = query.Count();
            return query.Skip(startRowIndex).Take(maximumRows);
        }
        private int count;
        public int Count(int startRowIndex, int maximumRows)
        {
            return count;
        }
    }
}
