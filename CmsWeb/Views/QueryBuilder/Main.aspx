<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.QueryModel>" %>

<asp:Content ID="indexHead" ContentPlaceHolderID="head" runat="server">
    <link href="/Content/jquery.multiSelect.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
<div>
    <script src="/Content/js/jquery.pagination.js" type="text/javascript"></script>
    <script src="/Content/js/jquery.multiSelect.js" type="text/javascript"></script>
    <script src="/Content/js/jquery.contextMenu.js" type="text/javascript"></script>
    <script src="/Scripts/QueryBuilder.js" type="text/javascript"></script>
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
                    <td align="right">Days:</td>
                    <td>
                        <%=Html.TextBoxClass("Days", "validate") %>
                    </td>
                </tr>
                <tr id="AgeDiv" style="display:none">
                    <td align="right">Age Less or Equal:</td>
                    <td>
                        <%=Html.TextBoxClass("Age", "validate") %>
                    </td>
                </tr>
                <tr id="QuartersDiv" style="display:none">
                    <td align="right">Quarters:</td>
                    <td>
                        <%=Html.TextBoxClass("Quarters", "validate") %>
                    </td>
                </tr>
                <tr id="TagsDiv" style="display:none">
                    <td align="right">Tags:</td>
                    <td>
                        <span id="tagvalues" style="display:none"></span>
                    </td>
                </tr>
                <tr id="SavedQueryDiv" style="display:none">
                    <td align="right">Saved Query:</td>
                    <td>
                        <%=Html.DropDownList("SavedQueryDesc", Model.SavedQueries())%>
                    </td>
                </tr>
                <tr id="StartDiv" style="display:none">
                    <td align="right">Start Date:</td>
                    <td>
                        <%=Html.TextBoxClass("StartDate", "datepicker validate") %>
                    </td>
                </tr>
                <tr id="EndDiv" style="display:none">
                    <td align="right">End Date:</td>
                    <td>
                        <%=Html.TextBoxClass("EndDate", "datepicker validate") %>
                    </td>
                </tr>
                <tr id="ProgDiv" style="display:none">
                    <td align="right">Program:</td>
                    <td>
                        <%=Html.DropDownList("Program", Model.Programs()) %>
                    </td>
                </tr>
                <tr id="DivDiv" style="display:none">
                    <td align="right">Division:</td>
                    <td>
                        <select id="Division" ></select>
                    </td>
                </tr>
                <tr id="OrgDiv" style="display:none">
                    <td align="right">Organization:</td>
                    <td>
                        <select id="Organization" ></select>
                        <span id="OrganizationError" class="validate"></span>
                    </td>
                </tr>
                <tr id="SchedDiv" style="display:none">
                    <td align="right">Schedule:</td>
                    <td>
                        <%=Html.DropDownList("Schedule", Model.Schedules()) %>
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
            <ul id="ConditionGrid">
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
<div id="QueryConditionSelect" class="modalPopup" title="Select a Condition" style="font-size: smaller;">
    <div>
        <a id="NewGroup" href="#">Or start a new group of conditions</a><br />
    </div>
    <div id="tabber" class="ui-tabs">
    <ul class="ui-tabs-nav">
<% foreach(var c in Model.FieldCategories())
   { %>
        <li><a href='<%= "#" + c.Name %>'><span><%= c.Title %></span></a></li>
<% } %>
    </ul>
<% foreach(var c in Model.FieldCategories())
   { %>
    <div id="<%=c.Name %>"  style="overflow: auto; height: 450px; margin:4px;" class="ui-tabs-panel ui-tabs-hide">
        <% foreach(var p in c.Fields) 
           { %>
                <div>
                    <a id='<%=p.Name %>' class="FieldLink" href="#"><%=p.Title %></a>
                    <span style="cursor: pointer" onclick="toggle(this);return false;"><img src="/images/help_out.gif" /></span>
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
<div id="SaveQueryDiv" class="modalDiv">
    <div style="text-align: left">
        Query Description:
        <%=Html.TextBox("SaveQueryDesc") %>
        &nbsp;
        <input id="IsPublic" type="checkbox"/> Is Public
        <span class="footer">
            <a id="SaveQuery" href="#">Save</a>
        </span>
        <br />
    </div>
</div>
</asp:Content>
