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

namespace CmsWeb.Contributions.Reports
{
    public partial class TotalsByFund2 : System.Web.UI.Page
    {
        BundleController ctl = new BundleController();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var pledged = Page.QueryString<bool?>("pledged");
                if (pledged.HasValue && pledged.Value)
                    Label1.Text = "Pledge Totals by Fund";
                var from = this.QueryString<DateTime?>("from");
                var today = Util.Now.Date;
                var first = new DateTime(today.Year, today.Month, 1);
                if (today.Day < 8)
                    first = first.AddMonths(-1);
                if (!from.HasValue)
                    from = first;
                FromDate.Text = from.Value.ToString("d");
                ToDate.Text = from.Value.AddMonths(1).AddDays(-1).ToString("d");
            }
        }
        protected void ListView1_DataBound(object sender, EventArgs e)
        {
            var c = ListView1.FindControl("Count") as Label;
            var t = ListView1.FindControl("Total") as Label;
            if (ctl.FundTotal.Count.HasValue)
                c.Text = ctl.FundTotal.Count.Value.ToString("n0");
            if (ctl.FundTotal.Total.HasValue)
                t.Text = ctl.FundTotal.Total.Value.ToString("c");
        }

        protected void ObjectDataSource1_ObjectCreated(object sender, ObjectDataSourceEventArgs e)
        {
            e.ObjectInstance = ctl;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            ListView1.Visible = true;
        }

        protected void ListView1_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                var link = e.Item.FindControl("FundNameLink") as HyperLink;
                var r = e.Item as ListViewDataItem;
                var d = r.DataItem as FundTotalInfo;
                link.NavigateUrl = "~/Contributions/Reports/JournalDetails.aspx?FundId={0}&dt1={1}&dt2={2}"
                    .Fmt(d.FundId, FromDate.Text, ToDate.Text);
            }
        }

    }
}
