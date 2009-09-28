<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<CmsData.VolInterestCode>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="/Content/js/jquery.jeditable.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function() {
            $(".clickEdit").editable("/VolOpportunity/EditCode/", {
                indicator: "<img src='/images/loading.gif'>",
                tooltip: "Click to edit...",
                style: 'display: inline',
                width: '200px'
            });
            $("a.delete").click(function(ev) {
                if(confirm("are you sure?"))
                    $.post("/VolOpportunity/DeleteCode/" + $(this).attr("id"), null, function(ret) {
                    window.location = '/VolOpportunity/Codes/<%=ViewData["OpportunityId"] %>';
                    });
            });
        });
    </script>
   <h2><a href="/VolOpportunity/">Volunteer Interest Codes</a></h2>
   Opportunity: <a href='/Volunteer/Index/<%=ViewData["OpportunityId"] %>'><%=ViewData["Opportunity"] %></a>
    <table>
        <tr>
            <th>
                Id
            </th>
            <th>
                Description
            </th>
            <th></th>
        </tr>

    <% foreach (var item in Model) 
       { %>
        <tr>
            <td><%=item.Id %></td>
            <td>
                <span id='<%=item.Id %>' 
                    class='clickEdit'><%=item.Description%></span>
            </td>
            <td>
                <a id='d<%= item.Id %>' href="#" class="delete"><img border="0" src="/images/delete.gif" /></a>
            </td>
        </tr>
    <% } %>

    </table>

    <% using (Html.BeginForm("CreateCode", "VolOpportunity"))
       { %>
       <%=Html.Hidden("OpportunityId", ViewData["OpportunityId"]) %>
    <p><input type="submit" value="Create" /></p>
    <% } %>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>

