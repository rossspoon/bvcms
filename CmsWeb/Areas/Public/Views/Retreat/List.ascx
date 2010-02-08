<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CMSWeb.Models.RetreatModel>" %>
<table cellpadding="0" cellspacing="0">
<% 
        if (Model.Found == true || Model.IsNew)
        {
            Html.RenderPartial("PersonDisplay", Model);
%>
    <tr><td colspan="2"><input id="submitit" type="submit" class="submitbutton" 
        value="Complete Registration and Pay" /></td></tr>
<%
        }
        else
            Html.RenderPartial("PersonEdit", Model);
%>
</table>