<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IList<CMSWeb.Models.EventModel>>" %>
<table cellpadding="0" cellspacing="0">
<% 
    decimal fee = CMSWeb.Areas.Public.Controllers.EventController.ComputeFee(Model);
    for(var i = 0; i < Model.Count; i++)
    {
        var m = Model[i];
        m.index = i;
        m.LastItem = i == (Model.Count - 1);
        if (m.Found == true || m.IsNew)
            Html.RenderPartial("PersonDisplay", m);
        else
            Html.RenderPartial("PersonEdit", m);
    }
    var last = Model[Model.Count - 1];
    if (last.Found == true || last.IsNew)
    {
%>
    <tr><td></td><td><%=Html.ValidationMessage("ShirtSize") %></td></tr>

    <tr><td colspan="2"><input id="submitit" type="submit"
             class="submitbutton" value='Complete Registration and Pay <%=fee.ToString("c") %>' />
        or <a href="/Event/AddAnotherPerson/" class="submitbutton">Add another household member</a></td>
    </tr>
<%  } %>
</table>