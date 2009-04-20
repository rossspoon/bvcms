<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="indexHead" ContentPlaceHolderID="head" runat="server">
    <title>Home Page</title>
</asp:Content>
<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Welcome to Bellevue Prays</h2>
    <% if (!User.Identity.IsAuthenticated)
       {
    %>
    <p>
        If you already have an account on disciples.bellevue.org and you remember your username and password, 
        you can login by clicking on the link in the top right corner of this page 
        (note: you do need to be a Bellevue member).
    </p>
    <p>
        If you don't have an account on disciples.bellevue.org
        or you can't remember your login information, 
        you can still register and sign up for our <b>Prayer Offering</b> 
        emphasis on this site. First you&#39;ll need to identify yourself. Click on the
        Registration tab above to do that. Then you will be taken to the Prayer Times page where
        you can choose your half hour.</p>
    <% }
       else
       { %>
    <p>
        Bellevue Members can sign up for our <b>Prayer Offering</b> emphasis on this site. Or
        you can choose another available half hour if you have already signed up.</p>
     <% } %>
    <h3>
        Help...</h3>
    <p>
        If you are having a difficult time or have questions or special requests,
        please call Bellevue's Prayer Ministry at 901-347-5750 or contact them via email at
        <a href="mailto:prayer@bellevue.org">prayer@bellevue.org</a> </p>
    <h3>
        Coming soon...</h3>
    <p>
        This site will eventually allow you to submit prayer requests, keep your own prayer
        journal, have reminders to pray, and be notified of urgent prayer requests.
    </p>
    <p>
        You will be able to have a customized prayer session where individual prayer
        requests will appear on your screen reminding you of the important points about
        the need. After thirty seconds (or an interval of your own choosing), a different
        request will appear or you can pause and linger on a special request. In this way,
        the site will guide you through a series of important needs of our fellowship and
        community.
    </p>
</asp:Content>
