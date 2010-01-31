<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IList<CMSWeb.Models.PersonEventModel>>" %>
<% 
    for(var i = 0; i < Model.Count; i++)
    {
        var m = Model[i];
        m.index = i;
        if (m.Found == true || m.IsNew)
            Html.RenderPartial("PersonDisplay", m);
        else
            Html.RenderPartial("PersonEdit", m);
    }
    var last = Model[Model.Count - 1];
    if (last.Found == true || last.IsNew)
    {
%>
    <tr><td colspan="2"><input type="submit" class="submitbutton" value="Complete Registration and Pay <%=ViewData["fee"]%>" />
        or <a href="/Event/AddAnotherPerson/" class="submitbutton">Add another household member</a></td>
    </tr>
<%  } %>
