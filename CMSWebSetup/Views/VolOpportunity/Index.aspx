<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<CmsData.VolOpportunity>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="/Content/js/jquery.jeditable.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function() {
            $(".clickEdit").editable("/VolOpportunity/Edit/", {
                indicator: "<img src='/images/loading.gif'>",
                tooltip: "Click to edit...",
                style: 'display: inline',
                width: '200px'
            });
            $(".clickEdit2").editable("/VolOpportunity/Edit/", {
                type: "textarea",
                indicator: "<img src='/images/loading.gif'>",
                tooltip: "Click to edit...",
                style: 'display: inline',
                width: '300px',
                height: '200px',
                submit : 'OK',
                cancel : 'cancel'
            });
            $("a.delete").click(function(ev) {
                if (confirm("are you sure?"))
                    $.post("/VolOpportunity/Delete/" + $(this).attr("id"), null, function(ret) {
                        window.location = "/VolOpportunity/";
                    });
            });
        });
    </script>
   <h2>Volunteer Opportunities</h2>

    <table>
        <tr>
            <th>
                Id
            </th>
            <th>
                Description
            </th>
            <th>
                UrlKey
            </th>
            <th>
                Email Yes Cva
            </th>
            <th>
                Email No Cva
            </th>
            <th>
                Extra Question
            </th>
            <th>Instructions</th>
            <th>Email</th>
            <th>MaxChecks</th>
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
                <span id='<%="UrlKey." + item.Id %>' 
                    class='clickEdit'><%=item.UrlKey%></span>
            </td>
            <td>
                <span id='<%="EmailYesCva." + item.Id %>' 
                    class='clickEdit2'><%=item.EmailYesCva%></span>
            </td>
            <td>
                <span id='<%="EmailNoCva." + item.Id %>' 
                    class='clickEdit2'><%=item.EmailNoCva%></span>
            </td>
            <td>
                <span id='<%="Question." + item.Id %>' 
                    class='clickEdit'><%=item.ExtraQuestion%></span>
            </td>
            <td>
                <span id='<%="Instructions." + item.Id %>' 
                    class='clickEdit'><%=item.ExtraInstructions%></span>
            </td>
            <td>
                <span id='<%="Email." + item.Id %>' 
                    class='clickEdit'><%=item.Email%></span>
            </td>
            <td>
                <span id='<%="MaxChecks." + item.Id %>' 
                    class='clickEdit'><%=item.MaxChecks%></span>
            </td>
            <td>
                <a href="/VolOpportunity/Codes/<%=item.Id %>">Interest Codes</a>
            </td>
            <td>
                <a id='d<%= item.Id %>' href="#" class="delete"><img border="0" src="/images/delete.gif" /></a>
            </td>
        </tr>
    <% } %>

    </table>

    <% using (Html.BeginForm("Create", "VolOpportunity"))
       { %>
    <p><input type="submit" value="Create" /></p>
    <% } %>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>

