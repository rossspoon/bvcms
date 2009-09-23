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

namespace CMSWeb.Dialog
{
    public partial class Search : System.Web.UI.Page
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            GridPager.SetPageSize(GridView1);
        }

        PersonSearchController ctl = new PersonSearchController();
        string parentButtonId;
        public bool selectSingle;

        protected void Page_Load(object sender, EventArgs e)
        {
            ctl.TagTypeId = DbUtil.TagTypeId_AddSelected;
            ctl.TagOwner = Util.UserPeopleId;
            ctl.TagName = Util.SessionId;
            if (!IsPostBack)
            {
                var t = DbUtil.Db.FetchOrCreateTag(Util.SessionId, ctl.TagOwner, ctl.TagTypeId);
                DbUtil.Db.TagPeople.DeleteAllOnSubmit(t.PersonTags);
                DbUtil.Db.SubmitChanges();
            }
            parentButtonId = this.QueryString<string>("parentButton");
            Parameters.SearchButtonClicked += new EventHandler(SearchButton_Click);
            Parameters.ClearButtonClicked += new EventHandler(Parameters_ClearButtonClicked);
            selectSingle = this.QueryString<string>("selectsingle").HasValue();
            var users = this.QueryString<int?>("usersonly");
            UsersOnly.Value = (users.HasValue && users.Value > 0).ToString();
        }

        void Parameters_ClearButtonClicked(object sender, EventArgs e)
        {
            GridView1.Visible = false;
            GridCount.Text = "";
        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            GridView1.Visible = true;
            GridView1.DataBind();
        }

        protected void PeopleData_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e.ReturnValue is int)
                GridCount.Text = "{0:N0}".Fmt(e.ReturnValue);
        }
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow)
                return;
            var r = e.Row.DataItem as TaggedPersonInfo;
            string text = "ID: {0}\nMobile Phone: {1}\nWork Phone: {2}\nBirthDate: {3}\nStatus: {4}"
                .Fmt(r.PeopleId, r.CellPhone, r.WorkPhone, r.BirthDate, r.MemberStatus);
            e.Row.Attributes.Add("title", text);
        }

        [System.Web.Services.WebMethod]
        public static string ToggleTag(int PeopleId, string controlid)
        {
            var Db = DbUtil.Db;
            var r = new ToggleTagReturn { ControlId = controlid };
            var tag = Db.FetchOrCreateTag(Util.SessionId, Util.UserPeopleId, DbUtil.TagTypeId_AddSelected);
            var tagp = Db.TagPeople.SingleOrDefault(tp => tp.PeopleId == PeopleId && tp.Id == tag.Id);
            r.HasTag = tagp == null;
            if (tagp == null)
                tag.PersonTags.Add(new TagPerson { PeopleId = PeopleId });
            else
                Db.TagPeople.DeleteOnSubmit(tagp);
            Db.SubmitChanges();
            return JsonReturnStr(r);
        }
        internal static string JsonReturnStr(ToggleTagReturn r)
        {
            var jss = new DataContractJsonSerializer(typeof(ToggleTagReturn));
            var ms = new MemoryStream();
            jss.WriteObject(ms, r);
            var s = Encoding.Default.GetString(ms.ToArray());
            return s;
        }

        public static IQueryable<Person> SelectedPeople()
        {
            var tag = DbUtil.Db.FetchOrCreateTag(Util.SessionId, Util.UserPeopleId, DbUtil.TagTypeId_AddSelected);
            return DbUtil.Db.People.Where(p => p.Tags.Any(t => t.Id == tag.Id));
        }
        protected void PeopleData_ObjectCreating(object sender, ObjectDataSourceEventArgs e)
        {
            e.ObjectInstance = ctl;
        }
        private string AddSelectedScript
        {
            get { return "parent.AddSelected('{0}');".Fmt(parentButtonId); }
        }
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "select")
                ToggleTag(e.CommandArgument.ToInt(), "");
            Page.ClientScript.RegisterStartupScript(typeof(Search), "return", AddSelectedScript, true);
        }

    }
}
