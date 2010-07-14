<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Address.ascx.cs" Inherits="CmsWeb.Address" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <br />
        <table>
            <tr>
                <td>
                    <table class="Design2">
                        <tr>
                            <th>
                                Address:
                            </th>
                            <td>
                                <cc1:DisplayOrEditText ID="Line1" runat="server"
                                    Width="205px" />
                            </td>
                            <th class="Design2">
                                Bad Address Flag:
                            </th>
                            <td>
                                <cc1:DisplayOrEditCheckbox ID="BadAddressFlag" runat="server" BindingSource="Person"
                                    Text=" " />
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <cc1:DisplayOrEditText ID="Line2" runat="server" Width="205px" />
                            </td>
                            <th class="Design2">
                                Resident Code:
                            </th>
                            <td>
                                <cc1:DisplayOrEditDropDown ID="ResidentCode" runat="server" DataValueField="Id" 
                                    DataTextField="Value" MakeDefault0="True">
                                </cc1:DisplayOrEditDropDown>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                City:
                            </th>
                            <td>
                                <cc1:DisplayOrEditText ID="City" runat="server" Width="205px" />
                            </td>
                        </tr>
                        <tr>
                            <th>
                                State:
                            </th>
                            <td>
                                <cc1:DisplayOrEditDropDown ID="State" runat="server" DataTextField="Value" DataValueField="Code"
                                    DisplayMode="Code" MarkFirstMatch="True" MaxLength="10"
                                    BindingMember="StateCode" BindingMode="TwoWay" MakeDefault0="False">
                                </cc1:DisplayOrEditDropDown>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                Zip:
                            </th>
                            <td>
                                <cc1:DisplayOrEditMaskedText ID="Zip" runat="server" MaskType="Zip" ontextchanged="Zip_TextChanged" 
                                    />
<%--                                <asp:RegularExpressionValidator ID="ZipValidator" ControlToValidate="Zip" runat="server" ValidationExpression="\d{5}(-\d{4})?" ErrorMessage="Enter valid zip"></asp:RegularExpressionValidator>                     
--%>                            </td>
                            <td>
                                <asp:Literal ID="VerifyAddress2" runat="server"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <th style="vertical-align: top">
                                Effective Dates:
                            </th>
                            <td colspan="3">
                                <table class="Design2">
                                    <tr>
                                        <th>
                                            From:
                                        </th>
                                        <td>
                                            <cc1:DisplayOrEditDate ID="FromDate" runat="server"></cc1:DisplayOrEditDate>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>
                                            To:
                                        </th>
                                        <td>
                                            <cc1:DisplayOrEditDate ID="ToDate" runat="server"></cc1:DisplayOrEditDate><asp:CompareValidator
                                                ID="ToDateValidator" runat="server" ErrorMessage="CompareValidator" ControlToValidate="ToDate"
                                                ControlToCompare="FromDate" Operator="GreaterThanEqual" SetFocusOnError="True"
                                                ValidationGroup="vgDates" Type="Date" Text="To Date must be after From Date."></asp:CompareValidator>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr runat="server" id="trPreferredAddress">
                            <th>
                                Preferred Address
                            </th>
                            <td colspan="3">
                                <input type="radio" <%=Disabled%> name="prefaddress" <%=OnChange%> <%=Checked%> />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <cc2:CalendarExtender ID="FromExtender" runat="server" TargetControlID="FromDate">
        </cc2:CalendarExtender>
        <cc2:CalendarExtender ID="ToDateExtender" runat="server" TargetControlID="ToDate">
        </cc2:CalendarExtender>
    </ContentTemplate>
</asp:UpdatePanel>
