<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/onlinereg.Master" Inherits="System.Web.Mvc.ViewPage<CmsWeb.Models.OnlineRegModel0>" %>
<asp:Content ID="registerContent" ContentPlaceHolderID="MainContent" runat="server">
    <%= SquishIt.Framework.Bundle.JavaScript()
        .Add("/Content/js/jquery-1.4.2.js")
        .Add("/Content/js/jquery-ui-1.8.2.custom.js")
        .Add("/Content/js/jquery.idle-timer.js")
        .Add("/Scripts/OnlineReg.js")
        .Render("/Content/OnLineReg_#.js")
            %>  
    <script type="text/javascript">
        $(function() {
            $(document).bind("idle.idleTimer", function() {
                window.location.href = '<%=Model.URL %>';
            });
            var tmout = parseInt('<%=ViewData["timeout"] %>');

            if ($('input:text[value!=""]').length == 0)
                $(document).bind("keydown", function() {
                    $(document).unbind("keydown");
                    $.idleTimer(tmout);
                });
            else
                $.idleTimer(tmout);
        });
    </script>
    <h2><%=Model.Header%></h2>
    <% if (Model.IsEnded())
       { %>
    <h4 style="color:Red">Registration has ended</h4>
    <%=Model.Instructions%>
    <% }
       else
       { %>
    <%=Model.Instructions%>
    <form class="DisplayEdit" action="/OnlineReg/CompleteRegistration/<%=Model.qtesting %>" method="post">
    <% Html.RenderPartial("List", Model); %>
    </form>
    <% } %>
</asp:Content>