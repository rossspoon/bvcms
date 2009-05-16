<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<CmsData.Ministry>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="/Content/js/jquery.jeditable.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function() {
            $(".clickEdit").editable("/Ministry/Edit/", {
                indicator: "<img src='/images/loading.gif'>",
                tooltip: "Click to edit...",
                style: 'display: inline',
                width: '200px'
            });
            $("a.delete").click(function(ev) {
                if(confirm("are you sure?"))
                    $.post("/Ministry/Delete/" + $(this).attr("id"), null, function(ret) {
                        window.location = "/Ministry/";
                    });
            });
        });
    </script>
   <h2>Ministries</h2>

    <table>
        <tr>
            <th>
                MinistryId
            </th>
            <th>
                MinistryName
            </th>
            <th></th>
        </tr>

    <% foreach (var item in Model) 
       { %>
        <tr>
            <td><%=item.MinistryId %></td>
            <td>
                <span id='<%="MinistryName." + item.MinistryId %>' 
                    class='clickEdit'>
                <%=item.MinistryName%>
                </span>
            </td>
            <td>
                <a id='d<%= item.MinistryId %>' href="#" class="delete"><img border="0" src="/images/delete.gif" /></a>
            </td>
        </tr>
    <% } %>

    </table>

    <% using (Html.BeginForm("Create", "Ministry"))
       { %>
    <p><input type="submit" value="Create" /></p>
    <% } %>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>

