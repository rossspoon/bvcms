﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CmsWeb.Models.OnlineRegModel>" %>
<div>
    <% foreach (var fm in Model.FamilyMembers())
       { %>
    Register <a href="/OnlineReg/Register/<%=fm.PeopleId %>" class="submitlink"><%=fm.Name %></a> (<%=fm.Age %>)<br />
    <% } %>
    <%=Html.ValidationMessage("classidfam")%>
</div>