<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CMSWeb.Models.OrgMembersModel>" %>
<%=Html.Hidden("Sort") %>
<%=Html.Hidden("Dir") %>
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
            <td></td><td></td>
            <td colspan="2">
                <%=Html.CheckBox("MembersOnly")%> Exclude Teachers&nbsp;&nbsp;&nbsp;
                <%=Html.SubmitButton("move", "Move") %>
            </td>
        </tr>
    </table>
</div>
<br />
<table class="grid">
<thead>
<tr>
    <th><a href="#" class="sortable">Mixed</a></th>
    <th align="left"><a href="#" class="sortable">Name</a></th>
    <th align="left"><a href="#" class="sortable">Organization</a></th>
    <th>Type</th>
    <th><a href="#" class="sortable">Grade</a></th>
    <th>Age</th>
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
    <td><%=m.Grade %></td>
    <td><%=m.Age %></td>
    <td><%=m.Request %></td>
</tr>
<% } %>
</tbody>
</table>
