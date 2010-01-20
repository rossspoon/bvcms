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
                                    <%=Model.person.DeceasedDate.Value.ToShortDateString() %>
                                </td>
                            </tr>
                            <% } %>
                            <tr>
                                <td><a href="http://www.google.com/maps?q=<%=Model.person.AddrCityStateZip %>" target="_blank">
                                        <%=Model.person.PrimaryAddress %></a>
                                </td>
                                <td><a id="clipaddr" href="#" title="copy name and address to clipboard">clipboard</a>
                                </td>
                            </tr>
                            <% if (Model.person.PrimaryAddress2.HasValue())
                               { %>
                            <tr>
                                <td colspan="2">
                                    <a href="http://www.google.com/maps?q=<%=Model.person.Addr2CityStateZip %>"
                                        target="_blank"><%=Model.person.PrimaryAddress2 %></a>
                                </td>
                            </tr>
                            <% } %>
                            <tr>
                                <td><%=Model.person.CityStateZip %></td>
                                <td><a href='http://www.google.com/maps?f=d&saddr=2000+Appling+Rd,+Cordova,+Tennessee+38016&pw=2&daddr=<%=Model.person.AddrCityStateZip %>'
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
                                <td><%=Model.person.DoNotCallFlag ? "Do Not Call" : "" %></td>
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
                    <img alt="portrait" border="0" src="/Image.aspx?portrait=1&id=<%=Model.SmallPic %>" />
                    </a>
                </td>
                <td class="style1"></td>
                <td valign="top">
                    <br />
                    <table>
                        <tr><th></th><th align="left" colspan="3"><a href="/Family.aspx?id=<%=Model.person.FamilyId %>"><strong>Family Members</strong></a></th></tr>
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
            <div style="float:left">
                <table class="Design2">
                    <tr><th>Goes By:</th>
                        <td><%=Model.person.NickName %></td>
                    </tr>
                    <tr><th>Title:</th>
                        <td><%=Model.person.TitleCode %></td>
                    </tr>
                    <tr><th>First:</th>
                        <td><%=Model.person.FirstName %></td>
                    </tr>
                    <tr><th>Middle:</th>
                        <td><%=Model.person.MiddleName %></td>
                    </tr>
                    <tr><th>Last:</th>
                        <td><%=Model.person.LastName %></td>
                    </tr>
                    <tr><th>Suffix:</th>
                        <td><%=Model.person.SuffixCode %></td>
                    </tr>
                    <tr><th>Former:</th>
                        <td><%=Model.person.MaidenName %></td>
                    </tr>
                    <tr><th>Gender:</th>
                        <td><%=Model.person.Gender.Description %></td>
                    </tr>
                </table>
            </div>
            <div style="float:left">
                <table class="Design2">
                    <tr>
                        <th>Home Phone:</th>
                        <td><%=Model.person.Family.HomePhone.FmtFone() %></td>
                    </tr>
                    <tr>
                        <th>Cell Phone:</th>
                        <td></td>
                    </tr>
                    <tr>
                        <th>Work Phone:</th>
                        <td><%=Model.person.WorkPhone.FmtFone() %></td>
                    </tr>
                    <tr>
                        <th>Email:</th>
                        <td><%=Model.person.EmailAddress %></td>
                    </tr>
                    <tr>
                        <th>School:</th>
                        <td><%=Model.person.SchoolOther %></td>
                    </tr>
                    <tr>
                        <th>Grade:</th>
                        <td><%=Model.person.Grade %></td>
                    </tr>
                    <tr>
                        <th>Employer:</th>
                        <td><%=Model.person.EmployerOther %></td>
                    </tr>
                    <tr>
                        <th>Occupation:</th>
                        <td><%=Model.person.OccupationOther %></td>
                    </tr>
                </table>
            </div>
            <div style="float:left">
                <table class="Design2">
                    <tr>
                        <th>Campus:</th>
                        <td><%=Model.person.Campu.Description %></td>
                    </tr>
                    <tr>
                        <th>Member Status:</th>
                        <td><%=Model.person.MemberStatus.Description %></td>
                    </tr>
                    <tr>
                        <th>Joined:</th>
                        <td><%=Model.person.JoinDate.ToShortDateStr() %></td>
                    </tr>
                    <tr>
                        <th>Marital Status:</th>
                        <td><%=Model.person.MaritalStatus.Description %></td>
                    </tr>
                    <tr>
                        <th>Spouse:</th>
                        <td><%=Model.person.SpouseName %></td>
                    </tr>
                    <tr>
                        <th>Wedding Date:</th>
                        <td><%=Model.person.WeddingDate.ToShortDateStr() %></td>
                    </tr>
                    <tr>
                        <th>Birthday:</th>
                        <td><%=Model.person.DOB %></td>
                    </tr>
                    <tr>
                        <th>Deceased:</th>
                        <td><%=Model.person.DeceasedDate.ToShortDateStr() %></td>
                    </tr>
                    <tr>
                        <th>Age:</th>
                        <td><%=Model.person.Age %></td>
                    </tr>
                </table>
            </div>
            <div style="float:left">
                <table class="Design2">
                    <tr><td><%=Model.person.DoNotCallFlag ? "Do Not Call" : "" %></td></tr>
                    <tr><td><%=Model.person.DoNotVisitFlag ? "Do Not Visit" : "" %></td></tr>
                    <tr><td><%=Model.person.DoNotMailFlag ? "Do Not Mail" : "" %></td></tr>
                </table>
            </div>
            <div style="clear:both"></div>
        </div>
        <div id="address-tab" class="ui-tabs-hide ui-tabs-panel">
            <ul class="ui-tabs-nav">
                <li><a href="#personal1-tab"><span>Personal</span></a></li>
                <li><a href="#personal2-tab"><span>Personal Alternate</span></a></li>
                <li><a href="#family1-tab"><span>Family</span></a></li>
                <li><a href="#family2-tab"><span>Family Alternate</span></a></li>
            </ul>
            <div id="personal1-tab" class="ui-tabs-hide ui-tabs-panel">
                <% Html.RenderPartial("Address", Model.GetAddress("personal1-tab")); %>
            </div>
            <div id="personal2-tab" class="ui-tabs-hide ui-tabs-panel">
                <% Html.RenderPartial("Address", Model.GetAddress("personal2-tab")); %>
            </div>
            <div id="family1-tab" class="ui-tabs-hide ui-tabs-panel">
                <% Html.RenderPartial("Address", Model.GetAddress("family1-tab")); %>
            </div>
            <div id="family2-tab" class="ui-tabs-hide ui-tabs-panel">
                <% Html.RenderPartial("Address", Model.GetAddress("family2-tab")); %>
            </div>
            <%=Html.Hidden("addrtab") %>
        </div>
        <div id="enrollment-tab" class="ui-tabs-hide ui-tabs-panel">
