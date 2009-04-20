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

namespace CMSWeb
{
    public partial class ContactSearch : System.Web.UI.Page
    {
        private ContactSearchController ctrl = new ContactSearchController();

        protected void Page_Load(object sender, EventArgs e)
        {
            var site = (CMSWeb.Site)Page.Master;
            site.ScriptManager.EnablePageMethods = true;
            if (!IsPostBack)
            {
                GridPager.SetPageSize(ContactGrid);
            }
        }

        protected void ContactData_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e.ReturnValue is int)
                GridCount.Text = e.ReturnValue.ToString();
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

    }
}
