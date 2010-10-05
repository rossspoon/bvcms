<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<%  int tp, tf;
    string ts, tt;
    bool finished;
    CmsWeb.Areas.Main.Controllers.TagDuplicatesStatus.GetStatus(out tp, out tf, out ts, out tt, out finished); 
%>
<html>
<head>
    <% if (!finished)
       { %>
    <meta http-equiv="refresh" content="5" />
    <% } %> 
</head>
<body>
    <h2>Tag Duplicates Progress</h2>
    <table>
    <tr><td>Processed</td><td><%=tp %></td></tr>
    <tr><td>Found</td><td><%=tf %></td></tr>
    <tr><td>Speed</td><td><%=ts %></td></tr>
    <tr><td>Time</td><td><%=tt %></td></tr>
    </table>
    <% if (finished)
       { %>
       <a href="/mytags.aspx">finished</a>
    <% } %> 
</body>
</html>