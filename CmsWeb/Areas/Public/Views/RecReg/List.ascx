<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CMSWeb.Models.RecRegModel>" %>
<table cellpadding="0" cellspacing="0">
<% 
        if (Model.RecAgeDiv != null && (Model.Found == true || Model.IsNew) && Model.IsFilled != true)
        {
            Html.RenderPartial("PersonDisplay", Model);
            if (Model.OtherOK)
                Html.RenderPartial("OtherDisplay", Model);
            else
                Html.RenderPartial("OtherEdit", Model);
        }
        else
            Html.RenderPartial("PersonEdit", Model);
%>
</table>