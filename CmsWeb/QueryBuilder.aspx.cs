/* Author: David Carroll
 * Copyright (c) 2008, 2009 Bellevue Baptist Church 
 * Licensed under the GNU General Public License (GPL v2)
 * you may not use this code except in compliance with the License.
 * You may obtain a copy of the License at http://bvcms.codeplex.com/license 
 */
using System;
using System.Web.UI.WebControls;
using CMSPresenter;
using UtilityExtensions;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.Configuration;
using System.Web.Services;
using System.Web;
using CmsData;

namespace CMSWeb
{
    public partial class QueryBuilder
    {
        private QueryBuilderController QueryControl { get; set; }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            //ExistingQueries.DataSource = CVController.UserQueries();
            //ExistingQueries.DataBind();
            if (!IsPostBack)
            {
                var id = Page.QueryString<int?>("id");
                if (id.HasValue)
                    QueryId = id;
            }
            QueryControl = new QueryBuilderController(this);
            ExportToolBar1.queryId = QueryId.Value;
            QueryControl.UpdateExpressionView();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Util.OrgMembersOnly)
            {
                DbUtil.LogActivity("Trying to visit QB");
                Response.EndShowMessage("You do not have sufficient privileges to do this");
            }
            var site = (CMSWeb.Site)Page.Master;
            site.ScriptManager.EnablePageMethods = true;

            if (!site.ScriptManager.IsInAsyncPostBack)
            {
                site.SetQueryBuilder();
                ExportToolBar1.TaggedEvent += new EventHandler(ExportToolBar1_TaggedEvent);
            }
            ConditionSelection.Selected += new QueryConditions.ConditionSelectHandler(ConditionSelection_Selected);