<%--            <ul class="ui-tabs-nav">
                <li><a href="#current-tab"><span>Current</span></a></li>
                <li><a id="previous-link" href="#previous-tab"><span>Previous</span></a></li>
                <li><a id="pending-link" href="#pending-tab"><span>Pending</span></a></li>
                <li><a id="attendance-link" href="#attendance-tab"><span>Attendance History</span></a></li>
            </ul>
            <div id="current-tab" class="ui-tabs-hide ui-tabs-panel">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:GridView ID="EnrollGrid" runat="server" AutoGenerateColumns="False" DataSourceID="EnrollData"
                            SkinID="GridViewSkin" AllowPaging="true" AllowSorting="true" Visible="false"
                            PageSize="10" EmptyDataText="No Current Enrollments Found." OnRowDataBound="Enroll_RowDataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="Organization" SortExpression="Name">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="OrgLink" runat="server" NavigateUrl='<%# Eval("Id", "~/Organization.aspx?id={0}") %>'
                                            Text='<%# Eval("Name") %>' ToolTip='<%# Eval("DivisionName") %>'></asp:HyperLink></ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Location" HeaderText="Location" />
                                <asp:TemplateField HeaderText="Leader">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="LeaderLink" runat="server" NavigateUrl='<%# Eval("LeaderId", "~/Person.aspx?id={0}") %>'
                                            Text='<%# Eval("LeaderName") %>'></asp:HyperLink></ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Schedule" HeaderText="Schedule" />
                                <asp:BoundField DataField="EnrollDate" DataFormatString="{0:d}" HeaderText="Enroll Date"
                                    SortExpression="EnrollDate" />
                                <asp:TemplateField HeaderText="MemberType">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="MemberLink" runat="server" CssClass="thickbox3" Text='<%# Eval("MemberType") %>'></asp:HyperLink></ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="AttendPct" DataFormatString="{0:N1}" HeaderText="AttendPct" />
                            </Columns>
                            <PagerTemplate>
                                <uc1:GridPager ID="GridPager1" runat="server" />
                            </PagerTemplate>
                        </asp:GridView>
                        <div style="visibility: hidden">
                            <asp:Button ID="HiddenButton1" runat="server" OnClick="HiddenButton1_Click" Text="Button" />
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="HiddenButton1" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div id="previous-tab" class="ui-tabs-hide ui-tabs-panel">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:GridView ID="PrevEnrollGrid" runat="server" AutoGenerateColumns="False" DataSourceID="PreviousEnrollData"
                            SkinID="GridViewSkin" AllowPaging="true" AllowSorting="true" Visible="false"
                            EmptyDataText="No Previous Enrollments Found." PageSize="10" OnRowDataBound="PrevEnroll_RowDataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="Organization" SortExpression="Name">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="OrgLink" runat="server" NavigateUrl='<%# Eval("Id", "~/Organization.aspx?id={0}") %>'
                                            Text='<%# Eval("Name") %>' ToolTip='<%# Eval("DivisionName") %>'></asp:HyperLink></ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Location" HeaderText="Location" />
                                <asp:TemplateField HeaderText="Leader">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="LeaderLink" runat="server" NavigateUrl='<%# Eval("LeaderId", "~/Person.aspx?id={0}") %>'
                                            Text='<%# Eval("LeaderName") %>'></asp:HyperLink></ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Schedule" HeaderText="Schedule" />
                                <asp:BoundField DataField="EnrollDate" DataFormatString="{0:d}" SortExpression="EnrollDate"
                                    HeaderText="EnrollDate" />
                                <asp:BoundField DataField="DropDate" DataFormatString="{0:d}" HeaderText="DropDate" />
                                <asp:TemplateField HeaderText="MemberType">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="MemberLink" runat="server" Target="_blank" Text='<%# Eval("MemberType") %>'></asp:HyperLink></ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="AttendPct" DataFormatString="{0:N1}" HeaderText="AttendPct" />
                            </Columns>
                            <PagerTemplate>
                                <uc1:GridPager ID="GridPager1" runat="server" />
                            </PagerTemplate>
                        </asp:GridView>
                        <div style="visibility: hidden">
                            <asp:Button ID="HiddenButton2" runat="server" OnClick="HiddenButton2_Click" Text="Button" />
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="HiddenButton2" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div id="pending-tab" class="ui-tabs-hide ui-tabs-panel">
                <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:GridView ID="PendingEnrollGrid" runat="server" AutoGenerateColumns="False" DataSourceID="PendingEnrollData"
                            SkinID="GridViewSkin" Visible="false" EmptyDataText="No Pending Enrollments Found.">
                            <Columns>
                                <asp:TemplateField HeaderText="Organization" SortExpression="OrganizationName">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="OrgLink" runat="server" NavigateUrl='<%# Eval("Id", "~/Organization.aspx?id={0}") %>'
                                            Text='<%# Eval("Name") %>' ToolTip='<%# Eval("DivisionName") %>'></asp:HyperLink></ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Location" HeaderText="Location" SortExpression="Location" />
                                <asp:TemplateField HeaderText="Leader" SortExpression="Leader">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="LeaderLink" runat="server" NavigateUrl='<%# Eval("LeaderId", "~/Person.aspx?id={0}") %>'
                                            Text='<%# Eval("LeaderName") %>'></asp:HyperLink></ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Schedule" HeaderText="Schedule" SortExpression="Schedule" />
                                <asp:BoundField DataField="EnrollDate" DataFormatString="{0:d}" HeaderText="EnrollDate" />
                                <asp:BoundField DataField="MemberType" HeaderText="MemberType" SortExpression="MemberType" />
                            </Columns>
                        </asp:GridView>
                        <div style="visibility: hidden">
                            <asp:Button ID="HiddenButton3" runat="server" OnClick="HiddenButton3_Click" Text="Button" />
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="HiddenButton3" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div id="attendance-tab" class="ui-tabs-hide ui-tabs-panel">
                <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:CheckBox ID="future" runat="server" Text="Future Commitments" AutoPostBack="true" OnCheckedChanged="ShowAttends" />
                        <asp:GridView ID="AttendGrid" runat="server" AllowPaging="True" SkinID="GridViewSkin"
                            AllowSorting="True" AutoGenerateColumns="False" DataSourceID="AttendData" PageSize="10"
                            EmptyDataText="No Attendance History Found." Visible="false">
                            <Columns>
                                <asp:HyperLinkField DataNavigateUrlFields="MeetingId" DataNavigateUrlFormatString="~/Meeting.aspx?id={0}"
                                    DataTextField="MeetingDate" DataTextFormatString="{0:MM/dd/yy h:mmtt}" HeaderText="Meeting"
                                    SortExpression="MeetingDate" />
                                <asp:HyperLinkField DataNavigateUrlFields="OrganizationId" DataNavigateUrlFormatString="~/Organization.aspx?id={0}"
                                    DataTextField="OrganizationName" DataTextFormatString="{0}" HeaderText="Organization"
                                    SortExpression="Organization" />
                                <asp:BoundField DataField="MemberType" HeaderText="MemberType" SortExpression="MemberType" />
                                <asp:BoundField DataField="AttendType" HeaderText="AttendType" SortExpression="AttendType" />
                            </Columns>
                            <PagerTemplate>
                                <uc1:GridPager ID="GridPager1" runat="server" />
                            </PagerTemplate>
                        </asp:GridView>
                        <div style="visibility: hidden">
                            <asp:Button ID="HiddenButton4" runat="server" OnClick="HiddenButton4_Click" Text="Button" />
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="HiddenButton4" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
--%>        </div>
        <div id="member-tab" class="ui-tabs-hide ui-tabs-panel">
