<%@ Page ValidateRequest="false" Language="C#" AutoEventWireup="True" Inherits="CMSParagraph"
    CodeBehind="CMSParagraph.aspx.cs" %>
<%@ Register Assembly="FredCK.FCKeditorV2" Namespace="FredCK.FCKeditorV2" TagPrefix="fck" %>

<%@ OutputCache NoStore="true" Location="None" %>
<html>
<head runat="server">
</head>
<body>
    <form id="Form1" runat="server">
    <div style="text-align: center; margin: 10px;">
        <div>
            Title:
            <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
        </div>
        <div>
            <fck:FCKeditor id="txtContent" BasePath="~/FCKeditor/" runat="server"  Height="500" Width="700"/>
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
