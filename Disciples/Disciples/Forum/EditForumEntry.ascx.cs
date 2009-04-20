using System;
using System.Web;
using DiscData;
using System.Net.Mail;
using System.Threading;
using System.Collections.Generic;

public partial class EditForumEntry : System.Web.UI.UserControl
{
    private string _CancelUrl;
    public string CancelUrl
    {
        get { return _CancelUrl; }
        set { _CancelUrl = value; }
    }
    private ForumEntry _Entry;
    public ForumEntry Entry
    {
        get { return _Entry; }
        set
        {
            _Entry = value;
            Forum f = value.Forum;
            CheckMembership(f);
            Label1.Text = "Editing Entry in: " + f.Description;
            if (!Page.IsPostBack)
            {
                EntryText.Value = value.Entry;
                EntryTitle.Text = value.Title;
            }
        }
    }
    private Forum _Forum;
    public Forum Forum
    {
        get { return _Forum; }
        set
        {
            _Forum = value;
            CheckMembership(value);
            Label1.Text = "New Post to: " + value.Description;
        }
    }
    private ForumEntry _Reply;
    public ForumEntry Reply
    {
        get { return _Reply; }
        set
        {
            _Reply = value;
            ForumEntryDisplay1.Entry = value;
            replyto.Visible = true;
            Label1.Visible = false;
        }
    }
    protected void Page_Load(System.Object sender, EventArgs e)
    {
        //if (HttpContext.Current.User.IsInRole("Administrator"))
        //    EntryText.ToolbarSet = "Simpler2";
    }
    private void CheckMembership(Forum f)
    {
        if (f==null || !f.IsMember)
            Response.Redirect("/");
    }
    private string returnloc;
    private ICollection<MailAddress> addresses;
    protected void Save_Click(object sender, EventArgs e)
    {
        if (Reply != null)
            Entry = Reply.NewReply(EntryTitle.Text, EntryText.Value);
        else if (Forum != null)
            Entry = Forum.NewEntry(EntryTitle.Text, EntryText.Value);
        else if (Entry != null)
        {
            Entry.Title = EntryTitle.Text;
            Entry.Entry = EntryText.Value;
            Entry.CreatedBy = Util.CurrentUser.UserId;
            Entry.CreatedOn = DateTime.Now;
            DbUtil.Db.SubmitChanges();
        }
        string returnloc2 = "/Forum/Thread/{0}.aspx?selected={1}"
            .Fmt(Entry.ThreadId, Entry.Id);
        returnloc = "http://{0}{1}".Fmt(Request.Url.Authority, returnloc2);
        addresses = Entry.ThreadPost.GetNotificationList();
        ThreadPool.QueueUserWorkItem(new WaitCallback(SendEmails));
        Response.Redirect(returnloc2);
    }
    protected void Cancel_Click(object sender, EventArgs e)
    {
        Response.Redirect(CancelUrl);
    }
    private void SendEmails(Object stateInfo)
    {
        var smtp = new SmtpClient();
        var from = new MailAddress("bbcms01@bellevue.org");
        var subject = "New Message: " + Entry.Title + ", From: " + Util.CurrentUser.Username;
        var body = string.Format(
            "<br><br>--<br>View this message online at: <a href=\"{0}\">{0}</a><br>--<br>", returnloc); ;
        foreach (var a in addresses)
        {
            var msg = new MailMessage(from, a);
            msg.Subject = subject;
            msg.Body = body;
            msg.IsBodyHtml = true;
            smtp.Send(msg);
        }
    }
}
