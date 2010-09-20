<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CmsWeb.Models.PersonPage.PersonModel>" %>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <link href="/Content/jquery.autocomplete.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%= SquishIt.Framework.Bundle.JavaScript()
            .Add("/Content/js/jquery.pagination.js")
            .Add("/Scripts/SearchPeople.js")
            .Add("/Content/js/jquery.autocomplete.js")
            .Add("/Scripts/Pager.js")
            .Add("/Scripts/Person.js")
        .Render("/Content/Person_#.js")
            %>        
    <% CmsWeb.Models.PersonPage.PersonInfo p = Model.displayperson; %>
    <table class="PersonHead" border="0">
        <tr>
            <td><%=Model.Name %></td>
            <td align="right"></td>
            <td>&nbsp;</td>
        </tr>
    </table>
    <table>
        <tr>
            <td>
                <div id="businesscard" href="/Person/BusinessCard/<%=p.PeopleId %>">
                <% Html.RenderPartial("BusinessCard", p); %>
                </div>
                <table>
                    <tr>
                        <td><%=p.basic.DoNotCall %></td>
                        <td>
                            <%=Html.HyperlinkIf(User.IsInRole("Finance"), "/Reports/ContributionYears/" + p.PeopleId, "Contributions", null, null)%>
                            <%=Html.HyperlinkIf(Model.CanCheckIn, "/CheckIn/CheckIn/{0}?pid={1}".Fmt(Model.ckorg, p.PeopleId), "CheckIn", null, null)%>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <% if (Page.User.IsInRole("Admin"))
                               { %>
                            <a id="deleteperson" href="/Person/Delete/<%=p.PeopleId %>"><img border="0" src="/images/delete.gif" /></a>
                            <a id="moveperson" href="/Person/Move/<%=p.PeopleId %>">move</a>
                            <% } %>
                        </td>
                    </tr>
                </table>
            </td>
            <td valign="top">
                <a id="Picture" href="/UploadPicture.aspx?id=<%=p.PeopleId %>" title="Click to see larger version or upload new">
                <img alt="portrait" border="0" src="/Image.aspx?portrait=1&id=<%=p.SmallPicId %>" />
                </a>
            </td>
            <td></td>
            <td valign="top">
                <br />
                <table>
                    <tr>
                        <th align="left" colspan="4">
                        <% if (User.IsInRole("Access"))
                           { %>
                            <a href="/Family.aspx?id=<%=p.FamilyId %>"><strong>Family Members</strong></a>
                        <% }
                           else
                           { %>
                           <strong>Family Members</strong>
                        <% } %>
                        </th>
                    </tr>
                <% foreach (var m in Model.FamilyMembers())
                   { %>
                    <tr>
                        <td><a href="/Person/Index/<%=m.Id %>"><span style='color: <%=m.Color%>'><%=m.Name %></span></a></td>
                        <td><%=m.SpouseIndicator %></td>
                        <td><%=m.Age %></td>
                        <td><%=m.PositionInFamily %></td>
                        <td><%=m.Email %></td>
                    </tr>
                <% } %>
                </table>
            </td>
       </tr>
    </table>
    <div id="main-tab" class="ui-tabs">
        <ul class="ui-tabs-nav">
            <li><a href="#basic-tab"><span>Basic</span></a></li>
            <li><a href="#address-tab"><span>Addresses</span></a></li>
            <li><a id="enrollment-link" href="#enrollment-tab"><span>Enrollment</span></a></li>
            <li><a href="#member-tab"><span>Member Profile</span></a></li>
