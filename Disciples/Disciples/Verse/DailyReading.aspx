<%@ Page StylesheetTheme="Default" Language="C#" MasterPageFile="~/site.master" AutoEventWireup="True" Inherits="Verse_DailyReading"
    Title="Daily Bible Reading" CodeBehind="DailyReading.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
    <div id="main" class="wide">
        <h1>
            One-Year-Bible reading plan.</h1>
        <p>
            This plan provides a good balance of Old and New Testament readings and includes
            readings from Psalms and Proverbs all through the year. You actually end up reading
            through the Psalms twice on this plan.</p>
        <p>
            Click on the date to read the verses indicated on the BibleGateway.com site. You
            can change the version there. Click on "current" to make that reading be for today's
            date (good for catching up or for starting in the middle of the year)</p>
        <p>
            There is also a link listen to Brian Hardin at <a href="http://DailyAudioBible.com">
                DailyAudioBible.com</a></p>
        <p>
            <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="#doy">Jump to today&#39;s Reading</asp:HyperLink></p>
        <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
    </div>
</asp:Content>
