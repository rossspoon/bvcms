<%@ Page Language="C#" MasterPageFile="~/site.master" AutoEventWireup="True"
    Inherits="Add" Title="Add Verse to Category" Codebehind="Add.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
    <div id="main" class="wide">
        <h4>
            Add Verse to Category:
            <asp:Label ID="Category" runat="server" Text="Label"></asp:Label></h4>
        <table cellpadding="4">
            <tr>
                <td>
                    Version:</td>
                <td style="width: 281px">
                    <asp:DropDownList ID="Version" runat="server" AutoPostBack="True" OnSelectedIndexChanged="Version_SelectedIndexChanged">
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td valign="top">
                    Verse:</td>
                <td style="width: 281px">
                    <asp:UpdatePanel ID="UpdatePanelRef" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="RefText" runat="server"></asp:TextBox>
                            <asp:LinkButton ID="Preview" runat="server" OnClick="Preview_Click">Preview</asp:LinkButton>
                            <asp:CustomValidator ID="RefBadValidator" runat="server" ErrorMessage="Missing or invalid reference"
                                Display="Dynamic"></asp:CustomValidator>
                            <p>
                                <asp:Label ID="VerseLit" runat="server"></asp:Label></p>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="AddVerse" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="Version" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td style="width: 281px">
                    <p>
                        &nbsp;<asp:Button ID="AddVerse" runat="server" Text="Add Verse to Category" Width="210px"
                            OnClick="AddVerse_Click" /></p>
                    <p>
                        &nbsp;<asp:Button ID="Cancel" runat="server" OnClick="Cancel_Click" Text="Cancel and Return to Verses"
                            Width="210px" /></p>
                </td>
            </tr>
        </table>
    </div>
    Verses are retrieved from <a href="http://www.BibleGateway.com">www.BibleGateway.com</a>
</asp:Content>
