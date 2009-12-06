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
using System.Collections.Generic;
using CmsData;
using UtilityExtensions;

public partial class All : System.Web.UI.Page
{
    VerseCategory cat;
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        GridView1.DataKeyNames = new string[] { "id" };
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        cat = VerseCategory.LoadById(Request.QueryString<int>("cat"));
        var site = (Disciples.Site)Page.Master;

        site.AddCrumb("Verses", "~/Verse/Default.aspx?cat={0}", cat.Id)
            .Add("Select Verses");
        if (Page.IsPostBack)
            RememberChecks();
        GridView1.DataBound += new EventHandler(GridView1_DataBound);
        Category.Text = cat.Name;
    }

    void GridView1_DataBound(object sender, EventArgs e)
    {
        RePopulateChecks();
    }

    private const string checks = "checks";
    private Dictionary<int, bool> CheckList
    {
        get
        {
            if (ViewState[checks] != null)
                return ViewState[checks] as Dictionary<int, bool>;
            else
                return (ViewState[checks] = new Dictionary<int, bool>()) as Dictionary<int, bool>;
        }
    }
    private void RememberChecks()
    {
        foreach (GridViewRow row in GridView1.Rows)
        {
            int id = (int)GridView1.DataKeys[row.RowIndex].Value;
            CheckBox c = (CheckBox)row.FindControl("CheckBox1");
            CheckList[id] = c.Checked;
        }
    }
    private void RePopulateChecks()
    {
        if (CheckList.Count > 0)
            foreach (GridViewRow row in GridView1.Rows)
            {
                int id = (int)GridView1.DataKeys[row.RowIndex].Value;
                if (CheckList.ContainsKey(id))
                {
                    CheckBox c = (CheckBox)row.FindControl("CheckBox1");
                    c.Checked = CheckList[id];
                }
            }
    }
    protected void SaveChanges_Click(object sender, EventArgs e)
    {
        foreach (KeyValuePair<int, bool> CheckBox in CheckList)
        {
            bool verseInCat = cat.HasVerse(CheckBox.Key);
            if (!verseInCat && CheckBox.Value)
                VerseCategoryXref.InsertOnSubmit(cat.Id, CheckBox.Key);
            if (verseInCat && !CheckBox.Value)
                VerseCategoryXref.DeleteOnSubmit(cat.Id, CheckBox.Key);
        }
        DbUtil.Db.SubmitChanges();
        Response.Redirect("~/Verse/?cat=" + cat.Id);
    }
}
