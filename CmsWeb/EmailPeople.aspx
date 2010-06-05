<%@ Page Language="C#" Async="true" AsyncTimeout="1000" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EmailPeople.aspx.cs"
    Inherits="CMSWeb.EmailPeople" Title="Email People" validateRequest="false"  %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        p.MsoNormal
        {
            margin-bottom: .0001pt;
            font-size: 12.0pt;
            font-family: "Times New Roman" , "serif";
            margin-left: 0in;
            margin-right: 0in;
            margin-top: 0in;
        }
        .style1
        {
            color: #FF0000;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:PlaceHolder ID="CKEditPanel" runat="server">
<script src="/ckeditor/ckeditor.js" type="text/javascript"></script>
<script type="text/javascript">
    $(function() {
        CKEDITOR.replace( "<%=EmailBody.UniqueID %>", {
                filebrowserUploadUrl : '/Account/CKEditorUpload/',
                filebrowserImageUploadUrl: '/Account/CKEditorUpload/'
            });
    });
</script>
    </asp:PlaceHolder>
<blockquote style="width: 80%"><span class="style1">Please Note</span>: your session will timeout in 20 minutes. 
If you hit send after that, your message will not be sent correctly.
Unless this is an email&nbsp; you can type up quickly,
please compose your message in an external text editor such as notepad or Word.
Then come back to a fresh version of this page and copy/paste your message into the 
text box below. You can then fill out your subject, add any attachment and send it 
    without worry of losing your work.</blockquote>
    <table width="100%">
        <tr>
            <td>
                &nbsp;
            </td>
            <td>
                Number of Emails:
                <asp:Label ID="Count" runat="server" Text="Label"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td valign="top">
                <asp:Button ID="SendEmail" runat="server" Text="Send" Width="62px" OnClientClick="$.blockUI()" OnClick="SendEmail_Click"
                    Height="42px" />
                <asp:Label ID="Label1" runat="server" Text="Email has been sent" Font-Size="Large"
                    ForeColor="#58DF55" Height="30px" Visible="False"></asp:Label>
                <br />
                <asp:LinkButton ID="TestSendEmail" runat="server" Text="Test (Send To Yourself)"
                    OnClick="TestSendEmail_Click" />
                &nbsp;
            </td>
        </tr>
        <tr>
            <td align="right">
                From:
            </td>
            <td>
                <asp:DropDownList ID="EmailFrom" runat="server" DataTextField="Value" DataValueField="Code"
                    DataSourceID="ODSEmailFrom">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td align="right">
                Subject:
            </td>
            <td>
                <asp:TextBox ID="SubjectLine" runat="server" Width="90%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                Attachment:</td>
            <td>
                <asp:FileUpload ID="FileUpload1" runat="server" />
            </td>
        </tr>
        <tr>
            <td align="right">
                Body:<br />
                <br />
                <asp:CheckBox ID="IsHtml" runat="server" AutoPostBack="true" Text="HTML Editor" 
                    oncheckedchanged="IsHtml_CheckedChanged" />&nbsp;</td>
            <td>
                <asp:TextBox ID="EmailBody" runat="server" Rows="16" TextMode="MultiLine" Width="90%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <br />
                In the body of the text, the following:<br />
                <br />
                <div class="boxThinBorder">
                    <pre>
Hi {first},

I could call you by your whole name like this: {name}.

*This is italics*, **this is bold** and ***this is underlined***
This is a hyperlink: http://www.twitter.com
&gt;&gt;&gt;
This is indented (like a block quote)
And so is this. (the trick is the pointy brackets)
&lt;&lt;&lt;
This text: "Goto Google" = "www.google.com" is a hyperlink too.
-David
</pre>
                </div>
                <br />
                would look like this in the email:<br />
                <div class="boxThinBorder">
                    <p>
                        Hi David,<br />
                        <br />
                        I could call you by your whole name like this: David Carroll.<br />
                        <br />
                        <i>This is italics</i>, <b>this is bold</b> and <u>this is underlined</u><br />
                        This is a hyperlink: <a href="http://www.twiter.com" target="_new">www.twitter.com</a></p>
                    <blockquote>
                        This is indented (like a block quote)<br />
                        And so is this. (the trick is the pointy brackets)</blockquote>
                    <p>
                        This text: <a href="http://www.google.com" target="_new">Goto Google</a> is
                        a hyperlink too.<br />
                        -David</p>
                </div>
                <br />
            </td>
        </tr>
    </table>
    <asp:ObjectDataSource ID="ODSEmailFrom" runat="server" SelectMethod="UsersToEmailFrom"
        TypeName="CMSPresenter.CodeValueController"></asp:ObjectDataSource>
</asp:Content>
