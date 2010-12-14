<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CmsWeb.Models.RecreationModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Recreation</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript">
        $(function() {
            $('#LeagueId').change(RefreshPage);
            $('#AgeDivId').change(RefreshPage);
            $('#FilterUnassigned').click(RefreshPage);
            $('#NormalMembersOnly').click(RefreshPage);
            $('input.check').click(UpdateTotals);
            $('#Participants > thead a.sortable').click(function(ev) {
                ev.preventDefault();
                var newsort = $(this).text();
                var oldsort = $("#Sort").val();
                $("#Sort").val(newsort);
                var dir = $("#Dir").val();
                if (oldsort == newsort && dir == 'asc')
                    $("#Dir").val('desc');
                else
                    $("#Dir").val('asc');
                RefreshList();
            });
            $('#total').text($('.check').length);
            UpdateTotals();
            $("#seeall").click(function(ev) {
                $.navigate("/Recreation/All/" + $('#LeagueId').val());
            });
            $("a.createdetail").click(function(ev) {
                ev.preventDefault();
                $.post("/Recreation/Create/" + $(this).attr("pid"), { oid: $(this).attr("oid") }, function(ret) {
                    window.location = "/Recreation/Detail/" + ret.id;
                }, "json");
                return false;
            });
        });
        function RefreshPage() {
            var q = $('#form').serialize();
            $.navigate("/Recreation/Index", q);
        }
        function RefreshList() {
            var q = $('#form').serialize();
            $.post('/Recreation/List/', q, function(ret) {
                $('#Participants > tbody').html(ret);
                $('input.check').click(UpdateTotals);
            });
        }
        function UpdateTotals() {
            $('#ttotal').text($('.check:checked').length);
        }
        $(function() {

        });
    </script>
    
    <form id="form" method="post" action="/Recreation/AssignToTeam">
    <div class="modalPopup">
       League: <%=Html.DropDownList("LeagueId", Model.Leagues())%>
       <a href="#" id="seeall">see all</a>
       Age Division: <%=Html.DropDownList("AgeDivId", Model.AgeDivisions())%>&nbsp;&nbsp;&nbsp;
       <%=Html.CheckBox("FilterUnassigned") %> Unassigned Only
       <%=Html.CheckBox("NormalMembersOnly")%> Exclude Teachers<br />
       Target Team: <%=Html.DropDownList("TargetTeamName", Model.TargetTeams())%>
       <input type="submit" name="submit" value="Assign to Group" />
       <% if((Model.AgeDivId ?? 0) > 0)
          { %>
       <%=Html.HyperLink("/Organization/Index/" + Model.AgeDivId, Model.AgeDiv.OrganizationName) %>
       <% } %>
    </div>
    <%=Html.Hidden("Sort", Model.Sort) %>
    <%=Html.Hidden("Dir", Model.Dir) %>
    League Total: <span id="total"></span> | TeamTotal: <span id="ttotal"></span>
    <table id="Participants">
        <thead>
        <tr>
            <th><a href="#" class="sortable">Mixed</a></th>
            <th><a href="#" class="sortable">Name</a></th>
            <th><a href="#" class="sortable">Type</a></th>
            <th><a href="#" class="sortable">Church</a></th>
            <th><a href="#" class="sortable">Team</a></th>
            <th><a href="#" class="sortable">Birthday</a></th>
            <th><a href="#" class="sortable">ShirtSize</a></th>
            <th><a href="#" class="sortable">FeePaid</a></th>
            <th align="left"><a href="#" class="sortable">Request</a></th>
        </tr>
        </thead>
        <tbody>
        <% Html.RenderPartial("List", Model); %>
        </tbody>
    </table>
    </form>
</asp:Content>
