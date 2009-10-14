<%@ Control Language="C#" AutoEventWireup="True" Inherits="EditForumEntry" CodeBehind="EditForumEntry.ascx.cs" %>
<asp:Literal ID="Literal1" runat="server" Visible="false">
<link href="../App_Themes/Default/forum.css" rel="stylesheet" type="text/css" />
<link href="../App_Themes/Default/Common.css" rel="stylesheet" type="text/css" />
</asp:Literal>
<table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 100%">
    <tr>
        <td colspan="2">
            <asp:Label ID="Label1" runat="server" EnableViewState="False" Text="Label"></asp:Label>
        </td>
    </tr>
    <tr id="replyto" runat="server" visible="false">
        <td align="right" valign="top">
            Replying to:
        </td>
        <td>
            <cc1:ForumEntryDisplay ID="ForumEntryDisplay1" runat="server"></cc1:ForumEntryDisplay>
        </td>
    </tr>
    <tr>
        <td align="right" style="height: 24px">
            Title:
        </td>
        <td style="height: 24px">
            <asp:TextBox ID="EntryTitle" runat="server" EnableViewState="False" Width="388px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td align="right" valign="top">
            Entry:
        </td>
        <td>
            <asp:TextBox ID="EntryText2" CssClass="ckeditor" runat="server" TextMode="MultiLine" Height="500" Width="700" ></asp:TextBox>
      </td>
    </tr>
    <tr>
        <td>
        </td>
        <td>
            <asp:Button ID="Save" runat="server" Text="Save" EnableViewState="False" OnClick="Save_Click" />&nbsp;<asp:Button
                ID="Cancel" runat="server" OnClick="Cancel_Click" Text="Cancel" />
        </td>
    </tr>
</table>
