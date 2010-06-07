<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<CmsData.SaleItem>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="/Content/js/jquery.jeditable.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function() {
            $(".clickEdit").editable("/Sales/ItemEdit/", {
                indicator: "<img src='/images/loading.gif'>",
                tooltip: "Click to edit...",
                style: 'display: inline',
                width: '200px'
            });
            $("a.delete").click(function(ev) {
                ev.preventDefault();
                if (confirm("are you sure?"))
                    $.post("/Sales/ItemDelete/" + $(this).attr("id"), null, function(ret) {
                        window.location = "/Sales/Items";
                    });
            });
        });
    </script>
   <h2>Sale Items</h2>

    <table>
        <tr>
            <th>Id</th>
            <th>Description</th>
            <th>Price</th>
            <th>Available</th>
            <th>Url</th>
            <th>Email</th>
            <th>MaxItems</th>
            <th>DefaultItems</th>
            <th></th>
            <th></th>
            <th></th>
        </tr>

    <% foreach (var item in Model)
       { %>
        <tr>
            <td><%=item.Id %></td>
            <td>
                <span id='<%="Description." + item.Id %>' 
                    class='clickEdit'><%=item.Description%></span>
            </td>
            <td>
                <span id='<%="Price." + item.Id %>' 
                    class='clickEdit'><%=item.Price%></span>
            </td>
            <td>
                <span id='<%="Available." + item.Id %>' 
                    class='clickEdit'><%=item.Available%></span>
            </td>
            <td>
                <span id='<%="Url." + item.Id %>' 
                    class='clickEdit'><%=item.Url%></span>
            </td>
            <td>
                <span id='<%="Email." + item.Id %>' 
                    class='clickEdit'><%=item.Email%></span>
            </td>
            <td>
                <span id='<%="MaxItems." + item.Id %>' 
                    class='clickEdit'><%=item.MaxItems%></span>
            </td>
            <td>
                <span id='<%="DefaultItems." + item.Id %>' 
                    class='clickEdit'><%=item.DefaultItems%></span>
            </td>
            <td>
                <a href="/Display/EditPage/SaleMessage-<%=item.Id%>">Confirmation</a>
            </td>
            <td>
                <a href="/Display/EditPage/SaleDownload-<%=item.Id%>">Download</a>
            </td>
            <td>
                <a id='d<%= item.Id %>' href="#" class="delete"><img border="0" src="/images/delete.gif" /></a>
            </td>
        </tr>
    <% } %>

    </table>

    <% using (Html.BeginForm("ItemCreate", "Sales"))
       { %>
    <p><input type="submit" value="Create" /></p>
    <% } %>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>

