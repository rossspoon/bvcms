<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.TaskModel>" %>
<% Html.RenderPartial("Tabs", ViewData.Model); %>
<---------->
<% foreach(var item in ViewData.Model.ActionItems())
   { %>
<option value="<%=item.Value%>"<%=(item.Selected ? " selected" : "")%>><%=item.Text%></option>
<% } %>