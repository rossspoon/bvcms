<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.RecreationModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Coaches</h2>
<table>
<% foreach (var r in Model.FetchWannabeCoaches())
   { %>
<tr>
    <td><a href="/Person/Index/<%=r.PeopleId %>"><%=r.Person.Name%></a></td>
    <td><%=r.Fname %></td>
    <td><%=r.Mname %></td>
    <td><a href="/Recreation/Detail/<%=r.Id%>"><%=r.Uploaded.Value.ToString("M/d H:mm") %></a></td>
</tr>
<% } %>
</table>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
