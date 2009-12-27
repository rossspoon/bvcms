using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using UtilityExtensions;
using CMSPresenter;

namespace CMSWeb
{
    public partial class MeetingGrid : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
                GridPager.SetPageSize(Grid);
            Grid.Columns[5].Visible = Page.User.IsInRole("Edit");
        }
        public override void DataBind()
        {
            Grid.DataBind();
        }
        public string DataSourceID
        {
            set { Grid.DataSourceID = value; }
        }
        public object DataSource
        {
            set { Grid.DataSource = value; }
        }
        public void DefaultSort()
        {
            Grid.Sort("MeetingDate", SortDirection.Descending);
        }
        public void DefaultSort2()
        {
            Grid.Sort("MeetingDate", SortDirection.Ascending);
        }
        public string GridClientID
        {
            get
            {
                return Grid.ClientID;
            }
        }

        protected void Grid_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            e.Cancel = !Page.User.IsInRole("Edit");
        }
        protected void ttt(object sender, GridViewCommandEventArgs e)
        {
        }
    }
}