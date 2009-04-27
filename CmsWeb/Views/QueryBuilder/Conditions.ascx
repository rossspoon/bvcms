<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CMSWeb.Models.QueryModel>" %>
<% foreach(var c in Model.ConditionList())
   { %>
   <li>
        <img src="/images/1pix.gif" width="<%=c.Level.ToString() %>" height="1px" />
        <img src="/images/DownRight.gif" width="19px" height="12px" />
        <a id='<%=c.Clause.QueryId %>'
            href='#'><%=c.Clause.ToString() %></a>
   </li>
<% } %>
