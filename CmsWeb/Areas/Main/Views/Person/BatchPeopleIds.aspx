<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Batch Load PeopleIds</h2>
    
<form action="/Person/BatchUploadPeopleIds" method="post" enctype="multipart/form-data">
    <label>Filename: <input type="file" name="file" /></label>
    <label>Tag name: </label> <%=Html.TextBox("name") %>
    <input type="submit" value="Submit" />
    <%=Html.TextArea("text", "", new { rows = "10", cols = "100", wrap = "off", style = "width:90%" }) %><br />
</form>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
