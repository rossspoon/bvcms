<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<IEnumerable<CmsData.Campu>>" %>
<Campuses>
    <% foreach (var c in Model)
       { %>
    <campus id="<%=c.Id %>" name="<%=c.Description %>" />
    <% } %>
</Campuses>
