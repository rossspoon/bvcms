using System;
using CmsData;
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
        Literal1.Text = 
@"
<script src=""{0}"" type=""text/javascript""></script>
<script src=""{1}"" type=""text/javascript""></script>
<script type=""text/javascript"">
    $(function() {{
        CKEDITOR.replace('{2}', {{
            filebrowserUploadUrl: '{3}',
            filebrowserImageUploadUrl: '{3}'
        }});
    }});
</script>".Fmt(Util.ResolveUrl("~/js/jquery-1.2.6.js"), Util.ResolveUrl("~/ckeditor/ckeditor.js"),
          txtContent2.UniqueID, Util.ResolveUrl("~/CKUpload.ashx"));

        ContentId = Request.QueryString<int?>("id");
        if (!Page.IsPostBack)
        {
            if (ContentId.HasValue)
            {
                ParaContent text = ContentService.GetContent(ContentId.Value);
                if (text.Body != null)
                    txtContent2.Text = text.Body;
                TextBox1.Text = text.Title;
            }
        }
    }

    protected void btnSave_Click(object sender, System.EventArgs e)
    {
        ParaContent text = ContentService.GetContent(ContentId.Value);
        if (text == null)
        {
            text = new ParaContent();
            DbUtil.Db.ParaContents.InsertOnSubmit(text);
            text.CreatedById = DbUtil.Db.CurrentUser.UserId;
            text.CreatedOn = DateTime.Now;
        }
        text.Body = txtContent2.Text;
        text.Title = TextBox1.Text;
        DbUtil.Db.SubmitChanges();
        ResultMessage1.ShowSuccess("Content Saved");
    }
}
