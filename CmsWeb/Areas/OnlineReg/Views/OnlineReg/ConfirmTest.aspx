<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<IEnumerable<CmsWeb.Areas.OnlineReg.Controllers.OnlineRegController.ConfirmTestInfo>>" %>
<html>
<head><title>Past Confirmations</title></head>
<body>
<table border=1 cellpadding=2 cellspacing=0>
<thead><tr><th>Id</th><th>Time</th><th>Header</th><th>User</th></tr></thead>
<% foreach(var i in Model)
   {
       string link = null;
       if (i.m.divid.HasValue)
           link = "<a href='/OrgSearch/Index/?div={0}'>{1}</a>".Fmt(i.m.divid, i.m.Header);
       else if (i.m.orgid > 0)
           link = "<a href='/Organization/Index/{0}'>{1}</a>".Fmt(i.m.orgid, i.m.Header);
       else
           link = i.m.Header;
        %>
   <tr>
       <td><%=i.ed.Id%></td>
       <td><%=i.ed.Stamp.Value%></td>
       <td><%=link %></td>
       <td><%=i.m.LoginName%></td>
   </tr>
   <% for(var n = 0;n < i.m.List.Count; n++)
      {
          var p = i.m.List[n]; %>
   <tr><td></td><td>Item <%=n %></td><td colspan="2">
   <%=p.first %> <%=p.last %><br />
   <%=p.dob %> (<%=p.age %>)<br/>
   <%=p.phone.FmtFone() %><br/>
   <%=p.email %><br/>
       <% if (p.person != null && p.person.EmailAddress == p.email)
          { %>
   <%=p.person.EmailAddress%><br/>
       <% }
          if (p.mname.HasValue())
          { %>
   <%=p.mname%><br/>
       <% }
          if (p.fname.HasValue())
          { %>
   <%=p.fname%><br/>
       <% }
          if (p.ShowAddress)
          { %>
   <%=p.address%>, <%=p.city%><br/>
       <% }
      } %>
   </td></tr>
<% } %>
</table>
</body>
</html>