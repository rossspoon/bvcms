<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.PostBundleModel>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <link href="/Content/jquery.autocomplete.css" rel="stylesheet" type="text/css" />
<style> .alt { background-color: #eee; } </style>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2>PostBundle</h2>
    <%= SquishIt.Framework.Bundle.JavaScript()
                .Add("/Content/js/jquery.jeditable.js")
                .Add("/Content/js/jquery.autocomplete.js")
                .Add("/Scripts/PostBundle.js")
                .Render("/Content/PostBundle_#.js")
            %>        
<form id='pbform' action='/PostBundle/InsertRow' method="post">
<table>
<tr><th>BundleId</th><td><%=Model.id %><%=Html.Hidden("id") %></td></tr>
<tr><th>Total</th><td></td></tr>
</table>

<table id='bundle'>
    <thead>
        <tr>
            <th>PeopleId</th>
            <th>Name</th>
            <th>Amount</th>
            <th>Fund(s)</th>
            <th>Notes</th>
        </tr>
        <tr id="entry">
            <td><%=Html.TextBox("pid") %></td>
            <td><%=Html.TextBox("name") %></td>
            <td><%=Html.TextBox("amt") %></td>
            <td><%=Html.TextBox("fund") %></td>
            <td><%=Html.TextBox("notes") %></td>
        </tr>
    </thead>
    <tbody>
    <% foreach(var c in Model.FetchContributions())
       { %>
       <tr>
            <td><%=c.PeopleId %></td>
            <td><%=c.Name %></td>
            <td><%=c.AmtDisplay %></td>
            <td><%=c.FundDisplay %></td>
            <td><%=c.Notes %></td>
       </tr>
    <% } %>
    </tbody>
</table>
</form>

</asp:Content>
