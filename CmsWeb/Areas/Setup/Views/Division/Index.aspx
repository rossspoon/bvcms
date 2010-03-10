<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<CMSWeb.Areas.Setup.Controllers.DivisionController.DivisionInfo>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="/Content/js/jquery.jeditable.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function() {
            $(".clickEdit").editable("/Setup/Division/Edit/", {
                indicator: "<img src='/images/loading.gif'>",
                tooltip: "Click to edit...",
                style: 'display: inline',
                width: '200px'
            });
            $(".clickSelect").editable("/Setup/Division/Edit/", {
                indicator: '<img src="/images/loading.gif">',
                loadtype: 'post',
                loadurl: "/Setup/Division/ProgramCodes/",
                type: "select",
                submit: "OK",
                style: 'display: inline'
            });
            $("a.delete").click(function(ev) {
                if (confirm("are you sure?"))
                    $.post("/Setup/Division/Delete/" + $(this).attr("id"), null, function(ret) {
                        window.location = "/Setup/Division/";
                    });
                return false;
            });
            $("table tr:even").attr("style", "background-color:#ddd");
        });
    </script>
    <h2>Divisions</h2>
    <table>
        <tr>
            <th colspan="3"></th>
            <th colspan="6">Counts</th>
            <th></th>
        </tr>
        <tr>
            <th>Id</th>
            <th>Division</th>
            <th>Program</th>
            <th>Orgs</th>
            <th>DivOrgs</th>
            <th>RecAgeDiv</th>
            <th>RecReg</th>
            <th>ToPromotion</th>
            <th>FromPromotion</th>
            <th></th>
        </tr>

    <% foreach (var i in Model) 
       { %>
        <tr>
            <td>
                <span><%=i.Id %></span>
            </td>
            <td>
                <span id='n<%=i.Id %>' 
                    class='clickEdit'><%=i.Name%></span>
            </td>
            <td>
                <span id='p<%=i.Id %>' 
                    class='clickSelect'><%=i.ProgId == null? "click to set" : i.Program%></span>
            </td>
            <td><%=i.NoZero(i.OrgCount) %></td>
<% if (DbUtil.Db.UserPreference("neworgsearch").ToBool())
   { %>
            <td><a href="/OrgSearch/Index/?div=<%=i.Id%>&progid=<%=i.ProgId%>"><%=i.NoZero(i.DivOrgsCount)%></a></td>
<% }
   else
   { %>            
            <td><a href="/OrganizationSearch.aspx?div=<%=i.Id%>&progid=<%=i.ProgId%>"><%=i.NoZero(i.DivOrgsCount)%></a></td>
<% } %>
            <td><%=i.NoZero(i.RecAgeDivCount) %></td>
            <td><%=i.NoZero(i.RecRegCount) %></td>
            <td><%=i.NoZero(i.ToPromotionsCount) %></td>
            <td><%=i.NoZero(i.FromPromotionsCount) %></td>
            <td>
            <% if (i.CanDelete)
               { %>
                <a id='x<%=i.Id %>' href="#" class="delete"><img border="0" src="/images/delete.gif" /></a>
            <% } %>
            </td>
        </tr>
    <% } %>

    </table>

    <% using (Html.BeginForm("Create", "Division"))
       { %>
    <p><input type="submit" value="Create" /></p>
    <% } %>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>

