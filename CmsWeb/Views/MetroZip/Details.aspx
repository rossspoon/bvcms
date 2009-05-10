<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CmsData.Zip>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Detail</h2>

    <fieldset>
        <legend>Fields</legend>
        <p>
            ZipCode:
            <%= Html.Encode(Model.ZipCode) %>
        </p>
        <p>
            MetroMarginalCode:
            <%= Html.Encode(Model.MetroMarginalCode) %>
        </p>
    </fieldset>
    <p>

        <%=Html.ActionLink("Edit", "Edit", new { id=Model.ZipCode }) %> |
        <%=Html.ActionLink("Delete", "Delete", new { id=Model.ZipCode }) %> |
        <%=Html.ActionLink("Back to List", "Index") %>
    </p>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>

