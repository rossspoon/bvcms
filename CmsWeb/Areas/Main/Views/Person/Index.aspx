<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.PersonModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <script src="/Content/js/jquery.pagination.js" type="text/javascript"></script>
    <script src="/Content/js/jquery.form.js" type="text/javascript"></script>
    <script src="/Content/js/jquery.form2.js" type="text/javascript"></script>
    <script src="/Scripts/SearchPeople.js" type="text/javascript"></script>
    <script src="/Scripts/Person.js" type="text/javascript"></script>

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
                        <table>
                            <%if (Model.person.Deceased)
                              { %>
                            <tr>
                                <td style="color: Red" colspan="2">Deceased:
                                    <%=Model.person.DeceasedDate %>
                                </td>
                            </tr>
                            <% } %>
                            <tr>
                                <td><a href="http://www.google.com/maps?q=<%=Model.person.PrimaryAddr.AddrCityStateZip() %>" target="_blank">
                                        <%=Model.person.PrimaryAddr.Address1 %></a>
                                </td>
                                <td><a id="clipaddr" href="#" title="copy name and address to clipboard">clipboard</a>
                                </td>
                            </tr>
                            <% if (Model.person.PrimaryAddr.Address2.HasValue())
                               { %>
                            <tr>
                                <td colspan="2">
                                    <a href="http://www.google.com/maps?q=<%=Model.person.PrimaryAddr.Addr2CityStateZip() %>"
                                        target="_blank"><%=Model.person.PrimaryAddr.Address2 %></a>
                                </td>
                            </tr>
                            <% } %>
                            <tr>
                                <td><%=Model.person.PrimaryAddr.CityStateZip() %></td>
                                <td><a href='http://www.google.com/maps?f=d&saddr=2000+Appling+Rd,+Cordova,+Tennessee+38016&pw=2&daddr=<%=Model.person.PrimaryAddr.AddrCityStateZip() %>'
                                        target="_blank">driving directions</a>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <a href="mailto:<%=Model.person.EmailAddress %>"><%=Model.person.EmailAddress %></a>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2"><%=Model.person.HomePhone.FmtFone() %></td>
                            </tr>
                            <tr>
                                <td><%=Model.person.DoNotCall %></td>
                                <td>
                                    <%=Html.HyperlinkIf(Model.IsFinance, Model.ContributionsLink, "Contributions", null, null) %>
                                    <%=Html.HyperlinkIf(Model.HasRecReg, Model.RecRegLink, "RecForm", null, null)  %>
                                    <%=Html.HyperlinkIf(Model.CanCheckIn, Model.CheckInLink, "CheckIn", null, null)  %>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                <% using (Html.BeginForm("Edit", "Person"))
                                   { %>
                                    <% if (Model.IsAdmin)
                                       { %>
                                    <input type="submit" value="Edit" />
                                    <% } %>
                                    <% if (Model.IsAdmin)
                                       { %>
                                    <a id="deleteperson" pid="<%=Model.person.PeopleId %>" href="#"><img border="0" src="/images/delete.gif" /></a>
                                    <a id="moveperson" pid="<%=Model.person.PeopleId %>" href="#">move</a>
                                    <% } %>
                                <% } %>
                                </td>
                            </tr>
                        </table>
                    </td>
                <td valign="top">
                    <a id="Picture" href="/UploadPicture.aspx?id=<%=Model.person.PeopleId %>" title="Click to see larger version or upload new">
                    <img alt="portrait" border="0" src="/Image.aspx?portrait=1&id=<%=Model.person.SmallPicId %>" />
                    </a>
                </td>
                <td class="style1"></td>
                <td valign="top">
                    <br />
                    <table>
                        <tr>
                            <th align="left" colspan="4">
                                <a href="/Family.aspx?id=<%=Model.person.FamilyId %>"><strong>Family Members</strong></a>
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
            <li><a href="#growth-tab"><span>Growth</span></a></li>
            <li><a href="#volunteer-tab"><span>Volunteer</span></a></li>
        </ul>
        <div id="basic-tab" class="ui-tabs-panel ui-tabs-hide">
            <% Html.RenderPartial("Basic", Model.person); %>
        </div>
        <div id="address-tab" class="ui-tabs-hide ui-tabs-panel">
            <ul class="ui-tabs-nav">
                <li><a href="#PersonalAddr"><span>Personal</span></a></li>
                <li><a href="#AltPersonalAddr"><span>Personal Alternate</span></a></li>
                <li><a href="#FamilyAddr"><span>Family</span></a></li>
                <li><a href="#AltPersonalAddr"><span>Family Alternate</span></a></li>
            </ul>
            <div id="PersonalAddr" class="ui-tabs-hide ui-tabs-panel">
                <% Html.RenderPartial("Address", Model.person.PersonalAddr); %>
            </div>
            <div id="AltPersonalAddr" class="ui-tabs-hide ui-tabs-panel">
                <% Html.RenderPartial("Address", Model.person.AltPersonalAddr); %>
            </div>
            <div id="FamilyAddr" class="ui-tabs-hide ui-tabs-panel">
                <% Html.RenderPartial("Address", Model.person.FamilyAddr); %>
            </div>
            <div id="AltFamilyAddr" class="ui-tabs-hide ui-tabs-panel">
                <% Html.RenderPartial("Address", Model.person.AltFamilyAddr); %>
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
                <% Html.RenderPartial("EnrollGrid", Model.enrollments); %>
            </div>
            <div id="previous-tab" class="ui-tabs-hide ui-tabs-panel">
                <% Html.RenderPartial("PrevEnrollGrid", Model.prevEnrollments); %>
            </div>
            <div id="pending-tab" class="ui-tabs-hide ui-tabs-panel">
                <% Html.RenderPartial("PendingEnrollGrid", Model.pendingEnrollments); %>
            </div>
            <div id="attendance-tab" class="ui-tabs-hide ui-tabs-panel">
                <% Html.RenderPartial("AttendanceGrid", Model.attendances); %>
            </div>
        </div>
        <div id="member-tab" class="ui-tabs-hide ui-tabs-panel">
            <ul class="ui-tabs-nav">
                <li><a href="#membersum-tab"><span>Summary</span></a></li>
                <li><a href="#membernotes-tab"><span>Notes</span></a></li>
            </ul>
            <div id="membersum-tab" class="ui-tabs-hide ui-tabs-panel">
                <% Html.RenderPartial("Membership", Model.person); %>
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
                                    <td><%=Model.person.LetterStatus %></td>
                                    <th>Date Requested:</th>
                                    <td><%=Model.person.LetterRequested %></td>
                                    <th>Date Received:</th>
                                    <td><%=Model.person.LetterReceived %></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <strong>Notes:</strong><br />
                            <%=Model.person.LetterNotes %>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div id="growth-tab" class="ui-tabs-hide ui-tabs-panel">
                <% Html.RenderPartial("Growth", Model.person); %>
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
                    <td><a href="/AppReview/VolunteerApp.aspx?id=<%=Model.person.PeopleId %>"
                        <%=User.IsInRole("ApplicationReview") ? "disabled='disabled'" : "" %>>Volunteer Application Review</a></td>
                </tr>
                <tr>
                    <th>Approval Date:</th>
                    <td colspan="2"><%=Model.vol.ProcessedDate.FormatDate() %></td>
                </tr>
            </table>
        </div>
    </div>
<textarea id="addrhidden" rows="5" cols="20" style="display: none"><%=Model.person.Name %>
    <%=Model.person.PrimaryAddr.Address1 %>
    <% if (Model.person.PrimaryAddr.Address2.HasValue())
       { %><%=Model.person.PrimaryAddr.Address2 %>
    <% } %><%=Model.person.PrimaryAddr.CityStateZip() %>
</textarea>
    <div id="dialogbox" title="Search People" style="width: 560px; overflow: scroll">
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
