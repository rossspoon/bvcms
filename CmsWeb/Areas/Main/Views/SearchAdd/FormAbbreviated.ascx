<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CMSWeb.Models.SearchModel>" %>
<input type="hidden" name="m.type" value="<%=Model.type%>" />
<input type="hidden" name="m.typeid" value="<%=Model.typeid%>" />
<input type="hidden" name="m.from" value="<%=Model.from%>" />
<input type="hidden" name="m.name" value="<%=Model.name%>" />
<input type="hidden" name="m.dob" value="<%=Model.dob%>" />
<input type="hidden" name="m.address" value="<%=Model.address%>" />
<input type="hidden" name="m.phone" value="<%=Model.phone%>" />
<% 
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
<input type="hidden" name="m.List.Index" value="<%=n%>" />
<input type="hidden" name="m.List[<%=n %>].FamilyId" value="<%=np.FamilyId %>" />
<input type="hidden" name="m.List[<%=n %>].homephone" value="<%=np.homephone%>" />
<input type="hidden" name="m.List[<%=n %>].address" value="<%=np.address%>" />
<input type="hidden" name="m.List[<%=n %>].address2" value="<%=np.address2%>" />
<input type="hidden" name="m.List[<%=n %>].zip" value="<%=np.zip%>" />
<input type="hidden" name="m.List[<%=n %>].city" value="<%=np.city%>" />
<input type="hidden" name="m.List[<%=n %>].state" value="<%=np.state%>" />
<table width="100%">
    <tr><th colspan="2">Add Person To Existing Family</th>
        <td align="right"><a href="/SearchAdd/PersonCancel/<%=n %>" class="formlink" title="<%=Model.List.Count > 0 ? "back to selections" : "back to search person"%>">go back</a></td>
    </tr>
    <% Html.RenderPartial("EditPerson", np);%>
    <tr><td colspan="2"></td>
        <td><a href="/SearchAdd/AddToFamily/" class="bt formlink default">Submit</a></td>
    </tr>
</table>