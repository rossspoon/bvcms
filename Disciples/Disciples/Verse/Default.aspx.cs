using System;
using System.Web;
using System.Web.UI.WebControls;
using DiscData;
using CustomControls;
using UtilityExtensions;

public partial class Verse_Default : System.Web.UI.Page
{
    string user;
    VerseCategory cat = null;
    protected override void OnInit(EventArgs e)
    {
        Category.PostDataLoaded += new EventHandler(Category_PostDataLoaded);
        user = DbUtil.Db.CurrentUser.Username;
        BindCategories();
        base.OnInit(e);
    }
    void Category_PostDataLoaded(object sender, EventArgs e)
    {
        cat = VerseCategory.LoadById(Category.SelectedValue.ToInt());
//        VerseController.CategoryInContext = cat;
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        ((BellevueTeachers.Site)Master).AddCrumb("Verses", "/Verse/");
        if (cat == null)
            cat = VerseCategory.LoadById(Category.SelectedValue.ToInt());
    }
    protected override void OnPreRender(EventArgs e)
    {
        AddVerse.NavigateUrl = "/Verse/Add.aspx?id=" + Category.SelectedValue;
        SelectVerses.NavigateUrl = "/Verse/All.aspx?cat=" + Category.SelectedValue;
        bool owner = IsOwner;
        SelectVerses.Enabled = owner;
        AddVerse.Enabled = owner;
        base.OnPreRender(e);
    }
    private void BindCategories()
    {
        VerseCategory c = null;
        int? catid = Request.QueryString<int?>("cat");
        if (!Page.IsPostBack && catid.HasValue)
        {
            c = VerseCategory.LoadById(catid.Value);
            if (cat != null)
                IncludeAdmin.Checked = cat.User.Username == "admin";
        }
        Category.DataSource = new VerseCategoryController().GetCategoriesForOwner(
            Request.Form[IncludeAdmin.UniqueID] == "on", user);
        Category.DataBind();
        if (!Page.IsPostBack)
            if (c != null)
                Category.Items.FindByValue(c.Id.ToString()).Selected = true;
            else if (Request.Cookies["preferences"] != null)
            {
                HttpCookie cookie = Request.Cookies["preferences"];
                ListItem item = Category.Items.FindByValue(cookie.Values["cat"]);
                if (item != null)
                    item.Selected = true;
            }
    }

    public bool IsOwner
    {
        get { return cat.CreatedBy == DbUtil.Db.CurrentUser.UserId || user == "admin"; }
    }

    protected void Category_SelectedIndexChanged(object sender, EventArgs e)
    {
        HttpCookie cookie = new HttpCookie("preferences");
        cookie.Values["cat"] = cat.Id.ToString();
        cookie.Expires = DateTime.MaxValue;
        Response.AppendCookie(cookie);
    }
}
