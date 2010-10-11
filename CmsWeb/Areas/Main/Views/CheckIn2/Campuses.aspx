<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<IEnumerable<CmsWeb.Areas.Main.Controllers.Checkin2Controller.CampusItem>>" %>
<Campuses>
    <% foreach (var i in Model)
       { %>
    <campus id="<%=i.Campus.Id %>" name="<%=i.Campus.Description %>" password="<%=i.password %>" />
    <% } %>
</Campuses>
