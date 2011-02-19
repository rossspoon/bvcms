<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/onlinereg.Master"
    Inherits="System.Web.Mvc.ViewPage<CmsWeb.Models.OnlineRegModel>" %>

<asp:Content ID="registerContent" ContentPlaceHolderID="MainContent" runat="server">
<div class="regform">
    <%= SquishIt.Framework.Bundle.JavaScript()
        .Add("/Content/js/jquery-1.4.4.js")
        .Add("/Content/js/jquery-ui-1.8.9.custom.js")
        .Add("/Content/js/jquery.idle-timer.js")
        .Add("/Scripts/OnlineReg.js")
        .Render("/Content/OnLineReg_#.js")
    %>
    <script type="text/javascript">
        $(function () {
            $(document).bind("idle.idleTimer", function () {
                window.location.href = '<%=Model.URL %>';
            });
            var tmout = parseInt('<%=ViewData["timeout"] %>');

            if ($('input:text[value!=""]').length == 0)
                $(document).bind("keydown", function () {
                    $(document).unbind("keydown");
                    $.idleTimer(tmout);
                });
            else
                $.idleTimer(tmout);
            $(".personheader a").live("click", function (e) {
                e.preventDefault();
                $(this).closest('div').nextAll('table').slideToggle();
                return false;
            });
        });
    </script>
    <h2>
        <%=Model.Header%></h2>
    <% var msg = Model.Filled();
       if (Model.NotAvailable())
       { %>
    <h4>Sorry, Registration is not available at this time</h4>
    <% }
       else if (msg.HasValue())
       { %>
    <h4>Sorry, <%=msg %></h4>
    <% }
       else
       { %>
    <%=Util.PickFirst(Model.Instructions, @"
<div class=""instructions login"">
</div>
<div class=""instructions find"">
</div>
<div class=""instructions select"">
</div>
<div class=""instructions other"">
</div>
<div class=""instructions submit"">
</div>
") %>
    <form class="DisplayEdit" action="/OnlineReg/CompleteRegistration/<%=Model.qtesting %>"
    method="post">
    <% Html.RenderPartial("Flow/List", Model); %>
    </form>
    <% } %>
</div>
</asp:Content>
