<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CMSWeb.Models.OnlineRegModel>" %>
<%=Html.Hidden("m.divid", Model.divid) %>
<%=Html.Hidden("m.orgid", Model.orgid) %>
<%=Html.Hidden("m.testing", Model.testing) %>
<table cellpadding="0" cellspacing="0">
<% 
    for (var i = 0; i < Model.List.Count; i++)
    {
        var p = Model.List[i];
        p.index = i;
%>
<tr <%=p.index % 2 == 1 ? "class='alt'" : "" %>><td>
<%=Html.Hidden("m.List.index", p.index) %>
<%=Html.Hidden3("m.List[" + p.index + "].orgid", p.orgid) %>
<%=Html.Hidden3("m.List[" + p.index + "].divid", p.divid) %>
<%=Html.Hidden3("m.List[" + p.index + "].ShowAddress", p.ShowAddress) %>
<%
        p.LastItem = i == (Model.List.Count - 1);
        if (p.org != null && (p.Found == true || p.IsNew) && p.IsFilled != true)
        {
            Html.RenderPartial("PersonDisplay", p);
            if (p.OtherOK)
                Html.RenderPartial("OtherDisplay", p);
            else
                Html.RenderPartial("OtherEdit", p);
        }
        else
            Html.RenderPartial("PersonEdit", p);
%>
</td></tr>
<%  }
    var last = Model.List[Model.List.Count - 1];
    if (last.OtherOK)
    {
%>
<tr><td colspan="2">
        <input id="submitit" type="submit"
             class="submitbutton" value='Complete Registration and Pay <%=Model.TotalAmount().ToString("c") %>' />
<% if (Model.org.AllowOnlyOne != true && Model.org.AskTickets != true)
   { %>
        or <a href="/OnlineReg/AddAnotherPerson/" class="submitbutton">Add another household member</a>
<% } %>
</td></tr>
<%  } %>
</table>