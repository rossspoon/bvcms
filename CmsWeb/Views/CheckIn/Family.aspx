<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<System.Collections.Generic.IEnumerable<CMSWeb.Models.Attendee>>" %>
<Attendees>
    <% foreach (var c in Model)
       { %>
    <attendee id="<%=c.Id %>" 
        name="<%=c.DisplayName %>" 
        bday="<%=c.Birthday %>" 
        org="<%=c.DisplayClass %>" 
        orgid="<%=c.OrgId %>" 
        loc="<%=c.Location %>" 
        gender="<%=c.Gender %>" 
        age="<%=c.Age %>" 
        numlabels="<%=c.NumLabels %>" 
        checkedin="<%=c.CheckedIn %>" />
    <% } %>
</Attendees>
