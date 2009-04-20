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
using UtilityExtensions;
using CmsData;

namespace CMSWeb
{
    public partial class Meetings : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                var dt = Util.Now.Date;
                dt = dt.AddDays(-(int)dt.DayOfWeek); //sunday before today
                if ((int)Util.Now.Date.DayOfWeek > (int)DayOfWeek.Wednesday)
                    dt = dt.AddDays(3);
                MeetingDate.Text = dt.ToString("d");
                DbUtil.LogActivity("Viewing Meetings for {0:d}".Fmt(dt));
            }
        }
    }
}
