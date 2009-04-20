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

namespace CMSWeb.Contributions
{
    public partial class Years : System.Web.UI.Page
    {
        public int? id;
        protected void Page_Load(object sender, EventArgs e)
        {
            id = this.QueryString<int?>("id");
            var person = DbUtil.Db.People.SingleOrDefault(p => p.PeopleId == id);
            if (person == null)
                Response.EndShowMessage("no person");

            PersonLink.Text = person.Name;
            PersonLink.NavigateUrl = "~/Person.aspx?id=" + person.PeopleId;

            hlContributionStatement.Text = "Contribution Statement";
            //hlContributionStatement.NavigateUrl = "~/Contributions/Reports/ContributionStatementRpt.aspx?id=" + person.PeopleId + 
            //                                      "&fdate=" + Util.Now.Date.Date.AddDays(-270).ToShortDateString() + 
            //                                      "&tdate=" + Util.Now.Date.Date.ToShortDateString() +
            //                                      "&typ=1";
            litContributionStatement.Visible = true;

            if (!IsPostBack)
            {
                var intQtr = (Util.Now.Date.Month) / 3 + 1;

                if (intQtr == 1)
                {
                    FromDate.Text = new DateTime(Util.Now.Date.Year - 1, 1, 1).ToShortDateString();
                    ToDate.Text = new DateTime(Util.Now.Date.Year - 1, 12, 31).ToShortDateString();
                }
                else
                {
                    FromDate.Text = new DateTime(Util.Now.Date.Year, 1, 1).ToShortDateString();
                    ToDate.Text = (new DateTime(Util.Now.Date.Year, ((intQtr - 1) * 3), 1)).AddMonths(1).AddDays(-1).ToShortDateString();
                }

            }
        }
    }
}
