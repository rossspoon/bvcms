<%@ Page Language="C#" AutoEventWireup="True" Inherits="Practice" Codebehind="Practice2.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Verse Practice</title>
</head>
<body onload="return window_onload()" onkeydown="return window_onkeydown()">
    <bgsound id="beep" src="#" />
    <form id="form1" runat="server">
        <asp:HiddenField ID="HiddenField1" runat="server" />
        <div>
            <h3>
                <asp:Label ID="Ref" runat="server" Text="Label"></asp:Label></h3>
            <div id="verse" class="Verse">
            </div>
            <div>
                Just type the first letter of each word in the verse. If you make a mistake it will
                beep. If you get it right it will show the word in the box. If you give up on the
                next word, just press space and it will show up. You will be notified when you are
                finished. Start over by pressing the Enter key. To leave this screen, press the
                Esc key or close the window.</div>
        </div>
    </form>
</body>
</html>
