<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<CmsData.View.OrphanedImage>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<script type="text/javascript">
    $(function() {
        $("a.delete").click(function(ev) {
            $.post("/Setting/DeleteImage/" + $(this).attr("id"), {}, function(ret) {
                $(ret).remove();
            });
            return false;
        });
    });
</script>
    <h2>OrphanedImages</h2>

    <table>
        <tr>
            <th>
                Id
            </th>
            <th>
                Length
            </th>
            <th></th>
            <th>
                Mimetype
            </th>
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr id="r<%= Html.Encode(item.Id) %>">
            <td>
                <a href="/Image.aspx?id=<%= Html.Encode(item.Id) %>" target="image"><%= Html.Encode(item.Id) %></a>
            </td>
            <td>
                <%= Html.Encode(item.Length) %>
            </td>
            <td><a href="#" class="delete" id="d<%= Html.Encode(item.Id) %>">delete</a></td>
            <td>
                <%= Html.Encode(item.Mimetype) %>
            </td>
        </tr>
    
    <% } %>

    </table>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>

