<%@ Page StylesheetTheme="Default" ValidateRequest="false" Language="C#" AutoEventWireup="True" Inherits="CMSParagraph"
    CodeBehind="CMSParagraph.aspx.cs" %>
<%@ OutputCache NoStore="true" Location="None" %>
<html>
<head runat="server">

<asp:Literal ID="Literal1" runat="server">
</asp:Literal>
</head>
<body>
    <form id="Form1" runat="server">
    <div style="text-align: center; margin: 10px;">
        <div>
            Title:
            <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
        </div>
        <div>
            <asp:TextBox ID="txtContent2" runat="server" TextMode="MultiLine" Height="600" Width="700" ></asp:TextBox>
        </div>
        <div>
            <cms:ResultMessage ID="ResultMessage1" runat="server" />
            <asp:Button ID="btnSave" runat="server" CausesValidation="False" Text="Save" OnClick="btnSave_Click">
            </asp:Button>&nbsp;
            <input type="button" value="Close" onclick="parent.hidePopWin(false);parent.location.reload();" />
        </div>
    </div>
    </form>
</body>
</html>