<% if (User.IsInRole("Access"))
   { %>
            <li><a id="growth-link" href="#growth-tab"><span>Growth</span></a></li>
<% }
   if (User.IsInRole("Edit"))
   { %>
            <li><a id="system-link" href="#system-tab"><span>System</span></a></li>
<% } %>
        </ul>
        <div id="basic-tab" class="ui-tabs-panel ui-tabs-hide">
            <form class="DisplayEdit" action="">
            <% Html.RenderPartial("BasicDisplay", p.basic); %>
            </form>
        </div>
        <div id="address-tab" class="ui-tabs-hide ui-tabs-panel">
            <ul class="ui-tabs-nav">
                <li><a href="#PersonalAddr"><span>Personal</span></a></li>
                <li><a href="#AltPersonalAddr"><span>Personal Alternate</span></a></li>
                <li><a href="#FamilyAddr"><span>Family</span></a></li>
                <li><a href="#AltFamilyAddr"><span>Family Alternate</span></a></li>
            </ul>
            <div id="PersonalAddr" class="ui-tabs-hide ui-tabs-panel">
                <form class="DisplayEdit" action="">
                <% Html.RenderPartial("AddressDisplay", p.PersonalAddr); %>
                </form>
            </div>
            <div id="AltPersonalAddr" class="ui-tabs-hide ui-tabs-panel">
                <form class="DisplayEdit" action="">
                <% Html.RenderPartial("AddressDisplay", p.AltPersonalAddr); %>
                </form>
            </div>
            <div id="FamilyAddr" class="ui-tabs-hide ui-tabs-panel">
                <form class="DisplayEdit" action="">
                <% Html.RenderPartial("AddressDisplay", p.FamilyAddr); %>
                </form>
            </div>
            <div id="AltFamilyAddr" class="ui-tabs-hide ui-tabs-panel">
                <form class="DisplayEdit" action="">
                <% Html.RenderPartial("AddressDisplay", p.AltFamilyAddr); %>
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
                <li><a id="recreg-link" href="#recreg-tab"><span>Registration</span></a></li>
            </ul>
            <div id="current-tab" class="ui-tabs-hide ui-tabs-panel">
                <form action="/Person/EnrollGrid/<%=p.PeopleId %>">
                </form>
            </div>
            <div id="previous-tab" class="ui-tabs-hide ui-tabs-panel">
                <form action="/Person/PrevEnrollGrid/<%=p.PeopleId %>">
                </form>
            </div>
            <div id="pending-tab" class="ui-tabs-hide ui-tabs-panel">
                <form action="/Person/PendingEnrollGrid/<%=p.PeopleId %>">
                </form>
            </div>
            <div id="attendance-tab" class="ui-tabs-hide ui-tabs-panel">
                <form action="/Person/AttendanceGrid/<%=p.PeopleId %>">
                </form>
            </div>
            <div id="recreg-tab" class="ui-tabs-panel ui-tabs-hide">
                <form class="DisplayEdit" action="/Person/RecRegDisplay/<%=p.PeopleId %>">
                </form>
            </div>
        </div>
        <div id="member-tab" class="ui-tabs-hide ui-tabs-panel">
            <ul class="ui-tabs-nav">
                <li><a href="#membersum-tab"><span>Summary</span></a></li>
                <li><a href="#membernotes-tab"><span>Notes</span></a></li>
            </ul>
            <div id="membersum-tab" class="ui-tabs-hide ui-tabs-panel">
                <form class="DisplayEdit" action="">
                <% Html.RenderPartial("MemberDisplay", p.member); %>
                </form>
            </div>
            <div id="membernotes-tab" class="ui-tabs-hide ui-tabs-panel">
                <form class="DisplayEdit" action="">
                <% Html.RenderPartial("MemberNotesDisplay", p.membernotes); %>
                </form>
            </div>
        </div>
<% if (User.IsInRole("Access"))
   { %>
        <div id="growth-tab" class="ui-tabs-hide ui-tabs-panel">
            <ul class="ui-tabs-nav">
                <li><a href="#entry-tab"><span>Entry</span></a></li>
                <li><a href="#contacts-tab"><span>Contacts</span></a></li>
                <li><a href="#comments-tab"><span>Comments</span></a></li>
                <li><a href="#volunteer-tab"><span>Volunteer</span></a></li>
            </ul>
            <div id="entry-tab" class="ui-tabs-hide ui-tabs-panel">
                <form class="DisplayEdit" action="">
                <% Html.RenderPartial("GrowthDisplay", p.growth); %>
                </form>
            </div>
            <div id="contacts-tab" class="ui-tabs-hide ui-tabs-panel">
                <% Html.RenderPartial("ContactsDisplay", p.PeopleId); %>
            </div>
            <div id="comments-tab" class="ui-tabs-hide ui-tabs-panel">
                <form class="DisplayEdit" action="">
                <% Html.RenderPartial("CommentsDisplay"); %>
                </form>
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
                        <td>
        <% if (User.IsInRole("ApplicationReview"))
           { %>
                        <a href="/AppReview/VolunteerApp.aspx?id=<%=p.PeopleId %>">
                        Volunteer Application Review</a>
        <% } %>
                        </td>
                    </tr>
                    <tr>
                        <th>Approval Date:</th>
                        <td colspan="2"><%=Model.vol.ProcessedDate.FormatDate()%></td>
                    </tr>
                </table>
            </div>
        </div>
<% }
   if (User.IsInRole("Edit"))
   { %>
        <div id="system-tab" class="ui-tabs-hide ui-tabs-panel">
            <ul class="ui-tabs-nav">
                <li><a href="#user-tab"><span>User</span></a></li>
                <li><a href="#changes-tab"><span>Changes</span></a></li>
            </ul>
            <div id="user-tab" class="ui-tabs-hide ui-tabs-panel">
                <form action="/Person/UserInfoGrid/<%=p.PeopleId %>">
                </form>
            </div>
            <div id="changes-tab" class="ui-tabs-hide ui-tabs-panel">
                <form action="/Person/PeopleExtrasGrid/<%=p.PeopleId %>">
                </form>
            </div>
        </div>
<% } %>
    </div>
    <div id="dialogbox" title="Search People" style="width: 560px; overflow: scroll">
    </div>
    <div id="memberDialog">
    <iframe style="width:99%;height:99%"></iframe>
    </div>
</asp:Content>
