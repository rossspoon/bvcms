<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.VolunteersModel>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <script src="/Content/js/jquery.pagination.js" type="text/javascript"></script>
    <script src="/Content/js/jquery.form.js" type="text/javascript"></script>
    <script src="/Content/js/jquery.form2.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function() {
            $('#OpportunityId').change(RefreshList);
            $('#InterestId').change(RefreshList);
            $('#Volunteers > thead a.sortable').click(function(ev) {
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
            $("a.delete").click(function(ev) {
                if (confirm("are you sure?"))
                    $.post("/Volunteers/Delete/" + $(this).attr("id"), null, function(ret) {
                        window.location = "/Volunteers/";
                    });
            });
        });
        (function($) {
            $.fn.fillOptions = function(a, multiple) {
                var options = '';
                if (a)
                    for (var i = 0; i < a.length; i++) {
                    options += '<option value="' + a[i].Value + '"';
                    if (a[i].Selected)
                        options += ' selected=\'selected\''
                    options += '>' + a[i].Text + '</option>';
                }
                return this.each(function() {
                    var s = "<select id='" + this.id + "' name='" + this.id + "'";
                    if (multiple)
                        s += " multiple='multiple'";
                    s += ">" + options + "</select>";
                    $(this).replaceWith(s);
                });
            };
            $.fn.multiSelectRemove = function() {
                $(this).each(function() {
                    $(this).next('.multiSelect').remove();
                    $(this).next('.multiSelectOptions').remove();
                });
                return $(this);
            };
        })(jQuery);
        function RefreshList() {
            var q = $('#form').formSerialize2();
            $.navigate("/Volunteers/Index", q);
        }
        function GotoPage(pg) {
            var q = $('#form').formSerialize2();
            q = q.appendQuery("Page=" + pg);
            $.navigate("/Volunteers/Index", q);
        }
        function SetPageSize(sz) {
            var q = $('#form').formSerialize2();
            q = q.appendQuery("PageSize=" + sz);
            $.navigate("/Volunteers/Index", q);
        }
    </script>
    <form id="form" method="get" action="/Volunteers/Index">
    <div class="modalPopup">
       Opportunities: <%=Html.DropDownList("OpportunityId", Model.Opportunities())%>
       Interests: <%=Html.DropDownList("InterestId", Model.Interests())%>
    </div>
    <%=Html.Hidden("Sort", Model.Sort) %>
    <%=Html.Hidden("Dir", Model.Dir) %>
    <table id="Volunteers">
        <thead>
        <tr>
            <th><a href="#" class="sortable">Date</a></th>
            <th><a href="#" class="sortable">Name</a></th>
            <th>Interests</th>
            <th>Question</th>
        </tr>
        </thead>
        <tbody>
        <% foreach (var v in Model.FetchVolunteers())
           { %>
        <tr>
            <td><%="{0:MM-dd-yy HH:mm}".Fmt(v.Created)%></td>
            <td>
                <a href='/Person.aspx?id=<%=v.PeopleId%>'><%=v.Name%></a>
            </td>
            <td><%=v.Interests%></td>
            <td><%=v.Answer%></td>
            <td>
                <a id='d<%= v.Id %>' href="#" class="delete"><img border="0" src="/images/delete.gif" /></a>
            </td>
        </tr>
        <% } %>
        </tbody>
    </table>
    <% Html.RenderPartial("Pager", Model.pagerModel()); %>
<input type="hidden" id="Count" value='<%=Model.Count%>' />
</form>
</asp:Content>

