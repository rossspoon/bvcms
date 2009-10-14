using System;
using DiscData;
using System.Web;
using UtilityExtensions;

public partial class CMSParagraph : System.Web.UI.Page
{
    protected int? ContentId;
    protected string ContentText = "";

    protected void Page_Init(object sender, EventArgs e)
    {
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        ContentId = Request.QueryString<int?>("id");
        if (!Page.IsPostBack)
        {
            if (ContentId.HasValue)
            {
                Content text = ContentService.GetContent(ContentId.Value);
                if (text.Body != null)
                    txtContent2.Text = text.Body;
                TextBox1.Text = text.Title;
            }
        }
    }

    protected void btnSave_Click(object sender, System.EventArgs e)
    {
        Content text = ContentService.GetContent(ContentId.Value);
        if (text == null)
        {
            text = new Content();
            DbUtil.Db.Contents.InsertOnSubmit(text);
            text.CreatedBy = HttpContext.Current.User.Identity.Name;
            text.CreatedOn = DateTime.Now;
        }
        else
        {
            text.ModifiedBy = HttpContext.Current.User.Identity.Name;
            text.ModifiedOn = DateTime.Now;
        }
        text.Body = txtContent2.Text;
        text.Title = TextBox1.Text;
        DbUtil.Db.SubmitChanges();
        ResultMessage1.ShowSuccess("Content Saved");
    }
}
