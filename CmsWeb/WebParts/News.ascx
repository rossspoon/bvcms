<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="News.ascx.cs" Inherits="CMSWeb.WebParts.News" %>
<style type="text/css">
    .style1
    {
        font-size: small;
        color:Red;
    }
</style>
<h4 style="font:verdana; color:#2b637d">
    <asp:HyperLink ID="HyperLink1" runat="server" Target="_blank">CMS2 News</asp:HyperLink> 
</h4>
<div style="overflow:auto;height:600px;">
<asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
    ShowHeader="False" SkinID="GridViewSkin">
    <Columns>
        <asp:TemplateField>
            <ItemTemplate>
                <asp:HyperLink ID="HyperLink1" runat="server" 
                    NavigateUrl='<%# Eval("Url") %>' Target="_blank" 
                    Text='<%# Eval("Title") %>'></asp:HyperLink>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Date">
            <ItemTemplate>
                <asp:Label ID="Label2" runat="server" Text='<%# Eval("Published", "{0:d}") %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Author">
            <ItemTemplate>
                <asp:Label ID="Label1" runat="server" Text='<%# Eval("Author") %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
</div>
