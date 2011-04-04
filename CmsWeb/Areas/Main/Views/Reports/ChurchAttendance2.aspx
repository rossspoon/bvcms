<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Reports.Master" Inherits="System.Web.Mvc.ViewPage<CmsWeb.Areas.Main.Models.Report.ChurchAttendance2Model>" %>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        body
        {
            font-size: 100%;
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
            font-size: 100%;
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
        Skip Dates (optional, comma separated sundays): <%=Html.TextBox("skipweeks") %>
        <input type="submit" name="Submit" value="Run" />
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
                  {
                      var a = d.Column(c.TimeOfDay); %>
           <td align="right" title='<%="{0}/{1}".Fmt(a.totalpeople,a.totalmeetings) %>'><%=a.avg.ToString("n0")%></td>
               <% }
                  var ta = d.Total(); %>
           <td align="right" title='<%="{0}/{1}".Fmt(ta.totalpeople,ta.totalmeetings) %>'><%=ta.avg.ToString("n0") %></td>
           </tr>
                <% } %>
           <tr class="totalrow">
           <td align="left">Total</td>
               <% foreach (var c in p.Cols)
                  {
                      var a = p.Column(c.TimeOfDay);%>
           <td title='<%="{0}/{1}".Fmt(a.totalpeople,a.totalmeetings) %>'><%=a.avg.ToString("n0")%></td>
               <% }
                  var tta = p.Total(); %>
           <td title='<%="{0}/{1}".Fmt(tta.totalpeople,tta.totalmeetings) %>'><%=tta.avg.ToString("n0")%></td>
           </tr>
        </table>
        </div>
        <div>&nbsp;</div>
        <% } %>
    </div>
</asp:Content>
