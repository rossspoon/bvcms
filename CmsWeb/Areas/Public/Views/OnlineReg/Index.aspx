<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.OnlineRegModel>" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html>
<head>
<title>Online Registration</title>
<%=DbUtil.Content("OnlineRegHeader", @"
<meta http-equiv='Content-Type' content='text/html; charset=iso-8859-1' />
<link rel='shortcut icon' href='/Content/favicon.ico' type='image/x-icon' />
<link href='/Content/Site2.css?v=2' rel='stylesheet' type='text/css' />
") %>
</head>
<body>
<%=DbUtil.Content("OnlineRegTop", @"
<div class='page'>
<div id='header'>
<div id='title'>
<h1><img alt='logo' src='/Content/Crosses.png' align='middle' />&nbsp;Online Registration</h1>
</div>
</div>
<div id='main'>
") %>
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
<%=DbUtil.Content("OnlineRegBottom", @"
<div id='footer'></div>
</div>
</div>
") %>
</body>
</html>
