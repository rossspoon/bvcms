using System;
using DiscData;
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
        var cat = DbUtil.Db.Categories.SingleOrDefault(ca => ca.Name == TextBox1.Text);
        if(cat == null)
        {
            var bc = new DiscData.Category { Name = TextBox1.Text };
            DbUtil.Db.Categories.InsertOnSubmit(bc);
            DbUtil.Db.SubmitChanges();
        }
    }
}
