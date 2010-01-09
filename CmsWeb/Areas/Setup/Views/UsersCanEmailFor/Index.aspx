<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<CmsData.UserCanEmailFor>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Index</h2>

    <table>
        <tr>
            <th></th>
            <th>
                UserId
            </th>
            <th>
                CanEmailFor
            </th>
        </tr>

    <% foreach (var item in Model) 
       { %>
        <tr>
            <td>
                <%= Html.ActionLink("Delete", "Delete", new { id = item.UserId, CanEmailFor = item.CanEmailFor }) %>
            </td>
            <td>
                <%= Html.Encode(item.UserId) %>
            </td>
            <td>
                <%= Html.Encode(item.CanEmailFor) %>
            </td>
        </tr>
    <% } %>

    </table>

    <% using (Html.BeginForm("Create", "UsersCanEmailFor")) {%>

        <fieldset>
            <legend>Fields</legend>
            <p>
                <label for="UserId">UserId:</label>
                <%= Html.TextBox("UserId") %>
                <%= Html.ValidationMessage("UserId", "*") %>
            </p>
            <p>
                <label for="CanEmailFor">CanEmailFor:</label>
                <%= Html.TextBox("CanEmailFor") %>
                <%= Html.ValidationMessage("CanEmailFor", "*") %>
            </p>
            <p>
                <input type="submit" value="Create" />
            </p>
        </fieldset>

    <% } %>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>

