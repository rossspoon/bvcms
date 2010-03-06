<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CMSWeb.Models.SearchModel>" %>
<% 
    Html.RenderPartial("HiddenModel", Model);
    var n = 0;
    for (; n < Model.List.Count - 1; n++)
    {
        var p = Model.List[n];
        p.index = n;
        Html.RenderPartial("HiddenPerson", p);
    }
    var np = Model.List[n];
    np.index = n;
%>
<input type="hidden" name="m.List.Index" value="<%=n %>" />
<input type="hidden" name="m.list[<%=n %>].FamilyId" value="<%=np.FamilyId %>" />
<table width="100%">
    <tr><th colspan="2">Add Person To New Family</th>
        <td align="right"><a href="/SearchAdd/PersonCancel/<%=n %>" class="formlink" title="<%=Model.List.Count > 0 ? "back to selections" : "back to search person"%>">go back</a></td>
    </tr>
<% Html.RenderPartial("EditPerson", np);
   Html.RenderPartial("EditAddress", np);
%>
    <tr><td colspan="2"></td>
        <td><a href="/SearchAdd/AddNewFamily/" class="bt formlink default">Submit</a></td>
    </tr>
</table>