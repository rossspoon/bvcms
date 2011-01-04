<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CmsWeb.Models.SlotModel.SlotInfo>" %>
<% 
    if(ViewData["returnval"] != null)
    { %>
<%=ViewData["returnval"] %>
<!-- -->
 <% } %>
<td class="<%=Model.classAttr %>" title="<%=Model.slot.name %> - <%=string.Join(" - ", Model.owners) %>">
<input id="<%=Html.Encode(Model.slot.name) %>" type="checkbox" <%=Model.checkedstr %> />
</td>
