using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CmsData;
using UtilityExtensions;

namespace CmsWeb.Contributions
{
    public partial class All : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DataPager1.PageSize = Util.GetPageSizeCookie();
            DataPager2.PageSize = Util.GetPageSizeCookie();
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
    }
}
