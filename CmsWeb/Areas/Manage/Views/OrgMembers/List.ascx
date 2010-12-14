<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CmsWeb.Models.OrgMembersModel>" %>
<%=Html.Hidden("Sort") %>
<%=Html.Hidden("Direction") %>
<div class="modalPopup">
    <table>
        <tr>
            <th>Program:</th>
            <td><%=Html.DropDownList("ProgId", Model.Programs())%></td>
            <th>Source:</th>
            <td><%=Html.DropDownList("SourceId", Model.Organizations())%></td>
        </tr>
        <tr>
            <th>Division:</th>
            <td><%=Html.DropDownList("DivId", Model.Divisions())%></td>
            <th>Target:</th>
            <td><%=Html.DropDownList("TargetId", Model.Organizations2())%></td>
        </tr>
        <tr>
            <td></td>
            <td colspan="3">
                Grades: <%=Html.TextBox("Grades") %>&nbsp;&nbsp;&nbsp;
                <%=Html.CheckBox("MembersOnly")%> Exclude Teachers&nbsp;&nbsp;&nbsp;
                <input type="submit" name="move" value="Move" />
                <a href="#" id="EmailNotices">Email Room Notices(<%=Model.MovedCount() %>)</a>
            </td>
        </tr>
    </table>
</div>
<br />
Count: <%=Model.Count() %>
<table class="grid">
<thead>
<tr>
    <th><a href="#" class="sortable">Mixed</a></th>
    <th align="left"><a href="#" class="sortable">Name</a></th>
    <th align="left"><a href="#" class="sortable">Organization</a></th>
    <th>Type</th>
    <th><a href="#" class="sortable">Gender</a></th>
    <th><a href="#" class="sortable">Grade</a></th>
    <th>Age</th>
    <th><a href="#" class="sortable">DOB</a></th>
    <th align="left">Request</th>
</tr>
</thead>
<tbody>
<% foreach (var m in Model.Members())
   { %>
<tr>
    <td><input name="List" type="checkbox" <%=m.Checked %> value="<%=m.PeopleId %>.<%=m.OrgId %>" class="check" /></td>
    <td><a href="/Person/Index/<%=m.PeopleId %>"><%=m.Name%></a></td>
    <td><a href="/Organization/Index/<%=m.OrgId %>"><%=m.OrgName %></a></td>
    <td><%=m.MemberStatus %></td>
    <td align="center"><%=m.Gender %></td>
    <td align="center"><%=m.Grade %></td>
    <td><%=m.Age %></td>
    <td><%=m.DOB %></td>
    <td><%=m.Request %></td>
</tr>
<% } %>
</tbody>
</table>
