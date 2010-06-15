<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.PostBundleModel>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <link href="/Content/jquery.autocomplete.css" rel="stylesheet" type="text/css" />
    <link href="/Content/jquery.tooltip.css" rel="stylesheet" type="text/css" />
<style type="text/css"> .alt { background-color: #eee; } </style>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2>PostBundle</h2>
    <%= SquishIt.Framework.Bundle.JavaScript()
                .Add("/Content/js/jquery.jeditable.js")
                .Add("/Content/js/jquery.autocomplete.js")
                .Add("/Content/js/jquery.tooltip.js")
                .Add("/Scripts/PostBundle.js")
                .Render("/Content/PostBundle_#.js")
            %>        
<%=Html.Hidden("fundid", Model.bundle.FundId) %>
<% var b = Model.bundle; %>
<table>
    <tr><th>BundleId</th><td><a href="/Contributions/Bundle.aspx?id=<%=Model.id %>"><%=Model.id %><%=Html.Hidden("id") %></a></td></tr>
    <tr><th>Date</th><td><%=Model.bundle.ContributionDate.ToShortDateString() %></td></tr>
    <tr><th>Fund</th><td><a target="_blank" href='/PostBundle/FundTotals/<%=Model.id %>'><%="{0} ({1})".Fmt(Model.bundle.Fund.FundDescription, Model.bundle.FundId) %></a></td></tr>
    <tr><th>Pledge</th><td><%=Html.CheckBox("pledge") %></td></tr>
    <tr><th>Total</th><td><%=(b.TotalCash + b.TotalChecks + b.TotalEnvelopes).ToString2("c") %></td></tr>
    <tr><th>Total Items</th><td><span id="totalitems"><%=b.BundleDetails.Sum(bd => bd.Contribution.ContributionAmount).ToString2("c") %></span></td></tr>
    <tr><th>Item Count</th><td><span id="itemcount"><%=b.BundleDetails.Count() %></span></td></tr>
</table>
<form id='pbform' action='/PostBundle/InsertRow' method="post">
<%=Html.Hidden("id") %>
<%=Html.Hidden("editid") %>
<table id='bundle'>
    <thead>
        <tr>
            <th>PeopleId</th>
            <th>Name</th>
            <th>Amount</th>
            <th>Fund(s)</th>
            <th>Notes</th>
            <th></th>
            <th></th>
        </tr>
        <tr id="entry">
            <td><input id="pid" name="pid" type="text" style="width:5em" /></td>
            <td><input id="name" name="name" type="text" /></td>
            <td><input id="amt" name="amt" type="text" style="width:5em" /></td>
            <td><%=Html.DropDownList("fund", Model.Funds()) %></td>
            <td><input id="notes" name="notes" type="text" /></td>
            <td><a class="update" href='#'>update</a></td>
            <td></td>
        </tr>
    </thead>
    <tbody>
    <% foreach(var c in Model.FetchContributions())
       { %>
       <tr cid="<%=c.ContributionId %>">
            <td><a href="/SearchAdd/Index/<%=c.ContributionId %>?type=contributor" class="pid"><%=c.PeopleId == null? "select" : c.PeopleId.ToString() %></a></td>
            <td class="name" title="<%=c.tip %>"><%=c.Name %></td>
            <td class="amt" val="<%=c.Amt %>" align="right">
            <span id='a<%=c.ContributionId %>' class='clickEdit'><%=c.AmtDisplay %></span>            
            </td>
            <td class="fund" val="<%=c.FundId %>">
                <span id='f<%=c.ContributionId %>' 
                    class='clickSelect'><%=c.FundDisplay%></span>
            </td>
            <td class="notes"><%=c.Notes %></td>
            <td><a class="edit" href='#'>edit</a></td>
            <td><a class="delete" href='#'>delete</a></td>
       </tr>
    <% } %>
    </tbody>
</table>
</form>
    <div id="searchDialog">
    <iframe style="width:99%;height:99%"></iframe>
    </div>

</asp:Content>
