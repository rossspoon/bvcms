<%@ Page Language="C#" StylesheetTheme="Standard" AutoEventWireup="true" CodeBehind="ContributionStatementExtract.aspx.cs" MasterPageFile="~/Site.Master"  Inherits="CMSWeb.Contributions.Reports.ContributionStatementExtract" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<script type="text/javascript">
    function CreateStatementExtract() {
        Page_ClientValidate("vgDates");
        if (Page_IsValid) {
            var fd = $get('<%=FromDate.ClientID %>').value;
            var td = $get('<%=ToDate.ClientID %>').value;
            var args = "?fd=" + fd + "&td=" + td;
            var newWindowUrl = "ContributionStatementExtract.ashx" + args
            window.open(newWindowUrl);
        }
        return Page_IsValid;
    }

    function showDiv() {
        var uploadButtonId = '';
        WebForm_DoPostBackWithOptions(new WebForm_PostBackOptions(uploadButtonId, "", true, "", "", false, false));

        var validated = ValidatorOnSubmit();

        if (validated == true) {
            var hiddenDiv = '<%=myHiddenDiv.ClientID%>';
            var animatedImg = '<%=myAnimatedImage.ClientID%>';    
            document.getElementById(hiddenDiv).style.display = "";
            setTimeout('document.images["' + animatedImg + '"].src = "/images/work.gif"', 200);
        }
    } 
    </script>


    <table class="modalPopup">
        <tr>
            <th colspan="2">
                Contribution Statements Extract Export 
            </th>
            <td rowspan="4">
                <span runat="server" id='myHiddenDiv' style='display:none'> 
                    <img runat="server" src="~/images/work.gif" id='myAnimatedImage' alt="In process of "/>
                    <b style="color:#8CC63E;font-size:x-large;"> 
                        Creating....
                    </b>
        </span> </td>
        </tr>
        <tr>
            <th>Start Date:</th>
            <td><asp:TextBox ID="FromDate" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <th>End Date:</th>
            <td><asp:TextBox ID="ToDate" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <td colspan="2" align="center"><asp:Button ID="btnCSExtract" Width="114px"
                    ToolTip="Press to create Contribution Statement Extract" 
                    CausesValidation="true" runat="server" Text="Extract" ValidationGroup="vgDates" 
                    onclick="ProcessRequest" /></td>
        </tr>
    </table>
    <cc2:CalendarExtender ID="FromExtender" runat="server" TargetControlID="FromDate"></cc2:CalendarExtender>
    <cc2:CalendarExtender ID="ToDateExtender" runat="server" TargetControlID="ToDate"></cc2:CalendarExtender>
    <asp:CompareValidator ID="FromDateValidator" runat="server" ErrorMessage="CompareValidator" ControlToValidate="FromDate" ControlToCompare="ToDate" Operator="LessThanEqual" SetFocusOnError="True" ValidationGroup="vgDates" Type="Date" Text="From Date must be before To Date."></asp:CompareValidator><br />
    <asp:CompareValidator ID="ToDateValidator" runat="server" ErrorMessage="CompareValidator" ControlToValidate="ToDate" ControlToCompare="FromDate" Operator="GreaterThanEqual" SetFocusOnError="True" ValidationGroup="vgDates" Type="Date" Text="To Date must be after From Date."></asp:CompareValidator>
</asp:Content>
