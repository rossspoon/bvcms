<%@ Page Language="C#" MasterPageFile="~/site.master" AutoEventWireup="True" Inherits="Verse_DailyReadingChron"
    Title="Daily Bible Reading" CodeBehind="DailyReadingChron.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
    <div id="main" class="wide">
        <h1>
            Chronological-Bible reading plan.</h1>
        <p>
            Read the Bible as its events occurred in real time. 
            For example, Job lived sometime after the beginning of creation (Genesis 1) 
            but before Abraham was born (Genesis 12). 
            As a result, the Book of Job is integrated into the Book of Genesis.</p>
        <p>
            Click on the date to read the verses indicated on the BibleGateway.com site. You
            can change the version there. Click on "current" to make that reading be for today's
            date (good for catching up or for starting in the middle of the year)</p>

        <p>
            <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="#doy">Jump to today&#39;s Reading</asp:HyperLink></p>
        <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
    </div>
</asp:Content>
