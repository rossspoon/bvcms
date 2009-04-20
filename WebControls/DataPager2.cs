using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using UtilityExtensions;

namespace CustomControls
{
    public class DataPager2 : DataPager
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (this.Page.Request.Cookies[PagerField.STR_Preferences] != null)
            {
                var cookie = this.Page.Request.Cookies[PagerField.STR_Preferences];
                if (cookie.Values[PagerField.STR_PageSize] != null)
                    this.PageSize = cookie.Values[PagerField.STR_PageSize].ToInt();
                else
                    this.PageSize = 15;
            }
        }
    }
}
