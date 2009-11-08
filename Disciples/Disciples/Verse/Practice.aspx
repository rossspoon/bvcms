<%@ Page Language="C#" AutoEventWireup="True" EnableTheming="false"  Inherits="Verse_Practice" Codebehind="Practice.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Verse Practice</title>
<style type="text/css" media="screen">
  .noPrint{ display: block; }
</style> 
<style type="text/css" media="print">
  .noPrint{ display: none; }
</style>
    <asp:PlaceHolder ID="head" runat="server"></asp:PlaceHolder>
</head>
<body>
    <form id="form1" runat="server">
        <asp:HiddenField ID="HiddenField1" runat="server" />
        <a id="helpshow" href="#" class="noPrint">Show Help</a>
        <a class="noPrint" href="javascript:window.print()">Print</a><br />
        <center>
            <table border="0" cellpadding="4" cellspacing="5" width = "85%">
                <tr>
                    <td align="left">
                        <div id="refh">
                        <h3>
                            <asp:Label ID="Ref" runat="server" Text="Label"></asp:Label></h3></div>
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
        <div style="display:none"> 
			<div id='helppanel' class="modalPopup"> 
            Just type the first letter of each word in the verse. If you make a mistake it will
            flash red. If you get it right it will show the word in the box. If you give up on the
            next word, just press space and it will show up. When you finish, the verse will highlight yellow. 
            <b>Start over by pressing the Enter key.</b> When you're finished, press the
            Esc key or just close the window.
            <p align="center">
                <a id="helphide" href="#">Dismiss Help</a></p>
			</div> 
		</div>
    </form>
</body>
</html>
