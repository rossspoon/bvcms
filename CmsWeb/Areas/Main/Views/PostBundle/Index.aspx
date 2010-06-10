<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.PostBundleModel>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <link href="/Content/jquery.autocomplete.css" rel="stylesheet" type="text/css" />
<style type="text/css"> .alt { background-color: #eee; } </style>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2>PostBundle</h2>
    <%= SquishIt.Framework.Bundle.JavaScript()
                .Add("/Content/js/jquery.jeditable.js")
                .Add("/Content/js/jquery.autocomplete.js")
                .Add("/Scripts/PostBundle.js")
                .Render("/Content/PostBundle_#.js")
            %>        
<%=Html.Hidden("fundid", Model.bundle.FundId) %>
<% var b = Model.bundle; %>
<form id='pbform' action='/PostBundle/InsertRow' method="post">
<table>
<tr><th>BundleId</th><td><%=Model.id %><%=Html.Hidden("id") %></td></tr>
<tr><th>Date</th><td><%=Model.bundle.ContributionDate.ToShortDateString() %></td></tr>
<tr><th>Fund</th><td><%="{0} ({1})".Fmt(Model.bundle.Fund.FundDescription, Model.bundle.FundId) %></td></tr>
<tr><th>Pledge</th><td><%=Html.CheckBox("pledge") %></td></tr>
<tr><th>Total</th><td><%=(b.TotalCash + b.TotalChecks + b.TotalEnvelopes).ToString2("c") %></td></tr>
<tr><th>Total Items</th><td><span id="totalitems"><%=b.BundleDetails.Sum(bd => bd.Contribution.ContributionAmount).ToString2("c") %></span></td></tr>
<tr><th>Item Count</th><td><span id="itemcount"><%=b.BundleDetails.Count() %></span></td></tr>
</table>
<%=Html.Hidden("editid") %>
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
            <td><input id="pid" name="pid" type="text" style="width:5em" /></td>
            <td><input id="name" name="name" type="text" /></td>
            <td><input id="amt" name="amt" type="text" style="width:5em" /></td>
            <td><%=Html.DropDownList("fund", Model.Funds()) %></td>
            <td><input id="notes" name="notes" type="text" /></td>
            <td><a class="update" href='#'>update</a></td>
        </tr>
    </thead>
    <tbody>
    <% foreach(var c in Model.FetchContributions())
       { %>
       <tr>
            <td class="pid"><%=c.PeopleId %></td>
            <td class="name"><%=c.Name %></td>
            <td class="amt" val="<%=c.Amt %>" align="right"><%=c.AmtDisplay %></td>
            <td class="fund" val="<%=c.FundId %>"><%=c.FundDisplay %></td>
            <td class="notes"><%=c.Notes %></td>
            <td><a class="edit" cid="<%=c.ContributionId %>" href='#'>edit</a></td>
       </tr>
    <% } %>
    </tbody>
</table>
</form>

</asp:Content>
