<%@ Page Language="C#" StylesheetTheme="Standard" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EmailPeople.aspx.cs"
    Inherits="CMSWeb.EmailPeople" Title="Email People" %>

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
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
                <asp:Button ID="SendEmail" runat="server" Text="Send" Width="62px" OnClick="SendEmail_Click"
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
            <td>
                From:
            </td>
            <td>
                <asp:DropDownList ID="EmailFrom" runat="server" DataTextField="Value" DataValueField="Code"
                    DataSourceID="ODSEmailFrom">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                Subject:
            </td>
            <td>
                <asp:TextBox ID="SubjectLine" runat="server" Width="90%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                Attachment:</td>
            <td>
                <asp:FileUpload ID="FileUpload1" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                Body:
            </td>
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
This is a hyperlink: http://disciples.bellevue.org
&gt;&gt;&gt;
This is indented (like a block quote)
And so is this. (the trick is the pointy brackets)
&lt;&lt;&lt;
This text: "Goto Bellevue" = "www.bellevue.org" is a hyperlink too.
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
                        This is a hyperlink: <a href="http://disciples.bellevue.org" target="_new">disciples.bellevue.org</a></p>
                    <blockquote>
                        This is indented (like a block quote)<br />
                        And so is this. (the trick is the pointy brackets)</blockquote>
                    <p>
                        This text: <a href="http://www.bellevue.org" target="_new">Goto Bellevue</a> is
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
