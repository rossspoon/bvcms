<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<System.Collections.Generic.IEnumerable<CMSWeb.Controllers.Fam>>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Match</title>
    <link href="/Content/touch.css" rel="stylesheet" type="text/css" />
    <script src="/Content/js/jquery-1.3.2.min.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(function() {
            $('td.btn').click(function() {
                var id = $(this).attr("id").split('.');
                window.location = "/Checkin/Children/" + id[1];
                return false;
            });
        });

    </script>
</head>
<body>
    <a href="javascript:history.go(-1)" class="btn"><span class="goback">Go&nbsp;Back</span></a>
    <div>
        <table cellspacing="8" cellpadding="0"style="width: 100%">
            <% foreach (var f in Model)
               { %>
            <tr>
                <td id="fam.<%=f.FamId %>" class="btn">
                    <%= f.ToString()%>
                </td>
            </tr>
            <% } %>
        </table>
    </div>
</body>
</html>
