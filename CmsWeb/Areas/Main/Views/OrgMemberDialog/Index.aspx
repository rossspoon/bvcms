<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<CmsData.OrganizationMember>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Index</title>
    <link href="/Content/style.css" rel="stylesheet" type="text/css" />
    <link href="/Content/jquery-ui-1.8.1.custom.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <script src="/Content/js/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script src="/Content/js/jquery-ui-1.8.1.custom.min.js" type="text/javascript"></script>    
    <script src="/Scripts/OrgMemberDialog.js" type="text/javascript"></script>
<% string comboid = "-" + Model.OrganizationId + "-" + Model.PeopleId; %>
<%=Html.Hidden("from") %>
<table>
    <tr>
    <td>Attendance:</td>
        <td colspan="3">
            <a href='<%="/AttendStrDetail.aspx?id={1}&oid={0}".Fmt(Model.OrganizationId, Model.PeopleId) %>' 
            target="_blank" style="font-family: Courier New"><%=Model.AttendStr %></a>
        </td>
    </tr>
    <tr>
    <td valign="top">
    <div style="border: solid 1px black; padding:1em">
    <form class="DisplayEdit" action="/OrgMemberDialog/Update/<%=Model.OrganizationId %>&pid=<%=Model.PeopleId %>" method="post">
    <% Html.RenderPartial("Display", Model); %>
    </form>
    </div>
    </td>
    <td></td>
    <td valign="top">
        <div style="border: solid 1px black; padding:1em">
        <strong>Groups:</strong><br />
        <% foreach (var mt in Model.Organization.MemberTags.OrderBy(t => t.Name))
           { %>
           <input id="smallgroup<%=comboid %>-<%=mt.Id %>" class="ckbox" type="checkbox" <%=Model.OrgMemMemTags.Any(omt => omt.MemberTagId == mt.Id) ? "checked='checked'" : "" %> />
           <%=mt.Name %><br />
        <% } %>
        </div>
    </td>
    </tr>
</table>
</body>
</html>
