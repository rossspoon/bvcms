using System;
using System.Web.UI.WebControls;
using DiscData;
using System.Text.RegularExpressions;
using System.Linq;
using UtilityExtensions;

public partial class Forum_Thread : System.Web.UI.Page
{
    int? threadid;
    int? selected;
    protected void Page_Load(object sender, EventArgs e)
    {
        threadid = Request.QueryString<int?>("threadid");
        var entry = ForumEntry.LoadById(threadid.Value);
        if (entry == null || !entry.Forum.IsMember)
            Response.Redirect("/");

        ((BellevueTeachers.Site)Master).AddCrumb("Topics", "~/Forum/{0}.aspx", entry.ForumId)
            .Add("Thread", "~/Forum/Thread/{0}.aspx", threadid);

        selected = Request.QueryString<int?>("selected");
        if (!selected.HasValue)
            selected = entry.Id;
        if (!Page.IsPostBack)
            BindTree();
        else
            DisplayEntry();
        DbUtil.Db.SubmitChanges();
    }
    private void BindTree()
    {
        tree.Nodes.Clear();
        var q = DbUtil.Db.ForumEntries.Where(fe => fe.ThreadId == threadid.Value);

        var row = q.First();

        TreeNode node = NewTreeNode(row);
        tree.Nodes.Add(node);
        BindChildren(node, row);
        DisplayEntry();
        UpdateTreePanel.Update();
    }
    private void BindChildren(TreeNode pn, ForumEntry p)
    {
        foreach (ForumEntry c in p.Replies)
        {
            TreeNode cn = NewTreeNode(c);
            pn.ChildNodes.Add(cn);
            BindChildren(cn, c);
        }
    }
    private TreeNode NewTreeNode(ForumEntry r)
    {
        TreeNode cn = new TreeNode();
        SetText(cn, r.IsRead(DbUtil.Db.CurrentUser) ? SetTextParam.Read : SetTextParam.UnRead, r.Title, r.User);
        cn.Value = r.Id.ToString();
        if (selected == r.Id)
            cn.Select();
        return cn;
    }
    private void SetText(TreeNode n, SetTextParam rendering, string Title, User Poster)
    {
        string Text = Title + " (" + Poster.Username + ")";
        SetText(n, rendering, Text);
    }
    private void SetText(TreeNode n, SetTextParam rendering, string Text)
    {
        Text = Server.HtmlEncode(Regex.Replace(Text, "\\<.*?\\>", ""));
        switch (rendering)
        {
            case SetTextParam.Read:
                n.Text = Text;
                break;
            case SetTextParam.UnRead:
                n.Text = "<b>" + Text + "</b>";
                break;
            case SetTextParam.Current:
                n.Text = "<span style=\"color: blue\">" + Text + "</span>";
                break;
        }
    }
    private void DisplayEntry()
    {
        if (!string.IsNullOrEmpty((string)ViewState["lastseen"]))
        {
            TreeNode n = tree.FindNode(ViewState["lastseen"].ToString());
            SetText(n, SetTextParam.Read, Server.HtmlDecode(n.Text));
        }
        if (tree.SelectedNode == null)
            tree.Nodes[0].Select();
        var e = ForumEntry.LoadById(tree.SelectedNode.Value.ToInt());
        ForumEntryDisplay1.Entry = e;
        Reply.NavigateUrl = "~/Forum/Edit.aspx?reply=true&id={0}".Fmt(e.Id);
        Edit.NavigateUrl = "~/Forum/Edit.aspx?id={0}".Fmt(e.Id);
        e.ShowAsRead(DbUtil.Db.CurrentUser);
        SetText(tree.SelectedNode, SetTextParam.Current, e.Title, e.User);
        ViewState["lastseen"] = tree.SelectedNode.ValuePath;
        Edit.Visible = e.IsOwner || e.Forum.IsAdmin || User.IsInRole("Administrator");
    }
    protected void tree_SelectedNodeChanged(object sender, EventArgs e)
    {
    }
}
public enum SetTextParam
{
    Current,
    Read,
    UnRead
}
