<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<CmsData.Program>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="/Content/js/jquery.jeditable.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function() {
            $(".clickEdit").editable("/Program/Edit/", {
                indicator: "<img src='/images/loading.gif'>",
                tooltip: "Click to edit...",
                style: 'display: inline',
                width: '200px'
            });
            $('.clickSelect').editable('/Program/Edit', {
                data: " {'true':'True','false':'False'}",
                type: 'select',
                submit: 'OK',
                callback: function(value, settings) {
                    window.location = "/Program/";
                }
            });
            $("a.delete").click(function(ev) {
                if (confirm("are you sure?"))
                    $.post("/Program/Delete/" + $(this).attr("id"), null, function(ret) {
                        window.location = "/Program/";
                    });
            });
        });
    </script>
   <h2>Programs</h2>

    <table>
        <tr>
            <th>
                ProgramId
            </th>
            <th>
                ProgramName
            </th>
            <th>
                Has Main Fellowship Orgs
            </th>
            <th></th>
        </tr>

    <% foreach (var item in Model) 
       { %>
        <tr>
            <td><%=item.Id %></td>
            <td>
                <span id='<%="ProgramName." + item.Id %>' 
                    class='clickEdit'><%=item.Name%></span>
            </td>
            <td>
                <span id='<%="MainFellowship." + item.Id %>' 
                    class='clickSelect'><%=item.BFProgram%></span>
            </td>
            <td>
                <a id='d<%= item.Id %>' href="#" class="delete"><img border="0" src="/images/delete.gif" /></a>
            </td>
        </tr>
    <% } %>

    </table>

    <% using (Html.BeginForm("Create", "Program"))
       { %>
    <p><input type="submit" value="Create" /></p>
    <% } %>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>

