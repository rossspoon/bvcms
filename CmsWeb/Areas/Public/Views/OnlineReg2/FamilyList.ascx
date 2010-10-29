<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CmsWeb.Models.OnlineRegModel2>" %>
<div>
    <% foreach (var fm in Model.FamilyMembers())
       { %>
    Register <a href="/OnlineReg2/Register/<%=fm.PeopleId %>" class="submitlink"><%=fm.Name %></a> (<%=fm.Age %>)<br />
    <% } %>
    <%=Html.ValidationMessage("classidfam")%>
</div>