<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CmsWeb.Models.PersonPage.DuplicatesModel>" %>
<table class="grid" cellpadding="3">
<thead>
    <tr>
        <th align="left" valign="top">First<br />Nick<br />Middle</th>
        <th align="left" valign="top">Last<br />Maiden</th>
        <th align="left" valign="top">BirthDay</th>
        <th align="left" valign="top">Email<br />Member Status</th>
        <th align="left" valign="top">FamAddr<br />PerAddr</th>
    </tr>
</thead>
<tbody>
<%  var list = Model.person.PossibleDuplicates2(); %>
    <tr><td colspan="9"><span style="font-weight:bold">this person</span></td></tr>
<%      Html.RenderPartial("DuplicateRow", list[0]);  %>
    <tr><td colspan="9"><span style="font-weight:bold">Last and 3 of (first, email, bday, phone, addr)</span></td></tr>
<% foreach (var d in Model.person.PossibleDuplicates())
        Html.RenderPartial("DuplicateRow", d); %>
    <tr><td colspan="9"><span style="font-weight:bold">first and partial birthday</span></td></tr>
<% foreach (var d in list.Where(d => d.s1 == true))
        Html.RenderPartial("DuplicateRow", d);  %>
    <tr><td colspan="9"><span style="font-weight:bold">first and birthday</span></td></tr>
<% foreach (var d in list.Where(d => d.s2 == true))
        Html.RenderPartial("DuplicateRow", d);  %>
    <tr><td colspan="9"><span style="font-weight:bold">first and last and no birthday</span></td></tr>
<% foreach (var d in list.Where(d => d.s3 == true))
        Html.RenderPartial("DuplicateRow", d);  %>
    <tr><td colspan="9"><span style="font-weight:bold">first and address</span></td></tr>
<% foreach (var d in list.Where(d => d.s4 == true))
        Html.RenderPartial("DuplicateRow", d);  %>
    <tr><td colspan="9"><span style="font-weight:bold">first and email</span></td></tr>
<% foreach (var d in list.Where(d => d.s5 == true))
        Html.RenderPartial("DuplicateRow", d);  %>
    <tr><td colspan="9"><span style="font-weight:bold">last and birthday</span></td></tr>
<% foreach (var d in list.Where(d => d.s6 == true))
        Html.RenderPartial("DuplicateRow", d); %>
</tbody>
</table>