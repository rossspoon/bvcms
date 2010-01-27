<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.PersonModel>" %>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <link href="/Content/jquery.autocomplete.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="/Content/js/jquery.autocomplete.min.js" type="text/javascript"></script>
    <script src="/Scripts/SearchPeople.js" type="text/javascript"></script>
    <script src="/Scripts/Person.js" type="text/javascript"></script>
    <script src="/Scripts/Pager.js" type="text/javascript"></script>

    <% CMSWeb.Models.PersonModel.PersonInfo person = Model.displayperson; %>
    <table class="PersonHead" border="0">
        <tr>
            <td><%=Model.Name %></td>
            <td align="right"></td>
            <td>&nbsp;</td>
        </tr>
    </table>
    <% Html.RenderPartial("ExportToolBar"); %>
    <div style="clear: both; margin-top: 8px;">
        <table>
            <tr>
                <td>
                    <div id="businesscard" href="/Person/BusinessCard/<%=person.PeopleId %>">
                    <% Html.RenderPartial("BusinessCard", person); %>
                    </div>
                    <table>
                        <tr>
                            <td><%=person.DoNotCall %></td>
                            <td>
                                <%=Html.HyperlinkIf(User.IsInRole("Finance"), "/Contributions/Years.aspx?id=" + person.PeopleId, "Contributions", null, null)%>
                                <%=Html.HyperlinkIf(Model.HasRecReg, "/Recreation/Detail/" + Model.recregid, "RecForm", null, null)  %>
                                <%=Html.HyperlinkIf(Model.CanCheckIn, "/CheckIn/CheckIn/{0}?pid={1}".Fmt(Model.ckorg, person.PeopleId), "CheckIn", null, null)%>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <% if (Page.User.IsInRole("Admin"))
                                   { %>
                                <a id="deleteperson" href="/Person/Delete/<%=person.PeopleId %>"><img border="0" src="/images/delete.gif" /></a>
                                <a id="moveperson" href="/Person/Move/<%=person.PeopleId %>">move</a>
                                <% } %>
                            </td>
                        </tr>
                    </table>
                </td>
                <td valign="top">
                    <a id="Picture" href="/UploadPicture.aspx?id=<%=person.PeopleId %>" title="Click to see larger version or upload new">
                    <img alt="portrait" border="0" src="/Image.aspx?portrait=1&id=<%=person.SmallPicId %>" />
                    </a>
                </td>
                <td></td>
                <td valign="top">
                    <br />
                    <table>
                        <tr>
                            <th align="left" colspan="4">
                                <a href="/Family.aspx?id=<%=person.FamilyId %>"><strong>Family Members</strong></a>
                            </th>
                        </tr>
                    <% foreach (var m in Model.FamilyMembers())
                       { %>
                        <tr>
                            <td><a href="/Person/Index/<%=m.Id %>"><span style='<%=m.Color%>'><%=m.Name %></span></a></td>
                            <td><%=m.SpouseIndicator %></td>
                            <td><%=m.Age %></td>
                            <td><%=m.PositionInFamily %></td>
                        </tr>
                    <% } %>
                    </table>
                </td>
           </tr>
        </table>
    </div>
    <div id="main-tab" class="ui-tabs">
        <ul class="ui-tabs-nav">
            <li><a href="#basic-tab"><span>Basic</span></a></li>
            <li><a href="#address-tab"><span>Addresses</span></a></li>
            <li><a id="enrollment-link" href="#enrollment-tab"><span>Enrollment</span></a></li>
            <li><a id="member-link" href="#member-tab"><span>Member Profile</span></a></li>
            <li><a id="growth-link" href="#growth-tab"><span>Growth</span></a></li>
            <li><a href="#volunteer-tab"><span>Volunteer</span></a></li>
        </ul>
        <div id="basic-tab" class="ui-tabs-panel ui-tabs-hide">
            <form class="DisplayEdit" action="">
            <% Html.RenderPartial("BasicDisplay", person); %>
            </form>
        </div>
        <div id="address-tab" class="ui-tabs-hide ui-tabs-panel">
            <ul class="ui-tabs-nav">
                <li><a href="#PersonalAddr"><span>Personal</span></a></li>
                <li><a href="#AltPersonalAddr"><span>Personal Alternate</span></a></li>
                <li><a href="#FamilyAddr"><span>Family</span></a></li>
                <li><a href="#AltPersonalAddr"><span>Family Alternate</span></a></li>
            </ul>
            <div id="PersonalAddr" class="ui-tabs-hide ui-tabs-panel">
                <form class="DisplayEdit" action="">
                <% Html.RenderPartial("AddressDisplay", person.PersonalAddr); %>
                </form>
            </div>
            <div id="AltPersonalAddr" class="ui-tabs-hide ui-tabs-panel">
                <form class="DisplayEdit" action="">
                <% Html.RenderPartial("AddressDisplay", person.AltPersonalAddr); %>
                </form>
            </div>
            <div id="FamilyAddr" class="ui-tabs-hide ui-tabs-panel">
                <form class="DisplayEdit" action="">
                <% Html.RenderPartial("AddressDisplay", person.FamilyAddr); %>
                </form>
            </div>
            <div id="AltFamilyAddr" class="ui-tabs-hide ui-tabs-panel">
                <form class="DisplayEdit" action="">
                <% Html.RenderPartial("AddressDisplay", person.AltFamilyAddr); %>
                </form>
            </div>
            <%=Html.Hidden("addrtab") %>
        </div>
        <div id="enrollment-tab" class="ui-tabs-hide ui-tabs-panel">
            <ul class="ui-tabs-nav">
                <li><a href="#current-tab"><span>Current</span></a></li>
                <li><a id="previous-link" href="#previous-tab"><span>Previous</span></a></li>
                <li><a id="pending-link" href="#pending-tab"><span>Pending</span></a></li>
                <li><a id="attendance-link" href="#attendance-tab"><span>Attendance History</span></a></li>
            </ul>
            <div id="current-tab" class="ui-tabs-hide ui-tabs-panel">
                <form action="/Person/EnrollGrid/<%=person.PeopleId %>">
                </form>
            </div>
            <div id="previous-tab" class="ui-tabs-hide ui-tabs-panel">
                <form action="/Person/PrevEnrollGrid/<%=person.PeopleId %>">
                </form>
            </div>
            <div id="pending-tab" class="ui-tabs-hide ui-tabs-panel">
                <form action="/Person/PendingEnrollGrid/<%=person.PeopleId %>">
                </form>
            </div>
            <div id="attendance-tab" class="ui-tabs-hide ui-tabs-panel">
                <form action="/Person/AttendanceGrid/<%=person.PeopleId %>">
                </form>
            </div>
        </div>
        <div id="member-tab" class="ui-tabs-hide ui-tabs-panel">
            <ul class="ui-tabs-nav">
                <li><a href="#membersum-tab"><span>Summary</span></a></li>
                <li><a href="#membernotes-tab"><span>Notes</span></a></li>
            </ul>
            <div id="membersum-tab" class="ui-tabs-hide ui-tabs-panel">
                <% Html.RenderPartial("Membership", person); %>
            </div>
            <div id="membernotes-tab" class="ui-tabs-hide ui-tabs-panel">
                <table>
                    <tr>
                        <td valign="top">
                            <table class="Design2">
                                <tr>
                                    <th colspan="6" class="LightBlueBG">Letter</th>
                                </tr>
                                <tr>
                                    <th>Status:</th>
                                    <td><%=person.LetterStatus %></td>
                                    <th>Date Requested:</th>
                                    <td><%=person.LetterRequested %></td>
                                    <th>Date Received:</th>
                                    <td><%=person.LetterReceived %></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <strong>Notes:</strong><br />
                            <%=person.LetterNotes %>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div id="growth-tab" class="ui-tabs-hide ui-tabs-panel">
                <% Html.RenderPartial("Growth", person); %>
        </div>
        <div id="volunteer-tab" class="ui-tabs-hide ui-tabs-panel">
            <table class="Design2" style="border-style: groove; border-width: thin;">
                <tr>
                    <th>Approvals:</th>
                    <td><input type="checkbox" <%=Model.vol.Standard ? "checked='checked'" : "" %> disabled="disabled" />
                        Standard (references only)<br />
                        <input type="checkbox" <%=Model.vol.Leader ? "checked='checked'" : "" %> disabled="disabled" />
                        Leadership (background check)
                    </td>
                    <td><a href="/AppReview/VolunteerApp.aspx?id=<%=person.PeopleId %>"
                        <%=User.IsInRole("ApplicationReview") ? "disabled='disabled'" : "" %>>Volunteer Application Review</a></td>
                </tr>
                <tr>
                    <th>Approval Date:</th>
                    <td colspan="2"><%=Model.vol.ProcessedDate.FormatDate() %></td>
                </tr>
            </table>
        </div>
    </div>
    <div id="dialogbox" title="Search People" style="width: 560px; overflow: scroll">
    </div>
</asp:Content>
