<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/onlinereg.Master" Inherits="System.Web.Mvc.ViewPage<CmsWeb.Models.OnlineRegModel>" %>
<asp:Content ID="registerContent" ContentPlaceHolderID="MainContent" runat="server">
    <script src="/Content/js/jquery-1.4.4.min.js" type="text/javascript"></script>
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
<p style="color: Black">
Email sent to <%=Util.ObscureEmail((string)ViewData["email"]) %> (email obscured on purpose)
</p>
<%  if (Model.ManagingSubscriptions())
    { %>
<p><strong>One-Time Link</strong><br/>
We have sent you a One-Time Link for you to manage your subscriptions. You should receive that shortly.</p>
<%  }
    if (Model.List[0].CreatedAccount)
    { %>
<p><strong>Account Created</strong><br />
We have created an account for you on our church database. You should receive your credentials shortly.</p>
<%  }
    if (Model.List[0].SawExistingAccount)
    { %>
<p><strong>Account Aready Exists</strong><br />
We noticed you already have an account in our church database. We sent you some instructions on how to reset your password.
You should receive your credentials shortly.</p>
<%  }
    if (((string)Session["gobackurl"]).HasValue())
    { %>
<p style="color: Blue"><a href='<%=Session["gobackurl"] %>'>Go back to your registration</a></p>
<%  } %>
</asp:Content>
