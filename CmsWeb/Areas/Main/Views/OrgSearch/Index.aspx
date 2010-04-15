<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.OrgSearchModel>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
<%--    <link href="/Content/jquery.cluetip.css" rel="stylesheet" type="text/css" />
--%>    <link href="/Content/jquery.tooltip.css" rel="stylesheet" type="text/css" />    
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="/Content/js/jquery.jeditable.mini.js" type="text/javascript"></script>    
<%--    <script src="/Content/js/jquery.cluetip.min.js" type="text/javascript"></script>    
--%>    <script src="/Content/js/jquery.tooltip.min.js" type="text/javascript"></script>
    <script src="/Scripts/OrgSearch.js" type="text/javascript"></script>
    <form>
    <table>
        <tr>
            <td>
                <table class="modalPopup">
                <tr><td><a href="/OrgSearch/Results?clear=true" id="clear">clear</a></td>
                    <td align="right" colspan="3"><a href="/OrgSearch/UseOldOrgSearch">Use Old Org Search</a></td>
                    </tr>
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
                            <select id="DivisionId" name="DivisionId">
                                <option value="0">(select a program)</option>
                            </select>
                        </td>
                    </tr>
                    <tr>
                        <th>Campus:</th>
                        <td colspan="2"><%=Html.DropDownList("CampusId", Model.CampusIds()) %></td>
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
                        <th align="right">Tag Division:</th>
                        <td><%=Html.DropDownList("TagProgramId", Model.ProgramIds())%><br />
                            <select id="TagDiv">
                                <option value="0">(select a program)</option>
                            </select><br />
                            <input type="checkbox" id="maindiv" /> make this the main Division when tagging<br />
                        </td>
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
        <a id="Rollsheet" href='#'>Create Roll Sheet(s)</a> | 
        <a id="Meetings" href="#">Meetings</a> | 
        <a id="Roster" href="#">Roster</a> | 
        <a class="ViewReport" href="/Reports/OrgLeaders/">Leaders</a> | 
        <a class="ViewReport" href="/Reports/ClassList/">Class List</a> | 
        <a id="AttDetail" href='#'>Meetings Attendance</a>
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
        <tr><td align="right" colspan="2">
            <a class="bt" id="rollsheet" href="#">create</a>
        </tr>
        </table>
    </div>
    <div id="PanelAttDetail" class="modalDiv dialog" style="display: none">
        <table>
        <tr><th colspan="2">Select a date range</th></tr>
        <tr><th>Meetings Start Date</th><td><input id="MeetingDate1" type="text"  class='datepicker' /></td></tr>
        <tr><th>Meetings End Date</th><td><input id="MeetingDate2" type="text" class='datepicker' /></td></tr>
        <tr><td align="right" colspan="2">
            <a class="bt" id="attdetail" href="#"> run </a>
        </tr>
        </table>
    </div>
</asp:Content>
