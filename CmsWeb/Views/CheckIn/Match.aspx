<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<System.Collections.Generic.IEnumerable<CMSWeb.Controllers.Fam>>" %><?xml version="1.0" encoding="utf-8"?>
<Fams>
    <% foreach (var f in Model)
       { %>
    <fam id="<%=f.FamId %>">
        <%=f.ToString() %>
    </fam>
    <% } %>
</Fams>
