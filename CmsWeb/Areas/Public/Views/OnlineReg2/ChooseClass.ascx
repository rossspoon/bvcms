<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CmsWeb.Models.OnlineRegModel2>" %>
<% if (Model.Classes().Count() > 1)
   { %>
        <%=Html.DropDownList3(null, "m.classid", Model.Classes(), Model.classid.ToString())%>
        <%=Html.ValidationMessage("classid")%>
<% }
   else
   { %>
        <span class="instruct">Here is the only available class:</span><br />
        <%=Model.Classes().Single().Text%>
        <%=Html.Hidden("m.classid", Model.Classes().Single().Value)%>
<% } %>
<% if (Model.FilledClasses().Count() > 0)
   { %>
    <h3 style="color:Red;padding-top:8px">The following groups are filled:</h3>
    <div style="padding-left:1em">
    <% foreach (var c in Model.FilledClasses())
       { %>
       <%=c%><br />
    <% } %>
    </div>
<% } %>