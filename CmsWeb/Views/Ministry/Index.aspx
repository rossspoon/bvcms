<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<CmsData.Ministry>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<script type="text/javascript">
    //id=elements_id&value=user_edited_content
    $(".click").editable("http://www.appelsiini.net/projects/jeditable/php/echo.php", {
        indicator: "<img src='img/indicator.gif'>",
        tooltip: "Click to edit...",
        style: "inherit"
    });
    $(".editable_select_json").editable("http://www.appelsiini.net/projects/jeditable/php/save.php", {
        indicator: '<img src="img/indicator.gif">',
        loadurl: "http://www.appelsiini.net/projects/jeditable/php/json.php", //{"D":"Letter D","E":"Letter E","F":"Letter F","G":"Letter G","selected":"F"}
        type: "select",
        submit: "OK",
        style: "inherit"
    });
</script>
    <h2>Ministries</h2>

    <table>
        <tr>
            <th></th>
            <th>
                MinistryId
            </th>
            <th>
                MinistryName
            </th>
            <th>
                MinistryDescription
            </th>
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>
            <td>
                <%= Html.ActionLink("Edit", "Edit", new { id=item.MinistryId }) %> |
                <%= Html.ActionLink("Details", "Details", new { id = item.MinistryId })%>
            </td>
            <td>
                <%= Html.Encode(item.MinistryId)%>
            </td>
            <td>
                <%= Html.Encode(item.MinistryName) %>
            </td>
            <td>
                <%= Html.Encode(item.MinistryDescription) %>
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

