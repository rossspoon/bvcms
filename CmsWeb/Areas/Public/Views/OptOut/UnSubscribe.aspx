<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>ConfirmOptout</title>
</head>
<body>
    <div>
        <h1>Please Confirm Your Opt Out</h1>
        <hr />
        <div>Your email address: <strong><%=ViewData["toemail"] %></strong></div>
        <div>(If this is not you, you have not been added to any lists)</div>
        <p>Are you sure you wish to stop ALL emails from <%=ViewData["fromemail"] %> sent to your email address?</p>
        <form method="post" action="/OptOut/UnSubscribe">
        <%=Html.Hidden("id", ViewData["id"]) %>
        <%=Html.Hidden("enc", ViewData["enc"]) %>
        <div style="padding-left: 10px">
            <div style="margin: 12px 0px 15px 0px">
                <input name="optout" type="submit" value="Yes, unsubscribe me" />
                <input name="cancel" type="submit" value="Cancel" />
            </div>
        </div>
        </form>
    </div>
</body>
</html>
