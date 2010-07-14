<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CmsWeb.Models.OrganizationPage.OrganizationModel>" %>
<%=Html.DropDownList("smallgroupid", Model.MemberModel.SmallGroups()) %>
