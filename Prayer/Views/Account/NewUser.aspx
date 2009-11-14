<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="loginHead" ContentPlaceHolderID="head" runat="server">
    <title>Logged On</title>
</asp:Content>
<asp:Content ID="loginContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Logged On</h2>
    <p>
        Hi <%= DbUtil.Db.CurrentUser.FirstName %>, glad you are here!<br />
        The information on this page will allow you to log back in to this site the next
        time you visit.</p>
    <p>
        When you have finished reviewing this information, you can go to <a href="/Signup/Index/<%=ViewData["userid"]%>">select</a> your prayer
        times on the Prayer Times tab above</p>
    <div>
        <fieldset>
            <legend>Temporary username and password:</legend>
            <table>
                <tr>
                    <th>Username:</th><td><%= ViewData["username"] %></td>
                </tr>
                <tr>
                    <th>Password:</th><td><%= ViewData["password"] %></td>
                </tr>
            </table>
        </fieldset>
    </div>
</asp:Content>
