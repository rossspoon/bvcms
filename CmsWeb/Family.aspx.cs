/* Author: David Carroll
 * Copyright (c) 2008, 2009 Bellevue Baptist Church 
 * Licensed under the GNU General Public License (GPL v2)
 * you may not use this code except in compliance with the License.
 * You may obtain a copy of the License at http://bvcms.codeplex.com/license 
 */
using System;
using UtilityExtensions;
using CMSPresenter;
using System.Linq;
using CmsData;
using System.Drawing;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using CustomControls;
using System.Data.Linq;

namespace CMSWeb
{
    public partial class FamilyPage : System.Web.UI.Page
    {
        public Family family;
        public Person person;

        protected void Page_Load(object sender, EventArgs e)
        {
            int? id = this.QueryString<int?>("id");
            if (!id.HasValue)
                Response.EndShowMessage("no familyId supplied", "/", "home");
            family = DbUtil.Db.Families.SingleOrDefault(f => f.FamilyId == id.Value);
            if (family == null)
                Response.EndShowMessage("family not found", "/", "home");
            person = family.People.SingleOrDefault(p => p.PeopleId == family.HeadOfHouseholdId);
            if (person == null)
                person = family.People.FirstOrDefault();
            if (person != null)
            {
                FamilyPrimaryAddr.person = person;
                FamilyAltAddr.person = person;
                if(!IsPostBack)
                    DbUtil.LogActivity("Viewing Family for {0}".Fmt(person.Name));
            }

            bool canUpdate = User.IsInRole("Edit");
            AddRelatedFamily2.Visible = canUpdate;
            AddMembers2.Visible = canUpdate;
            DeleteFamily.Visible = User.IsInRole("Edit") && family.MemberCount == 0;

            FamilyPrimaryAddr.showPreferredAddress = false;

            FamilyAltAddr.showPreferredAddress = false;
            trAddressLineTwo.Visible = family.AddressLineTwo.HasValue();

            EditUpdateButton1.DataBind();
            AddMembers2.NavigateUrl = "~/Dialog/AddFamilyMembers.aspx?id={0}&TB_iframe=true&height=450&width=600".Fmt(family.FamilyId);
            AddRelatedFamily2.NavigateUrl = "~/Dialog/AddFamily.aspx?id={0}&TB_iframe=true&height=450&width=600".Fmt(family.FamilyId);
        }

        protected void GridView1_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var h = e.Row.FindControl("namelink") as HyperLink;
                var ed = e.Row.FindControl("Edit") as LinkButton;
                var l = e.Row.FindControl("btnSplit") as LinkButtonConfirm;
                l.Enabled = User.IsInRole("Edit");
                if (ed != null)
                    ed.Enabled = l.Enabled;
                var d = e.Row.DataItem as FamilyMember;
                l.CommandName = "split";
                l.CommandArgument = d.PeopleId.ToString();

                if (d.Deceased)
                    h.ForeColor = Color.Red;
            }
        }

        protected void RelatedFamilyGrid_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var h = e.Row.FindControl("familylink") as HyperLink;
                var l = e.Row.FindControl("btnRemoveRelation") as LinkButtonConfirm;
                var ed = e.Row.FindControl("Edit") as LinkButtonConfirm;
                var d = e.Row.DataItem as RelatedFamily;
                Family f;

                if (d.RelatedFamilyId == family.FamilyId)
                {
                    f = DbUtil.Db.Families.Single(g => g.FamilyId == d.FamilyId);
                    h.NavigateUrl = "~/Family.aspx?Id=" + d.FamilyId.ToString();
                    l.CommandArgument = d.FamilyId.ToString();
                }
                else
                {
                    f = DbUtil.Db.Families.Single(g => g.FamilyId == d.RelatedFamilyId);
                    h.NavigateUrl = "~/Family.aspx?Id=" + d.RelatedFamilyId.ToString();
                    l.CommandArgument = d.RelatedFamilyId.ToString();
                }
                h.Text = f.FamilyName;
                l.Enabled = User.IsInRole("Edit");
                if (ed != null)
                    ed.Enabled = l.Enabled;
            }
        }

        protected void RefreshGrids_Click(object sender, EventArgs e)
        {
            FamilyGrid.DataBind();
            RelatedFamilyGrid.DataBind();
        }

        protected void EditUpdateButton1_Click(object sender, EventArgs e)
        {
            if (EditUpdateButton1.Updating)
            {
                DbUtil.Db.SubmitChanges();
                EditUpdateButton1.DataBind();
            }
        }

        protected void btnSplit_Click(object sender, EventArgs e)
        {
            var l = sender as LinkButtonConfirm;
            var d = family.People.Single(p => p.PeopleId == l.CommandArgument.ToInt());
            var f = new Family
            {
                CreatedDate = DateTime.Now,
                CreatedBy = Util.UserId1,
                AddressLineOne = d.PrimaryAddress,
                AddressLineTwo = d.PrimaryAddress2,
                CityName = d.PrimaryCity,
                StateCode = d.PrimaryState,
                ZipCode = d.ZipCode
            };
            f.People.Add(d);
            DbUtil.Db.Families.InsertOnSubmit(f);
            DbUtil.Db.SubmitChanges();

            DbUtil.LogActivity("Splitting Family for {0}".Fmt(person.Name));
            Response.Redirect("~/Family.aspx?id=" + f.FamilyId);
        }

        protected void btnRemoveRelation_Click(object sender, EventArgs e)
        {
            var l = sender as LinkButtonConfirm;
            var d = family.RelatedFamilies1.SingleOrDefault(p => p.RelatedFamilyId == l.CommandArgument.ToInt());
            if (d == null)
                d = family.RelatedFamilies2.SingleOrDefault(p => p.FamilyId == l.CommandArgument.ToInt());
            DbUtil.Db.RelatedFamilies.DeleteOnSubmit(d);
            DbUtil.Db.SubmitChanges();
            if (person != null)
                DbUtil.LogActivity("Removing Related Family for {0}".Fmt(person.Name));
            Response.Redirect("~/Family.aspx?id=" + family.FamilyId);

        }

        protected void DeleteFamily_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            DbUtil.Db.RelatedFamilies.DeleteAllOnSubmit(family.RelatedFamilies1);
            DbUtil.Db.RelatedFamilies.DeleteAllOnSubmit(family.RelatedFamilies2);
            DbUtil.Db.Families.DeleteOnSubmit(family);
            DbUtil.Db.SubmitChanges();
            Response.EndShowMessage("Family Deleted", "/", "click here");
        }
    }
}
