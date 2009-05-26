<%@ Page Language="C#" StylesheetTheme="Standard" MasterPageFile="~/Site.Master" AutoEventWireup="True" CodeBehind="Person.aspx.cs"
    Inherits="CMSWeb.PersonPage" Title="Person" EnableEventValidation="false" %>

<%@ Register Src="UserControls/ExportToolBar.ascx" TagName="ExportToolBar" TagPrefix="uc1" %>
<%@ Register Src="UserControls/GridPager.ascx" TagName="GridPager" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<%@ Register Src="UserControls/Address.ascx" TagName="address" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            width: 150px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript">
        var $decisiondt;
        $(function() {
            if ('<%=EditUpdateButton1.Editing?"true":"false"%>' == 'true') {
                $('a.thickbox2').unbind("click")
            }
            else {
                tb_init('a.thickbox2');
            }
            imgLoader = new Image();
            imgLoader.src = tb_pathToImage;
            decisiondt = $get('<%=DecisionTypeId.ClientID%>');

            $("#member-tab").tabs();
            var $maintabs = $("#main-tab").tabs();
            $("#enrollment-tab").tabs();
            var $addrtabs = $("#address-tab").tabs();

            var t = $.cookie('maintab');
            if (t && t != "2") // exclude enrollment
                $maintabs.tabs('select', parseInt(t));
            $addrtabs.tabs('select', '#<%=addrtab%>');

            $("#main-tab > ul > li > a").click(function() {
                var selected = $maintabs.data('selected.tabs');
                $.cookie('maintab', selected);
            });

            $("#enrollment-link").click(function() {
                if (!$get('<%=EnrollGrid.ClientID%>')) {
                    $get('<%=HiddenButton1.ClientID%>').click();
                }
            });
            $("#previous-link").click(function() {
                if (!$get('<%=PrevEnrollGrid.ClientID%>'))
                    $get('<%=HiddenButton2.ClientID%>').click();
            });
            $("#pending-link").click(function() {
                if (!$get('<%=PendingEnrollGrid.ClientID%>'))
                    $get('<%=HiddenButton3.ClientID%>').click();
            });
            $("#attendance-link").click(function() {
                if (!$get('<%=AttendGrid.ClientID%>'))
                    $get('<%=HiddenButton4.ClientID%>').click();
            });
        });

        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(partialLoadHandler);
        function partialLoadHandler(sender, args) {
           if(Array.contains(args.get_panelsUpdated(), $get('<%= UpdatePanel1.ClientID %>'))) {
                tb_init('a.thickbox3');
           }
        }
        function RebindMemberGrids(panel) {
            tb_remove();
            __doPostBack(panel, 'RebindMemberGrids');
        }
        function ToggleTagCallback(e) {
            var result = eval('(' + e + ')');
            $('#'+result.ControlId).text(result.HasTag ? "Remove" : "Add");
        }
        function copyclip(inElement) {
            if (inElement.createTextRange) {
                var range = inElement.createTextRange();
                if (range)
                    range.execCommand('Copy');
            }
        }
        function FinishMove(loc) {
            tb_remove();
            if (loc)
                window.location = loc;
        }
        function prefchange(dd, i) {
            $get(dd).selectedIndex = i;
        }
        function ValidateDecisionDate(source, args) {
            args.IsValid = true;
            if ($decisiondt && '<%=EditUpdateButton1.Editing?"true":"false"%>' == 'true') {
                if ($get('<%=DecisionTypeId.ClientID%>').value != '0')
                    if (!isValidDate($get('<%=DecisionDate.ClientID%>').value)) {
                        args.IsValid = false;
                        alert('need a valid Decision date');
                    }
            }
        }
        function ValidateStep1Date(source, args) {
            args.IsValid = true;
            if ($decisiondt && '<%=EditUpdateButton1.Editing?"true":"false"%>' == 'true') {
                var s = $get('<%=DiscoveryClassStatusID.ClientID%>').value;
                if (s == '20' || s == '30' || s == '50')
                    if (!isValidDate($get('<%=DiscoveryClassDate.ClientID%>').value)) {
                    args.IsValid = false;
                    alert('need a valid step 1 date');
                }
            }
        }
        function ValidateBaptismDate(source, args) {
            args.IsValid = true;
            if ($decisiondt && '<%=EditUpdateButton1.Editing?"true":"false"%>' == 'true')
                if ($get('<%=BaptismStatusId.ClientID%>').value == '30')
                    if (!isValidDate($get('<%=BaptismDate.ClientID%>').value)) {
                        args.IsValid = false;
                        alert('need a valid baptism date');
                    }
        }
        function isValidDate(dateStr) {
            var reg1 = /^\d{1,2}(\-|\/|\.)\d{1,2}\1\d{2}$/
            var reg2 = /^\d{1,2}(\-|\/|\.)\d{1,2}\1\d{4}$/
            // If it doesn't conform to the right format (with either a 2 digit year or 4 digit year), fail
            if ( (reg1.test(dateStr) == false) && (reg2.test(dateStr) == false) ) { return false; }
            var parts = dateStr.split(RegExp.$1); // Split into 3 parts based on what the divider was
            // Check to see if the 3 parts end up making a valid date
            var mm = parts[0];
            var dd = parts[1];
            var yy = parts[2];
            if (parseFloat(yy) <= 50) 
                yy = (parseFloat(yy) + 2000).toString();
            if (parseFloat(yy) <= 99) 
                yy = (parseFloat(yy) + 1900).toString();
            var dt = new Date(parseFloat(yy), parseFloat(mm)-1, parseFloat(dd), 0, 0, 0, 0);
            if (parseFloat(dd) != dt.getDate()) 
                return false;
            if (parseFloat(mm)-1 != dt.getMonth()) 
                return false;
            return true;
        }
        function VerifyCallback(e) {
            var r = eval('(' + e + ')');
            var ans = confirm(r.address + "\nUse this Address?");
            if (ans) {
                $(r.selector + 'Line1')[0].value = r.Line1;
                $(r.selector + 'Line2')[0].value = r.Line2;
                $(r.selector + 'City')[0].value = r.City;
                $(r.selector + 'State')[0].value = r.State;
                $(r.selector + 'Zip')[0].value = r.Zip;
            }
        }

    </script>

    <table class="PersonHead" border="0">
        <tr>
            <td>
                <cc1:DisplayLabel ID="Name" runat="server" BindingSource="person" />
            </td>
            <td align="right">
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
    </table>
    <uc1:ExportToolBar ID="ExportToolBar1" runat="server" Single="true" />
    <div style="clear: both; margin-top: 8px;">
        <table>
            <tr>
                <td>
                    <table>
                        <tr id="deceased" runat="server" visible="false">
                            <td style="color: Red">
                                <asp:Label ID="Label1" runat="server" Text="Label">Deceased:</asp:Label>
                                <cc1:DisplayOrEditDate ID="DeceasedDate" BindingSource="person" runat="server" BindingMode="OneWay" />
                            </td>
                            <td style="color: Red">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <cc1:DisplayHyperlink ID="PrimaryAddress" runat="server" BindingSource="person" BindingUrlFormat="http://www.google.com/maps?q={0}"
                                    BindingUrlMember="AddrCityStateZip" Target="_blank" />
                                &nbsp;
                            </td>
                            <td>
                                <asp:LinkButton ToolTip="copy name and address to clipboard" ID="CopyToClipboard"
                                    runat="server">clipboard</asp:LinkButton>
                            </td>
                        </tr>
                        <tr runat="server" id="trPrimaryAddress2">
                            <td>
                                <cc1:DisplayHyperlink ID="PrimaryAddress2" runat="server" BindingSource="person"
                                    BindingUrlFormat="http://www.google.com/maps?q={0}" BindingUrlMember="Addr2CityStateZip"
                                    Target="_blank">[Address]</cc1:DisplayHyperlink>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <cc1:DisplayLabel ID="CityStateZip" BindingSource="person" runat="server"></cc1:DisplayLabel>
                            </td>
                            <td>
                                <a href='<%="http://www.google.com/maps?f=d&saddr=2000+Appling+Rd,+Cordova,+Tennessee+38016&pw=2&daddr=" + person.AddrCityStateZip %>'
                                    target="_blank">driving directions</a>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <cc1:DisplayHyperlink ID="DisplayHyperlink1" runat="server" BindingMember="EmailAddress"
                                    BindingUrlFormat="mailto://{0}" BindingUrlMember="EmailAddress" BindingSource="person">[EmailAddress]</cc1:DisplayHyperlink>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <cc1:DisplayOrEditMaskedText ID="HomePhone0" runat="server" BindingSource="person.Family"
                                    BindingMember="HomePhone" BindingMode="OneWay" Height="18px" Width="135px" MaskType="Phone"></cc1:DisplayOrEditMaskedText>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <cc1:DisplayOrEditDropDown ID="PreferredAddressDropDown" BindingMember="AddressTypeId"
                                    BindingSource="person" runat="server" DataTextField="Value" DataValueField="Id"
                                    Style="visibility: hidden;" DataSourceID="ODSPreferredAddressDropDown" />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <cc1:DisplayOrEditCheckbox ID="DoNotCallFlag" runat="server" BindingSource="person"
                                    Text="Do Not Call" TextIfChecked="Do Not Call" BindingMode="OneWay" />
                            </td>
                            <td>
                                <asp:HyperLink ID="ContributionsLink" runat="server">Contributions</asp:HyperLink>
                                <asp:HyperLink ID="VBSFormLink" runat="server" Visible="false">VBS Form</asp:HyperLink>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <cc1:EditUpdateButton ID="EditUpdateButton1" runat="server" OnClick="EditUpdateButton1_Click" />
                                <asp:ImageButton ID="DeletePerson" runat="server" ImageUrl="~/images/delete.gif"
                                    OnClick="DeletePerson_Click" OnClientClick="return confirm('Are you sure you want to delete?')" />
                                <asp:HyperLink ID="movestuff" runat="server" CssClass="thickbox2">move</asp:HyperLink>
                                <asp:HyperLink ID="goback" runat="server">goback</asp:HyperLink>
                            </td>
                            <td>
                                &nbsp;
                                <asp:CustomValidator ID="ValidateDelete" runat="server" Display="Dynamic" ErrorMessage="Too many relationships remain"></asp:CustomValidator>
                            </td>
                        </tr>
                    </table>
                </td>
                <td valign="top">
                    <asp:HyperLink ID="Picture" ToolTip="Click to see larger version or upload new" runat="server">
                    </asp:HyperLink>
                </td>
                <td class="style1">
                </td>
                <td valign="top" nowrap="nowrap">
                    <asp:HyperLink ID="FamilyLink" Text="Family Members" runat="server" Font-Bold="true"
                        Style="line-height: 2"></asp:HyperLink>
                    <asp:GridView ID="FamilyGrid" runat="server" AutoGenerateColumns="False" DataSourceID="ODSFamily"
                        ShowHeader="False" OnRowDataBound="FamilyGrid_RowDataBound" CellPadding="3">
                        <Columns>
                            <asp:TemplateField HeaderText="Name">
                                <ItemTemplate>
                                    <asp:HyperLink ID="namelink" runat="server" NavigateUrl='<%# Eval("Id", "~/Person.aspx?Id={0}") %>'
                                        Text='<%# Eval("Name") %>'></asp:HyperLink>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Age" HeaderText="Age" SortExpression="Age" />
                        </Columns>
                    </asp:GridView>
                    <asp:Label runat="server" ID="SpouseIndicatorNote" Text="* Indicates Spouse"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    <asp:TextBox ID="HiddenAddress" runat="server" Style="display: none" TextMode="MultiLine"
        Rows="5"></asp:TextBox>
    <div id="main-tab">
        <ul>
            <li><a href="#basic-tab"><span>Basic</span></a></li>
            <li><a href="#address-tab"><span>Addresses</span></a></li>
            <li><a id="enrollment-link" href="#enrollment-tab"><span>Enrollment</span></a></li>
            <li><a id="member-link" href="#member-tab"><span>Member Profile</span></a></li>
            <li><a href="#growth-tab"><span>Growth</span></a></li>
            <li><a href="#volunteer-tab"><span>Volunteer</span></a></li>
        </ul>
        <div id="basic-tab" class="ui-tabs-hide">
            <table>
                <tr>
                    <td valign="top">
                        <table class="Design2">
                            <tr>
                                <th>
                                    Goes By:
                                </th>
                                <td>
                                    <cc1:DisplayOrEditText ID="Nickname" runat="server" BindingMember="Nickname" BindingMode="TwoWay"
                                        BindingSource="person" Width="135px" ChangedStatus="False"></cc1:DisplayOrEditText>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    Title:
                                </th>
                                <td>
                                    <cc1:DisplayOrEditDropDown ID="TitleCode" runat="server" BindingMember="TitleCode"
                                        BindingMode="TwoWay" BindingSource="person" DataTextField="Value" DataValueField="Code"
                                        Width="150px" MakeDefault0="False" DataSourceID="ODSTitleCode" ChangedStatus="False">
                                    </cc1:DisplayOrEditDropDown>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    First:
                                </th>
                                <td>
                                    <cc1:DisplayOrEditText ID="FirstName" runat="server" BindingMember="FirstName" BindingMode="TwoWay"
                                        BindingSource="person" Width="135px" ChangedStatus="False"></cc1:DisplayOrEditText>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    Middle:
                                </th>
                                <td>
                                    <cc1:DisplayOrEditText ID="MiddleName" runat="server" BindingMember="MiddleName"
                                        BindingMode="TwoWay" BindingSource="person" Width="135px" ChangedStatus="False"></cc1:DisplayOrEditText>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    Last:
                                </th>
                                <td>
                                    <cc1:DisplayOrEditText ID="LastName" runat="server" BindingMember="LastName" BindingMode="TwoWay"
                                        BindingSource="person" Width="135px" ChangedStatus="False"></cc1:DisplayOrEditText>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    Suffix:
                                </th>
                                <td>
                                    <cc1:DisplayOrEditText ID="SuffixCode" runat="server" BindingMember="SuffixCode"
                                        BindingMode="TwoWay" BindingSource="person" Width="135px" ChangedStatus="False"></cc1:DisplayOrEditText>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    Former:
                                </th>
                                <td>
                                    <cc1:DisplayOrEditText ID="MaidenName" runat="server" BindingMember="MaidenName"
                                        BindingMode="TwoWay" BindingSource="person" Width="135px" ChangedStatus="False"></cc1:DisplayOrEditText>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    Gender:
                                </th>
                                <td>
                                    <cc1:DisplayOrEditDropDown ID="GenderId" runat="server" BindingMember="GenderId"
                                        BindingMode="TwoWay" BindingSource="person" DataTextField="Value" DataValueField="Id"
                                        MakeDefault0="False" Width="150px" DataSourceID="ODSGenderId" ChangedStatus="False">
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
                                    Home Phone:
                                </th>
                                <td>
                                    <cc1:DisplayOrEditMaskedText ID="HomePhone" runat="server" BindingMember="HomePhone"
                                        BindingMode="TwoWay" BindingSource="person.Family" Width="135px" ChangedStatus="False"></cc1:DisplayOrEditMaskedText>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    Cell Phone:
                                </th>
                                <td>
                                    <cc1:DisplayOrEditMaskedText ID="CellPhone" runat="server" BindingMember="CellPhone"
                                        BindingMode="TwoWay" BindingSource="person" Width="135px" ChangedStatus="False"></cc1:DisplayOrEditMaskedText>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    Work Phone:
                                </th>
                                <td>
                                    <cc1:DisplayOrEditMaskedText ID="WorkPhone" runat="server" BindingMember="WorkPhone"
                                        BindingMode="TwoWay" BindingSource="person" Width="135px" ChangedStatus="False"></cc1:DisplayOrEditMaskedText>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    Email:
                                </th>
                                <td>
                                    <cc1:DisplayOrEditText ID="Email" runat="server" BindingMember="EmailAddress" BindingMode="TwoWay"
                                        BindingSource="person" Width="135px" ChangedStatus="False"></cc1:DisplayOrEditText>
                                </td>
                            </tr>
                            <tr id="trSchool" runat="server">
                                <th runat="server">
                                    School:
                                </th>
                                <td runat="server">
                                    <cc1:DisplayOrEditText ID="SchoolOther" runat="server" BindingMode="TwoWay" BindingSource="person"
                                        Width="135px" ChangedStatus="False"></cc1:DisplayOrEditText>
                                    <cc2:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" TargetControlID="SchoolOther"
                                        ServiceMethod="GetSchoolCompletionList" BehaviorID="autoComplete4" CompletionSetCount="10"
                                        MinimumPrefixLength="2" CompletionInterval="100" CompletionListCssClass="autocomplete_list"
                                        CompletionListItemCssClass="autocomplete_listItem" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem" />
                                </td>
                            </tr>
                            <tr id="tr1" runat="server">
                                <th id="Th1" runat="server">
                                    Grade:
                                </th>
                                <td id="Td1" runat="server">
                                    <cc1:DisplayOrEditText ID="Grade" runat="server" BindingMode="OneWay" BindingSource="person"
                                        Width="135px" ChangedStatus="False"></cc1:DisplayOrEditText>
                                </td>
                            </tr>
                            <tr id="trEmployer" runat="server">
                                <th runat="server">
                                    Employer:
                                </th>
                                <td runat="server">
                                    <cc1:DisplayOrEditText ID="EmployerOther" runat="server" BindingMode="TwoWay" BindingSource="person"
                                        Width="135px" BindingMember="EmployerOther" ChangedStatus="False"></cc1:DisplayOrEditText>
                                    <cc2:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" TargetControlID="EmployerOther"
                                        ServiceMethod="GetEmployerCompletionList" BehaviorID="autoComplete3" CompletionSetCount="10"
                                        MinimumPrefixLength="2" CompletionInterval="100" CompletionListCssClass="autocomplete_list"
                                        CompletionListItemCssClass="autocomplete_listItem" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem" />
                                </td>
                            </tr>
                            <tr id="trOccupation" runat="server">
                                <th runat="server">
                                    Occupation:
                                </th>
                                <td runat="server">
                                    <cc1:DisplayOrEditText ID="OccupationOther" runat="server" BindingMode="TwoWay" BindingSource="person"
                                        Width="135px" BindingMember="OccupationOther" ChangedStatus="False"></cc1:DisplayOrEditText>
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
                                    Member Status:
                                </th>
                                <td>
                                    <cc1:DisplayOrEditDropDown ID="MemberStatusId0" runat="server" BindingMember="MemberStatusId"
                                        BindingMode="OneWay" BindingSource="person" DataTextField="Value" DataValueField="Id"
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
                                    <cc1:DisplayOrEditDate ID="JoinDate0" runat="server" BindingMember="JoinDate" BindingMode="OneWay"
                                        BindingSource="person" ChangedStatus="False" EditRole="Membership"></cc1:DisplayOrEditDate>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    Marital Status:
                                </th>
                                <td>
                                    <cc1:DisplayOrEditDropDown ID="MaritalStatusId" runat="server" BindingMember="MaritalStatusId"
                                        BindingMode="TwoWay" BindingSource="person" DataTextField="Value" DataValueField="Id"
                                        MakeDefault0="False" Width="150px" DataSourceID="ODSMaritalStatusId" ChangedStatus="False">
                                    </cc1:DisplayOrEditDropDown>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    Spouse:
                                </th>
                                <td>
                                    <cc1:DisplayOrEditText ID="DisplayOrEditText" runat="server" BindingMember="SpouseName"
                                        BindingMode="OneWay" BindingSource="person" Width="135px" ChangedStatus="False"></cc1:DisplayOrEditText>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    Wedding Date:
                                </th>
                                <td>
                                    <cc1:DisplayOrEditDate BindingMode="TwoWay" runat="server" ID="WeddingDate" BindingSource="person"
                                        Width="135px"></cc1:DisplayOrEditDate>
                                    <cc2:CalendarExtender ID="WeddingDate_CalendarExtender" runat="server" Enabled="True"
                                        TargetControlID="WeddingDate">
                                    </cc2:CalendarExtender>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    Birthday:
                                </th>
                                <td>
                                    <cc1:DisplayOrEditText BindingMode="TwoWay" runat="server" ID="DOB" BindingSource="person"
                                        ToolTip="YYYY or MM or MM/DD or MM/DD/YY" BindingMember="DOB" ChangedStatus="False"
                                        Width="135px"></cc1:DisplayOrEditText>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    Deceased:
                                </th>
                                <td>
                                    <cc1:DisplayOrEditDate ID="DeceasedDate3" BindingSource="person" BindingMember="DeceasedDate"
                                        Width="135px" runat="server"></cc1:DisplayOrEditDate>
                                    <cc2:CalendarExtender ID="DeceasedDate3_CalendarExtender" runat="server" Enabled="True"
                                        TargetControlID="DeceasedDate3">
                                    </cc2:CalendarExtender>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    Age:
                                </th>
                                <td>
                                    <cc1:DisplayLabel ID="Age" runat="server" BindingSource="person" BindingMember="Age"
                                        BindingMode="OneWay" ChangedStatus="False" />
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
                                </th>
                                <td>
                                    <cc1:DisplayOrEditCheckbox ID="DoNotCallFlag0" runat="server" BindingMember="DoNotCallFlag"
                                        BindingSource="person" Text="Do Not Call" TextIfChecked="Do Not Call" BindingMode="TwoWay"
                                        ChangedStatus="False" TextIfNotChecked="" />
                                </td>
                            </tr>
                            <tr>
                                <th>
                                </th>
                                <td>
                                    <cc1:DisplayOrEditCheckbox ID="DoNoVisitFlag0" runat="server" BindingMember="DoNotVisitFlag"
                                        BindingSource="person" Text="Do Not Visit" TextIfChecked="Do Not Visit" BindingMode="TwoWay"
                                        ChangedStatus="False" TextIfNotChecked="" />
                                </td>
                            </tr>
                            <tr>
                                <th>
                                </th>
                                <td>
                                    <cc1:DisplayOrEditCheckbox ID="DoNotMailFlag0" runat="server" BindingMember="DoNotMailFlag"
                                        BindingSource="person" Text="Do Not Mail" TextIfChecked="Do Not Mail" BindingMode="TwoWay"
                                        ChangedStatus="False" TextIfNotChecked="" />
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
        <div id="address-tab" class="ui-tabs-hide ui-tabs">
            <ul>
                <li><a href="#personal1-tab"><span>Personal</span></a></li>
                <li><a href="#personal2-tab"><span>Personal Alternate</span></a></li>
                <li><a href="#family1-tab"><span>Family</span></a></li>
                <li><a href="#family2-tab"><span>Family Alternate</span></a></li>
            </ul>
            <div id="personal1-tab" class="ui-tabs-hide">
                <uc1:address ID="PersonPrimaryAddr" runat="server" AddressType="Personal" />
            </div>
            <div id="personal2-tab" class="ui-tabs-hide">
                <uc1:address ID="PersonAltAddr" runat="server" AddressType="PersonalAlternate" />
            </div>
            <div id="family1-tab" class="ui-tabs-hide">
                <uc1:address ID="FamilyPrimaryAddr" runat="server" AddressType="Family" />
            </div>
            <div id="family2-tab" class="ui-tabs-hide">
                <uc1:address ID="FamilyAltAddr" runat="server" AddressType="FamilyAlternate" />
            </div>
        </div>
        <div id="enrollment-tab" class="ui-tabs-hide ui-tabs">
            <ul>
                <li><a href="#current-tab"><span>Current</span></a></li>
                <li><a id="previous-link" href="#previous-tab"><span>Previous</span></a></li>
                <li><a id="pending-link" href="#pending-tab"><span>Pending</span></a></li>
                <li><a id="attendance-link" href="#attendance-tab"><span>Attendance History</span></a></li>
            </ul>
            <div id="current-tab" class="ui-tabs-hide">
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
            <div id="previous-tab" class="ui-tabs-hide">
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
            <div id="pending-tab" class="ui-tabs-hide">
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
            <div id="attendance-tab" class="ui-tabs-hide">
                <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
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
        </div>
        <div id="member-tab" class="ui-tabs-hide ui-tabs">
            <ul>
                <li><a href="#membersum-tab"><span>Summary</span></a></li>
                <li><a href="#membernotes-tab"><span>Notes</span></a></li>
            </ul>
            <div id="membersum-tab" class="ui-tabs-hide">
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
                                    <th runat="server">
                                        Date:
                                    </th>
                                    <td runat="server">
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
                                    <th runat="server">
                                        Scheduled:
                                    </th>
                                    <td runat="server">
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
            <div id="membernotes-tab" class="ui-tabs-hide">
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
        </div>
        <div id="growth-tab" class="ui-tabs-hide">
            <table>
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
                                        BindingMember="MemberAnyChurch" BindingMode="TwoWay" TextIfChecked="Yes" TextIfNotChecked="No" />
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
                                        BindingMember="ChristAsSavior" BindingMode="TwoWay" TextIfChecked="Yes" TextIfNotChecked="No" />
                                </td>
                                <th>
                                    Prayed for Christ as Savior
                                </th>
                            </tr>
                            <tr>
                                <td>
                                    <cc1:DisplayOrEditCheckbox ID="PleaseVisitFlag0" runat="server" BindingSource="person"
                                        BindingMember="PleaseVisit" BindingMode="TwoWay" TextIfChecked="Yes" TextIfNotChecked="No" />
                                </td>
                                <th>
                                    Would like someone to visit
                                </th>
                            </tr>
                            <tr>
                                <td>
                                    <cc1:DisplayOrEditCheckbox ID="InterestedInJoiningFlag0" runat="server" BindingSource="person"
                                        BindingMember="InterestedInJoining" BindingMode="TwoWay" TextIfChecked="Yes"
                                        TextIfNotChecked="No" />
                                </td>
                                <th>
                                    Interested in joining Bellevue
                                </th>
                            </tr>
                            <tr>
                                <td>
                                    <cc1:DisplayOrEditCheckbox ID="InfoBecomeAChristianFlag0" runat="server" BindingSource="person"
                                        BindingMember="InfoBecomeAChristian" BindingMode="TwoWay" TextIfChecked="Yes"
                                        TextIfNotChecked="No" />
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
        </div>
        <div id="volunteer-tab" class="ui-tabs-hide">
            <table class="Design2" style="border-style: groove; border-width: thin;">
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
        </div>
    </div>
    <asp:ObjectDataSource ID="ODSFamily" runat="server" SelectMethod="FamilyMembers"
        TypeName="CMSPresenter.PersonController">
        <SelectParameters>
            <asp:SessionParameter Name="PersonId" SessionField="ActivePersonId" Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="EnrollData" runat="server" SelectMethod="EnrollData" TypeName="CMSPresenter.PersonController"
        EnablePaging="true" SortParameterName="sortExpression" SelectCountMethod="EnrollDataCount">
        <SelectParameters>
            <asp:SessionParameter Name="pid" SessionField="ActivePersonId" Type="Int32" />
            <asp:Parameter Name="sortExpression" Type="String" DefaultValue="Organization" />
            <asp:Parameter Name="maximumRows" Type="Int32" />
            <asp:Parameter Name="startRowIndex" Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="PreviousEnrollData" runat="server" SelectMethod="PreviousEnrollData"
        TypeName="CMSPresenter.PersonController" EnablePaging="True" SelectCountMethod="PreviousEnrollDataCount"
        SortParameterName="sortExpression">
        <SelectParameters>
            <asp:SessionParameter Name="pid" SessionField="ActivePersonId" Type="Int32" />
            <asp:Parameter Name="sortExpression" Type="String" DefaultValue="Organization" />
            <asp:Parameter Name="maximumRows" Type="Int32" />
            <asp:Parameter Name="startRowIndex" Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="PendingEnrollData" runat="server" SelectMethod="PendingEnrollData"
        TypeName="CMSPresenter.PersonController">
        <SelectParameters>
            <asp:SessionParameter Name="pid" SessionField="ActivePersonId" Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="AttendData" runat="server" EnablePaging="True" SelectCountMethod="HistoryCount"
        SelectMethod="AttendHistory" SortParameterName="sortExpression" TypeName="CMSPresenter.AttendController">
        <SelectParameters>
            <asp:SessionParameter Name="pid" SessionField="ActivePersonId" Type="Int32" />
            <asp:Parameter Name="sortExpression" Type="String" DefaultValue="MeetingDate DESC" />
            <asp:Parameter Name="maximumRows" Type="Int32" />
            <asp:Parameter Name="startRowIndex" Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="ContactedList" runat="server" EnablePaging="True" SelectCountMethod="ContactsCount"
        SelectMethod="ContactedList" SortParameterName="sortExpression" TypeName="CMSPresenter.ContactController">
        <SelectParameters>
            <asp:SessionParameter Name="pid" SessionField="ActivePersonId" Type="Int32" />
            <asp:Parameter Name="sortExpression" Type="String" DefaultValue="Date DESC" />
            <asp:Parameter Name="maximumRows" Type="Int32" />
            <asp:Parameter Name="startRowIndex" Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="ContactsMadeList" runat="server" EnablePaging="True" SelectCountMethod="ContactsMadeCount"
        SelectMethod="ContactsMadeList" SortParameterName="sortExpression" TypeName="CMSPresenter.ContactController">
        <SelectParameters>
            <asp:SessionParameter Name="pid" SessionField="ActivePersonId" Type="Int32" />
            <asp:Parameter Name="sortExpression" Type="String" DefaultValue="Date DESC" />
            <asp:Parameter Name="maximumRows" Type="Int32" />
            <asp:Parameter Name="startRowIndex" Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="ODSSchoolId" runat="server" SelectMethod="Schools" TypeName="CMSPresenter.CodeValueController">
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="ODSMemberStatusId" runat="server" SelectMethod="MemberStatusCodes"
        TypeName="CMSPresenter.CodeValueController"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="ODSPreferredAddressDropDown" runat="server" SelectMethod="AddressTypeCodes"
        TypeName="CMSPresenter.CodeValueController"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="ODSMaritalStatusId" runat="server" SelectMethod="MaritalStatusCodes"
        TypeName="CMSPresenter.CodeValueController"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="ODSGenderId" runat="server" SelectMethod="GenderCodes"
        TypeName="CMSPresenter.CodeValueController"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="ODSTitleCode" runat="server" SelectMethod="TitleCodes"
        TypeName="CMSPresenter.CodeValueController"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="ODSDecisionTypeId" runat="server" SelectMethod="DecisionCodes"
        TypeName="CMSPresenter.CodeValueController"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="ODSBaptismTypeId" runat="server" SelectMethod="BaptismTypes"
        TypeName="CMSPresenter.CodeValueController"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="ODSBaptismStatusId" runat="server" SelectMethod="BaptismStatuses"
        TypeName="CMSPresenter.CodeValueController"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="ODSInterestPointId" runat="server" SelectMethod="InterestPoints"
        TypeName="CMSPresenter.CodeValueController"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="ODSOriginId" runat="server" SelectMethod="Origins" TypeName="CMSPresenter.CodeValueController">
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="ODSEntryPointId" runat="server" SelectMethod="EntryPoints"
        TypeName="CMSPresenter.CodeValueController"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="ODSJoinCodeId" runat="server" SelectMethod="JoinTypes"
        TypeName="CMSPresenter.CodeValueController"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="ODSDropCodeId" runat="server" SelectMethod="DropTypes"
        TypeName="CMSPresenter.CodeValueController"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="ODSEnvelopeOptionsId" runat="server" SelectMethod="EnvelopeOptions"
        TypeName="CMSPresenter.CodeValueController"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="ODSDiscoveryClassStatusID" runat="server" SelectMethod="DiscoveryClassStatusCodes"
        TypeName="CMSPresenter.CodeValueController"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="ODSLetterStatusId" runat="server" SelectMethod="LetterStatusCodes"
        TypeName="CMSPresenter.CodeValueController"></asp:ObjectDataSource>
</asp:Content>