<%--            <ul class="ui-tabs-nav">
                <li><a href="#membersum-tab"><span>Summary</span></a></li>
                <li><a href="#membernotes-tab"><span>Notes</span></a></li>
            </ul>
            <div id="membersum-tab" class="ui-tabs-hide ui-tabs-panel">
                <table>
                    <tr>
                        <td valign="top">
                            <table class="Design2">
                                <tr>
                                    <th>
                                        Contribution Statement:
                                    </th>
                                    <td>
                                        <cc1:DisplayOrEditDropDown ID="EnvelopeOptionsId0" runat="server" BindingMember="ContributionOptionsId"
                                            BindingMode="TwoWay" BindingSource="person" DataSourceID="ODSEnvelopeOptionsId"
                                            DataTextField="Value" DataValueField="Id" EditRole="Membership" MakeDefault0="True"
                                            Width="150px">
                                        </cc1:DisplayOrEditDropDown>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>
                        </td>
                        <td valign="top">
                            <table class="Design2">
                                <tr>
                                    <th>
                                        Envelope Option:
                                    </th>
                                    <td>
                                        <cc1:DisplayOrEditDropDown ID="EnvelopeOptionsId" runat="server" BindingMode="TwoWay"
                                            MakeDefault0="True" BindingSource="person" BindingMember="EnvelopeOptionsId"
                                            DataTextField="Value" DataValueField="Id" Width="150px" DataSourceID="ODSEnvelopeOptionsId"
                                            EditRole="Membership">
                                        </cc1:DisplayOrEditDropDown>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <table class="Design2">
                                <tr>
                                    <th colspan="2" class="LightBlueBG">
                                        Decision
                                    </th>
                                </tr>
                                <tr>
                                    <th>
                                        Type:
                                    </th>
                                    <td>
                                        <cc1:DisplayOrEditDropDown ID="DecisionTypeId" runat="server" BindingMode="TwoWay"
                                            MakeDefault0="True" BindingSource="person" BindingMember="DecisionTypeId" DataTextField="Value"
                                            DataValueField="Id" Width="150px" DataSourceID="ODSDecisionTypeId" 
                                            EditRole="Membership">
                                        </cc1:DisplayOrEditDropDown>
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                        Date:
                                    </th>
                                    <td>
                                        <cc1:DisplayOrEditDate ID="DecisionDate" runat="server" BindingSource="person" BindingMember="DecisionDate"
                                            BindingMode="TwoWay" EditRole="Membership" />
                                        <cc2:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="DecisionDate"
                                            PopupPosition="TopLeft" Enabled="True">
                                        </cc2:CalendarExtender>
                                <asp:CustomValidator ID="ValidateDecision" runat="server" 
                                    ErrorMessage="ValidateDecision" ClientValidationFunction="ValidateDecisionDate"
                                    Display="None" ValidateEmptyText="True"></asp:CustomValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                    </th>
                                    <td>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>
                        </td>
                        <td valign="top">
                            <table class="Design2">
                                <tr>
                                    <th colspan="2" class="LightBlueBG">
                                        Join
                                    </th>
                                </tr>
                                <tr>
                                    <th>
                                        Type:
                                    </th>
                                    <td>
                                        <cc1:DisplayOrEditDropDown ID="JoinCodeId" runat="server" BindingMode="TwoWay" BindingSource="person"
                                            BindingMember="JoinCodeId" DataTextField="Value" DataValueField="Id" Width="150px"
                                            DataSourceID="ODSJoinCodeId" EditRole="Membership">
                                        </cc1:DisplayOrEditDropDown>
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                        Date:
                                    </th>
                                    <td>
                                        <cc1:DisplayOrEditDate ID="JoinDate1" runat="server" BindingSource="person" BindingMember="JoinDate"
                                            BindingMode="TwoWay" EditRole="Membership" />
                                        <cc2:CalendarExtender ID="CalendarExtender5" runat="server" TargetControlID="JoinDate1"
                                            PopupPosition="TopLeft" Enabled="True">
                                        </cc2:CalendarExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                        Previous Church:
                                    </th>
                                    <td>
                                        <cc1:DisplayOrEditText ID="OtherPreviousChurch" runat="server" BindingMember="OtherPreviousChurch"
                                            BindingMode="TwoWay" BindingSource="person" Width="135px" ChangedStatus="False"></cc1:DisplayOrEditText>
                                        <cc2:AutoCompleteExtender ID="ContextStr_AutoCompleteExtender" runat="server" TargetControlID="OtherPreviousChurch"
                                            ServiceMethod="GetPrevChurchCompletionList" BehaviorID="autoComplete" CompletionSetCount="10"
                                            MinimumPrefixLength="2" CompletionInterval="100" CompletionListCssClass="autocomplete_list"
                                            CompletionListItemCssClass="autocomplete_listItem" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td valign="top">
                            <table class="Design2" width="100%">
                                <tr>
                                    <th colspan="2" class="LightBlueBG">
                                        Church Membership
                                    </th>
                                </tr>
                                <tr>
                                    <th>
                                        Member Status:
                                    </th>
                                    <td>
                                        <cc1:DisplayOrEditDropDown ID="MemberStatusId" runat="server" BindingMember="MemberStatusId"
                                            BindingMode="TwoWay" BindingSource="person" DataTextField="Value" DataValueField="Id"
                                            MakeDefault0="False" Width="150px" DataSourceID="ODSMemberStatusId" ChangedStatus="False"
                                            EditRole="Membership">
                                        </cc1:DisplayOrEditDropDown>
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                        Joined:
                                    </th>
                                    <td>
                                        <cc1:DisplayOrEditDate ID="JoinDate" runat="server" BindingMember="JoinDate" BindingMode="TwoWay"
                                            BindingSource="person" ChangedStatus="False" EditRole="Membership"></cc1:DisplayOrEditDate>
                                        <cc2:CalendarExtender ID="CalendarExtender8" runat="server" Enabled="True" PopupPosition="TopLeft"
                                            TargetControlID="JoinDate">
                                        </cc2:CalendarExtender>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <table class="Design2" width="100%">
                                <tr>
                                    <th colspan="2" class="LightBlueBG">
                                        Baptism
                                    </th>
                                </tr>
                                <tr>
                                    <th>
                                        Type:
                                    </th>
                                    <td>
                                        <cc1:DisplayOrEditDropDown ID="BaptismTypeId" runat="server" BindingMode="TwoWay"
                                            MakeDefault0="True" BindingSource="person" BindingMember="BaptismTypeId" DataTextField="Value"
                                            DataValueField="Id" Width="150px" DataSourceID="ODSBaptismTypeId" EditRole="Membership">
                                        </cc1:DisplayOrEditDropDown>
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                        Status:
                                    </th>
                                    <td>
                                        <cc1:DisplayOrEditDropDown ID="BaptismStatusId" runat="server" BindingMode="TwoWay"
                                            MakeDefault0="True" BindingSource="person" BindingMember="BaptismStatusId" DataTextField="Value"
                                            DataValueField="Id" Width="150px" DataSourceID="ODSBaptismStatusId" EditRole="Membership">
                                        </cc1:DisplayOrEditDropDown>
                                    </td>
                                </tr>
                                <tr id="trBaptismDate" runat="server">
                                    <th id="Th5" runat="server">
                                        Date:
                                    </th>
                                    <td id="Td5" runat="server">
                                        <cc1:DisplayOrEditDate ID="BaptismDate" runat="server" BindingSource="person" BindingMember="BaptismDate"
                                            BindingMode="TwoWay" EditRole="Membership" />
                                        <cc2:CalendarExtender ID="CalendarExtender4" runat="server" TargetControlID="BaptismDate"
                                            PopupPosition="TopLeft" Enabled="True">
                                        </cc2:CalendarExtender>
                                <asp:CustomValidator ID="ValidateBaptism" runat="server" 
                                    ErrorMessage="ValidateBaptism" ClientValidationFunction="ValidateBaptismDate"
                                    Display="None" ValidateEmptyText="True"></asp:CustomValidator>
                                    </td>
                                </tr>
                                <tr id="trBaptismScheduleDate" runat="server">
                                    <th id="Th6" runat="server">
                                        Scheduled:
                                    </th>
                                    <td id="Td6" runat="server">
                                        <cc1:DisplayOrEditDate ID="BaptismScheduleDate" runat="server" BindingSource="person"
                                            BindingMember="BaptismSchedDate" BindingMode="TwoWay" EditRole="Membership" />
                                        <cc2:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="BaptismScheduleDate"
                                            PopupPosition="TopLeft" Enabled="True">
                                        </cc2:CalendarExtender>
                                    </td>
                                </tr>
                            </table>
                            <td>
                            </td>
                            <td valign="top">
                                <table class="Design2" width="100%">
                                    <tr>
                                        <th colspan="2" class="LightBlueBG">
                                            Drop
                                        </th>
                                    </tr>
                                    <tr>
                                        <th>
                                            Type:
                                        </th>
                                        <td>
                                            <cc1:DisplayOrEditDropDown ID="DropCodeId" runat="server" BindingMode="TwoWay" BindingSource="person"
                                                DataTextField="Value" DataValueField="Id" Width="150px" BindingMember="DropCodeId"
                                                DataSourceID="ODSDropCodeId" EditRole="Membership">
                                            </cc1:DisplayOrEditDropDown>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>
                                            Date:
                                        </th>
                                        <td>
                                            <cc1:DisplayOrEditDate ID="DropDate" runat="server" BindingSource="person" BindingMode="TwoWay"
                                                BindingMember="DropDate" EditRole="Membership" />
                                            <cc2:CalendarExtender ID="CalendarExtender6" runat="server" TargetControlID="DropDate"
                                                PopupPosition="TopLeft" Enabled="True">
                                            </cc2:CalendarExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>
                                            New Church:
                                        </th>
                                        <td>
                                            <cc1:DisplayOrEditText ID="OtherNewChurch" runat="server" BindingMember="OtherNewChurch"
                                                BindingMode="TwoWay" BindingSource="person" Width="135px" ChangedStatus="False"
                                                EditRole="Membership"></cc1:DisplayOrEditText>
                                            <cc2:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" TargetControlID="OtherNewChurch"
                                                ServiceMethod="GetNewChurchCompletionList" BehaviorID="autoComplete2" CompletionSetCount="10"
                                                MinimumPrefixLength="2" CompletionInterval="100" CompletionListCssClass="autocomplete_list"
                                                CompletionListItemCssClass="autocomplete_listItem" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td valign="top">
                                <table class="Design2" width="100%">
                                    <tr>
                                        <th colspan="2" class="LightBlueBG">
                                            Step 1 Class
                                        </th>
                                    </tr>
                                    <tr>
                                        <th>
                                            Status:
                                        </th>
                                        <td>
                                            <cc1:DisplayOrEditDropDown ID="DiscoveryClassStatusID" runat="server" BindingMode="TwoWay"
                                                MakeDefault0="True" BindingSource="person" BindingMember="DiscoveryClassStatusId"
                                                DataTextField="Value" DataValueField="Id" Width="150px" DataSourceID="ODSDiscoveryClassStatusID"
                                                EditRole="Membership">
                                            </cc1:DisplayOrEditDropDown>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>
                                            Date:
                                        </th>
                                        <td>
                                            <cc1:DisplayOrEditDate ID="DiscoveryClassDate" runat="server" BindingSource="person"
                                                BindingMember="DiscoveryClassDate" BindingMode="TwoWay" EditRole="Membership" />
                                            <cc2:CalendarExtender ID="CalendarExtender7" runat="server" TargetControlID="DiscoveryClassDate"
                                                PopupPosition="TopLeft" Enabled="True">
                                            </cc2:CalendarExtender>
                                <asp:CustomValidator ID="ValidateStep1" runat="server" 
                                    ErrorMessage="ValidateStep1" ClientValidationFunction="ValidateStep1Date"
                                    Display="None" ValidateEmptyText="True"></asp:CustomValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>
                                            &nbsp;
                                        </th>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </td>
                    </tr>
                </table>
            </div>
            <div id="membernotes-tab" class="ui-tabs-hide ui-tabs-panel">
                <table>
                    <tr>
                        <td valign="top">
                            <table class="Design2">
                                <tr>
                                    <th colspan="6" class="LightBlueBG">
                                        Letter
                                    </th>
                                </tr>
                                <tr>
                                    <th>
                                        Status:
                                    </th>
                                    <td>
                                        <cc1:DisplayOrEditDropDown ID="LetterStatusId" runat="server" BindingMode="TwoWay"
                                            MakeDefault0="true" BindingSource="person" DataTextField="Value" DataValueField="Id"
                                            Width="150px" DataSourceID="ODSLetterStatusId">
                                        </cc1:DisplayOrEditDropDown>
                                    </td>
                                    <th>
                                        Date Requested:
                                    </th>
                                    <td>
                                        <cc1:DisplayOrEditDate ID="LetterDateRequested" runat="server" BindingSource="person"
                                            BindingMode="TwoWay" />
                                        <cc2:CalendarExtender CssClass="" ID="CalendarExtender9" runat="server" TargetControlID="LetterDateRequested">
                                        </cc2:CalendarExtender>
                                    </td>
                                    <th>
                                        Date Received:
                                    </th>
                                    <td>
                                        <cc1:DisplayOrEditDate ID="LetterDateReceived" runat="server" BindingSource="person"
                                            BindingMode="TwoWay" />
                                        <cc2:CalendarExtender CssClass="" ID="CalendarExtender11" runat="server" TargetControlID="LetterDateReceived">
                                        </cc2:CalendarExtender>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <strong>Notes:</strong>
                            <br />
                            <cc1:DisplayOrEditText ID="LetterStatusNotes" runat="server" BindingSource="person"
                                BindingMode="TwoWay" TextMode="MultiLine" Height="150" Width="400"></cc1:DisplayOrEditText>
                        </td>
                    </tr>
                </table>
            </div>
