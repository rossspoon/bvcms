<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/onlinereg.Master" Inherits="System.Web.Mvc.ViewPage" %>
<asp:Content ID="registerContent" ContentPlaceHolderID="MainContent" runat="server">
    <script src="/Content/js/jquery-1.4.2.min.js" type="text/javascript"></script>
    <script src="/Content/js/jquery.idle-timer.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function() {
            $(document).bind("idle.idleTimer", function() {
                window.location.href = '<%=ViewData["URL"] %>';
            });
            var tmout = parseInt('<%=ViewData["timeout"] %>');

            $(document).bind("keydown", function() {
                $(document).unbind("keydown");
                $.idleTimer(tmout);
            });
            $.idleTimer(tmout);
        });
    </script>

    <h2>Account Created</h2>
    <p style="color: Black">
        Thank you for creating an account on our church database.
        You should receive a confirmation email at <%=Util.ObscureEmail((string)ViewData["email"]) %> shortly.
        (email obscured on purpose)
    </p>
    <%
       if (((string)Session["gobackurl"]).HasValue())
       { %>
    <p style="color: Blue"><a href='<%=Session["gobackurl"] %>'>Go back to your registration</a></p>
    <% } %>
</asp:Content>
