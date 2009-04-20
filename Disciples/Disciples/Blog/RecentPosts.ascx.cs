using System;

public partial class RecentPosts : System.Web.UI.UserControl
{
    private int _BlogId;
    public int BlogId
    {
        get { return _BlogId; }
        set { _BlogId = value; }
    }
    protected override void OnLoad(EventArgs e)
    {
        ObjectDataSource2.SelectParameters["blogid"].DefaultValue = BlogId.ToString();
        base.OnLoad(e);
    }
}
