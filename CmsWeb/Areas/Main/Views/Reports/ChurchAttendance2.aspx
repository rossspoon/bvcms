<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Reports.Master" Inherits="System.Web.Mvc.ViewPage<CmsWeb.Areas.Main.Models.Report.ChurchAttendance2Model>" %>
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
        });
    </script>
    <div style="text-align: center">
        <h1>
            Church Attendance</h1>
        <form action="/Reports/ChurchAttendance2/" method = "post">
        Start Date:
        <%=Html.DatePicker("Dt1") %> End Date: <%=Html.DatePicker("Dt2") %><br />
        Skip Dates (optional, comma separated sundays): <%=Html.TextBox("skipweeks") %> <%=Html.SubmitButton("submit", "Run") %>
        </form> 
       <hr />
        <% foreach (var p in Model.FetchInfo())
           { %>
        <div>
        <table align="center" cellpadding="2">
        <thead>
           <tr>
               <th colspan="<%=p.Cols.Count+4 %>"><%=p.Name %></th>
           </tr>
           <tr>
                <th colspan="<%=p.Cols.Count+2 %>"></th>
           </tr>
           <tr class="headerrow">
               <td></td>
                   <% foreach (var c in p.Cols)
                      { %>
               <th><%="{0:h:mm tt}".Fmt(c) %></th>
                   <% } %>
               <th>Total</th>
           </tr>
        </thead>
                <% foreach (var d in p.Divs)
                   { %>
           <tr>
           <td align="left"><%=d.Name%></td>
               <% foreach (var c in p.Cols)
                  { %>
           <td align="right"><%=d.Column(c.TimeOfDay).ToString("n0")%></td>
               <% } %>
           <td align="right"><%=d.Total().ToString("n0") %></td>
           </tr>
                <% } %>
           <tr class="totalrow">
           <td align="left">Total</td>
               <% foreach (var c in p.Cols)
                  { %>
           <td><%=p.Column(c.TimeOfDay).ToString("n0")%></td>
               <% } %>
           <td><%=p.Total().ToString("n0")%></td>
           </tr>
        </table>
        </div>
        <div>&nbsp;</div>
        <% } %>
    </div>
</asp:Content>
