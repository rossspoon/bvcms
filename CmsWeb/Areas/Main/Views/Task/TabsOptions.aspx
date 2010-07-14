<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<CmsWeb.Models.TaskModel>" %>
<% Html.RenderPartial("Tabs", Model); %>
<---------->
<% foreach(var item in Model.ActionItems())
   { %>
<option value="<%=item.Value%>"<%=(item.Selected ? " selected" : "")%>><%=item.Text%></option>
<% } %>