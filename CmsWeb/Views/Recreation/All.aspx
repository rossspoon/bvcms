<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.RecreationModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Recreation</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <script src="/Content/js/jquery.pagination.js" type="text/javascript"></script>
    <script src="/Content/js/jquery.form.js" type="text/javascript"></script>
    <script src="/Content/js/jquery.form2.js" type="text/javascript"></script>
    <script src="/Content/js/jquery.pagination.js" type="text/javascript"></script>
    <script src="/Content/js/jquery.form.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function() {
            $('#LeagueId').change(RefreshPage);
            $('#Participants > thead a.sortable').click(function(ev) {
                var newsort = $(this).text();
                var oldsort = $("#Sort").val();
                $("#Sort").val(newsort);
                var dir = $("#Dir").val();
                if (oldsort == newsort && dir == 'asc')
                    $("#Dir").val('desc');
                else
                    $("#Dir").val('asc');
                RefreshPage();
            });
        });
        function RefreshPage() {
            var q = $('#form').formSerialize();
            $.navigate("/Recreation/All/" + $('#LeagueId').val(),q);
        }
    </script>
    <form id="form">
    <div class="modalPopup">
       League: <%=Html.DropDownList("LeagueId", Model.Leagues())%>
    </div>
    <%=Html.Hidden("Sort", Model.Sort) %>
    <%=Html.Hidden("Dir", Model.Dir) %>
    </form>
<h2>Shirt Sizes</h2>
    <table id="ShirtSizes">
<% foreach (var r in Model.ShirtSizes())
   { %>
        <tr>
            <td><%=r.Text%></td>
            <td><%=r.Value%></td>
        </tr>
<% } %>
    </table>


<h2>Participants</h2>
    <table id="Participants">
        <thead>
        <tr>
            <th><a href="#" class="sortable">Name</a></th>
            <th><a href="#" class="sortable">Type</a></th>
            <th><a href="#" class="sortable">Team</a></th>
            <th><a href="#" class="sortable">Birthday</a></th>
            <th><a href="#" class="sortable">ShirtSize</a></th>
            <th><a href="#" class="sortable">FeePaid</a></th>
            <th><a href="#" class="sortable">Request</a></th>
            <th><a href="#" class="sortable">Uploaded</a></th>
        </tr>
        </thead>
        <tbody>
<% foreach (var r in Model.FetchAll())
   { %>
        <tr>
            <td><a href="/Person.aspx?id=<%=r.PeopleId %>"><%=r.Name%></a></td>
            <td><%=r.MemberType %></td>
            <td><%=r.TeamName%></td>
            <td><%=r.Birthday%></td>
            <td><%=r.ShirtSize%></td>
            <td><%=r.FeePaid%></td>
            <td><%=r.Request%></td>
            <td><a href="/Recreation/Detail/<%=r.Id%>"><%=r.Uploaded.Value.ToString("M/d H:mm") %></a></td>
        </tr>
<% } %>
        </tbody>
    </table>
</asp:Content>
