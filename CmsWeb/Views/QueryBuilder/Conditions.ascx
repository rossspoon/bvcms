<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CMSWeb.Models.QueryModel>" %>
<% foreach(var c in Model.ConditionList())
   { %>
   <li class="conditionPopup">
        <img src="/images/1pix.gif" width="<%=c.Level.ToString() %>" height="1px" />
        <img src="/images/DownRight.gif" width="19px" height="12px" />
        <a id='<%=c.Clause.QueryId %>' href='#'><%=c.Clause.ToString() %></a>
        <ul class="popupMenu no-style" style="margin-left:0">
        <li><a href="#" class="insGroupAbove">Insert Group Above</a></li>
        <li><a href="#" class="copyAsNew">Copy As New</a></li>
        </ul>
   </li>
<% } %>

