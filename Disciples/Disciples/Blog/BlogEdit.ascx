<%@ Control Language="C#" AutoEventWireup="True" Inherits="BlogEdit" CodeBehind="BlogEdit.ascx.cs" %>
<asp:Literal ID="Literal1" runat="server" Visible="false">
<link href="../App_Themes/Default/blog.css" rel="stylesheet" type="text/css" />
<link href="../App_Themes/Default/Common.css" rel="stylesheet" type="text/css" />
</asp:Literal>
<div>
    <p>
        <b>Title</b><br />
        <asp:TextBox ID="BlogTitle" runat="server" EnableViewState="False" Width="388px"></asp:TextBox>
    </p>
    <p id="daterow" runat="server">
        <b>Entry Date</b><br />
        <asp:TextBox ID="EntryDate" runat="server"></asp:TextBox></p>
    <p>
        <b>Categories<br />
        </b>
        <asp:CheckBoxList ID="CheckBoxList1" runat="server" DataSourceID="ObjectDataSource1"
            DataTextField="Category" DataValueField="Category" OnDataBound="CheckBoxList1_DataBound">
        </asp:CheckBoxList>
        <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>&nbsp;<span style="font-size: 8pt">
            (new category)</span><asp:ObjectDataSource ID="ObjectDataSource1" runat="server"
                OldValuesParameterFormatString="original_{0}" SelectMethod="GetCategorySummary"
                TypeName="DiscData.BlogCategoryController"></asp:ObjectDataSource>
    </p>
    <p>
        <b>Body</b><br />
            <asp:TextBox ID="PostText2" CssClass="ckeditor" runat="server" TextMode="MultiLine" Height="500" Width="700" ></asp:TextBox>
    </p>
    <p>
        <asp:Button ID="Save" runat="server" Text="Save" EnableViewState="False" OnClick="Save_Click" />
        <asp:Button ID="Delete" runat="server" OnClick="Delete_Click" Text="Delete" />&nbsp;<asp:Button
            ID="Cancel" runat="server" Text="Cancel" OnClick="Cancel_Click" />&nbsp;<asp:CheckBox
                ID="NotifyByEmail" runat="server" Checked="True" Text="Notify By Email" />
    </p>
</div>
