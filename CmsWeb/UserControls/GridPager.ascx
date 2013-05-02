<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GridPager.ascx.cs" Inherits="CmsWeb.GridPager" %>
        <asp:Label ID="Label6" runat="server" Text="Show rows:" />
        <asp:DropDownList ID="PageSize" runat="server" AutoPostBack="true" OnSelectedIndexChanged="PageSize_SelectedIndexChanged">
            <asp:ListItem Value="10" />
            <asp:ListItem Value="25" />
            <asp:ListItem Value="50" />
            <asp:ListItem Value="75" />
            <asp:ListItem Value="100" />
            <asp:ListItem Value="200" />
        </asp:DropDownList>
        Page
        <asp:TextBox ID="GoToPage" runat="server" AutoPostBack="true" OnTextChanged="GoToPage_TextChanged"
            Style="font-size: x-small; width: 20px;" />
        of
        <asp:Label ID="TotalNumberOfPages" runat="server" />
        <asp:ImageButton ID="GoPrev" runat="server" ImageUrl="~/images/arrow_left.gif"
            CommandName="Page" CommandArgument="Prev" oncommand="Go_Command" />
        <asp:ImageButton ID="GoNext" runat="server" ImageUrl="~/images/arrow_right2.gif"
            CommandName="Page" CommandArgument="Next" oncommand="Go_Command" />
