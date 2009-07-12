/* Author: David Carroll
 * Copyright (c) 2008, 2009 Bellevue Baptist Church 
 * Licensed under the GNU General Public License (GPL v2)
 * you may not use this code except in compliance with the License.
 * You may obtain a copy of the License at http://bvcms.codeplex.com/license 
 */
using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Text;
using UtilityExtensions;
using CMSPresenter;
using CmsData;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Collections.Generic;
using System.Web;

namespace CMSWeb
{
    public partial class EditMemberDialog : System.Web.UI.Page
    {
        public CmsData.OrganizationMember OrgMember;

        private string from;
        protected void Page_Load(object sender, EventArgs e)
        {
            var oid = Page.QueryString<int?>("oid");
            var pid = Page.QueryString<int?>("pid");
            from = Page.QueryString<string>("from");
            if (!oid.HasValue || !pid.HasValue || !from.HasValue())
                throw new Exception("Cannot visit EditMemberDialog this way");
            OrgMember = DbUtil.Db.OrganizationMembers.SingleOrDefault(om =>
                om.OrganizationId == oid && om.PeopleId == pid);
            if (OrgMember == null)
            {
                this.Page.ClientScript.RegisterStartupScript(typeof(EditMemberDialog),
                    "closeThickBox", "self.parent.RebindMemberGrids('{0}');".Fmt(from), true);
                return;
            }
            Name.Text = OrgMember.Person.Name;
            EditUpdateButton1.Enabled = Page.User.IsInRole("Attendance");
            Delete.Enabled = EditUpdateButton1.Enabled;
            EditUpdateButton1.DataBind();
            AttendString.NavigateUrl = "~/AttendStrDetail.aspx?id={0}&oid={1}"
                .Fmt(OrgMember.PeopleId, OrgMember.OrganizationId);
            AttendString.Text = OrgMember.AttendStr;
            AttendString.Target = "_blank";
            Groups.Visible = EditUpdateButton1.Editing == false;
        }
        protected void EditUpdateButton1_Click(object sender, EventArgs e)
        {
            if (EditUpdateButton1.Updating && EditUpdateButton1.Changes > 0)
            {
                // no need to create transaction for inactiveDate change only
                if (MemberTypeId.HadBeenChanged)
                {
                    var et = new EnrollmentTransaction
                    {
                        OrganizationId = OrgMember.OrganizationId,
                        PeopleId = OrgMember.PeopleId,
                        MemberTypeId = OrgMember.MemberTypeId,
                        OrganizationName = OrgMember.Organization.OrganizationName,
                        TransactionDate = Util.Now,
                        TransactionTypeId = 3,// change
                        VipWeek1 = OrgMember.VipWeek1,
                        VipWeek2 = OrgMember.VipWeek2,
                        VipWeek3 = OrgMember.VipWeek3,
                        VipWeek4 = OrgMember.VipWeek4,
                        VipWeek5 = OrgMember.VipWeek5,
                        Pending = Pending.Checked,
                        AttendancePercentage = OrgMember.AttendPct
                    };
                    DbUtil.Db.EnrollmentTransactions.InsertOnSubmit(et);
                }
                DbUtil.Db.SubmitChanges();
                EditUpdateButton1.DataBind();
                this.Page.ClientScript.RegisterStartupScript(typeof(EditMemberDialog),
                    "closeThickBox", "self.parent.RebindMemberGrids('{0}');".Fmt(from), true);
            }
        }

        protected void Delete_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            var pid = OrgMember.PeopleId;
            OrgMember.Drop();
            DbUtil.Db.SubmitChanges();
            OrganizationMember.UpdateMeetingsToUpdate();
            DbUtil.Db.UpdateSchoolGrade(pid);

            this.Page.ClientScript.RegisterStartupScript(typeof(EditMemberDialog),
               "closeThickBox", "self.parent.RebindMemberGrids('{0}');".Fmt(from), true);
        }
        protected void Groups_DataBound(object sender, EventArgs e)
        {
            var cbl = sender as CheckBoxList;
            foreach (ListItem li in cbl.Items)
                li.Selected = OrgMember.OrgMemMemTags.Any(mt => mt.MemberTagId == li.Value.ToInt());
        }

        protected void UpdateGroups_Click(object sender, EventArgs e)
        {
            var mlist = OrgMember.OrgMemMemTags.ToList();
            var clist = new List<int>();
            for (var i = 0; i < Groups.Items.Count; i++)
            {
                var item = Groups.Items[i] as ListItem;
                if (item.Selected)
                    clist.Add(item.Value.ToInt());
            }
            var deletes = mlist.Where(g => !clist.Contains(g.MemberTagId));
            DbUtil.Db.OrgMemMemTags.DeleteAllOnSubmit(deletes);
            var mlist2 = mlist.Select(m => m.MemberTagId).ToList();
            var adds = clist.Where(g => !mlist2.Contains(g));
            foreach (var i in adds)
            {
                var mt = new OrgMemMemTag
                {
                    MemberTagId = i,
                    PeopleId = OrgMember.PeopleId,
                    OrgId = OrgMember.OrganizationId
                };
                DbUtil.Db.OrgMemMemTags.InsertOnSubmit(mt);
            }
            DbUtil.Db.SubmitChanges();
            this.Page.ClientScript.RegisterStartupScript(typeof(EditMemberDialog),
              "closeThickBox", "self.parent.RebindMemberGrids('{0}');".Fmt(from), true);
        }
    }
}
