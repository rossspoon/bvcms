using System;
using DiscData;

public partial class Admin_BlogCategories : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        var bc = new BlogCategory();
        bc.Category = TextBox1.Text;
        bc.BlogPostId = 1;
        DbUtil.Db.BlogCategories.InsertOnSubmit(bc);
        DbUtil.Db.SubmitChanges();
    }
}
