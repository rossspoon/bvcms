<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<SelectListItem>>" %>
<%=Html.DropDownList("TagDiv", Model) %>