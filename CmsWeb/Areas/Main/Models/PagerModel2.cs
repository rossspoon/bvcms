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
using System.Web.Routing;
using UtilityExtensions;
using System.Text;
using CmsData;

namespace CMSWeb.Models
{
    public class PagerModel2
    {
        public PagerModel2(CountDelegate count)
        {
            GetCount = new CountDelegate(count);
        }
        public string Sort { get; set; }
        public string Direction { get; set; }
        public delegate int CountDelegate();
        private CountDelegate GetCount;
        
        public int PageSize
        {
            get { return DbUtil.Db.UserPreference("PageSize", "10").ToInt(); }
            set { DbUtil.Db.SetUserPreference("PageSize", value); }
        }
        private int? _Page;
        public int? Page
        {
            get { return _Page ?? 1; }
            set { _Page = value; }
        }
        public int LastPage
        {
            get { return (int)Math.Ceiling(GetCount() / (double)PageSize); }
        }
        public int StartRow
        {
            get { return (Page.Value - 1) * PageSize; }
        }
        public IEnumerable<int> PageSizeList()
        {
            int[] pagesizes = { 10, 25, 50, 100, 200 };
            return pagesizes.AsEnumerable();
        }
        public IEnumerable<int> PageList()
        {
            for (var i = 1; i <= LastPage;i++ )
            {
                if (i > 1 && i < Page - 2)
                {
                    i = Page.Value - 3;
                    yield return 0;
                }
                else if (i < LastPage && i > Page + 2)
                {
                    i = LastPage - 1;
                    yield return 0;
                }
                else
                    yield return i;
            }
        }
    }
}
