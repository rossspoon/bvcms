<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CmsWeb.Models.PostBundleModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2>FundTotals For Bundle Id <%=Model.bundle.BundleHeaderId %> <%=Model.bundle.ContributionDate.ToShortDateString() %></h2>
<table>
<% foreach (var f in Model.TotalsByFund())
   { %>
<tr><td><%=f.Name %></td><td align="right"><%=f.Total.ToString2("N2") %></td></tr>
<% } %>
</table>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
