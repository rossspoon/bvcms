using System;
using CmsData;

public partial class ForumEntryDisplay : System.Web.UI.UserControl
{
    public ForumEntry Entry
    {
        set
        {
            PostBody.Text = value.Entry;
            PostTitle.Text = value.Title;
            UserName.Text = value.User.Username;
            PubDate.Text = value.CreatedOn.Value.ToString();
        }
    }
    protected void Page_Load(System.Object sender, EventArgs e)
    {
    }
}
