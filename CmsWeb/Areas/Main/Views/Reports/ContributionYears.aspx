<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Reports.Master" Inherits="System.Web.Mvc.ViewPage<CmsWeb.Areas.Main.Models.Report.ContributionModel>" %>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        body
        {
            font-size: large;
        }
        .totalrow td
        {
            border-top: 2px solid black;
            font-weight: bold;
            text-align: right;
        }
        .headerrow th
        {
            border-bottom: 2px solid black;
            text-align: center;
        }
        input#SundayDate
        {
            width: 100px;
            font-size: large;
        }
    </style>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript">
        $(function() {
            $(".datepicker").datepicker({
                dateFormat: 'm/d/yy',
                changeMonth: true,
                changeYear: true
            });
            $("#StatementDialog").dialog({ autoOpen: false });
            $('#Statement').live("click", function (ev) {
                ev.preventDefault();
                var a = this;
                var d = $("#StatementDialog");
                d.dialog("option", "buttons", {
                    "Ok": function () {
                    var fd = $('#FromDate').val();
                    var td = $('#ToDate').val();
                    var typ = $('input[name=indfam]:radio:checked').val();
                    var args = "<%=Model.person.PeopleId %>?FromDate=" + fd + "&ToDate=" + td + "&typ=" + typ;
                    var newWindowUrl = "/Reports/ContributionStatement/" + args;
                    window.open(newWindowUrl);
                    d.dialog('close');
                    }
                });
                d.dialog('open');
                return false;
            });
        });
    </script>
    <h1>Yearly Contributions</h1>
    <a href="/Person/Index/<%=Model.person.PeopleId %>"><%=Model.person.Name %></a>
    | <a id="Statement" href="#">statement</a><br />

    <div style="text-align: center">
        <table border="0">
            <tr>
                <th>Year</th>
                <th align="right">Count</th>
                <th align="right">Amount</th>
            </tr>
            <% foreach (var y in Model.FetchYears())
               { %>
            <tr>
                <td><%=y.Year%></td>
                <td align="right"><%=y.Count.ToString2("n0")%></td>
                <td align="right">
                    <a href='/Contributions/Individual.aspx?id=<%=y.PeopleId%>&year=<%=y.Year%>'><%=y.Amount.ToString2("n") %></a>
                </td>
           </tr>
           <% } %>

        </table>
    </div>
    <div id="StatementDialog" class="modalDiv" style="display:none">
    <table>
        <tr>
            <th colspan="2" style="font-size: larger; font-weight: bold">
                Please select start and end dates:
            </th>
        </tr>
        <tr>
            <th>Start Date:</th>
            <td><%=Html.DatePicker("FromDate") %></td>
        </tr>
        <tr>
            <th>End Date:</th>
            <td><%=Html.DatePicker("ToDate")%></td>
        </tr>
        <tr>
            <th></th>
            <td>
            <input type="radio" name="indfam" checked="checked" value="1" /> Individual
            <input type="radio" name="indfam" checked="checked" value="2" /> Family
            </td>
        </tr>
        <tr>
            <td colspan="2"></td>
        </tr>
    </table>
    </div>
</asp:Content>
