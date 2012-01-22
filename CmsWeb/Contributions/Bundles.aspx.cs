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
using System.Web.UI;
using System.Web.UI.WebControls;
using CMSPresenter;
using UtilityExtensions;
using CmsData;

namespace CmsWeb.Contributions
{
   public partial class Bundles : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DataPager1.PageSize = UtilityExtensions.Util.GetPageSizeCookie();
        }

        protected void NewBundle_Click(object sender, EventArgs e)
        {
            var ctl = new BundleController();
            var b = ctl.NewBundle();
            Response.Redirect("~/Contributions/Bundle.aspx?id={0}&edit=1".Fmt(b.BundleHeaderId));
        }
    }
}
