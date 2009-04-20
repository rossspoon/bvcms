<%@ Page Language="C#" AutoEventWireup="True" StylesheetTheme="VerseMemory" Inherits="Verse_Practice" Codebehind="Practice.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="header" runat="server">
    <title>Verse Practice</title>
<style type="text/css" media="screen">
  .noPrint{ display: block; }
</style> 
<style type="text/css" media="print">
  .noPrint{ display: none; }
</style>
</head>
<body onload="return window_onload()" onkeydown="return window_onkeydown()">
    <bgsound id="beep" src="#" />
    <form id="form1" runat="server">
        <asp:HiddenField ID="HiddenField1" runat="server" />
        <ajx:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </ajx:ToolkitScriptManager>
        <asp:LinkButton ID="LinkButton2" runat="server" CssClass="noPrint">Show Help</asp:LinkButton>
        <a class="noPrint" href="javascript:window.print()">Print</a><br />
        <center>
            <table border="0" cellpadding="4" cellspacing="5" width = "85%">
                <tr>
                    <td align="left">
                        <h3>
                            <asp:Label ID="Ref" runat="server" Text="Label"></asp:Label></h3>
                    </td>
                </tr>
                <tr valign="top" align="left">
                    <td>
                        <div id="verse" class="Verse">
                        </div>
                    </td>
                </tr>
            </table>
        </center>
        <asp:Panel runat="server" CssClass="modalPopup" ID="Panel1" Style="display: none;
            padding: 10px">
            Just type the first letter of each word in the verse. If you make a mistake it will
            beep. If you get it right it will show the word in the box. If you give up on the
            next word, just press space and it will show up. You will be notified when you are
            finished. Start over by pressing the Enter key. When you're finished, press the
            Esc key or just close the window.
            <p align="center">
                <asp:LinkButton ID="LinkButton1" runat="server">Dismiss Help</asp:LinkButton></p>
        </asp:Panel>
    <ajx:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="LinkButton2"
            PopupControlID="Panel1" PopupDragHandleControlID="programmaticPopupDragHandle"
            BackgroundCssClass="modalBackground" CancelControlID="LinkButton1">
        </ajx:ModalPopupExtender>
  </form>
</body>
</html>
