using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using CmsData;
using UtilityExtensions;

public partial class Category : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        UserId.Value = User.Identity.Name;
        ((Disciples.Site)Master).AddCrumb("Verses", "~/Verse/Default.aspx")
            .Add("Categories", "~/Verse/Category.aspx");
    }

    protected internal void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string Owner = DataBinder.Eval(e.Row.DataItem, "Username").ToString();
            string user = User.Identity.Name;
            bool isOwner = Owner == user || user == "admin" || User.IsInRole("Administrator");
            if ((e.Row.RowState & DataControlRowState.Edit) == 0)
            {
                ((ImageButton)e.Row.FindControl("Delete")).Visible = isOwner;
                ((ImageButton)e.Row.FindControl("Rename")).Visible = isOwner;
            }
        }
    }

    protected internal void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Copy")
        {
            int id = e.CommandArgument.ToInt();
            VerseCategory.LoadById(id).CopyWithVerses();
            DbUtil.Db.SubmitChanges();
            GridView1.DataBind();
        }
    }
    protected void AddNewCategory_Click(object sender, EventArgs e)
    {
        new VerseCategoryController().Insert("A New Category");
        GridView1.DataBind();
    }

}
