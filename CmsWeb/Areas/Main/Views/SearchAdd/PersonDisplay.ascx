<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CMSWeb.Models.SearchPersonModel>" %>
<tr>
    <td><%=Model.first + " " + Model.last%></a></td>
    <td class="addrcol" title="<%=Model.ToolTip %>"><%=Model.address%></td>
    <td><%=Model.CityStateZip%></td>
    <td><%=Model.age%></td>
    <td align="right"><a class="formlink" href="/SearchAdd/PersonCancel/<%=Model.index %>" title="remove this person from the list">remove</a></td>
</tr>
