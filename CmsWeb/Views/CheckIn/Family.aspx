<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<System.Collections.Generic.IEnumerable<CMSWeb.Controllers.Attendee>>" %>
<Attendees>
    <% foreach (var c in Model)
       { %>
    <attendee id="<%=c.Id %>" name="<%=c.Name %>" bday="<%=c.Birthday %>" org="<%=c.Class %>" orgid="<%=c.OrgId %>" loc="<%=c.Location %>" gender="<%=c.Gender %>" age="<%=c.Age %>" numlabels="<%=c.NumLabels %>" />
    <% } %>
</Attendees>
