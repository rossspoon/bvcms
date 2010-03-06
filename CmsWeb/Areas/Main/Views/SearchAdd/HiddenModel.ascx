<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CMSWeb.Models.SearchModel>" %>
<input id="from" type="hidden" name="m.from" value="<%=Model.from%>" />
<input type="hidden" name="m.type" value="<%=Model.type%>" />
<input type="hidden" name="m.typeid" value="<%=Model.typeid%>" />
