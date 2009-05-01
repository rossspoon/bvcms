<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.QueryModel>" %>

<asp:Content ID="indexHead" ContentPlaceHolderID="head" runat="server">
    <link href="/Content/jqueryMultiSelect.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
<div>
    <script src="/Content/js/jquery.pagination.js" type="text/javascript"></script>
    <script src="/Content/js/ui.tabs.js" type="text/javascript"></script>
    <script src="/Content/js/jqueryMultiSelect.js" type="text/javascript"></script>
    <script src="/Content/js/jquery.contextMenu.js" type="text/javascript"></script>
    <script src="/Scripts/QueryBuilder.js" type="text/javascript"></script>
    <script src="/Scripts/ExportToolbar2.js" type="text/javascript"></script>
</div>
<form id="conditionForm" action="/QueryBuilder/Main" method="get">
<h3 id="Description"><%=Model.Description %></h3>
<table class="modalPopup">
    <tr class="QBConditionTitle"><td colspan="3">Configure the query condition</td></tr>
    <tr>
        <td valign="top">
            <table>
                <tr><th colspan="2">Left Side</th></tr>
                <tr id="CategoryDiv2">
                    <td>
                      <input id="SelectCondition" type="button" value="Select Condition" />
                    </td>
                    <td>
                        <span id='ConditionText' style="font-size: large">Group</span>
                        <%=Html.Hidden("ConditionName") %>
                    </td>
                </tr>
                <tr id="DaysDiv" style="display:none">
                    <td>Days:</td>
                    <td>
                        <%=Html.TextBoxClass("Days", "validate") %>
                    </td>
                </tr>
                <tr id="WeekDiv" style="display:none">
                    <td>Week:</td>
                    <td>
                        <%=Html.TextBoxClass("Week", "validate") %>
                    </td>
                </tr>
                <tr id="QuartersDiv" style="display:none">
                    <td>Quarters:</td>
                    <td>
                        <%=Html.TextBoxClass("Quarters", "validate") %>
                    </td>
                </tr>
                <tr id="TagDiv" style="display:none">
                    <td>Tags:</td>
                    <td>
                        <%=Html.TextBoxClass("Tags", "validate")%>
                    </td>
                </tr>
                <tr id="SavedQueryDiv" style="display:none">
                    <td>Saved Query:</td>
                    <td>
                        <%=Html.TextBoxClass("SavedQueryDesc", "validate")%>
                    </td>
                </tr>
                <tr id="StartDiv" style="display:none">
                    <td>Start Date:</td>
                    <td>
                        <%=Html.TextBoxClass("StartDate", "datepicker validate") %>
                    </td>
                </tr>
                <tr id="EndDiv" style="display:none">
                    <td>End Date:</td>
                    <td>
                        <%=Html.TextBoxClass("EndDate", "datepicker validate") %>
                    </td>
                </tr>
                <tr id="ProgDiv" style="display:none">
                    <td>Program:</td>
                    <td>
                        <%=Html.DropDownList("Program", Model.Programs()) %>
                    </td>
                </tr>
                <tr id="DivDiv" style="display:none">
                    <td>Division:</td>
                    <td>
                        <select id="Division" ></select>
                    </td>
                </tr>
                <tr id="OrgDiv" style="display:none">
                    <td>Organization:</td>
                    <td>
                        <select id="Organization" ></select>
                    </td>
                </tr>
            </table>
        </td>
        <td valign="top">
            <table>
                <tr><th>Comparison</th></tr>
                <tr>
                    <td>
                        <%=Html.DropDownList("Comparison", Model.Comparisons()) %>
                    </td>
                </tr>
            </table>
        </td>
        <td id="RightPanel" valign="top" style="display:none">
            <table>
                <tr><th>Right Side</th></tr>
                <tr>
                    <td>
                        <span id="values" style="display:none"></span>
                        <%=Html.TextBoxClass("TextValue", "validate")%>
                        <%=Html.TextBoxClass("NumberValue", "validate")%>
                        <%=Html.TextBoxClass("IntegerValue", "validate")%>
                        <%=Html.TextBoxClass("DateValue", "datepicker validate")%>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td align="right" colspan="3">
            <input id="Update" type="button" class="conditionButton" value="Update" disabled="disabled" />
            <input id="Add" type="button" class="conditionButton" value="Add" disabled="disabled" />
            <input id="AddToGroup" type="button" class="conditionButton" value="Add To Group" />
            <input id="Remove" type="button" class="conditionButton" value="Remove" disabled="disabled" />
        </td>
    </tr>
</table>
<%=Html.Hidden("SelectedId") %>
<table class="modalPopup" style="margin-top: 6px">
    <tr><td colspan="2" style="background-color: #a9cfe2; border: solid 1px Gray; color: Black;
            font-size: medium">Conditions</td>
    </tr>
    <tr>
        <td>
            <ul id="ConditionGrid" style="list-style-type: none; margin-left: 0px; font-size: large;">
            <% Html.RenderPartial("Conditions", Model); %>
            </ul>
        </td>
        <td align="center" valign="bottom">
            <div>
                <input id="Run" type="image" src="/images/Run.png" alt="Run Query" title="Run Query" />
            </div>
            <div>Run</div>
        </td>
    </tr>
</table>
<ul id="InsCopyMenu" class="popupMenu">
    <li><a href="#ins">Insert Group Above</a></li>
    <li><a href="#copy">Copy As New</a></li>
</ul>
<div id="toolbar" style='<%=Model.ShowResults? "" : "display:none"%>'>
<% Html.RenderPartial("ExportToolBar"); %>
</div>
<div id="Results" style="clear:both; margin-top: 6px">
    <% if (Model.ShowResults)
       Html.RenderPartial("Results", Model); %>
</div>
</form>
<div id="QueryConditionSelect" class="modalPopup" title="Select a Condition" style="margin-top: 6px; width: 700px; height: 500px;">
    <div>
        <a id="NewGroup" href="#">Or start a new group of conditions</a><br />
    </div>
    <div id="tabber" style="overflow: auto; height: 450px; margin:4px; border-top:solid 2px gray">
        <ul>
<% foreach(var c in Model.FieldCategories())
   { %>
            <li><a href='<%= "#" + c.Name %>'><span><%= c.Title %></span></a></li>
<% } %>
        </ul>
<% foreach(var c in Model.FieldCategories())
   { %>
        <div id='<%=c.Name %>'>
    <% foreach(var p in c.Fields) 
       { %>
            <div>
                <a id='<%=p.Name %>' class="FieldLink" href="#"><%=p.Title %></a>
                <span style="cursor: pointer" onclick="toggle(this)"><img src="/images/help_out.gif" /></span>
            </div>
            <div class="moreinfo">
                <%=p.Description %>
            </div>
    <% } %>
        </div>
<% } %>
    </div>
</div>
<div id="OpenQueryDiv" title="Open Query" class="modalDiv">
    <select id="ExistingQueries"></select>
        <div style="text-align:right; vertical-align:bottom">
            <input id="OpenQuery" type="button" value="Open" />
        </div>
    </div>
</div>
<div id="SaveQueryDiv" class="modalDiv">
    <div style="text-align: left">
        Query Description:
        <%=Html.TextBox("SavedQueryDesc") %>
        &nbsp;
        <%=Html.CheckBox("IsPublic") %> Is Public
        <span class="footer">
            <a id="SaveQuery" href="#">Save</a>
        </span>
        <br />
    </div>
</div>
</asp:Content>
