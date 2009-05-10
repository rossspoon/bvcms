<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.MetroZipModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Edit</h2>

    <%= Html.ValidationSummary("Edit was unsuccessful. Please correct the errors and try again.") %>

    <% using (Html.BeginForm("Update", "MetroZip")) {%>

        <fieldset>
            <legend>Fields</legend>
            <p>
                <label for="ZipCode">ZipCode:</label>
                <%= Html.TextBox("zip.ZipCode") %>
            </p>
            <p>
                <label for="MinistryName">MinistryName:</label>
                <%= Html.DropDownList("zip.MetroMarginalCode", Model.MetroMarginalCodes())%>
                <%= Html.ValidationMessage("MinistryName", "*")%>
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

