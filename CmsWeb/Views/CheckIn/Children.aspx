<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<System.Collections.Generic.IEnumerable<CMSWeb.Controllers.Child>>" %>
<Children>
    <% foreach (var c in Model)
       { %>
    <child id="<%=c.Id %>" name="<%=c.Name %>" bday="<%=c.Birthday %>" org="<%=c.Class %>" orgid="<%=c.OrgId %>" loc="<%=c.Location %>" gender="<%=c.Gender %>" age="<%=c.Age %>" />
    <% } %>
</Children>
