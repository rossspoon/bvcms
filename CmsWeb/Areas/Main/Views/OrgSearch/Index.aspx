<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CmsWeb.Models.OrgSearchModel>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <link href="/Content/jquery.tooltip.css" rel="stylesheet" type="text/css" />    
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%= SquishIt.Framework.Bundle.JavaScript()
        .Add("/Content/js/jquery.jeditable.js")
        .Add("/Content/js/jquery.tooltip.js")
        .Add("/Scripts/OrgSearch.js")
        .Render("/Content/OrgSearch_#.js")
            %>  
    <h3>Organization Search</h3>
    <form method="post">
    <table>
        <tr>
            <td>
                <table class="modalPopup">
                    <tr>
                        <th>Name:</th>
                        <td><%=Html.TextBox("Name", Model.Name, new { title="OrganizationId, Location or part of Name (organization, leader, division)"}) %></td>
                        <th>Status:</th>
                        <td><%=Html.DropDownList("StatusId", Model.StatusIds()) %></td>
                    </tr>
                    <tr>
                        <th>Program:</th>
                        <td><%=Html.DropDownList("ProgramId", Model.ProgramIds()) %></td>
                        <th>Schedule:</th>
                        <td><%=Html.DropDownList("ScheduleId", Model.ScheduleIds()) %></td>
                    </tr>
                    <tr>
                        <th>Division:</th>
                        <td colspan="3">
                        <%=Html.DropDownList("DivisionId", Model.DivisionIds()) %>
                        </td>
                    </tr>
                    <tr>
                        <th>Campus:</th>
                        <td colspan="2"><%=Html.DropDownList("CampusId", Model.CampusIds()) %></td>
                        <td align="right"><a href="/OrgSearch/Results?clear=true" id="clear">clear</a></td>
                    </tr>
                    <tr>
                        <th>Online Reg.:</th>
                        <td colspan="2"><%=Html.DropDownList("OnlineReg", Model.RegistrationTypeIds()) %></td>
                        <td align="right"><a href="/OrgSearch/Results" id="search" class="bt default formlink">Search</a></td>
                    </tr>
                </table>
            </td>
            <td>
<% if (User.IsInRole("OrgTagger"))
   { %>
                <table>
                    <tr>
                        <th colspan="2">Manage Divisions</th>
                    </tr>
                    <tr>
                        <th align="right">Program:</th>
                        <td><%=Html.DropDownList("TagProgramId", Model.ProgramIds())%></td>
                    </tr>
                    <tr>
                        <th align="right">Target Division:</th>
                        <td><%=Html.DropDownList("TagDiv", Model.DivisionIds())%></td>
                    </tr>
                </table>
<% } %>
            </td>

        </tr>
    </table>
    <hr />
    <% Html.RenderPartial("Results", Model); %>
    </form>
    <div>
        <a id="ExportExcel" href="#">Export to Excel</a> | 
        <a id="rollsheet1" href='#'>Create Roll Sheet(s)</a> | 
        <a id="Meetings" href="#">Meetings</a> | 
        <a id="Roster" href="#">Roster</a> | 
        <a class="ViewReport" href="/Reports/OrgLeaders/">Leaders</a> | 
        <a class="ViewReport" href="/Reports/ClassList/">Class List</a> | 
        <a id="attdetail1" href='#'>Meetings Attendance</a>
<% if (Session["OrgCopySettings"] != null)
   { %>
        | <a id="PasteSettings" href="/OrgSearch/PasteSettings">Paste Settings</a>

<% } %>
    </div>
    <div id="PanelRollsheet" class="modalDiv dialog" style="display: none">
        <table>
        <tr><th colspan="2">Select a meeting date and time</th></tr>
        <tr><th>Meeting Date</th><td><input id="MeetingDate" type="text" class='datepicker' /></td></tr>
        <tr><th>Meeting Time</th><td><input id="MeetingTime" title="Time in Format hh:mm am or pm" type="text" /></td></tr>
        <tr>
            <th id="altnames">Use Alt Names</th>
            <td><%=Html.CheckBox("altnames") %></td>
        </tr>
        <tr><td align="right" colspan="2">
            <a class="bt" id="rollsheet2" href="#">create</a>
        </tr>
        </table>
    </div>
    <div id="PanelAttDetail" class="modalDiv dialog" style="display: none">
        <table>
        <tr><th colspan="2">Select a date range</th></tr>
        <tr><th>Meetings Start Date</th><td><input id="MeetingDate1" type="text"  class='datepicker' /></td></tr>
        <tr><th>Meetings End Date</th><td><input id="MeetingDate2" type="text" class='datepicker' /></td></tr>
        <tr><td align="right" colspan="2">
            <a class="bt" id="attdetail2" href="#"> run </a>
        </tr>
        </table>
    </div>
</asp:Content>
