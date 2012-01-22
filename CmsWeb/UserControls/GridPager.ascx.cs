using System;
using System.Web;
using System.Web.UI.WebControls;
using UtilityExtensions;

namespace CmsWeb
{
    public partial class GridPager : System.Web.UI.UserControl
    {
        private const string STR_Preferences = "Preferences";
        private const string STR_PageSize = "PageSize";

        private GridView Grid;
        private GridViewRow Row;
        public bool nodisable { get; set; }

        public event CommandEventHandler GoNextPrev;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Grid = Parent.Parent.Parent.Parent as GridView;
            Row = Parent.Parent as GridViewRow;
            Grid.DataBound += new EventHandler(Grid_DataBound);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (nodisable)
            {
                GoPrev.Attributes.Add("nodisable", "true");
                GoNext.Attributes.Add("nodisable", "true");
            }
        }
        void Grid_DataBound(object sender, EventArgs e)
        {
            //if (Grid.DataSource is PagedDataSource)
            //{
            //    var pds = Grid.DataSource as PagedDataSource;
            //    TotalNumberOfPages.Text = pds.PageCount.ToString();
            //}
            //else
            TotalNumberOfPages.Text = Grid.PageCount.ToString();
            Grid.DataBound -= new EventHandler(Grid_DataBound);
            GoToPage.Text = (Grid.PageIndex + 1).ToString();
            try
            {
                PageSize.SelectedValue = Grid.PageSize.ToString();
            }
            catch
            {
                PageSize.SelectedValue = "10";
            }
            Row.Visible = true;
        }

        protected void PageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            var dropDown = sender as DropDownList;
            Grid.PageSize = dropDown.SelectedValue.ToInt();

            HttpCookie cookie = new HttpCookie(STR_Preferences);
            cookie.Values[STR_PageSize] = Grid.PageSize.ToString();
            cookie.Expires = DateTime.MaxValue;
            Response.AppendCookie(cookie);
        }
        protected void GoToPage_TextChanged(object sender, EventArgs e)
        {
            var GoToPage = sender as TextBox;
            int n;
            if (int.TryParse(GoToPage.Text.Trim(), out n) && n > 0 && n <= Grid.PageCount)
                Grid.PageIndex = n - 1;
            else
                Grid.PageIndex = 0;
        }
        public static void SetPageSize(GridView grid)
        {
            if (grid.Page.Request.Cookies[STR_Preferences] != null)
            {
                var cookie = grid.Page.Request.Cookies[STR_Preferences];
                if (cookie.Values[STR_PageSize] != null)
                    grid.PageSize = cookie.Values[STR_PageSize].ToInt();
                else
                    grid.PageSize = 10;
            }
        }

        protected void Go_Command(object sender, CommandEventArgs e)
        {
            if (GoNextPrev != null)
                GoNextPrev(this, e);
        }
    }
}