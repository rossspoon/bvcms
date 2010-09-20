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

    <h2>Registration Received</h2>
    <p style="color: Black">
        Thank you for registering for <%=ViewData["orgname"] %>.  
        You should receive a confirmation email at <%=ViewData["email"] %> shortly.
    </p>
    <p style="color: Blue"><a href='<%=ViewData["URL"] %>'>Start a New Registration</a></p>
</asp:Content>
