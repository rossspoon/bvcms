using System;
using System.Web;
using DiscData;

public partial class Add : System.Web.UI.Page
{
    VerseCategory cat;

    protected void Page_Load(object sender, EventArgs e)
    {
        cat = VerseCategory.LoadById(Request.QueryString<int>("id"));
        ((BellevueTeachers.Site)Master).AddCrumb("Verses", "/Verse/Default.aspx?cat={0}", cat.Id)
            .Add("Add Verse");

        if (!Page.IsPostBack)
        {
            Version.DataSource = Verse.Versions;
            Version.DataTextField = "Key";
            Version.DataBind();
            Category.Text = cat.Name;

            HttpCookie c = Request.Cookies["version"];
            if (c!=null)
                Version.SelectedIndex = Version.Items.IndexOf(
                    Version.Items.FindByValue(c.Value));
        }
    }

    protected void AddVerse_Click(object sender, EventArgs e)
    {
        var v = Verse.Lookup(RefText.Text, Version.SelectedValue);
        RefBadValidator.IsValid = v != null;
        if (RefBadValidator.IsValid)
        {
            if (v.Id <= 0)
            {
                v.CreatedBy = Util.CurrentUser.UserId;
                v.CreatedOn = DateTime.Now;
            }
            cat.AddVerse(v);
            DbUtil.Db.SubmitChanges();
            HttpCookie cookie = new HttpCookie("version");
            cookie.Value = Version.SelectedValue;
            cookie.Expires = DateTime.MaxValue;
            Response.AppendCookie(cookie);
            Response.Redirect("/Verse/Default.aspx?cat=" + cat.Id);
        }
    }
    protected void Version_SelectedIndexChanged(object sender, EventArgs e)
    {
        VerseLit.Text = "";
    }
    protected void Preview_Click(object sender, EventArgs e)
    {
        var v = Verse.Lookup(RefText.Text, Version.SelectedValue);
        RefBadValidator.IsValid = v != null;
        if (RefBadValidator.IsValid)
        {
            RefText.Text = v.VerseRef;
            VerseLit.Text = v.VerseText;
        }
        else
            VerseLit.Text = "";
    }
    protected void Cancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("/Verse/Default.aspx?cat=" + cat.Id, true);
    }
}
