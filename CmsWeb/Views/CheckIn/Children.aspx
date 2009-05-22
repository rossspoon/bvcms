<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<System.Collections.Generic.IEnumerable<CMSWeb.Controllers.Child>>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Match</title>
    <link href="/Content/touch.css" rel="stylesheet" type="text/css" />
    <style>
        table th
        {
            text-align: left;
        }
    </style>
</head>
<body>

    <script src="/Content/js/jquery-1.2.6.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(function() {
            $('a.btn').click(function() {
                var id = $(this).attr("id").split('.');
                if (id[0] == "select") {
                    var t = $("span", this).text();
                    if (t == "ü")
                        $("span", this).text(" ");
                    else
                        $("span", this).text("ü");
                    return false;
                }
                return true;
            });
        });

    </script>
    <a href="javascript:history.go(-1)" class="btn"><span class="goback">Go&nbsp;Back</span></a>

    <div>
        <table cellspacing="0" cellpadding="8" style="width: 100%; font-size: large">
            <tr>
                <th>
                    Name
                </th>
                <th>
                    Birthday
                </th>
                <th>
                    Last Class
                </th>
                <th>
                    Select
                </th>
                <th>
                </th>
            </tr>
            <% foreach (var c in Model)
               { %>
            <tr>
                <td>
                    <%=c.Name%>
                </td>
                <td>
                    <%=c.Birthday %>
                </td>
                <td>
                    <%=c.Class%>
                </td>
                <td>
                    <a id="select.<%=c.Id %>" href="#" class="btn"><span style="font-family: Wingdings">
                        &nbsp;</span></a>
                </td>
                <td>
                    <a id="print.<%=c.Id %>" href="#" class="btn"><span style="font-size: medium">Print<br />
                        Labels</span></a>
                </td>
            </tr>
            <% } %>
        </table>
    </div>
</body>
</html>
