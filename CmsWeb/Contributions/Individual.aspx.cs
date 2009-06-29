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
using CmsData;
using UtilityExtensions;
using CMSPresenter;

namespace CMSWeb.Contributions
{
    public partial class Individual : System.Web.UI.Page
    {
        int peopleid;
        protected void Page_Load(object sender, EventArgs e)
        {
            var id = this.QueryString<int?>("id");
            var person = DbUtil.Db.People.SingleOrDefault(p => p.PeopleId == id);
            if (person == null)
                Response.EndShowMessage("no person");
            peopleid = id.Value;
            PersonLink.Text = person.Name;
            PersonLink.NavigateUrl = "~/Contributions/Years.aspx?id=" + person.PeopleId;
            int sz = Util.GetPageSizeCookie();
            DataPager1.PageSize = sz;
            DataPager2.PageSize = sz;
        }

        protected void ListView1_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName.StartsWith("Re"))
            {
                var id = e.CommandArgument.ToInt();
                var c = DbUtil.Db.Contributions.Single(ic => ic.ContributionId == id);
                var now = Util.Now;
                var r = new Contribution
                {
                    ContributionStatusId = (int)Contribution.StatusCode.Recorded,
                    CreatedBy = Util.UserId1,
                    CreatedDate = now,
                    PeopleId = c.PeopleId,
                    ContributionAmount = c.ContributionAmount,
                    ContributionDate = now.Date,
                    PostingDate = now,
                    FundId = c.FundId,
                };
                DbUtil.Db.Contributions.InsertOnSubmit(r);
                switch (e.CommandName)
                {
                    case "Reverse":
                        c.ContributionStatusId = (int)Contribution.StatusCode.Reversed;
                        r.ContributionTypeId = (int)Contribution.TypeCode.Reversed;
                        r.ContributionDesc = "Reversed Contribution Id = " + c.ContributionId;
                        break;
                    case "Return":
                        c.ContributionStatusId = (int)Contribution.StatusCode.Returned;
                        r.ContributionTypeId = (int)Contribution.TypeCode.ReturnedCheck;
                        r.ContributionDesc = "Returned Check for Contribution Id = " + c.ContributionId;
                        break;
                }
                DbUtil.Db.SubmitChanges();
                ListView1.DataBind();
            }
        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            ListView1.DataBind();
        }

        protected void YearSearch_DataBound(object sender, EventArgs e)
        {
            var year = this.QueryString<int?>("year");
            if (year.HasValue && !Page.IsPostBack)
            {
                var i = YearSearch.Items.FindByText(year.ToString());
                if (i != null)
                    YearSearch.SelectedIndex = YearSearch.Items.IndexOf(i);
            }
        }

        protected void ListView1_DataBound(object sender, EventArgs e)
        {
            var ctl = new BundleController();
            Total.Text = ctl.Total(peopleid,
                YearSearch.SelectedValue.ToInt(),
                StatusSearch.SelectedValue.ToInt(),
                TypeSearch.SelectedValue.ToInt(),
                FundSearch.SelectedValue.ToInt())
                .ToString("c");
        }

        protected void ListView1_ItemCreated(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                var r = e.Item as ListViewDataItem;
                var d = r.DataItem as ContributionInfo;
                if (d != null)
                {
                    var ctl = e.Item.FindControl("StatusLabel") as Label;
                    if (d.NotIncluded)
                        ctl.Style.Add("color", "Red");
                }
            }
        }

    }
}