--%>        </div>
        <div id="growth-tab" class="ui-tabs-hide ui-tabs-panel">
<%--            <table>
                <tr>
                    <td valign="top" style="border-style: groove; border-width: thin;">
                        <table class="Design2">
                            <tr>
                                <th>
                                    How did you hear about our Church?
                                </th>
                                <td>
                                    <cc1:DisplayOrEditDropDown ID="InterestPointId" runat="server" BindingMode="TwoWay"
                                        MakeDefault0="True" BindingSource="person" BindingMember="InterestPointId" DataTextField="Value"
                                        DataValueField="Id" Width="150px" DataSourceID="ODSInterestPointId">
                                    </cc1:DisplayOrEditDropDown>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    Origin:
                                </th>
                                <td>
                                    <cc1:DisplayOrEditDropDown ID="OriginId" runat="server" BindingMode="TwoWay" MakeDefault0="True"
                                        BindingSource="person" BindingMember="OriginId" DataTextField="Value" DataValueField="Id"
                                        Width="150px" DataSourceID="ODSOriginId">
                                    </cc1:DisplayOrEditDropDown>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    Entry Point:
                                </th>
                                <td>
                                    <cc1:DisplayOrEditDropDown ID="EntryPointId" runat="server" BindingMode="TwoWay"
                                        MakeDefault0="True" BindingSource="person" BindingMember="EntryPointId" DataTextField="Value"
                                        DataValueField="Id" Width="150px" DataSourceID="ODSEntryPointId">
                                    </cc1:DisplayOrEditDropDown>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    Is a member of another Church?
                                </th>
                                <td>
                                    <cc1:DisplayOrEditCheckbox ID="MemberAnyChurchFlag0" runat="server" BindingSource="person"
                                        BindingMember="MemberAnyChurch" BindingMode="TwoWay" />
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td>
                    </td>
                    <td valign="top" style="border-style: groove; border-width: thin;">
                        <table class="Design2">
                            <tr>
                                <td>
                                    <cc1:DisplayOrEditCheckbox ID="ChristAsSaviorFlag0" runat="server" BindingSource="person"
                                        BindingMember="ChristAsSavior" BindingMode="TwoWay" />
                                </td>
                                <th>
                                    Prayed for Christ as Savior
                                </th>
                            </tr>
                            <tr>
                                <td>
                                    <cc1:DisplayOrEditCheckbox ID="PleaseVisitFlag0" runat="server" BindingSource="person"
                                        BindingMember="PleaseVisit" BindingMode="TwoWay" />
                                </td>
                                <th>
                                    Would like someone to visit
                                </th>
                            </tr>
                            <tr>
                                <td>
                                    <cc1:DisplayOrEditCheckbox ID="InterestedInJoiningFlag0" runat="server" BindingSource="person"
                                        BindingMember="InterestedInJoining" BindingMode="TwoWay" />
                                </td>
                                <th>
                                    Interested in joining Church
                                </th>
                            </tr>
                            <tr>
                                <td>
                                    <cc1:DisplayOrEditCheckbox ID="InfoBecomeAChristianFlag0" runat="server" BindingSource="person"
                                        BindingMember="InfoBecomeAChristian" BindingMode="TwoWay" />
                                </td>
                                <th>
                                    Would like to know how to become a Christian
                                </th>
                            </tr>
                        </table>
                    </td>
                    <td>
                    </td>
                    <td valign="top" style="border-style: groove; border-width: thin;">
                        <table class="Design2">
                            <tr>
                                <td height="100%" width="100%">
                                    <strong>Comments:</strong>
                                    <br />
                                    <cc1:DisplayOrEditText ID="Comments" runat="server" BindingSource="person" BindingMember="Comments"
                                        BindingMode="TwoWay" TextMode="MultiLine" Height="150" Width="150"></cc1:DisplayOrEditText>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <strong>Contacts Given:</strong>
                        <cc1:LinkButtonConfirm ID="AddContactMadeLink" runat="server" OnClick="AddContactMade_Click"
                            Confirm="Are you sure you want to add a new contact record?">Add New</cc1:LinkButtonConfirm>
                    </td>
                    <td>
                    </td>
                    <td>
                        <strong>Contacts Received:</strong>
                        <cc1:LinkButtonConfirm ID="AddContactLink" runat="server" OnClick="AddContact_Click"
                            Confirm="Are you sure you want to add a new contact record?">Add New Contact</cc1:LinkButtonConfirm>
                        &nbsp;<cc1:LinkButtonConfirm ID="AddAboutTask" runat="server" OnClick="AddAboutTask_Click"
                            Confirm="Are you sure you want to add a new task about this person?">Add New Task</cc1:LinkButtonConfirm>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: top">
                        <asp:GridView ID="ContactsMadeGrid" runat="server" AllowPaging="True" SkinID="GridViewSkin"
                            AllowSorting="True" AutoGenerateColumns="False" DataSourceID="ContactsMadeList"
                            PageSize="10" EmptyDataText="No Contacts Made.">
                            <Columns>
                                <asp:TemplateField HeaderText="Contact Date" SortExpression="OrganizationName">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="ContactLink" runat="server" NavigateUrl='<%# Eval("ContactId", "~/Contact.aspx?id={0}") %>'
                                            Text='<%# Eval("ContactDate", "{0:d}") %>'></asp:HyperLink>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="TypeOfContact" HeaderText="Type" SortExpression="ContactType" />
                                <asp:BoundField DataField="ContactReason" HeaderText="Reason" SortExpression="ContactReason" />
                            </Columns>
                        </asp:GridView>
                    </td>
                    <td>
                    </td>
                    <td style="vertical-align: top">
                        <asp:GridView ID="PendingContacts" runat="server" SkinID="GridViewSkin"
                            AutoGenerateColumns="False" DataSourceID="TaskList"
                            EmptyDataText="No Tasks Found.">
                            <Columns>
                                <asp:TemplateField HeaderText="Task">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="TaskLink" runat="server" NavigateUrl='<%# Eval("link") %>'
                                            Text='<%# Eval("Desc") %>'></asp:HyperLink>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="AssignedDt" HeaderText="Date" DataFormatString="{0:d}" />
                                <asp:BoundField DataField="AssignedTo" HeaderText="Assigned To" />
                            </Columns>
                        </asp:GridView>
                        <asp:GridView ID="ContactedListGrid" runat="server" AllowPaging="True" SkinID="GridViewSkin"
                            AllowSorting="True" AutoGenerateColumns="False" DataSourceID="ContactedList"
                            PageSize="10" EmptyDataText="No Contacts Found.">
                            <Columns>
                                <asp:TemplateField HeaderText="Contact Date" SortExpression="OrganizationName">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="ContactLink" runat="server" NavigateUrl='<%# Eval("ContactId", "~/Contact.aspx?id={0}") %>'
                                            Text='<%# Eval("ContactDate", "{0:d}") %>'></asp:HyperLink>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="TypeOfContact" HeaderText="Type" SortExpression="ContactType" />
                                <asp:BoundField DataField="ContactReason" HeaderText="Reason" SortExpression="ContactReason" />
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
--%>        </div>
        <div id="volunteer-tab" class="ui-tabs-hide ui-tabs-panel">
