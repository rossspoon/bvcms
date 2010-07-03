<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CMSWeb.Models.DivisionModel>" %>
<table id="results">
<thead>
    <tr>
        <th></th>
        <th>Id</th>
        <th>Division</th>
        <th>Program</th>
        <th>RptLine</th>
        <th>TargetProg</th>
        <th>MainProg</th>
        <th></th>
    </tr>
</thead>
<tbody>
<% foreach (var i in Model.DivisionList()) 
   {
       Html.RenderPartial("Row", i);
   } %>
</tbody>
</table>
