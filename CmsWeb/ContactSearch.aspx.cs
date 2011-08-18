using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using UtilityExtensions;
using CMSPresenter;
using System.Web.Configuration;
using CmsData;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text;
using System.Data.Linq;

namespace CmsWeb
{
    public partial class ContactSearch : System.Web.UI.Page
    {
        private ContactSearchController ctrl = new ContactSearchController();

        protected void Page_Load(object sender, EventArgs e)
        {
            var site = (CmsWeb.Site)Page.Master;
            site.ScriptManager.EnablePageMethods = true;
            if (!IsPostBack)
            {
                GridPager.SetPageSize(ContactGrid);
            }
        }

        protected void ContactData_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e.ReturnValue is int)
                GridCount.Text = e.ReturnValue.ToInt().ToString("N0");
        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            ContactGrid.Visible = true;
        }

        protected void NewSearch_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ContactSearch.aspx");
        }

        protected string GetContacteeList(int ContactId)
        {
            return ctrl.GetContacteeList(ContactId);
        }
        protected void QuerySearch_Click(object sender, EventArgs e)
        {
            var qb = DbUtil.Db.QueryBuilderScratchPad();
            qb.CleanSlate(DbUtil.Db);
            var comp = CompareType.Equal;
            var clause = qb.AddNewClause(QueryType.MadeContactTypeAsOf, comp, "1,T");
            clause.Program = MinistryList.SelectedValue.ToInt();
            clause.StartDate = startDate.Text.ToDate() ?? DateTime.Parse("1/1/2000");
            clause.EndDate = endDate.Text.ToDate() ?? DateTime.Today;
            var cvc = new CodeValueController();
            var q = from v in cvc.ContactTypeCodes0()
                    where v.Id == TypeList.SelectedValue.ToInt()
                    select v.IdCode;
            var idvalue = q.Single();
            clause.CodeIdValue = idvalue;
            DbUtil.Db.SubmitChanges();
            Response.Redirect("/QueryBuilder/Main/{0}".Fmt(qb.QueryId));
        }
    }
}
