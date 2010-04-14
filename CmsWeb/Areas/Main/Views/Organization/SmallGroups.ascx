<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CMSWeb.Models.OrganizationPage.OrganizationModel>" %>
<%=Html.DropDownList("smallgroupid", Model.MemberModel.SmallGroups()) %>
