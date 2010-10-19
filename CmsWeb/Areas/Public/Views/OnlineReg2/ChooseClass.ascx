<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CmsWeb.Models.OnlineRegPersonModel2>" %>
<table cellspacing="6">
<% if ((Model.classid ?? 0) > 0)
   { %>
    <tr>
        <td colspan="3"><label for="classid">Chosen Class:</label>
        <%=Html.CodeDesc("classid", Model.Classes())%></td>
    </tr>
<% }
   else
   {
       if (Model.FilledClasses().Count() > 0)
       { %>
    <tr>
        <td>
        <span style="color:Red">Filled Classes:</span><br />
        <% foreach (var c in Model.FilledClasses())
           { %>
           <%=c%><br />
        <% } %>
        </td>
    </tr>
    <% } %>
    <tr>
        <td>
<% if (Model.Classes().Count() > 1)
   { %>
        <span class="blue">Select a class:</span><br />
        <%=Html.DropDownList3(null, "m.List[" + Model.index + "].classid", Model.Classes(), Model.classid.ToString())%>
        <%=Html.ValidationMessage("classid")%>
<% }
   else
   { %>
        <span class="blue">Here is the only available class:</span><br />
        <%=Model.Classes().Single().Text%>
        <%=Html.Hidden("m.List[" + Model.index + "].classid", Model.Classes().Single().Value)%>
<% } %>
        </td>
    </tr>
<% } %>
</table>