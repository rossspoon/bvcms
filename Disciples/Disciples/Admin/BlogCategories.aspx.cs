using System;
using CmsData;
using System.Linq;
using System.Data.Linq;
using System.Collections.Generic;

public partial class Admin_BlogCategories : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        var cat = DbUtil.Db.BlogCategories.SingleOrDefault(ca => ca.Name == TextBox1.Text);
        if(cat == null)
        {
            var bc = new BlogCategory { Name = TextBox1.Text };
            DbUtil.Db.BlogCategories.InsertOnSubmit(bc);
            DbUtil.Db.SubmitChanges();
        }
    }
}
