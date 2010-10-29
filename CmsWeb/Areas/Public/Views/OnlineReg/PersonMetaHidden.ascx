<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CmsWeb.Models.OnlineRegPersonModel>" %>
<%=Html.Hidden3("m.List[" + Model.index + "].orgid", Model.orgid)%>
<%=Html.Hidden3("m.List[" + Model.index + "].divid", Model.divid)%>
<%=Html.Hidden3("m.List[" + Model.index + "].classid", Model.classid)%>
<%=Html.Hidden3("m.List[" + Model.index + "].PeopleId", Model.PeopleId) %>
<%=Html.Hidden3("m.List[" + Model.index + "].Found", Model.Found) %>
<%=Html.Hidden3("m.List[" + Model.index + "].IsNew", Model.IsNew) %>
<%=Html.Hidden3("m.List[" + Model.index + "].OtherOK", Model.OtherOK) %>
<%=Html.Hidden3("m.List[" + Model.index + "].LoggedIn", Model.LoggedIn) %>
<%=Html.Hidden3("m.List[" + Model.index + "].IsValidForExisting", Model.IsValidForExisting)%>
<%=Html.Hidden3("m.List[" + Model.index + "].IsValidForNew", Model.IsValidForNew)%>
<%=Html.Hidden3("m.List[" + Model.index + "].ShowAddress", Model.ShowAddress) %>