            if (!IsPostBack)
            {
                if (this.QueryString<string>("run") == "true")
                {
                    DbUtil.LogActivity("Running Query");
                    BindDisplayGrid();
                }
                else
                    DbUtil.LogActivity("Visiting QueryBuilder");
            }
            if (IsPostBack && Session.IsNewSession)
                Response.Redirect("~/QueryBuilder.aspx");
        }

        void ConditionSelection_Selected(string field)
        {
            FieldText = field;
            QueryControl.UpdateExpressionView();
        }

        void ExportToolBar1_TaggedEvent(object sender, EventArgs e)
        {
            BindDisplayGrid();
        }
        protected void ObjectCreating(object sender, ObjectDataSourceEventArgs e)
        {
            e.ObjectInstance = QueryControl;
        }
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            SavedQueryName.ToolTip = QueryId.ToString();
        }
        protected void RunQuery_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                DbUtil.LogActivity("Running Query");
                BindDisplayGrid();
            }
        }
        private void BindDisplayGrid()
        {
            ResultsVisible = true;
            PersonGrid1.DataBind();
        }

        private void ReBindDivOrg()
        {
            if (SubDivOrg.Visible)
            {
                SubDivOrg.DataSource = CVController
                    .OrgSubDivTags(DivOrg.SelectedValue.ToInt());
                SubDivOrg.DataBind();
            }
            ReBindOrg();
        }
        protected void DivOrg_SelectedIndexChanged(object o, EventArgs e)
        {
            DivOrg.Items.FindByValue("0").Enabled = false;
            Organization.SelectedIndex = -1;
            SubDivOrg.SelectedIndex = -1;
            ReBindDivOrg();
        }

        private void ReBindOrg()
        {
            if (Organization.Visible)
            {
                Organization.DataSource = CVController
                    .Organizations(SubDivOrg.SelectedValue.ToInt());
                Organization.DataBind();
            }
        }
        protected void SubDivOrg_SelectedIndexChanged(object o, EventArgs e)
        {
            Organization.SelectedIndex = -1;
            ReBindOrg();
        }

        protected void Comparison_SelectedIndexChanged(object o, EventArgs e)
        {
            SelectMultiple = Regex.Match(ComparisonText, "OneOf|NotOneOf").Success; ;
        }
        protected void QueryGridCommand(object sender, GridViewCommandEventArgs e)
        {
            var i = e.CommandArgument.ToInt();
            switch (e.CommandName)
            {
                case "insgroup":
                    DbUtil.LogActivity("Inserting Group", false);
                    QueryControl.InsertGroupAbove(i);
                    ResultsVisible = false;
                    break;
                case "edit":
                    SelectedId = i;
                    QueryControl.EditClause(i);
                    break;
                case "copyasnew":
                    DbUtil.LogActivity("Copying as New", false);
                    QueryControl.CopyAsNew(i);
                    Response.Redirect("~/QueryBuilder.aspx");
                    break;
            }
            ConditionGrid.DataBind();
        }
        protected void Update_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;
            ResultsVisible = false;
            if (!QueryControl.UpdateClause(SelectedId))
                Response.Redirect("~/QueryBuilder.aspx"); // something went wrong
            ConditionGrid.DataBind();
        }
        protected void Add_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;
            ResultsVisible = false;
            QueryControl.AddConditionAfterCurrent(SelectedId);
            ConditionGrid.DataBind();
        }
        protected void Remove_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;
            ResultsVisible = false;
            if (!QueryControl.DeleteCondition(SelectedId))
                Response.Redirect("~/QueryBuilder.aspx"); // something went wrong
            ConditionGrid.DataBind();
        }
        protected void AddToGroup_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;
            ResultsVisible = false;
            QueryControl.AddConditionToGroup(SelectedId);
            ConditionGrid.DataBind();
        }

        [System.Web.Services.WebMethod]
        public static string ToggleTag(int PeopleId, string controlid)
        {
            return MyTags.ToggleTag(PeopleId, controlid);
        }

        protected void OpenQuery(object sender, EventArgs e)
        {
            Response.Redirect("~/QueryBuilder.aspx?id=" + ExistingQueries.SelectedValue);
        }

        protected void SaveQuery(object sender, EventArgs e)
        {
            QueryControl.SaveQuery();
            ExistingQueries.DataBind();
        }

        protected void ValidateStartDate_ServerValidate(object source, ServerValidateEventArgs args)
        {
            DateTime sd;
            args.IsValid = !StartDate.Visible || DateTime.TryParse(StartDate.Text, out sd);
        }

        protected void ValidateEndDate_ServerValidate(object source, ServerValidateEventArgs args)
        {
            DateTime sd;
            args.IsValid = !EndDate.Visible
                || !EndDate.Text.HasValue()
                || DateTime.TryParse(EndDate.Text, out sd);
        }

        protected void ValidateDays_ServerValidate(object source, ServerValidateEventArgs args)
        {
            int i;
            args.IsValid = !Days.Visible || int.TryParse(Days.Text, out i);
        }
        protected void ValidateWeek_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = !Week.Visible || Regex.IsMatch(Week.Text, "^(1|2|3|4|5)(,(1|2|3|4|5))*$");
        }

        protected void ValidateQuarters_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = !Quarters.Visible || Regex.IsMatch(Quarters.Text, "^(1|2|3|4)(,(1|2|3|4))*$");
        }

        protected void ValidateSavedQuery_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = !SavedQueryDesc.Visible || SavedQueryDesc.SelectedItem != null;
        }

        protected void ValidateTags_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = !Tags.Visible || Tags.Text.HasValue();
        }

        protected void ValidateRightSide_ServerValidate(object source, ServerValidateEventArgs args)
        {
            string message = null;
            args.IsValid = true; // assume this to begin
            if (ValueText.Visible)
            {
                int i;
                decimal d;
                switch (HiddenTextType.Value)
                {
                    case "i":
                        args.IsValid = Comparison.SelectedItem.Text.Contains("Null")
                            || int.TryParse(ValueText.Text, out i);
                        message = "invalid integer";
                        break;
                    case "n":
                        args.IsValid = Comparison.SelectedItem.Text.Contains("Null")
                            || decimal.TryParse(ValueText.Text, out d);
                        message = "invalid number";
                        break;
                }
            }
            else if (ValueCheckCodes.Visible)
            {
                args.IsValid = ValueCheckCodes.Text.HasValue();
                message = "must select items";
            }
            else if (ValueDate.Visible)
            {
                DateTime sd;
                args.IsValid = DateTime.TryParse(ValueDate.Text, out sd);
                message = "invalid date";
            }
            ValidateRightSide.ErrorMessage = message;
        }
    }
}
