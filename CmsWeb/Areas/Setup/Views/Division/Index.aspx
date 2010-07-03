<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.DivisionModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%= SquishIt.Framework.Bundle.JavaScript()
                .Add("/Content/js/jquery.jeditable.js")
                .Add("/Content/js/jquery.tooltip.js")
                .Add("/scripts/Divisions.js")
        .Render("/Content/Divisions_#.js")
            %>  
<form id="progform">
Filter for Program: <%=Html.DropDownList("ProgramId", Model.ProgramIds())%>
<input id="refresh" type="submit" value="Refresh" />
Target Program: <%=Html.DropDownList("TagProgramId", Model.ProgramIds())%>
</form>
    <h2>Divisions</h2>
<% Html.RenderPartial("Results", Model); %>

    <% using (Html.BeginForm("Create", "Division"))
       { %>
    <p><input id="create" type="submit" value="Create" /></p>
    <% } %>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <link href="/Content/jquery.tooltip.css" rel="stylesheet" type="text/css" />
</asp:Content>

