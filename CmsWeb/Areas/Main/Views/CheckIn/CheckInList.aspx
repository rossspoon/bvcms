<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<CmsData.CheckInTime>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2>CheckInList</h2>

    <table>
        <tr>
            <th>
                Id
            </th>
            <th>
                PeopleId
            </th>
            <th>
                Name
            </th>
            <th>
                OrganizationId
            </th>
            <th>
                CheckInDay
            </th>
            <th>
                CheckInTimeX
            </th>
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>
            <td>
                <%= Html.Encode(item.Id) %>
            </td>
            <td>
                <%= Html.Encode(item.PeopleId) %>
            </td>
            <td>
                <%= Html.Encode(item.Person.Name) %>
            </td>
            <td>
                <%= Html.Encode(item.OrganizationId) %>
            </td>
            <td>
                <%= Html.Encode(item.CheckInDay.Value.ToString("ddd M/d/yy")) %>
            </td>
            <td>
                <%= Html.Encode(item.CheckInTimeX.Value.ToShortTimeString()) %>
            </td>
        </tr>
    
    <% } %>

    </table>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>

