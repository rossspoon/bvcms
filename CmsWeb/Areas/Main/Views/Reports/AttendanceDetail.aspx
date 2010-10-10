<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CmsWeb.Areas.Main.Models.Report.AttendanceDetailModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <table cellpadding="3">
    <thead>
    <tr>
    <th align="left">OrgName</th>
    <th align="left">Leader</th>
    <th align="left">Date</th>
    <th align="right">Present</th>
    <th align="right">Visitors</th>
    <th align="right">OutTowners</th>
    </tr>
    </thead>
    <tbody>
    <% foreach (var m in Model.FetchMeetings())
       { %>
        <tr>
        <td><%=m.OrgName %></td>
        <td><%=m.Leader %></td>
        <td><%=m.date %></td>
        <td align="right"><%=m.Present %></td>
        <td align="right"><%=m.Visitors %></td>
        <td align="right"><%=m.OutTowners %></td>
        </tr>
    <% } %>
    </tbody>
    </table>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">

</asp:Content>
