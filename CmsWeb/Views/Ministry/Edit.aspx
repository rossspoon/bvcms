<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.MinistryModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Edit</h2>

    <%= Html.ValidationSummary("Edit was unsuccessful. Please correct the errors and try again.") %>

    <% using (Html.BeginForm("Update", "Ministry")) {%>

        <fieldset>
            <legend>Fields</legend>
            <p>
                <label for="MinistryId">MinistryId:</label>
                <%= Html.Encode(Model.ministry.MinistryId) %>
            </p>
            <p>
                <label for="MinistryName">MinistryName:</label>
                <%= Html.TextBox("ministry.MinistryName", Model.ministry.MinistryName)%>
                <%= Html.ValidationMessage("MinistryName", "*")%>
            </p>
            <p>
                <label for="MinistryDescription">MinistryDescription:</label>
                <%= Html.TextBox("ministry.MinistryDescription", Model.ministry.MinistryDescription)%>
                <%= Html.ValidationMessage("MinistryDescription", "*")%>
            </p>
            <p>
                <input type="submit" value="Save" />
            </p>
        </fieldset>

    <% } %>

    <div>
        <%=Html.ActionLink("Back to List", "Index") %>
    </div>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>

