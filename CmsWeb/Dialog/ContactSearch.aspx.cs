using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CMSPresenter;
using UtilityExtensions;

namespace CMSWeb.Dialog
{
    public partial class ContactSearch : System.Web.UI.Page
    {
        private ContactSearchController ctrl = new ContactSearchController();

        protected void Page_Load(object sender, EventArgs e)
        {
            DataPager1.PageSize = Util.GetPageSizeCookie();
            DataPager2.PageSize = Util.GetPageSizeCookie();
        }
        protected void SearchButton_Click(object sender, EventArgs e)
        {
            ListView1.Visible = true;
        }
        protected string GetContacteeList(int ContactId)
        {
            return ctrl.GetContacteeList(ContactId);
        }

        protected void AddNew1_Click(object sender, EventArgs e)
        {

        }
    }
}
