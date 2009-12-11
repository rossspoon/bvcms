<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<CmsData.Setting>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="/Content/js/jquery.jeditable.js" type="text/javascript"></script>
    <script src="/Content/js/jquery.autocomplete.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function() {
            $(".clickEdit").editable("/Setting/Edit", {
                indicator: "<img src='/images/loading.gif'>",
                width: 400,
                tooltip: "Click to edit..."
            }); 
            $("a.delete").click(function(ev) {
                if(confirm("are you sure?"))
                    $.post("/Setting/Delete/" + $(this).attr("id"), null, function(ret) {
                        window.location = "/Setting/";
                    });
            });
        });
    </script>
   <h2>Settings</h2>

    <table>
        <tr>
            <th>
                SettingId
            </th>
            <th>
                Setting
            </th>
            <th></th>
        </tr>

    <% foreach (var item in Model) 
       { %>
        <tr>
            <td><%=item.Id %></td>
            <td>
                <span id='<%=item.Id %>' class='clickEdit'><%=item.SettingX%></span>
            </td>
            <td>
                <a id='d<%= item.Id %>' href="#" class="delete"><img border="0" src="/images/delete.gif" /></a>
            </td>
        </tr>
    <% } %>

    </table>

    <% using (Html.BeginForm("Create", "Setting"))
       { %>
    <p>
        New Setting: <%= Html.TextBox("Id") %>
        <input type="submit" value="Create" />
    </p>
    <% } %>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>

