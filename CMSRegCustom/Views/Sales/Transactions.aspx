<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<CmsData.SaleTransaction>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Registrations</h2>

    <table>
        <tr>
            <th>
                Name
            </th>
            <th>
                Quantity
            </th>
            <th>
                Description
            </th>
            <th>
                TransactionId
            </th>
            <th>
                RegisteredOn
            </th>
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>
            <td>
                <a href="/Person.aspx?id=<%= Html.Encode(item.Person.PeopleId) %>"><%= Html.Encode(item.Person.Name) %></a>
            </td>
            <td>
                <%= Html.Encode(item.Quantity) %>
            </td>
            <td>
                <%= Html.Encode(item.Amount.ToString("C")) %>
            </td>
            <td>
                <%= Html.Encode(item.ItemDescription) %>
            </td>
            <td>
                <%= Html.Encode(item.TransactionId) %>
            </td>
            <td>
                <%= Html.Encode(String.Format("{0:ddd MMM d hh:mm}", item.SaleDate)) %>
            </td>
        </tr>
    
    <% } %>

    </table>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>

