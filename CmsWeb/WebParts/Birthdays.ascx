<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Birthdays.ascx.cs" Inherits="CMSWeb.WebParts.Birthdays" %>
<style type="text/css">
    .style1
    {
        font-size: small;
        color:Red;
    }
</style>
<asp:HyperLink ID="BFClass" runat="server">BFClass</asp:HyperLink>
<div style="overflow:auto;height:100px">
<asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
    ShowHeader="False" SkinID="GridViewSkin">
    <Columns>
        <asp:HyperLinkField DataNavigateUrlFields="Id" 
            DataNavigateUrlFormatString="~/Person.aspx?id={0}" DataTextField="Name" 
            DataTextFormatString="{0}" />
        <asp:BoundField DataField="Birthday" DataFormatString="{0:MMM dd}" />
    </Columns>
</asp:GridView>
</div>
