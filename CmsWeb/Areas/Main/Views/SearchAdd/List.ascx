<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IList<CMSWeb.Models.SearchAddModel>>" %>
<table cellpadding="0" cellspacing="0">
<% 
    CMSWeb.Models.SearchAddModel m = null;
    for(var i = 0; i < Model.Count; i++)
    {
        m = Model[i];
        m.index = i;
        m.LastItem = i == (Model.Count - 1);
        if (m.Found == true || m.IsNew)
            Html.RenderPartial("PersonDisplay", m);
        else
            Html.RenderPartial("PersonEdit", m);
    }
    if (ViewData["mode"] == "search")
    {
%>
    <tr><td></td>
    </tr>
<%  }
    else
    { %>
    <tr><td colspan="2"><input id="submitit" type="submit"
             class="submitbutton" value='Add Complete' />
        or <a href="/SearchAdd/AddAnotherPerson/" class="submitbutton">Search for another person</a></td>
    </tr>
 <% } %>
</table>