<%--            <table class="Design2" style="border-style: groove; border-width: thin;">
                <tr>
                    <th>
                        Approvals:
                    </th>
                    <td>
                        <cc1:DisplayOrEditCheckbox ID="Standard" runat="server" BindingSource="vol" Text="Standard (references only)"
                            BindingMode="OneWay" /><br />
                        <cc1:DisplayOrEditCheckbox ID="Leader" runat="server" BindingSource="vol" Text="Leadership (background check)"
                            BindingMode="OneWay" />
                    </td>
                    <td>
                        &nbsp;
                        <asp:HyperLink ID="VolAppReview" runat="server">Volunteer Application Review</asp:HyperLink>
                    </td>
                </tr>
                <tr>
                    <th>
                        Approval Date:
                    </th>
                    <td colspan="2">
                        <cc1:DisplayOrEditDate ID="ProcessedDate" runat="server" BindingSource="vol" BindingMode="OneWay">
                        </cc1:DisplayOrEditDate>
                    </td>
                </tr>
            </table>
--%>        </div>
    </div>
<textarea id="addrhidden" rows="5" cols="20" style="display: none"><%=Model.person.Name %>
    <%=Model.person.PrimaryAddress %>
    <% if (Model.person.PrimaryAddress2.HasValue())
       { %><%=Model.person.PrimaryAddress2%>
    <% } %><%=Model.person.CityStateZip %>
</textarea>
    <div id="dialogbox" title="Search People" style="width: 560px; overflow: scroll">
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
