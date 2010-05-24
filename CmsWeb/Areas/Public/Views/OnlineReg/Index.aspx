<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/onlinereg.Master" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.OnlineRegModel>" %>
<asp:Content ID="registerContent" ContentPlaceHolderID="MainContent" runat="server">
<style type="text/css">
tr.alt
{
    background-color: #ddd;
    width: 100%;
}
.blue
{
    color: blue;
}
</style>
    <script src="/Content/js/jquery-1.4.2.min.js" type="text/javascript"></script>    
    <script src="/Content/js/jquery.idle-timer.js" type="text/javascript"></script>
    <script src="/Scripts/OnlineReg.js" type="text/javascript"></script>
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
    <% }
       else
       { %>
    <%=Model.Instructions%>
    <form class="DisplayEdit" action="/OnlineReg/CompleteRegistration/<%=Model.qtesting %>" method="post">
    <% Html.RenderPartial("List", Model); %>
    </form>
    <% } %>
</asp:Content>