<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<CmsData.Zip>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Zips</h2>

    <table>
        <tr>
            <th></th>
            <th>
                ZipCode
            </th>
            <th>
                MetroMarginalCode
            </th>
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>
            <td>
                <%= Html.ActionLink("Edit", "Edit", new { id=item.ZipCode }) %> |
                <%= Html.ActionLink("Details", "Details", new { id = item.ZipCode })%>
            </td>
            <td>
                <%= Html.Encode(item.ZipCode)%>
            </td>
            <td>
                <%= Html.Encode(item.MetroMarginalCode) %>
            </td>
        </tr>
    
    <% } %>

    </table>

    <p>
        <%= Html.ActionLink("Create New", "Create") %>
    </p>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>

