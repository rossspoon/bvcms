<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Reports.Master" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Areas.Main.Models.Report.ChurchAttendanceModel>" %>
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
        }
        .headerrow th
        {
            border-bottom: 2px solid black;
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
            $(".datepicker").change(function() {
                window.location = "/Reports/ChurchAttendance/" + this.value.replace(/\//ig, "-");
            });
        });
    </script>
    <div style="text-align: center">
        <h1>
            Church Attendance</h1>
        Sunday Date:
        <%=Html.DatePicker("Sunday") %>
        <hr />
        <% foreach (var p in Model.FetchInfo())
           { %>
        <div>
        <table align="center" cellpadding="2">
           <tr>
           <th colspan="3"><%=p.Name %></th>
           </tr>
           <tr class="headerrow">
           <td></td>
           <td align="right">Present</td>
           <td align="right">Visitors</td>
           </tr>
        <% foreach (var d in p.Divs)
           { %>
           <tr>
           <td align="left"><%=d.Name %></td>
           <td align="right"><%=d.Meetings.Sum(m => m.Present).ToString("n0") %></td>
           <td align="right"><%=d.Meetings.Sum(m => m.Visitors).ToString("n0") %></td>
           </tr>
        <% } %>
           <tr class="totalrow">
           <td align="left">Total</td>
           <td align="right"><%=p.Divs.Sum(d => d.Meetings.Sum(m => m.Present)).ToString("n0")%></td>
           <td align="right"><%=p.Divs.Sum(d => d.Meetings.Sum(m => m.Visitors)).ToString("n0")%></td>
           </tr>
        </table>
        </div>
        <div>&nbsp;</div>
        <% } %>
    </div>
</asp:Content>
