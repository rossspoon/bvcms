<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<System.Collections.Generic.IEnumerable<CMSWeb.Models.FamilyInfo>>" %>
<Families>
    <% foreach (var f in Model)
       { %>
    <family id="<%=f.FamilyId %>" name="<%=f.Name %>" />
    <% } %>
</Families>
